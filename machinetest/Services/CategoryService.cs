using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using machinetest.Models;

namespace machinetest.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly machinetestEntities _dbContext;

        public CategoryService(machinetestEntities dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Category>> GetPagedCategorysAsync(int page, int pageSize)
        {
            return await _dbContext.Categories
                .Include(p => p.Products) // Include related data if necessary
                .OrderBy(p => p.CategoryID) // Sort by a stable column
                .Skip((page - 1) * pageSize) // Skip records for previous pages
                .Take(pageSize) // Take the current page records
                .ToListAsync();
        }

        public async Task<int> GetTotalPagesAsync(int pageSize)
        {
            int totalCount = await _dbContext.Categories.CountAsync();
            return (int)Math.Ceiling((double)totalCount / pageSize);
        }
        public async Task<bool> IsCategoryNameDuplicateAsync(string categoryName)
        {
            return await _dbContext.Categories.AnyAsync(c => c.CategoryName == categoryName);
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _dbContext.Categories.ToListAsync();
        }
        public async Task<IEnumerable<Category>> GetAllProductsAsync()
        {
            return await _dbContext.Categories
                .Include(p => p.Products) // Include related Category data
                .ToListAsync();
        }
        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _dbContext.Categories.FindAsync(id);
        }

        public async Task AddCategoryAsync(Category category)
        {
            _dbContext.Categories.Add(category);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            _dbContext.Entry(category).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await GetCategoryByIdAsync(id);
            if (category != null)
            {
                _dbContext.Categories.Remove(category);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
