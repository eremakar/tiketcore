// Пример использования GeoJsonStationService
//
// 1. Загрузка данных из GeoJSON файла:
//
//    var geoService = new GeoJsonStationService();
//    await geoService.LoadFromFileAsync("export.geojson");
//
// 2. Расчет расстояния между двумя станциями по их именам:
//
//    try 
//    {
//        var distance = geoService.CalculateDistance("Алматы", "Астана");
//        Console.WriteLine($"Расстояние: {distance:F2} км");
//    }
//    catch (Exception ex) 
//    {
//        Console.WriteLine($"Ошибка: {ex.Message}");
//    }
//
// 3. Поиск станции по имени:
//
//    var station = geoService.FindStationByName("Алматы");
//    if (station != null) 
//    {
//        Console.WriteLine($"Станция: {station.Name}");
//        Console.WriteLine($"Координаты: {station.Latitude}, {station.Longitude}");
//        Console.WriteLine($"UIC код: {station.UicRef}");
//    }
//
// 4. Получение списка всех станций:
//
//    var allStations = geoService.GetAllStations();
//    foreach (var station in allStations) 
//    {
//        Console.WriteLine($"{station.Name} - ({station.Latitude}, {station.Longitude})");
//    }
//
// Примечания:
// - Поиск станций работает с точным и частичным совпадением имени (регистронезависимо)
// - Расстояние вычисляется по формуле Гаверсинуса (учитывает кривизну Земли)
// - Если станция не найдена, выбрасывается исключение

