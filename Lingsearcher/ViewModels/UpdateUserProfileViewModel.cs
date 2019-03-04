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
        [MaxLength(30)]
        public string PostalCode { get; set; }

        [Required]
        [Display(Name = "Rua")]
        [MaxLength(50)]
        public string Street { get; set; }

        [Required]
        [Display(Name = "Bairro")]
        [MaxLength(50)]
        public string Neighbourhood { get; set; }

        [Required]
        [Display(Name = "Cidade")]
        [MaxLength(50)]
        public string City { get; set; }

        [Required]
        [Display(Name = "Estado")]
        [MaxLength(2)]
        public string State { get; set; }

        [Required]
        [Display(Name = "País")]
        [MaxLength(50)]
        public string Country { get; set; }


    }
}