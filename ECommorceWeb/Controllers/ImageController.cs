using Business.Abstract;
using DataAcsess.Abstract;
using DataAcsess.Migrations;
using ECommorceWeb.Models;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Org.BouncyCastle.Asn1.Cms;
using static ECommorceWeb.Models.ProductListViewModel;

namespace ECommorceWeb.Controllers
{
    public class ImageController : Controller
    {

        IGenericDal<Product> _productService;
        IGenericDal<ProductImages> _productImagesService;
        IGenericDal<Category> _categoryService;
        IGenericDal<SubCategories> _subcategoryService;

        public ImageController(IGenericDal<Product> productService, IGenericDal<Category> categoryService, IGenericDal<SubCategories> subcategoryService, IGenericDal<ProductImages> productImagesService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _subcategoryService = subcategoryService;
            _productImagesService = productImagesService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddImage(int productId)
        {

            ViewBag.ProductId = productId;
            var result = _productService.GetList().ToList();


            // Ürünleri al
            List<Product> products = result;
            Dictionary<int, List<ImageViewModel>> productImages = new Dictionary<int, List<ImageViewModel>>();

            foreach (var product in products)
            {
                var imageResult = _productImagesService.GetList(x => x.productId == product.ProductId).ToList();
         
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
            // Modeli oluştur
            var viewModel = new ProductListViewModel
            {
                ProductImages = productImages
            };
            return View(viewModel);

        }



        [HttpPost]
        public IActionResult AddImage(IFormFile imageFile, int productId)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                using var ms = new MemoryStream();
                imageFile.CopyTo(ms);
                var imageData = ms.ToArray();

                var productImage = new ProductImages
                {
                    ImageData = imageData,
                    ContentType = imageFile.ContentType,   // <-- Burada null değil!
                    productId = productId,
                };

                _productImagesService.Add(productImage);
            }

            return RedirectToAction("AddImage", new { productId = productId });
        }
    }
}
