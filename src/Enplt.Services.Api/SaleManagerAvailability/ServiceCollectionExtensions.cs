namespace Enplt.Services.Api.SaleManagerAvailability;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSaleManagersAvailability(this IServiceCollection services)
    {
        services.AddSingleton<ISaleManagersAvailabilityRepository, SaleManagersAvailabilityRepository>();
        services.AddSingleton<SaleManagerAvailabilityQuery>();

        return services;
    }
}