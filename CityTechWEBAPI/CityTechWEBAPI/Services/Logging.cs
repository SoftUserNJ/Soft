using CityTechWEBAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityTechWEBAPI.Services
{
    public class Logging
    {
        private readonly CityTechDevContext _dbContext;
        
        public Logging(CityTechDevContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ProcessLog(int UserId, DateTime LogDate, string Activity, int IncidentNo, string Latitude, string Longitude)
        {
            int? nullableIncidentNo = IncidentNo == 0 ? null : IncidentNo;
            

                var log = new TblLog()
                    {
                    Tag = "app",
                        UserId = UserId,
                        LogDate = LogDate,
                        Activity = Activity,
                        IncidentNo = nullableIncidentNo,
                        Latitude = Latitude,
                        Longitude = Longitude
                    };
                    await _dbContext.TblLogs.AddAsync(log);
                    await _dbContext.SaveChangesAsync();
                    

        }

    }
}
