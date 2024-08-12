namespace SoftaxeERP_API.VM
{
  public class DOVM
    {
        public int GpNo { get; set; }
        public DateTime GpDate { get; set; }
        public string VehicleNo { get; set; }
        public string DriverName { get; set; }
        public string Phone { get; set; }
        public string BiltyNo { get; set; }
        public int DONO { get; set; }
        public decimal DELQTY { get; set; }

        public int Freight { get; set;}

        public string ProductCode { get; set; }

        public int sumDelQty { get; set; }

    }
}