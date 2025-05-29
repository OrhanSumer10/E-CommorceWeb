using Business.Abstract;
using DataAcsess.Abstract;
using ECommorceWeb.Models;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.Security.Claims;

namespace ECommorceWeb.Controllers
{
    public class UserController : Controller
    {
        IGenericDal<ApUser> _applicationuserService;
        private readonly IViewComponentHelper _viewComponentHelper;

        public UserController(IGenericDal<ApUser> applicationuserService, IViewComponentHelper viewComponentHelper)
        {
            _applicationuserService = applicationuserService;
            _viewComponentHelper = viewComponentHelper;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminUser()
        {
            var result = _applicationuserService.GetList().ToList();

            var model = new AUListViewModel
            {
                ListUser = result,
            };
            return View(model);
        }








        public IActionResult Profile()
        {
            // Kullanıcının kimliğini al
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Kullanıcıyı veritabanından al
            var user = _applicationuserService.Get(x=>x.ApplicationUserId == Convert.ToInt32(userId));

            var model = new AUListViewModel
            {
                AUser = user
            };
            return View(model);
        }


        [HttpPost]
        public IActionResult UpdateUser(ApUser user)
        {
            // 1. Kullanıcıyı veritabanından al
            var userNew = _applicationuserService.Get(x=>x.ApplicationUserId == user.ApplicationUserId);
            var existingUser = userNew;



            if (existingUser == null)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }

            // 2. Güncellenecek modeli oluştur
            var model = new ApUser
            {
                // Var olan verileri güncelleme
                ApplicationUserId = existingUser.ApplicationUserId,  // ID'yi koru
                Name = user.Name,  // Null kontrolü ekle
                LastName = user.LastName,
                UserName = user.UserName ?? existingUser.UserName,
                Email = user.Email ?? existingUser.Email,
                Gender = user.Gender,
                IsActive = user.IsActive ?? existingUser.IsActive,
                IsAdmin = user.IsAdmin ?? existingUser.IsAdmin,
                DateOfBirth = user.DateOfBirth,
                CreatedAt = user.CreatedAt ?? existingUser.CreatedAt,
                ProfilePictureUrl = user.ProfilePictureUrl,
                PhoneNumber = user.PhoneNumber ?? existingUser.PhoneNumber,
                LastLogin = DateTime.Now,  // Genellikle güncel zamanı al
            };

         
            // 4. Güncellenmiş kullanıcıyı veritabanına kaydet
            _applicationuserService.Update(model);

            // 5. Profil sayfasına yönlendir
            return RedirectToAction("Profile", "User");
        }


        [HttpPost]
        public IActionResult UpdatePassword(ApUser user , string PasswordHash)
        {
            // 1. Kullanıcıyı veritabanından al
            var userNew = _applicationuserService.Get(x => x.ApplicationUserId == user.ApplicationUserId);
            var existingUser = userNew;



            if (existingUser == null)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }

            if (existingUser.PasswordHash == PasswordHash)
            {
                return NotFound("Mevcut şifre yanlış");
            }

            // 3. Yeni şifreyi ata
            existingUser.PasswordHash = PasswordHash;

            // 4. Güncelle
            _applicationuserService.Update(existingUser);
            // 5. Profil sayfasına yönlendir
            return RedirectToAction("Profile", "User");
        }




        [HttpPost]

        public IActionResult DeleteUser(int ApplicationUserId)
        {
            var user = _applicationuserService.Get(x=>x.ApplicationUserId == ApplicationUserId);
            try
            {
                _applicationuserService.Delete(user);
                return RedirectToAction("AdminUser","User");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
