using Business.Abstract;
using DataAcsess.Abstract;
using ECommorceWeb.Models;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommorceWeb.Controllers
{

    public class BrandController : Controller
    {
        private IGenericDal<Brand> _brandService;

        public BrandController(IGenericDal<Brand> brandService)
        {
            _brandService = brandService;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var result = _brandService.GetList().ToList();

            List<Brand> brands = result;

            var model = new BrandListViewModel { Brands = brands };
            try
            {
                return View(model);
            }
            catch (Exception ex) { return BadRequest("Hata : " + ex.Message); }


        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AddBrand()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddBrand(Brand brand)
        {
            _brandService.Add(brand);
            try
            {
                TempData["Message"] = "Ürün başarıyla eklendi.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ürün eklenemedi: " + ex.Message;
            }
            return RedirectToAction("Index", "Brand");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult UpdateBrand(int brandId)
        {
            var result = _brandService.Get(x => x.brandId == brandId);
            var brand = result;
            try
            {
                TempData["Message"] = "Ürün başarıyla Güncellendi.";

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ürün bulunamadı." + ex.Message;
                return RedirectToAction("Index", "Brand");
            }
            var model = new BrandListViewModel
            {
                Brand = brand
            };
            try
            {
                return View(model);
            }
            catch (Exception ex)
            {
                return BadRequest("Hata : " + ex.Message);
            }
            // Hata durumunu işleyin

        }
        [HttpPost]
        public IActionResult UpdateBrand(Brand brand)
        {
            _brandService.Update(brand);
            try
            {
                TempData["Message"] = "Ürün başarıyla Güncellendi.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ürün güncellenemedi: " + ex.Message;
            }
            return RedirectToAction("Index", "Brand");
        }


        [HttpPost]
        public IActionResult DeleteBrand(int brandId)
        {
            var brand = _brandService.Get(x=>x.brandId == brandId);
           _brandService.Delete(brand);

            try
            {
                TempData["Message"] = "Ürün başarıyla silindi.";
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = "Ürün silinemedi: " + ex.Message;
            }
            return RedirectToAction("Index", "Brand");
        }
    }
}
