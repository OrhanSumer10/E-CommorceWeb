using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using ECommorceWeb.Models;
using System.Diagnostics.Contracts;
using DataAcsess.Abstract;
using System.Diagnostics;

namespace ECommorceWeb.Controllers
{
    public class LoginController : Controller
    {
        IGenericDal<ApUser> _loginService;
        public LoginController(IGenericDal<ApUser> loginService)
        {
            _loginService = loginService;
        }

        public IActionResult Index()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Profile", "User");
                }
                return View();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
            // Eğer kullanıcı zaten oturum açmışsa, anasayfaya yönlendir
           
        }
        [HttpPost]
        public async Task<IActionResult> Index(string username, string password, bool isAdmin)
        {
            // Burada isAdmin parametresini sabit true yapıyorsunuz; gerçek uygulamada bunu kaldırabilirsiniz
            isAdmin = true;

            // Kullanıcı listesini çekiyoruz
            var loginList = _loginService.GetList();

            // Kullanıcı adı ve şifre ile eşleşen kullanıcıyı buluyoruz
            var user = loginList.FirstOrDefault(c => c.UserName == username && c.PasswordHash == password);

            if (user != null)
            {
                // Kullanıcı doğrulandıysa, claim'leri oluşturuyoruz
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username), // Kullanıcı adı claim olarak ekleniyor
                    new Claim("IsAdmin", user.IsAdmin), // Kullanıcının admin olup olmadığı claim olarak ekleniyor,
                    new Claim(ClaimTypes.Role , user.IsAdmin),// claimrole isadmin bool değerini string olarak atıyoruz
                     new Claim(ClaimTypes.NameIdentifier, user.ApplicationUserId.ToString()),
    // Diğer claim'ler
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    // Authentication özelliklerini yapılandırabilirsiniz
                };

                // Kullanıcıyı giriş yapmış olarak işaretliyoruz
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), authProperties);
                // Ana sayfaya yönlendiriyoruz
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Giriş başarısızsa, hata mesajı gösteriyoruz
                TempData["Message"] = "Geçersiz kullanıcı adı veya şifre.";
                return View();
            }
        }


        [HttpPost]
        public IActionResult Register(ApUser applicationUser)
        {
            var existingUsers = _loginService.GetList().ToList();

            if (existingUsers.Any(u => u.UserName == applicationUser.UserName || u.Email == applicationUser.Email))
            {
                TempData["Message"] = "Bu kullanıcı adı veya e-posta zaten kayıtlı.";
                return RedirectToAction("Index", "Login");
            }

            var user = new ApUser
            {
                UserName = applicationUser.UserName,
                PasswordHash = applicationUser.PasswordHash,
                Email = applicationUser.Email,
                IsAdmin = "Normal",
                CreatedAt = DateTime.UtcNow,
                LastLogin = DateTime.UtcNow,
            };

             _loginService.Add(user);
            try
            {

                TempData["Message"] = "Başarıyla Kayıt Olundu.";
                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex) 
            {
                TempData["ErrorMessage"] = "Kayıt Olunamadı." + ex.Message;
            }
            return BadRequest("Hata : ");




        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
