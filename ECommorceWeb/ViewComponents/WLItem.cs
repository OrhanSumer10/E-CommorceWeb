using Business.Abstract;
using DataAcsess.Abstract;
using ECommorceWeb.Models;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static ECommorceWeb.Models.ProductListViewModel;

namespace ECommorceWeb.ViewComponents
{

    public class WLItemViewComponent : ViewComponent
    {
        private readonly IGenericDal<ApUser> _userService; // Kullanıcı hizmetlerini yöneten servis
        private readonly IGenericDal<WishlistItem> _wishListService; // Sepet hizmetlerini yöneten servis
        private readonly IGenericDal<Product> _productService; // Ürün hizmetlerini yöneten servis
        private readonly IGenericDal<ProductImages> _productImgService; // Ürün hizmetlerini yöneten servis
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGenericDal<ProductReview> _prService;

        public WLItemViewComponent(IGenericDal<ProductReview> prService, IGenericDal<ProductImages> productImgService, IGenericDal<ApUser> userService, IGenericDal<WishlistItem> wishListService, IGenericDal<Product> productService, IHttpContextAccessor httpContextAccessor)
        {
            // Constructor içinde bağımlılıkları enjekte et
            _userService = userService;
            _wishListService = wishListService;
            _productService = productService;
            _productImgService = productImgService;
            _httpContextAccessor = httpContextAccessor;
            _prService = prService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userIdStr = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId))
                return View(new ProductListViewModel()); // Kullanıcı yoksa boş model dön

            var allWishItems = _wishListService.GetList().ToList();
            var userWishItems = allWishItems
                .Where(ci => ci.ApplicationUserId == userId)
                .ToList();

            var productIds = userWishItems.Select(ci => ci.ProductId).Distinct().ToList();

            // Sadece wishlist'teki ürünleri al
            var products = _productService.GetList(p => productIds.Contains(p.ProductId)).ToList();

            // Ratingleri getir
            getRating(products);

            // Wishlist item'larına ürün bilgilerini ekle
            foreach (var item in userWishItems)
            {
                var product = products.FirstOrDefault(p => p.ProductId == item.ProductId);
                if (product != null)
                {
                    item.Product = product; // Direkt olarak bağlayabilirsin
                }
            }

            // Görsel verilerini hazırla
            var productImgs = new Dictionary<int, List<ImageViewModel>>();
            foreach (var product in products)
            {
                var imageResult = _productImgService.GetList(x => x.productId == product.ProductId).ToList();
                if (imageResult.Any())
                {
                    productImgs[product.ProductId] = imageResult.Select(img => new ImageViewModel
                    {
                        pImgId = img.pImgId,
                        ImageBase64 = $"data:{img.ContentType};base64,{Convert.ToBase64String(img.ImageData)}"
                    }).ToList();
                }
            }

            var model = new ProductListViewModel
            {
                ProductImages = productImgs,
                Product = products.FirstOrDefault(), // Sayfada yalnızca bir ürün gösteriliyorsa
                wishlistItems = userWishItems
            };

            return View(model);
        }



      public void getRating(List<Product> products)
{
    var productRatings = new Dictionary<int, double>();

    foreach (var product in products)
    {
        var productReview = _prService.GetList(x => x.ProductId == product.ProductId).ToList();
        double rating = 0;

        if (productReview.Any())
        {
            rating = productReview.Average(x => x.Rating);
        }

        productRatings[product.ProductId] = rating;

        // Ürünün kendi Rating property'sini güncelle
        product.Rating = rating;
    }

    ViewBag.ProductRatings = productRatings;
}

    }
}
