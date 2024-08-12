using System.ComponentModel;

namespace SoftaxeERP_API.Reports
{
    public partial class PurchaseInvoice : DevExpress.XtraReports.UI.XtraReport
    {
        
        public PurchaseInvoice()
        {
            InitializeComponent();
        }


        private void xrPictureBox1_BeforePrint(object sender, CancelEventArgs e)
        {
            //if (auth != null)
            //{
            //    string imagePath = Path.Combine(env.WebRootPath, "Companies", auth.CmpName, "CompanyLogo", auth.Cmplogo);
            //    if (File.Exists(imagePath))
            //    {
            //        xrPictureBox1.ImageSource = ImageSource.FromFile(imagePath);
            //    }
            //}
        }

        private void xrLabel61_BeforePrint(object sender, CancelEventArgs e)
        {
            //var totalDue =  Convert.ToDouble(GetCurrentColumnValue("TOTALDUE"));
            //var recAmount =  Convert.ToDouble(GetCurrentColumnValue("RECAMOUNT"));
            //var grossAmount =  Convert.ToDouble(GetCurrentColumnValue("sumSum([GROSSAMOUNT])"));


            //var netDue = Math.Round((grossAmount + totalDue) - recAmount, 0);

            //var inWords = _dataLogic.NumberToWordAmount(netDue);

            //xrLabel61.Text = inWords.ToString();
        }
    }
}
