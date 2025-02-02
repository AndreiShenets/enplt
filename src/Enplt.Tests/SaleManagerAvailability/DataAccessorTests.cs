using Enplt.Services.Api.Database;
using Enplt.Services.Api.Database.Entities;
using Enplt.Services.Api.Domain;
using Enplt.Services.Api.SaleManagerAvailability;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;

namespace Enplt.Tests.SaleManagerAvailability;

public sealed class DataAccessorTests : VerifyBase
{
    public DataAccessorTests() : base()
    {
    }

    // IMPORTANT! The test is just an example of testability and doesn't pretend to be a source of truth
    [Fact]
    public async Task DataAccessor_applies_correct_data_conditions()
    {
        // Arrange
        SaleManagerAvailabilityDataAccessor dataAccessor = new (CreateMockDbContextFactory());

        // Act
        List<CalendarSlotEntity> result =
            await dataAccessor.GetCalendarSlotsAsync(
                new DateOnly(2024, 05, 03),
                [ Products.Heatpumps ],
                SpokenLanguages.German,
                CustomerRatings.Silver,
                CancellationToken.None
            );

        // Assert
        await Verify(result).DontScrubDateTimes();
    }

    private IDatabaseContextFactory CreateMockDbContextFactory()
    {
        IDatabaseContext dbContext = Substitute.For<IDatabaseContext>();

        DbSet<SaleManagerEntity> saleManagerDbSet = SaleManagerEntities.AsQueryable().BuildMockDbSet();
        dbContext.SaleManagers.Returns(saleManagerDbSet);

        DbSet<CalendarSlotEntity> calendarSlotDbSet = CalendarSlotEntities.AsQueryable().BuildMockDbSet();
        dbContext.CalendarSlots.Returns(calendarSlotDbSet);

        var dbContextFactory = Substitute.For<IDatabaseContextFactory>();
        dbContextFactory.CreateDbContextAsync().Returns(Task.FromResult(dbContext));

        return dbContextFactory;
    }

    private static List<SaleManagerEntity> SaleManagerEntities { get; } =
        [
            new ()
            {
                Id = 1,
                Name = "Seller 1",
                Languages = new List<SpokenLanguages> { SpokenLanguages.German },
                Products = new List<Products> { Products.SolarPanels },
                CustomerRatings = new List<CustomerRatings> { CustomerRatings.Bronze }
            },
            new ()
            {
                Id = 2,
                Name = "Seller 2",
                Languages = new List<SpokenLanguages> { SpokenLanguages.German, SpokenLanguages.English },
                Products = new List<Products> { Products.SolarPanels, Products.Heatpumps },
                CustomerRatings = new List<CustomerRatings> { CustomerRatings.Gold, CustomerRatings.Silver, CustomerRatings.Bronze }
            },
            new ()
            {
                Id = 3,
                Name = "Seller 3",
                Languages = new List<SpokenLanguages> { SpokenLanguages.German, SpokenLanguages.English },
                Products = new List<Products> { Products.Heatpumps },
                CustomerRatings = new List<CustomerRatings> { CustomerRatings.Gold, CustomerRatings.Silver, CustomerRatings.Bronze }
            }
        ];

    private static List<CalendarSlotEntity> CalendarSlotEntities { get; } =
        [
            new ()
            {
                Id = 1,
                SaleManagerId = 1,
                From = new DateTimeOffset(2024, 05, 03, 10, 30, 00, TimeSpan.Zero),
                To = new DateTimeOffset(2024, 05, 03, 11, 30, 00, TimeSpan.Zero),
                Booked = false
            },
            new ()
            {
                Id = 2,
                SaleManagerId = 1,
                From = new DateTimeOffset(2024, 05, 03, 11, 00, 00, TimeSpan.Zero),
                To = new DateTimeOffset(2024, 05, 03, 12, 00, 00, TimeSpan.Zero),
                Booked = true
            },
            new ()
            {
                Id = 3,
                SaleManagerId = 1,
                From = new DateTimeOffset(2024, 05, 03, 11, 30, 00, TimeSpan.Zero),
                To = new DateTimeOffset(2024, 05, 03, 12, 30, 00, TimeSpan.Zero),
                Booked = false
            },
            new ()
            {
                Id = 4,
                SaleManagerId = 2,
                From = new DateTimeOffset(2024, 05, 03, 10, 30, 00, TimeSpan.Zero),
                To = new DateTimeOffset(2024, 05, 03, 11, 30, 00, TimeSpan.Zero),
                Booked = false
            },
            new ()
            {
                Id = 5,
                SaleManagerId = 2,
                From = new DateTimeOffset(2024, 05, 03, 11, 00, 00, TimeSpan.Zero),
                To = new DateTimeOffset(2024, 05, 03, 12, 00, 00, TimeSpan.Zero),
                Booked = false
            },
            new ()
            {
                Id = 6,
                SaleManagerId = 2,
                From = new DateTimeOffset(2024, 05, 03, 11, 30, 00, TimeSpan.Zero),
                To = new DateTimeOffset(2024, 05, 03, 12, 30, 00, TimeSpan.Zero),
                Booked = false
            },
            new ()
            {
                Id = 7,
                SaleManagerId = 3,
                From = new DateTimeOffset(2024, 05, 03, 10, 30, 00, TimeSpan.Zero),
                To = new DateTimeOffset(2024, 05, 03, 11, 30, 00, TimeSpan.Zero),
                Booked = true
            },
            new ()
            {
                Id = 8,
                SaleManagerId = 3,
                From = new DateTimeOffset(2024, 05, 03, 11, 00, 00, TimeSpan.Zero),
                To = new DateTimeOffset(2024, 05, 03, 12, 00, 00, TimeSpan.Zero),
                Booked = false
            },
            new ()
            {
                Id = 9,
                SaleManagerId = 3,
                From = new DateTimeOffset(2024, 05, 03, 11, 30, 00, TimeSpan.Zero),
                To = new DateTimeOffset(2024, 05, 03, 12, 30, 00, TimeSpan.Zero),
                Booked = false
            },
            new ()
            {
                Id = 10,
                SaleManagerId = 1,
                From = new DateTimeOffset(2024, 05, 04, 10, 30, 00, TimeSpan.Zero),
                To = new DateTimeOffset(2024, 05, 04, 11, 30, 00, TimeSpan.Zero),
                Booked = false
            },
            new ()
            {
                Id = 11,
                SaleManagerId = 1,
                From = new DateTimeOffset(2024, 05, 04, 11, 00, 00, TimeSpan.Zero),
                To = new DateTimeOffset(2024, 05, 04, 12, 00, 00, TimeSpan.Zero),
                Booked = true
            },
            new ()
            {
                Id = 12,
                SaleManagerId = 1,
                From = new DateTimeOffset(2024, 05, 04, 11, 30, 00, TimeSpan.Zero),
                To = new DateTimeOffset(2024, 05, 04, 12, 30, 00, TimeSpan.Zero),
                Booked = false
            },
            new ()
            {
                Id = 13,
                SaleManagerId = 2,
                From = new DateTimeOffset(2024, 05, 04, 10, 30, 00, TimeSpan.Zero),
                To = new DateTimeOffset(2024, 05, 04, 11, 30, 00, TimeSpan.Zero),
                Booked = false
            },
            new ()
            {
                Id = 14,
                SaleManagerId = 2,
                From = new DateTimeOffset(2024, 05, 04, 11, 00, 00, TimeSpan.Zero),
                To = new DateTimeOffset(2024, 05, 04, 12, 00, 00, TimeSpan.Zero),
                Booked = true
            },
            new ()
            {
                Id = 15,
                SaleManagerId = 2,
                From = new DateTimeOffset(2024, 05, 04, 11, 30, 00, TimeSpan.Zero),
                To = new DateTimeOffset(2024, 05, 04, 12, 30, 00, TimeSpan.Zero),
                Booked = false
            },
            new ()
            {
                Id = 16,
                SaleManagerId = 3,
                From = new DateTimeOffset(2024, 05, 04, 10, 30, 00, TimeSpan.Zero),
                To = new DateTimeOffset(2024, 05, 04, 11, 30, 00, TimeSpan.Zero),
                Booked = false
            },
            new ()
            {
                Id = 17,
                SaleManagerId = 3,
                From = new DateTimeOffset(2024, 05, 04, 11, 00, 00, TimeSpan.Zero),
                To = new DateTimeOffset(2024, 05, 04, 12, 00, 00, TimeSpan.Zero),
                Booked = false
            },
            new ()
            {
                Id = 18,
                SaleManagerId = 3,
                From = new DateTimeOffset(2024, 05, 04, 11, 30, 00, TimeSpan.Zero),
                To = new DateTimeOffset(2024, 05, 04, 12, 30, 00, TimeSpan.Zero),
                Booked = false
            }
        ];
}
