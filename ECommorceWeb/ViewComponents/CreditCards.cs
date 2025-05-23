using DataAcsess.Abstract;
using ECommorceWeb.Controllers;
using ECommorceWeb.Models;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommorceWeb.ViewComponents
{
    public class CreditCardViewComponent : ViewComponent
    {
        private readonly IGenericDal<ApUser> _userService; // Kullanıcı hizmetlerini yöneten servis
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGenericDal<CreditCard> _ccService;
        public CreditCardViewComponent(IGenericDal<CreditCard> ccService, IGenericDal<ApUser> userService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _ccService = ccService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);


            
            var resultCreditCard = _ccService.GetList();
            var allCard = resultCreditCard?.Where(x => x.userId == Convert.ToInt32(userId)).ToList();

            var user = _userService.Get(x => x.ApplicationUserId == Convert.ToInt32(userId));

            var model = new CreditCardListViewModel
            {
                CreditCards = allCard,
                apUser = user
            };
            return View(model);

        }
    }
}
