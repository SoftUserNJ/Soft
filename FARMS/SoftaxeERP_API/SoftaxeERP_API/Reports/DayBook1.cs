using System.ComponentModel;

namespace SoftaxeERP_API.Reports
{
    public partial class DayBook1 : DevExpress.XtraReports.UI.XtraReport
    {

        //private IWebHostEnvironment env;
        //private readonly IAuth _auth;

        //readonly AuthUser? auth = new();

        public DayBook1()
        {
            InitializeComponent();
            //env = environment;
            //_auth = authData;

            //auth = _auth.GetUserData();
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
