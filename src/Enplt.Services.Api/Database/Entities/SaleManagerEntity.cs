using Enplt.Services.Api.Domain;

namespace Enplt.Services.Api.Database.Entities;

public sealed class SaleManagerEntity
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required List<SpokenLanguages> Languages { get; init; }
    public required List<Products> Products { get; init; }
    public required List<CustomerRating> CustomerRatings { get; init; }

    public List<CalendarSlotEntity>? CalendarSlots { get; init; }
}