using SoftaxeERP_API.Models;
using SoftaxeERP_API.VM;
using System.Security.Claims;

namespace SoftaxeERP_API.Services
{
    public interface IAuth
    {
        AuthVM GetUserData();
    }

    public class Auth : IAuth
    {
        private readonly ErpSoftaxeContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Auth(ErpSoftaxeContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public AuthVM GetUserData()
        {
            int userId = 0;
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext?.User.Identity is ClaimsIdentity claimsIdentity)
            {
                userId = Convert.ToInt32(claimsIdentity.FindFirst("UserId")?.Value);
            }

            //string token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault().Split(" ").Last();

            
            var user = (from U in _context.Users
                        join C in _context.Companies on U.CmpId equals C.CmpId into comp
                        from nC in comp.DefaultIfEmpty()
                        join G in _context.CompanyGroups on nC.GrpId equals G.GrpId
                        join F in _context.Tblfinyears on U.CmpId equals F.CompId into f
                        from nF in f.DefaultIfEmpty()
                        join M in _context.TblMonthCloses on U.CmpId equals M.CompId into m
                        from nM in m.DefaultIfEmpty()
                        join L in _context.Locations on U.CmpId equals L.CmpId into l
                        from nL in l.DefaultIfEmpty()
                        //join dis in _context.TblDiscodes on U.CmpId equals dis.CmpId into d
                        //from nd in d.DefaultIfEmpty()
                        where U.Id == userId
                        select new AuthVM
                        {
                            UserId = U.Id,
                            UserName = U.UserName,
                            UserType = U.Type,
                            LocId = U.LocId,
                            FinId = nF.Finid,
                            CmpId = nC.CmpId,
                            GroupId = G.GrpId,
                            CmpName = nC.CmpName,
                            IsSuperAdmin = U.IsSuperAdmin,
                            CmpLogo = nC.Logo, 
                            IsAdmin = U.Admin,
                            IsRound = nC.RoundVal,
                            IsApprovalSystem = nC.ApprovalSystem,
                            ApprovalSystem = (nC.ApprovalSystem == true) ? " AND ISNULL(M.APROVE,0) <> 1" : "",
                            LocationControl = (nC.LocationWise == "Location Wise") ? $" AND (L5.MAPPEDCODE LIKE '%{U.LocId}%' OR ISNULL(L5.MAPPEDCODE, '') = '') " : (nC.LocationWise == "Fix Location") ? $" AND L5.LOCID = '{U.LocId}' " : "",
                            LocationWise = nC.LocationWise,
                            CostOfSale = nC.CostofSale,
                            AccountOpening = nC.AccountOpningCode,
                            DistributionPos = nC.DistributionPos,
                            StkAdjustmentCode = nC.StkAdjustmentCode,
                            StkTransferCode = nC.StkTransferCode,
                            CreditLimit = nC.CreditLimit,
                            ShipmentPurchase = nC.ShipmentPurchaseCode,
                            DiscountPurchase = nC.DiscountCodePurchase,
                            OtherCreditPurchase = nC.OtherCreditCodePurchase,
                            DiscountSale = nC.DiscountCodeSale,
                            ShipmentSale = nC.ShipmentSaleCode,
                            OtherCreditSale = nC.OtherCreditCodeSale,
                            InputSaleTaxCode = nC.InputSaleTax,
                            OtherSaleTaxCode = nC.OtherSaleTax,
                            FTaxCode = nC.FtaxCode,
                            WHTaxCode = nC.Whtcode,
                            Tax1Code = nC.Tax1Code,
                            Tax2Code = nC.Tax2Code,
                            FreightPayableCode = nC.FreightPayableCode,
                            CashCode = nC.CashCode,
                            DayCloseTime = nL.DayCloseTime,

                        }).FirstOrDefault();

            if (user == null)
            {
                return new AuthVM();
            }

            return user;
        }
    }
}
