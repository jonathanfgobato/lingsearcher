using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lingsearcher.ViewModels
{
    public class CreateProductViewModel
    {
        [Required]
        [Display(Name = "Nome do produto")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Descrição")]
        public string Description { get; set; }
        [Display(Name = "Marca")]
        public int BrandId { get; set; }
        [Required]
        public IEnumerable<SelectListItem> Brands { get; set; }
        [Display(Name = "Categoria")]
        public int CategoryId { get; set; }
        [Required]
        public IEnumerable<SelectListItem> Categorys { get; set; }

    }
}