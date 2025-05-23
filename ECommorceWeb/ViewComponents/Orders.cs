using DataAcsess.Abstract;
using ECommorceWeb.Models;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommorceWeb.ViewComponents
{
    public class OrdersViewComponent : ViewComponent
    {

        private IGenericDal<Order> _orderdal;
        private IGenericDal<CartItem> _cartdal;
        private IGenericDal<ApUser> _apuserdal;
        private IGenericDal<Address> _adresdal;
        private IGenericDal<ProductImages> _pidal;
        private IGenericDal<Product> _productDal; // Ürün hizmetlerini yöneten servis

        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrdersViewComponent(IGenericDal<Order> orderdal, IHttpContextAccessor httpContextAccessor, IGenericDal<CartItem> cartdal, IGenericDal<Product> productDal, IGenericDal<ApUser> apuserdal, IGenericDal<Address> adresdal, IGenericDal<ProductImages> pidal)
        {
            _orderdal = orderdal;
            _cartdal = cartdal;
            _apuserdal = apuserdal;
            _adresdal = adresdal;
            _pidal = pidal;
            _productDal = productDal;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Siparişleri al (UserId'ye göre)
            var orderlist = _orderdal.GetList(x => x.UserId == Convert.ToInt32(userId)).ToList(); // UserId'ye göre siparişler

           

            // Kullanıcıya ait sepet öğelerini al (Her sipariş için)
            var cartItemsForOrders = _cartdal.GetList(x => x.ApplicationUserId == Convert.ToInt32(userId)).ToList();
            var productImgs = _pidal.GetList().ToList();

            // Kullanıcı adreslerini al (Her sipariş için)
            var addressesForOrders = _adresdal.GetList(x => x.UserId == Convert.ToInt32(userId)).ToList();
            // Farklı ürün ID'lerini al
            var productIds = cartItemsForOrders.Select(ci => ci.ProductId).Distinct().ToList(); // Benzersiz ürün ID'lerini listeye al

            // Tek seferde ürünleri al
            List<Product> products = _productDal.GetList(x => productIds.Contains(x.ProductId)).ToList(); // Veritabanından ürünleri al

            // Sepet öğelerini güncelle
            foreach (var item in cartItemsForOrders)
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


            // Benzersiz kullanıcı ID'lerini al
            var usersIds = cartItemsForOrders.Select(ci => ci.ApplicationUserId).Distinct().ToList();

            // Kullanıcıları tek sorguda al
            List<ApUser> userss = _apuserdal.GetList(x => usersIds.Contains(x.ApplicationUserId)).ToList();

            // Sepet öğelerini güncelle
            foreach (var item in cartItemsForOrders)
            {
                var user = userss.FirstOrDefault(u => u.ApplicationUserId == item.ApplicationUserId);

                if (user != null)
                {
                    if (item.ApplicationUser == null)
                    {
                        // Eğer item.ApplicationUser null ise, yeni bir nesne oluştur
                        item.ApplicationUser = new ApUser();
                    }

                    item.ApplicationUser.Name = user.Name;
                    item.ApplicationUser.LastName = user.LastName;
                }
            }

            var model = new ProductListViewModel
            {
                orders = orderlist,
                cartItems = cartItemsForOrders, // Sepet öğeleri
                addresses = addressesForOrders,// Adresler
                productImages = productImgs,
                apUsers = userss,
            };

            return View(model);
        }

    }
}
