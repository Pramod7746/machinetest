using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;
using machinetest.Models;
using machinetest.Services;

namespace machinetest.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<ActionResult> CategoryData()
        {
            var categories = await _categoryService.GetAllProductsAsync();
            if (categories == null || !categories.Any())
                return HttpNotFound();

            // Return the list of products to the simplified view
            return View(categories);
        }
        public async Task<ActionResult> Index(int page = 1)
        {
            int pageSize = 10; // Number of products per page
            var categories = await _categoryService.GetPagedCategorysAsync(page, pageSize);

            ViewBag.Page = page; // Current page
            ViewBag.PageSize = pageSize; // Page size for pagination
            ViewBag.TotalPages = await _categoryService.GetTotalPagesAsync(pageSize); // Total pages

            return View(categories);
        }
       

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var category = await _categoryService.GetCategoryByIdAsync(id.Value);
            if (category == null)
                return HttpNotFound();

            return View(category);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                // Check if the category name is duplicate
                var isDuplicate = await _categoryService.IsCategoryNameDuplicateAsync(category.CategoryName);
                if (isDuplicate)
                {
                    ModelState.AddModelError("CategoryName", "A category with this name already exists.");
                    return View(category);
                }

                // Add the category if it's not a duplicate
                await _categoryService.AddCategoryAsync(category);
                return RedirectToAction("Index");
            }

            return View(category);
        }


        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var category = await _categoryService.GetCategoryByIdAsync(id.Value);
            if (category == null)
                return HttpNotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                await _categoryService.UpdateCategoryAsync(category);
                return RedirectToAction("Index");
            }

            return View(category);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var category = await _categoryService.GetCategoryByIdAsync(id.Value);
            if (category == null)
                return HttpNotFound();

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return RedirectToAction("Index");
        }
    }
}
