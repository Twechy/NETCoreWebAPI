using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Db.DbContext
{
    public class CarContextFactory : IDesignTimeDbContextFactory<CarDb>
    {
        public CarDb CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CarDb>();
            optionsBuilder.UseSqlServer("Server=.;database=CarDb;Trusted_Connection=true;");
            return new CarDb(optionsBuilder.Options);
        }
    }
}