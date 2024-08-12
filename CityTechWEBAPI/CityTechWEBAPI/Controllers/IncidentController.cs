using CityTechWEBAPI.Models;
using CityTechWEBAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using System.Diagnostics;

namespace CityTechWEBAPI.Controllers
{
 	[Authorize]
	[ApiController]
	[Route("api/[controller]")]

	public class IncidentController : ControllerBase
	{
		private readonly CityTechDevContext _dbContext;
		private readonly IConfiguration _configuration;
        private readonly Logging _logging;
        public IncidentController(CityTechDevContext dbContext, IConfiguration configuration, Logging logging)
		{
			_dbContext = dbContext;
			_configuration = configuration;
			_logging = logging;

        }

		#region editPassword
		[HttpPut("EditUserPassword")]
		public async Task<IActionResult> EditPassword(int userId, string oldPassword, string newPassword)
		{
			using (var transaction = await _dbContext.Database.BeginTransactionAsync())
			{
				try
				{
					var user = await _dbContext.TblUsers.FindAsync(userId);
					if (user == null)
					{
						return NotFound();
					}

					if (oldPassword != user.Password)
					{
						return BadRequest("Incorrect old password.");
					}


					user.Password = newPassword;
					await _dbContext.SaveChangesAsync();
					await transaction.CommitAsync();

					return Ok("Password updated successfully.");
				}
				catch (Exception ex)
				{
					await transaction.RollbackAsync();
					return StatusCode(500, "An error occurred while updating the password.");
				}
			}
		}
		#endregion




		[HttpPost("ProcessIncident")]
		public async Task<IActionResult> ProcessIncident([FromQuery] IncidentAction action, int mechanicId, string? Activity = null, string? Lat = null, string? Long = null, DateTime? LogDate = null, bool? Revisit = null, string? WorkDes = null, string? WorkActivity = null, int? incidentNo = null, bool? isAccept = null, bool? isWorkDone = null, DateTime? WorkDate = null, int? userid = null, DateTime? scheduledDate = null, DateTime? TravelStart = null, DateTime? WorkEnd = null, DateTime? WorkStart = null, string? invoiceStatus = null, string? workDoneStatus = null)
		{
			try
			{
				var incident = await _dbContext.TblIncidents.FirstOrDefaultAsync(i => i.IncidentNo == incidentNo);

				using (var transaction = await _dbContext.Database.BeginTransactionAsync())
				{
					switch (action)
					{
						case IncidentAction.GetIncidents:
							var mech =  GetIncidentDetailsAsync(mechanicId,incidentNo);
							return Ok(mech);


						case IncidentAction.AcceptIncident:
							if (!isAccept.HasValue)
							{
								return BadRequest("Parameter 'isAccept' is missing.");
							}

							incident.Accepted = isAccept.Value;
                            //var log = new TblLog
                            //{
                            //                         Tag = "app",
                            //                         Activity = Activity,
                            //	Latitude = Lat,
                            //	Longitude = Long,
                            //	LogDate = LogDate,
                            //	UserId = mechanicId,
                            //	IncidentNo = incidentNo

                            //};

                            //await _dbContext.TblLogs.AddAsync(log);
                            await _logging.ProcessLog(mechanicId, LogDate ?? DateTime.MinValue, Activity, incidentNo.HasValue ? incidentNo.Value : 0, Long, Lat);
                            break;

						case IncidentAction.TravelStart:
							if (!TravelStart.HasValue)
							{
								return BadRequest("Parameter 'isAccept' is missing.");
							}

							incident.TravelStart = TravelStart.Value;
                            //var Tslog = new TblLog
                            //{
                            //                         Tag = "app",
                            //                         Activity = Activity,
                            //	Latitude = Lat,
                            //	Longitude = Long,
                            //	LogDate = LogDate,
                            //	UserId = mechanicId,
                            //	IncidentNo = incidentNo

                            //};

                            //await _dbContext.TblLogs.AddAsync(Tslog);
                            await _logging.ProcessLog(mechanicId, LogDate ?? DateTime.MinValue, Activity, incidentNo.HasValue ? incidentNo.Value : 0, Long, Lat);

                            break;

						case IncidentAction.WorkDone:
							if (!isWorkDone.HasValue)
							{
								return BadRequest("Parameter 'isWorkDone' is missing.");
							}

							incident.WorkDone = isWorkDone.Value;
							incident.WorkEnd = WorkEnd;
							incident.Revisit = false;
							incident.InvoiceStatus = invoiceStatus;
							incident.WorkDoneStatus = workDoneStatus;
                            //var WDlog = new TblLog
                            //{
                            //                         Tag = "app",
                            //                         Activity = Activity,
                            //	Latitude = Lat,
                            //	Longitude = Long,
                            //	LogDate = LogDate,
                            //	UserId = mechanicId,
                            //	IncidentNo = incidentNo

                            //};

                            //await _dbContext.TblLogs.AddAsync(WDlog);
                            await _logging.ProcessLog(mechanicId, LogDate ?? DateTime.MinValue, Activity, incidentNo.HasValue ? incidentNo.Value : 0, Long, Lat);
                            break;

						case IncidentAction.Revisit:
							if (!Revisit.HasValue)
							{
								return BadRequest("Parameter 'isWorkDone' is missing.");
							}

							incident.Revisit = Revisit.Value;
							incident.WorkDone = false;
                            incident.WorkDoneStatus = workDoneStatus;
                            //var Revisitlog = new TblLog
                            //{
                            //                         Tag = "app",
                            //                         Activity = Activity,
                            //	Latitude = Lat,
                            //	Longitude = Long,
                            //	LogDate = LogDate,
                            //	UserId = mechanicId,
                            //	IncidentNo = incidentNo

                            //};

                            //await _dbContext.TblLogs.AddAsync(Revisitlog);
                            await _logging.ProcessLog(mechanicId, LogDate ?? DateTime.MinValue, Activity, incidentNo.HasValue ? incidentNo.Value : 0, Long, Lat);
                            break;

						case IncidentAction.ProcessActivity:
							int mWORKSNO = 0;
							int? maxWORKSNO = _dbContext.TblIncidentWorks.Where(i => i.IncidentNo == incidentNo).Max(i => (int?)i.WorkSno);
							if (maxWORKSNO.HasValue)
							{
								mWORKSNO = maxWORKSNO.Value + 1;
							}
							else
							{

								mWORKSNO = 1;
							}



							var workIncident = new TblIncidentWork()
							{
								WorkSno = mWORKSNO,
								WorkStatus = "Activity",
								Activity = WorkActivity,
								IncidentNo = incidentNo ?? 0,
								MechanicId = mechanicId,
								UserId = userid,
								WorkDate = WorkDate,
								WorkDes = WorkDes

							};

							_dbContext.TblIncidentWorks.Add(workIncident);
                            //var Activitylog = new TblLog
                            //{
                            //                         Tag = "app",
                            //                         Activity = Activity,
                            //	Latitude = Lat,
                            //	Longitude = Long,
                            //	LogDate = LogDate,
                            //	UserId = mechanicId,
                            //	IncidentNo = incidentNo

                            //};

                            //await _dbContext.TblLogs.AddAsync(Activitylog);
                            await _logging.ProcessLog(mechanicId, LogDate ?? DateTime.MinValue, Activity, incidentNo.HasValue ? incidentNo.Value : 0, Long, Lat);
                            break;

						case IncidentAction.WorkStart:
							if (!WorkStart.HasValue)
							{
								return BadRequest("Parameter 'isWorkDone' is missing.");
							}

							incident.WorkStart = WorkStart.Value;

                            //var Wslog = new TblLog
                            //{
                            //                         Tag = "app",
                            //                         Activity = Activity,
                            //	Latitude = Lat,
                            //	Longitude = Long,
                            //	LogDate = LogDate,
                            //	UserId = mechanicId,
                            //	IncidentNo = incidentNo

                            //};

                            //await _dbContext.TblLogs.AddAsync(Wslog);
                            await _logging.ProcessLog(mechanicId, LogDate ?? DateTime.MinValue, Activity, incidentNo.HasValue ? incidentNo.Value : 0, Long, Lat);
                            break;
						default:
							return BadRequest("Invalid action.");
					}

					await _dbContext.SaveChangesAsync();
					await transaction.CommitAsync();

					var accepted = incident.Accepted ?? false;
					var workDone = incident.WorkDone ?? false;

					var incidentDetails =  GetIncidentDetailsAsync(mechanicId, incidentNo, accepted, workDone);
					return Ok(incidentDetails);
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, "An error occurred while updating the status.");
			}
		}




		private string GetIncidentDetailsAsync(int mechanicId, int? incidentNo = null, bool? accepted = null, bool? workDone = null)
		{

			string whrincidentNo = "";
			string whraccepted = "";
			string whrworkDone = "";


				if (incidentNo == null) { whrincidentNo = "And  ISNULL( i.IncidentNo,0) like '%'"; } else { whrincidentNo = " And ISNULL(i.IncidentNo,0) = '" + incidentNo + "'"; }
				if (accepted == null) { whraccepted = " And ISNULL( i.accepted,0) like '%'"; } else { whraccepted = " And ISNULL(i.accepted,0) = '" + accepted + "'"; }
				if (workDone == null) { whrworkDone = "And ISNULL( i.workdone,0) like '%'"; } else { whrworkDone = " And ISNULL(i.workdone,0) = '" + workDone + "'"; }
			

			DataLogic dl = new DataLogic(_configuration);
			string qry = @"SELECT i.IncidentNo, i.IncidentDate, i.IncidentTypeId, i.ObjectId, i.MechanicId, i.UserId,
				i.ScheduleDate, i.AssignedSecure, i.AssignedFixed, i.IsScheduled, ue.UserName AS IncidentManager,
				it.IncidentName AS issue, o.ObjectName, it.priotype, i.requirement, i.prepration, i.IncidentTag, c.Email, ol.Longi, ol.Lati,ol.ContactPerson, ol.ContactPersonPhone, ol.LocName2, i.WorkDetail, s.Station, ol.LocName, u.UserName AS MechanicName
               , (  CASE
               WHEN isnull(IsScheduled ,0) = 1  and isnull(Accepted,0)=0 and ISNULL(WorkDone,0)=0 THEN 'NEW JOB'
               WHEN isnull(IsScheduled ,0) = 1  and isnull(Accepted,0)=1 and ISNULL(WorkDone,0)=0 THEN 'ACCEPTED'
			    WHEN isnull(IsScheduled ,0) = 1  and isnull(Accepted,0)=1 and ISNULL(WorkDone,0)=1 THEN 'WORK DONE'
               END)  AS 'Status'
                FROM TblIncidents i
                Left JOIN TblIncidentTypes it ON i.IncidentTypeId = it.IncidentTypeId
                Left JOIN TblObjects o ON i.ObjectId = o.ObjectId
                Left Join tblcustomers c ON o.customerId = c.customerId
                Left JOIN TblObjectLocations ol ON i.locid = ol.LocId
				Left Join tblStation s On s.Id = ol.StationId
                Left JOIN TblUsers u ON i.MechanicId = u.UserId
                Left JOIN TblUsers ue ON i.UserId = ue.UserId
                WHERE i.IsScheduled=1 AND i.MechanicId = '" + mechanicId + "' "  + whrincidentNo + whraccepted+ whrworkDone;

			string qry1 = @" Select  IncidentNo,WorkSno, WorkDes, WorkStatus, WorkDate, UserId from TblIncidentWork iw 
                WHERE    iw.MechanicId = '" + mechanicId + "'  and workstatus='Rejected'";
			

			var dt = dl.LoadData(qry);
			var dt1 = dl.LoadData(qry1);


			var result = new {
				IncidentDetail = dt,
			   	IncidentWork = dt1

			};

			return JsonConvert.SerializeObject(result);
		}




		public enum IncidentAction
		{
			GetIncidents,
			AcceptIncident,
			TravelStart,
			ProcessActivity,
			WorkDone,
			WorkStart,
			Revisit
		}


		[HttpPost("RejectIncident")]
		public async Task<IActionResult> RejectIncident(int incidentNo, int mechanicId, string Activity, string Lat, string Long, DateTime LogDate, string WorkStatus, int userId, DateTime scheduledDate, DateTime WorkRejectedAt, string reason)
		{
			using (var transaction = await _dbContext.Database.BeginTransactionAsync())
			{
				try
				{

					int mWORKSNO = 0;
					int? maxWORKSNO = _dbContext.TblIncidentWorks.Where(i => i.IncidentNo == incidentNo).Max(i => (int?)i.WorkSno);
					if (maxWORKSNO.HasValue)
					{
						mWORKSNO = maxWORKSNO.Value + 1;
					}
					else
					{

						mWORKSNO = 1;
					}



					var workIncident = new TblIncidentWork()
					{   WorkSno = mWORKSNO,
						WorkStatus = WorkStatus,
						IncidentNo = incidentNo,
						MechanicId = mechanicId,
						UserId = userId,
						ScheduleDate = scheduledDate,
						WorkDate = WorkRejectedAt,
						WorkDes = reason

					};

					_dbContext.TblIncidentWorks.Add(workIncident);


					var incidentToUpdate = await _dbContext.TblIncidents.FirstOrDefaultAsync(i => i.IncidentNo == incidentNo);
					if (incidentToUpdate != null)
					{
						incidentToUpdate.IsScheduled = false;
						incidentToUpdate.Rejected = true;
						incidentToUpdate.Accepted = false;
						incidentToUpdate.WorkDone = false;
						_dbContext.Update(incidentToUpdate);
						await _dbContext.SaveChangesAsync();
					}

                    //var log = new TblLog
                    //{
                    //                   Tag = "app",
                    //                   Activity = Activity,
                    //	Latitude = Lat,
                    //	Longitude = Long,
                    //	LogDate = LogDate,
                    //	UserId = mechanicId,
                    //	IncidentNo = incidentNo
                    //};

                    //await _dbContext.TblLogs.AddAsync(log);
                    await _logging.ProcessLog(mechanicId, LogDate, Activity, incidentNo, Long, Lat);


                    await _dbContext.SaveChangesAsync();
					await transaction.CommitAsync();

					return Ok("Incident Sucessfully Rejected");
				}
				catch (Exception ex)
				{
					await transaction.RollbackAsync();
					return StatusCode(500, "Failed to save data.");
				}
			}
		}



		[HttpPost("ProcessLog")]
		public async Task<IActionResult> ProcessLog(int UserId, DateTime LogDate, string Activity, int IncidentNo, string Latitude, string Longitude)
		{
			using (var transaction = await _dbContext.Database.BeginTransactionAsync())
			{
				try
				{
					var log = new TblLog()
					{
						UserId = UserId,
						LogDate = LogDate,
						Activity = Activity,
						IncidentNo = IncidentNo,
						Latitude = Latitude,
						Longitude = Longitude
					};
					await _dbContext.TblLogs.AddAsync(log);
					await _dbContext.SaveChangesAsync();
					await transaction.CommitAsync();
					return Ok("Log Created Successfully");

				}
				catch (Exception ex)
				{
					await transaction.RollbackAsync();
					return StatusCode(500, "Failed to save data.");
				}
			}
		}

	}

}
