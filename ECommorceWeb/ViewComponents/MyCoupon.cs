using DataAcsess.Abstract;
using ECommorceWeb.Models;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommorceWeb.ViewComponents
{
    public class MyCouponViewComponent : ViewComponent
    {


        private readonly IGenericDal<ApUser> _userService; // Kullanıcı hizmetlerini yöneten servis
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGenericDal<Coupon> _CouponService;
        private readonly IGenericDal<CouponUser> _CouponUserService;
        public MyCouponViewComponent(IGenericDal<Coupon> CouponService, IGenericDal<CouponUser> CouponUserService, IGenericDal<ApUser> userService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _CouponService = CouponService;
            _CouponUserService = CouponUserService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var usercoupon = _CouponUserService.GetList(x=>x.UserId == Convert.ToInt32(userId)).ToList();
            var couponIds = usercoupon.Select(x => x.CouponId).ToList();

            var coupon = _CouponService.GetList(x => couponIds.Contains(x.CouponId)).ToList();
            var model = new ProductListViewModel
            {
               couponUsers = usercoupon,
               coupons = coupon
            };
            return View(model);

        }
    }
}
