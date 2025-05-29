using Business.Abstract;
using Core.Utilities.Results;
using DataAcsess.Abstract;
using ECommorceWeb.Models;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow.ValueContentAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECommorceWeb.Controllers
{
    public class CartController : Controller
    {
        private readonly IGenericDal<ApUser> _userService; // Kullanıcı hizmetlerini yöneten servis
        private readonly IGenericDal<CartItem> _cartService; // Sepet hizmetlerini yöneten servis
        private readonly IGenericDal<Product> _productService; // Ürün hizmetlerini yöneten servis
        private readonly IGenericDal<ProductOption> _potService; // Ürün hizmetlerini yöneten servis
        private readonly IGenericDal<ProductImages> _productImgService; // Ürün hizmetlerini yöneten servis
        private readonly IGenericDal<Coupon> _cService; // Ürün hizmetlerini yöneten servis
        private readonly IGenericDal<CouponUser> _cuService; // Ürün hizmetlerini yöneten servis



        public CartController(IGenericDal<ProductOption> potService, IGenericDal<Coupon> cService, IGenericDal<CouponUser> cuService, IGenericDal<ApUser> userService, IGenericDal<CartItem> cartService, IGenericDal<Product> productService, IGenericDal<ProductImages> productImgService)
        {
            // Constructor içinde bağımlılıkları enjekte et
            _userService = userService;
            _cartService = cartService;
            _productService = productService;
            _productImgService = productImgService;
            _cService = cService;
            _cuService = cuService;
            _potService = potService;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var getImg = _productImgService.GetList().ToList();
            var resultCart = _cartService.GetList().ToList();
            List<CartItem> allCartItems = resultCart;

            var userCartItems = allCartItems.Where(ci => ci.ApplicationUserId == Convert.ToInt32(userId)).ToList();
            var productIds = userCartItems.Select(ci => ci.ProductId).Distinct().ToList();

            // Ürün bilgilerini topluca çekiyoruz
            var products = _productService.GetList(x => productIds.Contains(x.ProductId)).ToList();

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

            return View(model);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, int userCouponId, string selectedValue, decimal selectedPrice, int selectedQuantity)
        {


            var pot = _potService.Get(x => x.ProductId == productId && x.OptionValue == selectedValue);
            if (pot != null)
            {
                pot.StockQuantity -= selectedQuantity;
                _potService.Update(pot);
            }

            var product = _productService.Get(x => x.ProductId == productId);
            if (product == null)
            {
                TempData["alert"] = "Ürün Bulunamadı.";
                TempData["alertType"] = "invalid";
                return RedirectToAction("Index", "Product");

            }


            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;


            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Login");
            }
            var existingCartItem = _cartService.Get(x => x.ApplicationUserId == Convert.ToInt32(userId) && x.SelectedValue == selectedValue && x.SelectedPrice == selectedPrice && x.isPaid == false);

            if (existingCartItem == null)
            {
                var newCartItem = new CartItem
                {
                    ProductId = productId,
                    ApplicationUserId = Convert.ToInt32(userId),
                    SelectedValue = selectedValue,
                    SelectedPrice = selectedPrice,
                    SelectedQuantity = selectedQuantity,
                    OrderId = 0,
                    isPaid = false,
                    AddedDate = DateTime.Now,
                };

                
                    _cartService.Add(newCartItem);
                    TempData["alert"] = "Başarıyla Sepete Eklendi.";
                    TempData["alertType"] = "success";
               
               

            }
            else
            {
                existingCartItem.SelectedQuantity += selectedQuantity;
                _cartService.Update(existingCartItem);
                TempData["alert"] = "Ürün sepetteki miktarı artırıldı.";
                TempData["alertType"] = "success";
            }
            // Mevcut veriyi çek
            var userCoupon = _cuService.Get(x => x.CouponId == userCouponId);

            if (userCoupon != null)
            {
                userCoupon.isUsed = true;
                _cuService.Update(userCoupon); // Aynı nesneyi güncelliyoruz
            }

            return RedirectToAction("Index", "Product");
        }





        [HttpPost]
        public IActionResult RemoveFromCart(int cartItemId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            if (userId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var cartItem = _cartService.Get(x => x.CartItemId == cartItemId);


            var pot = _potService.Get(x=>x.ProductId == cartItem.ProductId && x.OptionValue == cartItem.SelectedValue);
            if (pot != null)
            {
                pot.StockQuantity += cartItem.SelectedQuantity;
                _potService.Update(pot);
            }
            if (cartItem == null)
            {
                TempData["ErrorMessage"] = "Sepet öğesi bulunamadı.";
                return RedirectToAction("Index", "Cart");
            }

            _cartService.Delete(cartItem);
            TempData["Message"] = "Silme işlemi başarılı";

            return RedirectToAction("Index", "Cart");
        }
    }
}
