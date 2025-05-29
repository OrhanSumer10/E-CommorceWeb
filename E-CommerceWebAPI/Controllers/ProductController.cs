using Microsoft.AspNetCore.Mvc;
using DataAcsess.Concrete;
using DataAcsess.Abstract;
using Entities.Concrete;

namespace E_CommerceWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {

        private IGenericDal<Product> _productService;
        public IActionResult Index()
        {
            return View();
        }
    }
}
