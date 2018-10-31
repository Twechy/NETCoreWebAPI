using System.Collections.Generic;
using System.Threading.Tasks;
using Db.CarServices.User;
using Db.DbContext;
using Db.DbContext.Db_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace netcorewebapi.Controllers.v1
{
    [Authorize]
    [Route("api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly CarDb _carDb;
        private readonly IUser _userService;

        public UserController(CarDb carDb, IUser userService)
        {
            _carDb = carDb;
            _userService = userService;
        }

        [HttpGet]
        [Route("userInfo")]
        public async Task<IActionResult> GetUserInfo([FromQuery] string userId = "", string userName = "")
        {
            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrWhiteSpace(userId))
            {
                return Ok(await _userService.GetUserInfo(userId).ConfigureAwait(false));
            }

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrWhiteSpace(userName))
            {
                return Ok(await _userService.GetUserInfo(userName: userName));
            }

            return NoContent();
        }

        [HttpGet]
        [Route("getUsers")]
        public async Task<IActionResult> GetUsers([FromQuery] string name)
        {
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrWhiteSpace(name))
            {
                return Ok(await _userService.GetUsers(name));
            }

            return NoContent();
        }
    }
}