using DataAcsess.Abstract;
using ECommorceWeb.Models;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static ECommorceWeb.Models.ProductListViewModel;

namespace ECommorceWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        IGenericDal<Category> _categoryService;
        IGenericDal<Product> _productService;
        IGenericDal<ProductImages> _productImgService;

        public HomeController(ILogger<HomeController> logger,IGenericDal<ProductImages> productImgService, IGenericDal<Category> category, IGenericDal<Product> productService)
        {
            _logger = logger;
            _categoryService = category;
            _productService = productService;
            _productImgService = productImgService;
        }


        Dictionary<int, List<ImageViewModel>> productImages = new Dictionary<int, List<ImageViewModel>>();

        public IActionResult Index()
        {
            var categories = _categoryService.GetList().ToList();
            var last3men = GetLast3Product ("Mens");
            var last3Women = GetLast3Product ("Womens");
            var last3Kid = GetLast3Product ("Kids");
     

            List<Product> products = _productService.GetList().ToList();

            Dictionary<int, List<ImageViewModel>> productImages = new Dictionary<int, List<ImageViewModel>>();

            foreach (var product in products)
            {
                var imageResult = _productImgService.GetList(x => x.productId == product.ProductId).ToList();

                if (imageResult != null && imageResult.Any())
                {
                    productImages[product.ProductId] = imageResult.Select(img => new ImageViewModel
                    {
                        pImgId = img.pImgId,
                        ImageBase64 = $"data:{img.ContentType};base64,{Convert.ToBase64String(img.ImageData)}"
                    }).ToList();
                }
                else
                {
                    productImages[product.ProductId] = new List<ImageViewModel>();
                }
            }



            var model = new ProductListViewModel
            {
                Categories = categories,
                Products = products,
                Last3MenProduct = last3men,
                Last3KidProduct = last3Kid,
                Last3WomenProduct = last3Women,
                ProductImages = productImages
            };


            return View(model);
        }



        public List<Product> GetLast3Product(string? filter = null)
        {
            var allProducts = _productService.GetList().ToList();
            var allCategories = _categoryService.GetList().ToList();

            if (!string.IsNullOrEmpty(filter))
            {
                string lowerFilter = filter.Trim().ToLower();

                Console.WriteLine($"Filter: '{filter}'");
                foreach (var c in allCategories)
                {
                    Console.WriteLine($"Kategori: '{c.Name}'");
                }

                var matchedCategoryIds = allCategories
                    .Where(c => !string.IsNullOrEmpty(c.Name) &&
                                c.Name.Trim().ToLower() == lowerFilter)
                    .Select(c => c.CategoryId)
                    .ToList();

                Console.WriteLine("Eşleşen Kategori ID'leri:");
                foreach (var id in matchedCategoryIds)
                {
                    Console.WriteLine(id);
                }

                var filteredProducts = allProducts
                    .Where(p => matchedCategoryIds.Contains(p.CategoryId))
                    .OrderByDescending(p => p.ProductId)
                    .Take(3)
                    .ToList();

                Console.WriteLine($"Filtrelenen ürün sayısı: {filteredProducts.Count}");

                return filteredProducts;
            }


            return new List<Product>();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult AboutUs() 
        {

            return View();
        }
        public IActionResult ContactUs()
        {

            return View();
        }
    }
}