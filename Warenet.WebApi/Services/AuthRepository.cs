using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace Warenet.WebApi
{
    public class AuthRepository : IDisposable
    {
        private UserManager<IdentityUser> _userManager;
        private IdentityDbContext _db;

        public AuthRepository(string Site)
        {
            _db = new IdentityDbContext(Site);
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_db));
        }

        public async Task<IdentityResult> RegisterUser(JObject user)
        {
            string UserName = user["userName"].Value<string>();
            string Pwd = user["password"].Value<string>();

            IdentityUser sysUser = new IdentityUser
            {
                UserName = UserName
            };
            var result = await _userManager.CreateAsync(sysUser, Pwd);
            return result;
        }

        public async Task<IdentityUser> FindUser(string userId, string pwd)
        {
            IdentityUser user = await _userManager.FindAsync(userId,pwd);
            return user;
        }

        public void Dispose()
        {
            _userManager.Dispose();
        }
    }
}