using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities;
using Api.AspNetCore.Models.Secure;
using Data.Repository;

// Config
var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Development";
var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

var loaderConfig = new LoaderConfig();
config.Bind(loaderConfig);

var apiBase = loaderConfig.Api?.BaseUrl?.TrimEnd('/');
var authUrl = loaderConfig.Api?.AuthUrl;
var stationsUrl = loaderConfig.Api?.StationsUrl;
var username = loaderConfig.Api?.Username;
var password = loaderConfig.Api?.Password;
var paging = loaderConfig.Paging ?? new PagingConfig();

var connectionString = config.GetConnectionString("PostgresConnection") ?? Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

// Backward compatibility: derive endpoints from BaseUrl if not explicitly set
if ((string.IsNullOrWhiteSpace(authUrl) || string.IsNullOrWhiteSpace(stationsUrl)) && !string.IsNullOrWhiteSpace(apiBase))
{
    authUrl ??= $"{apiBase}/api/v1/authenticate";
    stationsUrl ??= $"{apiBase}/api/v1/stations/search";
}

if (string.IsNullOrWhiteSpace(connectionString) || string.IsNullOrWhiteSpace(authUrl) || string.IsNullOrWhiteSpace(stationsUrl) || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
{
    Console.Error.WriteLine("Missing configuration. Ensure ConnectionStrings:PostgresConnection and Api:AuthUrl, Api:StationsUrl, Api:Username, Api:Password are set (or Api:BaseUrl for fallback).");
    return 1;
}

using var http = new HttpClient();

// Auth
var authResponse = await http.PostAsJsonAsync(authUrl, new JwtTokenRequest
{
    Username = username,
    Password = password
});
authResponse.EnsureSuccessStatusCode();
var token = await authResponse.Content.ReadFromJsonAsync<JwtToken>();
if (token == null || string.IsNullOrWhiteSpace(token.Key))
{
    Console.Error.WriteLine("Failed to obtain JWT token.");
    return 1;
}

http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Key);

// Fetch stations
var searchPayload = new
{
    paging = new { skip = paging.Skip, returnCount = paging.ReturnCount }
};
var stationsResp = await http.PostAsJsonAsync(stationsUrl, searchPayload);
stationsResp.EnsureSuccessStatusCode();
var stationsPaged = await stationsResp.Content.ReadFromJsonAsync<PagedList<StationDtoWire>>();
if (stationsPaged?.Result == null)
{
    Console.Error.WriteLine("Stations response is empty.");
    return 1;
}

// Save to DB
var dbOptions = new DbContextOptionsBuilder<TicketDbContext>()
    .UseNpgsql(connectionString)
    .Options;

using var db = new TicketDbContext(dbOptions);

foreach (var s in stationsPaged.Result)
{
    var entity = await db.Stations!.FirstOrDefaultAsync(_ => _.Code == s.code);
    if (entity == null)
    {
        entity = new Station();
        db.Stations!.Add(entity);
    }

    entity.Name = s.name;
    entity.Code = s.code;
    entity.ShortName = s.shortName;
    entity.ShortNameLatin = s.shortNameEn ?? s.shortName;
    entity.CityCode = s.countryCode;
}

await db.SaveChangesAsync();

Console.WriteLine($"Loaded {stationsPaged.Result.Count} stations.");
return 0;

// minimal DTO for deserialization
public class StationDtoWire
{
    public long id { get; set; }
    public string? name { get; set; }
    public string? code { get; set; }
    public string? shortName { get; set; }
    public string? nameEn { get; set; }
    public string? shortNameEn { get; set; }
    public string? railwayName { get; set; }
    public string? railwayShortName { get; set; }
    public string? countryCode { get; set; }
    public string? countryTlf { get; set; }
}

public class LoaderConfig
{
    public ApiConfig? Api { get; set; }
    public PagingConfig? Paging { get; set; }
}

public class ApiConfig
{
    public string? BaseUrl { get; set; }
    public string? AuthUrl { get; set; }
    public string? StationsUrl { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
}

public class PagingConfig
{
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 1000;
    public bool ReturnCount { get; set; } = false;
}
