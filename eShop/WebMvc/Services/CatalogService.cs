using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WebMvc.Models;

namespace WebMvc.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _httpClient;
        private const string BASE_URL = "https://localhost:44386";

        public CatalogService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CatalogItem>> GetCatalogItems()
        {
            var uri = $"{BASE_URL}/Catalog/AllItems";

            var responseString = await _httpClient.GetStringAsync(uri);

            var catalog = JsonConvert.DeserializeObject<List<CatalogItem>>(responseString);

            return catalog;
        }

        public async Task<string> Add()
        {
            var uri = BASE_URL + "Catalog/Item/1";

            var responseString = await _httpClient.GetStringAsync(uri);

            return responseString;
        }
    }
}
