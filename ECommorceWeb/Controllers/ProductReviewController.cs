
using DataAcsess.Abstract;
using ECommorceWeb.Models;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommorceWeb.Controllers
{

    [Authorize]
    public class ProductReviewController : Controller
    {
        IGenericDal<ProductReview> _prService;
        IGenericDal<ApUser> _userService;
        IGenericDal<Product> _productService;

        public ProductReviewController(IGenericDal<ProductReview> prService, IGenericDal<ApUser> userService, IGenericDal<Product> productService)
        {
            _prService = prService;
            _userService = userService;
            _productService = productService;

        }
        public IActionResult Index()
        {
            var preview = _prService.GetList().OrderByDescending(x => x.ProductReviewId).ToList();
            var userIdS = preview.Select(x => x.UserId).Distinct().ToList();
            var prodcutIds = preview.Select(x => x.ProductId).Distinct().ToList();

            var users = _userService.GetList().Where(x => userIdS.Contains(x.ApplicationUserId)).ToList();
            var products = _productService.GetList().Where(x => prodcutIds.Contains(x.ProductId)).ToList();


            var model = new ProductListViewModel
            {
                reviews = preview,
                apUsers = users,
                Products = products
            };

            return View(model);
        }



        [HttpPost]
        public IActionResult AddReview(ProductReview review)
        {

            try
            {
                _prService.Add(review);

                TempData["Message"] = "Başarıyla Eklendi";
                return Redirect(Request.Headers["Referer"].ToString());
            }
            catch (Exception ex)
            {
                return BadRequest("Hata : " + ex.ToString());
            }
        }

        [HttpPost]
        public IActionResult MyReviewDelete(int reviewId)
        {
            var review = _prService.Get(x => x.ProductReviewId == reviewId);
            try
            {
                _prService.Delete(review);  
                return RedirectToAction("Profile","User");
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }


           
        }


    }
}
