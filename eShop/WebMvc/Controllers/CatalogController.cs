using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebMvc.Models;
using WebMvc.Models.ViewModels;
using WebMvc.Services;

namespace WebMvc.Controllers
{
    public class CatalogController : Controller
    {
        private ICatalogService _catalogSvc;

        public CatalogController(ICatalogService catalogSeivice) =>
            _catalogSvc = catalogSeivice;

        [HttpGet]
        [Route("Catalog/GetProducts")]
        public async Task<IActionResult> GetProducts()
        {
            var items = await _catalogSvc.GetCatalogItems();

            var vm = new AllProducts
            {
                AllCatalogItems = items
            };

            return View(vm);
        }

        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(CatalogItem model)
        {
            if (ModelState.IsValid)
            {
                var result = await _catalogSvc.Add(model);
                if(result.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    return RedirectToAction("GetProducts");
                }
            }

            return View(model);
        }
    }
}
