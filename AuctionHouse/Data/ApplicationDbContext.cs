using AuctionHouse.Models.Entities;
using Microsoft.EntityFrameworkCore;
namespace AuctionHouse.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<VehicleEntity> Vehicles { get; set; }
    }

    public interface IApplicationDbContext
    {
        DbSet<VehicleEntity> Vehicles { get; set; }

        int SaveChanges();
    }
}