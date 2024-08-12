using System.ComponentModel;

namespace SoftaxeERP_API.Reports
{
    public partial class BalanceSheet : DevExpress.XtraReports.UI.XtraReport
    {
       
        public BalanceSheet()
        {
            InitializeComponent();
        }

        private void xrPictureBox1_BeforePrint_1(object sender, CancelEventArgs e)
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
    }
}
