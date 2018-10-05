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
            var url = $"{BASE_URL}/Catalog/AllItems";

            var responseString = await _httpClient.GetStringAsync(url);

            var catalog = JsonConvert.DeserializeObject<List<CatalogItem>>(responseString);

            return catalog;
        }

        public async Task<HttpResponseMessage> Add(CatalogItem model)
        {
            var url = $"{BASE_URL}/Catalog/Item";

            var content = new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);

            return response;
        }
    }
}
