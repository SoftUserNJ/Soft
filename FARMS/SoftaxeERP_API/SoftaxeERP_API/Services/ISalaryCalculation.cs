using DevExpress.Office;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftaxeERP_API.Models;
using SoftaxeERP_API.VM;
using System.Data;

namespace SoftaxeERP_API.Services
{
    public interface ISalaryCalculation
    {
        DataTable GetSalaryTypeList();
        bool SaveSalaryType([FromBody] TblSalaryType requestData);
        bool DelSalaryType(int Id);

        DataTable getSalaryPayables(int Month, int year);


    }
    public class SalaryCalculation : ISalaryCalculation
    {
        private readonly ErpSoftaxeContext _context;
        private readonly IDataLogic _dataLogic;
        private readonly IAuth _auth;
        readonly AuthVM auth = new();
        public SalaryCalculation(ErpSoftaxeContext context, IDataLogic dataLogic, IAuth authData)
        {
            _context = context;
            _dataLogic = dataLogic;
            _auth = authData;
            auth = _auth.GetUserData();
        }

        // Salary Type Entry

        public DataTable GetSalaryTypeList()
        {
            String qry = $@"Select Id, SalaryType from TblSalaryType where LocId = '"+auth.LocId+"' and Comp_id = '"+auth.CmpId+"'";
            return _dataLogic.LoadData(qry);
        }

        public bool SaveSalaryType([FromBody] TblSalaryType requestData)
        {

            using var transaction = _context.Database.BeginTransaction();

            try
            {
                DateTime dtNow = DateTime.Now;
               
                bool status = false;


                if (requestData.Id == 0)
                {

                    var existingType = _context.TblSalaryTypes
                     .FirstOrDefault(x => x.SalaryType.Trim().Replace(" ", "") == requestData.SalaryType.Trim().Replace(" ", "")
                            && x.CompId == auth.CmpId && x.LocId == auth.LocId);

                    if (existingType != null)
                    {
                        return false;
                    }



                    requestData.Id = (_context.TblSalaryTypes.Where(x => x.CompId == auth.CmpId && x.LocId == auth.LocId)
                        .Max(x => (int?)x.Id) ?? 0) + 1;
                    status = true;
                }

                _context.TblSalaryTypes
                    .Where(x => x.Id == requestData.Id && x.CompId == auth.CmpId && x.LocId == auth.LocId)
                    .ExecuteDelete();


                _context.TblSalaryTypes.Add(new TblSalaryType
                {
                    Id = requestData.Id,
                    SalaryType = requestData.SalaryType,
                    CompId = auth.CmpId,
                    LocId = auth.LocId
                });


                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(requestData.Id, "Salary Type", $"{((status == true) ? "Add" : "Edit")} Salary Type - {requestData.SalaryType} ", 0, dtNow, 0, 0, 0, dtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }
        }

        public bool DelSalaryType(int Id)
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
              var  type =  _context.TblSalaryTypes.Where(x => x.Id == Id && x.CompId == auth.CmpId && x.LocId == auth.LocId).FirstOrDefault();
                if (type == null)
                {
                    return false;
                }

                _context.TblSalaryTypes.Remove(type);

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(Id, "Salary Type", $"Deleted Salary Type - {type.SalaryType}", 0, dtNow, 0, 0, 0, dtNow);

                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }

        }


        // Salary Payable
        public DataTable getSalaryPayables(int Month, int year)
        {
            String qry = $@" Select empy_id, Name, Department, Designation, isnull(bsalaryA,0) as basic, gsalary, netsalary, isnull(LV,0) as Leave, isnull(ILI,0) as Insurance, isnull(PLI,0) as ProvidentLoan,
                            isnull(AD,0) as Advance, isnull(Bonus,0) as Bonus, isnull(SLI,0) as StaffLoan, isnull(VLI,0) as VehicleLoan, isnull(EOBI,0) as EOBI, isnull(Level2A,0) as Level2, isnull(Level3A,0) as Level3, isnull(Level4A,0) as Level4,
                            isnull(Level5A,0) as Level5, isnull(Level6A,0) as Level6, isnull(Level7A,0) as Level7, l.LocName
                            from tblSalary s
                            Left Join location l on l.LocID = s.LocId and l.Cmp_id = s.Comp_id
                            where Mnth = '" +Month+"' and yr = '"+year+"' and s.Comp_id = '"+auth.CmpId+"' and s.LocId = '"+auth.LocId+"'";
            return _dataLogic.LoadData(qry);
        }
    }
}
