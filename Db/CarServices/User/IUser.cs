using System.Collections.Generic;
using System.Threading.Tasks;

using Db.DbContext.Db_Models;
using Db.ViewModels;

namespace Db.CarServices.User {
    public interface IUser {
        Task<AppUser> GetUserInfo (string userId = "", string userName = "");
        Task<IList<CarModel>> GetCarsAsync (int carId = 0, string userId = "");
        Task<IList<UserVm>> GetUsers (string name);
    }
}