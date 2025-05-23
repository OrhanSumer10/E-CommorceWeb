using Business.Abstract;
using DataAcsess.Abstract;
using ECommorceWeb.Models;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace ECommorceWeb.Controllers
{
    public class AddressController : Controller
    {
        private readonly IGenericDal<ApUser> _userService; // Kullanıcı hizmetlerini yöneten servis
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGenericDal<Address> _addressService;
        public AddressController(IGenericDal<Address> addressService, IGenericDal<ApUser> userService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _addressService = addressService;
        }

        public IActionResult Index()
        {
            return View();
        }


        [Authorize]
        public IActionResult AddAddress()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddAddress(Address address)
        {

            // Oturum açmış olan kullanıcının ID'sini alıyoruz
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "Kullanıcı oturum açmamış.";
                return RedirectToAction("Login", "Account");
            }
            var model = new Address
            {
                UserId = Convert.ToInt32(userId),
                AddressTitle = address.AddressTitle,
                City = address.City,
                State = address.State,
                Neighbourhood = address.Neighbourhood,
                Street = address.Street,
                IsDefaultBilling = false,
                IsDefaultShipping = false
            };

            _addressService.Add(model);
            try
            {

                TempData["Message"] = "Adres başarıyla eklendi.";
                return RedirectToAction("Profile", "User");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Adres eklenemedi." + ex.Message;
            }
            return BadRequest("Hata : ");

        }

        [Authorize]
        [HttpPost]
        public IActionResult deleteAddress(int AddressId)
        {

            var address = _addressService.Get(x => x.AddressId == AddressId);

            if (address == null)
            {
                return NotFound("Address not found.");
            }

            _addressService.Delete(address);

            try
            {
                return RedirectToAction(actionName: "Profile", controllerName: "User");
            }
            catch (Exception ex) {

                return BadRequest("Hata : " + ex.Message); // Silme başarısızsa hata mesajını döndür
            }

        }



        [Authorize]
        [HttpGet]
        public IActionResult UpdateAddress()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult UpdateAddress(Address address)
        {

            // Kullanıcının kimliğini al
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Yeni model oluştur
            var model = new Address
            {
                AddressId = address.AddressId,
                UserId = Convert.ToInt32(userId),
                AddressTitle = address.AddressTitle,
                City = address.City,
                State = address.State,
                Neighbourhood = address.Neighbourhood,
                Street = address.Street,
                IsDefaultBilling = address.IsDefaultBilling,
                IsDefaultShipping = address.IsDefaultShipping,
            };

            // Güncelleme işlemi
            _addressService.Update(model);


            // Güncelleme başarılıysa Profile sayfasına yönlendir
            return RedirectToAction("Profile", "User");
        }

    }
}
