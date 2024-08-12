using System.ComponentModel;

namespace SoftaxeERP_API.Reports
{
    public partial class PurchaseInvoice1 : DevExpress.XtraReports.UI.XtraReport
    {
        //private IWebHostEnvironment env;
        //private IDataLogic _dataLogic;
        //private readonly IAuth _auth;

        //readonly AuthUser? auth = new();
        //public PuechaseInvoice(IWebHostEnvironment environment, IAuth authData, IDataLogic dataLogic)
        //{
        //    InitializeComponent();
        //    env = environment;
        //    _dataLogic = dataLogic;
        //    _auth = authData;

        //    auth = _auth.GetUserData();
        //}  
        public PurchaseInvoice1()
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

        private void xrLabel61_BeforePrint(object sender, CancelEventArgs e)
        {
            //var netDue =  Convert.ToDouble(GetCurrentColumnValue("NETDUE"));

            //var inWords = _dataLogic.NumberToWordAmount(netDue);

            //xrLabel61.Text = inWords.ToString();
        }
    }
}
