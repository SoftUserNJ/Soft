using DevExpress.CodeParser;
using DevExpress.XtraPrinting.Drawing;
using DevExpress.XtraReports.UI;
using SoftaxeERP_API.Services;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace Inventory_Pos_Softaxe.Reports
{
    public partial class EmployeeDetailForm : DevExpress.XtraReports.UI.XtraReport
    {

        //private IWebHostEnvironment env;
        //private readonly IAuth _auth;

        //readonly AuthUser? auth = new();

        //public DayBook(IWebHostEnvironment environment, IAuth authData)
        //{
        //    InitializeComponent();
        //    env = environment;
        //    _auth = authData;

        //    auth = _auth.GetUserData();
        //}
        public EmployeeDetailForm( )
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
