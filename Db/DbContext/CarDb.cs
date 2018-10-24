using Db.DbContext.Db_Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Db.DbContext
{
    public class CarDb : IdentityDbContext<AppUser>
    {
        public CarDb(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;database=CarDb;Trusted_Connection=true;");
            }
        }
    }
}