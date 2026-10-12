
using hackWebApplication1.Models;
using Microsoft.EntityFrameworkCore;

namespace hackWebApplication1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Room> Rooms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasIndex(r => r.RoomNumber)
                      .IsUnique();

                entity.Property(r => r.MonthlyFee)
                      .HasPrecision(18, 2);
            });
        }
    }
}