using Business.Abstract;
using ECommorceWeb.Models;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommorceWeb.ViewComponents
{
    
    public class ApplicationUserViewComponent : ViewComponent
    {
      
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }

    }
}
