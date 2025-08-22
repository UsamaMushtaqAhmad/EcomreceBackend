using Dapper;
using backend.Dapper;
using backend.DTOs;
using backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Services.Poroduct
{
    public class ProductService : IProductService
    {
        private readonly DapperContext _context;
        public ProductService(DapperContext context)
        {
            _context = context;
        }

        private string BaseSelect = @"
            SELECT 
                p.ProductId, 
                p.Name AS ProductName, 
                p.Description, 
                p.Price, 
                p.Stock, 
                p.ImageUrl, 
                p.IsHotSeller, 
                p.CreatedDate, 
                c.CategoryName
            FROM Products p
            INNER JOIN Categories c ON p.CategoryId = c.CategoryId
        ";

        public async Task<IEnumerable<ProductDto>> GetAllProducts()
        {
            using var conn = _context.CreateConnection();
            var query = BaseSelect + " ORDER BY p.ProductId DESC";
            return await conn.QueryAsync<ProductDto>(query);
        }

        public async Task<ProductDto?> GetProductById(int id)
        {
            using var conn = _context.CreateConnection();
            var query = BaseSelect + " WHERE p.ProductId = @Id";
            return await conn.QueryFirstOrDefaultAsync<ProductDto>(query, new { Id = id });
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByCategory(int categoryId)
        {
            using var conn = _context.CreateConnection();
            var query = BaseSelect + " WHERE p.CategoryId = @CategoryId ORDER BY p.ProductId DESC";
            return await conn.QueryAsync<ProductDto>(query, new { CategoryId = categoryId });
        }

        public async Task<IEnumerable<ProductDto>> GetHotSellers()
        {
            using var conn = _context.CreateConnection();
            var query = BaseSelect + " WHERE p.IsHotSeller = 1 ORDER BY p.CreatedDate DESC";
            return await conn.QueryAsync<ProductDto>(query);
        }

        public async Task<IEnumerable<ProductDto>> GetLatestProducts(int count = 10)
        {
            using var conn = _context.CreateConnection();
            var query = BaseSelect + " ORDER BY p.CreatedDate DESC OFFSET 0 ROWS FETCH NEXT @Count ROWS ONLY;";
            return await conn.QueryAsync<ProductDto>(query, new { Count = count });
        }

        public async Task<int> AddProduct(Product product)
        {
            using var conn = _context.CreateConnection();
            var sql = @"
                INSERT INTO Products (Name, Description, Price, Stock, ImageUrl, IsHotSeller, CreatedDate, CategoryId)
                VALUES (@Name, @Description, @Price, @Stock, @ImageUrl, @IsHotSeller, @CreatedDate, @CategoryId);
                SELECT CAST(SCOPE_IDENTITY() as int);
            ";
            var id = await conn.QuerySingleAsync<int>(sql, product);
            return id;
        }

        public async Task<int> UpdateProduct(Product product)
        {
            using var conn = _context.CreateConnection();
            var sql = @"
                UPDATE Products SET
                    Name = @Name,
                    Description = @Description,
                    Price = @Price,
                    Stock = @Stock,
                    ImageUrl = @ImageUrl,
                    IsHotSeller = @IsHotSeller,
                    CategoryId = @CategoryId
                WHERE ProductId = @ProductId
            ";
            return await conn.ExecuteAsync(sql, product);
        }

        public async Task<int> DeleteProduct(int id)
        {
            using var conn = _context.CreateConnection();
            var sql = "DELETE FROM Products WHERE ProductId = @Id";
            return await conn.ExecuteAsync(sql, new { Id = id });
        }
    }
}
