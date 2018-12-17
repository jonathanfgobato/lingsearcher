using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Lingsearcher.ViewModels
{
    public class UpdateUserProfileViewModel
    {
        [Required]
        [Display(Name = "Nome Completo")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Editable(false)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "CEP")]
        public string PostalCode { get; set; }

        [Required]
        [Display(Name = "Rua")]
        public string Street { get; set; }

        [Required]
        [Display(Name = "Bairro")]
        public string Neighbourhood { get; set; }

        [Required]
        [Display(Name = "Cidade")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public string State { get; set; }


        [Required]
        [Display(Name = "País")]
        public string Country { get; set; }


    }
}