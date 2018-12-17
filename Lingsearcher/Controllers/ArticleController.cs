using Lingsearcher.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lingsearcher.Controllers
{
    public class ArticleController : Controller
    {
            [HttpGet]
            [Authorize]
            public ActionResult ArticleCreate()
            {
                return View();
            }

            [HttpPost]
            [Authorize]
            public ActionResult ArticleCreate(ArticleCreateViewModel model)
            {
                return View();
            }
    }
}