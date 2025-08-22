using Dapper;
using backend.Dapper;
using backend.DTOs;


namespace backend.Services.NewFolder
{
    public class CategoryService : ICategoryService
    {
        private readonly DapperContext _context;

        public CategoryService(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategories()
        {
            var query = "SELECT CategoryId, CategoryName FROM Categories";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<CategoryDto>(query);
            }
        }

        public async Task<CategoryDto?> GetCategoryById(int id)
        {
            var query = "SELECT CategoryId, CategoryName FROM Categories WHERE CategoryId = @Id";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<CategoryDto>(query, new { Id = id });
            }
        }

        public async Task<int> AddCategory(CategoryDto category)
        {
            var query = "INSERT INTO Categories (CategoryName) VALUES (@CategoryName)";
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(query, category);
            }
        }

        public async Task<int> UpdateCategory(CategoryDto category)
        {
            var query = "UPDATE Categories SET CategoryName = @CategoryName WHERE CategoryId = @CategoryId";
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(query, category);
            }
        }

        public async Task<int> DeleteCategory(int id)
        {
            var query = "DELETE FROM Categories WHERE CategoryId = @Id";
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(query, new { Id = id });
            }
        }
    }
}