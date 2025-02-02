using Enplt.Services.Api.Database;
using Enplt.Services.Api.Database.Entities;
using Enplt.Services.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace Enplt.Services.Api.SaleManagerAvailability;

public sealed class SaleManagerAvailabilityDataAccessor : ISaleManagerAvailabilityDataAccessor
{
    private readonly IDatabaseContextFactory _dbContextFactory;

    public SaleManagerAvailabilityDataAccessor(IDatabaseContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<List<CalendarSlotEntity>> GetCalendarSlotsAsync(
        DateOnly date,
        IReadOnlyCollection<Products> products,
        SpokenLanguages language,
        CustomerRatings rating,
        CancellationToken cancellationToken
    )
    {
        await using IDatabaseContext dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        IQueryable<int> matchingSaleManagerIds =
            dbContext.SaleManagers
                .Where(
                    manager =>
                        manager.CustomerRatings.Contains(rating)
                        && manager.Languages.Contains(language)
                        && products.All(p => manager.Products.Contains(p))
                )
                .Select(manager => manager.Id);

        DateTimeOffset from = new (date, TimeOnly.MinValue, TimeSpan.Zero);
        DateTimeOffset to = new (date, TimeOnly.MaxValue, TimeSpan.Zero);

        return await dbContext.CalendarSlots
            .Where(
                slot =>
                    matchingSaleManagerIds.Contains(slot.SaleManagerId)
                    && slot.From >= from
                    && slot.To <= to
            )
            .ToListAsync(cancellationToken);
    }
}