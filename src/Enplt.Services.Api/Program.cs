using System.Net;
using System.Text.Json.Serialization;
using Enplt.Services.Api;
using Enplt.Services.Api.Converters;
using Enplt.Services.Api.Mappings;
using Enplt.Services.Api.SaleManagerAvailability;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// As port 3000 is a hard requirement, I am hardcoding it here to not deal with appsettings.json and environment variables
builder.WebHost.ConfigureKestrel((_, serverOptions) => serverOptions.Listen(IPAddress.Loopback, 3000));
builder.Services.ConfigureHttpJsonOptions(
    options =>
    {
        // Adds support of enums in requests
        options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.SerializerOptions.Converters.Add(new UtcDateTimeOffsetConverter());
    }
);

builder.Services.AddSaleManagersAvailability();


WebApplication app = builder.Build();

app.MapPost("/calendar/query", CalendarQueryPostMapping.ExecuteAsync);

await app.RunAsync();
