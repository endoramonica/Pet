using ECommerceSystem.GUI.Apis;
using ECommerceSystem.GUI.Models;
using ECommerceSystem.Shared.DTOs.Category;
using ECommerceSystem.Shared.DTOs.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ECommerceSystem.GUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoryApi _categoryApi;
        private readonly IProductApi _productApi;

        public HomeController(ILogger<HomeController> logger, ICategoryApi categoryApi, IProductApi productApi)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _categoryApi = categoryApi ?? throw new ArgumentNullException(nameof(categoryApi));
            _productApi = productApi ?? throw new ArgumentNullException(nameof(productApi));
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            const int PageSize = 9;

            try
            {
                // Lấy danh sách danh mục
                var categories = await _categoryApi.GetAllAsync() ?? new List<CategoryDTO>();
                _logger.LogInformation("Fetched {Count} categories successfully.", categories.Count);

                // Lấy danh sách sản phẩm với phân trang
                var productResponse = await _productApi.GetProductsAsync(
                    search: null,
                    categoryId: null,
                    minPrice: null,
                    maxPrice: null,
                    sortBy: null,
                    promotion: null,
                    page: page,
                    pageSize: PageSize
                );

                if (productResponse == null || productResponse.Products == null)
                {
                    _logger.LogWarning("Product response or products list is null.");
                    return View(new List<ProductDTO>());
                }

              

                return View(productResponse.Products);
            }
            
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching data for Index page.");
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}