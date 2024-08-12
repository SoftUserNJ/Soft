using System.ComponentModel;

namespace SoftaxeERP_API.Reports
{
    public partial class DoSummary : DevExpress.XtraReports.UI.XtraReport
    {
     
        public DoSummary()
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
    }
}
