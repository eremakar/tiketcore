namespace Ticketing.Data.TicketDb.Ids
{
    public enum TicketStateIds : long
    {
        Undefined = 0,
        created = 1,
        payed = 2,
        completed = 3,
        rejected = 4,
        returned = 5,
        moneyReturned = 6
    }
}
