using Enplt.Services.Api.Database.Entities;
using Enplt.Services.Api.SaleManagerAvailability;

namespace Enplt.Tests.SaleManagerAvailability;

public sealed class AvailabilityCalculation : VerifyBase
{
    public AvailabilityCalculation() : base()
    {
    }

    // IMPORTANT! The test is just an example of testability and doesn't pretend to be a source of truth
    [Fact]
    public async Task AvailabilityCalculator_calculates_correct_result_for_happy_path()
    {
        // Arrange
        AvailabilityCalculator availabilityCalculator = new (CalendarSlotEntities);

        // Act
        List<Availability> result = availabilityCalculator.Calculate();

        // Assert
        await Verify(result).DontScrubDateTimes();
    }

    private static List<CalendarSlotEntity> CalendarSlotEntities { get; } =
        [
            new ()
            {
                Id = 4,
                From = new DateTimeOffset(2024, 05, 03, 10, 30, 00, TimeSpan.Zero),
                To = new DateTimeOffset(2024, 05, 03, 11, 30, 00, TimeSpan.Zero),
                Booked = false,
                SaleManagerId = 2
            },
            new ()
            {
                Id = 5,
                From = new DateTimeOffset(2024, 05, 03, 11, 00, 00, TimeSpan.Zero),
                To = new DateTimeOffset(2024, 05, 03, 12, 00, 00, TimeSpan.Zero),
                Booked = false,
                SaleManagerId = 2
            },
            new ()
            {
                Id = 6,
                From = new DateTimeOffset(2024, 05, 03, 11, 30, 00, TimeSpan.Zero),
                To = new DateTimeOffset(2024, 05, 03, 12, 30, 00, TimeSpan.Zero),
                Booked = false,
                SaleManagerId = 2
            },
            new ()
            {
                Id = 7,
                From = new DateTimeOffset(2024, 05, 03, 10, 30, 00, TimeSpan.Zero),
                To = new DateTimeOffset(2024, 05, 03, 11, 30, 00, TimeSpan.Zero),
                Booked = true,
                SaleManagerId = 3
            },
            new ()
            {
                Id = 8,
                From = new DateTimeOffset(2024, 05, 03, 11, 00, 00, TimeSpan.Zero),
                To = new DateTimeOffset(2024, 05, 03, 12, 00, 00, TimeSpan.Zero),
                Booked = false,
                SaleManagerId = 3
            },
            new ()
            {
                Id = 9,
                From = new DateTimeOffset(2024, 05, 03, 11, 30, 00, TimeSpan.Zero),
                To = new DateTimeOffset(2024, 05, 03, 12, 30, 00, TimeSpan.Zero),
                Booked = false,
                SaleManagerId = 3
            }
        ];
}
