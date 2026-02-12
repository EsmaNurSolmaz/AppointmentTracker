using Microsoft.EntityFrameworkCore;
using RandevuYonetimSistemi.Models;
using RandevuYonetimSistemi.Helpers;

namespace RandevuYonetimSistemi.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<AppointmentModel> Appointments { get; set; }
        public DbSet<CustomerModel> Customers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionStringConfig.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppointmentModel>()
                .HasKey(a => a.AppointmentId);

            modelBuilder.Entity<CustomerModel>()
                .HasKey(c => c.CustomerId);

            modelBuilder.Entity<AppointmentModel>()
                .HasOne(a => a.Customer)
                .WithMany(c => c.Appointments)
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
