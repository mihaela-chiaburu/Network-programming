using HTTPClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTPClient.Interfaces
{
    public interface IProductService
    {
        Task<List<Product>> GetProductsAsync(int categoryId);
        Task CreateProductAsync(int categoryId, string name, decimal price);
    }
}
