using DataAcsess.Abstract;
using ECommorceWeb.Models;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommorceWeb.Controllers
{
    [Authorize]
    public class CreditCardController : Controller
    {

        IGenericDal<CreditCard> _dal;
        public CreditCardController(IGenericDal<CreditCard> dal)
        {
            _dal = dal;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult AddCreditCard(CreditCard creditCard)
        {
            // Oturum açmış olan kullanıcının ID'sini alıyoruz
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var model = new CreditCard
            {
                cartNumber = creditCard.cartNumber,
                cvv = creditCard.cvv,
                month = creditCard.month,
                year = creditCard.year,
                userId = Convert.ToInt32(userId),
                IsDefaultCard = true,
            };

            _dal.Add(model);
            try
            {

                TempData["Message"] = "Kredi Kartı başarıyla eklendi.";
                return RedirectToAction("Profile", "User");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Kredi Kartı eklenemedi." + ex.Message;
            }
            return BadRequest("Hata : ");

        }



        public IActionResult DeleteCard(int id)
        {
            var card = _dal.Get(x => x.Id == id);

            _dal.Delete(card);
            try
            {

                TempData["Message"] = "Kredi Kartı başarıyla silindi.";
                return RedirectToAction("Profile", "User");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Kredi Kartı silinemedi." + ex.Message;
            }
            return BadRequest("Hata : ");

        }
    }
}
