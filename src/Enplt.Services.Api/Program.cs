using System.Net;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

// As port 3000 is a hard requirement, I am hardcoding it here to not deal with appsettings.json and environment variables
builder.WebHost.ConfigureKestrel((_, serverOptions) => serverOptions.Listen(IPAddress.Loopback, 3000));


WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

await app.RunAsync();
