namespace Enplt.Services.Api.Database.Entities;

public sealed class CalendarSlotEntity
{
    public required int Id { get; init; }
    public required DateTimeOffset From { get; init; }
    public required DateTimeOffset To { get; init; }
    public bool Booked { get; set; }
    public required int SaleManagerId { get; init; }

    public SaleManagerEntity? SaleManager { get; set; }
}