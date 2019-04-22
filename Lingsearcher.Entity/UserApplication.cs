using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Lingsearcher.Entity
{
    public class UserApplication : IdentityUser
    {
        public string FullName { get; set; }
        public string Password { get; set; }
    }
}
