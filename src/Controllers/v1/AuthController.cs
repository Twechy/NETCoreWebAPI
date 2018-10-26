using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Db.CarServices.User;
using Db.DbContext;
using Db.DbContext.Db_Models;
using Db.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using netcorewebapi.Ioc;

namespace netcorewebapi.Controllers.v1
{
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUser UserService;
        private readonly CarDb _db;
        private readonly SignInManager<AppUser> _mSignInManager;
        private readonly UserManager<AppUser> _mUserManager;

        public AuthController(CarDb context,
            UserManager<AppUser> userManager,
            IUser userService,
            SignInManager<AppUser> signInManager)
        {
            UserService = userService;
            _db = context;
            _mUserManager = userManager;
            _mSignInManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginVm user)
        {
            await _mSignInManager.SignOutAsync().ConfigureAwait(false);
            var userFromDb = await _db.Users.FirstOrDefaultAsync(x => x.UserName.Equals(user.UserName))
                .ConfigureAwait(false);
            if (userFromDb == null) return Ok(ResponseVm.FailearLogin("Login Failed"));
            var signInAsync = await _mSignInManager.PasswordSignInAsync(userFromDb, user.Password, true, false)
                .ConfigureAwait(false);

            if (!signInAsync.Succeeded) return Unauthorized();

            var jwtToken = GenerateToken(userFromDb);

            userFromDb.AccessToken = jwtToken;
            _db.Entry(userFromDb).Property("AccessToken").IsModified = true;
            _db.Users.Update(userFromDb);
            await _db.SaveChangesAsync().ConfigureAwait(false);

            return Ok(new ResponseVm
            {
                Status = 1,
                Message = "Login Successful",
                userId = userFromDb.Id,
                Rule = userFromDb.Rule,
                Email = user.Email,
                UserName = user.UserName,
                ProviderName = "ServerProvider",
                AccessToken = jwtToken
            });
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get( /*[FromBody] LoginVm user*/)
        {
            var carsAsync = await UserService.GetCarsAsync();
            return Ok(carsAsync);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] LoginVm user)
        {
            /* setup a datault rule for default users */

            var result = await _mUserManager.CreateAsync(new AppUser
            {
                UserName = user.UserName,
                Email = user.Email,
                Rule = user.Rule
            }, user.Password).ConfigureAwait(false);

            if (!result.Succeeded) return Ok();
            var userFromDb = await _mUserManager.FindByNameAsync(user.UserName).ConfigureAwait(false);

            if (userFromDb == null) return Unauthorized();
            var token = GenerateToken(userFromDb);

            return Ok(new ResponseVm
            {
                Status = 1,
                userId = userFromDb.Id,
                Rule = userFromDb.Rule,
                Message = "Login successful",
                AccessToken = token,
                ProviderName = "Server_Provider",
            });
        }

        // [Authorize]
        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> Logout([FromQuery] string userId)
        {
            var user = await _mUserManager.FindByIdAsync(userId).ConfigureAwait(false);
            // var user = await GetUserFromJwt (HttpContext, _mUserManager).ConfigureAwait (false);

            if (user == null) return NotFound(ResponseVm.FailearLogin());
            var r = HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

            await _mUserManager.UpdateSecurityStampAsync(user).ConfigureAwait(false);

            var response = new ResponseVm();
            if (r.IsCompletedSuccessfully)
            {
                user.AccessToken = null;
                _db.Entry(user).Property("AccessToken").IsModified = true;
                _db.Users.Update(user);
                await _db.SaveChangesAsync().ConfigureAwait(false);

                response.Status = 1;
                response.Message = "Logout Successful";

                return Ok(new ResponseVm
                {
                    Status = 1,
                    Message = "Logout Successful"
                });
            }

            response.Status = 0;
            response.Message = "Logout Failed";

            return Ok(new ResponseVm
            {
                Status = 0,
                Message = "Logout Failed"
            });
        }

        [Authorize]
        [HttpGet]
        [Route("userList")]
        public async Task<IActionResult> GetUserList()
        {
            var list = await _db.Users.ToListAsync().ConfigureAwait(false);

            var userList = new List<UserVm>();

            foreach (var i in list)
            {
                var model = new UserVm
                {
                    UserId = i.Id,
                    Email = i.Email,
                    UserName = i.UserName,
                    Rule = i.Rule
                };
                userList.Add(model);
            }

            return Ok(userList);
        }

        private static async Task<AppUser> GetUserFromJwt(
            HttpContext context,
            UserManager<AppUser> userManager)
        {
            var contextUser = context.User;
            var name = contextUser.Identity.Name;

            var user = await userManager.FindByNameAsync(name).ConfigureAwait(false);
            return user;
        }

        private static string GenerateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Rule),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var jwtSecret = IoCContainer.Configuration["Jwt:SecretKey"];
            var jwtIssuer = IoCContainer.Configuration["Jwt:Issuer"];
            var jwtAudience = IoCContainer.Configuration["Jwt:Audience"];

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                jwtIssuer,
                jwtAudience,
                claims,
                expires: DateTime.Now.AddMonths(8),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}