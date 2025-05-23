using DataAcsess.Abstract;
using ECommorceWeb.Models;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommorceWeb.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {


        private IGenericDal<Category> _categoryDal;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public MenuViewComponent( IHttpContextAccessor httpContextAccessor, IGenericDal<Category> categoryDal)
        {
            _categoryDal = categoryDal;
            
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {

            var categories = _categoryDal.GetList()
                             .OrderBy(x => x.CategoryId)
                             .ToList();
            var model = new ProductListViewModel
            {
             Categories = categories,
            };

            return View(model);
        }

    }
}
