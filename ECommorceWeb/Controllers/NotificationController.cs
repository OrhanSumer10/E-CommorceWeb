using DataAcsess.Abstract;
using ECommorceWeb.Models;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using static Entities.Concrete.Notification;

namespace ECommorceWeb.Controllers
{

    [Authorize]
    public class NotificationController : Controller
    {

        IGenericDal<Notification> _not;
        public NotificationController(IGenericDal<Notification> not)
        {
            _not = not;
        }



        public IActionResult Index()
        {
            var newNot = _not.GetList().OrderByDescending(n => n.CreatedAt)
                 .ToList(); ;
            var model = new ProductListViewModel
            {
                notifications = newNot
            };

            return View(model);
        }


        public IActionResult addNot()
        {
            ViewBag.StatusList = Enum.GetValues(typeof(NotificationStatus))
                         .Cast<NotificationStatus>()
                         .Select(s => new SelectListItem
                         {
                             Value = s.ToString(),
                             Text = s.ToString()
                         }).ToList();
            return View();
        }

        [HttpPost]
        public IActionResult addNot(Notification notification)
        {
            try
            {
                _not.Add(notification);
                return RedirectToAction("Index", "Notification");
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
        }

        [HttpPost]
        public IActionResult deleteNot(int NotificationId)
        {
            try
            {
                var not = _not.Get(x => x.NotificationId == NotificationId);
                if (not == null)
                {
                    return NotFound(); // Böyle bir bildirim yoksa
                }
                _not.Delete(not);
                return RedirectToAction("Index", "Notification");

            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }

        }
    }
}
