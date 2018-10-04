using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}
