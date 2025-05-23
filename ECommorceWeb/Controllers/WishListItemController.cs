using Business.Abstract;
using DataAcsess.Abstract;
using ECommorceWeb.Models;
using ECommorceWeb.ViewComponents;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static ECommorceWeb.Models.ProductListViewModel;

namespace ECommorceWeb.Controllers
{

    public class WishListItemController : Controller
    {
        private readonly IGenericDal<WishlistItem> _wishListService;
        private readonly IGenericDal<Entities.Concrete.ApUser> _userService;
        private readonly IGenericDal<Product> _productService;
        private readonly IGenericDal<ProductReview> _prService;
        private readonly IGenericDal<ProductImages> _productImgService;

        public WishListItemController(IGenericDal<ProductReview> prService ,IGenericDal<ProductImages> productImgService, IGenericDal<WishlistItem> wishListService, IGenericDal<Entities.Concrete.ApUser> applicationuserService, IGenericDal<Product> productService)
        {
            _wishListService = wishListService;
            _userService = applicationuserService;
            _productService = productService;
            _productImgService = productImgService;
            _prService = prService;

        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId))
                return View(new ProductListViewModel()); // Kullanıcı yoksa boş model dön

            // Kullanıcının wishlist ürünleri
            var allWishItems = _wishListService.GetList().ToList();
            var userWishListItems = allWishItems.Where(ci => ci.ApplicationUserId == userId).ToList();

            var productIds = userWishListItems.Select(ci => ci.ProductId).Distinct().ToList();

            // Wishlist'teki ürünleri al
            var products = _productService.GetList(p => productIds.Contains(p.ProductId)).ToList();

            // Ürünlere rating ekle
            getRating(products);

            // Ürün resimlerini getir
            var productImages = new Dictionary<int, List<ImageViewModel>>();
            foreach (var product in products)
            {
                var images = _productImgService.GetList(x => x.productId == product.ProductId).ToList();
                if (images.Any())
                {
                    productImages[product.ProductId] = images.Select(img => new ImageViewModel
                    {
                        pImgId = img.pImgId,
                        ImageBase64 = $"data:{img.ContentType};base64,{Convert.ToBase64String(img.ImageData)}"
                    }).ToList();
                }
            }

            // Wishlist itemlarına ürün bilgilerini bağla
            foreach (var item in userWishListItems)
            {
                var product = products.FirstOrDefault(p => p.ProductId == item.ProductId);
                if (product != null)
                {
                    item.Product = product;
                }
            }

            var model = new ProductListViewModel
            {
                Product = products.FirstOrDefault(), // İstersen belirli bir ürün seçebilirsin
                ProductImages = productImages,
                wishlistItems = userWishListItems
            };

            return View(model);
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

        [HttpPost]
        public IActionResult AddToWL(int productId)
        {
            // Kullanıcının kimliğini al
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToAction("Index", "Login"); // Giriş yapmamışsa giriş sayfasına yönlendir
            }

            // Ürünü al
            var product = _productService.Get(x => x.ProductId == productId);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Ürün bulunamadı."; // Ürün yoksa hata mesajı göster
                return RedirectToAction("Index", "Product"); // Ürün sayfasına yönlendir
            }

            // Sepetteki mevcut ürünleri al
            var existingWishItem = _wishListService.Get(x => x.ProductId == productId && x.ApplicationUserId == Convert.ToInt32(userId));
            if (existingWishItem != null)
            {

                // Eğer sepetteki ürün varsa, miktarı artır
                if (existingWishItem.ProductId == productId && existingWishItem.ApplicationUserId == Convert.ToInt32(userId))
                {
                    existingWishItem.Product = product;
                    existingWishItem.ApplicationUserId = Convert.ToInt32(userId);
                    try
                    {
                        // Sepet öğesini güncelle
                        _wishListService.Add(existingWishItem); // Sepeti güncelle
                        TempData["Message"] = "Ürün Favorilere Eklendi."; // Başarılı güncelleme mesajı

                    }
                    catch (DbUpdateException ex)
                    {
                        TempData["ErrorMessage"] = "Güncelleme sırasında bir hata oluştu: " + ex.InnerException?.Message; // Hata mesajı göster
                    }

                    return RedirectToAction("Index", "Product"); // Sepet sayfasına dön
                }
            }


            // Eğer sepetteki ürün yoksa, yeni ürün ekle
            var newWishItem = new WishlistItem
            {
                ProductId = productId,
                ApplicationUserId = Convert.ToInt32(userId),
                AddedDate = DateTime.Now, // Eklenme tarihi
            };

            _wishListService.Add(newWishItem); // Yeni ürünü sepete ekle

            TempData["Message"] = "Ürün Favorilere eklendi."; // Başarılı ekleme mesajı


            return RedirectToAction("Index", "Product"); // Ürün sayfasına geri dön
        }

        [HttpPost]
        public IActionResult RemoveFromWLI(int WLItemId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToAction("Index", "Login"); // Giriş yapmamışsa giriş sayfasına yönlendir
            }

            var WLItem = _wishListService.Get(x => x.WishlistItemId == WLItemId); // Wl öğesini al
                                                                                  // Ürünü al
            var product = _productService.GetList();
            WLItem.Product = product.FirstOrDefault();
            if (WLItem == null)
            {
                TempData["ErrorMessage"] = "Favori öğesi bulunamadı."; // Hata mesajı göster
                return RedirectToAction("Index", "WishListItem"); // Sepet sayfasına dön
            }
            _wishListService.Delete(WLItem); // Sepet öğesini sil


            TempData["Message"] = "Favori öğesi silndi."; // Başarılı silme mesajı


            return RedirectToAction("Index", "WishListItem"); // Sepet sayfasına geri dön
        }
    }
}
