using Enplt.Services.Api.Database;
using Enplt.Services.Api.Database.Entities;
using Enplt.Services.Api.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using static System.Console;

try
{
    HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
    builder.Logging.ClearProviders();
    builder.Logging.AddSimpleConsole();

    builder.Services.AddDatabaseContextFactory(builder.Configuration);

    IHost host = builder.Build();


    Dictionary<int, List<SpokenLanguages>> languages =
        new ()
        {
            { 0, [SpokenLanguages.English] },
            { 1, [SpokenLanguages.German] },
            { 2, [SpokenLanguages.English, SpokenLanguages.German] },
        };

    Dictionary<int, List<Products>> products =
        new ()
        {
            { 0, [Products.Heatpumps] },
            { 1, [Products.SolarPanels] },
            { 2, [Products.Heatpumps, Products.SolarPanels] },
        };

    Dictionary<int, List<CustomerRatings>> customerRatings =
        new ()
        {
            { 0, [CustomerRatings.Gold] },
            { 1, [CustomerRatings.Silver] },
            { 2, [CustomerRatings.Bronze] },
            { 3, [CustomerRatings.Gold, CustomerRatings.Silver] },
            { 4, [CustomerRatings.Gold, CustomerRatings.Bronze] },
            { 5, [CustomerRatings.Silver, CustomerRatings.Bronze] },
            { 6, [CustomerRatings.Gold, CustomerRatings.Silver, CustomerRatings.Bronze] },
        };

    Random random = Random.Shared;

    IServiceProvider serviceProvider = host.Services.GetRequiredService<IServiceProvider>();
    using IServiceScope scope = serviceProvider.CreateScope();

    IDbContextFactory<DatabaseContext> dbContextFactory =
        scope.ServiceProvider.GetRequiredService<IDbContextFactory<DatabaseContext>>();

    using var dbContext = dbContextFactory.CreateDbContext();

    IExecutionStrategy executionStrategy = dbContext.Database.CreateExecutionStrategy();

    executionStrategy.ExecuteInTransaction(
        () =>
        {
            for (int i = 0; i < 1000; i++)
            {
                dbContext.SaleManagers.Add(
                    new SaleManagerEntity
                    {
                        Id = 0, //autogeneration
                        Name = $"Seller {i}",
                        Languages = languages[random.Next(0, 3)],
                        Products = products[random.Next(0, 3)],
                        CustomerRatings = customerRatings[random.Next(0, 6)],
                    }
                );
            }

            WriteLine("Adding sales managers...");
            dbContext.SaveChanges();
            WriteLine("Done");

            SaleManagerEntity[] saleManagers = dbContext.SaleManagers.ToArray();

            // 5 years each day including weekends 16 slots from 8:00 to 17:00.
            // Around 35 sale managers per day to get 999999 slots
            DateTimeOffset startDate = new (2024, 05, 05, hour: 00, 00, 00, TimeSpan.Zero);
            for (int yearIndex = 0; yearIndex < 5; yearIndex++)
            {
                DateTimeOffset yearStartDate = startDate.AddYears(yearIndex);

                for (int dayIndex = 0; dayIndex < 365; dayIndex++)
                {
                    DateTimeOffset dayStartDateTime = yearStartDate.AddDays(dayIndex).AddHours(8);
                    DateTimeOffset dayEndDateTime = dayStartDateTime.AddHours(9);

                    WriteLine($"Adding calendar slots for {dayStartDateTime:yyyy-MM-dd HH:mm:ss} to {dayEndDateTime:yyyy-MM-dd HH:mm:ss}");

                    while (dayStartDateTime < dayEndDateTime)
                    {
                        random.Shuffle(saleManagers);

                        for (int i = 0; i < 35; i++)
                        {
                            dbContext.CalendarSlots.Add(
                                new CalendarSlotEntity
                                {
                                    Id = 0, //autogeneration
                                    SaleManagerId = saleManagers[i].Id,
                                    From = dayStartDateTime,
                                    To = dayStartDateTime.AddHours(1),
                                    Booked = random.NextDouble() > 0.75
                                }
                            );
                        }

                        dayStartDateTime = dayStartDateTime.AddMinutes(30);
                    }

                    dbContext.SaveChanges();
                }
            }
        },
        () => true
    );
}
catch (Exception e)
{
    WriteLine(e);
}

WriteLine("Done");
ReadLine();

return 0;
