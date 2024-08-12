using System.Collections.Generic;
using CityTech.Models; // Assuming this is the namespace for TblUser

namespace CityTech.Models.ViewModel
{
    public class PlanningViewModel
    {
        public List<PlanningSlot> PlanningSlots { get; set; }
        public List<TblUser> Users { get; set; }
        public List<PlanningTask> Tasks { get; set; }
    }
}
