using Enplt.Services.Api.Database.Entities;
using Enplt.Services.Api.Domain;

namespace Enplt.Services.Api.SaleManagerAvailability;

public sealed class SaleManagerAvailabilityQuery
{
    private readonly ISaleManagerAvailabilityDataAccessor _availabilityDataAccessor;

    public SaleManagerAvailabilityQuery(ISaleManagerAvailabilityDataAccessor availabilityDataAccessor)
    {
        _availabilityDataAccessor = availabilityDataAccessor;
    }

    public async Task<List<Availability>> ExecuteAsync(
        DateOnly date,
        IReadOnlyCollection<Products> products,
        SpokenLanguages language,
        CustomerRatings rating,
        CancellationToken cancellationToken
    )
    {
        List<CalendarSlotEntity> slotEntities =
            await _availabilityDataAccessor.GetCalendarSlotsAsync(
                date,
                products,
                language,
                rating,
                cancellationToken
            );

        AvailabilityCalculator availabilityCalculator = new (slotEntities);
        List<Availability> availabilities = availabilityCalculator.Calculate();

        return availabilities;
    }
}
