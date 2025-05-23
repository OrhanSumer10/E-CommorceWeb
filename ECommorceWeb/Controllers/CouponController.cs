using DataAcsess.Abstract;
using ECommorceWeb.Models;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ECommorceWeb.Controllers
{
    [Authorize]
    public class CouponController : Controller
    {
        IGenericDal<Coupon> _coupon;
        IGenericDal<CouponProduct> _couponproduct;
        IGenericDal<Product> _product;
        IGenericDal<ApUser> _apUser;
        IGenericDal<CouponUser> _couponUser;
        public CouponController(IGenericDal<Product> product, IGenericDal<Coupon> coupon, IGenericDal<CouponProduct> couponproduct, IGenericDal<ApUser> apUser, IGenericDal<CouponUser> couponUser)
        {
            _coupon = coupon;
            _couponproduct = couponproduct;
            _apUser = apUser;
            _couponUser = couponUser;
            _product = product;

        }
        public IActionResult Index()
        {
            var coupons = _coupon.GetList().ToList();
            var couponproducts = _couponproduct.GetList().ToList(); // ürün servisini kullandığını varsayıyorum
            var users = _apUser.GetList().ToList();       // kullanıcı servisini kullandığını varsayıyorum
            var apUsers = _couponUser.GetList().ToList();       // kullanıcı servisini kullandığını varsayıyorum
            var products = _product.GetList().ToList();       // kullanıcı servisini kullandığını varsayıyorum

            var model = new ProductListViewModel
            {
                coupons = coupons,
                couponProducts = couponproducts,
                apUsers = users,
                couponUsers = apUsers,
                Products = products,
            };

            return View(model);
        }



        public IActionResult addCoupon()
        {
            var coupons = _coupon.GetList().ToList();
            var couponproducts = _couponproduct.GetList().ToList(); // ürün servisini kullandığını varsayıyorum
            var users = _apUser.GetList().ToList();       // kullanıcı servisini kullandığını varsayıyorum
            var apUsers = _couponUser.GetList().ToList();       // kullanıcı servisini kullandığını varsayıyorum
            var products = _product.GetList().ToList();       // kullanıcı servisini kullandığını varsayıyorum

            var model = new ProductListViewModel
            {
                coupons = coupons,
                couponProducts = couponproducts,
                apUsers = users,
                couponUsers = apUsers,
                Products = products,
            };


            return View(model);
        }

        [HttpPost]
        public IActionResult addCoupon(Coupon coupon, List<int> productIds, List<int> userIds)
        {
            try
            {
                // 1. Kuponu veritabanına kaydet ve Id'sini al
                _coupon.Add(coupon); // burada context.SaveChanges() çağrılıyor olmalı

                // 2. Seçilen ürünler için CouponProduct kayıtları oluştur
                foreach (var productId in productIds)
                {
                    _couponproduct.Add(new CouponProduct
                    {
                        ProductId = productId,
                        CouponId = coupon.CouponId // ilişkili kupon
                    });
                }

                // 3. Seçilen kullanıcılar için CouponUser kayıtları oluştur
                foreach (var userId in userIds)
                {
                    _couponUser.Add(new CouponUser
                    {
                        UserId = userId,
                        CouponId = coupon.CouponId // ilişkili kupon
                    });
                }

                return RedirectToAction("Index", "Coupon");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPost]
        public IActionResult DeleteCoupon(int CouponId)
        {
            // 1. CouponUser kayıtlarını sil
            var couponUsers = _couponUser.GetList().Where(x => x.CouponId == CouponId);

            foreach (var user in couponUsers)
            {

                _couponUser.Delete(user);

            }


            // 2. CouponProduct kayıtlarını sil
            var couponProducts = _couponproduct.GetList().Where(x => x.CouponId == CouponId);
            foreach (var product in couponProducts)
            {
                _couponproduct.Delete(product);
            }

            // 3. Kuponu sil
            var coupon = _coupon.GetList().FirstOrDefault(x => x.CouponId == CouponId);
            if (coupon != null)
            {
                _coupon.Delete(coupon);
            }

            return RedirectToAction("Index");
        }



      
    }
}
