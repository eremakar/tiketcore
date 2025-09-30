using Microsoft.EntityFrameworkCore;
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
                .Include(st => st.Tariff)
                    .ThenInclude(t => t.BaseFare)
                .FirstOrDefaultAsync(st => st.Id == seatTariffId);

            if (seatTariff == null)
                throw new ArgumentException($"SeatTariff with ID {seatTariffId} not found");

            if (seatTariff.Train?.Route?.Stations == null || seatTariff.Train.Route.Stations.Count < 2)
                throw new InvalidOperationException("Train route must contain at least 2 stations");

            if (seatTariff.Tariff?.BaseFare == null)
                throw new InvalidOperationException("SeatTariff must have Tariff with BaseFare");

            var stations = seatTariff.Train.Route.Stations
                .OrderBy(rs => rs.Order)
                .ToList();

            var processedItems = 0;

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

                    // Рассчитываем цену: расстояние * Tariff.IndexCoefficient * Tariff.VAT * Tariff.BaseFare.Price
                    var price = distance *
                               seatTariff.Tariff.IndexCoefficient *
                               seatTariff.Tariff.VAT *
                               seatTariff.Tariff.BaseFare.Price;
                    //var price = distance;

                    // Проверяем существующий элемент тарифа
                    var existingItem = await db.Set<SeatTariffItem>()
                        .FirstOrDefaultAsync(sti => sti.SeatTariffId == seatTariffId && 
                                                   sti.FromId == fromStation.StationId && 
                                                   sti.ToId == toStation.StationId);

                    if (existingItem != null)
                    {
                        // Обновляем существующий элемент
                        existingItem.Distance = distance;
                        existingItem.Price = price;
                    }
                    else
                    {
                    // Создаем новый элемент тарифа
                    var seatTariffItem = new SeatTariffItem
                    {
                        SeatTariffId = seatTariffId,
                        FromId = fromStation.StationId,
                        ToId = toStation.StationId,
                        Distance = distance,
                        Price = price
                    };

                        db.Set<SeatTariffItem>().Add(seatTariffItem);
                    }

                    processedItems++;
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
