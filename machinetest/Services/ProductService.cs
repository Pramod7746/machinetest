using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using machinetest.Models;

public class ProductService : IProductService
{
    private readonly machinetestEntities _dbContext;

    public ProductService(machinetestEntities dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Product>> GetPagedProductsAsync(int page, int pageSize)
    {
        return await _dbContext.Products
            .Include(p => p.Category) // Include related data if necessary
            .OrderBy(p => p.ProductId) // Sort by a stable column
            .Skip((page - 1) * pageSize) // Skip records for previous pages
            .Take(pageSize) // Take the current page records
            .ToListAsync();
    }

    public async Task<bool> IsProductNameDuplicateAsync(string productName)
    {
        return await _dbContext.Products.AnyAsync(c => c.ProductName == productName);
    }
    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _dbContext.Products
            .Include(p => p.Category) // Include related Category data
            .ToListAsync();
    }


    public async Task<int> GetTotalPagesAsync(int pageSize)
    {
        int totalCount = await _dbContext.Products.CountAsync();
        return (int)Math.Ceiling((double)totalCount / pageSize);
    }


    public async Task<Product> GetProductByIdAsync(int id)
    {
        return await _dbContext.Products.FindAsync(id);

    }

    public async Task<IEnumerable<Category>> GetCategoriesAsync()
    {
        return await _dbContext.Categories.ToListAsync();
    }

    public async Task CreateProductAsync(Product product)
    {
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateProductAsync(Product product)
    {
        _dbContext.Entry(product).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteProductAsync(int id)
    {
        var product = await _dbContext.Products.FindAsync(id);
        if (product != null)
        {
            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();
        }
    }
}
