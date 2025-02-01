using System.Net;
using System.Text.Json.Serialization;
using Enplt.Services.Api.Converters;
using Enplt.Services.Api.Database;
using Enplt.Services.Api.Mappings;
using Enplt.Services.Api.SaleManagerAvailability;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.ConfigureWebHost();

builder.Services.ConfigureHttpJsonOptions();

builder.Services.AddDatabaseContextFactory(builder.Configuration);
builder.Services.AddSaleManagersAvailability();


WebApplication app = builder.Build();

app.MapPost("/calendar/query", CalendarQueryPostMapping.ExecuteAsync);

await app.RunAsync();


public static class ProgramExtensions
{
    public static void ConfigureHttpJsonOptions(this IServiceCollection services) =>
        services.ConfigureHttpJsonOptions(
            options =>
            {
                // Adds support of enums in requests
                options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.SerializerOptions.Converters.Add(new UtcDateTimeOffsetConverter());
            }
        );

    public static void AddDatabaseContextFactory(this IServiceCollection services, IConfiguration configuration) =>
        services.AddDbContextFactory<DatabaseContext>(
            options =>
            {
                const string ConnectionStringName = "Postgres";

                string connectionString = configuration.GetConnectionString(ConnectionStringName)
                    ?? throw new InvalidOperationException(
                        $"Connection string '{ConnectionStringName}' must be provided"
                    );

                options.UseNpgsql(
                    connectionString,
                    npgSqlOptions =>
                    {
                        npgSqlOptions.EnableRetryOnFailure();
                    }
                );
            }
        );

    public static void ConfigureWebHost(this WebApplicationBuilder builder) =>
        // As port 3000 is a hard requirement, I am hardcoding it here to not deal with appsettings.json and environment variables
        builder.WebHost.ConfigureKestrel((_, serverOptions) => serverOptions.Listen(IPAddress.Loopback, 3000));
}