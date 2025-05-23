using DataAcsess.Abstract;
using ECommorceWeb.Models;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Entities.Concrete.Notification;

namespace ECommorceWeb.ViewComponents
{
    public class NotificationViewComponent : ViewComponent
    {

        IGenericDal<Notification> _notification;
        public NotificationViewComponent(IGenericDal<Notification> notification)
        {
            _notification = notification;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
           
            var notifications = _notification.GetList().OrderByDescending(n => n.CreatedAt)
                 .ToList(); ; ;

            var model = new ProductListViewModel
            {
                notifications = notifications
            };

            return View(model);
        }


    }
}
