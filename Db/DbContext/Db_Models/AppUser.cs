using Microsoft.AspNetCore.Identity;

namespace Db.DbContext.Db_Models
{
    public class AppUser : IdentityUser
    {
        public string AccessToken { get; set; }
        public string Provider { get; set; }
        public string Rule { get; set; }
        public string ImageUrl { get; set; }
    }
}