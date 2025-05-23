using DataAcsess.Abstract;
using ECommorceWeb.Models;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace ECommorceWeb.ViewComponents
{
    public class MyReviewViewComponent : ViewComponent
    {

        private readonly IGenericDal<ApUser> _userService; // Kullanıcı hizmetlerini yöneten servis
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGenericDal<ProductReview> _productReviews;
        private readonly IGenericDal<Product> _product;
        public MyReviewViewComponent(IGenericDal<ProductReview> productReviews, IGenericDal<ApUser> userService, IHttpContextAccessor httpContextAccessor, IGenericDal<Product> product)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _productReviews = productReviews;
            _product = product;
        }
        public async Task<IViewComponentResult> InvokeAsync(int userId)
        {
            var reviews = _productReviews.GetList(x => x.UserId == userId).ToList();
            var userIds = reviews.Select(x => x.UserId).Distinct().ToList(); // distinc ile tekrarlayanları attık 
            var productIds = reviews.Select(x => x.ProductId).Distinct().ToList(); // distinc ile tekrarlayanları attık 
            var users = _userService.GetList().Where(x => userIds.Contains(x.ApplicationUserId)).ToList();
            var products = _product.GetList().Where(x => productIds.Contains(x.ProductId)).ToList();

            var model = new ProductListViewModel
            {
                reviews = reviews,
                apUsers = users,
                Products = products,
            };
            return View(model);
        }
    }
}
