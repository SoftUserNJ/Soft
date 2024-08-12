namespace CityTech.VMs
{
    public class AllowForm
    {
        public string MenuId { get; set; }
        public string Url { get; set; }
        public string MenuName { get; set; }
        public bool Save { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }
        public bool PDF { get; set; }
        public bool Excel { get; set; }
        public bool Csv { get; set; }
        public bool Word { get; set; }
    }
}
