using Enplt.Services.Api.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Enplt.Services.Api.Database;

public interface IDatabaseContext : IDisposable, IAsyncDisposable
{
    DbSet<SaleManagerEntity> SaleManagers { get; set; }

    DbSet<CalendarSlotEntity> CalendarSlots { get; set; }
}
