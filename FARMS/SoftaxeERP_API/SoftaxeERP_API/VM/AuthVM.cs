using DevExpress.Charts.Native;

namespace SoftaxeERP_API.VM
{
    public class AuthVM
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserType { get; set; }
        public int GroupId { get; set; }
        public int CmpId { get; set; }
        public string CmpName { get; set; }
        public string CmpLogo { get; set; }
        public string LocId { get; set; }
        public int FinId { get; set; }
        public string CostOfSale { get; set; }
        public string DistributionPos { get; set; }
        public bool? IsSuperAdmin { get; set; }
        public bool? IsAdmin { get; set; }
        public bool? IsRound { get; set; }
        public bool? IsApprovalSystem { get; set; }
        public string ApprovalSystem { get; set; }
        public string LocationControl { get; set; }
        public string LocationWise { get; set; }
        public bool? CreditLimit { get; set; }
        public string AccountOpening { get; set; }
        public string StkAdjustmentCode { get; set; }
        public string StkTransferCode { get; set; }
        public string DiscountPurchase { get; set; }
        public string OtherCreditPurchase { get; set; }
        public string ShipmentPurchase { get; set; }
        public string DiscountSale { get; set; }
        public string OtherCreditSale { get; set; }
        public string ShipmentSale { get; set; }
        public string InputSaleTaxCode { get; set; }
        public string OtherSaleTaxCode { get; set; }
        public string WHTaxCode { get; set; }
        public string FTaxCode { get; set; }
        public string Tax1Code { get; set; }
        public string Tax2Code { get; set; }


        public string CashCode { get; set; }

        public string FreightPayableCode { get; set; }

        public string DayCloseTime { get; set; }
    }
}