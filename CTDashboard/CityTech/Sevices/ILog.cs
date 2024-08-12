using CityTech.Models;
using Microsoft.VisualBasic;
using System.Globalization;
using System;
using System.Security.AccessControl;

namespace CityTech.Sevices
{
	public interface ILog
	{
		void LogEntry(string logDate, string activityMessage, int incNo, string latitude, string longitude);
	}

	public class Log : ILog
	{
        private readonly CityTechContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Log(CityTechContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
		public void LogEntry(string logDate, string activityMessage, int incNo, string latitude, string longitude)
		{
            int? incidentNumber = incNo == 0 ? null : incNo;
            DateTime logDateTime = DateTime.ParseExact(logDate, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
            var auth = new SessionData(_httpContextAccessor).GetData();

            _context.TblLogs.Add(new TblLog
            {
                UserId = auth.UserId,
                LogDate = logDateTime,
                Activity = activityMessage,
                IncidentNo = incidentNumber,
                Latitude = latitude,
                Longitude = longitude,
                Tag = "dashboard"
            });
            _context.SaveChanges();
        }
	}
}
