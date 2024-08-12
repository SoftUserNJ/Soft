using CityTechWEBAPI;
using CityTechWEBAPI.Configurations;
using CityTechWEBAPI.Models;
using CityTechWEBAPI.Services;
using Infobip.Api.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using YourNamespace.Services;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();

// Configure DbContext
builder.Services.AddDbContext<CityTechDevContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CityTechConStr"));
});

// Add authentication-related services and configure
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// Sms
builder.Services.AddHttpClient<ISmsService, InfoBipSmsService>();
// Voice
builder.Services.AddScoped<InfoBipCallService>();
// WhatsApp
builder.Services.AddScoped<IWhatsappService, InfoBipWhatsappService>();
// For Log
builder.Services.AddScoped<Logging>();
    
var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();


app.Run();
