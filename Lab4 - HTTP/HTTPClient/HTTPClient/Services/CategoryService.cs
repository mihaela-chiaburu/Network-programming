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
    public class CategoryService : ICategoryService
    {
        private readonly IHttpService _httpService;

        public CategoryService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _httpService.GetAsync<List<Category>>(AppConstants.Endpoints.CATEGORIES);
        }

        public async Task<int> GetCategoryIdByNameAsync(string categoryName)
        {
            try
            {
                string encodedName = Uri.EscapeDataString(categoryName);
                string endpoint = $"{AppConstants.Endpoints.CATEGORIES_SEARCH}?categoryName={encodedName}";

                var response = await _httpService.GetAsync<string>(endpoint);
                if (int.TryParse(response, out int categoryId))
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

        public async Task CreateCategoryAsync(string title)
        {
            var request = new CreateCategoryRequest { Title = title };
            await _httpService.PostAsync(AppConstants.Endpoints.CATEGORIES, request);
        }

        public async Task EditCategoryAsync(int categoryId, string newTitle)
        {
            var request = new UpdateCategoryRequest { Title = newTitle };
            string endpoint = string.Format(AppConstants.Endpoints.CATEGORY_BY_ID, categoryId);
            await _httpService.PutAsync(endpoint, request);
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            string endpoint = string.Format(AppConstants.Endpoints.DELETE_CATEGORY, categoryId);
            await _httpService.DeleteAsync(endpoint);
        }
    }
}
