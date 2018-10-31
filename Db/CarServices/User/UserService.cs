using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Db.DbContext;
using Db.DbContext.Db_Models;
using Db.ViewModels;

using Microsoft.EntityFrameworkCore;

namespace Db.CarServices.User {
    public class UserService : IUser {
        private CarDb CarDb { get; }

        public UserService (CarDb carDb) {
            CarDb = carDb;
        }

        public async Task<AppUser> GetUserInfo (string userId = "", string userName = "") {
            if (!string.IsNullOrEmpty (userId) && !string.IsNullOrWhiteSpace (userId)) {
                var user1 = await CarDb.Users.FirstOrDefaultAsync (x => x.UserName.Equals (userName))
                    .ConfigureAwait (false);
                return user1;
            }

            if (string.IsNullOrEmpty (userName) || string.IsNullOrWhiteSpace (userName)) return new AppUser (); {
                var user2 = await CarDb.Users.FirstOrDefaultAsync (x => x.Id.Equals (userId)).ConfigureAwait (false);
                return user2;
            }
        }

        public async Task<IList<CarModel>> GetCarsAsync (int carId = 0, string userId = "") {
            var carList = await CarDb.Cars.ToListAsync ();
            if (!string.IsNullOrEmpty (userId) && !string.IsNullOrWhiteSpace (userId)) {
                return carList.Where (x => x.AppUser.Id.Equals (userId)).ToList ();
            }

            return carId != 0 ? carList.Where (x => x.Id == carId).ToList () : carList;
        }

        public async Task<IList<UserVm>> GetUsers (string name) {
            var list = await CarDb.Users.Where (x => x.UserName.Equals (name) == false)
                .ToListAsync ();

            var returnedList = new List<UserVm> ();
            foreach (var user in list) {
                var userModel = new UserVm {
                    UserName = user.UserName,
                    Email = user.Email,
                    UserId = user.Id
                };
                returnedList.Add (userModel);
            }

            return returnedList;
        }
    }
}