using MediaOutdoor_Backend.Models;
using Microsoft.EntityFrameworkCore;
using MediaOutdoor_Backend.Services;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using MediaOutdoor_Backend.Sevices;



var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<MediaOutdoorContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")/*, optionsBuilder => optionsBuilder.EnableRetryOnFailure()*/));


// Add services to the container.

builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();
builder.Services.AddMvc();
builder.Services.AddHttpClient();

builder.Services.AddHostedService<ScheduleMessage>();


builder.Services.AddSession(s =>
{
    s.IdleTimeout = TimeSpan.FromDays(30);
    s.Cookie.HttpOnly = true;
    s.Cookie.IsEssential = true;
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped<IEmail, Email>();
builder.Services.AddScoped<IPushNotification, PushNotification>();

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

app.UseSession();
app.UseCors("MyPolicy");


app.UseRequestLocalization();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Backend}/{action=Login}/{id?}");

app.Run();
