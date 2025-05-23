using DataAcsess.Abstract;
using ECommorceWeb.Models;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using ZstdSharp.Unsafe;

namespace ECommorceWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class POTController : Controller
    {
        IGenericDal<ProductOption> _potdal;


        public POTController(IGenericDal<ProductOption> potdal)
        {
            _potdal = potdal;

        }

        [HttpGet]
        public IActionResult Index(int productId)
        {
            var getPot = _potdal.GetList(x => x.ProductId == productId).ToList();
            ViewBag.ProductId = productId; // ID'yi ViewBag ile frontend'e taşı

            var model = new POTViewModel
            {
                ProductOptions = getPot,
            };
            return View(model);
        }




        [HttpGet]
        public IActionResult AddPOT(int productId)
        {
            ViewBag.ProductId = productId; // ID'yi ViewBag ile frontend'e taşı
            return View();
        }


        [HttpPost]
        public IActionResult AddPOT([FromBody] List<ProductOption> options)
        {
            try
            {
                if (options == null || !options.Any())
                {
                    return Json(new { success = false, message = "Lütfen en az bir seçenek ekleyin!" });
                }

                foreach (var option in options)
                {
                    _potdal.Add(option); // Tek tek ekleme
                }

                return Json(new { success = true, message = "Ürün seçenekleri başarıyla eklendi!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Hata oluştu: " + ex.Message });
            }
        }


    }
}

