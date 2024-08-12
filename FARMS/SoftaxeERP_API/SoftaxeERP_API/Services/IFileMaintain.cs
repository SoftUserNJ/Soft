using DevExpress.Office;
using DevExpress.XtraRichEdit.Import.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftaxeERP_API.Models;
using SoftaxeERP_API.VM;
using System.Data;
using System.Drawing.Drawing2D;

namespace SoftaxeERP_API.Services
{
    public interface IFileMaintain
    {
        // Department Entry

        bool SaveDepartment([FromBody] Tblcompanydepartment requestData);

        DataTable GetDepartmentList();

        bool DelDepartment(int Id);

        // Designation Entry

        bool SaveDesignation([FromBody] Tblcompanydesignation requestData);

        DataTable GetDesignationList();

        bool DelDesignation(int Id);

        // Holiday Setup

        bool SaveHoliday([FromBody] Tblholidaysetup requestData);
        DataTable GetHolidayList();
        bool DelHoliday(int Id);

        // Leaves Type Entry
        bool SaveHrSetup([FromBody] Tblhrsetup requestData);

        DataTable GetHrSetupList();

        bool DelHRSetup(int vchNo);


        // Shift Entry

        bool SaveShift([FromBody] TblEmployeeShift requestData);

        DataTable GetShiftList();

        bool DelShift(int Id);

        // Employee Type Entry

        bool SaveEmployeeType([FromBody] TblEmployeeType requestData);

        DataTable GetEmpTypeList();

        bool DelEmployeeType(int Id);

        // Salary Reason Entry

        bool SaveSalaryReason([FromBody] TblSalaryReason requestData);

        DataTable GetSalaryReasonList();

        bool DelSalaryReason(int Id);

        // Salary Settlement Labels

        bool SaveLabels([FromBody] TblSalarydtLable requestData);

        DataTable GetSalaryLabels();

        // Salary Days

        bool SaveSalaryDays([FromBody] SalaryDay requestData);

        DataTable GetSalaryDays();


        // Set Month and Year

        bool SaveMonthYear([FromBody] TblMonth requestData);

        DataTable GetMonthYear();

        // Overtime Formula

        DataTable GetOvertimeFormula();

        bool SaveOvertimeFormula([FromBody] Tblotformula requestData);

    }

    public class FileMaintain : IFileMaintain
    {
        private readonly ErpSoftaxeContext _context;
        private readonly IDataLogic _dataLogic;
        private readonly IAuth _auth;
        readonly AuthVM auth = new();
        public FileMaintain(ErpSoftaxeContext context, IDataLogic dataLogic, IAuth authData)
        {
            _context = context;
            _dataLogic = dataLogic;
            _auth = authData;

            auth = _auth.GetUserData();
        }


        // Department Entry


        public DataTable GetDepartmentList()
        {
            String qry = $@"Select Id, Department from tblcompanydepartment where comp_id = '"+ auth.CmpId +"' and locid = '"+ auth.LocId +"'";
            return _dataLogic.LoadData(qry);
        }

        public bool SaveDepartment([FromBody] Tblcompanydepartment requestData)
        {
            DateTime dtNow = DateTime.Now;
            using var transaction = _context.Database.BeginTransaction();

            try
            {
              
                bool status = false;

                if (requestData.Id == 0)
                {
                    var existingDepartment = _context.Tblcompanydepartments
                    .FirstOrDefault(x => x.Department.Trim().Replace(" ", "") == requestData.Department.Trim().Replace(" ", "")
                         && x.CompId == auth.CmpId && x.LocId == auth.LocId);

                    if (existingDepartment != null)
                    {
                        return false;
                    }


                    requestData.Id = (_context.Tblcompanydepartments.Where(x => x.CompId == auth.CmpId && x.LocId == auth.LocId)
                        .Max(x => (int?)x.Id) ?? 0) + 1;
                    status = true;
                }

                _context.Tblcompanydepartments
                    .Where(x => x.Id == requestData.Id && x.CompId == auth.CmpId && x.LocId == auth.LocId)
                    .ExecuteDelete();


                _context.Tblcompanydepartments.Add(new Tblcompanydepartment
                {
                    Id = requestData.Id,
                    Department = requestData.Department,
                    CompId = auth.CmpId,
                    LocId = auth.LocId
                });


                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(requestData.Id, "Department", $"{((status == true) ? "Add" : "Edit")} Department - {requestData.Department} ", 0, dtNow, 0, 0, 0, dtNow);

                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }
        }

        public bool DelDepartment(int Id)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                bool isInUse = _context.TblEmployeeSetups.Any(t => t.DeptId == Id && t.CompId == auth.CmpId && t.LocId == auth.LocId) ||
                    _context.Tblemploysalarydts.Any(t=>t.DeptId == Id && t.CompId == auth.CmpId && t.LocId == auth.LocId);
                               

                if (isInUse)
                {
                    return false;
                }

                DateTime dtNow = DateTime.Now;

              var department =  _context.Tblcompanydepartments.Where(x => x.Id == Id && x.CompId == auth.CmpId && x.LocId == auth.LocId).FirstOrDefault();

                if (department == null)
                {
                    return false;
                }

                _context.Tblcompanydepartments.Remove(department);

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(Id, "Department", $"Deleted Department - {department.Department}", 0, dtNow, 0, 0, 0, dtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }

        }


        // Designation Entry


        public DataTable GetDesignationList()
        {
            String qry = $@"Select Id, Designation from tblcompanydesignation where comp_id = '"+ auth.CmpId +"' and locid = '"+ auth.LocId +"'";
            return _dataLogic.LoadData(qry);
        }

        public bool SaveDesignation([FromBody] Tblcompanydesignation requestData)
        {

            using var transaction = _context.Database.BeginTransaction();

            try
            {
                DateTime dtNow = DateTime.Now;
               

                bool status = false;


                if (requestData.Id == 0)
                {
                    var existingDepartment = _context.Tblcompanydesignations
                        .FirstOrDefault(x => x.Designation.Trim().Replace(" ", "") == requestData.Designation.Trim().Replace(" ", "")
                            && x.CompId == auth.CmpId && x.LocId == auth.LocId);

                    if (existingDepartment != null)
                    {
                        return false;
                    }


                    requestData.Id = (_context.Tblcompanydesignations.Where(x=>x.CompId == auth.CmpId && x.LocId == auth.LocId)
                        .Max(x => (int?)x.Id) ?? 0) + 1;
                    status = true;
                }

                _context.Tblcompanydesignations
                    .Where(x => x.Id == requestData.Id && x.CompId == auth.CmpId && x.LocId == auth.LocId)
                    .ExecuteDelete();


                _context.Tblcompanydesignations.Add(new Tblcompanydesignation
                {
                    Id = requestData.Id,
                    Designation = requestData.Designation,
                    CompId = auth.CmpId,
                    LocId = auth.LocId
                });


                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(requestData.Id, "Designation", $"{((status == true) ? "Add" : "Edit")} Designation - {requestData.Designation} ", 0, dtNow, 0, 0, 0, dtNow);

                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }
        }

        public bool DelDesignation(int Id)
        {
            bool isInUse = _context.TblEmployeeSetups.Any(t => t.DesgnId == Id && t.CompId == auth.CmpId && t.LocId == auth.LocId) ||
                _context.Tblemploysalarydts.Any(t => t.DesgId == Id && t.CompId == auth.CmpId && t.LocId == auth.LocId);


            if (isInUse)
            {
                return false;
            }


            DateTime dtNow = DateTime.Now;
            using var transaction = _context.Database.BeginTransaction();
            try
            {
               var designation = _context.Tblcompanydesignations.Where(x => x.Id == Id && x.CompId == auth.CmpId && x.LocId == auth.LocId).FirstOrDefault();

                if (designation == null)
                {
                    return false;
                }

                _context.Tblcompanydesignations.Remove(designation);

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(Id, "Department", $"Deleted Designation - {designation.Designation}", 0, dtNow, 0, 0, 0, dtNow);

                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }

        }


       // Holiday Setup

        public DataTable GetHolidayList()
        {
            String qry = $@"SELECT  Id, Holiday, From_Date, To_Date FROM tblholidaysetup  where comp_id = '"+ auth.CmpId +"' and locid = '"+ auth.LocId +"';";
            return _dataLogic.LoadData(qry);
        }

        public bool SaveHoliday([FromBody] Tblholidaysetup requestData)
        {

            using var transaction = _context.Database.BeginTransaction();

            try
            {
                DateTime dtNow = DateTime.Now;
                
                bool status = false;

                
                if (requestData.Id == 0)
                {
                    var existingHolidays = _context.Tblholidaysetups
                        .FirstOrDefault(x => x.Holiday.Trim().Replace(" ", "") == requestData.Holiday.Trim().Replace(" ", "")
                         && x.CompId == auth.CmpId && x.LocId == auth.LocId && x.FromDate == requestData.FromDate && x.ToDate == requestData.ToDate);

                    if (existingHolidays != null)
                    {
                        return false;
                    }


                    requestData.Id = (_context.Tblholidaysetups.Where(x => x.CompId == auth.CmpId && x.LocId == auth.LocId)
                        .Max(x => (int?)x.Id) ?? 0) + 1;

                    status = true;
                }

                _context.Tblholidaysetups
                    .Where(x => x.Id == requestData.Id && x.CompId == auth.CmpId && x.LocId == auth.LocId)
                    .ExecuteDelete();


                _context.Tblholidaysetups.Add(new Tblholidaysetup
                {
                    Id = requestData.Id,
                    Holiday = requestData.Holiday,
                    FromDate = requestData.FromDate,
                    ToDate = requestData.ToDate,
                    LocId = auth.LocId,
                    CompId = auth.CmpId
 
                });


                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(requestData.Id, "Holiday Setup", $"{((status == true) ? "Add" : "Edit")} Holiday - {requestData.Holiday} ", 0, dtNow, 0, 0, 0, dtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }
        }

        public bool DelHoliday(int Id)
        {
            DateTime dtNow = DateTime.Now;
            using var transaction = _context.Database.BeginTransaction();
            try
            {

              var holiday =  _context.Tblholidaysetups.Where(x => x.Id == Id && x.CompId == auth.CmpId && x.LocId == auth.LocId).FirstOrDefault();

                if(holiday is null)
                {
                    return false;
                }
                _context.Tblholidaysetups.Remove(holiday);

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(Id, "Department", $"Deleted Designation - {holiday.Holiday}", 0, dtNow, 0, 0, 0, dtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }

        }


        // Leaves Type Entry

        public bool SaveHrSetup([FromBody] Tblhrsetup requestData)
        {
            DateTime dtNow = DateTime.Now;
            using var transaction = _context.Database.BeginTransaction();

            try
            {


                bool status = false;

                if (requestData.HrSetupId == 0)
                {
                    var existingName = _context.Tblhrsetups
                     .FirstOrDefault(x => x.Name.Trim().Replace(" ", "") == requestData.Name.Trim().Replace(" ", "")
                        && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.Type == requestData.Type && x.Category == requestData.Category);

                    if (existingName != null)
                    {
                        return false;
                    }


                    requestData.HrSetupId = (_context.Tblhrsetups
                        .Where(x => x.CmpId == auth.CmpId && x.LocId == auth.LocId)
                        .Max(x => (int?)x.HrSetupId) ?? 0) + 1;
                    status = true;
                }

                _context.Tblhrsetups
                    .Where(x => x.CmpId == auth.CmpId && x.HrSetupId == requestData.HrSetupId && x.LocId == auth.LocId)
                    .ExecuteDelete();


                    _context.Tblhrsetups.Add(new Tblhrsetup
                    {
                        HrSetupId = requestData.HrSetupId,
                        Name = requestData.Name,
                        Category = requestData.Category,
                        Type = requestData.Type,
                        Amount = requestData.Amount,
                        CmpId = auth.CmpId,
                        LocId = auth.LocId
                    });


                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(requestData.HrSetupId, "Leaves Type", $"{((status == true) ? "Add" : "Edit")} Leaves - {requestData.Name} ", 0, dtNow, 0, 0, 0, dtNow);

                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }
        }


        public DataTable GetHrSetupList()
        {
            String qry = $@"Select HrSetupId, Type, Name, Category, Amount from tblhrsetup WHERE Cmp_Id = '{auth.CmpId}' and  LocID = '"+ auth.LocId +"'";
            return _dataLogic.LoadData(qry);
        }

        public bool DelHRSetup(int vchNo)
        {
            DateTime dtNow = DateTime.Now;
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                bool isInUse = _context.Tblempleaves.Any(t => t.LvId == vchNo && t.CompId == auth.CmpId && t.LocId == auth.LocId);


                if (isInUse)
                {
                    return false;
                }


                var leaves =  _context.Tblhrsetups.Where(x => x.HrSetupId == vchNo && x.CmpId == auth.CmpId && x.LocId == auth.LocId).ToList();
                if (leaves is null)
                {
                    return false;
                }
                foreach (var leave in leaves)
                {
                    _context.Tblhrsetups.Remove(leave);
                }

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(vchNo, "Leaves", $"Deleted Leaves", 0, dtNow, 0, 0, 0, dtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }

        }


        // Employee Type Entry

        public DataTable GetEmpTypeList()
        {
            String qry = $@"Select Id, EmployeeType from TblEmployeeType where comp_id = '"+auth.CmpId+"' and locid = '"+auth.LocId+"'";
            return _dataLogic.LoadData(qry);
        }

        public bool SaveEmployeeType([FromBody] TblEmployeeType requestData)
        {

            using var transaction = _context.Database.BeginTransaction();

            try
            {
                DateTime dtNow = DateTime.Now;
                
                bool status = false;

                if (requestData.Id == 0)
                {

                    var existingType = _context.TblEmployeeTypes
                     .FirstOrDefault(x => x.EmployeeType.Trim().Replace(" ", "") == requestData.EmployeeType.Trim().Replace(" ", "")
                        && x.CompId == auth.CmpId && x.LocId == auth.LocId);

                    if (existingType != null)
                    {
                        return false;
                    }


                    requestData.Id = (_context.TblEmployeeTypes.Where(x => x.CompId == auth.CmpId && x.LocId == auth.LocId)
                        .Max(x => (int?)x.Id) ?? 0) + 1;
                    status = true;
                }

                _context.TblEmployeeTypes
                    .Where(x => x.Id == requestData.Id && x.CompId == auth.CmpId && x.LocId == auth.LocId)
                    .ExecuteDelete();


                _context.TblEmployeeTypes.Add(new TblEmployeeType
                {
                    Id = requestData.Id,
                    EmployeeType = requestData.EmployeeType,
                    CompId = auth.CmpId,
                    LocId = auth.LocId

                });


                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(requestData.Id, "Employee Type", $"{((status == true) ? "Add" : "Edit")} Employee Type - {requestData.EmployeeType} ", 0, dtNow, 0, 0, 0, dtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }
        }

        public bool DelEmployeeType(int Id)
        {
           
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                bool isInUse = _context.Tblemploysalarydts.Any(t => t.EmpyType == Id && t.CompId == auth.CmpId && t.LocId == auth.LocId);


                if (isInUse)
                {
                    return false;
                }


                DateTime dtNow = DateTime.Now;

                var type =  _context.TblEmployeeTypes.Where(x => x.Id == Id && x.CompId == auth.CmpId && x.LocId == auth.LocId).FirstOrDefault();
                if (type == null)
                {
                    return false;
                }

                _context.TblEmployeeTypes.Remove(type);

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(Id, "Employee Type", $"Deleted Employee Type - {type.EmployeeType}", 0, dtNow, 0, 0, 0, dtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }

        }



        // Shift Entry

        public DataTable GetShiftList()
        {
            String qry = $@"Select Id, Shift from TblEmployeeShift where comp_id = '"+auth.CmpId+"' and locid = '"+auth.LocId+"'";
            return _dataLogic.LoadData(qry);
        }

        public bool SaveShift([FromBody] TblEmployeeShift requestData)
        {

            using var transaction = _context.Database.BeginTransaction();

            try
            {
                DateTime dtNow = DateTime.Now;

                bool status = false;

                if (requestData.Id == 0)
                {

                    var existingShift = _context.TblEmployeeShifts
                        .FirstOrDefault(x => x.Shift.Trim().Replace(" ", "") == requestData.Shift.Trim().Replace(" ", "")
                         && x.CompId == auth.CmpId && x.LocId == auth.LocId);

                    if (existingShift != null)
                    {
                        return false;
                    }


                    requestData.Id = (_context.TblEmployeeShifts.Where(x => x.CompId == auth.CmpId && x.LocId == auth.LocId)
                        .Max(x => (int?)x.Id) ?? 0) + 1;
                    status = true;
                }

                _context.TblEmployeeShifts
                    .Where(x => x.Id == requestData.Id && x.CompId == auth.CmpId && x.LocId == auth.LocId)
                    .ExecuteDelete();


                _context.TblEmployeeShifts.Add(new TblEmployeeShift
                {
                    Id = requestData.Id,
                    Shift = requestData.Shift,
                    CompId = auth.CmpId,
                    LocId = auth.LocId
                });


                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(requestData.Id, "Shift", $"{((status == true) ? "Add" : "Edit")} Shift - {requestData.Shift} ", 0, dtNow, 0, 0, 0, dtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }
        }

        public bool DelShift(int Id)
        {
            DateTime dtNow = DateTime.Now;
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                bool isInUse = _context.TblEmployeeSetups.Any(t => t.Shift == Id && t.CompId == auth.CmpId && t.LocId == auth.LocId);


                if (isInUse)
                {
                    return false;
                }


              var shift =  _context.TblEmployeeShifts.Where(x => x.Id == Id && x.CompId == auth.CmpId && x.LocId == auth.LocId).FirstOrDefault();
                if (shift == null)
                {
                    return false;
                }
                _context.TblEmployeeShifts.Remove(shift);

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(Id, "Shift", $"Deleted Shift - {shift.Shift}", 0, dtNow, 0, 0, 0, dtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }

        }


        // Salary Reason Entry

        public DataTable GetSalaryReasonList()
        {
            String qry = $@"Select Id, Reason from TblSalaryReason where comp_id = '"+auth.CmpId+"' and locid = '"+auth.LocId+"'";
            return _dataLogic.LoadData(qry);
        }

        public bool SaveSalaryReason([FromBody] TblSalaryReason requestData)
        {

            using var transaction = _context.Database.BeginTransaction();

            try
            {
                DateTime dtNow = DateTime.Now;

                bool status = false;


                if (requestData.Id == 0)
                {

                    var existingReason = _context.TblSalaryReasons
                        .FirstOrDefault(x => x.Reason.Trim().Replace(" ", "") == requestData.Reason.Trim().Replace(" ", "")
                            && x.CompId == auth.CmpId && x.LocId == auth.LocId);

                    if (existingReason != null)
                    {
                        return false;
                    }


                    requestData.Id = (_context.TblSalaryReasons.Where(x => x.CompId == auth.CmpId && x.LocId == auth.LocId)
                        .Max(x => (int?)x.Id) ?? 0) + 1;

                    status = true;
                }

                _context.TblSalaryReasons
                    .Where(x => x.Id == requestData.Id && x.CompId == auth.CmpId && x.LocId == auth.LocId)
                    .ExecuteDelete();


                _context.TblSalaryReasons.Add(new TblSalaryReason
                {
                    Id = requestData.Id,
                    Reason = requestData.Reason,
                    CompId = auth.CmpId,
                    LocId = auth.LocId
                });


                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(requestData.Id, "Employee Type", $"{((status == true) ? "Add" : "Edit")} Employee Type - {requestData.Reason} ", 0, dtNow, 0, 0, 0, dtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }
        }

        public bool DelSalaryReason(int Id)
        {
            DateTime dtNow = DateTime.Now;
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                bool isInUse = _context.Tblemploysalarydts.Any(t => t.Reasons == Id && t.CompId == auth.CmpId && t.LocId == auth.LocId);


                if (isInUse)
                {
                    return false;
                }


                var reason = _context.TblSalaryReasons.Where(x => x.Id == Id && x.CompId == auth.CmpId && x.LocId == auth.LocId).FirstOrDefault();

                if (reason == null)
                {
                    return false;
                }

                _context.TblSalaryReasons.Remove(reason);
                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(Id, "Salary Reason", $"Deleted Reason - {reason.Reason}", 0, dtNow, 0, 0, 0, dtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }

        }

        // Salary Settlement Labels

        public bool SaveLabels([FromBody] TblSalarydtLable requestData)
        {
            DateTime dtNow = DateTime.Now;
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                bool status = true;
               var labels = _context.TblSalarydtLables
                    .Where(x => x.LableCode == requestData.LableCode && x.CmpId == auth.CmpId && x.LocId == auth.LocId)
                    .FirstOrDefault();

                if (labels != null)
                {
                    _context.TblSalarydtLables.Remove(labels);
                    status = false;
                }


                _context.TblSalarydtLables.Add(new TblSalarydtLable
                {
                    LableCode = requestData.LableCode,
                    Lbl1 = requestData.Lbl1,
                    Lbl2 = requestData.Lbl2,
                    Lbl3 = requestData.Lbl3,
                    Lbl4 = requestData.Lbl4,
                    Lbl5 = requestData.Lbl5,
                    Lbl6 = requestData.Lbl6,
                    Lbl7 = requestData.Lbl7,
                    P1 = requestData.P1,
                    P2 = requestData.P2,
                    P3 = requestData.P3,
                    P4 = requestData.P4,
                    P5 = requestData.P5,
                    P6 = requestData.P6,
                    P7 = requestData.P7,
                    CmpId = auth.CmpId,
                    LocId = auth.LocId

                });


                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(requestData.LableCode, "Salary Label", $"{((status == true) ? "Add" : "Edit")} Salary Label - {requestData.LableCode} ", 0, dtNow, 0, 0, 0, dtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }
        }

        public DataTable GetSalaryLabels()
        {
            String qry = $@"Select * from tblSalarydtLables where cmp_id = '"+auth.CmpId+"' and locid = '"+auth.LocId+"'";

            return _dataLogic.LoadData(qry);
        }


        // Set Salary Days

        public bool SaveSalaryDays([FromBody] SalaryDay requestData)
        {
            DateTime dtNow = DateTime.Now;
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                bool status = true;
             var days=   _context.SalaryDays
                    .Where(x => x.Srno == requestData.Srno && x.CompId == auth.CmpId && x.LocId == auth.LocId)
                    .FirstOrDefault();

                if (days != null)
                {
                    _context.SalaryDays.Remove(days);
                    status = false;
                }


                _context.SalaryDays.Add(new SalaryDay
                {
                    Srno = requestData.Srno,
                    SalaryDays = requestData.SalaryDays,
                    CompId = auth.CmpId,
                    LocId = auth.LocId

                });

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(requestData.Srno, "Salary Days", $"{((status == true) ? "Add" : "Edit")} Salary Days - {requestData.SalaryDays} ", 0, dtNow, 0, 0, 0, dtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }
        }

        public DataTable GetSalaryDays()
        {
            String qry = $@"Select srno, SalaryDays from SalaryDays where Comp_id = '" + auth.CmpId + "' and LocId = '" + auth.LocId + "'";

            return _dataLogic.LoadData(qry);
        }



        // Set Month and Years

        public bool SaveMonthYear([FromBody] TblMonth requestData)
        {

            using var transaction = _context.Database.BeginTransaction();

            try
            {
                DateTime dtNow = DateTime.Now;
                bool status = true;
               var month = _context.TblMonths
                    .Where(x => x.FinId == requestData.FinId && x.CompId == auth.CmpId && x.LocId == auth.LocId)
                    .FirstOrDefault();

                if (month != null)
                {
                    _context.TblMonths.Remove(month);
                    status = false;
                }


                _context.TblMonths.Add(new TblMonth
                {
                    Mnth = requestData.Mnth,
                    Year = requestData.Year,
                    CompId = auth.CmpId,
                    LocId = auth.LocId,
                    FinId = requestData.FinId

                });

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(requestData.FinId, "Salary Month", $"{((status == true) ? "Add" : "Edit")} Salary Month - {requestData.Mnth} ", 0, dtNow, 0, 0, 0, dtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }
        }

        public DataTable GetMonthYear()
        {
            String qry = $@"Select mnth, year, finID from tblMonth where Comp_id = '" + auth.CmpId + "' and LocId = '" + auth.LocId + "'";

            return _dataLogic.LoadData(qry);
        }


        // Overtime Formula Calculation

        public bool SaveOvertimeFormula([FromBody] Tblotformula requestData)
        {

            using var transaction = _context.Database.BeginTransaction();

            try
            {
                DateTime dtNow = DateTime.Now;
                bool status = true;

               var formula = _context.Tblotformulas
                    .Where(x => x.Id == requestData.Id && x.CompId == auth.CmpId && x.LocId == auth.LocId)
                    .FirstOrDefault();

                if (formula != null)
                {
                    _context.Tblotformulas.Remove(formula);
                    status = false;
                }

                _context.Tblotformulas.Add(new Tblotformula
                {
                    Id = requestData.Id,
                    Formula = requestData.Formula,
                    CompId = auth.CmpId,
                    LocId = auth.LocId

                });


                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(requestData.Id, "Overtime Formula", $"{((status == true) ? "Add" : "Edit")} Overtime Formula - {requestData.Formula} ", 0, dtNow, 0, 0, 0, dtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }
        }

        public DataTable GetOvertimeFormula()
        {
            String qry = $@"Select * from tblotformula where comp_id = '"+auth.CmpId+"' and locid = '"+auth.LocId+"'";

            return _dataLogic.LoadData(qry);
        }
    }
}
