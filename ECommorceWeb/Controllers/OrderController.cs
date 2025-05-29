using DataAcsess.Abstract;
using ECommorceWeb.Models;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using System.Security.Claims;

namespace ECommorceWeb.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private IGenericDal<Order> _orderdal;
        private IGenericDal<CartItem> _cartdal;
        private IGenericDal<ApUser> _apuserdal;
        private IGenericDal<Address> _adresdal;
        private IGenericDal<ProductImages> _pidal;
        private IGenericDal<Product> _productDal; // Ürün hizmetlerini yöneten servis


        public OrderController(IGenericDal<Order> orderdal, IGenericDal<CartItem> cartdal, IGenericDal<Product> productDal, IGenericDal<ApUser> apuserdal, IGenericDal<Address> adresdal, IGenericDal<ProductImages> pidal)
        {
            _orderdal = orderdal;
            _cartdal = cartdal;
            _apuserdal = apuserdal;
            _adresdal = adresdal;
            _pidal = pidal;
            _productDal = productDal;

        }

        public IActionResult Index()
        {
            // Siparişleri al (UserId'ye göre)
            var orderlist = _orderdal.GetList().Where(x => x.UserId != null).OrderByDescending(p => p.OrderId).ToList(); // UserId'ye göre siparişler

            // Kullanıcıları al
            var userIds = orderlist.Select(o => o.UserId).Distinct().ToList(); // Siparişlerden benzersiz UserId'leri al
            var users = _apuserdal.GetList(x => userIds.Contains(x.ApplicationUserId)).ToList(); // Kullanıcıları getir

            // Kullanıcıya ait sepet öğelerini al (Her sipariş için)
            var cartItemsForOrders = _cartdal.GetList(x => userIds.Contains(x.ApplicationUserId)).ToList();
            var productImgs = _pidal.GetList().ToList();

            // Kullanıcı adreslerini al (Her sipariş için)
            var addressesForOrders = _adresdal.GetList(x => userIds.Contains(x.UserId)).ToList();
            // Farklı ürün ID'lerini al
            var productIds = cartItemsForOrders.Select(ci => ci.ProductId).Distinct().ToList(); // Benzersiz ürün ID'lerini listeye al

            // Tek seferde ürünleri al
            List<Product> products = _productDal.GetList(x => productIds.Contains(x.ProductId)).ToList(); // Veritabanından ürünleri al

            // Sepet öğelerini güncelle
            foreach (var item in cartItemsForOrders)
            {

                if (item.Product == null)
                    item.Product = new Product(); // <-- boş Product oluştur
                var product = products.FirstOrDefault(p => p.ProductId == item.ProductId); // Ürün bilgilerini eşleştir
                if (product != null)
                {
                    // Sepet öğesinin ürün bilgilerini güncelle
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


            // Modeli oluştur
            var model = new ProductListViewModel
            {
                orders = orderlist,
                apUsers = users, // Kullanıcılar
                cartItems = cartItemsForOrders, // Sepet öğeleri
                addresses = addressesForOrders,// Adresler
                productImages = productImgs
            };

            // View'a modeli gönder
            return View(model);


        }



        [HttpGet]
        public IActionResult Invoice(int orderId) // id = OrderId
        {
            // GÖNDERİLEN orderId'ye göre Order'ı getiriyoruz
            var order = _orderdal.Get(x => x.OrderId == orderId);

            if (order == null)
                return NotFound(); // geçersiz id geldiğinde hata döner

            // Devamında cartItem, product, user, address vs. çekebilirsin
            var cartItem = _cartdal.Get(ci => ci.CartItemId == order.cartitemId);
            var product = _productDal.Get(p => p.ProductId == cartItem.ProductId);
            var user = _apuserdal.Get(u => u.ApplicationUserId == cartItem.ApplicationUserId);
            var address = _adresdal.Get(a => a.UserId == order.UserId);

            var document = new invoice(order, cartItem, product, user, address);
            var pdfBytes = document.GeneratePdf();

            return File(pdfBytes, "application/pdf", $"Fatura-{orderId}.pdf");
        }




        [HttpPost]

        public IActionResult OSUpdate(int orderId, Order order)
        {
            var id = orderId;

            try
            {
                var exitingOrder = _orderdal.GetList().FirstOrDefault(o => o.OrderId == orderId);


                exitingOrder.Status = order.Status;

                _orderdal.Update(exitingOrder);
                return RedirectToAction("Index", "Order");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }

        }



        [HttpPost]
        public IActionResult ODelete(int orderId)
        {
            try
            {
                var order = _orderdal.GetWithIncludes(x => x.OrderId == orderId,
              c => c.CartItems
          );
                _orderdal.Delete(order);
                return RedirectToAction("Index", "Order");

            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
        }
    }
}
