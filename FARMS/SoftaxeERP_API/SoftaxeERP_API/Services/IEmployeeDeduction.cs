using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SoftaxeERP_API.Models;
using SoftaxeERP_API.VM;
using System.Data;
using System.Reflection.Emit;

namespace SoftaxeERP_API.Services
{
    public interface IEmployeeDeduction
    {
        // Employee Deduction

        string SaveIncomeTax([FromBody] TblIncomeTax requestData);

        string SaveEOBI([FromBody] TblEobi requestData);


        string GetEditIncomeTax(int empy_id);
        string GetEditEOBITax(int empy_id);

        bool DelIncomeTax(int empy_id, int SrNo);
        bool DelEOBITax(int empy_id, int SrNo);

        // Advance Salary

        DataTable getLevel5Accounts();
        string SaveAdvanceSalary([FromBody] TblAdvanceSalary requestData);

        DataTable GetEditAdvSalaryList(int empy_id);

        bool DelAdvSalary(int empy_id, string Vch, int SrNo);

        // Employee Staff Loan
        string SaveStaffLoan([FromBody] TblStaffLoan requestData);
        string GetEditLoan(int empy_id);
        bool DelStaffLoan(int empy_id, string Vch, int SrNo);

        // Vehcile Loan
        string SaveVehicleLoan([FromBody] TblVehicleLoan requestData);
        string GetEditVehicleLoan(int empy_id);
        bool DelVehicleLoan(int empy_id, string Vch, int SrNo);


        // Insurance Entry

        bool DelInsrnceLoan(int empy_id, string Vch, int SrNo);
        DataTable GetEditInsrnceLoan(int empy_id);
        string SaveInsrnceLoan([FromBody] Tblinsuranceloan requestData);

        // Loan Status

        string UpdateLoanStatus(List<LoanStatusVM> status);
        string GetLoanStatus(string type, bool status);


    }

    public class EmployeDeduction : IEmployeeDeduction
    {
        private readonly ErpSoftaxeContext _context;
        private readonly IDataLogic _dataLogic;
        private readonly IAuth _auth;
        readonly AuthVM auth = new();
        public EmployeDeduction(ErpSoftaxeContext context, IDataLogic dataLogic, IAuth authData)
        {
            _context = context;
            _dataLogic = dataLogic;
            _auth = authData;
            auth = _auth.GetUserData();
        }


        // Employee Deductions Income Tax and EOBI


        public string SaveIncomeTax([FromBody] TblIncomeTax requestData)
        {
            
            using var transaction = _context.Database.BeginTransaction();

                try
                {

                        if (requestData.Srno == 0)
                        {
                             requestData.Srno = (_context.TblIncomeTaxes
                              .Where(x => x.CompId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId && x.EmpyId == requestData.EmpyId)
                                 .Max(x => (int?)x.Srno) ?? 0) + 1;
                        }

                _context.TblIncomeTaxes
                        .Where(x => x.Srno == requestData.Srno && x.CompId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId && x.EmpyId == requestData.EmpyId)
                    .ExecuteDelete();

                    DateTime dtNow = DateTime.Now;


                        _context.TblIncomeTaxes.Add(new TblIncomeTax
                        {
                            Srno = requestData.Srno,
                            EmpyId = requestData.EmpyId,
                            Stdate = requestData.Stdate,
                            Trdate = dtNow,
                            Reference = requestData.Reference,
                            Active = requestData.Active,
                            CompId = auth.CmpId,
                            IcomeTaxdeducation = requestData.IcomeTaxdeducation,
                            Remarks = requestData.Remarks,
                            LocId = auth.LocId,
                            Sent = requestData.Sent,
                            FinId = auth.FinId,
                            Vch = "IT-VCH"

                        });

                    _context.SaveChanges();
                    transaction.Commit();
                    _dataLogic.LogEntry(requestData.EmpyId, "Income Tax", $"Add/Edit Income Tax - Employee Id - {requestData.EmpyId} - VchNo = {requestData.Srno}", 0, dtNow, 0, 0, 0, dtNow);
                    return "true";
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return "false";
                    throw;

                }

        }

        public string SaveEOBI([FromBody] TblEobi requestData)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                if (requestData.Srno == 0)
                {
                    requestData.Srno = (_context.TblEobis
                     .Where(x => x.CompId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId && x.EmpyId == requestData.EmpyId)
                        .Max(x => (int?)x.Srno) ?? 0) + 1;
                }

                _context.TblEobis
                        .Where(x => x.Srno == requestData.Srno && x.CompId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId && x.EmpyId == requestData.EmpyId)
                    .ExecuteDelete();


                DateTime dtNow = DateTime.Now;


                _context.TblEobis.Add(new TblEobi
                {
                    Srno = requestData.Srno,
                    EmpyId = requestData.EmpyId,
                    Stdate = requestData.Stdate,
                    Trdate = dtNow,
                    Reference = requestData.Reference,
                    CompId = auth.CmpId,
                    EobiDeducation = requestData.EobiDeducation,
                    Remarks = requestData.Remarks,
                    LocId = auth.LocId,
                    Active = requestData.Active,
                    Sent = requestData.Sent,
                    FinId = auth.FinId,
                    Vch = "EI-VCH"

                });

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(requestData.EmpyId, "EOBI", $"Add/Edit EOBI - Employee Id- {requestData.EmpyId} - VchNo = {requestData.Srno}", 0, dtNow, 0, 0, 0, dtNow);
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
                throw;

            }
        
    }


        public string GetEditIncomeTax(int empy_id)
        {
                String qry = $@"Select d.Id, emp.name as EmpName, d.empy_id, d.Vch, 
                            d.srno, d.stDate, d.Reference, d.icomeTaxdeducation, d.Active, d.Remarks, isnull(d.sent,0) as sent
                            from tblIncomeTax d
                            Join tblEmployeeSetup emp On emp.empy_id = d.empy_id and emp.Comp_id = d.Comp_id and emp.LocId = d.LocId
                            where d.comp_id = '" + auth.CmpId + "' and d.empy_id = '" + empy_id + "' And d.LocId = '" + auth.LocId + "' and Finid = '"+auth.FinId+"'";

                var result = _dataLogic.LoadData(qry);
                return JsonConvert.SerializeObject(result);

        }

        public string GetEditEOBITax(int empy_id)
        {
            String qry = $@"Select d.Id, emp.name as EmpName, d.empy_id, d.Vch, 
            d.srno, d.stDate, d.Reference, d.EobiDeducation, d.Active, d.Remarks, isnull(d.sent,0) as sent
            from tblEobi d
            Join tblEmployeeSetup emp On emp.empy_id = d.empy_id and emp.Comp_id = d.Comp_id and emp.LocId = d.LocId
            where d.comp_id = '" + auth.CmpId + "' and d.empy_id = '" + empy_id + "' And d.LocId = '" + auth.LocId + "'  and Finid = '"+auth.FinId+"'";

            var result = _dataLogic.LoadData(qry);
            return JsonConvert.SerializeObject(result);

        }


        public bool DelIncomeTax(int empy_id, int SrNo)
        {
            DateTime dtNow = DateTime.Now;

                using var transaction = _context.Database.BeginTransaction();
                try
                {
               
                  var tax =  _context.TblIncomeTaxes.Where(x => x.EmpyId == empy_id && x.CompId == auth.CmpId && x.Srno == SrNo && x.FinId == auth.FinId && (x.Sent == false || x.Sent == null) && x.LocId == auth.LocId).ToList();
                    if (tax != null)
                    {
                        _context.TblIncomeTaxes.RemoveRange(tax);
                    }
                    _context.SaveChanges();
                    transaction.Commit();
                    _dataLogic.LogEntry(empy_id, "Income Tax", $"Deleted Income Tax - Employee Id: {empy_id} - VchNo = {SrNo}", 0, dtNow, 0, 0, 0, dtNow);
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                    throw;
                }

        }
        public bool DelEOBITax(int empy_id, int SrNo)
        {
            DateTime dtNow = DateTime.Now;
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var eobi = _context.TblEobis.Where(x => x.EmpyId == empy_id && x.CompId == auth.CmpId && x.Srno == SrNo && x.FinId == auth.FinId && (x.Sent == false || x.Sent == null) && x.LocId == auth.LocId).FirstOrDefault();
                if (eobi != null)
                {
                    _context.TblEobis.RemoveRange(eobi);
                }
                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(empy_id, "EOBI", $"Deleted EOBI - Employee Id: {empy_id} - VchNo = {SrNo}", 0, dtNow, 0, 0, 0, dtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }
        }



        // Employee Loan - Staff Loan and Vehicle Loan

        public string SaveStaffLoan([FromBody] TblStaffLoan requestData)
        {
            DateTime dtNow = DateTime.Now;
            using var transaction = _context.Database.BeginTransaction();

            try
            {


                if (requestData.Srno == 0)
                {
                    requestData.Srno = (_context.TblStaffLoans
                        .Where(x => x.CompId == auth.CmpId && x.LocId == auth.LocId && x.Finid == auth.FinId && x.Vch == requestData.Vch)
                        .Max(x => (int?)x.Srno) ?? 0) + 1;
                }



                _context.TblStaffLoans
                    .Where(x => x.CompId == auth.CmpId && x.Srno == requestData.Srno && x.LocId == auth.LocId && x.Finid == auth.FinId && x.Vch == requestData.Vch)
                .ExecuteDelete();



                _context.TblStaffLoans.Add(new TblStaffLoan
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
                    Finid = auth.FinId,
                    FinEntry = requestData.FinEntry,
                    AccountCode = requestData.AccountCode
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


                    var defaultCode = _context.Level4s.FirstOrDefault(x => x.CompId == auth.CmpId && x.LocId == auth.LocId && x.Tag == "HR-SL")
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

                _dataLogic.LogEntry(requestData.EmpyId, "Staff Loan", $"Add/Edit Staff Loan - Employee Id - {requestData.EmpyId} - VchNo = {requestData.Srno}", 0, dtNow, 0, 0, 0, dtNow);
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
                throw;

            }



        }

        public string GetEditLoan(int empy_id)
        {

                String qry = $@"Select sl.empy_id, sl.Active, sl.instamt, sl.loanamt, sl.noofmnth, sl.remarks, isnull(sl.sent,0) AS sent,
                            sl.srno, sl.stdate, sl.FinEntry, sl.accountCode, sl.Id, sl.Vch, emp.name from tblStaffLoan sl
                            Inner Join tblEmployeeSetup emp ON emp.empy_id = sl.empy_id and emp.Comp_id = sl.Comp_id and emp.LocId = sl.LocId
                            where sl.comp_id = '" + auth.CmpId + "'  AND sl.empy_id = '" + empy_id + "' And sl.LocId = '" + auth.LocId + "' and sl.finid = '"+auth.FinId+"'";

                var result = _dataLogic.LoadData(qry);
                return JsonConvert.SerializeObject(result);

        }

        public bool DelStaffLoan(int empy_id, string Vch, int SrNo)
        {
            DateTime dtNow = DateTime.Now;



                using var transaction = _context.Database.BeginTransaction();
                try
                {
                    _context.TblStaffLoans.Where(x => x.EmpyId == empy_id && x.Finid == auth.FinId && x.CompId == auth.CmpId && x.Vch == Vch && x.Srno == SrNo && (x.Sent == false || x.Sent == null) && x.LocId == auth.LocId).ExecuteDelete();
                    _context.TransMains
                        .Where(x => x.CmpId == auth.CmpId && x.VchNo == SrNo && x.LocId == auth.LocId && x.FinId == auth.FinId && x.VchType == Vch)
                    .ExecuteDelete();



                     _context.TblTransVches
                    .Where(x => x.CmpId == auth.CmpId && x.VchNo == SrNo && x.LocId == auth.LocId && x.FinId == auth.FinId && x.VchType == Vch)
                        .ExecuteDelete();


                _context.SaveChanges();
                    transaction.Commit();
                    _dataLogic.LogEntry(empy_id, "Staff Loan", $"Deleted Staff Loan - Employee Id: {empy_id}", 0, dtNow, 0, 0, 0, dtNow);
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                    throw;
                }


        }

        // Vehicle Loan
        public string SaveVehicleLoan([FromBody] TblVehicleLoan requestData)
        {
            DateTime dtNow = DateTime.Now;
            using var transaction = _context.Database.BeginTransaction();

            try
            {


                if (requestData.Srno == 0)
                {
                    requestData.Srno = (_context.TblVehicleLoans
                        .Where(x => x.CompId == auth.CmpId && x.LocId == auth.LocId && x.Finid == auth.FinId && x.Vch == requestData.Vch)
                        .Max(x => (int?)x.Srno) ?? 0) + 1;
                }



                _context.TblVehicleLoans
                    .Where(x => x.CompId == auth.CmpId && x.Srno == requestData.Srno && x.LocId == auth.LocId && x.Finid == auth.FinId && x.Vch == requestData.Vch)
                .ExecuteDelete();



                _context.TblVehicleLoans.Add(new TblVehicleLoan
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
                    Finid = auth.FinId,
                    FinEntry = requestData.FinEntry,
                    AccountCode = requestData.AccountCode,
                    Engineno = requestData.Engineno,
                    Chasisno = requestData.Chasisno,
                    Vehicleno = requestData.Vehicleno
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

                _dataLogic.LogEntry(requestData.EmpyId, "Vehicle Loan", $"Add/Edit Vehicle Loan - Employee Id - {requestData.EmpyId} - VchNo = {requestData.Srno}", 0, dtNow, 0, 0, 0, dtNow);
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
                throw;

            }



        }

        public string GetEditVehicleLoan(int empy_id)
        {

            String qry = $@"Select sl.empy_id, sl.Active, sl.instamt, sl.loanamt, sl.noofmnth, sl.remarks, isnull(sl.sent,0) AS sent,
                            sl.srno, sl.stdate, sl.EngineNo, sl.VehicleNo, sl.ChasisNo, sl.FinEntry, sl.accountCode, sl.Id, sl.Vch, emp.name from tblVehicleLoan sl
                            Inner Join tblEmployeeSetup emp ON emp.empy_id = sl.empy_id and emp.Comp_id = sl.Comp_id and emp.LocId = sl.LocId
                            where sl.comp_id = '" + auth.CmpId + "'  AND sl.empy_id = '" + empy_id + "' And sl.LocId = '" + auth.LocId + "' and sl.finid = '" + auth.FinId + "'";

            var result = _dataLogic.LoadData(qry);
            return JsonConvert.SerializeObject(result);

        }

        public bool DelVehicleLoan(int empy_id, string Vch, int SrNo)
        {
            DateTime dtNow = DateTime.Now;



            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.TblVehicleLoans.Where(x => x.EmpyId == empy_id && x.CompId == auth.CmpId && x.Finid == auth.FinId && x.Vch == Vch && x.Srno == SrNo && (x.Sent == false || x.Sent == null) && x.LocId == auth.LocId).ExecuteDelete();
                _context.TransMains
                    .Where(x => x.CmpId == auth.CmpId && x.VchNo == SrNo && x.LocId == auth.LocId && x.FinId == auth.FinId && x.VchType == Vch)
                .ExecuteDelete();



                _context.TblTransVches
               .Where(x => x.CmpId == auth.CmpId && x.VchNo == SrNo && x.LocId == auth.LocId && x.FinId == auth.FinId && x.VchType == Vch)
                   .ExecuteDelete();


                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(empy_id, "Vehicle Loan", $"Deleted Vehicle Loan - Employee Id: {empy_id}", 0, dtNow, 0, 0, 0, dtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }


        }

        // Advance Salary 

        public DataTable getLevel5Accounts()
        {
            String qry = $@"Select Level4+Level5 as code, Names  from Level5 where comp_id = '" +auth.CmpId+"' and LocID = '"+auth.LocId+"'";

            return _dataLogic.LoadData(qry);
        }
        public string SaveAdvanceSalary([FromBody] TblAdvanceSalary requestData)
        {
           DateTime dtNow = DateTime.Now;
            using var transaction = _context.Database.BeginTransaction();

            try
            {


                if (requestData.Srno == 0)
                {
                    requestData.Srno = (_context.TblAdvanceSalaries
                        .Where(x => x.CompId == auth.CmpId && x.LocId == auth.LocId && x.Finid == auth.FinId && x.Vch == requestData.Vch)
                        .Max(x => (int?)x.Srno) ?? 0) + 1;
                }



                _context.TblAdvanceSalaries
                    .Where(x =>  x.CompId == auth.CmpId && x.Srno == requestData.Srno && x.LocId == auth.LocId && x.Finid == auth.FinId && x.Vch == requestData.Vch)
                .ExecuteDelete();



                    _context.TblAdvanceSalaries.Add(new TblAdvanceSalary
                    {   
                        CompId = auth.CmpId,
                        Srno = requestData.Srno,
                        EmpyId = requestData.EmpyId,
                        Stdate = requestData.Stdate,
                        Remarks = requestData.Remarks,
                        AdvanceSalary = requestData.AdvanceSalary,
                        Trdate = dtNow,
                        Reference = requestData.Reference,
                        Vch = requestData.Vch,
                        LocId = auth.LocId,
                        Finid = auth.FinId,
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


                    var defaultCode = _context.Level4s.FirstOrDefault(x => x.CompId == auth.CmpId && x.LocId == auth.LocId && x.Tag == "HR-AD")
                                      ?? _context.Level4s.FirstOrDefault(x => x.CompId == auth.CmpId && x.LocId == auth.LocId && x.Tag == "HRSALARY");


                    _context.TblTransVches.Add(new TblTransVch
                    {
                        VchType = requestData.Vch,
                        VchNo = requestData.Srno,
                        CmpId = auth.CmpId,
                        LocId = auth.LocId,
                        FinId = auth.FinId,
                        VchDate = dtNow,
                        Dmcode = defaultCode.Level3+defaultCode.Level41,
                        Code = $"{requestData.EmpyId:D5}",
                        Mcode = requestData.AccountCode,
                        Debit = requestData.AdvanceSalary,
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
                        Credit = requestData.AdvanceSalary,
                        Descrp = requestData.Remarks,
                        Tucks = 9,


                    });

                }




                _context.SaveChanges();
                transaction.Commit();
               
                _dataLogic.LogEntry(requestData.EmpyId, "Advance Salary", $"Add/Edit Advance Salary - Employee Id - {requestData.EmpyId} - VchNo = {requestData.Srno}", 0, dtNow, 0, 0, 0, dtNow);
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
                throw;

            }



        }

        public DataTable GetEditAdvSalaryList(int empy_id)
        {
            String qry = $@"Select d.Id, emp.name as EmpName, d.empy_id, d.Vch, 
                 d.srno, d.stDate, d.Reference, d.accountCode, d.FinEntry, d.AdvanceSalary, d.Remarks, isnull(d.sent,0) as sent
                 from tbladvanceSalary d
                 Join tblEmployeeSetup emp On emp.empy_id = d.empy_id and emp.Comp_id = d.Comp_id and emp.LocId = d.LocId
                 where d.comp_id = '" + auth.CmpId + "' and d.empy_id = '" + empy_id + "' And d.LocId = '" + auth.LocId + "' and d.finid = '"+auth.FinId+"'";

            return _dataLogic.LoadData(qry);
        }

        public bool DelAdvSalary(int empy_id, string Vch, int SrNo)
        {
            DateTime dtNow = DateTime.Now;
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.TblAdvanceSalaries.Where(x => x.EmpyId == empy_id && x.CompId == auth.CmpId && x.LocId == auth.LocId && x.Finid == auth.FinId && x.Vch == Vch && x.Srno == SrNo && (x.Sent == false || x.Sent == null)).ExecuteDelete();
                _context.TransMains
                        .Where(x => x.CmpId == auth.CmpId && x.VchNo == SrNo && x.LocId == auth.LocId && x.FinId == auth.FinId && x.VchType == Vch)
                    .ExecuteDelete();



                _context.TblTransVches
                    .Where(x => x.CmpId == auth.CmpId && x.VchNo == SrNo && x.LocId == auth.LocId && x.FinId == auth.FinId && x.VchType == Vch)
                .ExecuteDelete();
                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(empy_id, "Advance Salary", $"Deleted Advance Salary - Employee Id: {empy_id}", 0, dtNow, 0, 0, 0, dtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }
        }


        // Loan Status
        public string UpdateLoanStatus(List<LoanStatusVM> status)
        {
            DateTime dtNow = DateTime.Now;
            LoanStatusVM laon = status.First();

            if (laon.Type == "Staff Loan")
            {
                using var transaction = _context.Database.BeginTransaction();

                try
                {
                    foreach (var item in status)
                    {
                        var empLoans = _context.TblStaffLoans.Where(e => e.EmpyId == item.EmpyId && e.CompId == auth.CmpId && e.LocId == auth.LocId).ToList();

                        foreach (var empLoan in empLoans)
                        {

                            empLoan.Active = item.Active;
                        }

                    }

                    _context.SaveChanges();
                    transaction.Commit();
                    _dataLogic.LogEntry(laon.EmpyId, "Laon Status", $"Update Loan Status - {laon.Type} - Employee Id - {laon.EmpyId}", 0, dtNow, 0, 0, 0, dtNow);
                    return "true";
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return "false";
                }
            }

            if (laon.Type == "Vehicle Loan")
            {
                using var transaction = _context.Database.BeginTransaction();

                try
                {
                    foreach (var item in status)
                    {
                        var empLoans = _context.TblVehicleLoans.Where(e => e.EmpyId == item.EmpyId && e.CompId == auth.CmpId && e.LocId == auth.LocId).ToList();

                        foreach (var empLoan in empLoans)
                        {

                            empLoan.Active = item.Active;
                        }

                    }

                    _context.SaveChanges();
                    transaction.Commit();
                    _dataLogic.LogEntry(laon.EmpyId, "Laon Status", $"Update Loan Status - {laon.Type} - Employee Id - {laon.EmpyId}", 0, dtNow, 0, 0, 0, dtNow);
                    return "true";
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return "false";
                }
            }

            return "Undefined Type";

        }

        public string GetLoanStatus(string type, bool status)
        {
            if (type == "Staff Loan")
            {
                String qry = $@"Select Distinct  l.empy_id,'Staff Loan' Type, e.name as EmployeeName,l.Active from tblStaffLoan l
                            Inner Join tblEmployeeSetup e On e.empy_id = l.empy_id and e.Comp_id = l.Comp_id and e.LocId = l.LocId
                            where l.Comp_id = '"+auth.CmpId+"' and l.LocId = '"+auth.LocId+"' and l.Active = '" + status + "'";


                var result = _dataLogic.LoadData(qry);
                return JsonConvert.SerializeObject(result);
            }

            if (type == "Vehicle Loan")
            {
                String qry = $@"Select Distinct  l.empy_id, 'Vehicle Loan' Type, e.name as EmployeeName,l.Active from tblVehicleLoan l
                            Inner Join tblEmployeeSetup e On e.empy_id = l.empy_id and e.Comp_id = l.Comp_id and e.LocId = l.LocId
                            where l.Comp_id = '"+auth.CmpId+"' and l.LocId = '"+auth.LocId+"' and l.Active = '" + status + "'";


                var result = _dataLogic.LoadData(qry);
                return JsonConvert.SerializeObject(result);
            }

            return "Undefined Type";
        }


        // Insurance Loan

        public string SaveInsrnceLoan([FromBody] Tblinsuranceloan requestData)
        {
            DateTime dtNow = DateTime.Now;
            using var transaction = _context.Database.BeginTransaction();

            try
            {


                if (requestData.Srno == 0)
                {
                    requestData.Srno = (_context.Tblinsuranceloans
                        .Where(x => x.CompId == auth.CmpId && x.LocId == auth.LocId && x.Finid == auth.FinId && x.Vch == requestData.Vch)
                        .Max(x => (int?)x.Srno) ?? 0) + 1;
                }



                _context.Tblinsuranceloans
                    .Where(x => x.CompId == auth.CmpId && x.Srno == requestData.Srno && x.LocId == auth.LocId && x.Finid == auth.FinId && x.Vch == requestData.Vch)
                .ExecuteDelete();



                _context.Tblinsuranceloans.Add(new Tblinsuranceloan
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
                    Finid = auth.FinId,
                    FinEntry = requestData.FinEntry,
                    AccountCode = requestData.AccountCode,
                    Engineno = requestData.Engineno,
                    Chasisno = requestData.Chasisno,
                    Vehicleno = requestData.Vehicleno
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


                    var defaultCode = _context.Level4s.FirstOrDefault(x => x.CompId == auth.CmpId && x.LocId == auth.LocId && x.Tag == "HR-IL")
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

                _dataLogic.LogEntry(requestData.EmpyId, "Vehicle Loan", $"Add/Edit Vehicle Loan - Employee Id - {requestData.EmpyId} - VchNo = {requestData.Srno}", 0, dtNow, 0, 0, 0, dtNow);
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
                throw;

            }



        }


        public DataTable GetEditInsrnceLoan(int empy_id)
        {
            String qry = $@"Select sl.empy_id, sl.Active, sl.instamt, sl.loanamt, sl.noofmnth, sl.remarks, isnull(sl.sent,0) AS sent,
                        sl.srno, sl.Vch, sl.stdate, sl.Id, sl.FinEntry, sl.accountCode, sl.Vehicleno,sl.opening,sl.Engineno, sl.Chasisno, sl.Vch, emp.name from tblinsuranceloan sl
                    Inner Join tblEmployeeSetup emp ON emp.empy_id = sl.empy_id  and emp.Comp_id = sl.Comp_id and emp.LocId = sl.LocId
                    where sl.comp_id = '" + auth.CmpId + "'  AND sl.empy_id = '" + empy_id + "' And sl.LocId = '" + auth.LocId + "' and sl.finid = '"+auth.FinId+"'";

            return _dataLogic.LoadData(qry);
        }

        public bool DelInsrnceLoan(int empy_id, string Vch, int SrNo)
        {
            DateTime dtNow = DateTime.Now;
            using var transaction = _context.Database.BeginTransaction();
            try
            {
               _context.Tblinsuranceloans.Where(x => x.EmpyId == empy_id && x.CompId == auth.CmpId && x.Finid == auth.FinId && x.Vch == Vch && x.Srno == SrNo && (x.Sent == false || x.Sent == null) && x.LocId == auth.LocId).ExecuteDelete();
                _context.TransMains
                       .Where(x => x.CmpId == auth.CmpId && x.VchNo == SrNo && x.LocId == auth.LocId && x.FinId == auth.FinId && x.VchType == Vch)
                   .ExecuteDelete();



                _context.TblTransVches
                    .Where(x => x.CmpId == auth.CmpId && x.VchNo == SrNo && x.LocId == auth.LocId && x.FinId == auth.FinId && x.VchType == Vch)
                .ExecuteDelete();

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(empy_id, "Insurance Loan", $"Deleted Insurance Loan - Employee Id: {empy_id}", 0, dtNow, 0, 0, 0, dtNow);
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
