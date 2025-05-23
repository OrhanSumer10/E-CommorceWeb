using DataAcsess.Abstract;
using ECommorceWeb.Models;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ECommorceWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        IGenericDal<Category> _category;

        public HomeController(ILogger<HomeController> logger, IGenericDal<Category> category)
        {
            _logger = logger;
            _category = category;
        }

        public IActionResult Index()
        {
            var categories = _category.GetList().ToList();
            var model = new ProductListViewModel
            {
                Categories = categories,
            };


            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}