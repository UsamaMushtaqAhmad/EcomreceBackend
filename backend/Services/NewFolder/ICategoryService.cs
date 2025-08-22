using backend.DTOs;

namespace backend.Services.NewFolder
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategories();
        Task<CategoryDto> GetCategoryById(int id);
        Task<int> AddCategory(CategoryDto category);
        Task<int> UpdateCategory(CategoryDto category);
        Task<int> DeleteCategory(int id);
    }
}
