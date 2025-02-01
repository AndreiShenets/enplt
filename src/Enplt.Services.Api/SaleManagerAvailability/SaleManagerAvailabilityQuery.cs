using Enplt.Services.Api.Database.Entities;
using Enplt.Services.Api.Domain;

namespace Enplt.Services.Api.SaleManagerAvailability;

public sealed class SaleManagerAvailabilityQuery
{
    private readonly ISaleManagerAvailabilityRepository _availabilityRepository;

    public SaleManagerAvailabilityQuery(ISaleManagerAvailabilityRepository availabilityRepository)
    {
        _availabilityRepository = availabilityRepository;
    }

    public async Task<List<Availability>> ExecuteAsync(
        DateOnly date,
        IReadOnlyCollection<Products> products,
        SpokenLanguages language,
        CustomerRating rating,
        CancellationToken cancellationToken
    )
    {
        List<CalendarSlotEntity> slotEntities =
            await _availabilityRepository.GetCalendarSlotsAsync(
                date,
                products,
                language,
                rating,
                cancellationToken
            );

        return [];
    }
}

public sealed class Availability
{
    public required int AvailableCount { get; init; }
    public required DateTimeOffset StartDate { get; init; }
}