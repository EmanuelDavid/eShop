
using CatalogMicroS.Models;
using System.Threading.Tasks;

namespace CatalogMicroS.DL
{
    public interface ICatalogRepository
    {
        Task<long> AddItem(CatalogItem model);
        Task<CatalogItem> GetItemById(long id);
    }
}
