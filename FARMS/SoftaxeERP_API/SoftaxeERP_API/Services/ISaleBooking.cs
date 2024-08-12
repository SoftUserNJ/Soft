using DevExpress.CodeParser;
using DevExpress.XtraReports.Web.WebDocumentViewer.Native.DataContracts;
using Microsoft.EntityFrameworkCore;
using SoftaxeERP_API.Models;
using SoftaxeERP_API.VM;
using System.Data;

namespace SoftaxeERP_API.Services
{
    public interface ISaleBooking
    {

        #region Sale Booking Entry

        DataTable GetSaleBookingList(DateTime fromDate, DateTime toDate);
        bool SaveSaleBookingEntry(List<SaleBookingVM> saleBooking);
        bool DelSaleBookingEntry(int vchNo);
        DataTable GetEditSaleBookingEntry(int vchNo);

        #endregion

        #region Party Terms and Conditions
        bool SaveTermsConditions(List<TermConditionsVM> TC);
        DataTable GetEditTermsConditions(int vchNo);

        DataTable GetPartyTermsList();

        bool DelTermsConditions(int vchNo);

        #endregion
        #region MRR
        DataTable GetMRRDetail(DateTime FromDate, DateTime toDate);
        #endregion
    }

    public class SaleBooking : ISaleBooking
    {

        private readonly ErpSoftaxeContext _context;
        private readonly IDataLogic _dataLogic;
        private readonly IAuth _auth;

        readonly AuthVM auth = new();
        public SaleBooking(ErpSoftaxeContext context, IDataLogic dataLogic, IAuth authData)
        {
            _context = context;
            _dataLogic = dataLogic;
            _auth = authData;

            auth = _auth.GetUserData();
        }

        #region Sale Booking Entry

        public DataTable GetSaleBookingList(DateTime fromDate, DateTime toDate)
        {
            String qry = $@"IF OBJECT_ID('tempdb..#NEWTBLBOOKING') IS NOT NULL DROP TABLE #NEWTBLBOOKING 
                            SELECT VCHTYPE, BOOKINGNO AS VCHNO, CONVERT(VARCHAR(10),BOOKINGDATE,103) AS VCHDATE INTO #NEWTBLBOOKING
                            FROM TBLBOOKING WHERE BOOKINGDATE BETWEEN '{fromDate.ToString("yyyy/MM/dd")}'
                            AND '{toDate.ToString("yyyy/MM/dd")}' AND VCHTYPE = 'Booking' 
                            AND CMPID = '{auth.CmpId}' AND LOCID = '{auth.LocId}' AND FINID = '{auth.FinId}' 
                            SELECT DISTINCT * FROM #NEWTBLBOOKING ORDER BY VCHNO";
            return _dataLogic.LoadData(qry);
        }


        public bool SaveSaleBookingEntry(List<SaleBookingVM> saleBooking)
        {
            SaleBookingVM vch = saleBooking.First();
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                if (vch.VchNo == 0)
                {
                    vch.VchNo = (_context.Tblbookings
                        .Where(x => x.VchType == "Booking" && x.CmpId == auth.CmpId && x.Finid == auth.FinId && x.Locid == auth.LocId)
                        .Max(x => (int?)x.Bookingno) ?? 0) + 1;
                }

                _context.Tblbookings
                    .Where(x => x.VchType == "Booking" && x.CmpId == auth.CmpId && x.Finid == auth.FinId && x.Locid == auth.LocId && x.Bookingno == vch.VchNo)
                    .ExecuteDelete();

                foreach (var item in saleBooking)
                {
                    _context.Tblbookings.Add(new Tblbooking
                    {
                        VchType = "Booking",
                        Bookingno = vch.VchNo,
                        Bookingdate = vch.Date,

                        Cropyear = item.CropYear,
                        DeliveryTerm = vch.DeliveryTerm,
                        PaymentTerm = vch.PaymentTerm,
                        InvoiceType = vch.InvoiceType,
                        BrokerCode = vch.Broker,
                        Mcode = vch.Party,
                        Code = item.Item,
                        Qty = item.Qty,
                        Rate = item.Rate,
                        Amount = item.Amount,
                        BrokerComm = vch.BrokerComm,
                        BrokerCommUom = vch.BrokerUOM,
                        Uom = vch.RateUom,
                        Remarks = vch.Remarks,
                        Sno = (saleBooking.IndexOf(item) + 1).ToString(),

                        Finid = auth.FinId,
                        Locid = auth.LocId,
                        CmpId = auth.CmpId,
                    });
                }

                _context.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }
        }

        public bool DelSaleBookingEntry(int vchNo)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.Tblbookings.Where(x => x.VchType == "Booking" && x.Bookingno == vchNo && x.CmpId == auth.CmpId && x.Locid == auth.LocId && x.Finid == auth.FinId).ExecuteDelete();
                _context.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }
        }

        public DataTable GetEditSaleBookingEntry(int vchNo)
        {
            String qry = $@"SELECT L5.NAMES AS ITEMNAME,
                            B.VCHTYPE, B.BOOKINGNO AS VCHNO, CONVERT(VARCHAR(10),BOOKINGDATE,103) AS VCHDATE,
                            B.QTY, B.RATE, B.REMARKS, B.MCODE AS PARTYCODE, B.CODE AS ITEMCODE,
                            B.CROPYEAR, B.DELIVERYTERM, B.PAYMENTTERM, B.INVOICETYPE, B.BROKERCODE, B.BROKERCOMM, B.BROKERCOMMUOM, B.UOM
                            FROM TBLBOOKING B
                            INNER JOIN LEVEL5 L5 ON L5.LEVEL4+L5.LEVEL5 = B.CODE AND B.CMPID = L5.COMP_ID AND B.LOCID = L5.LOCID
                            WHERE B.CMPID = '{auth.CmpId}' AND B.LOCID = '{auth.LocId}' AND B.FINID = '{auth.FinId}' AND
                            B.BOOKINGNO = '{vchNo}' AND B.VCHTYPE = 'BOOKING' ORDER BY B.BOOKINGNO";

            return _dataLogic.LoadData(qry);
        }



        #endregion


        #region Party Terms and Condition




        public bool SaveTermsConditions(List<TermConditionsVM> TC)
        {
            TermConditionsVM tc = TC.First();
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var lb = _context.TblPartyTcs.Where(x => x.Id == tc.Id && x.CmpId == auth.CmpId && x.LocId == auth.LocId).FirstOrDefault();
                if (lb == null)
                {
                    tc.Id = (_context.TblPartyTcs
                    .Where(x => x.CmpId == auth.CmpId && x.LocId == auth.LocId)
                       .Max(x => (int?)x.Id) ?? 0) + 1;
                }



                _context.TblPartyTcs
                .Where(x => x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.Id == tc.Id)
                .ExecuteDelete();

               var Level5 = _context.Level5s.Where(x => x.Level4 + x.Level51 == tc.Code && x.CompId == auth.CmpId && x.LocId == auth.LocId).FirstOrDefault();

                if (Level5 != null)
                {
                    Level5.RateDiff = tc.RateDiff;
                    Level5.RateChoice = tc.RateChoice;
                    _context.Update(Level5);
                }


                foreach (var item in TC)
                    {
                        _context.TblPartyTcs.Add(new TblPartyTc
                        {
                            Id = tc.Id,
                            DmCode = item.DmCode,
                            Code = item.Code.Substring(9, 5),
                            MonthDisc = item.MonthDisc,
                            Bonus = item.Bonus,
                            Pbwandaonly = item.Pbwandaonly,
                            AllowWanda = item.AllowWanda,
                            Remarks = item.Remarks,
                            Con = item.Con,
                            Disc1 = item.Disc1,
                            Disc2 = item.Disc2,
                            Disc3 = item.Disc3,
                            Disc4 =item.Disc4,
                            Disc5 =item.Disc5,
                            Disc6 = item.Disc6,
                            Disc11 = item.Disc11,
                            Disc22 = item.Disc22,
                            Disc444 = item.Disc444,
                            Disc1111 = item.Disc1111,
                            Disc2222 = item.Disc2222,
                            Disc4444 = item.Disc4444,
                            Formula1 = item.Formula1,
                            Formula2 = item.Formula2,
                            Formula3 = item.Formula3,
                            Formula4 = item.Formula4,
                            Formula5 = item.Formula5,
                            Formula6 = item.Formula6,
                            CmpId = auth.CmpId,
                            LocId = auth.LocId,
                            Uid = auth.UserId
                        });
                    }
                  


                _context.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
            }
        }


        public DataTable GetEditTermsConditions(int vchNo)
        {
            String qry = @"SELECT Id, DmCode, Code, T.Disc1, Disc11, Formula1, P.Names as SubParty, L4.Names AS MainParty, T.Disc2, Formula2, T.Disc3, Formula3, T.Disc4, Disc44, Disc444,
                            Disc4444, Formula4, T.Disc5, Formula5,  P.RateDiff, P.RateChoice, T.Disc6, Formula6, T.Disc7, Formula7, Remarks, AllowWanda, Disc1111, Disc111,
                            Disc2222, Disc222, Disc22, Con, T.Bonus, PBWandaonly, MonthDisc  FROM TblPartyTC T
                            lEFT Join Level5 P ON P.Level4+P.Level5 = T.DmCode + Code and P.Comp_id = T.Cmp_id
                            Left Join Level4 L4 ON L4.Level3+L4.Level4 = T.DmCode and L4.comp_id = T.Cmp_id 
                            where T.Cmp_Id = '" + auth.CmpId+"' and T.LocId = '"+ auth.LocId + "' and T.Id = '"+ vchNo + "'";

            return _dataLogic.LoadData(qry);
        }

        public DataTable GetPartyTermsList()
        {
            String qry = @"
                        SELECT R.DmCode, R.Code, Id, L.Names as SubParty, L4.Names as PartyMain, isnull(R.Aprove, 0) as Approve, Con, R.Bonus, MonthDisc from  TblPartyTC  R
                        Left Join Level5 L on L.Level4+Level5 = R.DmCode+R.Code and L.comp_id = R.Cmp_id
                        Left Join Level4 L4 ON L4.Level3+L4.Level4 = R.DmCode and L4.comp_id = R.Cmp_id
                        where R.Cmp_id = '"+auth.CmpId+"' and R.LocId = '"+auth.LocId+"'";
            return _dataLogic.LoadData(qry);
        }


        public bool DelTermsConditions(int vchNo)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.TblPartyTcs.Where(x => x.Id == vchNo && x.CmpId == auth.CmpId && x.LocId == auth.LocId).ExecuteDelete();

                _context.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }
        }

        #endregion


        #region Material Receivng Report 

        public DataTable GetMRRDetail(DateTime FromDate, DateTime toDate)
        {
            String qry = @"[RptWbInward] 'RP-RAW', '0','999999999', '0', '9999999999',
            '"+ FromDate.ToString("yyyy/MM/dd") + "', '"+toDate.ToString("yyyy/MM/dd") +"', 1, 1, 'U1', '0', '99999999999999', '0', '99999999999999'";
            return _dataLogic.LoadData(qry);
        }

        #endregion
    }
}
