using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Lingsearcher.ViewModels
{
    public class AccountLoginViewModel
    {
        [Required]
        //[EmailAddress]
        [DisplayName("Username or Email")]
        public string UserNameOrEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Keep Logged In?")]
        public bool KeepLoggedIn { get; set; }
    }
}