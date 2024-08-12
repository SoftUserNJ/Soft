using Newtonsoft.Json;
using SoftaxeERP_API.Models;
using SoftaxeERP_API.VM;
using System.Data;

namespace SoftaxeERP_API.Services
{
    public interface IAudit
    {
        string GetVouchers(string type);
        string UpdateVoucherStatus(List<VoucherApprovalVM> status);

        bool AllowSameLeave(int empy_id);

    }

    public class Audit : IAudit
    {
        private readonly ErpSoftaxeContext _context;
        private readonly IDataLogic _dataLogic;
        private readonly IAuth _auth;
        readonly AuthVM auth = new();
        public Audit(ErpSoftaxeContext context, IDataLogic dataLogic, IAuth authData)
        {
            _context = context;
            _dataLogic = dataLogic;
            _auth = authData;
            auth = _auth.GetUserData();
        }

        // Vocuher Approval
        public string GetVouchers(string type)
        {
            if (type == "Staff Loan")
            {

                String qry = $@"Select l.empy_id, e.Name as EmpName, e.tumbid, l.SrNo as VchNo,
                            l.stdate as VchDate, l.loanamt as Amount, l.remarks as Remarks, 'Staff Loan' Type, l.sent  from tblStaffLoan l
                            Inner Join tblEmployeeSetup e On l.empy_id = e.empy_id and e.Comp_id = l.comp_id and e.LocId = l.LocId
                            Where l.comp_id = '" + auth.CmpId + "' and l.LocId = '" + auth.LocId + "'";

                DataTable data = _dataLogic.LoadData(qry);
                string result = JsonConvert.SerializeObject(data);
                return result;
            }

            if (type == "Vehicle Loan")
            {

                String qry = $@"Select l.empy_id, e.Name as EmpName, e.tumbid, l.SrNo as VchNo,
                            l.stdate as VchDate, l.loanamt as Amount, l.remarks as Remarks, 'Vehicle Loan' Type, l.sent  from tblVehicleLoan l
                            Inner Join tblEmployeeSetup e On l.empy_id = e.empy_id  and e.Comp_id = l.comp_id and e.LocId = l.LocId
                            Where l.comp_id = '" + auth.CmpId + "' and l.LocId = '" + auth.LocId + "'";

                DataTable data = _dataLogic.LoadData(qry);
                string result = JsonConvert.SerializeObject(data);
                return result;
            }

            if (type == "Income Tax")
            {
                String qry = $@"Select  d.empy_id, e.Name as EmpName, e.tumbid, d.SrNo as VchNo, d.stDate as VchDate, d.icomeTaxdeducation as Amount,
                               d.Remarks as Remarks, Type 'Income Tax', d.sent from tblIncomeTax  d
                               Inner Join tblEmployeeSetup e On d.empy_id = e.empy_id and e.Comp_id = d.comp_id and e.LocId = d.LocId
                               Where  d.comp_id = '" + auth.CmpId + "' and d.LocId = '" + auth.LocId + "'";

                DataTable data = _dataLogic.LoadData(qry);
                string result = JsonConvert.SerializeObject(data);
                return result;
            }

            if (type == "EOBI")
            {
                String qry = $@"Select  d.empy_id, e.Name as EmpName, e.tumbid, d.SrNo as VchNo, d.stDate as VchDate, d.EobiDeducation as Amount,
                               d.Remarks as Remarks, Type 'Income Tax', d.sent from tblEOBI  d
                               Inner Join tblEmployeeSetup e On d.empy_id = e.empy_id and e.Comp_id = d.comp_id and e.LocId = d.LocId
                               Where  d.comp_id = '" + auth.CmpId + "' and d.LocId = '" + auth.LocId + "'";

                DataTable data = _dataLogic.LoadData(qry);
                string result = JsonConvert.SerializeObject(data);
                return result;
            }


            if (type == "Advance Salary")
            {
                String qry = $@"Select  d.empy_id, e.Name as EmpName, e.tumbid, d.SrNo as VchNo, d.stDate as VchDate, d.AdvanceSalary as Amount, d.Remarks as Remarks, d.sent,  'Advance Salary' Type
                              from tblAdvanceSalary  d
                              Inner Join tblEmployeeSetup e On d.empy_id = e.empy_id and e.Comp_id = d.comp_id and e.LocId = d.LocId
                              Where d.comp_id = '" + auth.CmpId + "' and d.LocId = '" + auth.LocId + "'";

                DataTable data = _dataLogic.LoadData(qry);
                string result = JsonConvert.SerializeObject(data);
                return result;
            }

            return "No Records Found";
        }


        public string UpdateVoucherStatus(List<VoucherApprovalVM> status)
        {
            DateTime dtNow = DateTime.Now;
            using var transaction = _context.Database.BeginTransaction();
            VoucherApprovalVM approval = status.First();

            try
            {

                if (approval.VchType == "Staff Loan")
                {
                    foreach (var item in status)
                    {
                        var Vch = _context.TblStaffLoans.Where(e => e.EmpyId == item.EmpyId && e.Srno == item.SrNo && e.CompId == auth.CmpId && e.LocId == auth.LocId).ToList();

                        foreach (var empLoan in Vch)
                        {

                            empLoan.Sent = item.IsApproved;
                        }

                    }

                }

                if (approval.VchType == "Vehicle Loan")
                {
                    foreach (var item in status)
                    {
                        var Vch = _context.TblVehicleLoans.Where(e => e.EmpyId == item.EmpyId && e.Srno == item.SrNo && e.CompId == auth.CmpId && e.LocId == auth.LocId).ToList();

                        foreach (var empLoan in Vch)
                        {

                            empLoan.Sent = item.IsApproved;
                        }

                    }

                }

                if (approval.VchType == "EOBI")
                {
                    foreach (var item in status)
                    {
                        var Vch = _context.TblEobis.Where(e => e.EmpyId == item.EmpyId && e.Srno == item.SrNo && e.CompId == auth.CmpId && e.LocId == auth.LocId).ToList();

                        foreach (var empLoan in Vch)
                        {

                            empLoan.Sent = item.IsApproved;
                        }

                    }

                }


                if (approval.VchType == "Income Tax")
                {
                    foreach (var item in status)
                    {
                        var Vch = _context.TblIncomeTaxes.Where(e => e.EmpyId == item.EmpyId && e.Srno == item.SrNo && e.CompId == auth.CmpId && e.LocId == auth.LocId).ToList();

                        foreach (var empLoan in Vch)
                        {

                            empLoan.Sent = item.IsApproved;
                        }

                    }

                }


                if (approval.VchType == "Advance Salary")
                {
                    foreach (var item in status)
                    {
                        var Vch = _context.TblAdvanceSalaries.Where(e => e.EmpyId == item.EmpyId && e.Srno == item.SrNo && e.CompId == auth.CmpId && e.LocId == auth.LocId).ToList();

                        foreach (var empLoan in Vch)
                        {

                            empLoan.Sent = item.IsApproved;
                        }

                    }

                }
                var srNoList = status.Select(item => item.SrNo.ToString());
                string Srno = string.Join(",", srNoList);

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(approval.SrNo, "Voucher Approval", $"{approval.VchType} Voucher Approved - {Srno} ", 0, dtNow, 0, 0, 0, dtNow);
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
            }
        }


        // Allow Same Leave

        public bool AllowSameLeave(int empy_id)
        {
            DateTime dtNow = DateTime.Now;
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                DateTime currentDateTime = DateTime.Now;
                var employees = _context.TblEmployeeSetups.Where(x => x.CompId == auth.CmpId && x.LocId == auth.LocId).ToList();

                var empLeaves = _context.Tblempleaves.Where(x => x.EmpyId == empy_id && x.CompId == auth.CmpId && x.LocId == auth.LocId).ToList();

                var existingLeaves = _context.Tblempleaves.Where(x => x.CompId == auth.CmpId && x.LocId == auth.LocId);
                _context.Tblempleaves.RemoveRange(existingLeaves);

                _context.SaveChanges();

                foreach (var employee in employees)
                {
                    foreach (var empLeave in empLeaves)
                    {
                        var newEmpLeave = new Tblempleaf
                        {
                            EmpyId = employee.EmpyId,
                            LvId = empLeave.LvId,
                            Trdate = currentDateTime,
                            NoOfLvs = empLeave.NoOfLvs,
                            CompId = empLeave.CompId,
                            LocId = empLeave.LocId
                        };

                        _context.Tblempleaves.Add(newEmpLeave);
                    }
                }



                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(empy_id, "Allow Leave Same", $"Employee Id : {empy_id} - Leaves Allowed to All ", 0, dtNow, 0, 0, 0, dtNow);
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
