using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Lingsearcher.ViewModels
{
    public class UpdatePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Senha atual")]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Nova senha")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Repita a nova senha")]
        public string NewPasswordConfirmation { get; set; }
    }
}