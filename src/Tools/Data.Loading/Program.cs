using Data.Loading;
using Data.Loading.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ticketing.Data.TicketDb.DatabaseContext;
//using System.Text.Json;

// Конфигурация
var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// Настройка DI контейнера
var services = new ServiceCollection();

// Добавляем DbContext
var connectionString = configuration.GetConnectionString("PostgresConnection");
services.AddEntityFrameworkNpgsql()
    .AddDbContext<TicketDbContext>(options =>
    {
        options.UseNpgsql(connectionString, builder =>
        {
            builder.EnableRetryOnFailure();
        });
    });

// Регистрируем сервисы
services.AddScoped<RouteItemParser>();
services.AddScoped<TrainExcelLoader>();
services.AddScoped<TrainWordLoader>();
services.AddScoped<TrainDataMerger>();
services.AddScoped<RailwayGraphService>();
services.AddScoped<TrainsLoader>();
services.AddScoped<TrainsDatabaseService>();
services.AddScoped<ScheduleExcelLoader>();
services.AddScoped<DataFixService>();
//services.AddSingleton<GeoJsonStationService>();

var serviceProvider = services.BuildServiceProvider();

// Получаем сервисы
using var scope = serviceProvider.CreateScope();
var trainsLoader = scope.ServiceProvider.GetRequiredService<TrainsLoader>();
var dbService = scope.ServiceProvider.GetRequiredService<TrainsDatabaseService>();
//var scheduleLoader = scope.ServiceProvider.GetRequiredService<ScheduleExcelLoader>();
var dataFixService = scope.ServiceProvider.GetRequiredService<DataFixService>();

// Пути к файлам
var excelPath = Path.Combine(AppContext.BaseDirectory, "поезда 2025-2026гг..xlsx");
var wordDirectoryPath = Path.Combine(AppContext.BaseDirectory, "составы");
var dotPath = Path.Combine(AppContext.BaseDirectory, "railway_graph.dot");
var schedulePath = "Расписания.xlsx";
var scheduleCachePath = "schedule_cache.json";

//// Загружаем и обрабатываем все данные
var result = await trainsLoader.LoadAndProcessTrainsAsync(excelPath, wordDirectoryPath, dotPath);

// Исправляем данные
//await dataFixService.FixRouteNamesAsync();
//await dataFixService.FixRouteStationTimesAsync();
//await dataFixService.FixStopTimesAsync();
//await dataFixService.DeleteRouteStationsWithNullStopAsync();
//await dataFixService.DeleteInvalidWagonsAsync();
//await dataFixService.RestoreFirstAndLastStationsAsync(result);
await dataFixService.RestoreAllStationsTimesAsync(result);


// Сохраняем в БД
//await dbService.SaveTrainsAsync(result);

// Загружаем расписание
//List<Data.Loading.Models.Train>? scheduleTrains = null;

//if (File.Exists(scheduleCachePath))
//{
//    Console.WriteLine("\n=== ЗАГРУЗКА РАСПИСАНИЯ ИЗ КЕША ===");
//    var json = await File.ReadAllTextAsync(scheduleCachePath);
//    scheduleTrains = JsonSerializer.Deserialize<List<Data.Loading.Models.Train>>(json);
//    Console.WriteLine($"✓ Загружено {scheduleTrains?.Count ?? 0} поездов из кеша");
//}
//else if (File.Exists(schedulePath))
//{
//    Console.WriteLine("\n=== ЗАГРУЗКА РАСПИСАНИЯ ИЗ EXCEL ===");
//    scheduleTrains = await scheduleLoader.LoadScheduleAsync(schedulePath);
//    
//    // Сохраняем в кеш
//    var json = JsonSerializer.Serialize(scheduleTrains, new JsonSerializerOptions { WriteIndented = true });
//    await File.WriteAllTextAsync(scheduleCachePath, json);
//    Console.WriteLine($"✓ Расписание сохранено в кеш: {scheduleCachePath}");
//}
//else
//{
//    Console.WriteLine($"\n⚠ Файл расписания не найден: {schedulePath}");
//}

// Обновляем расстояния в БД
//if (scheduleTrains != null && scheduleTrains.Count > 0)
//{
//    await dbService.UpdateDistancesAsync(scheduleTrains);
//}

// Работа с GeoJSON станциями
//var geoJsonService = scope.ServiceProvider.GetRequiredService<GeoJsonStationService>();
//var geoJsonPath = Path.Combine(AppContext.BaseDirectory, "export.geojson");

//if (File.Exists(geoJsonPath))
//{
//    await geoJsonService.LoadFromFileAsync(geoJsonPath);

//    // Пример расчета расстояния между двумя станциями
//    // Раскомментируйте и укажите имена станций для расчета расстояния:
//    try
//    {
//        var distance = geoJsonService.CalculateDistance("Астана Нұрлы-Жол", "Сороковая");
//        Console.WriteLine($"Расстояние: {distance:F2} км");
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"Ошибка расчета расстояния: {ex.Message}");
//    }
//}
//else
//{
//    Console.WriteLine($"\nФайл GeoJSON не найден: {geoJsonPath}");
//}

return 0;
