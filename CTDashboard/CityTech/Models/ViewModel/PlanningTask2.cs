using System.Collections.Generic;
using CityTech.Models; // Assuming this is the namespace for TblUser

namespace CityTech.Models.ViewModel
{
    public class PlanningTask2
    {
        public int IncidentNo { get; set; }
        public int UserId { get; set; }
        public string AssignedSecureDate { get; set; } // Represented as a string in "dd/MM/yyyy" format
        public string TimeAssignSecure { get; set; }
        public string TimeAssignFixed { get; set; }
        public string WorkDetail { get; set; }



        public DateTime StartDateTimeUtc { get; set; }
        public DateTime EndDateTimeUtc { get; set; }

    }

}
