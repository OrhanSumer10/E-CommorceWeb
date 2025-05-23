using Business.Abstract;
using DataAcsess.Abstract;
using ECommorceWeb.Models;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

namespace ECommorceWeb.ViewComponents
{
    public class CartItemViewComponent : ViewComponent
    {
        private readonly IGenericDal<ApUser> _userService; // Kullanıcı hizmetlerini yöneten servis
        private readonly IGenericDal<CartItem> _cartService; // Sepet hizmetlerini yöneten servis
        private readonly IGenericDal<Product> _productService; // Ürün hizmetlerini yöneten servis
        private readonly IGenericDal<ProductImages> _productImagesService; // Ürün hizmetlerini yöneten servis
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CartItemViewComponent(IGenericDal<ApUser> userService, IGenericDal<CartItem> cartService, IGenericDal<Product> productService, IGenericDal<ProductImages> productImagesService, IHttpContextAccessor httpContextAccessor)
        {
            // Constructor içinde bağımlılıkları enjekte et
            _userService = userService;
            _cartService = cartService;
            _productService = productService;
            _httpContextAccessor = httpContextAccessor;
            _productImagesService = productImagesService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var getImg = _productImagesService.GetList().ToList();


            var resultCart = _cartService.GetList();
            var allCartItems = resultCart;

            var userCartItems = allCartItems.Where(ci => ci.ApplicationUserId == Convert.ToInt32(userId)).ToList();
            var productIds = userCartItems.Select(ci => ci.ProductId).Distinct().ToList();

            List<Product> products = new List<Product>();
            foreach (var productId in productIds)
            {
                var resultProduct = _productService.Get(x => x.ProductId == productId);


                products.Add(resultProduct);

            }



            foreach (var item in userCartItems)
            {
                if (item.Product == null)
                    item.Product = new Product(); // <-- boş Product oluştur

                var product = products.FirstOrDefault(p => p.ProductId == item.ProductId);
                if (product != null)
                {
                    item.Product.SKU = product.SKU;
                    item.Product.Name = product.Name;
                    item.Product.Description = product.Description;
                    item.Product.Price = product.Price;
                    item.Product.Rating = product.Rating;
                }
            }



            var model = new CartListViewModel
            {
                CartItems = userCartItems,
                ProductImages = getImg
            };

            return View(model); // "Default" ViewComponent kısmi görünümünün adı
        }
    }

}
