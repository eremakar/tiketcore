using Data.Loading.Models;
using Newtonsoft.Json;

namespace Data.Loading;

public class TrainWordLoader
{
    private readonly RouteItemParser routeItemParser;

    public TrainWordLoader(RouteItemParser routeItemParser)
    {
        this.routeItemParser = routeItemParser;
    }

    public async Task<List<RouteData>> LoadTrainsFromWordDocuments(string directoryPath)
    {
        Console.WriteLine("=== Загрузка данных о поездах из Word документов ===\n");

        var directory = new DirectoryInfo(directoryPath);
        if (!directory.Exists)
        {
            throw new DirectoryNotFoundException($"Папка не найдена: {directoryPath}");
        }

        // Получаем все .doc и .docx файлы
        var wordFiles = directory.GetFiles("*.docx", SearchOption.TopDirectoryOnly)
            .Concat(directory.GetFiles("*.doc", SearchOption.TopDirectoryOnly))
            .OrderBy(f => f.Name)
            .ToList();

        if (wordFiles.Count == 0)
        {
            Console.WriteLine($"✗ Не найдено Word документов в папке: {directoryPath}");
            return new List<RouteData>();
        }

        Console.WriteLine($"Найдено файлов: {wordFiles.Count}\n");

        var routes = new List<RouteData>();

        foreach (var file in wordFiles)
        {
            try
            {
                Console.WriteLine($"Обрабатываю: {file.Name}");

                var service = new TrainWordService(file.FullName);
                var routeData = service.ReadDocument();

                if (routeData != null)
                {
                    routes.Add(routeData);
                    Console.WriteLine($"  ✓ Добавлен маршрут: {routeData.Name}\n");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"✗ Ошибка при обработке {file.Name}: {ex.Message}\n");
            }
        }

        var s = JsonConvert.SerializeObject(routes, Formatting.Indented);
        File.WriteAllText("trainWagons.json", s);

        if (routes.Count > 0)
        {
            // Парсим строковые данные в типизированные поля
            routeItemParser.ParseRoutes(routes);

            var json = JsonConvert.SerializeObject(routes, Formatting.Indented);
            TrainWordService.PrintData(routes);

            Console.WriteLine($"\n✓ Успешно обработано маршрутов: {routes.Count}");
        }
        else
        {
            Console.WriteLine($"\n✗ Не удалось обработать ни одного маршрута");
        }

        return routes;
    }
}

