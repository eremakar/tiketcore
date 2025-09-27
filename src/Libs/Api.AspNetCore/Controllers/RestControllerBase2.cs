﻿using Api.AspNetCore.Models.Rest;
using Data.Mapping;
using Data.Repository.Dapper;
using Data.Repository.Helpers;
using Data.Repository.Stability;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Data.Repository
{
    public abstract class RestControllerBase2<T, TKey, TDto, TQuery, TMap> : ControllerBase
        where T : class, IEntityKey<TKey>
        where TQuery: IQueryBase<T>
        where TMap: MapBase2<T, TDto, MapOptions>
        where TDto: class
    {
        protected readonly ILogger<RestServiceBase<T, TKey>> logger;
        protected readonly IDapperDbContext restDapperDb;
        protected readonly DbContext restDb;
        protected readonly DbSet<T> restSet;
        protected readonly string tableName;
        protected string primaryKeyName = "Id";
        protected readonly TMap map;

        protected RestControllerBase2(ILogger<RestServiceBase<T, TKey>> logger, IDapperDbContext restDapperDb, DbContext restDb, string tableName,
            TMap map)
        {
            this.logger = logger;
            this.restDapperDb = restDapperDb;
            this.restDb = restDb;
            restSet = restDb.Set<T>();
            this.tableName = tableName;
            this.map = map;
        }

        public virtual async Task<PagedList<TDto>> SearchAsync(TQuery query)
        {
            return await SearchUsingDapperAsync(query);
        }

        protected virtual async Task<PagedList<TDto>> SearchUsingDapperAsync(TQuery query)
        {
            return await RetryHelper.RetryDbAsAsync(async () =>
            {
                var list = await FilterHelper.SearchUsingDapperAsync<T, TQuery>(query, restDapperDb, tableName);
                return new PagedList<TDto>(map.Map(list.Result), list.Total, query.Paging);
            });
        }

        protected virtual async Task<PagedList<TDto>> SearchUsingEfAsync<TFilter, TSort>(QueryBase<T, TFilter, TSort> query, 
            Func<IQueryable<T>, IQueryable<T>> expression, MapOptions mapOptions = null, Func<List<T>, Task> apply = null, Func<List<TDto>, Task> applyDto = null,
            Func<List<TDto>, Task<List<TDto>>> applyDto2 = null, FilterOptions options = null)
            where TFilter : FilterBase<T>
            where TSort : SortBase<T>
        {
            return await RetryHelper.RetryDbAsAsync(async () =>
            {
                var list = await FilterHelper.SearchUsingEfAsync(query, expression, restDb, apply, options);
                var dtoList = map.Map(list.Result, mapOptions);
                if (applyDto2 != null)
                    dtoList = await applyDto2(dtoList);
                else if (applyDto != null)
                    await applyDto(dtoList);                
                return new PagedList<TDto>(dtoList, list.Total, query.Paging);
            });
        }

        protected async Task<PagedList<TDto2>> SearchAsUsingEfAsync<TFilter2, TSort2, T2, TDto2>(QueryBase<T2, TFilter2, TSort2> query,
            Func<IQueryable<T2>, IQueryable<T2>> expression, 
            Func<List<T2>, MapOptions, List<TDto2>> map,
            MapOptions mapOptions = null, Func<List<T2>, Task> apply = null, Func<List<TDto2>, Task> applyDto = null,
            Func<List<TDto2>, Task<List<TDto2>>> applyDto2 = null)
            where TFilter2 : FilterBase<T2>
            where TSort2 : SortBase<T2>
            where T2: class
            where TDto2: class
        {
            return await RetryHelper.RetryDbAsAsync(async () =>
            {
                var list = await FilterHelper.SearchUsingEfAsync(query, expression, restDb, apply);
                var dtoList = map(list.Result, mapOptions);
                if (applyDto2 != null)
                    dtoList = await applyDto2(dtoList);
                else if (applyDto != null)
                    await applyDto(dtoList);
                return new PagedList<TDto2>(dtoList, list.Total, query.Paging);
            });
        }

        public virtual async Task<TDto> FindAsync(TKey key)
        {
            return await FindUsingDapperAsync(key);
        }

        protected virtual async Task<TDto> FindUsingDapperAsync(TKey key)
        {
            return await RetryHelper.RetryDbAsAsync(async () =>
            {
                var item = await restDapperDb.FindWhereColumnValueEqualsAsync<T>(tableName, primaryKeyName, key);
                return map.Map(item);
            });
        }

        protected virtual async Task<TDto> FindUsingEfAsync(TKey key,
            Func<IQueryable<T>, IQueryable<T>> expression, Func<T, T> postFunc = null, MapOptions mapOptions = null, FilterOptions options = null)
        {
            return await RetryHelper.RetryDbAsAsync(async () =>
            {
                var source = restDb.Set<T>().AsQueryable();

                if (options == null || options.NoTracking == true)
                    source = source.AsNoTracking();

                if (expression != null)
                    source = expression(source);

                source = source.AsSplitQuery();
                var item = await source.FirstOrDefaultAsync(_ => _.Id.Equals(key));

                if (item == null)
                    return null;

                if (postFunc != null)
                {
                    item = postFunc(item);
                    if (item == null)
                        return null;
                }

                return map.Map(item, mapOptions);
            });
        }

        protected virtual async Task<T> InvokeDbAsync<T>(Func<Task<T>> taskFunc)
        {
            return await RetryHelper.RetryDbAsAsync(async () =>
            {
                return await taskFunc();
            });
        }

        protected virtual async Task AddMapAsyncCore(T source, TDto request)
        {
        }

        protected virtual async Task AddAsyncCore(T source)
        {
        }

        public virtual async Task<object> AddAsync(TDto request)
        {
            var source = map.ReverseMap(request);

            var exist = await RetryHelper.RetryDbAsAsync(async () =>
            {
                return await restSet.AnyAsync(_ => _.Id.Equals(source.Id));
            });

            if (exist)
                return null;

            await AddMapAsyncCore(source, request);

            await restSet.AddAsync(source);

            await AddAsyncCore(source);

            await restDb.SaveChangesAsync();

            return source.Id;
        }

        protected virtual async Task UpdateAsyncCore(T source, T original)
        {
        }

        public virtual async Task<object> UpdateAsync(TDto request)
        {
            return await UpdateAsyncCore(request);
        }

        protected virtual async Task<object> UpdateAsyncCore(TDto request, Func<IQueryable<T>, IQueryable<T>> selector = null)
        {
            var source = map.ReverseMap(request);

            var original = await RetryHelper.RetryDbAsAsync(async () =>
            {
                var set = restSet.AsQueryable();
                if (selector != null)
                    set = selector(set);
                return await set.FirstOrDefaultAsync(_ => _.Id.Equals(source.Id));
            });

            if (original == null)
                return false;

            await WillUpdateAsyncCore(source, original);
            map.Map(source, original);
            await DidUpdateAsyncCore(source, original);

            await restDb.SaveChangesAsync();

            return true;
        }

        protected virtual async Task WillUpdateAsyncCore(T source, T original)
        {
        }

        protected virtual async Task DidUpdateAsyncCore(T source, T original)
        {
        }

        protected virtual async Task<object> UpdateAsync2(TDto request, Func<IQueryable<T>, IQueryable<T>> expression)
        {
            var source = map.ReverseMap(request);

            var original = await RetryHelper.RetryDbAsAsync(async () =>
            {
                var set = restSet.AsQueryable();

                if (expression != null)
                    set = expression(set);

                return await set.FirstOrDefaultAsync(_ => _.Id.Equals(source.Id));
            });

            if (original == null)
                return false;

            map.Map(source, original);

            await UpdateAsyncCore(source, original);

            await restDb.SaveChangesAsync();

            return true;
        }

        [HttpPatch]
        public virtual async Task<IActionResult> PatchAsync(TKey id, [FromBody] JsonPatchDocument<TDto> patch)
        {
            var originalDto = await FindAsync(id);

            if (originalDto == null)
                return NoContent();

            patch.ApplyTo(originalDto);

            var isValid = TryValidateModel(originalDto);
            if (!isValid)
            {
                return BadRequest(ModelState);
            }

            var result = await UpdateAsync(originalDto);

            return Ok(result);
        }

        protected virtual async Task WillRemoveAsyncCore(T source)
        {
        }

        public virtual async Task<object> RemoveAsync(TKey key)
        {            
            var original = await RetryHelper.RetryDbAsAsync(async () =>
            {
                return await restSet.FirstOrDefaultAsync(_ => _.Id.Equals(key));
            });

            if (original == null)
                return new RemoveOperationResult(RemoveOperationResult.RemoveResult.NotFound) { Succeded = false, ClientMistake = true };

            await WillRemoveAsyncCore(original);
            restSet.Remove(original);

            try
            {
                await restDb.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                var postgresException = e.InnerException as PostgresException;

                if (postgresException == null)
                    throw;

                if (postgresException.SqlState == "23503")
                    return new RemoveOperationResult(RemoveOperationResult.RemoveResult.ReferentialIntegrityViolation)
                    {
                        ErrorMessage = postgresException.MessageText,
                        Succeded = false
                    };

                throw;
            }

            return new RemoveOperationResult(RemoveOperationResult.RemoveResult.Success);
        }
    }
}
