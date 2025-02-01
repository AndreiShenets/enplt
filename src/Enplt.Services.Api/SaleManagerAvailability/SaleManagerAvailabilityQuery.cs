using Enplt.Services.Api.Domain;

namespace Enplt.Services.Api.SaleManagerAvailability;

public sealed class SaleManagerAvailabilityQuery
{
    public SaleManagerAvailabilityQuery(ISaleManagersAvailabilityRepository availabilityRepository)
    {
    }

    public Task<List<Availability>> ExecuteAsync(
        DateOnly date,
        List<Products> products,
        SpokenLanguages language,
        CustomerRating rating
    )
    {
        throw new NotImplementedException();
    }
}

public sealed class Availability
{
    public required int AvailableCount { get; init; }
    public required DateTimeOffset StartDate { get; init; }
}