using Microsoft.EntityFrameworkCore;

namespace VehicleTracker.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Records> Records { get; set; }
        public DbSet<Category> Category { get; set; }
    }
}
