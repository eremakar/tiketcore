using Data.Loading.Models;
using Newtonsoft.Json;

namespace Data.Loading;

public class TrainExcelLoader
{
    private readonly RouteItemParser routeItemParser;

    public TrainExcelLoader(RouteItemParser routeItemParser)
    {
        this.routeItemParser = routeItemParser;
    }

    public async Task<List<RouteData>> LoadTrainsFromExcel(string excelPath)
    {
        Console.WriteLine("=== Загрузка данных о поездах из Excel ===\n");

        var service = new TrainExcelService(excelPath);
        
        try
        {
            var routes = service.ReadAllSheets();
            
            // Парсим строковые данные в типизированные поля
            routeItemParser.ParseRoutes(routes);
            
            var s = JsonConvert.SerializeObject(routes, Formatting.Indented);
            service.PrintData(routes);

            Console.WriteLine($"\n✓ Успешно обработано маршрутов: {routes.Count}");
            
            return routes;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"✗ Ошибка при чтении Excel: {ex.Message}");
            throw;
        }
    }
}

