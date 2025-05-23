using DataAcsess.Abstract;
using ECommorceWeb.Models;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace ECommorceWeb.ViewComponents
{
    public class ReviewViewComponent : ViewComponent
    {

        private readonly IGenericDal<ApUser> _userService; // Kullanıcı hizmetlerini yöneten servis
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGenericDal<ProductReview> _productReviews;
        public ReviewViewComponent(IGenericDal<ProductReview> productReviews, IGenericDal<ApUser> userService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _productReviews = productReviews;
        }
        public async Task<IViewComponentResult> InvokeAsync(int productId)
        {
            var reviews = _productReviews.GetList(x => x.ProductId == productId).ToList();
            var userIds = reviews.Select(x => x.UserId).Distinct().ToList(); // distinc ile tekrarlayanları attık 
            var users = _userService.GetList().Where(x => userIds.Contains(x.ApplicationUserId)).ToList() ;

            var model = new ProductListViewModel
            {
                reviews = reviews,
                apUsers = users
            };
            return View(model);
        }
    }
}
