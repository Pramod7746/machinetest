using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;
using machinetest.Models;
using machinetest.Services;

namespace machinetest.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }


        public async Task<ActionResult> Index(int page = 1)
        {
            int pageSize = 10; // Number of products per page
            var products = await _productService.GetPagedProductsAsync(page, pageSize);

            ViewBag.Page = page; // Current page
            ViewBag.PageSize = pageSize; // Page size for pagination
            ViewBag.TotalPages = await _productService.GetTotalPagesAsync(pageSize); // Total pages

            return View(products);
        }



        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var product = await _productService.GetProductByIdAsync(id.Value);
            if (product == null)
                return HttpNotFound();

            return View(product);
        }

        
        public async Task<ActionResult> Create()
        {
            var categories = await _productService.GetCategoriesAsync();
            ViewBag.CategoryId = new SelectList(categories, "CategoryId", "CategoryName");
            return View();
        }
        public async Task<ActionResult> ProductData()
        {
            var products = await _productService.GetAllProductsAsync();
            if (products == null || !products.Any())
                return HttpNotFound();

            // Return the list of products to the simplified view
            return View(products);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ProductName,CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                var isDuplicate = await _productService.IsProductNameDuplicateAsync(product.ProductName);
                if (isDuplicate)
                {
                    ModelState.AddModelError("ProductName", "A product with this name already exists.");
                    var categories1 = await _productService.GetCategoriesAsync();
                    ViewBag.CategoryId = new SelectList(categories1, "CategoryId", "CategoryName", product.CategoryId);
                    return View(product);
                }

                await _productService.CreateProductAsync(product);
                return RedirectToAction("Index");
            }

            var categories = await _productService.GetCategoriesAsync();
            ViewBag.CategoryId = new SelectList(categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }
      


        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var product = await _productService.GetProductByIdAsync(id.Value);
            if (product == null)
                return HttpNotFound();

            var categories = await _productService.GetCategoriesAsync();
            ViewBag.CategoryId = new SelectList(categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ProductId,ProductName,CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                await _productService.UpdateProductAsync(product);
                return RedirectToAction("Index");
            }

            var categories = await _productService.GetCategoriesAsync();
            ViewBag.CategoryId = new SelectList(categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var product = await _productService.GetProductByIdAsync(id.Value);
            if (product == null)
                return HttpNotFound();

            return View(product);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _productService.DeleteProductAsync(id);
            return RedirectToAction("Index");
        }
    }
}
