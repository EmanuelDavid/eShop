using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CatalogMicroS.Models;
using System.Net;
using CatalogMicroS.DL;

namespace CatalogMicroS.Controllers
{
    public class CatalogController : Controller
    {
        ICatalogRepository _catalogRepository;

        public CatalogController(ICatalogRepository repository)
        {
            _catalogRepository = repository;
        }

        [HttpGet]
        [Route("Catalog/Item/{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(CatalogItem), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetItemById(long id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var item = await _catalogRepository.GetItemById(id);

            if (item != null)
            {
                return Ok(item);
            }

            return NotFound();
        }

        [HttpGet]
        [Route("Catalog/AllItems")]
        [ProducesResponseType(typeof(CatalogItem), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllItems()
        {
            var item = await _catalogRepository.GetAllItems();
            return Ok(item);
        }

        //POST api/v1/[controller]/items
        [HttpPost]
        [Route("Catalog/Item")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateProduct([FromBody]CatalogItem model)
        {
            var itemId = await _catalogRepository.AddItem(model);

            return CreatedAtAction("CreateProduct", new { id = itemId }, null);
        }
    }
}
