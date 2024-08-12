using SoftaxeERP_API.Models;
using SoftaxeERP_API.VM;
using System.Data;

namespace SoftaxeERP_API.Services
{
    public interface IReports
    {
        #region Expense Budget Report
        DataTable GetExpenseBudgetReport(string VchType, string VchNoFrom, string VchNoTo, string PFrom, string PTo, DateTime fromDate, int FinId, string LocId);
        #endregion

        #region Cheque Availability Report
        DataTable GetChequeAvailability(string LocId, string filterBy);
        #endregion

        #region Cheque Books Report

        DataTable GetChqBooksReport(int FinId, string LocId, string BFrom, string BTo, DateTime FromDate, DateTime ToDate, int CompId);

        #endregion

        #region Purchase Contract Detail

        DataTable GetPurchaseContractDetail(DateTime FromDate, DateTime ToDate);

        #endregion

        #region Purchase Contract Report

        DataTable GetPurchaseContractReport(string VchType, string PFrom, string PTo, string IFrom, string ITo, int FinId, string LocId, DateTime FromDate, DateTime ToDate, string Category, string cancel);

        #endregion

        #region Purchase Detail Report

        DataTable GetPurchaseDetailReport(string VchType, int VchnoFrom, string PFrom, string PTo, string IFrom, string ITo, int FinId, string LocId);

        #endregion

        #region Purchase Contract Canceled Report

        DataTable GetPurContractCanceledReport(string VchType, string PFrom, string PTo, string IFrom, string ITo, int FinId, string LocId, DateTime FromDate, DateTime ToDate, string Category);

        #endregion

        #region Purchase Contract Status Report

        DataTable GetPurContractStatusReport(string VchType, string PFrom, string PTo, string IFrom, string ITo, int FinId, string LocId, DateTime FromDate, DateTime ToDate, string Category);

        #endregion

        #region Purchase Contract Pending Report

        DataTable GetPurContractPendingReport(string VchType, string PFrom, string PTo, string IFrom, string ITo, int FinId, string LocId, DateTime FromDate, DateTime ToDate, string Category, string cancel);

        #endregion

        #region Product Wise Sale Report

        DataTable GetProductWiseSaleReport(string VchType, string InwardType, string VchNoFrom, string VchNoTo, string CatFrom, string CatTo, string PFrom, string PTo, string IFrom, string ITo, DateTime FromDate, DateTime ToDate, int FinId, string LocId);

        #endregion

        #region Product Party Wise Sale Report

        DataTable GetProdPartyWiseSaleReport(string VchType, string InwardType, string VchNoFrom, string VchNoTo, string CatFrom, string CatTo, string PFrom, string PTo, string IFrom, string ITo, DateTime FromDate, DateTime ToDate, int FinId, string LocId);

        #endregion

        #region Daily Product Wise Sale Report

        DataTable GetDailyProductWiseSaleReport(string VchType, string InwardType, string VchNoFrom, string VchNoTo, string CatFrom, string CatTo, string PFrom, string PTo, string IFrom, string ITo, DateTime FromDate, DateTime ToDate, int FinId, string LocId);

        #endregion

        #region Daily Weight Bridge Inward Report

        DataTable GetWeightBridgeInwardReport(string VchType, string VchNoFrom, string VchNoTo, string CatFrom, string CatTo, DateTime FromDate, DateTime ToDate, int FinId, string LocId, string PFrom, string PTo, string IFrom, string ITo);

        #endregion

        #region Daily Purchase Report

        DataTable GetDailyPurchaseReport(DateTime FromDate, DateTime ToDate, string PartyCode, string ItemCode, string Approvel);

        #endregion

        #region Daily Purchase Activity Report

        DataTable GetDailyPurchaseActivityReport(DateTime FromDate, DateTime ToDate, int finid, string locid, int cmpId);

        #endregion

        #region Daily Sale Activity Report

        DataTable GetDailySaleActivityReport(DateTime FromDate, DateTime ToDate, int finid, string locid, int cmpId);

        #endregion

        #region Month Wise Activty
        DataTable GetMonthWiseActivity(DateTime fromYear, DateTime toYear);

        #endregion

        #region Inward Purchase Status

        DataTable GetInwardPurchaseStatusReport(DateTime fromDate, DateTime toDate);


        #endregion

        DataTable GetItemList();
        DataTable GetCostCenter();
        DataTable GetLevel5List(string tag);

    }

    public class Reports : IReports
    {

        private readonly ErpSoftaxeContext _context;
        private readonly IDataLogic _dataLogic;
        private readonly IAuth _auth;

        readonly AuthVM auth = new();
        public Reports(ErpSoftaxeContext context, IDataLogic dataLogic, IAuth authData)
        {
            _context = context;
            _dataLogic = dataLogic;
            _auth = authData;

            auth = _auth.GetUserData();
        }

        #region Expense Budget Report

        public DataTable GetExpenseBudgetReport(string VchType, string VchNoFrom, string VchNoTo, string PFrom, string PTo, DateTime fromDate, int FinId, string LocId)
        {
            String qry = $@" EXEC RptBudget '{VchType}','{VchNoFrom}','{VchNoTo}','{PFrom}','{PTo}','{fromDate.ToString("yyyy/MM/dd")}','{FinId}','{LocId}'";
            return _dataLogic.LoadData(qry);
        }

        // RptBudget


        #endregion

        #region Cheque Availability Report


        public DataTable GetChequeAvailability(string LocId, string filterBy)
        {
            String qry = $@" EXEC RptChqAvailability '{LocId}','{filterBy}'";
            return _dataLogic.LoadData(qry);
        }

        #endregion

        #region Cheque Books Report

        public DataTable GetChqBooksReport(int FinId, string LocId, string BFrom, string BTo, DateTime FromDate, DateTime ToDate, int CompId)
        {
            String qry = $@" EXEC RptChqbooks '{FinId}','{LocId}','{BFrom}','{BTo}','{FromDate}','{ToDate}','{CompId}'";
            return _dataLogic.LoadData(qry);
        }

        #endregion

        #region Purchase Contract Detail

        public DataTable GetPurchaseContractDetail(DateTime FromDate, DateTime ToDate)
        {
            String qry = $@" EXEC RptPurContDetail '{FromDate}','{ToDate}'";
            return _dataLogic.LoadData(qry);
        }

        #endregion

        #region Purchase Contract Report

        public DataTable GetPurchaseContractReport(string VchType, string PFrom, string PTo, string IFrom, string ITo, int FinId, string LocId, DateTime FromDate, DateTime ToDate, string Category, string cancel)
        {
            String qry = $@" EXEC RptPurContract '{VchType}','{PFrom}','{PTo}','{IFrom}','{ITo}','{FinId}','{LocId}','{FromDate.ToString("yyyy/MM/dd")}','{ToDate.ToString("yyyy/MM/dd")}','{Category}','{cancel}'";
            return _dataLogic.LoadData(qry);
        }

        #endregion

        #region Purchase Detail Report

        public DataTable GetPurchaseDetailReport(string VchType, int VchnoFrom, string PFrom, string PTo, string IFrom, string ITo, int FinId, string LocId)
        {
            String qry = $@" EXEC RptPurContractDetail '{VchType}','{VchnoFrom}','{PFrom}','{PTo}','{IFrom}','{ITo}','{FinId}','{LocId}'";
            return _dataLogic.LoadData(qry);
        }

        #endregion

        #region Purchase Contract Canceled Report

        public DataTable GetPurContractCanceledReport(string VchType, string PFrom, string PTo, string IFrom, string ITo, int FinId, string LocId, DateTime FromDate, DateTime ToDate, string Category)
        {
            String qry = $@" EXEC RptPurContractCanceled '{VchType}','{PFrom}','{PTo}','{IFrom}','{ITo}','{FinId}','{LocId}','{FromDate}','{ToDate}','{Category}'";
            return _dataLogic.LoadData(qry);
        }

        #endregion

        #region Purchase Contract Status Report

        public DataTable GetPurContractStatusReport(string VchType, string PFrom, string PTo, string IFrom, string ITo, int FinId, string LocId, DateTime FromDate, DateTime ToDate, string Category)
        {
            String qry = $@" EXEC RptPurContractStatus '{VchType}','{PFrom}','{PTo}','{IFrom}','{ITo}','{FinId}','{LocId}','{FromDate}','{ToDate}','{Category}'";
            return _dataLogic.LoadData(qry);
        }

        #endregion

        #region Purchase Contract Pending Report

        public DataTable GetPurContractPendingReport(string VchType, string PFrom, string PTo, string IFrom, string ITo, int FinId, string LocId, DateTime FromDate, DateTime ToDate, string Category, string cancel)
        {
            String qry = $@" EXEC RptPurContractPending '{VchType}','{PFrom}','{PTo}','{IFrom}','{ITo}','{FinId}','{LocId}','{FromDate}','{ToDate}','{Category}','{cancel}'";
            return _dataLogic.LoadData(qry);
        }

        #endregion

        #region Product Wise Sale Report

        public DataTable GetProductWiseSaleReport(string VchType, string InwardType, string VchNoFrom, string VchNoTo, string CatFrom, string CatTo, string PFrom, string PTo, string IFrom, string ITo, DateTime FromDate, DateTime ToDate, int FinId, string LocId)
        {
            String qry = $@" EXEC RptProdWSale '{VchType}', '{InwardType}', '{VchNoFrom}', '{VchNoTo}', '{CatFrom}', '{CatTo}','{PFrom}','{PTo}','{IFrom}','{ITo}','{FromDate}','{ToDate}','{FinId}','{LocId}'";
            return _dataLogic.LoadData(qry);
        }
        #endregion

        #region Product Party Wise Sale Report

        public DataTable GetProdPartyWiseSaleReport(string VchType, string InwardType, string VchNoFrom, string VchNoTo, string CatFrom, string CatTo, string PFrom, string PTo, string IFrom, string ITo, DateTime FromDate, DateTime ToDate, int FinId, string LocId)
        {
            String qry = $@" EXEC RptProdWSalePI '{VchType}', '{InwardType}', '{VchNoFrom}', '{VchNoTo}', '{CatFrom}', '{CatTo}','{PFrom}','{PTo}','{IFrom}','{ITo}','{FromDate}','{ToDate}','{FinId}','{LocId}'";
            return _dataLogic.LoadData(qry);
        }
        #endregion

        #region Daily Product Wise Sale Report

        public DataTable GetDailyProductWiseSaleReport(string VchType, string InwardType, string VchNoFrom, string VchNoTo, string CatFrom, string CatTo, string PFrom, string PTo, string IFrom, string ITo, DateTime FromDate, DateTime ToDate, int FinId, string LocId)
        {
            String qry = $@" EXEC RptProdWSaleIP '{VchType}', '{InwardType}', '{VchNoFrom}', '{VchNoTo}', '{CatFrom}', '{CatTo}','{PFrom}','{PTo}','{IFrom}','{ITo}','{FromDate}','{ToDate}','{FinId}','{LocId}'";
            return _dataLogic.LoadData(qry);
        }
        #endregion

        #region Daily Weight Bridge Inward Report

        public DataTable GetWeightBridgeInwardReport(string VchType, string VchNoFrom, string VchNoTo, string CatFrom, string CatTo, DateTime FromDate, DateTime ToDate, int FinId, string LocId, string PFrom, string PTo, string IFrom, string ITo)
        {
            String qry = $@" EXEC RptWbInward '{VchType}', '{VchNoFrom}', '{VchNoTo}', '{CatFrom}', '{CatTo}', '{FromDate}','{ToDate}','{FinId}','{LocId}','{PFrom}','{PTo}','{IFrom}','{ITo}'";
            return _dataLogic.LoadData(qry);
        }
        #endregion

        #region Daily Purchase Report

        public DataTable GetDailyPurchaseReport(DateTime FromDate, DateTime ToDate, string PartyCode, string ItemCode, string Approvel)
        {
            String qry = $@" EXEC ItemPurchase '{FromDate}', '{ToDate}', '{PartyCode}', '{ItemCode}', '{Approvel}'";
            return _dataLogic.LoadData(qry);
        }
        #endregion

        #region Daily Sale Activity Report

        public DataTable GetDailySaleActivityReport(DateTime FromDate, DateTime ToDate, int finid, string locid, int cmpId)
        {
            String qry = $@" EXEC RptDailySaleArrival '{FromDate}', '{ToDate}', '{finid}', '{locid}', '{cmpId}'";
            return _dataLogic.LoadData(qry);
        }
        #endregion

        #region Daily Purchase Activity Report

        public DataTable GetDailyPurchaseActivityReport(DateTime FromDate, DateTime ToDate, int finid, string locid, int cmpId)
        {
            String qry = $@" EXEC RptDailyPurchaseArrival '{FromDate}', '{ToDate}', '{finid}', '{locid}', '{cmpId}'";
            return _dataLogic.LoadData(qry);
        }
        #endregion

        public DataTable GetItemList()
        {
            string qry = $@"SELECT L5.LEVEL4 + L5.LEVEL5 AS CODE, L5.NAMES
            FROM LEVEL5 L5
            INNER JOIN LEVEL4 L4 ON L4.LEVEL3 + L4.LEVEL4 = L5.LEVEL4 AND L4.COMP_ID = L5.COMP_ID
            WHERE L4.TAG1 = 'J' AND L5.COMP_ID = {auth.CmpId}
            ORDER BY L5.NAMES";
            return _dataLogic.LoadData(qry);
        }

        public DataTable GetCostCenter()
        {
            string qry = $@"SELECT Cmp_id, LocID ,LocName  FROM Location
                            where Cmp_id= {auth.CmpId}";
            return _dataLogic.LoadData(qry);
        }

        public DataTable GetLevel5List(string tag)
        {
            string qry = $@"SELECT L5.LEVEL4+L5.LEVEL5 CODE, L5.NAMES FROM LEVEL5 L5 
            INNER JOIN LEVEL4 L4 ON L4.LEVEL3+L4.LEVEL4=L5.LEVEL4 AND L4.COMP_ID=L5.COMP_ID
            WHERE L4.TAG IN ('{tag}') AND L5.COMP_ID = {auth.CmpId}";
            return _dataLogic.LoadData(qry);
        }


        #region Month Wise Activty

        public DataTable GetMonthWiseActivity(DateTime fromYear, DateTime toYear)
        {
            String qry = $@" EXEC RptMonthlyAccBal '{fromYear.Year}','{toYear.Year}'";
            return _dataLogic.LoadData(qry);
        }

        // RptMonthlyAccBal

        #endregion


        #region Inward Purchase Status


        public DataTable GetInwardPurchaseStatusReport(DateTime fromDate, DateTime toDate)
        {
            string qry = $@"select CONVERT (varchar(11) , VchDate ,105) Date,vchno ArNo,
                            isnull(gpno,0) GpNo,l5p.names Party,VehicleNo,l5I.names Product, isnull(sbags,0) Sbags,
                            isnull((sbags-bags),'') RejBg,isnull(bags,'') Bags,isnull(qty,0) StockWt,
                            (case when isnull(gpno,0)>0 then (sqty-(isnull(FirstWeight,0)-isnull(secweight,0))) else 0 end) QtDiff,
                            isnull(FirstWeight,0) FirstWeight,isnull(SecWeight,0) SecWeight,
                            isnull(FirstWeight,0)-isnull(SecWeight,0) NetWt,  isnull(gross,0)-isnull(Tare,0) PartyWt , Wtype,isnull(labded,0) Labded,
                            isnull(Payablewt1,0) Payablewt ,isnull(convert(varchar, Timein, 24),'')  Timein,
                            isnull(convert(varchar, Timeout, 24),'')  TimeOut,isnull(Bagsded,0) Bagsded, v.Freight , v.FreightType
                            ,isnull(Reject,0) Reject , FORMAT(vchdate,'yyyy-MM-dd') VchDate , v.Locid,
                            FORMAT(EntryDate, 'dd-MM-yyyy hh:mm:tt') AS EntryDate,
                            FORMAT(DATEADD(day, DATEDIFF(day, 0, DateIn), CAST(timein AS datetime)), 'dd-MM-yyyy hh:mm:tt') AS DateIn,
	                        FORMAT(DATEADD(day, DATEDIFF(day, 0, DateOut), CAST(TimeOut AS datetime)), 'dd-MM-yyyy hh:mm:tt') AS DateOut,
	                        CONVERT(varchar(12), DATEADD(minute,  DATEDIFF(minute, entrydate, DATEADD(day, DATEDIFF(day, 0, DateIn), CAST(timein AS datetime))), 0), 114)  GateDiffWithIn,
	                        CONVERT(varchar(12), DATEADD(minute,  DATEDIFF(minute, DATEADD(day, DATEDIFF(day, 0, DateIn), CAST(TimeIn AS datetime)), DATEADD(day, DATEDIFF(day, 0, DateOut), CAST(DateOut AS datetime))), 0), 114)  InDiffWithOut , 
	                        CONVERT(varchar(12), DATEADD(minute,  DATEDIFF(minute, entrydate, DATEADD(day, DATEDIFF(day, 0, DateOut), CAST(DateOut AS datetime))), 0), 114)  GateDiffWithOut


                            from tbltransvch v 
                            inner join level5 L5p on l5p.level4+l5p.level5=v.mcode and L5p.comp_id = Cmp_Id
                            inner join level5 L5I on l5I.level4+l5I.level5=v.dmcode+v.code and L5I.comp_id = Cmp_Id
                            where  vchtype='rp-raw' and tucks=8  and sno=1 
                            and v.Cmp_Id = '{auth.CmpId}' and  CONVERT(VARCHAR,v.VchDate,111) BETWEEN '{fromDate.ToString("yyyy/MM/dd")}'
                            AND '{toDate.ToString("yyyy/MM/dd")}' order by  vchdate,vchno,gpno";
            return _dataLogic.LoadData(qry);
        }

        #endregion

    }
}
