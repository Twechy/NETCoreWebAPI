using Db.DbContext.Db_Models;
using Db.DbContext.Db_Models.CarModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Db.DbContext
{
    public class CarDb : IdentityDbContext<AppUser>
    {
        public CarDb(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<PayType> PayType { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Engine> Engines { get; set; }
        public DbSet<Fule> Fules { get; set; }
        public Transmission Transmission { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer("Server=.;database=CarDb;Trusted_Connection=true;");
        }
    }
}