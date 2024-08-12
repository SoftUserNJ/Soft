using DevExpress.AspNetCore;
using DevExpress.XtraReports.Web.Extensions;
using SoftaxeERP_API;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SoftaxeERP_API.Models;
using SoftaxeERP_API.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ErpSoftaxeContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddMvc();
builder.Services.AddDevExpressControls();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IAccountsOpening, AccountsOpening>();
builder.Services.AddScoped<IAdmin, Admin>();
builder.Services.AddScoped<IApproval, Approval>();
builder.Services.AddScoped<IAuth, Auth>();
builder.Services.AddScoped<IBillDueStatus, BillDueStatus>();
builder.Services.AddScoped<IChartOfAccount, COA>();
builder.Services.AddScoped<ICostCentre, CostCentre>();
builder.Services.AddScoped<ICustomerSupplier, CustomerSupplier>();
builder.Services.AddScoped<IDataLogic, DataLogic>();
builder.Services.AddScoped<IFileUpload, FileUpload>();
builder.Services.AddScoped<IJournalVoucher, JournalVoucher>();
builder.Services.AddScoped<IManagePOS, ManagePOS>();
builder.Services.AddScoped<IPaymentReceipts, PaymentReceipts>();
builder.Services.AddScoped<IPaymentReceiptsStatus, PaymentReceiptsStatus>();
builder.Services.AddScoped<IPostDateCheque, PostDateCheque>();
builder.Services.AddScoped<IProduct, Product>();
builder.Services.AddScoped<IPurchaseInvoice, PurchaseInvoices>();
builder.Services.AddScoped<ISaleByOrder, SaleByOrder>();
builder.Services.AddScoped<ISaleInvoice, SaleInvoices>();
builder.Services.AddScoped<IServices, Services>();
builder.Services.AddScoped<IStock, Stock>();
builder.Services.AddScoped<ITrialBalance, TrialBalance>();
builder.Services.AddScoped<IUser, Users>();
builder.Services.AddScoped<IWarehouse, Warehouse>();
builder.Services.AddScoped<IFileMaintain, FileMaintain>();
builder.Services.AddScoped<IEmployee, Employee>();
builder.Services.AddScoped<IProvidentFund, ProvidentFund>();
builder.Services.AddScoped<IEmployeeDeduction, EmployeDeduction>();
builder.Services.AddScoped<IEmployeeIncentives, EmployeeIncentives>();
builder.Services.AddScoped<IEmployeeLeaves, EmployeeLeaves>();
builder.Services.AddScoped<ISalaryCalculation, SalaryCalculation>();
builder.Services.AddScoped<IAudit, Audit>();
builder.Services.AddScoped<IGatePassInward, GatePassInward>();
builder.Services.AddScoped<ILab, Lab>();
builder.Services.AddScoped<ICommonFieldsData, CommonFieldsData>();
builder.Services.AddScoped<IWeighBridge, WeighBridge>();
builder.Services.AddScoped<IAccounts, Accounts>();
builder.Services.AddScoped<IReports, Reports>();
builder.Services.AddScoped<ICurrency, Currency>();
builder.Services.AddScoped<IDoEntry, DoEntry>();
builder.Services.AddScoped<ISaleBooking, SaleBooking>();
builder.Services.AddScoped<IGpOutWard, GpOutWard>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("SoftaxeErpOrigins", policy =>
    {
        policy.WithOrigins("https://localhost:17955", // Local Mobile App origin for development
                           "https://pos.softaxe.com:1010", // Mobile App origin
                           "https://pos.softaxe.com", // Mobile App origin

                           "https://pos.softaxe.com:333",
                           "https://pos.softaxe.com:444",
                           "https://pos.softaxe.com:555",
                           "https://pos.softaxe.com:666",
                           "https://pos.softaxe.com:777",
                           "https://pos.softaxe.com:888",

                           "https://localhost:4200", // Frontend origin
                           "http://localhost:4200", // Frontend origin

                           "https://pos.softaxe.com:8091", // WebAPI own origin
                           "https://pos.softaxe.com:8092", // WebAPI own origin
                           "https://pos.softaxe.com:8094", // WebAPI own origin
                           "https://pos.softaxe.com:8081", // WebAPI own origin
                           "https://pos.softaxe.com:999",

                           "https://pos.softaxe.com:8082",
                           "http://154.12.225.160:777") // WebAPI own origin
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // Only if your frontend needs to send credentials like cookies or auth headers
    });
});


//builder.Services.AddCors(cors => cors.AddPolicy("SoftaxeErpOrigins", policy =>
//{
//    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
//}));

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddScoped<ReportStorageWebExtension, MyReportStorage>();
var app = builder.Build();
app.UseCors("SoftaxeErpOrigins");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseDevExpressControls();
app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();