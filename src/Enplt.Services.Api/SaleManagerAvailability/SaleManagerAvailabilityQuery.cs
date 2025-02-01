using Enplt.Services.Api.Database.Entities;
using Enplt.Services.Api.Domain;

using ManagerCount = int;
using SaleManagerId = int;

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
        CustomerRating rating,
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

        Dictionary<SaleManagerId, CalendarSlotEntity[]> bookedSlotsByManager =
            slotEntities
                .Where(slot => slot.Booked)
                .GroupBy(slot => slot.SaleManagerId)
                .ToDictionary(
                    group => group.Key,
                    group => group.ToArray()
                );

        Dictionary<SlotRange, ManagerCount> slotRanges = new();

        foreach (CalendarSlotEntity slot in slotEntities)
        {
            if (slot.Booked)
            {
                continue;
            }

            if (!bookedSlotsByManager.TryGetValue(slot.SaleManagerId, out CalendarSlotEntity[]? managerBookedSlots))
            {
                managerBookedSlots = [];
            }

            bool slotAlreadyBooked =
                managerBookedSlots.Any(
                    mbs =>
                        (mbs.From <= slot.From && slot.From <= mbs.To)
                        || (mbs.From <= slot.To && slot.To <= mbs.To)
                );

            if (!slotAlreadyBooked)
            {
                SlotRange slotRange = new (slot.From, slot.To);
                slotRanges.TryGetValue(slotRange, out ManagerCount count);
                ++count;
                slotRanges[slotRange] = count;
            }
        }

        List<Availability> availabilities =
            slotRanges.Select(
                slotRange =>
                    new Availability
                    {
                        StartDate = slotRange.Key.From,
                        AvailableCount = slotRange.Value
                    }
            )
            .ToList();

        return availabilities;
    }

    private record struct SlotRange(DateTimeOffset From, DateTimeOffset To);
}

public sealed class Availability
{
    public required int AvailableCount { get; init; }
    public required DateTimeOffset StartDate { get; init; }
}