using Business.Abstract;
using DataAcsess.Abstract;
using DataAcsess.Concrete.EntityFramework;
using ECommorceWeb.Models;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommorceWeb.ViewComponents
{
    public class AddressViewComponent : ViewComponent
    {
        private readonly IGenericDal<ApUser> _userService; // Kullanıcı hizmetlerini yöneten servis
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGenericDal<Address> _addressService;
        public AddressViewComponent(IGenericDal<Address> addressService, IGenericDal<ApUser> userService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _addressService = addressService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);



            var resultAddress = _addressService.GetList();
            var allAddresses = resultAddress?.Where(x => x.UserId == Convert.ToInt32(userId)).ToList();

            var model = new AddressListViewModel
            {
                Listaddress = allAddresses,
            };
            return View(model);

        }

    }
}
