using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WebMvc.Models;

namespace WebMvc.Services
{
    public interface ICatalogService
    {
        Task<List<CatalogItem>> GetCatalogItems();
        Task<HttpResponseMessage> Add(CatalogItem model);
    }
}
