using System.Collections.Generic;
using System.Threading.Tasks;
using machinetest.Models;

namespace machinetest.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetPagedCategorysAsync(int page, int pageSize);


            Task<int> GetTotalPagesAsync(int pageSize);
        Task<IEnumerable<Category>> GetAllProductsAsync();
        Task<bool> IsCategoryNameDuplicateAsync(string categoryName);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(int id);
        Task AddCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(int id);
    }
}
