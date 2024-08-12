namespace SoftaxeERP_API.VM
{
	public class CustomerSupplierVM
	{
		public string L4Code { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string Contact { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }
		public string PostalCode { get; set; }
		public string Ntn { get; set; }
		public string CNIC { get; set; }
		public string SaleTax { get; set; }
		public string WHTax { get; set; }
		public int MainAreaId { get; set; }
		public int SubAreaId { get; set; }
		public int Commission { get; set; }
		public long CreditLimit { get; set; }
		public bool InActive { get; set; }
		public string Tag { get; set; }
		public DateTime DtNow { get; set; }



		public string Strn { get; set; }

		public float RateDiff { get; set; }


        public string AccNo { get; set; }


    }
}
