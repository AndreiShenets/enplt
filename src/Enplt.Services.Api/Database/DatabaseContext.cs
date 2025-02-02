using Enplt.Services.Api.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Enplt.Services.Api.Database;

public class DatabaseContext : DbContext, IDatabaseContext
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
                entity.ToTable("sales_managers");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(250)
                    .IsRequired();

                entity.Property(e => e.Languages)
                    .HasColumnName("languages")
                    .HasColumnType("varchar(100)[]")
                    .IsRequired();

                entity.Property(e => e.Products)
                    .HasColumnName("products")
                    .HasColumnType("varchar(100)[]")
                    .IsRequired();

                entity.Property(e => e.CustomerRatings)
                    .HasColumnName("customer_ratings")
                    .HasColumnType("varchar(100)[]")
                    .IsRequired();
            }
        );

        modelBuilder.Entity<CalendarSlotEntity>(
            entity =>
            {
                entity.ToTable("slots");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.From)
                    .HasColumnName("start_date")
                    .IsRequired();

                entity.Property(e => e.To)
                    .HasColumnName("end_date")
                    .IsRequired();

                entity.Property(e => e.Booked)
                    .HasColumnName("booked")
                    .IsRequired();

                entity.Property(e => e.SaleManagerId)
                    .HasColumnName("sales_manager_id")
                    .IsRequired();

                entity.HasOne(e => e.SaleManager)
                    .WithMany(m => m.CalendarSlots)
                    .HasForeignKey(e => e.SaleManagerId);
            }
        );
    }
}