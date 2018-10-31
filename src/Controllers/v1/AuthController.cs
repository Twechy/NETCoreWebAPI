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

namespace netcorewebapi.Controllers.v1 {
    [Route ("api/v1/[controller]")]
    public class AuthController : ControllerBase {
        private readonly IUser userService;
        private readonly CarDb carDb;
        private readonly UserManager<AppUser> _mUserManager;
        private readonly SignInManager<AppUser> _mSignInManager;

        public AuthController (
            CarDb context,
            IUser userService,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager) {
            this.userService = userService;
            carDb = context;
            _mUserManager = userManager;
            _mSignInManager = signInManager;
        }

        [HttpPost ("login")]
        public async Task<IActionResult> Login ([FromBody] LoginVm user) {
            await _mSignInManager.SignOutAsync ().ConfigureAwait (false);
            var userFromDb = await carDb.Users.FirstOrDefaultAsync (x => x.UserName.Equals (user.UserName))
                .ConfigureAwait (false);
            if (userFromDb == null) return Ok (ResponseVm.FailearLogin ("Login Failed"));
            var signInAsync = await _mSignInManager.PasswordSignInAsync (userFromDb, user.Password, true, false)
                .ConfigureAwait (false);

            if (!signInAsync.Succeeded) return Unauthorized ();

            var jwtToken = GenerateToken (userFromDb);

            userFromDb.AccessToken = jwtToken;
            carDb.Entry (userFromDb).Property ("AccessToken").IsModified = true;
            carDb.Users.Update (userFromDb);
            await carDb.SaveChangesAsync ().ConfigureAwait (false);

            return Ok (new ResponseVm {
                Status = 1,
                    Message = "Login Successful",
                    UserId = userFromDb.Id,
                    Rule = userFromDb.Rule,
                    Email = userFromDb.Email,
                    UserName = userFromDb.UserName,
                    ProviderName = "ServerProvider",
                    AccessToken = jwtToken
            });
        }

        [HttpPost]
        [Route ("register")]
        public async Task<IActionResult> Register ([FromBody] LoginVm user) {
            /* setup a default rule for default users */

            var result = await _mUserManager.CreateAsync (new AppUser {
                UserName = user.UserName,
                    Email = user.Email
            }, user.Password).ConfigureAwait (false);

            if (!result.Succeeded) return Ok ();
            var userFromDb = await _mUserManager.FindByNameAsync (user.UserName).ConfigureAwait (false);

            if (userFromDb == null) return Unauthorized ();
            var token = GenerateToken (userFromDb);

            return Ok (new ResponseVm {
                Status = 1,
                    UserId = userFromDb.Id,
                    Rule = userFromDb.Rule,
                    Message = "Login successful",
                    AccessToken = token,
                    ProviderName = "Server_Provider",
            });
        }

        [Authorize]
        [HttpGet]
        [Route ("logout")]
        public async Task<IActionResult> Logout ([FromQuery] string userId) {
            var token = await GetUserFromJwt (HttpContext, _mUserManager).ConfigureAwait (false);

            if (token == null) return NotFound (ResponseVm.FailearLogin ());
            var r = HttpContext.SignOutAsync (IdentityConstants.ApplicationScheme);

            await _mUserManager.UpdateSecurityStampAsync (token).ConfigureAwait (false);

            var response = new ResponseVm ();
            if (r.IsCompletedSuccessfully) {
                token.AccessToken = null;
                carDb.Entry (token).Property ("AccessToken").IsModified = true;
                carDb.Users.Update (token);
                await carDb.SaveChangesAsync ().ConfigureAwait (false);

                response.Status = 1;
                response.Message = "Logout Successful";

                return Ok (new ResponseVm {
                    Status = 1,
                        Message = "Logout Successful"
                });
            }

            response.Status = 0;
            response.Message = "Logout Failed";

            return Ok (new ResponseVm {
                Status = 0,
                    Message = "Logout Failed"
            });
        }

        [Authorize]
        [HttpGet]
        [Route ("userList")]
        public async Task<IActionResult> GetUserList () {
            var list = await carDb.Users.ToListAsync ().ConfigureAwait (false);

            var userList = new List<UserVm> ();

            foreach (var i in list) {
                var model = new UserVm {
                    UserId = i.Id,
                    Email = i.Email,
                    UserName = i.UserName,
                    Rule = i.Rule
                };
                userList.Add (model);
            }

            return Ok (userList);
        }

        [HttpGet]
        [Route ("isAuth")]
        public async Task<IActionResult> IsAuth () {
            return Ok (await IsAuthenticated ());
        }

        private static string GenerateToken (AppUser user) {
            var claims = new List<Claim> {
                new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid ().ToString ("N")),
                new Claim (JwtRegisteredClaimNames.Email, user.Email),
                new Claim (ClaimsIdentity.DefaultNameClaimType, user.UserName),
                //                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Rule),
                new Claim (ClaimTypes.NameIdentifier, user.Id)
            };

            var jwtSecret = IoCContainer.Configuration["Jwt:SecretKey"];
            var jwtIssuer = IoCContainer.Configuration["Jwt:Issuer"];
            var jwtAudience = IoCContainer.Configuration["Jwt:Audience"];

            var credentials = new SigningCredentials (
                new SymmetricSecurityKey (Encoding.UTF8.GetBytes (jwtSecret)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken (
                jwtIssuer,
                jwtAudience,
                claims,
                expires : DateTime.Now.AddMonths (8),
                signingCredentials : credentials
            );
            return new JwtSecurityTokenHandler ().WriteToken (token);
        }

        private async Task<bool> IsAuthenticated () {
            var user = await GetUserFromJwt (HttpContext, _mUserManager);
            return user?.AccessToken != null;
        }

        private static async Task<AppUser> GetUserFromJwt (HttpContext context, UserManager<AppUser> userManager) {
            var contextUser = context.User;
            var name = contextUser.Identity.Name;

            if (string.IsNullOrEmpty (name) || string.IsNullOrWhiteSpace (name)) return null;
            var user = await userManager.FindByNameAsync (name).ConfigureAwait (false);
            return user;
        }
    }
}