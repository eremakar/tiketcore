using Data.Loading.Models;

namespace Data.Loading;

/// <summary>
/// Сервис для объединения данных о поездах из разных источников
/// </summary>
public class TrainDataMerger
{
    /// <summary>
    /// Объединяет данные из Excel (маршруты) и Word (составы вагонов)
    /// </summary>
    public List<RouteData> MergeExcelAndWordData(List<RouteData> excelRoutes, List<RouteData> wordRoutes)
    {
        Console.WriteLine($"\n=== ОБЪЕДИНЕНИЕ ДАННЫХ ===");
        Console.WriteLine($"Маршрутов из Excel: {excelRoutes.Count}");
        Console.WriteLine($"Маршрутов из Word: {wordRoutes.Count}");
        
        // Создаём словарь поездов из Word для быстрого поиска по номеру
        var wordTrainsDict = BuildTrainDictionary(wordRoutes);
        Console.WriteLine($"Поездов в словаре Word: {wordTrainsDict.Count}\n");

        // Объединяем данные: для каждого поезда из Excel ищем составы из Word
        int successCount = 0;
        int errorCount = 0;
        var missingTrains = new List<string>();

        foreach (var route in excelRoutes)
        {
            // Обрабатываем Train1
            if (MergeTrain(route.Train1, wordTrainsDict))
            {
                successCount++;
                Console.WriteLine($"✓ Поезд {route.Train1.Name}: объединены данные ({route.Train1.Wagons.Count} вагонов)");
            }
            else
            {
                errorCount++;
                missingTrains.Add(route.Train1.Name);
                Console.WriteLine($"✗ Поезд {route.Train1.Name}: составы не найдены в Word документах");
            }

            // Обрабатываем Train2
            if (MergeTrain(route.Train2, wordTrainsDict))
            {
                successCount++;
                Console.WriteLine($"✓ Поезд {route.Train2.Name}: объединены данные ({route.Train2.Wagons.Count} вагонов)");
            }
            else
            {
                errorCount++;
                missingTrains.Add(route.Train2.Name);
                Console.WriteLine($"✗ Поезд {route.Train2.Name}: составы не найдены в Word документах");
            }
        }

        PrintMergeStatistics(successCount, errorCount, missingTrains);

        return excelRoutes;
    }

    /// <summary>
    /// Создаёт словарь поездов из маршрутов для быстрого поиска
    /// Ключ - номер поезда без ведущих нулей
    /// </summary>
    private Dictionary<string, Train> BuildTrainDictionary(List<RouteData> routes)
    {
        var dict = new Dictionary<string, Train>();
        
        foreach (var route in routes)
        {
            var train1Key = route.Train1.Name.TrimStart('0');
            if (!dict.ContainsKey(train1Key))
                dict[train1Key] = route.Train1;
            
            var train2Key = route.Train2.Name.TrimStart('0');
            if (!dict.ContainsKey(train2Key))
                dict[train2Key] = route.Train2;
        }

        return dict;
    }

    /// <summary>
    /// Объединяет данные одного поезда
    /// Поиск выполняется по номеру без ведущих нулей
    /// </summary>
    private bool MergeTrain(Train excelTrain, Dictionary<string, Train> wordTrainsDict)
    {
        var trainKey = excelTrain.Name.TrimStart('0');
        if (wordTrainsDict.TryGetValue(trainKey, out var wordTrain))
        {
            excelTrain.Wagons = wordTrain.Wagons;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Выводит статистику объединения
    /// </summary>
    private void PrintMergeStatistics(int successCount, int errorCount, List<string> missingTrains)
    {
        Console.WriteLine($"\n=== ИТОГИ ОБЪЕДИНЕНИЯ ===");
        Console.WriteLine($"Успешно объединено: {successCount}");
        Console.WriteLine($"Не найдено: {errorCount}");

        if (missingTrains.Any())
        {
            Console.WriteLine($"\nОтсутствующие поезда:");
            foreach (var train in missingTrains)
            {
                Console.WriteLine($"  - {train}");
            }
        }
    }
}

