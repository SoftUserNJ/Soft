using CityTech.Models;
using MailKit.Net.Imap;
using MailKit.Security;
using MailKit;
using MailKit.Search;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using CityTech.Sevices;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace CityTech
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews()
               .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);
            builder.Services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });
            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("nl-NL"),
                };
                options.DefaultRequestCulture = new RequestCulture("nl-NL");
                options.SupportedUICultures = supportedCultures;
            });


            builder.Services.AddDbContext<CityTechContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")/*, optionsBuilder => optionsBuilder.EnableRetryOnFailure()*/));
            builder.Services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = null; // Unlimited
            });

            // Configure the maximum request body size for IIS
            builder.Services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = int.MaxValue; // Effectively unlimited
            });
            builder.Services.AddMvc();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(30);
            });

            // User Authentication
            //builder.Services.AddAuthentication(options =>
            //{
            //    options.DefaultScheme = IdentityConstants.ApplicationScheme;
            //    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            //}).AddIdentityCookies();

            //builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //.AddCookie(options =>
            //{
            //    options.LoginPath = "/Home/Login";
            //    options.AccessDeniedPath = "/Home/AccessDenied";
            //});



            //builder.Services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
            //});


            // Register IMAP IDLE background service in the DI container
            builder.Services.AddHostedService<EmailBackgroundService>();
            builder.Services.AddSignalR(); // Add SignalR setup
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<ILog, Log>();
         
            var app = builder.Build();
            app.UseRequestLocalization();
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession ();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Login}/{id?}");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<EmailHub>("/emailHub"); // Map the SignalR hub
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Login}/{id?}");
            });

            app.Run();
        }
    }


}
