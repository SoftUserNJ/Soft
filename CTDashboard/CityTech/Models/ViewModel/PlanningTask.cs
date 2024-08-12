using System.Collections.Generic;
using CityTech.Models; // Assuming this is the namespace for TblUser

namespace CityTech.Models.ViewModel
{
    public class PlanningTask
    {
        public int IncidentNo { get; set; }
        public int UserId { get; set; }
        public DateTime AssignedSecure { get; set; }
        public DateTime AssignedFixed { get; set; }
        public TimeSpan TimeAssignSecure => AssignedSecure.TimeOfDay; // Extract time part from AssignedSecure
        public TimeSpan TimeAssignFixed => AssignedFixed.TimeOfDay; // Extract time part from AssignedFixed
    }
}
