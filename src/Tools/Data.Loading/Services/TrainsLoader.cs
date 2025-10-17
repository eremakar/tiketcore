using Data.Loading.Models;

namespace Data.Loading;

/// <summary>
/// Сервис для загрузки и обработки данных о поездах
/// </summary>
public class TrainsLoader
{
    private readonly TrainExcelLoader trainExcelLoader;
    private readonly TrainWordLoader trainWordLoader;
    private readonly TrainDataMerger trainDataMerger;
    private readonly RailwayGraphService graphService;

    public TrainsLoader(
        TrainExcelLoader trainExcelLoader,
        TrainWordLoader trainWordLoader,
        TrainDataMerger trainDataMerger,
        RailwayGraphService graphService)
    {
        this.trainExcelLoader = trainExcelLoader;
        this.trainWordLoader = trainWordLoader;
        this.trainDataMerger = trainDataMerger;
        this.graphService = graphService;
    }

    /// <summary>
    /// Выполняет полную загрузку и обработку данных о поездах
    /// </summary>
    public async Task<TrainsLoadResult> LoadAndProcessTrainsAsync(string excelPath, string wordDirectoryPath, string dotOutputPath)
    {
        // Загрузка данных из Excel
        var routes = await trainExcelLoader.LoadTrainsFromExcel(excelPath);

        // Загрузка данных из Word документов
        var routesFromWord = await trainWordLoader.LoadTrainsFromWordDocuments(wordDirectoryPath);

        // Объединяем данные из Excel и Word
        var allRoutes = trainDataMerger.MergeExcelAndWordData(routes, routesFromWord);

        Console.WriteLine($"\n=== ОБЩАЯ СТАТИСТИКА ===");
        Console.WriteLine($"Всего маршрутов для обработки: {allRoutes.Count}\n");

        // Построение графа станций
        var graph = graphService.BuildGraph(allRoutes);

        // Получаем узловые станции
        var hubStations = graphService.GetHubStations(graph);

        // Проверка аномалий в расстояниях между станциями (с учетом hub станций)
        graphService.CheckDistanceAnomalies(allRoutes, graph);

        // Экспорт графа в формат DOT для визуализации
        graphService.ExportToDot(graph, dotOutputPath);

        return new TrainsLoadResult
        {
            Routes = allRoutes,
            Graph = graph,
            HubStations = hubStations
        };
    }
}

