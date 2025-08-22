using backend.DTOs;
using backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Services.Poroduct
{
    public interface IProductService
    {

        Task<IEnumerable<ProductDto>> GetAllProducts();
        Task<ProductDto?> GetProductById(int id);
        Task<IEnumerable<ProductDto>> GetProductsByCategory(int categoryId);
        Task<IEnumerable<ProductDto>> GetHotSellers();
        Task<IEnumerable<ProductDto>> GetLatestProducts(int count = 10);
        Task<int> AddProduct(Product product);
        Task<int> UpdateProduct(Product product);
        Task<int> DeleteProduct(int id);
    }
}
