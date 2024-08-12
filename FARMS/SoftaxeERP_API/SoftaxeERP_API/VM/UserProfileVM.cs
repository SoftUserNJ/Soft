namespace SoftaxeERP_API.VM
{
    public class UserProfileVM
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string OldPass { get; set; }
        public string NewPass { get; set; }
        public string ConfirmPass { get; set; }
        public IFormFile Image { get; set; }
        public DateTime Dtnow { get; set; }
    }
}
