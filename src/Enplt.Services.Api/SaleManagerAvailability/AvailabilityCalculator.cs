using Enplt.Services.Api.Database.Entities;

using ManagerCount = int;
using SaleManagerId = int;

namespace Enplt.Services.Api.SaleManagerAvailability;

// Potentially it is a domain logic and should be moved there with decoupling from CalendarSlotEntity
public sealed class AvailabilityCalculator
{
    private readonly IReadOnlyCollection<CalendarSlotEntity> _calendarSlots;

    public AvailabilityCalculator(IReadOnlyCollection<CalendarSlotEntity> calendarSlots)
    {
        _calendarSlots = calendarSlots;
    }

    public List<Availability> Calculate()
    {
        Dictionary<SaleManagerId, CalendarSlotEntity[]> bookedSlotsByManager =
            _calendarSlots
                .Where(slot => slot.Booked)
                .GroupBy(slot => slot.SaleManagerId)
                .ToDictionary(
                    group => group.Key,
                    group => group.ToArray()
                );

        Dictionary<SlotRange, ManagerCount> slotRanges = new();

        foreach (CalendarSlotEntity slot in _calendarSlots)
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
                        (mbs.From <= slot.From && slot.From < mbs.To)
                        || (mbs.From < slot.To && slot.To <= mbs.To)
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