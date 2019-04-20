using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using Lingsearcher.DAL;
using Lingsearcher.Entity;
using Lingsearcher.ViewModels;

namespace Lingsearcher.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        [Authorize]
        public ActionResult Index()
        {
            BaseDAO<Product> productDAO = new BaseDAO<Product>();
            ViewBag.AllProducts = (List<Product>)productDAO.GetAll(); ;

            return View();
        }

        // GET: Product/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            List<Brand> brands = (List<Brand>)new BaseDAO<Brand>().GetAll();
            List<Category> categorys = (List<Category>)new BaseDAO<Category>().GetAll();
            List<Store> stores = (List<Store>)new BaseDAO<Store>().GetAll();

            var model = new CreateProductViewModel
            {
                BrandId = brands.First().Id,
                CategoryId = categorys.First().Id,

                Brands = brands.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }),

                Categorys = categorys.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                })
            };

            //Adicionar campo de Id do produto de acordo com a quantidade de lojas
            ViewBag.ProductStore = stores;

            return View(model);
        }

        // POST: Product/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(CreateProductViewModel model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var file = Request.Files["Image"];
                    string fname = String.Empty;
                    string fullPath = String.Empty;

                    //Checa se o arquivo foi enviado
                    if (file != null && file.ContentLength > 0)
                    {
                        fname = Path.GetFileName(file.FileName);
                        //fullPath = Server.MapPath(Path.Combine("~/App_Data/", fname));
                        //fullPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), fname);
                        fullPath = $@"{HttpContext.ApplicationInstance.Server.MapPath("~/App_Data")}/{fname}";
                        file.SaveAs(fullPath);
                    }

                    model.ImageSrc = fname;

                    //Inserir produto
                    var newProduct = new Product
                    {
                        Name = model.Name,
                        Description = model.Description,
                        CategoryId = model.CategoryId,
                        BrandId = model.BrandId,
                        ImageSrc = model.ImageSrc
                    };

                    Product product = new BaseDAO<Product>().Insert(newProduct);

                    //Varre o array de productStore e insere de acordo
                    //com o id do produto inserido e da loja

                    var stores = (List<Store>)new BaseDAO<Store>().GetAll();
                    BaseDAO<ProductStore> productStoreDAO = new BaseDAO<ProductStore>();

                    for(int i = 0; i < model.ProductStore.Length; i++)
                    {
                        var newProductStore = new ProductStore
                        {
                            ProductId = product.Id,
                            StoreId = stores[i].Id,
                            ProductStoreId = model.ProductStore[i]
                        };
                        productStoreDAO.Insert(newProductStore);
                    }
                }

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        // GET: Product/Edit/5
        [HttpGet]
        [Authorize]
        public ActionResult Edit(int id)
        {
            Product product = new BaseDAO<Product>().GetById(id);
            List<Category> categorys = (List<Category>)new BaseDAO<Category>().GetAll();
            List<Brand> brands = (List<Brand>)new BaseDAO<Brand>().GetAll();
            List<Store> stores = (List<Store>)new BaseDAO<Store>().GetAll();

            //Busca de lista de product Store
            List<ProductStore> listProductStore = (List<ProductStore>)new ProductStoreDAO().GetByProductId(id);

            //Criação da view model
            EditProductViewModel model = new EditProductViewModel
            {
                ProductId = product.Id,
                Name = product.Name,
                Description = product.Description,
                BrandId = product.BrandId,
                CategoryId = product.CategoryId,
                ImageSrc = product.ImageSrc,

                Brands = brands.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }),

                Categorys = categorys.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                })

            };

            int cont = 0;
            model.ProductStore = new string[listProductStore.Count()];

            //Setar os valores de ProductStore no array de string
            foreach (var item in listProductStore)
            {
                model.ProductStore[cont] = item.ProductStoreId;
                cont++;
            }

            ViewBag.ProductStore = stores;

            //Devolver para a view com os dados preenchidos
            return View(model);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [Authorize]
        public ActionResult Edit(EditProductViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var file = Request.Files["Image"];
                    string fname = String.Empty;
                    string fullPath = String.Empty;

                    //Checa se o arquivo foi enviado
                    if (file != null && file.ContentLength > 0)
                    {
                        //Caso exista alguma imagem atrelada, apaga antes de subir a nova
                        //System.IO.File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), model.ImageSrc));
                        System.IO.File.Delete($@"{HttpContext.ApplicationInstance.Server.MapPath("~/App_Data")}/{model.ImageSrc}");
                        fname = Path.GetFileName(file.FileName);
                        //fullPath = Server.MapPath(Path.Combine("~/App_Data/", fname));
                        //Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), fname);
                        fullPath = $@"{HttpContext.ApplicationInstance.Server.MapPath("~/App_Data")}/{fname}";
                        file.SaveAs(fullPath);
                        model.ImageSrc = fname;
                    }

                    //Atualiza produto
                    var newProduct = new Product
                    {
                        Id = model.ProductId,
                        Name = model.Name,
                        Description = model.Description,
                        CategoryId = model.CategoryId,
                        BrandId = model.BrandId,
                        ImageSrc = model.ImageSrc
                    };

                    new BaseDAO<Product>().Update(newProduct);

                    //Deleta todo o mapeamento de ProductId com ProductStore
                    var productStoreDao = new ProductStoreDAO().Delete(newProduct.Id);

                    //Varre o array de productStore e insere de acordo
                    //com o id do produto inserido e da loja

                    var stores = (List<Store>)new BaseDAO<Store>().GetAll();
                    BaseDAO<ProductStore> productStoreDAO = new BaseDAO<ProductStore>();

                    for (int i = 0; i < model.ProductStore.Length; i++)
                    {
                        var newProductStore = new ProductStore
                        {
                            ProductId = newProduct.Id,
                            StoreId = stores[i].Id,
                            ProductStoreId = model.ProductStore[i]
                        };
                        productStoreDAO.Insert(newProductStore);
                    }
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Product/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            var productDAO = new BaseDAO<Product>();
            Product product = productDAO.GetById(id);

            //Deletar imagem
            //System.IO.File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), product.ImageSrc));

            System.IO.File.Delete($@"{HttpContext.ApplicationInstance.Server.MapPath("~/App_Data")}/{product.ImageSrc}");

            //Deletar product store
            new BaseDAO<ProductStore>().Delete(id);

            //Deletar produto
            productDAO.Delete(id);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ListProductsCompare(string json)
        {
            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<SearchProductsViewModel>(json);
            return View(model);
        }

    }
}
