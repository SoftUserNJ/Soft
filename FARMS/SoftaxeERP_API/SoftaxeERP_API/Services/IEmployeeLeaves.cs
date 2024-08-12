using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SoftaxeERP_API.Models;
using SoftaxeERP_API.VM;
using System.Data;

namespace SoftaxeERP_API.Services
{
    public interface IEmployeeLeaves
    {
        DataTable GetLeaveType();

        string SaveLeavesEntry([FromBody] TblleavesEntry requestData);

        string GetEditEmpLeaves(int empy_id);

        bool DelLeaves(int empy_id, int SrNo);
    }

    public class EmployeeLeaves : IEmployeeLeaves
    {
        private readonly ErpSoftaxeContext _context;
        private readonly IDataLogic _dataLogic;
        private readonly IAuth _auth;
        readonly AuthVM auth = new();
        public EmployeeLeaves(ErpSoftaxeContext context, IDataLogic dataLogic, IAuth authData)
        {
            _context = context;
            _dataLogic = dataLogic;
            _auth = authData;
            auth = _auth.GetUserData();
        }

        // Leaves Entry
         public DataTable GetLeaveType()
        {
            String qry = $@"Select Name, HrSetupId from TBLHRSetup where cmp_id = '"+auth.CmpId+"' and locid = '"+auth.LocId+"'";

            return _dataLogic.LoadData(qry);
        }


        public string SaveLeavesEntry([FromBody] TblleavesEntry requestData)
        {
            
            using var transaction = _context.Database.BeginTransaction();

            try
            {

                if (requestData.Srno == 0)
                {
                    requestData.Srno = (_context.TblleavesEntries
                     .Where(x => x.CompId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId && x.EmpyId == requestData.EmpyId)
                        .Max(x => (int?)x.Srno) ?? 0) + 1;
                }

                _context.TblleavesEntries
                        .Where(x => x.Srno == requestData.Srno && x.CompId == auth.CmpId && x.FinId == auth.FinId && x.LocId == auth.LocId && x.EmpyId == requestData.EmpyId)
                    .ExecuteDelete();


                DateTime dtNow = DateTime.Now;
               

                    _context.TblleavesEntries.Add(new TblleavesEntry
                    {
                        CompId = auth.CmpId,
                        Srno = requestData.Srno,
                        EmpyId = requestData.EmpyId,
                        Stdate = requestData.Stdate,
                        Remarks = requestData.Remarks,
                        LocId = auth.LocId,
                        EndDate = requestData.EndDate,
                        Nod = requestData.Nod,
                        TotalLeaves = requestData.TotalLeaves,
                        Vch = requestData.Vch,
                        Date = requestData.Date,
                        LvId = requestData.LvId,
                        FinId = auth.FinId



                    });

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(requestData.EmpyId, "Employee Leaves", $"Add/Edit Employee Leaves - Employee Id - {requestData.EmpyId} VchNo {requestData.Srno}", 0, dtNow, 0, 0, 0, dtNow);
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
                throw;

            }



        }

        public string GetEditEmpLeaves(int empy_id)
        {
            String qry1 = $@"Select d.Id, emp.name as EmpName, d.empy_id,
                d.srno, d.stDate, d.EndDate, d.Date, d.Lv_id, l.Name as LeaveName, d.NOD, d.TotalLeaves, d.Remarks
                from tblleavesEntry d
                Join tblEmployeeSetup emp On emp.empy_id = d.empy_id
                Join TBLHRSetup l On l.HrSetupId = d.Lv_id and d.Comp_id = emp.comp_id and d.LocId = emp.LocId
                where d.comp_id = '" + auth.CmpId + "' and d.empy_id = '" + empy_id +"'  And d.LocId = '" + auth.LocId + "' and d.Finid = '"+auth.FinId+"'";

            string qry2 = @"SELECT el.empy_id, el.lv_id, el.NoOfLvs, e.NOD, COALESCE((el.NoOfLvs - e.NOD), el.NoOfLvs) AS Total, h.Name AS LvName
                    FROM tblempleaves el
                    INNER JOIN TBLHRSetup h ON h.HrSetupId = el.lv_id and h.Cmp_id = el.comp_id and h.LocId = el.LocId
                    LEFT JOIN tblleavesEntry e ON el.lv_id = e.Lv_id AND el.empy_id = e.empy_id  and el.comp_id = e.comp_id and el.LocId = e.LocId
                    WHERE el.empy_id = '"+ empy_id + "' and el.Comp_id = '"+auth.CmpId+"' and el.locId = '"+auth.LocId+"'";


            var dt1 = _dataLogic.LoadData(qry1);
            var dt2 = _dataLogic.LoadData(qry2);

            return JsonConvert.SerializeObject(new { EmpData = dt1, LeaveData = dt2 });

        }


        public bool DelLeaves(int empy_id, int SrNo)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                DateTime dtNow = DateTime.Now;
                _context.TblleavesEntries.Where(x => x.EmpyId == empy_id && x.CompId == auth.CmpId && x.FinId == auth.FinId && x.Srno == SrNo && x.LocId == auth.LocId).ExecuteDelete();

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(empy_id, "Employee Leaves", $"Deleted Employee Leaves - Employee Id: {empy_id}", 0, dtNow, 0, 0, 0, dtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }
        }

    }
}
