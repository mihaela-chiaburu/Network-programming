using HTTPClient.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HTTPClient.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://localhost:44370/api/Category/";

        public ApiService()
        {
            _httpClient = new HttpClient();
        }

        // GET /api/Category/categories
        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await GetAsync<List<Category>>("categories");
        }

        // GET /api/Category/categories/search?categoryName={name}
        public async Task<int> GetCategoryIdByNameAsync(string categoryName)
        {
            try
            {
                string encodedName = Uri.EscapeDataString(categoryName);
                string endpoint = $"categories/search?categoryName={encodedName}";

                var response = await _httpClient.GetAsync(BaseUrl + endpoint);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                if (int.TryParse(responseBody, out int categoryId))
                {
                    return categoryId;
                }
                return -1;
            }
            catch
            {
                return -1;
            }
        }

        // GET /api/Category/categories/{id}/products
        public async Task<List<Product>> GetProductsAsync(int categoryId)
        {
            return await GetAsync<List<Product>>($"categories/{categoryId}/products");
        }

        // POST /api/Category/categories
        public async Task CreateCategoryAsync(string title)
        {
            var newCategory = new { Title = title };
            await PostAsync("categories", newCategory);
        }

        // PUT /api/Category/{id}
        public async Task EditCategoryAsync(int categoryId, string newTitle)
        {
            var updatedCategory = new { Title = newTitle };
            await PutAsync($"{categoryId}", updatedCategory);
        }

        // DELETE /api/Category/categories/{id}
        public async Task DeleteCategoryAsync(int categoryId)
        {
            await DeleteAsync($"categories/{categoryId}");
        }

        // POST /api/Category/categories/{id}/products
        public async Task CreateProductAsync(int categoryId, string name, decimal price)
        {
            var newProduct = new { title = name, price = price };
            await PostAsync($"categories/{categoryId}/products", newProduct);
        }

        // Metode helper pentru request-uri HTTP
        private async Task<T> GetAsync<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync(BaseUrl + endpoint);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        private async Task PostAsync(string endpoint, object data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(BaseUrl + endpoint, content);
            response.EnsureSuccessStatusCode();
        }

        private async Task PutAsync(string endpoint, object data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(BaseUrl + endpoint, content);
            response.EnsureSuccessStatusCode();
        }

        private async Task DeleteAsync(string endpoint)
        {
            var response = await _httpClient.DeleteAsync(BaseUrl + endpoint);
            response.EnsureSuccessStatusCode();
        }
    }
}