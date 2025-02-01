using Enplt.Services.Api.Database.Entities;
using Enplt.Services.Api.Domain;

namespace Enplt.Services.Api.SaleManagerAvailability;

public interface ISaleManagerAvailabilityDataAccessor
{
    Task<List<CalendarSlotEntity>> GetCalendarSlotsAsync(
        DateOnly date,
        IReadOnlyCollection<Products> products,
        SpokenLanguages language,
        CustomerRating rating,
        CancellationToken cancellationToken
    );
}