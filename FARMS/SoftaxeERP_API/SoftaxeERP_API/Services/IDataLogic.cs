using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SoftaxeERP_API.Models;
using SoftaxeERP_API.VM;
using System.Data;

namespace SoftaxeERP_API.Services
{
    public interface IDataLogic
    {
        DataTable LoadData(string qry);
        string GetLevel4Code(string tag);
        void LogEntry(int vchNo, string vchType, string remarks, decimal amount, DateTime vchDate, decimal purchaseRate, decimal maxRate, decimal minRate, DateTime dateNow);
        string NumberToWordAmount(double amount);

        public void MakePurchasePayable(string Vchtype, int? Vchno, bool MakeFreightEntry);
        public void MakePurchasePayableWithcalculated(string Vchtype, int? Vchno , string Locid);
        
    }

    public class DataLogic : IDataLogic
    {
        private readonly ErpSoftaxeContext _context;
        private readonly IAuth _auth;
        private readonly IConfiguration _configuration;

        public static string connectionSting = "";
        readonly AuthVM auth = new();

        public DataLogic(IConfiguration configuration, ErpSoftaxeContext context, IAuth authData)
        {
            _context = context;
            _auth = authData;

            _configuration = configuration;
            connectionSting = _configuration.GetConnectionString("DefaultConnection")!;
            auth = _auth.GetUserData();
        }

        public SqlConnection con = new(connectionSting);
        public SqlDataAdapter da = new();
        public SqlCommand cmd = new();

        private static String[] units = { "Zero", "One", "Two", "Three",
		"Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven",
		"Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen",
		"Seventeen", "Eighteen", "Nineteen" };

		private static String[] tens = { "", "", "Twenty", "Thirty", "Forty",
		"Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

        public DataTable LoadData(string qry)
        {
            try
            {
                DataTable dt = new();

                if (con != null && con.State == ConnectionState.Closed)
                {
                    con.ConnectionString = connectionSting;
                    con.Open();
                }
                dt.Clear();
                da = new SqlDataAdapter(qry, con);
                da.Fill(dt);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public string GetLevel4Code(string tag)
        {
            string qry = $@" SELECT TOP(1) LEVEL3 + LEVEL4 AS CODE FROM LEVEL4 L4 WHERE COMP_ID = {auth.CmpId} AND TAG1 = '{tag}' {auth.LocationControl.Replace("L5", "L4")}";
            DataTable dt = LoadData(qry);

            return dt.Rows[0]["CODE"].ToString();
        }

        public void LogEntry(int vchNo, string vchType, string remarks, decimal amount, DateTime vchDate, decimal purchaseRate, decimal maxRate, decimal minRate, DateTime dateNow)
        {
            if (auth.IsSuperAdmin == false)
            {
                _context.TblLogs.Add(new TblLog
                {
                    Vdate = dateNow,
                    VhrDate = vchDate,
                    Vchno = vchNo,
                    Vtype = vchType,
                    PurchaseRate = purchaseRate,
                    MaxRate = maxRate,
                    MinRate = minRate,
                    Remraks = remarks,
                    Amount = amount,
                    Uid = auth.UserId,
                    CmpId = auth.CmpId,
                    Locid = auth.LocId,
                    Finid = auth.FinId
                });
                _context.SaveChanges();
            }
        }

        public string NumberToWordAmount(double amount)
		{
			try
			{
				Int64 amount_int = (Int64)amount;
				Int64 amount_dec = (Int64)Math.Round((amount - (double)(amount_int)) * 100);
				if (amount_dec == 0)
				{
					return ConvertInt64(amount_int) + " Only.";
				}
				else
				{
					return ConvertInt64(amount_int) + " Point " + ConvertInt64(amount_dec) + " Only.";
				}
			}
			catch (Exception e)
			{
				// TODO: handle exception  
			}
			return "";
		}

		public string ConvertInt64(Int64 i)
		{
			if (i < 20)
			{
				return units[i];
			}
			if (i < 100)
			{
				return tens[i / 10] + ((i % 10 > 0) ? " " + ConvertInt64(i % 10) : "");
			}
			if (i < 1000)
			{
				return units[i / 100] + " Hundred"
						+ ((i % 100 > 0) ? " And " + ConvertInt64(i % 100) : "");
			}
			if (i < 100000)
			{
				return ConvertInt64(i / 1000) + " Thousand "
						+ ((i % 1000 > 0) ? " " + ConvertInt64(i % 1000) : "");
			}
			if (i < 10000000)
			{
				return ConvertInt64(i / 100000) + " Lakh "
						+ ((i % 100000 > 0) ? " " + ConvertInt64(i % 100000) : "");
			}
			if (i < 1000000000)
			{
				return ConvertInt64(i / 10000000) + " Crore "
						+ ((i % 10000000 > 0) ? " " + ConvertInt64(i % 10000000) : "");
			}
			return ConvertInt64(i / 1000000000) + " Arab "
					+ ((i % 1000000000 > 0) ? " " + ConvertInt64(i % 1000000000) : "");
		}

        public void MakePurchasePayable(string Vchtype, int? Vchno , bool MakeFreightEntry)
        {
            _context.TblTransVches.Where(x => x.VchType == Vchtype && x.VchNo == Vchno && x.FinId == auth.FinId && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.Sno == 51).ExecuteDelete();
            _context.TblTransVches.Where(x => x.VchType == Vchtype && x.VchNo == Vchno && x.FinId == auth.FinId && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.Sno != 51 && x.Tucks != 8).ExecuteDelete();
            var vches = _context.TblTransVches.Where(x => x.CmpId == auth.CmpId && x.FinId == auth.FinId && x.VchType == Vchtype && x.LocId == auth.LocId && x.VchNo == Vchno && x.Tucks == 8 && x.Sno != 51).ToList();
            var firstRow = vches.FirstOrDefault();
            var Partyinfo = _context.Level5s.Where(x => x.CompId == auth.CmpId && x.Level4 + x.Level51 == firstRow.Mcode).FirstOrDefault();
            var Productinfo = _context.Level5s.Where(x => x.CompId == auth.CmpId && x.Level4 + x.Level51 == firstRow.Dmcode + firstRow.Code).FirstOrDefault();
            decimal Tqty = 0;
            decimal TPayableWt1 = 0;
            double Tamount = 0;
            double TCommission = 0;
            double TSalesTaxAmt = 0;
            double Tfedamt = 0;

            double? Bagsamt1 = 0;
            double? Bagsamt2 = 0;
            double? Bagsamt3 = 0;
            double? Bagsrate1 = 0;
            double? Bagsrate2 = 0;
            double? Bagsrate3 = 0;
            string SalesTaxCode = auth.Tax1Code;
            string BrokerCode = "00000000000000";
            string FreightVchtype = "CP-FREIGHT";
            string DesComm = "";

            string Des9 = "";
            int sno = 0;
            foreach (var vch in vches)
            {
                double Gamount = 0;
                double Amount = 100;
                double AmountWithComm = 0;
                double Brat1 = 0;
                double Brat2 = 0;
                double Brat3 = 0;
                double WHtaxamt = 0;
                double SalesTaxAmt = 0;
                double fedamt = 0;
                double Rate = 0;
                double MPayablewt1 = 0;
                double Payablewt1 = 0;
                double Commission = 0;
                double BrokerComm = 0;
                string BrokerCommUom = "";
                //locid check pending
                var PoDetail = _context.TblPurchaseContractDetails.Where(x => x.CmpId == auth.CmpId && x.FinId == auth.FinId && x.VchType == "PO-Pur" && x.PoNo == vch.Pono && x.Icode + x.IsubCode == vch.Dmcode + vch.Code).FirstOrDefault();
                var uom = _context.TblUoms.Where(x => x.CompId == auth.CmpId && x.Id == Convert.ToInt16(vch.Uom)).FirstOrDefault();

                if (PoDetail != null)
                {
                    BrokerCode = PoDetail.Bcode + PoDetail.BsubCode;
                    MPayablewt1 = (Convert.ToDouble(vch.ExpWt) > 0) ? Convert.ToDouble(vch.ExpWt) : Convert.ToDouble(vch.PayableWt1);
                    Payablewt1 = Convert.ToDouble(MPayablewt1 / PoDetail.ItemDivUom);
                    Rate = Convert.ToDouble(PoDetail.Rate);
                    BrokerComm = Convert.ToDouble(PoDetail.BrokerComm);
                    BrokerCommUom = PoDetail.BrokerCommUom;
                    Gamount = (Payablewt1 * Rate);
                    Commission = Math.Round(GetCommAmount(Convert.ToDouble(PoDetail.BrokerComm), PoDetail.BrokerCommUom, Convert.ToInt32((vch.ExpWt > 0) ? vch.Sbags : vch.Bags), Gamount, MPayablewt1), 0);
                    SalesTaxAmt = Math.Round(GetTaxAmount(Gamount, Convert.ToDouble(PoDetail.SaleTax)), 0);
                    fedamt = Math.Round(GetTaxAmount(Convert.ToDouble(Commission), Convert.ToDouble(PoDetail.IncomeTax)), 0);
                    AmountWithComm = Gamount + Commission;


                    Bagsrate1 = GetBagsRate(AmountWithComm, MPayablewt1, Convert.ToDouble(vch.Bags1), Convert.ToDouble(vch.Ded1), vch.BagsType);
                    Bagsrate2 = GetBagsRate(AmountWithComm, MPayablewt1, Convert.ToDouble(vch.Bags2), Convert.ToDouble(vch.Ded1), vch.BagsType);
                    Bagsrate3 = GetBagsRate(AmountWithComm, MPayablewt1, Convert.ToDouble(vch.Bags3), Convert.ToDouble(vch.Ded1), vch.BagsType);

                    Bagsamt1 = Math.Round(Convert.ToDouble(vch.Bags1 * Bagsrate1), 0);
                    Bagsamt2 = Math.Round(Convert.ToDouble(vch.Bags2 * Bagsrate2), 0);
                    Bagsamt3 = Math.Round(Convert.ToDouble(vch.Bags3 * Bagsrate3), 0);
                    Amount = Gamount - Convert.ToDouble((Bagsamt1 + Bagsamt2 + Bagsamt3));



                }
                else
                {
                    BrokerCode = "";
                    MPayablewt1 = (Convert.ToDouble(vch.ExpWt) > 0) ? Convert.ToDouble(vch.ExpWt) : Convert.ToDouble(vch.PayableWt1);
                    Payablewt1 = Convert.ToDouble(MPayablewt1 / uom.Divuom);
                    Rate = Convert.ToDouble(vch.Rate);
                    BrokerComm = 0;
                    BrokerCommUom = "";
                    Gamount = (Payablewt1 * Rate);
                    Commission = 0;
                    SalesTaxAmt = 0;
                    fedamt = 0;
                    AmountWithComm = Gamount + Commission;

                    Bagsrate1 = GetBagsRate(AmountWithComm, MPayablewt1, Convert.ToDouble(vch.Bags1), Convert.ToDouble(vch.Ded1), vch.BagsType);
                    Bagsrate2 = GetBagsRate(AmountWithComm, MPayablewt1, Convert.ToDouble(vch.Bags2), Convert.ToDouble(vch.Ded1), vch.BagsType);
                    Bagsrate3 = GetBagsRate(AmountWithComm, MPayablewt1, Convert.ToDouble(vch.Bags3), Convert.ToDouble(vch.Ded1), vch.BagsType);
                    Bagsamt1 = Math.Round(Convert.ToDouble(vch.Bags1 * Bagsrate1), 0);
                    Bagsamt2 = Math.Round(Convert.ToDouble(vch.Bags2 * Bagsrate2), 0);
                    Bagsamt3 = Math.Round(Convert.ToDouble(vch.Bags3 * Bagsrate3), 0);
                    Amount = Gamount - Convert.ToDouble((Bagsamt1 + Bagsamt2 + Bagsamt3));
                }






                vch.Rate = Rate;
                vch.Debit = Amount;
                vch.SalesTax = Convert.ToInt32(SalesTaxAmt);
                vch.Fed = Convert.ToInt32(fedamt);
                vch.SalesTaxrate = (decimal)(PoDetail?.SaleTax ?? 0);
                vch.Fedrate = (int)(PoDetail?.IncomeTax ?? 0);
                vch.Comm = Convert.ToInt32(BrokerComm);
                vch.CommType = BrokerCommUom;
                vch.Commission= Convert.ToInt32(Commission);

                vch.Credit = 0;
                vch.WorkDone = true;
                vch.Bg1 = Bagsrate1;
                vch.Bg2 = Bagsrate2;
                vch.Bg3 = Bagsrate3;
                vch.SubCode = (BrokerCode == "" ? firstRow.Mcode : BrokerCode);
                vch.Uid = auth.UserId.ToString();
                _context.TblTransVches.Update(vch);


                decimal avgrate = Convert.ToDecimal(vch.Debit) / Convert.ToDecimal(vch.Qty);


                string Des8 = Partyinfo.Names + "Veh#: " + firstRow.VehicleNo + "Grn#:" + firstRow.VchNo + "Bill#:" + vch.SubName + " PO#:" + vch.Pono + "Qty:" + vch.Qty + uom.Uom + " @" + avgrate.ToString("0.0000") + " Bags:" + vch.Bags + " ";

                string Des88 = Partyinfo.Names + "Veh#: " + firstRow.VehicleNo + "Grn#:" + firstRow.VchNo + "Bill#:" + vch.SubName + " PO#:" + vch.Pono + "Qty:" + vch.PayableWt1 + uom.Uom + " @" + vch.Rate + " Bags:" + vch.Bags + " ";

                DesComm = "Commission " + PoDetail.BrokerComm + "@" + PoDetail.BrokerCommUom.Trim() + "-" + Partyinfo.Names + "- Veh: " + firstRow.VehicleNo + "- GRN#: " + vch.Gpno + "- BILL#: " + vch.SubName + "- PONO#: " + vch.Pono;
                Des9 = Des9 + Des88;
                vch.Descrp = Des8;

                Tqty += Convert.ToDecimal(vch.Qty);
                TPayableWt1 += Convert.ToDecimal(vch.PayableWt1);

                Tamount += Gamount;
                TCommission += Commission;
                TSalesTaxAmt += SalesTaxAmt;
                Tfedamt += fedamt;
                sno += 1;

            }


            if (TSalesTaxAmt > 0)
            {
                sno += 1;

                _context.TblTransVches.Add(new TblTransVch
                {
                    VchType = firstRow.VchType,
                    VehicleNo = firstRow.VehicleNo,
                    BilltyNo = firstRow.BilltyNo,
                    Gpno = firstRow.Gpno,
                    Sno = sno,
                    VchNo = firstRow.VchNo,
                    VchDate = firstRow.VchDate,
                    Dmcode = auth.InputSaleTaxCode.Substring(0, 9),
                    Code = auth.InputSaleTaxCode.Substring(9, 5),
                    Mcode = firstRow.Mcode,
                    Qty = 0,
                    Debit = TSalesTaxAmt,
                    Credit = 0,
                    Descrp = "Sales Tax",
                    FinId = auth.FinId,
                    LocId = auth.LocId,
                    CmpId = auth.CmpId,
                    Tucks = 5,
                    FirstWeight = 0,
                    SecWeight = 0,
                    WorkDone = true,
                    Uid = auth.UserId.ToString(),


            });
                Tamount = Tamount + TSalesTaxAmt;

            }



            if (Tfedamt > 0)
            {
                string Feddes = "Income Tax / Fed " + firstRow.Fedrate + "%@" + TCommission + " - "   + Partyinfo.Names + "- Veh: " + firstRow.VehicleNo + "- GRN#: " + firstRow.Gpno + "- BILL#: " + firstRow.SubName + "- PONO#: " + firstRow.Pono;

                sno += 1;
                _context.TblTransVches.Add(new TblTransVch
                {
                    VchType = firstRow.VchType,
                    VehicleNo = firstRow.VehicleNo,
                    BilltyNo = firstRow.BilltyNo,
                    Gpno = firstRow.Gpno,
                    Sno = sno,
                    VchNo = firstRow.VchNo,
                    VchDate = firstRow.VchDate,
                    Dmcode = auth.WHTaxCode.Substring(0, 9),
                    Code = auth.WHTaxCode.Substring(9, 5),
                    Mcode = firstRow.Mcode,
                    Qty = 0,
                    Debit = 0,
                    Credit = Tfedamt,
                    Descrp = Feddes,
                    FinId = auth.FinId,
                    LocId = firstRow.LocId,
                    CmpId = auth.CmpId,
                    Tucks = 5,
                    FirstWeight = 0,
                    SecWeight = 0,
                    WorkDone = true,
                    Uid = auth.UserId.ToString(),

                });


                sno += 1;
                _context.TblTransVches.Add(new TblTransVch
                {
                    VchType = firstRow.VchType,
                    VehicleNo = firstRow.VehicleNo,
                    BilltyNo = firstRow.BilltyNo,
                    Gpno = firstRow.Gpno,
                    Sno = sno,
                    VchNo = firstRow.VchNo,
                    VchDate = firstRow.VchDate,
                    Dmcode = (BrokerCode == "" ? firstRow.Mcode : BrokerCode).Substring(0, 9),
                    Code = (BrokerCode == "" ? firstRow.Mcode : BrokerCode).Substring(9, 5),
                    Mcode = firstRow.Mcode,
                    Qty = 0,
                    Debit = Tfedamt,
                    Credit = 0,
                    Descrp = Feddes,
                    FinId = auth.FinId,
                    LocId = firstRow.LocId,
                    CmpId = auth.CmpId,
                    Tucks = 5,
                    FirstWeight = 0,
                    SecWeight = 0,
                    WorkDone = true,
                    Uid = auth.UserId.ToString(),

                });


            }

            if (TCommission > 0)
            {
                sno += 1;



                _context.TblTransVches.Add(new TblTransVch
                {
                    VchType = firstRow.VchType,
                    VehicleNo = firstRow.VehicleNo,
                    BilltyNo = firstRow.BilltyNo,
                    Gpno = firstRow.Gpno,
                    Sno = sno,
                    VchNo = firstRow.VchNo,
                    VchDate = firstRow.VchDate,
                    Dmcode = firstRow.Dmcode,
                    Code = firstRow.Code,
                    Mcode = firstRow.Mcode,
                    Qty = 0,
                    Debit = TCommission,
                    Credit = 0,
                    Descrp = DesComm,
                    FinId = auth.FinId,
                    LocId = auth.LocId,
                    CmpId = auth.CmpId,
                    Tucks = 5,
                    FirstWeight = 0,
                    SecWeight = 0,
                    WorkDone = true,
                    Uid = auth.UserId.ToString(),
                });


                sno += 1;
                _context.TblTransVches.Add(new TblTransVch
                {
                    VchType = firstRow.VchType,
                    VehicleNo = firstRow.VehicleNo,
                    BilltyNo = firstRow.BilltyNo,
                    Gpno = firstRow.Gpno,
                    Sno = sno,
                    VchNo = firstRow.VchNo,
                    VchDate = firstRow.VchDate,
                    Dmcode = (BrokerCode == "" ? firstRow.Mcode : BrokerCode).Substring(0, 9),
                    Code = (BrokerCode == "" ? firstRow.Mcode : BrokerCode).Substring(9, 5),
                    Mcode = firstRow.Mcode,
                    Qty = 0,
                    Debit = 0,
                    Credit = TCommission,
                    Descrp = DesComm,
                    FinId = auth.FinId,
                    LocId = auth.LocId,
                    CmpId = auth.CmpId,
                    Tucks = 5,
                    FirstWeight = 0,
                    SecWeight = 0,
                    WorkDone = true,
                    Uid = auth.UserId.ToString(),
                });




            }






            if (Tamount > 0)
            {
                sno += 1;
                _context.TblTransVches.Add(new TblTransVch
                {
                    VchType = firstRow.VchType,
                    VehicleNo = firstRow.VehicleNo,
                    BilltyNo = firstRow.BilltyNo,
                    Gpno = firstRow.Gpno,
                    Sno = sno,
                    VchNo = firstRow.VchNo,
                    VchDate = firstRow.VchDate,
                    Dmcode = firstRow.Mcode.Substring(0, 9),
                    Code = firstRow.Mcode.Substring(9, 5),
                    Pono = firstRow.Pono,
                    Qty = TPayableWt1,
                    Debit = 0,
                    Credit = Tamount,
                    Descrp = Des9,
                    FinId = auth.FinId,
                    LocId = auth.LocId,
                    CmpId = auth.CmpId,
                    Tucks = 9,
                    FirstWeight = 0,
                    SecWeight = 0,
                    WorkDone = true,
                    Uid = auth.UserId.ToString(),
                });


            }



            if (firstRow.Cmb1 != "" && firstRow.Bags1 > 0)
            {


                string Desbg1 = Partyinfo.Names + "Veh#: " + firstRow.VehicleNo + "Grn#:" + firstRow.Gpno + "PO#:" + firstRow.Pono + "Qty:" + firstRow.Bags1 + "@" + Bagsrate1.ToString();

                _context.TblTransVches.Add(new TblTransVch
                {


                    VchType = firstRow.VchType,
                    VehicleNo = firstRow.VehicleNo,
                    BilltyNo = firstRow.BilltyNo,
                    Gpno = firstRow.Gpno,
                    Sno = 51,
                    VchNo = firstRow.VchNo,
                    VchDate = firstRow.VchDate,

                    Dmcode = firstRow.Cmb1.Substring(0, 9),
                    Code = firstRow.Cmb1.Substring(9, 5),
                    Mcode = firstRow.Mcode,
                    Qty = firstRow.Bags1,
                    Rate = Bagsrate1,
                    Debit = Bagsamt1 == 0 ? 100 : Bagsamt1,
                    Credit = 0,
                    Descrp = Desbg1,
                    FinId = auth.FinId,
                    LocId = auth.LocId,
                    CmpId = auth.CmpId,
                    Tucks = 8,
                    FirstWeight = 0,
                    SecWeight = 0,
                    WorkDone = true,
                    Uid = auth.UserId.ToString(),
                });

            }
            if (firstRow.Cmb2 != "" && firstRow.Bags2 > 0)
            {


                string Desbg2 = Partyinfo.Names + "Veh#: " + firstRow.VehicleNo + "Grn#:" + firstRow.Gpno + "PO#:" + firstRow.Pono + "Qty:" + firstRow.Bags2 + "@" + Bagsrate2.ToString();

                _context.TblTransVches.Add(new TblTransVch
                {


                    VchType = firstRow.VchType,
                    VehicleNo = firstRow.VehicleNo,
                    BilltyNo = firstRow.BilltyNo,
                    Gpno = firstRow.Gpno,
                    Sno = 51,
                    VchNo = firstRow.VchNo,
                    VchDate = firstRow.VchDate,

                    Dmcode = firstRow.Cmb2.Substring(0, 9),
                    Code = firstRow.Cmb2.Substring(9, 5),
                    Mcode = firstRow.Mcode,
                    Qty = firstRow.Bags2,
                    Rate = Bagsrate2,
                    Debit = Bagsamt2 == 0 ? 100 : Bagsamt2,
                    Credit = 0,
                    Descrp = Desbg2,
                    FinId = auth.FinId,
                    LocId = auth.LocId,
                    CmpId = auth.CmpId,
                    Tucks = 8,
                    FirstWeight = 0,
                    SecWeight = 0,
                    WorkDone = true,
                    Uid = auth.UserId.ToString(),
                });

            }
            if (firstRow.Cmb3 != "" && firstRow.Bags3 > 0)
            {


                string Desbg3 = Partyinfo.Names + "Veh#: " + firstRow.VehicleNo + "Grn#:" + firstRow.Gpno + "PO#:" + firstRow.Pono + "Qty:" + firstRow.Bags3 + "@" + Bagsrate3.ToString();

                _context.TblTransVches.Add(new TblTransVch
                {


                    VchType = firstRow.VchType,
                    VehicleNo = firstRow.VehicleNo,
                    BilltyNo = firstRow.BilltyNo,
                    Gpno = firstRow.Gpno,
                    Sno = 51,
                    VchNo = firstRow.VchNo,
                    VchDate = firstRow.VchDate,
                    Dmcode = firstRow.Cmb3.Substring(0, 9),
                    Code = firstRow.Cmb3.Substring(9, 5),
                    Mcode = firstRow.Mcode,
                    Qty = firstRow.Bags3,
                    Rate = Bagsrate3,
                    Debit = Bagsamt3 == 0 ? 100 : Bagsamt3,
                    Credit = 0,
                    Descrp = Desbg3,
                    FinId = auth.FinId,
                    LocId = auth.LocId,
                    CmpId = auth.CmpId,
                    Tucks = 8,
                    FirstWeight = 0,
                    SecWeight = 0,
                    WorkDone = true,
                    Uid = auth.UserId.ToString(),
                });

            }

         

            if (firstRow.Freight > 0 && MakeFreightEntry ==true)
            {
                string DesFrg = "FREIGHT PAID GRN#:" + firstRow.Gpno + " VEH#: " + firstRow.VehicleNo + " PARTY: " + Partyinfo.Names + " ITEM: " + Productinfo.Names + " Transporter: " + firstRow.DriverName + " Billty#: " + firstRow.BilltyNo;


                int VCHNO = (_context.TransMains.Where(x => x.VchType == FreightVchtype && x.CmpId == auth.CmpId && x.FinId == auth.FinId && x.LocId == auth.LocId).Max(x => (int?)x.VchNo) ?? 0) + 1;
                _context.TransMains.Where(x => x.VchType == FreightVchtype && x.VchNo == VCHNO && x.FinId == auth.FinId && x.CmpId == auth.CmpId && x.LocId == auth.LocId).ExecuteDelete();


                _context.TransMains.Add(new TransMain
                {

                    VchType = FreightVchtype,
                    VchNo = VCHNO,
                    VchDateM = firstRow.VchDate,
                    FinId = auth.FinId,
                    LocId = auth.LocId,
                    CmpId = auth.CmpId

                });

                _context.TblTransVches.Where(x => x.VchType == FreightVchtype && x.CmpId == auth.CmpId && x.FinId == auth.FinId && x.LocId == auth.LocId && x.Gpno == firstRow.VchNo).ExecuteDelete();





                int Freight = 0;
                if (firstRow.FreightType == "S")
                {

                    string FreightCode = firstRow.Mcode;

                    if (auth.FreightPayableCode != "")
                    {
                        FreightCode = auth.FreightPayableCode;
                    }
                    _context.TblTransVches.Add(new TblTransVch
                    {
                        VchType = FreightVchtype,
                        VehicleNo = firstRow.VehicleNo,
                        BilltyNo = firstRow.BilltyNo,
                        Gpno = firstRow.VchNo,
                        VchNo = VCHNO,
                        Uid = auth.UserId.ToString(),
                        VchDate = firstRow.VchDate,
                        Dmcode = vches[0].Mcode.Substring(0, 9),
                        Code = vches[0].Mcode.Substring(9, 5),
                        Debit = firstRow.Freight,
                        Credit = 0,
                        Descrp = DesFrg,
                        FinId = auth.FinId,
                        LocId = auth.LocId,
                        CmpId = auth.CmpId,
                        Mcode = auth.CashCode,
                        Tucks = 8,
                   

                    });

                }
                else
                {

                    foreach (var vch in vches)
                    {

                        decimal frgrate = (Convert.ToDecimal(firstRow.Freight) / Tqty);
                        int freight = Convert.ToInt32(Math.Round((double)(frgrate * vch.Qty)));


                        _context.TblTransVches.Add(new TblTransVch
                        {
                            VchType = FreightVchtype,
                            VehicleNo = firstRow.VehicleNo,
                            BilltyNo = firstRow.BilltyNo,
                            Gpno = firstRow.VchNo,
                            VchNo = VCHNO,
                            Uid = auth.UserId.ToString(),
                            VchDate = firstRow.VchDate,
                            Dmcode = vch.Dmcode,
                            Code = vch.Code,
                            Debit = freight,
                            Credit = 0,
                            Descrp = DesFrg,
                            FinId = auth.FinId,
                            LocId = auth.LocId,
                            CmpId = auth.CmpId,
                            Mcode = auth.CashCode,
                           

                        });

                    }

                }



                _context.TblTransVches.Add(new TblTransVch
                {
                    VchType = FreightVchtype,
                    VehicleNo = firstRow.VehicleNo,
                    BilltyNo = firstRow.BilltyNo,
                    Gpno = firstRow.VchNo,
                    VchNo = VCHNO,
                    Uid = auth.UserId.ToString(),
                    VchDate = firstRow.VchDate,
                    Dmcode = auth.CashCode.Substring(0, 9),
                    Code = auth.CashCode.Substring(9, 5), 
                    Debit = 0,
                    Credit = firstRow.Freight,
                    Descrp = DesFrg,
                    FinId = auth.FinId,
                    LocId = auth.LocId,
                    CmpId = auth.CmpId,
                    Tucks = 9,
                    

                }) ;;



            }

            _context.SaveChanges();
        }











        public void MakePurchasePayableWithcalculated(string Vchtype, int? Vchno , string LocId)
        {
            _context.TblTransVches.Where(x => x.VchType == Vchtype && x.VchNo == Vchno && x.FinId == auth.FinId && x.CmpId == auth.CmpId && x.LocId == LocId && x.Sno == 51).ExecuteDelete();
            _context.TblTransVches.Where(x => x.VchType == Vchtype && x.VchNo == Vchno && x.FinId == auth.FinId && x.CmpId == auth.CmpId && x.LocId == LocId && x.Sno != 51 && x.Tucks != 8).ExecuteDelete();
            var vches = _context.TblTransVches.Where(x => x.CmpId == auth.CmpId && x.FinId == auth.FinId && x.VchType == Vchtype && x.LocId == LocId && x.VchNo == Vchno && x.Tucks == 8 && x.Sno != 51).ToList();
            var firstRow = vches.FirstOrDefault();
            var Partyinfo = _context.Level5s.Where(x => x.CompId == auth.CmpId && x.Level4 + x.Level51 == firstRow.Mcode).FirstOrDefault();
            var Productinfo = _context.Level5s.Where(x => x.CompId == auth.CmpId && x.Level4 + x.Level51 == firstRow.Dmcode + firstRow.Code).FirstOrDefault();
            decimal Tqty = 0;
            decimal TPayableWt1 = 0;
            double Tamount = 0;
            double TCommission = 0;
            double TSalesTaxAmt = 0;
            double Tfedamt = 0;

            double? Bagsamt1 = 0;
            double? Bagsamt2 = 0;
            double? Bagsamt3 = 0;
            double? Bagsrate1 = 0;
            double? Bagsrate2 = 0;
            double? Bagsrate3 = 0;
            string SalesTaxCode = auth.Tax1Code;
            string BrokerCode = "00000000000000";
            string FreightVchtype = "CP-FREIGHT";
            string DesComm = "";
            string Des9 = "";
            int sno = 0;
            foreach (var vch in vches)
            {
                double Gamount = 0;
                double Amount = 100;
                double AmountWithComm = 0;
                double Brat1 = 0;
                double Brat2 = 0;
                double Brat3 = 0;
                double WHtaxamt = 0;
                double SalesTaxAmt = 0;
                double fedamt = 0;
                double Rate = 0;
                double MPayablewt1 = 0;
                double Payablewt1 = 0;
                double Commission = 0;
                double BrokerComm = 0;
                string BrokerCommUom = "";
                //locid check pending
                var uom = _context.TblUoms.Where(x => x.CompId == auth.CmpId && x.Id == Convert.ToInt16(vch.Uom)).FirstOrDefault();


                    BrokerCode = vch.SubCode;
                    BrokerComm = Convert.ToDouble(vch.Comm);
                    BrokerCommUom = vch.CommType;
                    Commission = Convert.ToDouble(vch.Commission);
                    SalesTaxAmt = Convert.ToDouble(vch.SalesTax);
                    fedamt = Convert.ToDouble(vch.Fed);




                    Bagsamt1 = Math.Round(Convert.ToDouble(vch.Bags1 * vch.Bg1), 0);
                    Bagsamt2 = Math.Round(Convert.ToDouble(vch.Bags2 * vch.Bg2), 0);
                    Bagsamt3 = Math.Round(Convert.ToDouble(vch.Bags3 * vch.Bg3), 0);
                    Gamount =  Convert.ToDouble(vch.Debit + Bagsamt1 + Bagsamt2 + Bagsamt3);






                decimal avgrate = Convert.ToDecimal(vch.Debit) / Convert.ToDecimal(vch.Qty);


                string Des8 = Partyinfo.Names + "Veh#: " + firstRow.VehicleNo + "Grn#:" + firstRow.VchNo + "Bill#:" + vch.SubName + " PO#:" + vch.Pono + "Qty:" + vch.Qty + uom.Uom + " @" + avgrate.ToString("0.0000") + " Bags:" + vch.Bags + " ";

               // string Des8 = Partyinfo.Names + "Veh#: " + firstRow.VehicleNo + "Grn#:" + firstRow.VchNo + "Bill#:" + vch.SubName + " PO#:" + vch.Pono + "Qty:" + vch.Qty + uom.Uom + " @" + vch.Rate + " Bags:" + vch.Bags + " ";

                string Des88 = Partyinfo.Names + "Veh#: " + firstRow.VehicleNo + "Grn#:" + firstRow.VchNo + "Bill#:" + vch.SubName + " PO#:" + vch.Pono + "Qty:" + vch.PayableWt1 + uom.Uom + " @" + vch.Rate + " Bags:" + vch.Bags + " ";

                DesComm = "Commission " + firstRow.Comm + "@" +  firstRow.CommType.Trim() + "-" + Partyinfo.Names + "- Veh: " + firstRow.VehicleNo + "- GRN#: " + vch.Gpno + "- BILL#: " + vch.SubName + "- PONO#: " + vch.Pono;
                Des9 = Des9 + Des88;
                vch.Descrp = Des8;


               

                Tqty += Convert.ToDecimal(vch.Qty);
                TPayableWt1 += Convert.ToDecimal(vch.PayableWt1);

                Tamount += Gamount;
                TCommission += Commission;
                TSalesTaxAmt += SalesTaxAmt;
                Tfedamt += fedamt;
                sno += 1;

            }


            if (TSalesTaxAmt > 0)
            {
                sno += 1;

                _context.TblTransVches.Add(new TblTransVch
                {
                    VchType = firstRow.VchType,
                    VehicleNo = firstRow.VehicleNo,
                    BilltyNo = firstRow.BilltyNo,
                    Gpno = firstRow.Gpno,
                    Sno = sno,
                    VchNo = firstRow.VchNo,
                    VchDate = firstRow.VchDate,
                    Dmcode = auth.InputSaleTaxCode.Substring(0, 9),
                    Code = auth.InputSaleTaxCode.Substring(9, 5),
                    Mcode = firstRow.Mcode,
                    Qty = 0,
                    Debit = TSalesTaxAmt,
                    Credit = 0,
                    Descrp = "Sales Tax",
                    FinId = auth.FinId,
                    LocId = LocId,
                    CmpId = auth.CmpId,
                    Tucks = 5,
                    FirstWeight = 0,
                    SecWeight = 0,
                    WorkDone = true,
                    Uid = auth.UserId.ToString(),

                });
                Tamount = Tamount + TSalesTaxAmt;

            }





            if (Tfedamt > 0)
            {
                string Feddes = "Income Tax / Fed " + firstRow.Fedrate + "%@" + firstRow.Commission + " - " + Partyinfo.Names + "- Veh: " + firstRow.VehicleNo + "- GRN#: " + firstRow.Gpno + "- BILL#: " + firstRow.SubName + "- PONO#: " + firstRow.Pono;

                sno += 1;
                _context.TblTransVches.Add(new TblTransVch
                {
                    VchType = firstRow.VchType,
                    VehicleNo = firstRow.VehicleNo,
                    BilltyNo = firstRow.BilltyNo,
                    Gpno = firstRow.Gpno,
                    Sno = sno,
                    VchNo = firstRow.VchNo,
                    VchDate = firstRow.VchDate,
                    Dmcode = auth.WHTaxCode.Substring(0, 9),
                    Code = auth.WHTaxCode.Substring(9, 5),
                    Mcode = firstRow.Mcode,
                    Qty = 0,
                    Debit = 0,
                    Credit = Tfedamt,
                    Descrp = Feddes,
                    FinId = auth.FinId,
                    LocId = firstRow.LocId,
                    CmpId = auth.CmpId,
                    Tucks = 5,
                    FirstWeight = 0,
                    SecWeight = 0,
                    WorkDone = true,
                    Uid = auth.UserId.ToString(),

                });


                sno += 1;
                _context.TblTransVches.Add(new TblTransVch
                {
                    VchType = firstRow.VchType,
                    VehicleNo = firstRow.VehicleNo,
                    BilltyNo = firstRow.BilltyNo,
                    Gpno = firstRow.Gpno,
                    Sno = sno,
                    VchNo = firstRow.VchNo,
                    VchDate = firstRow.VchDate,
                    Dmcode = (BrokerCode == "" ? firstRow.Mcode : BrokerCode).Substring(0, 9),
                    Code = (BrokerCode == "" ? firstRow.Mcode : BrokerCode).Substring(9, 5),
                    Mcode = firstRow.Mcode,
                    Qty = 0,
                    Debit = Tfedamt,
                    Credit = 0,
                    Descrp = Feddes,
                    FinId = auth.FinId,
                    LocId = firstRow.LocId,
                    CmpId = auth.CmpId,
                    Tucks = 5,
                    FirstWeight = 0,
                    SecWeight = 0,
                    WorkDone = true,
                    Uid = auth.UserId.ToString(),

                });


            }





            if (TCommission > 0)
            {
                sno += 1;



                _context.TblTransVches.Add(new TblTransVch
                {
                    VchType = firstRow.VchType,
                    VehicleNo = firstRow.VehicleNo,
                    BilltyNo = firstRow.BilltyNo,
                    Gpno = firstRow.Gpno,
                    Sno = sno,
                    VchNo = firstRow.VchNo,
                    VchDate = firstRow.VchDate,
                    Dmcode = firstRow.Dmcode,
                    Code = firstRow.Code,
                    Mcode = firstRow.Mcode,
                    Qty = 0,
                    Debit = TCommission,
                    Credit = 0,
                    Descrp = DesComm,
                    FinId = auth.FinId,
                    LocId = LocId,
                    CmpId = auth.CmpId,
                    Tucks = 5,
                    FirstWeight = 0,
                    SecWeight = 0,
                    WorkDone = true,
                    Uid = auth.UserId.ToString(),
                });


                sno += 1;
                _context.TblTransVches.Add(new TblTransVch
                {
                    VchType = firstRow.VchType,
                    VehicleNo = firstRow.VehicleNo,
                    BilltyNo = firstRow.BilltyNo,
                    Gpno = firstRow.Gpno,
                    Sno = sno,
                    VchNo = firstRow.VchNo,
                    VchDate = firstRow.VchDate,
                    Dmcode = (BrokerCode == "" ? firstRow.Mcode : BrokerCode).Substring(0, 9),
                    Code = (BrokerCode == "" ? firstRow.Mcode : BrokerCode).Substring(9, 5),
                    Mcode = firstRow.Mcode,
                    Qty = 0,
                    Debit = 0,
                    Credit = TCommission,
                    Descrp = DesComm,
                    FinId = auth.FinId,
                    LocId = LocId,
                    CmpId = auth.CmpId,
                    Tucks = 5,
                    FirstWeight = 0,
                    SecWeight = 0,
                    WorkDone = true,
                    Uid = auth.UserId.ToString(),
                });




            }






            if (Tamount > 0)
            {
                sno += 1;
                _context.TblTransVches.Add(new TblTransVch
                {
                    VchType = firstRow.VchType,
                    VehicleNo = firstRow.VehicleNo,
                    BilltyNo = firstRow.BilltyNo,
                    Gpno = firstRow.Gpno,
                    Sno = sno,
                    VchNo = firstRow.VchNo,
                    VchDate = firstRow.VchDate,
                    Dmcode = firstRow.Mcode.Substring(0, 9),
                    Code = firstRow.Mcode.Substring(9, 5),
                    Pono = firstRow.Pono,
                    Qty = TPayableWt1,
                    Debit = 0,
                    Credit = Tamount,
                    Descrp = Des9,
                    FinId = auth.FinId,
                    LocId = LocId,
                    CmpId = auth.CmpId,
                    Tucks = 9,
                    FirstWeight = 0,
                    SecWeight = 0,
                    WorkDone = true,
                    Uid = auth.UserId.ToString(),
                });


            }



            if (firstRow.Cmb1 != "" && firstRow.Bags1 > 0)
            {


                string Desbg1 = Partyinfo.Names + "Veh#: " + firstRow.VehicleNo + "Grn#:" + firstRow.Gpno + "PO#:" + firstRow.Pono + "Qty:" + firstRow.Bags1 + "@" + Bagsrate1.ToString();

                _context.TblTransVches.Add(new TblTransVch
                {


                    VchType = firstRow.VchType,
                    VehicleNo = firstRow.VehicleNo,
                    BilltyNo = firstRow.BilltyNo,
                    Gpno = firstRow.Gpno,
                    Sno = 51,
                    VchNo = firstRow.VchNo,
                    VchDate = firstRow.VchDate,

                    Dmcode = firstRow.Cmb1.Substring(0, 9),
                    Code = firstRow.Cmb1.Substring(9, 5),
                    Mcode = firstRow.Mcode,
                    Qty = firstRow.Bags1,
                    Rate = Bagsrate1,
                    Debit = Bagsamt1 == 0 ? 100 : Bagsamt1,
                    Credit = 0,
                    Descrp = Desbg1,
                    FinId = auth.FinId,
                    LocId = LocId,
                    CmpId = auth.CmpId,
                    Tucks = 8,
                    FirstWeight = 0,
                    SecWeight = 0,
                    WorkDone = true,
                    Uid = auth.UserId.ToString(),
                });

            }
            if (firstRow.Cmb2 != "" && firstRow.Bags2 > 0)
            {


                string Desbg2 = Partyinfo.Names + "Veh#: " + firstRow.VehicleNo + "Grn#:" + firstRow.Gpno + "PO#:" + firstRow.Pono + "Qty:" + firstRow.Bags2 + "@" + Bagsrate2.ToString();

                _context.TblTransVches.Add(new TblTransVch
                {


                    VchType = firstRow.VchType,
                    VehicleNo = firstRow.VehicleNo,
                    BilltyNo = firstRow.BilltyNo,
                    Gpno = firstRow.Gpno,
                    Sno = 51,
                    VchNo = firstRow.VchNo,
                    VchDate = firstRow.VchDate,

                    Dmcode = firstRow.Cmb2.Substring(0, 9),
                    Code = firstRow.Cmb2.Substring(9, 5),
                    Mcode = firstRow.Mcode,
                    Qty = firstRow.Bags2,
                    Rate = Bagsrate2,
                    Debit = Bagsamt2 == 0 ? 100 : Bagsamt2,
                    Credit = 0,
                    Descrp = Desbg2,
                    FinId = auth.FinId,
                    LocId = LocId,
                    CmpId = auth.CmpId,
                    Tucks = 8,
                    FirstWeight = 0,
                    SecWeight = 0,
                    WorkDone = true,
                    Uid = auth.UserId.ToString(),
                });

            }
            if (firstRow.Cmb3 != "" && firstRow.Bags3 > 0)
            {


                string Desbg3 = Partyinfo.Names + "Veh#: " + firstRow.VehicleNo + "Grn#:" + firstRow.Gpno + "PO#:" + firstRow.Pono + "Qty:" + firstRow.Bags3 + "@" + Bagsrate3.ToString();

                _context.TblTransVches.Add(new TblTransVch
                {


                    VchType = firstRow.VchType,
                    VehicleNo = firstRow.VehicleNo,
                    BilltyNo = firstRow.BilltyNo,
                    Gpno = firstRow.Gpno,
                    Sno = 51,
                    VchNo = firstRow.VchNo,
                    VchDate = firstRow.VchDate,
                    Dmcode = firstRow.Cmb3.Substring(0, 9),
                    Code = firstRow.Cmb3.Substring(9, 5),
                    Mcode = firstRow.Mcode,
                    Qty = firstRow.Bags3,
                    Rate = Bagsrate3,
                    Debit = Bagsamt3 == 0 ? 100 : Bagsamt3,
                    Credit = 0,
                    Descrp = Desbg3,
                    FinId = auth.FinId,
                    LocId =  LocId,
                    CmpId = auth.CmpId,
                    Tucks = 8,
                    FirstWeight = 0,
                    SecWeight = 0,
                    WorkDone = true,
                    Uid = auth.UserId.ToString(),
                });

            }

            //string DesFrg = "FREIGHT PAID GRN#:" + firstRow.Gpno + " VEH#: " + firstRow.VehicleNo + " PARTY: " + Partyinfo.Names + " ITEM: " + Productinfo.Names + " Transporter: " + firstRow.DriverName + " Billty#: " + firstRow.BilltyNo;

            //if (firstRow.Freight > 0)
            //{


            //    int VCHNO = (_context.TransMains.Where(x => x.VchType == FreightVchtype && x.CmpId == auth.CmpId && x.FinId == auth.FinId && x.LocId == LocId).Max(x => (int?)x.VchNo) ?? 0) + 1;
            //    _context.TransMains.Where(x => x.VchType == FreightVchtype && x.VchNo == VCHNO && x.FinId == auth.FinId && x.CmpId == auth.CmpId && x.LocId == LocId).ExecuteDelete();


            //    _context.TransMains.Add(new TransMain
            //    {

            //        VchType = FreightVchtype,
            //        VchNo = VCHNO,
            //        VchDateM = firstRow.VchDate,
            //        FinId = auth.FinId,
            //        LocId = LocId,
            //        CmpId = auth.CmpId

            //    });

            //    _context.TblTransVches.Where(x => x.VchType == FreightVchtype && x.CmpId == auth.CmpId && x.FinId == auth.FinId && x.LocId == LocId && x.Gpno == firstRow.VchNo).ExecuteDelete();





            //    int Freight = 0;
            //    if (firstRow.FreightType == "S")
            //    {

            //        string FreightCode = firstRow.Mcode;

            //        if (auth.FreightPayableCode != "")
            //        {
            //            FreightCode = auth.FreightPayableCode;
            //        }
            //        _context.TblTransVches.Add(new TblTransVch
            //        {
            //            VchType = FreightVchtype,
            //            VehicleNo = firstRow.VehicleNo,
            //            BilltyNo = firstRow.BilltyNo,
            //            Gpno = firstRow.VchNo,
            //            VchNo = VCHNO,
            //            Uid = auth.UserId.ToString(),
            //            VchDate = firstRow.VchDate,
            //            Dmcode = vches[0].Mcode.Substring(0, 9),
            //            Code = vches[0].Mcode.Substring(9, 5),
            //            Debit = firstRow.Freight,
            //            Credit = 0,
            //            Descrp = DesFrg,
            //            FinId = auth.FinId,
            //            LocId = LocId,
            //            CmpId = auth.CmpId,
            //            Mcode = auth.CashCode,
            //            Tucks = 8,
                  

            //        });

            //    }
            //    else
            //    {
                   
            //        foreach (var vch in vches)
            //        {
            //            decimal frgrate = (Convert.ToDecimal(firstRow.Freight) / Tqty);
            //            int freight = Convert.ToInt32(Math.Round((double)(frgrate * vch.Qty)));



            //            _context.TblTransVches.Add(new TblTransVch
            //            {
            //                VchType = FreightVchtype,
            //                VehicleNo = firstRow.VehicleNo,
            //                BilltyNo = firstRow.BilltyNo,
            //                Gpno = firstRow.VchNo,
            //                VchNo = VCHNO,
            //                Uid = auth.UserId.ToString(),
            //                VchDate = firstRow.VchDate,
            //                Dmcode = vch.Dmcode,
            //                Code = vch.Code,
            //                Debit = freight,
            //                Credit = 0,
            //                Descrp = DesFrg,
            //                FinId = auth.FinId,
            //                LocId = LocId,
            //                CmpId = auth.CmpId,
            //                Mcode = auth.CashCode,
            //                Tucks = 8,
                          

            //            });

            //        }

            //    }



            //    _context.TblTransVches.Add(new TblTransVch
            //    {
            //        VchType = FreightVchtype,
            //        VehicleNo = firstRow.VehicleNo,
            //        BilltyNo = firstRow.BilltyNo,
            //        Gpno = firstRow.VchNo,
            //        VchNo = VCHNO,
            //        Uid = auth.UserId.ToString(),
            //        VchDate = firstRow.VchDate,
            //        Dmcode = auth.CashCode.Substring(0, 9),
            //        Code = auth.CashCode.Substring(9, 5),
            //        Debit = 0,
            //        Credit = firstRow.Freight,
            //        Descrp = DesFrg,
            //        FinId = auth.FinId,
            //        LocId = LocId,
            //        CmpId = auth.CmpId,
            //        Tucks = 9,
                

            //    }); ;



            //}

            _context.SaveChanges();
        }


        public static double GetCommAmount(double mComm, string mCommType, int mBags, double PayableValue, double mPayableQty)
        {
            double mCommAmount = 0;

            if (mComm > 0)
            {
                if (mCommType.Trim() == "Amount %")
                {
                    mCommAmount = Math.Round(((PayableValue / 100) * mComm), 0);
                }
                else if (mCommType.Trim() == "Bags")
                {
                    mCommAmount = Math.Round((mBags * mComm), 0);
                }
                else if (mCommType.Trim() == "40Kgs")
                {
                    mCommAmount = Math.Round(((mPayableQty / 40) * mComm), 0);
                }
                else if (mCommType.Trim() == "Kgs")
                {
                    mCommAmount = Math.Round(((mPayableQty) * mComm), 0);
                }
            }
            return mCommAmount;
        }


        public static double GetTaxAmount(double Amount, double TaxRate)
        {
            double TaxAmount = 0;

            if (Amount > 0 && TaxRate > 0)
            {
                TaxAmount = Math.Round((Amount / 100 * TaxRate), 0);
            }
            return TaxAmount;
        }




        public static double GetBagsRate(double PayableAmount, double PayableQty, double SelectedBags, double BagsDed, string BagsType)
        {
            if (PayableAmount != 0 && PayableQty != 0 && SelectedBags != 0)
            {
                double mBagswt;
                double mBagrate;

                mBagswt = SelectedBags * BagsDed;
                mBagrate = Math.Round((PayableAmount / PayableQty * mBagswt), 0);
                mBagrate = Math.Round((mBagrate / SelectedBags), 2);

                if (BagsType != "P")
                {
                    return mBagrate;
                }
                else
                {
                    return 0;
                }
            }
            return 0;
        }
    }
}
