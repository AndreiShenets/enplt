namespace Enplt.Services.Api.Database;

public interface IDatabaseContextFactory
{
    Task<IDatabaseContext> CreateDbContextAsync(CancellationToken cancellationToken = default);
}
