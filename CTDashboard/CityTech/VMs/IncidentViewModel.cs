﻿namespace CityTech.VMs
{
    public class IncidentViewModel
    {
        public int IncidentNo { get; set; }

        public DateTime IncidentDate { get; set; }

        public int IncidentTypeId { get; set; }

        public int ObjectId { get; set; }

        public int MechanicId { get; set; }

        public int? UserId { get; set; }

        public DateTime? ScheduleDate { get; set; }

        public DateTime? AssignedSecure { get; set; }

        public DateTime? AssignedFixed { get; set; }

        public bool IsScheduled { get; set; }

        public string Prepration { get; set; } = null!;

        public string Requirement { get; set; } = null!;

        public bool? Accepted { get; set; }

        public bool? WorkDone { get; set; }

        public string? GlassBreakSiren { get; set; }

        public string? Relay2 { get; set; }

        public string? GlassBreak { get; set; }

        public string? GlassBreakFrequency { get; set; }

        public string? DoorContact { get; set; }

        public string? Empty { get; set; }

        public string? Empty2 { get; set; }

        public string? Relay4 { get; set; }

        public string? Relay3 { get; set; }

        public string? TemperatureSensor { get; set; }

        public string? Vin { get; set; }

        public string? Time { get; set; }

        public string? Router { get; set; }

        public string? Siren { get; set; }

        public string? GlassBreakDoor { get; set; }

        public string? GlassBreakDoorFrequency { get; set; }

        public string? MotionDetected { get; set; }

        public string? MotionDetectedCount { get; set; }

        public string? Voltageafterthecircuitbreaker { get; set; }

        public string? Voltagebeforethecircuitbreaker { get; set; }

        public string? MasterScreen { get; set; }

        public string? Player { get; set; }

        public string? Emailno { get; set; }
    }
}