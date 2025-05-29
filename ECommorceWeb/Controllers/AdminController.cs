using DataAcsess.Abstract;
using ECommorceWeb.Models;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommorceWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        IGenericDal<Product> _productService;
        IGenericDal<Order> _orderService;
        IGenericDal<ApUser> _userService;
        IGenericDal<CartItem> _cartService;
        IGenericDal<ProductReview> _prService;

        public AdminController(IGenericDal<Product> productService, IGenericDal<ProductReview> prService, IGenericDal<CartItem> cartService, IGenericDal<Order> orderService, IGenericDal<ApUser> userService)
        {
            _orderService = orderService;
            _productService = productService;
            _userService = userService;
            _cartService = cartService;
            _prService = prService;
        }
        public IActionResult Index()
        {
            var order = _orderService.GetList().ToList();
            var user = _userService.GetList().ToList();
            var product = _productService.GetList().ToList();
            var cart = _cartService.GetList().ToList();
            var pr = _prService.GetList().ToList();

            var totalSales = cart.Sum(p=>p.SelectedPrice);

            ViewBag.TotalSales = totalSales.ToString("F2");

            var model = new ProductListViewModel
            {
                orders = order,
                Products = product,
                ApplicationUsers = user,
                cartItems = cart,
                reviews = pr
            };


            return View(model);
        }
    }
}
