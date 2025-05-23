using Business.Abstract;
using Business.Concrete;
using DataAcsess.Abstract;
using DataAcsess.Concrete.EntityFramework;
using DataAcsess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using ServiceStack.Text;
using System;
using QuestPDF.Infrastructure;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.





builder.Services.AddScoped(typeof(IGenericDal<>), typeof(EfGenericDal<>));
builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericManager<>));

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSession();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
         .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
         {
             options.LoginPath = "/Login/Index";
             options.LogoutPath = "/Login/Logout";
             options.AccessDeniedPath = "/Login/AccessDenied";
         });


builder.Services.AddControllersWithViews();
var app = builder.Build();

 WebApplication.CreateBuilder(args);

// QuestPDF lisans tipi belirleniyor
QuestPDF.Settings.License = LicenseType.Community;

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication(); // Bu satýrý kontrol edin
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
