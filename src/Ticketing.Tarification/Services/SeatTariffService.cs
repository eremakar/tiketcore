using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Ticketing.Tarifications.Data.TicketDb.DatabaseContext;
using Ticketing.Tarifications.Data.TicketDb.Entities;
using Ticketing.Tarifications.Data.TicketDb.Entities.Tarifications;
using Ticketing.Tarifications.Models.Dtos.Tarifications;

namespace Ticketing.Tarifications.Services
{
    public class SeatTariffService
    {
        private readonly TicketDbContext db;

        public SeatTariffService(TicketDbContext db)
        {
            this.db = db;
        }

        /// <summary>
        /// Рассчитывает тарифы мест для всех пар станций маршрута и сохраняет их в базу данных
        /// </summary>
        /// <param name="seatTariffId">ID тарифа места</param>
        /// <returns>Количество обработанных элементов тарифа</returns>
        public async Task<int> CalculateAsync(long seatTariffId)
        {
            var seatTariff = await db.Set<SeatTariff>()
                .Include(st => st.Train)
                    .ThenInclude(t => t.Route)
                        .ThenInclude(r => r.Stations)
                            .ThenInclude(rs => rs.Station)
                .Include(st => st.BaseFare)
                .Include(st => st.TrainCategory)
                .Include(st => st.Items)
                .Include(st => st.Tariff)
                    .ThenInclude(t => t.TrainCategories)
                        .ThenInclude(tc => tc.TrainCategory)
                .Include(st => st.Tariff)
                    .ThenInclude(t => t.Wagons)
                        .ThenInclude(w => w.Wagon)
                            .ThenInclude(wm => wm.Type)
                .Include(st => st.Tariff)
                    .ThenInclude(t => t.Wagons)
                        .ThenInclude(w => w.SeatTypes)
                            .ThenInclude(st => st.SeatType)
                .Include(st => st.Tariff)
                    .ThenInclude(t => t.WagonTypes)
                        .ThenInclude(wt => wt.WagonType)
                .AsSplitQuery()
                .FirstOrDefaultAsync(st => st.Id == seatTariffId);

            if (seatTariff == null)
                throw new ArgumentException($"SeatTariff with ID {seatTariffId} not found");

            if (seatTariff.Train?.Route?.Stations == null || seatTariff.Train.Route.Stations.Count < 2)
                throw new InvalidOperationException("Train route must contain at least 2 stations");

            if (seatTariff.BaseFare == null)
                throw new InvalidOperationException("SeatTariff must have BaseFare");

            if (seatTariff.TrainCategory == null)
                throw new InvalidOperationException("SeatTariff must have TrainCategory");

            if (seatTariff.Tariff == null)
                throw new InvalidOperationException("SeatTariff must have Tariff");

            var stations = seatTariff.Train.Route.Stations
                .OrderBy(rs => rs.Order)
                .ToList();

            var processedItems = 0;

            // Выбираем tariffTrainCategory по TrainCategoryId
            var tariffTrainCategory = seatTariff.Tariff.TrainCategories?
                .FirstOrDefault(tc => tc.TrainCategoryId == seatTariff.Train.CategoryId);

            if (tariffTrainCategory == null)
                throw new InvalidOperationException($"TariffTrainCategoryItem not found for TrainCategoryId {seatTariff.TrainCategoryId}");

            // Создаем матрицу всех пар станций от-до
            for (int i = 0; i < stations.Count; i++)
            {
                for (int j = i + 1; j < stations.Count; j++)
                {
                    var fromStation = stations[i];
                    var toStation = stations[j];

                    // Пропускаем если fromStation == toStation
                    if (fromStation.StationId == toStation.StationId)
                        continue;

                    // Рассчитываем расстояние между станциями
                    var distance = CalculateDistance(fromStation, toStation);

                    // Проходим по вагонам в тарифе
                    foreach (var tariffWagon in seatTariff.Tariff.Wagons ?? Enumerable.Empty<TariffWagonItem>())
                    {
                        if (tariffWagon.Wagon == null)
                            continue;

                        // Выбираем tariffWagonType по WagonTypeId
                        var tariffWagonType = seatTariff.Tariff.WagonTypes?
                            .FirstOrDefault(wt => wt.WagonTypeId == tariffWagon.Wagon.TypeId);

                        if (tariffWagonType == null)
                            continue; // Пропускаем вагон, если для его типа нет тарифа

                        // Проходим по типам мест в тарифе вагона
                        foreach (var tariffSeatType in tariffWagon.SeatTypes ?? Enumerable.Empty<TariffSeatTypeItem>())
                        {
                            if (tariffSeatType.SeatTypeId == null)
                                continue;

                            // Рассчитываем indexCoefficient
                            var indexCoefficient = tariffTrainCategory.IndexCoefficient *
                                                 tariffWagon.IndexCoefficient *
                                                 tariffSeatType.IndexCoefficient *
                                                 tariffWagonType.IndexCoefficient;

                            // Рассчитываем базовую цену без НДС
                            var basePriceWithoutVAT = distance *
                                                     seatTariff.BaseFare.Price *
                                                     indexCoefficient;

                            // Применяем НДС: цена = базовая цена * (1 + VAT/100)
                            var price = basePriceWithoutVAT * (1 + seatTariff.Tariff.VAT / 100);

                            // Формируем параметры расчета
                            var calculationParams = new SeatTariffCalculationParameters
                            {
                                Expression = $"Price = distance:{distance} * baseFarePrice:{seatTariff.BaseFare.Price} * trainCategoryCoefficient:{tariffTrainCategory.IndexCoefficient} * wagonCoefficient:{tariffWagon.IndexCoefficient} * seatTypeCoefficient:{tariffSeatType.IndexCoefficient} * wagonTypeCoefficient:{tariffWagonType.IndexCoefficient} * vat:{seatTariff.Tariff.VAT} = {price}"
                            };

                            var calculationParamsJson = JsonSerializer.Serialize(calculationParams, new JsonSerializerOptions
                            {
                                WriteIndented = false,
                                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                            });

                            // Проверяем существующий элемент тарифа
                            var existingItem = seatTariff.Items?.FirstOrDefault(sti =>
                                sti.FromId == fromStation.StationId &&
                                sti.ToId == toStation.StationId &&
                                sti.WagonId == tariffWagon.WagonId &&
                                sti.SeatTypeId == tariffSeatType.SeatTypeId);

                            if (existingItem != null)
                            {
                                // Обновляем существующий элемент
                                existingItem.Distance = distance;
                                existingItem.Price = price;
                                existingItem.CalculationParameters = calculationParamsJson;
                            }
                            else
                            {
                                // Создаем новый элемент тарифа
                                var seatTariffItem = new SeatTariffItem
                                {
                                    SeatTariffId = seatTariffId,
                                    FromId = fromStation.StationId,
                                    ToId = toStation.StationId,
                                    WagonId = tariffWagon.WagonId,
                                    SeatTypeId = tariffSeatType.SeatTypeId,
                                    Distance = distance,
                                    Price = price,
                                    CalculationParameters = calculationParamsJson
                                };

                                db.Set<SeatTariffItem>().Add(seatTariffItem);
                            }

                            processedItems++;
                        }
                    }
                }
            }

            await db.SaveChangesAsync();
            return processedItems;
        }

        /// <summary>
        /// Рассчитывает расстояние между двумя станциями маршрута как разность накопленных расстояний
        /// </summary>
        private double CalculateDistance(RouteStation fromStation, RouteStation toStation)
        {
            // Если у станций есть накопленное расстояние (>= 0), используем разность
            if (fromStation.Distance >= 0 && toStation.Distance >= 0)
            {
                return Math.Abs(toStation.Distance - fromStation.Distance);
            }

            // Иначе используем порядок станций как приблизительное расстояние
            return Math.Abs(toStation.Order - fromStation.Order) * 10; // 10 км на станцию по умолчанию
        }
    }
}
