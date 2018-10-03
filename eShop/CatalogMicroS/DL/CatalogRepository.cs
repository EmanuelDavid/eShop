using CatalogMicroS.DL.Entities;
using CatalogMicroS.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CatalogMicroS.DL
{
    public class CatalogRepository : ICatalogRepository
    {
        private readonly CatalogContext _catalogContext;

        public CatalogRepository(CatalogContext context)
        {
            _catalogContext = context;
        }

        public async Task<long> AddItem(CatalogItem model)
        {
            var item = new CatalogItemEntity
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price
            };
            _catalogContext.CatalogItems.Add(item);

            await _catalogContext.SaveChangesAsync();

            return item.Id;
        }


        public async Task<CatalogItem> GetItemById(long id)
        {
            //TODO: Check wht sql does this generates
            var entity = await _catalogContext.CatalogItems.SingleOrDefaultAsync(ci => ci.Id == id);

            return BuildItem(entity);
        }

        private CatalogItem BuildItem(CatalogItemEntity entity)
        {
            return new CatalogItem {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Price = entity.Price,
                AvailableStock = entity.AvailableStock
            };

        }

    }
}
