using Enplt.Services.Api.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Enplt.Services.Api.Database;

public sealed class DatabaseContext : DbContext
{
    public DbSet<SaleManagerEntity> SaleManagers { get; set; } = null!;
    public DbSet<CalendarSlotEntity> CalendarSlots { get; set; } = null!;

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SaleManagerEntity>(
            entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .IsRequired();

                entity.Property(e => e.Languages).IsRequired();
                entity.Property(e => e.Products).IsRequired();
                entity.Property(e => e.CustomerRatings).IsRequired();
            }
        );

        modelBuilder.Entity<CalendarSlotEntity>(
            entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.From).IsRequired();
                entity.Property(e => e.To).IsRequired();
                entity.Property(e => e.Bookend).IsRequired();
                entity.Property(e => e.SaleManagerId).IsRequired();

                entity.HasOne(e => e.SaleManager)
                    .WithMany(m => m.CalendarSlots)
                    .HasForeignKey(e => e.SaleManagerId);
            }
        );
    }
}