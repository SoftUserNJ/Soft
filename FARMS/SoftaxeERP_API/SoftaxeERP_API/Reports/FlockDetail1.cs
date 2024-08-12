using DevExpress.CodeParser;
using DevExpress.XtraReports.UI;
using System.ComponentModel;
using System.Globalization;

namespace SoftaxeERP_API.Reports
{
    public partial class FlockDetail1 : DevExpress.XtraReports.UI.XtraReport
    {
        public FlockDetail1()
        {
            InitializeComponent();
        }

        int birdAge = 1;
        DateTime oldDate;
        DateTime dts;

        private void xrTableCell1_BeforePrint(object sender, CancelEventArgs e)
        {
            //string date = GetCurrentColumnValue("TRANSDATE").ToString();
            //string runBal = GetCurrentColumnValue("RUNNING_BALANCE").ToString();
            //string feedUsed = GetCurrentColumnValue("FEEDUSED").ToString();
            //string feedBalance = GetCurrentColumnValue("RUNNING_BALANCE").ToString();

            DateTime date = GetColumnValue<DateTime>("TRANSDATE", DateTime.UtcNow);
            double feedUsed = GetColumnValue<double>("FEEDUSED", 0);
            double remBird = GetColumnValue<double>("RUNNING_BALANCE", 0);

            DateTime dt = Convert.ToDateTime(date);

            if (oldDate != dts)
            {
                if (oldDate != dt)
                {
                    TimeSpan duration = dt - oldDate;
                    birdAge += (int)duration.TotalDays;
                }
            }

            var feedWithFormula = ((feedUsed * 50000) / remBird / birdAge);

            xrTableCell2.Text = birdAge.ToString();
            xrTableCell23.Text = feedWithFormula.ToString("#,##0.00;(#,##0.00);0");

            oldDate = dt;
        }

        public T GetColumnValue<T>(string columnName, T defaultValue = default)
        {
            object columnValue = GetCurrentColumnValue(columnName);
            return columnValue != null ? (T)columnValue : defaultValue;
        }
    }
}
