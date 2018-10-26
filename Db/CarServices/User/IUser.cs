using System.Collections.Generic;
using System.Threading.Tasks;
using Db.DbContext.Db_Models;

namespace Db.CarServices.User
{
    public interface IUser
    {
        Task<AppUser> getUserInfo(string userId = "", string userName = "");
        Task<IList<Car>> GetCarsAsync(int carId = 0, string userId = "");
    }
}