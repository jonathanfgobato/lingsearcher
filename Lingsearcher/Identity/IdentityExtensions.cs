using Lingseacher.Entity;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace Lingseacher.Identity
{
    public static class IdentityExtensions
    {
        public static async Task<UserApplication> FindByNameOrEmailAsync
            (this UserManager<UserApplication> userManager, string usernameOrEmail, string password)
        {
            var username = usernameOrEmail;

            if (usernameOrEmail.Contains("@"))
            {
                var userForEmail = await userManager.FindByEmailAsync(usernameOrEmail);
                if (userForEmail != null)
                {
                    username = userForEmail.UserName;
                }
            }
            return await userManager.FindAsync(username, password);
        }
    }
}