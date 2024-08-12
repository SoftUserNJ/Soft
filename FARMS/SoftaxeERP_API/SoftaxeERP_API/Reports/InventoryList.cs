using DevExpress.CodeParser;
using DevExpress.XtraPrinting.Drawing;
using System.ComponentModel;
using Microsoft.AspNetCore.Hosting;


namespace SoftaxeERP_API.Reports
{
	public partial class InventoryList : DevExpress.XtraReports.UI.XtraReport
	{
        //private IWebHostEnvironment env;
        //private readonly IAuth _auth;

        //readonly AuthUser? auth = new();

        //public InventoryList(IWebHostEnvironment environment, IAuth authData)
        //{
        //    InitializeComponent();
        //    env = environment;
        //    _auth = authData;

        //    auth = _auth.GetUserData();
        //}
        public InventoryList()
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
