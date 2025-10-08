namespace Ticketing.Data.TicketDb.Ids
{
    public enum PeriodicityIds : long
    {
        Undefined = 0,
        daily = 1,
        everyOtherDay = 2,
        oncePerWeek = 3,
        twicePerWeek = 4,
        thricePerWeek = 5,
        fourTimesPerWeek = 6,
        fiveTimesPerWeekExceptTueWed = 7,
        sixTimesPerWeekExceptTueWed = 8,
        oncePerEightDays = 9,
        evenDaysExceptWeekendsHolidays = 10,
        oddDays = 11,
        tuWedFriSatSun = 12,
        weekendsAndHolidays = 13,
        byDates = 14
    }
}
