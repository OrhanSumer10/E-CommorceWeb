using DataAcsess.Abstract;
using ECommorceWeb.Models;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommorceWeb.ViewComponents
{
    public class HelpViewComponent : ViewComponent
    {


        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }

    }
}
