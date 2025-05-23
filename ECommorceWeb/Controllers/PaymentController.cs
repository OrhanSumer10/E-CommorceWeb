
using DataAcsess.Abstract;
using ECommorceWeb.Models;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace ECommorceWeb.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        IGenericDal<Address> _adresdal;
        IGenericDal<CreditCard> _ccdal;
        IGenericDal<CartItem> _cidal;
        IGenericDal<ApUser> _userdal;
        IGenericDal<Order> _orderdal;
        IGenericDal<Product> _productdal;
        IGenericDal<ProductImages> _productImgdal;


        public PaymentController(IGenericDal<ProductImages> productImgdal,IGenericDal<Product> productdal,IGenericDal<Address> adresdal, IGenericDal<CreditCard> ccdal, IGenericDal<CartItem> cidal, IGenericDal<ApUser> userdal, IGenericDal<Order> orderdal)
        {
            _adresdal = adresdal;
            _ccdal = ccdal;
            _cidal = cidal;
            _userdal = userdal;
            _orderdal = orderdal;
            _productdal = productdal;
            _productImgdal = productImgdal;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var adresses = _adresdal.GetList(x => x.UserId == Convert.ToInt32(userId)).ToList();
            var credditcart = _ccdal.GetList(x => x.userId == Convert.ToInt32(userId) && x.IsDefaultCard == true).SingleOrDefault();
            var cartitem = _cidal.GetList(x => x.ApplicationUserId == Convert.ToInt32(userId) && x.isPaid == false).ToList();
            var apUser = _userdal.Get(x => x.ApplicationUserId == Convert.ToInt32(userId));
            var productIDs = cartitem.Select(x => x.ProductId).ToList();
            var product = _productdal.GetList(x => productIDs.Contains(x.ProductId)).ToList();
            var getImg = _productImgdal.GetList().ToList();

            if (cartitem.Any(x => x.isPaid == false))
            {
                var model = new ProductListViewModel
                {
                    addresses = adresses,
                    cartItems = cartitem,
                    CreditCard = credditcart,
                    apUser = apUser,
                    Products = product,
                    productImages = getImg
                };
                return View(model);
            }
            else
            {
                return RedirectToAction("Index","Cart");
            }
         
        }



        [HttpPost]
        public IActionResult addOrder()
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                int parsedUserId = Convert.ToInt32(userId);

                // Kullanıcının sepetindeki ödeme yapılmamış ürünleri al
                var cartItems = _cidal.GetList(x => x.ApplicationUserId == parsedUserId && !x.isPaid).ToList();

          

                if (cartItems.Any()) // Eğer sepet boş değilse sipariş oluştur
                {
                    try
                    {
                        // 1️⃣ Sepetteki ödeme yapılmamış ürünlerin ID'lerini al
                        foreach (var item in cartItems)
                        {
                            bool sd = item.isPaid;
                            if (item.isPaid == false)
                            {
                                item.isPaid = true;
                              
                                _cidal.Update(item);
                            }
                          
                            // Yeni sipariş oluştur
                            var newOrder = new Order
                            {
                                UserId = parsedUserId,
                                cartitemId = item.CartItemId, // Sepetteki ürünün ID’si
                                OrderDate = DateTime.Now,
                                Status = Order.OrderStatus.Pending, // Örnek: Yeni siparişin durumu
                                Payment = Order.PaymentType.Online // Örnek: Ödeme durumu
                            };
                            _orderdal.Add(newOrder); // Siparişi veritabanına ekle
                        }

                    }
                    catch (Exception ex)
                    {
                        TempData["ErrorMessage"] = "Sipariş oluşturulamadı: " + ex.Message;
                        return RedirectToAction("Profile", "User");
                    }

                    TempData["Message"] = "Sipariş başarıyla oluşturuldu.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Sepetinizde ödeme yapılmamış ürün bulunmamaktadır.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Sipariş oluşturulamadı: " + ex.Message;
            }

            return RedirectToAction("Profile", "User");
        }
    }
}
