using DevExpress.Office;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftaxeERP_API.Models;
using SoftaxeERP_API.VM;
using System.Data;
using System.Numerics;

namespace SoftaxeERP_API.Services
{
    public interface IProvidentFund
    {

        // Bank Entry

        DataTable GetBankList();
        bool SaveBank([FromBody] Tblbank requestData);
        bool DelBank(int Id);

        // Provident Loan

        string SaveProvidentLoan([FromBody] Tblploan requestData);

        DataTable GetEditPLoan(int empy_id);

        bool DelPLoan(int empy_id, string Vch, int SrNo);

        // Provident Fund Module

        string SavePfDeduction([FromBody] TblParovidentFund requestData);

        DataTable GetEditPfDeductionList(int empy_id);

        bool DelPfDeductions(int empy_id, int SrNo);
    }

    public class ProvidentFund : IProvidentFund
    {
        private readonly ErpSoftaxeContext _context;
        private readonly IDataLogic _dataLogic;
        private readonly IAuth _auth;
        readonly AuthVM auth = new();
        public ProvidentFund(ErpSoftaxeContext context, IDataLogic dataLogic, IAuth authData)
        {
            _context = context;
            _dataLogic = dataLogic;
            _auth = authData;
            auth = _auth.GetUserData();
        }

        #region Bank Entry 

        // BanK Entry

        public DataTable GetBankList()
        {
            String qry = $@"Select  Id as bankId, Bank as bankName, BranchCode as branchCode, AccNo as accNo, Address as address from TblBank  where comp_id = '" + auth.CmpId + "' and locId = '" + auth.LocId + "'";
            return _dataLogic.LoadData(qry);
        }

        public bool SaveBank([FromBody] Tblbank requestData)
        {

            using var transaction = _context.Database.BeginTransaction();

            try
            {
                DateTime dtNow = DateTime.Now;
                var existingBank = _context.Tblbanks
                .FirstOrDefault(x => x.Bank.Trim().Replace(" ", "") == requestData.Bank.Trim().Replace(" ", "")
                 && x.CompId == auth.CmpId && x.LocId == auth.LocId);

                if (existingBank != null)
                {
                    return false;
                }

                bool status = false;

                if (requestData.Id == 0)
                {
                    requestData.Id = (_context.Tblbanks.Where(x => x.CompId == auth.CmpId && x.LocId == auth.LocId)
                        .Max(x => (int?)x.Id) ?? 0) + 1;
                    status = true;
                }

                _context.Tblbanks
                    .Where(x => x.Id == requestData.Id && x.CompId == auth.CmpId && x.LocId == auth.LocId)
                    .ExecuteDelete();


                _context.Tblbanks.Add(new Tblbank
                {
                    Id = requestData.Id,
                    Bank = requestData.Bank,
                    BranchCode = requestData.BranchCode,
                    Address = requestData.Address,
                    AccNo = requestData.AccNo,
                    CompId = auth.CmpId,
                    LocId = auth.LocId
                });


                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(requestData.Id, "Bank", $"{((status == true) ? "Add" : "Edit")} Bank - {requestData.Bank} ", 0, dtNow, 0, 0, 0, dtNow);

                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }
        }

        public bool DelBank(int Id)
        {
            DateTime dtNow = DateTime.Now;
            using var transaction = _context.Database.BeginTransaction();
            try
            {
              var bank =  _context.Tblbanks.Where(x => x.Id == Id && x.CompId == auth.CmpId && x.LocId == auth.LocId).FirstOrDefault();
                if ( bank == null)
                {
                    return false;
                }

                _context.Tblbanks.Remove(bank);
                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(Id, "Department", $"Deleted Designation - {bank.Bank}", 0, dtNow, 0, 0, 0, dtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }

        }

        #endregion


        #region Provident Loan

        // Provident Loan



        public string SaveProvidentLoan([FromBody] Tblploan requestData)
        {
            DateTime dtNow = DateTime.Now;
            using var transaction = _context.Database.BeginTransaction();

            try
            {


                if (requestData.Srno == 0)
                {
                    requestData.Srno = (_context.Tblploans
                        .Where(x => x.CompId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId && x.Vch == requestData.Vch)
                        .Max(x => (int?)x.Srno) ?? 0) + 1;
                }



                _context.Tblploans
                    .Where(x => x.CompId == auth.CmpId && x.Srno == requestData.Srno && x.LocId == auth.LocId && x.FinId == auth.FinId && x.Vch == requestData.Vch)
                .ExecuteDelete();



                _context.Tblploans.Add(new Tblploan
                {
                    Srno = requestData.Srno,
                    Vch = requestData.Vch,
                    EmpyId = requestData.EmpyId,
                    Stdate = requestData.Stdate,
                    Loanamt = requestData.Loanamt,
                    Instamt = requestData.Instamt,
                    Remarks = requestData.Remarks,
                    Active = requestData.Active,
                    CompId = auth.CmpId,
                    Noofmnth = requestData.Noofmnth,
                    LocId = auth.LocId,
                    Sent = requestData.Sent,
                    FinId = auth.FinId,
                    FinEntry = requestData.FinEntry,
                    AccountCode = requestData.AccountCode,

                });
                if (requestData.FinEntry == true)
                {


                    _context.TransMains
                        .Where(x => x.CmpId == auth.CmpId && x.VchNo == requestData.Srno && x.LocId == auth.LocId && x.FinId == auth.FinId && x.VchType == requestData.Vch)
                    .ExecuteDelete();



                    _context.TblTransVches
                        .Where(x => x.CmpId == auth.CmpId && x.VchNo == requestData.Srno && x.LocId == auth.LocId && x.FinId == auth.FinId && x.VchType == requestData.Vch)
                    .ExecuteDelete();



                    _context.TransMains.Add(new TransMain
                    {
                        VchType = requestData.Vch,
                        VchNo = requestData.Srno,
                        CmpId = auth.CmpId,
                        LocId = auth.LocId,
                        FinId = auth.FinId,
                        VchDateM = dtNow,

                    });


                    var defaultCode = _context.Level4s.FirstOrDefault(x => x.CompId == auth.CmpId && x.LocId == auth.LocId && x.Tag == "HR-VL")
                                      ?? _context.Level4s.FirstOrDefault(x => x.CompId == auth.CmpId && x.LocId == auth.LocId && x.Tag == "HRSALARY");


                    _context.TblTransVches.Add(new TblTransVch
                    {
                        VchType = requestData.Vch,
                        VchNo = requestData.Srno,
                        CmpId = auth.CmpId,
                        LocId = auth.LocId,
                        FinId = auth.FinId,
                        VchDate = dtNow,
                        Dmcode = defaultCode.Level3 + defaultCode.Level41,
                        Code = $"{requestData.EmpyId:D5}",
                        Mcode = requestData.AccountCode,
                        Debit = requestData.Loanamt,
                        Credit = 0,
                        Descrp = requestData.Remarks,
                        Tucks = 8,


                    });
                    _context.TblTransVches.Add(new TblTransVch
                    {
                        VchType = requestData.Vch,
                        VchNo = requestData.Srno,
                        CmpId = auth.CmpId,
                        LocId = auth.LocId,
                        FinId = auth.FinId,
                        VchDate = dtNow,
                        Dmcode = requestData.AccountCode.Substring(0, 9),
                        Code = requestData.AccountCode.Substring(9, 5),
                        Debit = 0,
                        Credit = requestData.Loanamt,
                        Descrp = requestData.Remarks,
                        Tucks = 9,


                    });

                }




                _context.SaveChanges();
                transaction.Commit();

                _dataLogic.LogEntry(requestData.EmpyId, "Provident Loan", $"Add/Edit Providen Loan - Employee Id - {requestData.EmpyId} - VchNo = {requestData.Srno}", 0, dtNow, 0, 0, 0, dtNow);
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
                throw;

            }



        }


        public DataTable GetEditPLoan(int empy_id)
        {
            String qry = $@"Select sl.empy_id, sl.Active, sl.instamt, sl.loanamt, sl.noofmnth, sl.remarks, isnull(sl.sent,0) AS sent,
                        sl.srno, sl.stdate,  sl.Vch, sl.accountCode, sl.FinEntry, sl.Id, emp.name from tblploan sl
                        Inner Join tblEmployeeSetup emp ON emp.empy_id = sl.empy_id and sl.Comp_id = emp.Comp_id and sl.LocId = emp.LocId
                        where sl.comp_id = '" + auth.CmpId + "'  AND sl.empy_id = '" + empy_id + "' and sl.LocId = '"+auth.LocId+ "'  AND sl.FINID = '"+auth.FinId+"'";

            return _dataLogic.LoadData(qry);
        }

        public bool DelPLoan(int empy_id, string Vch, int SrNo)
        {
            DateTime dtNow = DateTime.Now;
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.Tblploans.Where(x => x.EmpyId == empy_id && x.FinId == auth.FinId && x.Vch == Vch && x.Srno == SrNo && x.CompId == auth.CmpId && (x.Sent == false || x.Sent == null) && x.LocId == auth.LocId).ExecuteDelete();
                _context.TransMains
                   .Where(x => x.CmpId == auth.CmpId && x.VchNo == SrNo && x.LocId == auth.LocId && x.FinId == auth.FinId && x.VchType == Vch)
               .ExecuteDelete();



                _context.TblTransVches
               .Where(x => x.CmpId == auth.CmpId && x.VchNo == SrNo && x.LocId == auth.LocId && x.FinId == auth.FinId && x.VchType == Vch)
                   .ExecuteDelete();
                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(empy_id, "Provident Loan", $"Deleted Provident Loan - Employee Id: {empy_id}", 0, dtNow, 0, 0, 0, dtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }
        }


        #endregion

        #region Provident Fund 

        // Provident Fund Module

        public string SavePfDeduction([FromBody] TblParovidentFund requestData)
        {

            DateTime dtNow = DateTime.Now;

            using var transaction = _context.Database.BeginTransaction();

            try
            {
                if (requestData.Srno == 0)
                {
                    requestData.Srno = (_context.TblParovidentFunds
                     .Where(x => x.CompId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId && x.EmpyId == requestData.EmpyId)
                        .Max(x => (int?)x.Srno) ?? 0) + 1;
                }

                _context.TblParovidentFunds
                        .Where(x => x.Srno == requestData.Srno && x.CompId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId && x.EmpyId == requestData.EmpyId)
                    .ExecuteDelete();


                    _context.TblParovidentFunds.Add(new TblParovidentFund
                    {
                        Srno = requestData.Srno,
                        EmpyId = requestData.EmpyId,
                        Stdate = requestData.Stdate,
                        Trdate = dtNow,
                        Reference = requestData.Reference,
                        Active = requestData.Active,
                        CompId = auth.CmpId,
                        PfundDeducation = requestData.PfundDeducation,
                        Remarks = requestData.Remarks,
                        LocId = auth.LocId,
                        Sent = requestData.Sent,
                        FinId = auth.FinId

                    });

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(requestData.EmpyId, "Provident Fund", $"Add/Edit Provident Fund - Employee Id - {requestData.EmpyId} - Vch: {requestData.Srno}", 0, dtNow, 0, 0, 0, dtNow);
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
                throw;

            }
        }


        public DataTable GetEditPfDeductionList(int empy_id)
        {
            String qry = $@"Select d.Id, emp.name as EmpName, d.empy_id, d.Vch, 
                    d.srno, d.stDate, d.Reference, d.PFundDeducation, d.Active, d.Remarks, isnull(d.sent,0) as sent
                    from tblParovidentFund d
                    Join tblEmployeeSetup emp On emp.empy_id = d.empy_id and d.Comp_id = emp.Comp_id and d.LocId = emp.LocId
                    where d.comp_id = '" + auth.CmpId + "' and d.empy_id = '" + empy_id + "'  And d.LocId = '" + auth.LocId + "' and d.Finid = '"+auth.FinId+"'";

            return _dataLogic.LoadData(qry);
        }


        public bool DelPfDeductions(int empy_id, int SrNo)
        {
            DateTime dtNow = DateTime.Now;
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.TblParovidentFunds.Where(x => x.EmpyId == empy_id && x.CompId == auth.CmpId && x.FinId == auth.FinId && x.Srno == SrNo && (x.Sent == false || x.Sent == null) && x.LocId == auth.LocId).ExecuteDelete();



                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(empy_id, "Provident Fund", $"Deleted Provident Fund - Employee Id: {empy_id} - Vch : {SrNo}", 0, dtNow, 0, 0, 0, dtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }
        }

        #endregion

    }
}
