using Microsoft.EntityFrameworkCore;

namespace Enplt.Services.Api.Database;

public class DatabaseContextFactory : IDatabaseContextFactory
{
    private readonly IDbContextFactory<DatabaseContext> _dbContextFactory;

    public DatabaseContextFactory(IDbContextFactory<DatabaseContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<IDatabaseContext> CreateDbContextAsync(CancellationToken cancellationToken = default)
    {
        DatabaseContext context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        return context;
    }
}
