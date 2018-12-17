using Lingseacher.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Lingseacher;

namespace Lingseacher.Bll
{
    public class RegisterBll
    {
        public static UserManager<UserApplication> _userManager;
        public void Insert(UserApplication model, UserManager<UserApplication> userManager)
        {
            _userManager = userManager;

            var newUser = new UserApplication
            {
                Email = model.Email,
                UserName = model.UserName,
                FullName = model.FullName
            };
        }
        public void Delete(UserApplication user)
        {

        }

        public void Update(UserApplication user)
        {

        }

        public void Read(UserApplication user)
        {

        }
    }
}
