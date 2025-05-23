using Business.Abstract;
using DataAcsess.Abstract;
using ECommorceWeb.Models;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ECommorceWeb.Controllers
{
    public class SubCategoriesController : Controller
    {
        private IGenericDal<SubCategories> _subcategoriesService;

        private IGenericDal<Category> _categroyService;
        public SubCategoriesController(IGenericDal<SubCategories> subcategoriesService, IGenericDal<Category> categroyService)
        {
            _subcategoriesService = subcategoriesService;
            _categroyService = categroyService;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            GetCategory();
            var result = _subcategoriesService.GetList().ToList();
            List<SubCategories> subcategories = result;

            Dictionary<int, string> categoryNames = new Dictionary<int, string>();

            // Ürünlerin kategorilerini işleyelim
            foreach (var item in subcategories)
            {
                // Kategori adını al
                var categoryName = GetCategoryWithId(item.ParentCategoryId);

                // Eğer sözlükte yoksa ekleyelim
                if (!categoryNames.ContainsKey(item.ParentCategoryId))
                {
                    categoryNames[item.ParentCategoryId] = categoryName;
                }
            }
            ViewBag.CategoryNames = categoryNames;



            var model = new SubCategoriesListViewModel
            {
                SubCategories = subcategories,
            };


            return View(model);


        }

        private string GetCategoryWithId(int id)
        {
            var categoryResult = _categroyService.Get(x => x.CategoryId == id); // ID'ye göre kategori al
            if (categoryResult != null)
            {
                return categoryResult.Name; // Kategori adını döndür
            }
            return "Bilinmeyen Kategori"; // Kategori bulunamazsa
        }


        [Authorize(Roles = "Admin")]

        public IActionResult AddSubCategories()
        {
            GetCategory();
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult AddSubCategories(SubCategories subCategories)
        {
            GetCategory();


            try
            {
                _subcategoriesService.Add(subCategories);
                TempData["Message"] = "Baraşıyla Eklendi";
                return RedirectToAction("Index", "SubCategories");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Eklenemedi " + ex.Message;
            }
            return BadRequest("Hata : ");
        }


        void GetCategory()
        {
            // Kategorileri al
            var categories = _categroyService.GetList();

            // Kategori listesini SelectListItem listesine dönüştür
            List<SelectListItem> categoryValue = categories.Select(c => new SelectListItem
            {
                Text = c.Name, // Kategori adı
                Value = c.CategoryId.ToString() // Kategori ID'si
            }).ToList();

            // ViewBag'e atama yap
            ViewBag.cv = categoryValue;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult UpdateSubCategories(int subCategoryId)
        {
            GetCategory();
            var result = _subcategoriesService.Get(x => x.SubCategoryId == subCategoryId);
            var getResult = result;
            var model = new SubCategoriesListViewModel
            {
                subCategory = getResult
            };
            try
            {
                return View(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }



        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult UpdateSubCategories(SubCategories subCategories)
        {

            try
            {
                _subcategoriesService.Update(subCategories);
                return RedirectToAction("Index", "SubCategories");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult DeleteSubCategories(int subCategoryId)
        {
            // Ürünü veritabanından al
            var getSC = _subcategoriesService.Get(x => x.SubCategoryId == subCategoryId);

            if (getSC == null)
            {
                TempData["ErrorMessage"] = "Boş veri";
                return RedirectToAction("Index", "SubCategories");
            }

            // Ürünü silme işlemi
            _subcategoriesService.Delete(getSC);

            try
            {
                TempData["Message"] = "Başarıyla silindi.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Silinemedi: " + ex.Message;
            }

            return RedirectToAction("Index", "SubCategories");
        }

    }
}
