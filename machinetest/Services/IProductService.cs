using System.Collections.Generic;
using System.Threading.Tasks;
using machinetest.Models;

public interface IProductService
{
    Task<bool> IsProductNameDuplicateAsync(string productName);
    Task<IEnumerable<Product>> GetAllProductsAsync();

    Task<IEnumerable<Product>> GetPagedProductsAsync(int page, int pageSize);
    Task<int> GetTotalPagesAsync(int pageSize);
    Task<Product> GetProductByIdAsync(int id);
    Task<IEnumerable<Category>> GetCategoriesAsync();
    Task CreateProductAsync(Product product);
    Task UpdateProductAsync(Product product);
    Task DeleteProductAsync(int id);
}
