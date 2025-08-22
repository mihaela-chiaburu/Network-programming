using HTTPClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTPClient.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> GetCategoriesAsync();
        Task<int> GetCategoryIdByNameAsync(string categoryName);
        Task CreateCategoryAsync(string title);
        Task EditCategoryAsync(int categoryId, string newTitle);
        Task DeleteCategoryAsync(int categoryId);
    }
}
