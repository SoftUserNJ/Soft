namespace SoftaxeERP_API.VM
{
    public class UserVM
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string LocationId { get; set; }
        public string UserType { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Designation { get; set; }
        public string Cnic { get; set; }
        public string Mobile { get; set; }
        public string Permission { get; set; }
        public bool Dashboard { get; set; }
        public IFormFile Picture {  get; set; } 
        public DateTime DtNow { get; set; }
    }
}
