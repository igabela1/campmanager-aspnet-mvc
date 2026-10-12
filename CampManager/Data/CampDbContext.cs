using CampManager.Models;
using Microsoft.EntityFrameworkCore;

namespace CampManager.Data;

public class CampDbContext : DbContext
{
    public CampDbContext(DbContextOptions<CampDbContext> options)
        : base(options)
    {
    }

    public DbSet<Accommodation> Accommodations => Set<Accommodation>();

    public DbSet<Reservation> Reservations => Set<Reservation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Accommodation>()
            .Property(a => a.PricePerNight)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Reservation>()
            .Property(r => r.TotalPrice)
            .HasPrecision(12, 2);

        modelBuilder.Entity<Reservation>()
            .Property(r => r.CheckIn)
            .HasColumnType("date");

        modelBuilder.Entity<Reservation>()
            .Property(r => r.CheckOut)
            .HasColumnType("date");

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Accommodation)
            .WithMany()
            .HasForeignKey(r => r.AccommodationId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Reservation>()
            .HasIndex(r => new
            {
                r.AccommodationId,
                r.CheckIn,
                r.CheckOut
            });
    }
}