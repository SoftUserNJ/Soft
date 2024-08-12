using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web.Extensions;
using Inventory_Pos_Softaxe.Reports;
using SoftaxeERP_API.Reports;
using SoftaxeERP_API.Services;

namespace SoftaxeERP_API
{
    public class MyReportStorage : ReportStorageWebExtension
    {
        public Dictionary<string, XtraReport> Reports = new Dictionary<string, XtraReport>();

        public MyReportStorage(IWebHostEnvironment environment, IAuth authData)
        {
            // COMMON REPORTS
            Reports.Add("PrintVoucherRangeWise", new PrintVoucherRangeWise());
            Reports.Add("AccountLedger", new Ledger());
            Reports.Add("Detailledger", new Detailledger());

            // ACCOUNTS REPORTS
            Reports.Add("ChartOfAccount", new ChartOfAccount());
            Reports.Add("TrailBalance1", new TrailBalance1());
            Reports.Add("TrailBalance", new TrailBalance());
            Reports.Add("DayBook", new DayBook());
            Reports.Add("DayBook1", new DayBook1());
            Reports.Add("ProfitLoss", new ProfitLoss());
            Reports.Add("BalanceSheet", new BalanceSheet());

            // INVENTORY REPORTS
            Reports.Add("TransferLoad", new TransferLoad());
            Reports.Add("InventoryList", new InventoryList1());
            Reports.Add("InventoryListSale", new InventoryListSale());
            Reports.Add("ProductLedger", new ProductLedger1());
            Reports.Add("FlockDetail", new FlockDetail());
            Reports.Add("FlockDetail1", new FlockDetail1());


            // SALE PURCHASE COMMON REPORTS
            Reports.Add("Aging", new Aging());
            Reports.Add("PartyPosition", new PartyPosition());
            Reports.Add("PartyPosition1", new PartyPosition1());
            Reports.Add("RptMaterialReceived", new RptMaterialReceived());

            // PURCHASE REPORTS
            Reports.Add("SultaniaPurchaseInvoice", new SultaniaPurchaseInvoice());
            Reports.Add("PurchaseInvoice", new PurchaseInvoice());
            Reports.Add("PurchaseInvoice1", new PurchaseInvoice1());
            Reports.Add("PurchaseReturn", new PurchaseReturn());
            Reports.Add("PurchaseReturn1", new PurchaseReturn1());
            Reports.Add("PurchaseUnloading", new PurchaseUnloading());


            Reports.Add("PurchaseContractReport", new PurchaseContractReport());
            Reports.Add("PurchaseDetailReport", new PurchaseDetailReport());
            Reports.Add("PurchaseContractPendingReport", new PurchaseContractPendingReport());
            Reports.Add("PurchaseContractCanceledReport", new PurchaseContractCanceledReport());
            Reports.Add("PurchaseContractStatusReport", new PurchaseContractStatusReport());
            Reports.Add("PrintPurContract", new PrintPurContract());
            Reports.Add("LabTestSlip", new LabTestSlip());


            // SALE REPORTS
            Reports.Add("FarmLedger", new FarmLedger());
            Reports.Add("SaleInvoice", new SaleInvoice());
            Reports.Add("SaleInvoice1", new SaleInvoice1());
            Reports.Add("SaleReturn", new SaleReturn());
            Reports.Add("SaleReturn1", new SaleReturn1());
            Reports.Add("SaleLoading", new SaleLoading());
            Reports.Add("DoSummary", new DoSummary());
            Reports.Add("LedgerDetail", new LedgerDetail());
            Reports.Add("DailySaleProduct", new DailySaleProduct());
            Reports.Add("DailySaleProductPartyAreaWise", new DailySaleProductPartyAreaWise());
            Reports.Add("ItemWiseSale", new ItemWiseSale());
            Reports.Add("CostParty", new CostParty());
            Reports.Add("CostCategory", new CostCategory());
            Reports.Add("CostProduct", new CostProduct());
            Reports.Add("CostSaleTeam", new CostSaleTeam());
            Reports.Add("SalesManCommission", new SalesManCommission());
            Reports.Add("OtCommission", new OtCommission());
            Reports.Add("PartiesTaxDeduction", new PartiesTaxDeduction());
            Reports.Add("PartiesTaxDeductionSummary", new PartiesTaxDeductionSummary());
            Reports.Add("RptFlockPerformance", new RptFlockPerformance());
            Reports.Add("CashFlow", new CashFlow());
            Reports.Add("CashFlowSubRpt", new CashFlowSubRpt());
            Reports.Add("ProductSaleIssue", new ProductSaleIssue());
            Reports.Add("FlockExpenses", new FlockExpenses());
            Reports.Add("RptSaleContract", new RptSaleContract());

            //PAYROLL REPORTS
            Reports.Add("EmployeeDetailForm", new EmployeeDetailForm());
            Reports.Add("EmployeeSalaryDetail", new EmployeeSalaryDetail());
            Reports.Add("EmpSalarySheet", new EmpSalarySheet());
            Reports.Add("empAdvanceSalary", new empAdvanceSalary());
            Reports.Add("RptStfLoanBlnce", new RptStfLoanBlnce());
            Reports.Add("VehLoanReport", new VehLoanReport());
            Reports.Add("InsuranceLoanReport", new InsuranceLoanReport());
            Reports.Add("RptIncomeEOBIProvDed", new RptIncomeEOBIProvDed());
            Reports.Add("ProvidentLoanReport", new ProvidentLoanReport());
            Reports.Add("RptDesignationListing", new RptDesignationListing());
            Reports.Add("RptDepartmentListing", new RptDepartmentListing());
            Reports.Add("RptProvLoanBalance", new RptProvLoanBalance());
            Reports.Add("RptInsurnceLoanBlnce", new RptInsurnceLoanBlnce());
            Reports.Add("RptVehLoanBalance", new RptVehLoanBalance());

            // Weighment

            Reports.Add("FirstWeightSlip", new FirstWeightSlip());


            Reports.Add("SecondWeightSlip", new SecondWeightSlip());
            Reports.Add("ReceivingOfGoods", new ReceivingOfGoods());


            // Weighment - Sale
            Reports.Add("SecWeightSlipSale", new SecWeightSlipSale());
            Reports.Add("DeliveryChallan", new DeliveryChallan());
            Reports.Add("GatePassOutwardSale", new GatePassOutwardSale());
            Reports.Add("SaleLoadingSlip", new SaleLoadingSlip());
            Reports.Add("DailyArrivalRpt", new DailyArrivalRpt());
            Reports.Add("PartyProdPur", new PartyProdPur());

        }

        public override bool CanSetData(string url)
        {
            return true;
        }

        public override byte[] GetData(string url)
        {
            try
            {
                string[] parts = url.Split("?");
                string reportName = parts[0];
                string parametersString = parts.Length > 1 ? parts[1] : String.Empty;
                XtraReport report = Reports[reportName];

                if (report != null)
                {
                    // Parse query parameters manually
                    Dictionary<string, string> parameters = ParseQueryParameters(parametersString);

                    // Assign parameters here
                    foreach (var parameterName in parameters.Keys)
                    {
                        // Find the parameter by name
                        var parameter = FindParameterByName(report, parameterName);

                        if (parameter != null)
                        {
                            parameter.Value = Convert.ChangeType(parameters[parameterName], parameter.Type);
                        }
                    }

                    report.RequestParameters = false;

                    using (MemoryStream ms = new MemoryStream())
                    {
                        report.SaveLayoutToXml(ms);
                        return ms.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DevExpress.XtraReports.Web.ClientControls.FaultException(
                    "Could not get report data.", ex);
            }

            throw new DevExpress.XtraReports.Web.ClientControls.FaultException(
                string.Format("Could not find report '{0}'.", url));
        }

        // Helper method to parse query parameters manually
        private Dictionary<string, string> ParseQueryParameters(string queryString)
        {
            var parameters = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(queryString))
            {
                string[] keyValuePairs = queryString.Split('&');
                foreach (var keyValuePair in keyValuePairs)
                {
                    string[] parts = keyValuePair.Split('=');
                    if (parts.Length == 2)
                    {
                        string key = parts[0];
                        string value = parts[1];
                        parameters[key] = Uri.UnescapeDataString(value);
                    }
                }
            }
            return parameters;
        }

        // Helper method to find a parameter by name
        private DevExpress.XtraReports.Parameters.Parameter FindParameterByName(XtraReport report, string parameterName)
        {
            foreach (DevExpress.XtraReports.Parameters.Parameter parameter in report.Parameters)
            {
                if (parameter.Name == parameterName)
                {
                    return parameter;
                }
            }
            return null; // Parameter not found
        }

        //public override byte[] GetData(string url)
        //{
        //var report = Reports[url];
        //using (MemoryStream stream = new MemoryStream())
        //{
        //    report.SaveLayoutToXml(stream);
        //    return stream.ToArray();
        //}
        //    var reportWithParams = Reports[url];

        //    // Get the parameters from the report and set their values
        //    reportWithParams.Parameters["parameter1"].Value = "Parameter1Value";

        //    using (MemoryStream stream = new MemoryStream())
        //    {
        //        reportWithParams.SaveLayoutToXml(stream);
        //        return stream.ToArray();
        //    }

        //}
        public override Dictionary<string, string> GetUrls()
        {
            return Reports.ToDictionary(x => x.Key, y => y.Key);
        }
        public override void SetData(XtraReport report, string url)
        {
            if (Reports.ContainsKey(url))
            {
                Reports[url] = report;
            }
            else
            {
                Reports.Add(url, report);
            }
        }
        public override string SetNewData(XtraReport report, string defaultUrl)
        {
            SetData(report, defaultUrl);
            return defaultUrl;
        }
        public override bool IsValidUrl(string url)
        {
            return true;
        }
    }
}