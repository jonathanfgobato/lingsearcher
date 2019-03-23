using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lingsearcher.ViewModels
{
    public class EditProductViewModel
    {
        public int ProductId { get; set; }
        [Required]
        [Display(Name = "Nome do produto")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Descrição")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Marca")]
        public int BrandId { get; set; }
        //[Required]
        public IEnumerable<SelectListItem> Brands { get; set; }
        [Required]
        [Display(Name = "Categoria")]
        public int CategoryId { get; set; }
        //[Required]
        public IEnumerable<SelectListItem> Categorys { get; set; }
        [Required]
        public string[] ProductStore { get; set; }
        public string ImageSrc { get; set; }
        public HttpPostedFileBase Image { get; set; }
    }
}