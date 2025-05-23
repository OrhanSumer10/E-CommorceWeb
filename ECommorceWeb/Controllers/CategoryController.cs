using Business.Abstract;
using DataAcsess.Abstract;
using ECommorceWeb.Models;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;

namespace ECommorceWeb.Controllers
{
    public class CategoryController : Controller
    {
        IGenericDal<Category> _categoryService;

        public CategoryController(IGenericDal<Category> categoryService)
        {
            _categoryService = categoryService;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var result = _categoryService.GetList().ToList();
            List<Category> categories = result;
            var model = new ProductListViewModel
            {
                Categories = categories,
            };
            try
            {
                return View(model);
            }
            catch (Exception ex)
            {
                return BadRequest("Hata : " + ex.Message);
            }


        }


        [Authorize(Roles = "Admin")]
        public IActionResult AddCategory()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult AddCategory(Category category)
        {
            _categoryService.Add(category);
            try
            {
                TempData["Message"] = "Kategori başarıyla eklendi.";
                return RedirectToAction("Index", "Category");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Kategori eklenemedi.";
                return BadRequest("Hata : " + ex.Message);
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult DeleteCategory(int categoryId)
        {
            var category = _categoryService.Get(x => x.CategoryId == categoryId);
            _categoryService.Delete(category);
            try
            {
                TempData["Message"] = "Kategori başarıyla eklendi.";
                return RedirectToAction("Index", "Category");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Kategori eklenemedi.";
                return BadRequest("Hata :  " + ex.Message);
            }

        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult UpdateCategory(int categoryId)
        {
            var category = _categoryService.Get(x => x.CategoryId == categoryId);
            var model = new CategoryListViewModel
            {
                category = category
            };


            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult UpdateCategory(Category category)
        {
            _categoryService.Update(category);
            try 
            {
                TempData["Message"] = "Kategori başarıyla Güncellendi.";
                return RedirectToAction("Index", "Category");
            }
            catch (Exception ex )
            {
                TempData["ErrorMessage"] = "Kategori Güncellenemedi.";
                return BadRequest("Hata :  " + ex.Message);
            }
         
        }

    }
}
