using System.Reflection;
using Data.Repository;

namespace Ticketing.Data.TicketDb.DatabaseContext
{
    public static class TicketDbContextExtension
    {
        public static bool AllMigrationsApplied(this TicketDbContext context)
        {
            return context.AllMigrationsAppliedCore();
        }

        public static void EnsureSeeded(this TicketDbContext context)
        {
            context.EnsureSeededCore(_ =>
                {
                    var dbAssembly = Assembly.GetExecutingAssembly();
                    context.AddSeedFromJson(context.Roles, dbAssembly, "Role", _ => _.Id, null, null, "Data.TicketDb");
                    context.AddSeedFromJson(context.WagonTypes, dbAssembly, "WagonType", _ => _.Id, null, null, "Data.TicketDb");
                    context.AddSeedFromJson(context.WagonFeatures, dbAssembly, "WagonFeature", _ => _.Id, (a, b) => { a.Name = b.Name; a.Code = b.Code; }, null, "Data.TicketDb");
                    context.AddSeedFromJson(context.Services, dbAssembly, "Service", _ => _.Id, null, null, "Data.TicketDb");
                    context.AddSeedFromJson(context.SeatTypes, dbAssembly, "SeatType", _ => _.Id, null, null, "Data.TicketDb");
                    context.AddSeedFromJson(context.SeatPurposes, dbAssembly, "SeatPurpose", _ => _.Id, null, null, "Data.TicketDb");
                    context.AddSeedFromJson(context.TicketStates, dbAssembly, "TicketState", _ => _.Id, null, null, "Data.TicketDb");
                    context.AddSeedFromJson(context.TrainCategories, dbAssembly, "TrainCategory", _ => _.Id, null, null, "Data.TicketDb");
                    context.AddSeedFromJson(context.WagonClasses, dbAssembly, "WagonClass", _ => _.Id, (a, b) => a.Name = b.Name, null, "Data.TicketDb");
                    context.AddSeedFromJson(context.Seasons, dbAssembly, "Season", _ => _.Id, null, null, "Data.TicketDb");
                    context.SaveChanges();
                });
        }
    }
}
