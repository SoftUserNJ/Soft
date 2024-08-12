using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SoftaxeERP_API.Models;
using SoftaxeERP_API.VM;
using System.Data;

namespace SoftaxeERP_API.Services
{
    public interface IEmployeeIncentives
    {
        // Employe Incentives

        string SaveEmpIncentive([FromBody] Tblinsentive requestData);

        DataTable GetEditIncentive(int empy_id);

        bool deleteIncentive(int empy_id, int SrNo);

        // Employe Overtime

        string SaveEmployeeOvertime([FromBody] TblOverTime requestData);

        string GetEditOvertime(int empy_id);

        bool DelOvertime(int empy_id, int SrNo);

        // Arrears
        string SaveEmpArrears([FromBody] TblArrear requestData);

        DataTable GetEditArrearsList(int empy_id);

        bool deleteEmpArrears(int empy_id, int SrNo);

        // Leave Enchasement 

        string SaveLeaveEnchasment([FromBody] TblLvEnchasment requestData);

        DataTable GetEditLeaveEnchasment(int empy_id);

        bool deleteLeaveEnchasment(int empy_id, int SrNo);

        // Yearly Bounus

        string SaveYearlyBonus([FromBody] TblYearlybonu requestData);
        DataTable GetEditBonusList(int empy_id);
        bool DelYearlyBonus(int empy_id, int SrNo);

    }

    public class EmployeeIncentives : IEmployeeIncentives
    {
        private readonly ErpSoftaxeContext _context;
        private readonly IDataLogic _dataLogic;
        private readonly IAuth _auth;
        readonly AuthVM auth = new();
        public EmployeeIncentives(ErpSoftaxeContext context, IDataLogic dataLogic, IAuth authData)
        {
            _context = context;
            _dataLogic = dataLogic;
            _auth = authData;
            auth = _auth.GetUserData();
        }
        // Employee Incentives
        public string SaveEmpIncentive([FromBody] Tblinsentive requestData)
        {
            DateTime dtNow = DateTime.Now;

            using var transaction = _context.Database.BeginTransaction();


            try
            {

                if (requestData.Srno == 0)
                {
                    requestData.Srno = (_context.Tblinsentives
                     .Where(x => x.CompId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId && x.EmpyId == requestData.EmpyId)
                        .Max(x => (int?)x.Srno) ?? 0) + 1;
                }

                _context.Tblinsentives
                        .Where(x => x.Srno == requestData.Srno && x.CompId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId && x.EmpyId == requestData.EmpyId)
                    .ExecuteDelete();


                    _context.Tblinsentives.Add(new Tblinsentive
                    {
                        CompId = auth.CmpId,
                        Srno = requestData.Srno,
                        EmpyId = requestData.EmpyId,
                        Trdate = requestData.Trdate,
                        Remarks = requestData.Remarks,
                        LocId = auth.LocId,
                        Tel = requestData.Tel,
                        Pet = requestData.Pet,
                        Tada = requestData.Tada,
                        Maint = requestData.Maint,
                        Other = requestData.Other,
                        Total = requestData.Total,
                        Gym = requestData.Gym,
                        Medical = requestData.Medical,
                        Family = requestData.Family,
                        Security = requestData.Security,
                        FinId = auth.FinId


                    });

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(requestData.EmpyId, "Employee Incentive", $"Add/Edit Employee Incentive - Employee Id - {requestData.EmpyId} - Vch No {requestData.Srno}", 0, dtNow, 0, 0, 0, dtNow);
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
                throw;

            }



        }

        public DataTable GetEditIncentive(int empy_id)
        {
            String qry = $@"Select d.Id, emp.name as EmpName, d.empy_id, 
                            d.srno, d.trdate, d.tel, d.pet, d.tada, d.Remarks, d.maint,
                            d.other, d.total, d.gym, d.medical, d.family, d.security
                            from tblinsentive d
                            Join tblEmployeeSetup emp On emp.empy_id = d.empy_id and d.Comp_id = emp.comp_id and d.LocId = emp.LocId
                            where d.comp_id = '" + auth.CmpId + "' and d.empy_id = '" + empy_id + "' And d.LocId = '" + auth.LocId + "' and d.FinId = '"+auth.FinId+"'";

            return _dataLogic.LoadData(qry);
        }

        public bool deleteIncentive(int empy_id, int SrNo)
        {
            DateTime dtNow = DateTime.Now;
            using var transaction = _context.Database.BeginTransaction();
            try
            {

                _context.Tblinsentives.Where(x => x.EmpyId == empy_id && x.CompId == auth.CmpId && x.FinId == auth.FinId && x.Srno == SrNo && x.LocId == auth.LocId).ExecuteDelete();

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(empy_id, "Employee Incentive", $"Deleted Employee Incentive - Employee Id: {empy_id} and VchNo {SrNo}", 0, dtNow, 0, 0, 0, dtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }
        }

        // Arrears

        public string SaveEmpArrears([FromBody] TblArrear requestData)
        {

            using var transaction = _context.Database.BeginTransaction();

            try
            {

                if (requestData.Srno == 0)
                {
                    requestData.Srno = (_context.TblArrears
                     .Where(x => x.CompId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId && x.EmpyId == requestData.EmpyId)
                        .Max(x => (int?)x.Srno) ?? 0) + 1;
                }

                _context.TblArrears
                        .Where(x => x.Srno == requestData.Srno && x.CompId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId && x.EmpyId == requestData.EmpyId)
                    .ExecuteDelete();

                DateTime dtNow = DateTime.Now;

                    _context.TblArrears.Add(new TblArrear
                    {
                        CompId = auth.CmpId,
                        Srno = requestData.Srno,
                        EmpyId = requestData.EmpyId,
                        Stdate = requestData.Stdate,
                        Trdate = dtNow,
                        Remarks = requestData.Remarks,
                        LocId = auth.LocId,
                        Reference = requestData.Reference,
                        Arrears = requestData.Arrears,
                        FinId = auth.FinId


                    });


                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(requestData.EmpyId, "Employee Arrears", $"Add/Edit Employee Arrears - Employee Id - {requestData.EmpyId} VchNo: {requestData.Srno}", 0, dtNow, 0, 0, 0, dtNow);
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
                throw;

            }



        }

        public DataTable GetEditArrearsList(int empy_id)
        {
            String qry = $@"Select d.Id, emp.name as EmpName, d.empy_id, 
                        d.srno, d.Remarks, d.stdate, d.Arrears, d.Reference
                        from tblArrears d
                        Join tblEmployeeSetup emp On emp.empy_id = d.empy_id and d.Comp_id = emp.comp_id and d.LocId = emp.LocId
                        where d.comp_id = '" + auth.CmpId + "' and d.empy_id = '" + empy_id + "'  And d.LocId = '" + auth.LocId + "'";

            return _dataLogic.LoadData(qry);
        }

        public bool deleteEmpArrears(int empy_id, int SrNo)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                DateTime dtNow = DateTime.Now;
                _context.TblArrears.Where(x => x.EmpyId == empy_id && x.CompId == auth.CmpId && x.FinId == auth.FinId && x.Srno == SrNo && x.LocId == auth.LocId).ExecuteDelete();

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(empy_id, "Employee Arrears", $"Deleted Employee Arrears - Employee Id: {empy_id} VchNo: {SrNo}", 0, dtNow, 0, 0, 0, dtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }
        }

        // Yearly Bonus

        public string SaveYearlyBonus([FromBody] TblYearlybonu requestData)
        {
            DateTime dtNow = DateTime.Now;
            using var transaction = _context.Database.BeginTransaction();

            try
            {


                if (requestData.Srno == 0)
                {
                    requestData.Srno = (_context.TblYearlybonus
                     .Where(x => x.CompId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId && x.EmpyId == requestData.EmpyId)
                        .Max(x => (int?)x.Srno) ?? 0) + 1;
                }

                _context.TblYearlybonus
                        .Where(x => x.Srno == requestData.Srno && x.CompId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId && x.EmpyId == requestData.EmpyId)
                    .ExecuteDelete();

                    _context.TblYearlybonus.Add(new TblYearlybonu
                    {
                        CompId = auth.CmpId,
                        Srno = requestData.Srno,
                        EmpyId = requestData.EmpyId,
                        Stdate = requestData.Stdate,
                        Remarks = requestData.Remarks,
                        YearlyBonus = requestData.YearlyBonus,
                        Trdate = dtNow,
                        Reference = requestData.Reference,
                        LocId = auth.LocId,
                        FinId = auth.FinId




                    });

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(requestData.EmpyId, "Yearly Bonus", $"Add/Edit Yearly Bonus - Employee Id - {requestData.EmpyId} VcHno: {requestData.Srno}", 0, dtNow, 0, 0, 0, dtNow);
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
                throw;

            }



        }

        public DataTable GetEditBonusList(int empy_id)
        {
            String qry = $@"Select d.Id, emp.name as EmpName, d.empy_id, 
                        d.srno, d.stDate, d.Reference, d.YearlyBonus, d.Remarks
                        from tblyearlyBonus d
                        Join tblEmployeeSetup emp On emp.empy_id = d.empy_id and d.Comp_id = emp.comp_id and d.LocId = emp.LocId
                        where d.comp_id = '" + auth.CmpId + "' and d.empy_id = '" + empy_id + "' And d.LocId = '" + auth.LocId + "'";

            return _dataLogic.LoadData(qry);
        }


        public bool DelYearlyBonus(int empy_id, int SrNo)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                DateTime dtNow = DateTime.Now;
                _context.TblYearlybonus.Where(x => x.EmpyId == empy_id && x.CompId == auth.CmpId && x.FinId == auth.FinId && x.Srno == SrNo && x.LocId == auth.LocId).ExecuteDelete();

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(empy_id, "Yearly Bonus", $"Deleted Yearly Bonus - Employee Id: {empy_id}", 0, dtNow, 0, 0, 0, dtNow);

                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }
        }

        // Leave Enchasement

        public string SaveLeaveEnchasment([FromBody] TblLvEnchasment requestData)
        {
          
            using var transaction = _context.Database.BeginTransaction();
            try
            {

                if (requestData.Srno == 0)
                {
                    requestData.Srno = (_context.TblLvEnchasments
                     .Where(x => x.CompId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId && x.EmpyId == requestData.EmpyId)
                        .Max(x => (int?)x.Srno) ?? 0) + 1;
                }

                _context.TblLvEnchasments
                        .Where(x => x.Srno == requestData.Srno && x.CompId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId && x.EmpyId == requestData.EmpyId)
                    .ExecuteDelete();

                DateTime dtNow = DateTime.Now;

                _context.TblLvEnchasments.Add(new TblLvEnchasment
                {
                    CompId = auth.CmpId,
                    EmpyId = requestData.EmpyId,
                    LocId = auth.LocId,
                    Srno = requestData.Srno,
                    Stdate = requestData.Stdate,
                    Grosssalary = requestData.Grosssalary,
                    Percentage = requestData.Percentage,
                    Lv = requestData.Lv,
                    Lvpaid = requestData.Lvpaid,
                    Lvbalance = requestData.Lvbalance,
                    Bamount = requestData.Bamount,
                    Itax = requestData.Itax,
                    Eobi = requestData.Eobi,
                    Pf = requestData.Pf,
                    Loan = requestData.Loan,
                    Vloan = requestData.Vloan,
                    Bonus = requestData.Bonus,
                    Remarks = requestData.Remarks,
                    Trdate = dtNow,
                    Reference = requestData.Reference,
                    FinId = auth.FinId
                    


                });

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(requestData.EmpyId, "Leave Enchasement", $"Add/Edit Leave Enchasement - Employee Id - {requestData.EmpyId} VchNo: {requestData.Srno}", 0, dtNow, 0, 0, 0, dtNow);

                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
                throw;

            }



        }

        public DataTable GetEditLeaveEnchasment(int empy_id)
        {
            String qry = $@"Select d.Id, emp.name as EmpName, d.empy_id, 
                        d.srno, d.Remarks, d.stdate, d.GROSSSALARY, d.Percentage, d.LV, d.LVPAID, d.LVBALANCE, d.BAMOUNT, d.ITAX, d.EOBI, d.PF, d.Loan,
                        d.vloan, d.Bonus, d.Reference
                        from tblLvEnchasment d
                        Join tblEmployeeSetup emp On emp.empy_id = d.empy_id and d.Comp_id = emp.comp_id and d.LocId = emp.LocId
                        where d.comp_id = '" + auth.CmpId + "' and d.empy_id = '" + empy_id + "'  And d.LocId = '" + auth.LocId + "' and d.FinId = '"+auth.FinId+"'";

            return _dataLogic.LoadData(qry);
        }

        public bool deleteLeaveEnchasment(int empy_id, int SrNo)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                DateTime dtNow = DateTime.Now;
                _context.TblLvEnchasments.Where(x => x.EmpyId == empy_id && x.CompId == auth.CmpId && x.FinId == auth.FinId && x.Srno == SrNo && x.LocId == auth.LocId).ExecuteDelete();

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(empy_id, "Leave Echasement", $"Deleted Leave Enchasement - Employee Id: {empy_id} VchNo {SrNo}", 0, dtNow, 0, 0, 0, dtNow);

                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }
        }

        public string SaveEmployeeOvertime([FromBody] TblOverTime requestData)
        {
           
            using var transaction = _context.Database.BeginTransaction();

            try
            {


                if (requestData.Srno == 0)
                {
                    requestData.Srno = (_context.TblOverTimes
                     .Where(x => x.CompId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId && x.EmpyId == requestData.EmpyId)
                        .Max(x => (int?)x.Srno) ?? 0) + 1;
                }

                _context.TblOverTimes
                        .Where(x => x.Srno == requestData.Srno && x.CompId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId && x.EmpyId == requestData.EmpyId)
                    .ExecuteDelete();

                DateTime dtNow = DateTime.Now;


                    _context.TblOverTimes.Add(new TblOverTime
                    {
                        CompId = auth.CmpId,
                        Srno = requestData.Srno,
                        EmpyId = requestData.EmpyId,
                        Stdate = requestData.Stdate,
                        Remarks = requestData.Remarks,
                        LocId = auth.LocId,
                        OverTimeAmount = requestData.OverTimeAmount,
                        Trdate = dtNow,
                        PerHourRate = requestData.PerHourRate,
                        TotalHrs = requestData.TotalHrs,
                        FinId = requestData.FinId

                    });

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(requestData.EmpyId, "Employee Overtime", $"Add/Edit Employee Overtime - Employee Id - {requestData.EmpyId} - VchNo = {requestData.Srno}", 0, dtNow, 0, 0, 0, dtNow);

                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
                throw;

            }



        }

        public bool DelOvertime(int empy_id, int SrNo)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                DateTime dtNow = DateTime.Now;
                _context.TblOverTimes.Where(x => x.EmpyId == empy_id && x.CompId == auth.CmpId && x.FinId == auth.FinId && x.Srno == SrNo && x.LocId == auth.LocId).ExecuteDelete();

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(empy_id, "Employee OverTime", $"Deleted Employee Ovetime - Employee Id: {empy_id} - VchNo = {SrNo}", 0, dtNow, 0, 0, 0, dtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }
        }

        public string GetEditOvertime(int empy_id)
        {
            string qry1 = @"Select d.Id, emp.name as EmpName, d.empy_id, 
                        d.srno, d.stDate, d.OverTimeAmount, d.PerHourRate, d.TotalHrs, d.Remarks
                        from tblOvertime d
                        Join tblEmployeeSetup emp On emp.empy_id = d.empy_id and d.Comp_id = emp.comp_id and d.LocId = emp.LocId
                        where d.comp_id = '" + auth.CmpId + "' and d.empy_id = '" + empy_id + "' And d.LocId = '" + auth.LocId + "'";

            string qry2 = @"Select formula from tblotformula where comp_id = '" + auth.CmpId + "' AND LocId = '" + auth.LocId + "'";

            string qry4 = @"Select salaryDays from SalaryDays where comp_id = '" + auth.CmpId + "' AND LocId = '" + auth.LocId + "'";

            string qry3 = @"Select gsalary from tblemploysalarydt where empy_id = '" + empy_id + "' and comp_id = '" + auth.CmpId + "' AND LocId = '" + auth.LocId + "'";


            var dt1 = _dataLogic.LoadData(qry1);
            var dt2 = _dataLogic.LoadData(qry2);
            var dt3 = _dataLogic.LoadData(qry3);
            var dt4 = _dataLogic.LoadData(qry4);

            return JsonConvert.SerializeObject(new { EmpData = dt1, Formula = dt2, Gross = dt3, SalarDays = dt4 });


        }



    }
}
