using DevExpress.CodeParser;
using DevExpress.XtraPrinting.Drawing;

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace SoftaxeERP_API.Reports
{
    public partial class ProductLedger1 : DevExpress.XtraReports.UI.XtraReport
    {
        //private IWebHostEnvironment env;
        //private readonly IAuth _auth;

        //readonly AuthUser? auth = new();

        //public ProductLedger(IWebHostEnvironment environment, IAuth authData)
        //{
        //    InitializeComponent();
        //    env = environment;
        //    _auth = authData;

        //    auth = _auth.GetUserData();
        //}
        public ProductLedger1()
        {
            InitializeComponent();

        }

        private void xrPictureBox1_BeforePrint_1(object sender, CancelEventArgs e)
        {
        //    if (auth != null)
        //    {
        //        string imagePath = Path.Combine(env.WebRootPath, "Companies", auth.CmpName, "CompanyLogo", auth.Cmplogo);
        //        if (File.Exists(imagePath))
        //        {
        //            xrPictureBox1.ImageSource = ImageSource.FromFile(imagePath);
        //        }
        //    }
        }
    }
}
