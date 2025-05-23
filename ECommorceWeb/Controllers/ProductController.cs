using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAcsess.Abstract;
using DataAcsess.Concrete.EntityFramework.Contexts;
using ECommorceWeb.Models;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;
using System.Diagnostics;
using System.Security.Claims;
using System.Xml.Linq;
using System.Linq;
using ServiceStack;
using static ECommorceWeb.Models.ProductListViewModel;

namespace ECommorceWeb.Controllers
{

    public class ProductController : Controller
    {
        IGenericDal<Product> _productService;
        IGenericDal<ProductOption> _potService;
        IGenericDal<ProductReview> _prService;
        IGenericDal<ProductImages> _productImagesService;
        IGenericDal<Category> _categoryService;
        IGenericDal<SubCategories> _subcategoryService;
        private readonly IGenericDal<Coupon> _couponService; // Ürün hizmetlerini yöneten servis
        private readonly IGenericDal<CouponProduct> _couponproductService; // Ürün hizmetlerini yöneten servis
        private readonly IGenericDal<CouponUser> _couponuserService; // Ürün hizmetlerini yöneten servis
        // Gerekli bağımlılıkları ekleyin
        private readonly IWebHostEnvironment _env;

        public ProductController(IGenericDal<Coupon> couponService, IGenericDal<CouponProduct> couponproductService, IGenericDal<CouponUser> couponuserService, IGenericDal<Product> productService, IGenericDal<ProductReview> prService, IGenericDal<Category> categoryService, IGenericDal<SubCategories> subcategoryService, IGenericDal<ProductImages> productImagesService, IWebHostEnvironment env, IGenericDal<ProductOption> potService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _subcategoryService = subcategoryService;
            _productImagesService = productImagesService;
            _prService = prService;
            _env = env;
            _potService = potService;
            _couponService = couponService;
            _couponproductService = couponproductService;
            _couponuserService = couponuserService;
        }

        public IActionResult Index()
        {
            GetCategory();
            GetSubCategory();
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



            var couponProduct = _couponproductService.GetList().ToList();
            // Sadece belirli ürünler için istiyorsan filtre ekleyebilirsin. Aksi halde hepsini alır.
            var couponIds = couponProduct
                .Select(x => x.CouponId)
                .Distinct()
                .ToList();

            var coupons = _couponService
                .GetList(c => couponIds.Contains(c.CouponId))
                .ToList();
            // Modeli oluştur
            var viewModel = new ProductListViewModel
            {
                Products = products,
                ProductImages = productImages,
                couponProducts = couponProduct,
                coupons = coupons

            };

            if (result != null)
            {
                // Modeli View'a gönder

                return View(viewModel);
            }
            // Hata durumunu işleyin
            return BadRequest("Hata : ");

        }


        [Authorize(Roles = "Admin")]//claim olarak eklediğimiz isadmin değerine göre ve iki seçenek kullanmak istiyorsak araya virgül atarak 
        public async Task<IActionResult> AdminProduct()
        {
            var result = _productService.GetList().ToList();
            List<Product> products = result;

            // Kategoriler ve alt kategoriler için adlar
            getCategoryNames(products);
            getSubCategoryNames(products);

            // Dictionary'i önce tanımlayın
            Dictionary<int, List<ImageViewModel>> productImages = new Dictionary<int, List<ImageViewModel>>();
            var productStocks = new Dictionary<int, int>(); // Ürün ID'sine göre toplam stoğu tutacak sözlük



            foreach (var product in products)
            {
                var productOptions = _potService.GetList(p => p.ProductId == product.ProductId);
                var totalStock = productOptions.Sum(p => (int?)p.StockQuantity) ?? 0;

                var imageResult = _productImagesService.GetList(x => x.productId == product.ProductId).ToList();
                if (imageResult != null)
                {
                    productImages[product.ProductId] = imageResult.Select(img => new ImageViewModel
                    {
                        pImgId = img.pImgId,
                        ImageBase64 = $"data:{img.ContentType};base64,{Convert.ToBase64String(img.ImageData)}"
                    }).ToList();
                }

                productStocks[product.ProductId] = totalStock;

            }

            getRating (products);


            // ViewBag ile tüm ürünlerin toplam stoğunu gönder
            ViewBag.ProductStocks = productStocks;

            // ViewModel'e ekleyin
            var viewModel = new ProductListViewModel
            {
                Products = products,
                ProductImages = productImages
            };

            if (result != null)
            {
                return View(viewModel);
            }
            return BadRequest("Hata : ");


        }


        // ViewModel'i oluşturuyoruz
        private void getCategoryNames(List<Product> products)
        {
            // Sözlüğü burada başlatıyoruz
            Dictionary<int, string> categoryNames = new Dictionary<int, string>();

            // Ürünlerin kategorilerini işleyelim
            foreach (var product in products)
            {
                // Kategori adını al
                var categoryName = GetCategoryWithId(product.CategoryId);

                // Eğer sözlükte yoksa ekleyelim
                if (!categoryNames.ContainsKey(product.CategoryId))
                {
                    categoryNames[product.CategoryId] = categoryName;

                }
            }
            ViewBag.CategoryNames = categoryNames;

        }
        private void getSubCategoryNames(List<Product> products)
        {
            // Sözlüğü burada başlatıyoruz
            Dictionary<int, string> subcategoryNames = new Dictionary<int, string>();

            // Ürünlerin kategorilerini işleyelim
            foreach (var product in products)
            {
                // Kategori adını al
                var subcategoryName = GetSubCategoryWithId(product.SubCategoryId);

                // Eğer sözlükte yoksa ekleyelim
                if (!subcategoryNames.ContainsKey(product.SubCategoryId))
                {
                    subcategoryNames[product.SubCategoryId] = subcategoryName;
                }
            }

            // ViewBag'e doğru sözlüğü atıyoruz
            ViewBag.subCategoryNames = subcategoryNames;

        }

        private string GetCategoryWithId(int id)
        {
            var categoryResult = _categoryService.GetList(x => x.CategoryId == id).SingleOrDefault(); // ID'ye göre kategori al
            if (categoryResult != null)
            {
                return categoryResult.Name; // Kategori adını döndür
            }
            return "Bilinmeyen Kategori"; // Kategori bulunamazsa
        }

        private string GetSubCategoryWithId(int SubCategoryId)
        {
            var categoryResult = _subcategoryService.GetList(x => x.SubCategoryId == SubCategoryId).SingleOrDefault(); // ID'ye göre kategori al

            if (categoryResult != null)
            {
                return categoryResult.SCName; // Kategori adını döndür
            }

            return "Bilinmeyen Kategori"; // Kategori bulunamazsa
        }



        public void getRating(List<Product> products)
        {

            var productRatings = new Dictionary<int, double>();

            foreach (var product in products)
            {
                // Ürün işlemleri...
                var productReview = _prService.GetList(x => x.ProductId == product.ProductId).ToList();
                double rating = 0;

                if (productReview.Any())
                {
                    rating = productReview.Average(x => x.Rating); // Nullable değilse böyle
                }

                productRatings[product.ProductId] = rating;
            }

            // 🔽 Tüm ürünler işlendi, şimdi ViewBag'e bir kere yaz
            ViewBag.ProductRatings = productRatings;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AddProduct()
        {
            GetCategory();
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult AddProduct(Product product, IFormFile ImageFile)
        {

            try
            {
                _productService.Add(product);

                var subcategories = getSubcategories(product.CategoryId);

                TempData["Message"] = "Başarıyla Eklendi";
                return RedirectToAction("AdminProduct", "Product");
            }
            catch (Exception ex)
            {
                return BadRequest("Hata : " + ex.ToString());
            }


        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult DeleteProduct(int productId)
        {
            // Ürünü veritabanından al
            var getProduct = _productService.GetList(x => x.ProductId == productId).SingleOrDefault();

            if (getProduct == null)
            {
                TempData["ErrorMessage"] = "Ürün bulunamadı.";
                return RedirectToAction("AdminProduct", "Product");
            }

            // Ürünü silme işlemi
            _productService.Delete(getProduct);

            try
            {
                TempData["Message"] = "Ürün başarıyla silindi.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ürün silinemedi: " + ex.Message;
            }

            return RedirectToAction("AdminProduct", "Product");
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult UpdateProduct(int productId)
        {
            GetCategory();
            var products = _productService.GetList().ToList();
            var result = _productService.Get(x => x.ProductId == productId); // SuccessDataResult<Product> nesnesini alıyoruz
            var product = result; // Product nesnesini alıyoruz
            getSubcategories(product.CategoryId);
            // Dictionary'i önce tanımlayın
            Dictionary<int, List<ImageViewModel>> productImages = new Dictionary<int, List<ImageViewModel>>();

            // Her ürün için resim verilerini al
            foreach (var productt in products)
            {
                var imageResult = _productImagesService.GetList(x => x.productId == productt.ProductId).ToList();

                if (imageResult != null && imageResult.Any())
                {
                    productImages[productt.ProductId] = imageResult.Select(img => new ImageViewModel
                    {
                        pImgId = img.pImgId,
                        ImageBase64 = $"data:{img.ContentType};base64,{Convert.ToBase64String(img.ImageData)}"
                    }).ToList();
                }
            }


            // Modeli oluştur
            var viewModel = new ProductListViewModel
            {
                Product = product,
                ProductImages = productImages
            };



            return View(viewModel);

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult UpdateProduct(Product product)
        {
            _productService.Update(product);

            return RedirectToAction("AdminProduct", "Product");
        }
        public JsonResult getSubcategories(int categoryId)
        {
            // _subcategoryService üzerinden belirtilen CategoryId'ye ait tüm alt kategorileri getiriyoruz.
            var getSubCategories = _subcategoryService.GetList(x => x.ParentCategoryId == categoryId);

            // Eğer getSubCategories listesi null değilse ve en az bir öğe içeriyorsa devam ediyoruz.
            if (getSubCategories != null && getSubCategories.Any())
            {
                // Alt kategori listesini frontend için uygun bir formata dönüştürüyoruz.
                var formattedSubCategories = getSubCategories.Select(x => new
                {
                    value = x.SubCategoryId, // Dropdown veya seçim kutusunda kullanılacak değer (ID).
                    text = x.SCName          // Kullanıcıya gösterilecek alt kategori adı.
                }).ToList();

                // JSON formatında dönüştürülmüş alt kategorileri döndürüyoruz.
                return Json(formattedSubCategories);
            }

            // Eğer alt kategori bulunamazsa, JSON olarak null döndürerek hata almamızı engelliyoruz.
            return Json(null);
        }

        //Methods
        void GetCategory()
        {
            var categories = _categoryService.GetList();

            // Eğer kategoriler null veya boşsa uyarı ver
            if (categories == null || !categories.Any())
            {
                Console.WriteLine("Kategori listesi boş veya null!");
            }

            List<SelectListItem> categoryValue = categories.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.CategoryId.ToString()
            }).ToList();

            // ViewBag'e atama yap
            ViewBag.cv = categoryValue;

            // Log at (böylece dolup dolmadığını görebilirsin)
            foreach (var item in categoryValue)
            {
                Console.WriteLine($"Kategori: {item.Text} - ID: {item.Value}");
            }
        }



        void GetSubCategory()
        {
            // Kategorileri al
            var Subcategories = _subcategoryService.GetList();

            // Kategori listesini SelectListItem listesine dönüştür
            List<SelectListItem> categoryValue = Subcategories.Select(c => new SelectListItem
            {
                Text = c.SCName, // Kategori adı
                Value = c.SubCategoryId.ToString() // Kategori ID'si
            }).ToList();

            // ViewBag'e atama yap
            ViewBag.subCv = categoryValue;
        }



        [HttpGet]
        public IActionResult ProductDetail(int productId)
        {
            try
            {
                var userIdString = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdString, out int userId))
                {
                    return Unauthorized("Kullanıcı bulunamadı.");
                }
                var cp = _couponproductService.Get(x => x.ProductId == productId && x.isUsed == false);

                List<Product> products = _productService.GetList().ToList() ;

                getRating(products);

                Coupon coupon = null;
                if (cp != null)
                {
                    ViewBag.DiscountForProduct = _couponService.Get(x => x.CouponId == cp.CouponId);
                    ViewBag.DiscountF = _couponService.Get(x => x.CouponId == cp.CouponId).Discount;
                    coupon = _couponService.Get(x => x.CouponId == cp.CouponId);
                }
                var cu = _couponuserService.Get(x => x.UserId == userId && x.isUsed == false);

                if (cu != null)
                {
                    ViewBag.DiscountForUser = _couponService.Get(x => x.CouponId == cu.CouponId);
                    ViewBag.DiscountU = _couponService.Get(x => x.CouponId == cu.CouponId).Discount;
                    ViewBag.DiscountForCode = _couponService.Get(x => x.CouponId == cu.CouponId).Code;
                    coupon = _couponService.Get(x => x.CouponId == cu.CouponId);
                }

                var getPot = _potService.GetList(x => x.ProductId == productId).ToList();
                // Ürün bilgisini alıyoruz
                var productResult = _productService.GetList(x => x.ProductId == productId).SingleOrDefault();

             

                if (productResult == null)
                {
                    return NotFound("Ürün bulunamadı.");
                }

                var product = productResult; // Product nesnesini alıyoruz

                // Resimleri almak için sözlük tanımlanıyor
                Dictionary<int, List<ImageViewModel>> productImages = new Dictionary<int, List<ImageViewModel>>();

                // Resimleri almak için servis çağrısı yapılıyor
                var imageResult = _productImagesService.GetList(x => x.productId == product.ProductId).ToList();

                if (imageResult != null && imageResult.Any())
                {
                    // Resim URL'lerini sözlüğe ekliyoruz
                    productImages[product.ProductId] = imageResult.Select(img => new ImageViewModel
                    {
                        pImgId = img.pImgId,
                        ImageBase64 = $"data:{img.ContentType};base64,{Convert.ToBase64String(img.ImageData)}"
                    }).ToList();
                }
                else
                {
                    // Resim yoksa, varsayılan bir resim eklenebilir (isteğe bağlı)
                    productImages[product.ProductId] = new List<ImageViewModel>
{
    new ImageViewModel
    {
        pImgId = 0,  // ID'yi 0 veya uygun varsayılanla koyabilirsin, otomatik artan veritabanı ID'si değil bu, sadece burada gösterim için.
        ImageBase64 = "/images/default.jpg"
    }
};
                }

                // ViewModel'i oluşturuyoruz
                var viewModel = new ProductListViewModel
                {
                    Product = product,
                    ProductImages = productImages,
                    ProductOptions = getPot,
                    couponProduct = cp,
                    couponUser = cu,
                    coupon = coupon

                };
                // Modeli View'a gönderiyoruz
                return View(viewModel);
            }
            catch (Exception ex)
            {
                // Hata durumunda, kullanıcıya anlaşılır bir mesaj veriyoruz
                return BadRequest("Hata: " + ex.Message);
            }
        }
    }
}
