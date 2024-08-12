
using MediaOutdoor.Models;
using MediaOutDoor.Models;
using MediaOutDoor.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<MediaOutdoorContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")/*, optionsBuilder => optionsBuilder.EnableRetryOnFailure()*/));


// Multilanguage Services.

builder.Services.AddControllersWithViews().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);
builder.Services.AddLocalization(option =>
{
    option.ResourcesPath = "Resources";
});

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCulture = new[]
    {
        new CultureInfo("en-US"),
        new CultureInfo("nl-NL")
    };
    options.DefaultRequestCulture = new RequestCulture("en-US");
    options.SupportedUICultures = supportedCulture;
});

builder.Services.AddCors(cors => cors.AddPolicy("MyPolicy", builder =>
{
    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));


// Session Services.
builder.Services.AddSession();
builder.Services.AddMvc();

builder.Services.AddSession(s =>
{
    s.IdleTimeout = TimeSpan.FromDays(30);
    s.Cookie.HttpOnly = true;
    s.Cookie.IsEssential = true;
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

builder.Services.AddScoped<IEmail, Email>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Session Services.
app.UseSession();

// Multilanguage Services.
app.UseCors("MyPolicy");
app.UseRequestLocalization();


app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=MOD}/{action=Index}/{id?}");

app.Run();
