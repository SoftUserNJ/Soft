using System.ComponentModel;

namespace SoftaxeERP_API.Reports
{
    public partial class Aging : DevExpress.XtraReports.UI.XtraReport
    {

        public Aging()
        {
            InitializeComponent();
        }

        private void xrPictureBox1_BeforePrint(object sender, CancelEventArgs e)
        {
        }
    }
}
