using DevExpress.Export.Xl;
using DevExpress.XtraReports.UI;
using SoftaxeERP_API.Services;
using System.ComponentModel;


namespace SoftaxeERP_API.Reports
{
    public partial class PrintVoucherRangeWise : DevExpress.XtraReports.UI.XtraReport
    {
        public PrintVoucherRangeWise()
        {
            InitializeComponent();
        }

        private static String[] units = { "Zero", "One", "Two", "Three",
        "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven",
        "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen",
        "Seventeen", "Eighteen", "Nineteen" };

        private static String[] tens = { "", "", "Twenty", "Thirty", "Forty",
        "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

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

        double credit = 0;
        string vchNo = "";
        string vchType = "";
        private void xrTableCell16_BeforePrint(object sender, CancelEventArgs e)
        {
            string vNo = GetCurrentColumnValue("VchNo").ToString();
            string vType = GetCurrentColumnValue("VchType").ToString();
            if (vchNo == vNo && vchType == vType)
            {
                credit += Convert.ToDouble(GetCurrentColumnValue("Credit"));
            }
            else
            {
                credit = 0;
            }

            vchNo = GetCurrentColumnValue("VchNo").ToString();
            vchType = GetCurrentColumnValue("VchType").ToString();
        }

        private void xrTableCell22_BeforePrint(object sender, CancelEventArgs e)
        {
            var inWords = NumberToWordAmount(credit);
            xrTableCell22.Text = inWords.ToString();
        }

        public string NumberToWordAmount(double amount)
        {
            try
            {
                Int64 amount_int = (Int64)amount;
                Int64 amount_dec = (Int64)Math.Round((amount - (double)(amount_int)) * 100);
                if (amount_dec == 0)
                {
                    return NumberToWords(amount_int) + " Only.";
                }
                else
                {
                    return NumberToWords(amount_int) + " Point " + NumberToWords(amount_dec) + " Only.";
                }
            }
            catch (Exception e)
            {
                // TODO: handle exception  
            }
            return "";
        }


        static string NumberToWords(long number)
        {
            if (number == 0)
                return "Zero";

            if (number < 0)
                return "Negative " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000000) > 0)
            {
                words += NumberToWords(number / 1000000000) + " Billion ";
                number %= 1000000000;
            }

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " Million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " Thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " Hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                string[] unitsArray = { "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" };
                string[] teensArray = { "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                string[] tensArray = { "", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

                if (number < 10)
                    words += unitsArray[number];
                else if (number < 20)
                    words += teensArray[number - 11];
                else
                {
                    words += tensArray[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsArray[number % 10];
                }
            }

            return words;
        }

        public String ConvertInt64(Int64 i)
        {
            if (i < 20)
            {
                return units[i];
            }
            if (i < 100)
            {
                return tens[i / 10] + ((i % 10 > 0) ? " " + ConvertInt64(i % 10) : "");
            }
            if (i < 1000)
            {
                return units[i / 100] + " Hundred"
                        + ((i % 100 > 0) ? " And " + ConvertInt64(i % 100) : "");
            }
            if (i < 100000)
            {
                return ConvertInt64(i / 1000) + " Thousand "
                        + ((i % 1000 > 0) ? " " + ConvertInt64(i % 1000) : "");
            }
            if (i < 10000000)
            {
                return ConvertInt64(i / 100000) + " Lakh "
                        + ((i % 100000 > 0) ? " " + ConvertInt64(i % 100000) : "");
            }
            if (i < 1000000000)
            {
                return ConvertInt64(i / 10000000) + " Crore "
                        + ((i % 10000000 > 0) ? " " + ConvertInt64(i % 10000000) : "");
            }
            return ConvertInt64(i / 1000000000) + " Arab "
                    + ((i % 1000000000 > 0) ? " " + ConvertInt64(i % 1000000000) : "");
        }
    }
}
