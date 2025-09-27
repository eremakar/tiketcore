namespace Ticketing.Data.TicketDb.Ids
{
    public enum ServiceIds : long
    {
        Undefined = 0,
        nonRefundableTickets = 1,
        childUnder5SameBlank = 2,
        childUnder5SeparateBlank = 3,
        childFreeDocSeparateRequest = 4,
        childUnder10 = 5,
        childFreeEBVipExtraSeatDOPP = 6,
        childrenInKazakhstan = 7,
        sapsanRoundTrip = 8,
        specialTariffOBRTRefund = 9,
        specialTariffSingle = 10,
        specialTariffTKS = 11,
        loyaltyProgram = 12,
        awardTicketsInternational = 13,
        kaliningradTransitLithuania = 14,
        fssBenefit = 15,
        finlandRussiaInteraction = 16,
        ticketsForDisabled = 17
    }
}
