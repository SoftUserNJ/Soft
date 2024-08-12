using DevExpress.CodeParser;
using DevExpress.XtraReports.UI;
using System.ComponentModel;
using System.Globalization;

namespace SoftaxeERP_API.Reports
{
    public partial class FlockDetail : DevExpress.XtraReports.UI.XtraReport
    {
        public FlockDetail()
        {
            InitializeComponent();
        }

        double previousAvgWeight = 0;
        int cell23 = 0;

        private void xrTableCell23_BeforePrint(object sender, CancelEventArgs e)
        {
            XRTableCell currentCell = (XRTableCell)sender;
            XRTableCell avgWeightCell = (XRTableCell)currentCell.Report.FindControl("xrTableCell21", true);
            double currentAvgWeight = Convert.ToDouble(avgWeightCell.Text == "" ? 0 : avgWeightCell.Text);
            double difference = 0;
            if (cell23 == 0)
            {
                difference = (currentAvgWeight - previousAvgWeight);
            }
            else
            {
                difference = (currentAvgWeight - previousAvgWeight) * 1000;
            }
            currentCell.Text = difference.ToString("#,##0.00;(#,##0.00);0");
            previousAvgWeight = currentAvgWeight;
            cell23 = 1;
        }

        
        double remChick = 0;
        double accSale = 0;
        int cell29 = 0;

        private void xrTableCell29_BeforePrint(object sender, CancelEventArgs e)
        {
            XRTableCell currentCell = (XRTableCell)sender;
            XRTableCell saleBroiler = (XRTableCell)currentCell.Report.FindControl("xrTableCell24", true);
            XRTableCell feedUsed = (XRTableCell)currentCell.Report.FindControl("xrTableCell15", true);

            XRTableCell remChicks = (XRTableCell)currentCell.Report.FindControl("xrTableCell19", true);
            XRTableCell wightBird = (XRTableCell)currentCell.Report.FindControl("xrTableCell21", true);
            XRTableCell accSales = (XRTableCell)currentCell.Report.FindControl("xrTableCell27", true);

            XRTableCell comFcr = (XRTableCell)currentCell.Report.FindControl("xrTableCell30", true);

            double currentSaleBro = Convert.ToDouble(saleBroiler.Text == "" ? 0 : saleBroiler.Text);
            double currentFeedUsed = Convert.ToDouble(feedUsed.Text == "" ? 0 : feedUsed.Text);
            double currentWightBird = Convert.ToDouble(wightBird.Text == "" ? 0 : wightBird.Text);

            double print = 0;
            if (cell29 == 0)
            {
                print = (currentSaleBro / currentFeedUsed);
            }
            else
            {
                print = (remChick * currentWightBird + accSale) / currentFeedUsed;
            }
            currentCell.Text = print.ToString("#,##0.00;(#,##0.00);0");

            accSale = Convert.ToDouble(accSales.Text == "" ? 0 : accSales.Text);
            remChick = Convert.ToDouble(remChicks.Text == "" ? 0 : remChicks.Text);
            cell29 = 1;
        }

        double oldFcr = 0;
        int cell30 = 0;
        private void xrTableCell30_BeforePrint(object sender, CancelEventArgs e)
        {
            XRTableCell currentCell = (XRTableCell)sender;
            XRTableCell fcr = (XRTableCell)currentCell.Report.FindControl("xrTableCell29", true);
            double currentFce = Convert.ToDouble(fcr.Text == "" ? 0 : fcr.Text);

            if (cell30 == 0)
            {
                currentCell.Text = "-";
            }
            else
            {
                currentCell.Text = (currentFce - oldFcr).ToString("#,##0.00;(#,##0.00);0");

            }
            oldFcr = currentFce;
            cell30 = 1;
        }


        int cell31 = 0;
        double rChicks = 0;
        private void xrTableCell31_BeforePrint(object sender, CancelEventArgs e)
        {
            XRTableCell currentCell = (XRTableCell)sender;
            XRTableCell remChicks = (XRTableCell)currentCell.Report.FindControl("xrTableCell19", true);
            XRTableCell feedUsed = (XRTableCell)currentCell.Report.FindControl("xrTableCell13", true);
            double currentRemChicks = Convert.ToDouble(remChicks.Text == "" ? 0 : remChicks.Text);
            double currentFeedUsed = Convert.ToDouble(feedUsed.Text == "" ? 0 : feedUsed.Text);

            double print = 0;
            if (cell31 == 0)
            {
                print = (currentFeedUsed * 50000/ currentRemChicks);
            }
            else
            {
                print = (currentFeedUsed * 50000 / rChicks);

            }
            currentCell.Text = print.ToString("#,##0.00;(#,##0.00);0");
            rChicks = currentRemChicks;
            cell31 = 1;
        }

        int birdAge = 1;
        DateTime oldDate;
        DateTime dts;

        private void xrTableCell1_BeforePrint(object sender, CancelEventArgs e)
        {
            string date = GetCurrentColumnValue("TRANSDATE").ToString();
            string runBal = GetCurrentColumnValue("RUNNING_BALANCE").ToString();
            DateTime dt = Convert.ToDateTime(date);

            if (oldDate != dts)
            {
                if (oldDate != dt)
                {
                    TimeSpan duration = dt - oldDate;
                    birdAge += (int)duration.TotalDays;
                }
            }
            
            var feedWithFormula = ((Convert.ToDouble(runBal) * 4.5) * birdAge) / 1000 / 50;

            xrTableCell2.Text = birdAge.ToString();
            xrTableCell20.Text = feedWithFormula.ToString("#,##0.00;(#,##0.00);0");

            oldDate = dt;
        }
    }
}
