using HTTPClient.Constants;
using HTTPClient.Interfaces;
using HTTPClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTPClient.Services
{
    public class ProductService : IProductService
    {
        private readonly IHttpService _httpService;

        public ProductService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<List<Product>> GetProductsAsync(int categoryId)
        {
            string endpoint = string.Format(AppConstants.Endpoints.CATEGORY_PRODUCTS, categoryId);
            return await _httpService.GetAsync<List<Product>>(endpoint);
        }

        public async Task CreateProductAsync(int categoryId, string name, decimal price)
        {
            var request = new CreateProductRequest { title = name, price = price };
            string endpoint = string.Format(AppConstants.Endpoints.CATEGORY_PRODUCTS, categoryId);
            await _httpService.PostAsync(endpoint, request);
        }
    }
}
