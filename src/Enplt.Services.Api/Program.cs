using System.Net;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// As port 3000 is a hard requirement, I am hardcoding it here to not deal with appsettings.json and environment variables
builder.WebHost.ConfigureKestrel((_, serverOptions) => serverOptions.Listen(IPAddress.Loopback, 3000));


WebApplication app = builder.Build();

app.MapPost("/calendar/query", context => context.Response.WriteAsync("Hello World!"));

await app.RunAsync();
