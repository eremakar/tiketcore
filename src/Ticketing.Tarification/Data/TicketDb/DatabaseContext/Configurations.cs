using Ticketing.Tarifications.Data.TicketDb.Entities;
using Ticketing.Tarifications.Data.TicketDb.Entities.Tarifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ticketing.Tarifications.Data.TicketDb.DatabaseContext
{
    public class UsersConfiguration : IEntityTypeConfiguration<User>
    {
        public bool IsInMemoryDb { get; set; }

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            if (!IsInMemoryDb)
            {
                builder.Property(_ => _.Avatar).HasColumnType("jsonb");
            }
            else
            {
                builder.Ignore(_ => _.Avatar);
            }
        }
    }

    public class RolesConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }

    public class UserRolesConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }

    public class RoutesConfiguration : IEntityTypeConfiguration<Entities.Route>
    {
        public void Configure(EntityTypeBuilder<Entities.Route> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Train)
                .WithMany();
        }
    }

    public class StationsConfiguration : IEntityTypeConfiguration<Station>
    {
        public bool IsInMemoryDb { get; set; }

        public void Configure(EntityTypeBuilder<Station> builder)
        {
            builder.HasKey(x => x.Id);

            if (!IsInMemoryDb)
            {
                builder.Property(_ => _.Depots).HasColumnType("jsonb");
            }
            else
            {
                builder.Ignore(_ => _.Depots);
            }
        }
    }

    public class RailwaiesConfiguration : IEntityTypeConfiguration<Railway>
    {
        public void Configure(EntityTypeBuilder<Railway> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }

    public class RailwayStationsConfiguration : IEntityTypeConfiguration<RailwayStation>
    {
        public void Configure(EntityTypeBuilder<RailwayStation> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }

    public class DepotsConfiguration : IEntityTypeConfiguration<Depot>
    {
        public void Configure(EntityTypeBuilder<Depot> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }

    public class RouteStationsConfiguration : IEntityTypeConfiguration<RouteStation>
    {
        public void Configure(EntityTypeBuilder<RouteStation> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }

    public class TrainsConfiguration : IEntityTypeConfiguration<Train>
    {
        public void Configure(EntityTypeBuilder<Train> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Route)
                .WithMany();
            builder.HasOne(x => x.Plan)
                .WithMany();
        }
    }

    public class TrainSchedulesConfiguration : IEntityTypeConfiguration<TrainSchedule>
    {
        public void Configure(EntityTypeBuilder<TrainSchedule> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }

    public class TrainWagonsConfiguration : IEntityTypeConfiguration<TrainWagon>
    {
        public void Configure(EntityTypeBuilder<TrainWagon> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }

    public class TrainWagonsPlansConfiguration : IEntityTypeConfiguration<TrainWagonsPlan>
    {
        public void Configure(EntityTypeBuilder<TrainWagonsPlan> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Train)
                .WithMany();
        }
    }

    public class TrainWagonsPlanWagonsConfiguration : IEntityTypeConfiguration<TrainWagonsPlanWagon>
    {
        public void Configure(EntityTypeBuilder<TrainWagonsPlanWagon> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }

    public class WagonModelsConfiguration : IEntityTypeConfiguration<WagonModel>
    {
        public bool IsInMemoryDb { get; set; }

        public void Configure(EntityTypeBuilder<WagonModel> builder)
        {
            builder.HasKey(x => x.Id);

            if (!IsInMemoryDb)
            {
                builder.Property(_ => _.PictureS3).HasColumnType("jsonb");
            }
            else
            {
                builder.Ignore(_ => _.PictureS3);
            }
        }
    }

    public class WagonTypesConfiguration : IEntityTypeConfiguration<WagonType>
    {
        public void Configure(EntityTypeBuilder<WagonType> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }

    public class ServicesConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }

    public class SeatTypesConfiguration : IEntityTypeConfiguration<SeatType>
    {
        public void Configure(EntityTypeBuilder<SeatType> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }

    public class SeatsConfiguration : IEntityTypeConfiguration<Seat>
    {
        public void Configure(EntityTypeBuilder<Seat> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }

    public class SeatSegmentsConfiguration : IEntityTypeConfiguration<SeatSegment>
    {
        public void Configure(EntityTypeBuilder<SeatSegment> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }

    public class SeatCountSegmentsConfiguration : IEntityTypeConfiguration<SeatCountSegment>
    {
        public bool IsInMemoryDb { get; set; }

        public void Configure(EntityTypeBuilder<SeatCountSegment> builder)
        {
            builder.HasKey(x => x.Id);

            if (!IsInMemoryDb)
            {
                builder.Property(_ => _.Tickets).HasColumnType("jsonb");
            }
            else
            {
                builder.Ignore(_ => _.Tickets);
            }
        }
    }

    public class SeatCountReservationsConfiguration : IEntityTypeConfiguration<SeatCountReservation>
    {
        public void Configure(EntityTypeBuilder<SeatCountReservation> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }

    public class ConnectionsConfiguration : IEntityTypeConfiguration<Connection>
    {
        public void Configure(EntityTypeBuilder<Connection> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }

    public class SeatReservationsConfiguration : IEntityTypeConfiguration<SeatReservation>
    {
        public void Configure(EntityTypeBuilder<SeatReservation> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }

    public class TicketsConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }

    public class TicketStatesConfiguration : IEntityTypeConfiguration<TicketState>
    {
        public void Configure(EntityTypeBuilder<TicketState> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }

    public class TicketServicesConfiguration : IEntityTypeConfiguration<TicketService>
    {
        public void Configure(EntityTypeBuilder<TicketService> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }

    public class TicketPaymentsConfiguration : IEntityTypeConfiguration<TicketPayment>
    {
        public void Configure(EntityTypeBuilder<TicketPayment> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }

    public class TrainCategoriesConfiguration : IEntityTypeConfiguration<TrainCategory>
    {
        public void Configure(EntityTypeBuilder<TrainCategory> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }

    public class WagonClassesConfiguration : IEntityTypeConfiguration<WagonClass>
    {
        public void Configure(EntityTypeBuilder<WagonClass> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }

    public class SeasonsConfiguration : IEntityTypeConfiguration<Season>
    {
        public void Configure(EntityTypeBuilder<Season> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }

    public class BaseFaresConfiguration : IEntityTypeConfiguration<BaseFare>
    {
        public void Configure(EntityTypeBuilder<BaseFare> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }

    public class TariffsConfiguration : IEntityTypeConfiguration<Tariff>
    {
        public void Configure(EntityTypeBuilder<Tariff> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }

    public class TariffTrainCategoryItemsConfiguration : IEntityTypeConfiguration<TariffTrainCategoryItem>
    {
        public void Configure(EntityTypeBuilder<TariffTrainCategoryItem> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }

    public class TariffWagonItemsConfiguration : IEntityTypeConfiguration<TariffWagonItem>
    {
        public void Configure(EntityTypeBuilder<TariffWagonItem> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }

    public class TariffWagonTypeItemsConfiguration : IEntityTypeConfiguration<TariffWagonTypeItem>
    {
        public void Configure(EntityTypeBuilder<TariffWagonTypeItem> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }

    public class TariffSeatTypeItemsConfiguration : IEntityTypeConfiguration<TariffSeatTypeItem>
    {
        public void Configure(EntityTypeBuilder<TariffSeatTypeItem> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }

    public class SeatTariffsConfiguration : IEntityTypeConfiguration<SeatTariff>
    {
        public void Configure(EntityTypeBuilder<SeatTariff> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }

    public class SeatTariffItemsConfiguration : IEntityTypeConfiguration<SeatTariffItem>
    {
        public void Configure(EntityTypeBuilder<SeatTariffItem> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }

    public class SeatTariffHistoriesConfiguration : IEntityTypeConfiguration<SeatTariffHistory>
    {
        public void Configure(EntityTypeBuilder<SeatTariffHistory> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
