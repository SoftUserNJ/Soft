using DevExpress.ClipboardSource.SpreadsheetML;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoftaxeERP_API.Models;
using SoftaxeERP_API.Services;
using SoftaxeERP_API.VM;

namespace SoftaxeERP_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PayrollController : ControllerBase
    {
        private readonly IFileMaintain _fileMaintain;
        private readonly IEmployee _employee;
        private readonly IProvidentFund _provident;
        private readonly IEmployeeDeduction _deduction;
        private readonly IEmployeeIncentives _incentives;
        private readonly IEmployeeLeaves _leaves;
        private readonly ISalaryCalculation _salaryCalculation;
        private readonly IAudit _audit;

        public PayrollController(IFileMaintain fileMaintain, IEmployee employee, IProvidentFund provident, IEmployeeDeduction deduction, IEmployeeIncentives incentives, IEmployeeLeaves leaves, ISalaryCalculation salaryCalculation, IAudit audit)
        {
            _fileMaintain = fileMaintain;
            _employee = employee;
            _provident = provident;
            _deduction = deduction;
            _incentives = incentives;
            _leaves = leaves;
            _salaryCalculation = salaryCalculation;
            _audit = audit;

        }

        #region File Maintain

        // Department Entry
        [HttpPost("SaveDepartment")]
        public IActionResult SaveDepartment([FromBody] Tblcompanydepartment requestData)
        {
            var data = _fileMaintain.SaveDepartment(requestData);
            return Ok(data);
        }

        [HttpGet("GetDepartmentList")]
        public IActionResult GetDepartmentList()
        {
            var data = _fileMaintain.GetDepartmentList();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);

        }

        [HttpDelete("DelDepartment")]
        public IActionResult DelDepartment(int Id)
        {
            var data = _fileMaintain.DelDepartment(Id);
            return Ok(data);
        }

        // Designation Enry
        [HttpPost("SaveDesignation")]
        public IActionResult SaveDesignation([FromBody] Tblcompanydesignation requestData)
        {
            var data = _fileMaintain.SaveDesignation(requestData);
            return Ok(data);
        }

        [HttpGet("GetDesignationList")]
        public IActionResult GetDesignationList()
        {
            var data = _fileMaintain.GetDesignationList();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);

        }

        [HttpDelete("DelDesignation")]
        public IActionResult DelDesignation(int Id)
        {
            var data = _fileMaintain.DelDesignation(Id);
            return Ok(data);
        }

        // holiday Setup

        [HttpPost("SaveHoliday")]
        public IActionResult SaveHoliday([FromBody] Tblholidaysetup requestData)
        {
            var data = _fileMaintain.SaveHoliday(requestData);
            return Ok(data);
        }

        [HttpGet("GetHolidayList")]
        public IActionResult GetHolidayList()
        {
            var data = _fileMaintain.GetHolidayList();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);

        }

        [HttpDelete("DelHoliday")]
        public IActionResult DelHoliday(int Id)
        {
            var data = _fileMaintain.DelHoliday(Id);
            return Ok(data);
        }


        // Leaves Type Entry

        [HttpPost("SaveHrSetup")]
        public IActionResult SaveHrSetup([FromBody] Tblhrsetup requestData)
        {
            var data = _fileMaintain.SaveHrSetup(requestData);
            return Ok(data);
        }


        [HttpGet("GetHrSetupList")]
        public IActionResult GetHrSetupList()
        {
            var data = _fileMaintain.GetHrSetupList();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);

        }

        [HttpDelete("DelHRSetup")]
        public IActionResult DelHRSetup(int vchNo)
        {
            var data = _fileMaintain.DelHRSetup(vchNo);
            return Ok(data);
        }

        // Employee Type Setting

        [HttpPost("SaveEmployeeType")]
        public IActionResult SaveEmployeeType([FromBody] TblEmployeeType requestData)
        {
            var data = _fileMaintain.SaveEmployeeType(requestData);
            return Ok(data);
        }

        [HttpGet("GetEmpTypeList")]
        public IActionResult GetEmpTypeList()
        {
            var data = _fileMaintain.GetEmpTypeList();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);

        }

        [HttpDelete("DelEmployeeType")]
        public IActionResult DelEmployeeType(int Id)
        {
            var data = _fileMaintain.DelEmployeeType(Id);
            return Ok(data);
        }


        [HttpPost("SaveShift")]
        public IActionResult SaveShift([FromBody] TblEmployeeShift requestData)
        {
            var data = _fileMaintain.SaveShift(requestData);
            return Ok(data);
        }

        [HttpGet("GetShiftList")]
        public IActionResult GetShiftList()
        {
            var data = _fileMaintain.GetShiftList();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);

        }

        [HttpDelete("DelShift")]
        public IActionResult DelShift(int Id)
        {
            var data = _fileMaintain.DelShift(Id);
            return Ok(data);
        }

        // Salary Reason

        [HttpPost("SaveSalaryReason")]
        public IActionResult SaveSalaryReason([FromBody] TblSalaryReason requestData)
        {
            var data = _fileMaintain.SaveSalaryReason(requestData);
            return Ok(data);
        }

        [HttpGet("GetSalaryReasonList")]
        public IActionResult GetSalaryReasonList()
        {
            var data = _fileMaintain.GetSalaryReasonList();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);

        }

        [HttpDelete("DelSalaryReason")]
        public IActionResult DelSalaryReason(int Id)
        {
            var data = _fileMaintain.DelSalaryReason(Id);
            return Ok(data);
        }


        // Salary Settlement Labels

        [HttpPost("SaveLabels")]
        public IActionResult SaveLabels([FromBody] TblSalarydtLable requestData)
        {
            var data = _fileMaintain.SaveLabels(requestData);
            return Ok(data);
        }

        [HttpGet("GetSalaryLabels")]
        public IActionResult GetSalaryLabels()
        {
            var List = _fileMaintain.GetSalaryLabels();
            string result = JsonConvert.SerializeObject(List);
            return Ok(result);
        }

        // Salary Days

        [HttpPost("SaveSalaryDays")]
        public IActionResult SaveMonthYear([FromBody] SalaryDay requestData)
        {
            var data = _fileMaintain.SaveSalaryDays(requestData);
            return Ok(data);
        }


        [HttpGet("GetSalaryDays")]
        public IActionResult GetSalaryDays()
        {
            var data = _fileMaintain.GetSalaryDays();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);

        }

        // Set Month and Year

        [HttpPost("SaveMonthYear")]
        public IActionResult SaveMonthYear([FromBody] TblMonth requestData)
        {
            var data = _fileMaintain.SaveMonthYear(requestData);
            return Ok(data);
        }


        [HttpGet("GetMonthYear")]
        public IActionResult GetMonthYear()
        {
            var data = _fileMaintain.GetMonthYear();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);

        }


        // Overtime Formula

        [HttpPost("SaveOvertimeFormula")]
        public IActionResult SaveOvertimeFormula([FromBody] Tblotformula requestData)
        {
            var result = _fileMaintain.SaveOvertimeFormula(requestData);
            return Ok(result);
        }


        [HttpGet("GetOvertimeFormula")]
        public IActionResult GetOvertimeFormula()
        {
            var overtime = _fileMaintain.GetOvertimeFormula();
            string result = JsonConvert.SerializeObject(overtime);
            return Ok(result);
        }

        #endregion

        #region Employee Information

        // Employee Information

        [HttpGet("GetMaxEmpId")]
        public IActionResult GetMaxEmpId()
        {
            var data = _employee.GetMaxEmpId();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);

        }


        [HttpGet("GetDepartment")]
        public IActionResult GetDepartment()
        {
            var data = _employee.GetDepartment();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);

        }

        [HttpGet("GetLocation")]
        public IActionResult GetLocation()
        {
            var data = _employee.GetLocation();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);

        }

        [HttpGet("GetMainLocation")]
        public IActionResult GetMainLocation()
        {
            var data = _employee.GetMainLocation();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);

        }

        [HttpGet("GetStatus")]
        public IActionResult GetStatus()
        {
            var data = _employee.GetStatus();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);

        }

        [HttpGet("GetShift")]
        public IActionResult GetShift()
        {
            var data = _employee.GetShift();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);

        }

        [HttpGet("GetEmployeeList")]
        public IActionResult GetEmployeeList()
        {
            var data = _employee.GetEmployeeList();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);

        }


        [HttpPost("AddUpdateEmployee")]
        public IActionResult AddUpdateEmployee([FromForm] EmployeeVM emp)
        {
            string result = _employee.AddUpdateEmployee(emp);
            return Ok(result);
        }

        [HttpGet("EditEmployee")]
        public IActionResult EditEmployee(int empy_id)
        {
            var data = _employee.EditEmployee(empy_id);

            return Ok(data);
        }

        [HttpDelete("DeleteEmployee")]
        public IActionResult DeleteEmployee(int empy_id)
        {
            var result = _employee.DeleteEmployee(empy_id);
            return Ok(result);
        }



        [HttpGet("GetEmployees")]
        public IActionResult GetEmployees()
        {
            var data = _employee.GetEmployees();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);

        }

        // Salary Setllment

        [HttpGet("GetSalarySettlementNo")]
        public IActionResult GetSalarySettlementNo()
        {
            var SettlementNo = _employee.GetSalarySettlementNo();
            string result = JsonConvert.SerializeObject(SettlementNo);
            return Ok(result);
        }

        [HttpGet("GetSalarySettlementLabels")]
        public IActionResult GetSalarySettlementLabels()
        {
            var SettlementNo = _employee.GetSalarySettlementLabels();
            string result = JsonConvert.SerializeObject(SettlementNo);
            return Ok(result);
        }


        [HttpPost("AddUpdateSalryStlmnt")]
        public IActionResult AddUpdateSalryStlmnt([FromBody] Tblemploysalarydt requestData)
        {
            string result = _employee.AddUpdateSalryStlmnt(requestData);
            return Ok(result);
        }

        [HttpGet("GetEditSalrySetlment")]
        public IActionResult GetEditSalrySetlment(int empy_id)
        {
            var SettlementNo = _employee.GetEditSalrySetlment(empy_id);
            string result = JsonConvert.SerializeObject(SettlementNo);
            return Ok(result);
        }

        [HttpDelete("DelSalrySetlment")]
        public IActionResult DelSalrySetlment(int empy_id, int SrNo)
        {
            var data = _employee.DelSalrySetlment(empy_id, SrNo);
            return Ok(data);
        }

        // Employee Family Information

        [HttpPost("AddUpdateEmpFamily")]
        public IActionResult AddUpdateEmpFamily(List<EmpFamilyVM> family)
        {
            string result = _employee.AddUpdateEmpFamily(family);
            return Ok(result);
        }

        [HttpGet("GetEmpFamilyList")]
        public IActionResult GetEmpFamilyList()
        {
            var List = _employee.GetEmpFamilyList();
            string result = JsonConvert.SerializeObject(List);
            return Ok(result);
        }

        [HttpGet("GetEditEmpFamily")]
        public IActionResult GetEditEmpFamily(int empy_id)
        {
            var List = _employee.GetEditEmpFamily(empy_id);
            string result = JsonConvert.SerializeObject(List);
            return Ok(result);
        }

        [HttpDelete("DelEmpFamily")]
        public IActionResult DelEmpFamily(int empy_id)
        {
            var data = _employee.DelEmpFamily(empy_id);
            return Ok(data);
        }

        #endregion

        #region Provident Fund

        [HttpPost("SavePfDeduction")]
        public IActionResult SavePfDeduction([FromBody] TblParovidentFund requestData)
        {
            string result = _provident.SavePfDeduction(requestData);
            return Ok(result);
        }


        [HttpGet("GetEditPfDeductionList")]
        public IActionResult GetEditPfDeductionList(int empy_id)
        {
            var List = _provident.GetEditPfDeductionList(empy_id);
            string result = JsonConvert.SerializeObject(List);
            return Ok(result);
        }

        [HttpDelete("DelPfDeductions")]
        public IActionResult DelPfDeductions(int empy_id, int SrNo)
        {
            var data = _provident.DelPfDeductions(empy_id, SrNo);
            return Ok(data);
        }

        // Provident Loan


        [HttpPost("SaveProvidentLoan")]
        public IActionResult SaveProvidentLoan([FromBody] Tblploan requestData)
        {
            var data = _provident.SaveProvidentLoan(requestData);
            return Ok(data);
        }

        [HttpGet("GetEditPLoan")]
        public IActionResult GetEditPLoan(int empy_id)
        {
            var List = _provident.GetEditPLoan(empy_id);
            string result = JsonConvert.SerializeObject(List);
            return Ok(result);
        }


        [HttpDelete("DelPLoan")]
        public IActionResult DelPLoan(int empy_id, string Vch, int SrNo)
        {
            var data = _provident.DelPLoan(empy_id, Vch, SrNo);
            return Ok(data);
        }

        // Bank Enrty


        [HttpPost("SaveBank")]
        public IActionResult SaveBank([FromBody] Tblbank requestData)
        {
            var data = _provident.SaveBank(requestData);
            return Ok(data);
        }

        [HttpGet("GetBankList")]
        public IActionResult GetBankList()
        {
            var data = _provident.GetBankList();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);

        }

        [HttpDelete("DelBank")]
        public IActionResult DelBank(int Id)
        {
            var data = _provident.DelBank(Id);
            return Ok(data);
        }

        #endregion

        #region Employee Deductions

        // Employee Deductions
        [HttpPost("SaveIncomeTax")]
        public IActionResult SaveIncomeTax([FromBody] TblIncomeTax requestData)
        {
            var data = _deduction.SaveIncomeTax(requestData);
            return Ok(data);
        }

        [HttpPost("SaveEOBI")]
        public IActionResult SaveEOBI([FromBody] TblEobi requestData)
        {
            var data = _deduction.SaveEOBI(requestData);
            return Ok(data);
        }


        [HttpGet("GetEditIncomeTax")]
        public IActionResult GetEditIncomeTax(int empy_id)
        {
            var Loan = _deduction.GetEditIncomeTax(empy_id);
            return Ok(Loan);
        }

        [HttpGet("GetEditEOBITax")]
        public IActionResult GetEditEOBITax(int empy_id)
        {
            var Loan = _deduction.GetEditEOBITax(empy_id);
            return Ok(Loan);
        }


        [HttpDelete("DelIncomeTax")]
        public IActionResult DelIncomeTax(int empy_id,  int SrNo)
        {
            var data = _deduction.DelIncomeTax(empy_id, SrNo);
            return Ok(data);
        }

        [HttpDelete("DelEOBITax")]
        public IActionResult DelEOBITax(int empy_id, int SrNo)
        {
            var data = _deduction.DelEOBITax(empy_id, SrNo);
            return Ok(data);
        }

        // Loan Deductions


        [HttpPost("SaveStaffLoan")]
        public IActionResult SaveStaffLoan([FromBody] TblStaffLoan requestData)
        {
            var data = _deduction.SaveStaffLoan(requestData);
            return Ok(data);
        }


        [HttpGet("GetEditLoan")]
        public IActionResult GetEditLoan(int empy_id)
        {
            var Loan = _deduction.GetEditLoan(empy_id);
            return Ok(Loan);
        }


        [HttpDelete("DelStaffLoan")]
        public IActionResult DelStaffLoan(int empy_id, string Vch, int SrNo)
        {
            var data = _deduction.DelStaffLoan(empy_id, Vch, SrNo);
            return Ok(data);
        }

        // Vehicel Loan

        [HttpPost("SaveVehicleLoan")]
        public IActionResult SaveVehicleLoan([FromBody] TblVehicleLoan requestData)
        {
            var data = _deduction.SaveVehicleLoan(requestData);
            return Ok(data);
        }


        [HttpGet("GetEditVehicleLoan")]
        public IActionResult GetEditVehicleLoan(int empy_id)
        {
            var Loan = _deduction.GetEditVehicleLoan(empy_id);
            return Ok(Loan);
        }


        [HttpDelete("DelVehicleLoan")]
        public IActionResult DelVehicleLoan(int empy_id, string Vch, int SrNo)
        {
            var data = _deduction.DelVehicleLoan(empy_id, Vch, SrNo);
            return Ok(data);
        }

        // Advance Salary

        [HttpPost("SaveAdvanceSalary")]
        public IActionResult SaveAdvanceSalary([FromBody] TblAdvanceSalary requestData)
        {
            var data = _deduction.SaveAdvanceSalary(requestData);
            return Ok(data);
        }

        [HttpGet("getLevel5Accounts")]
        public IActionResult getLevel5Accounts()
        {
            var data = _deduction.getLevel5Accounts();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);

        }

        [HttpGet("GetEditAdvSalaryList")]
        public IActionResult GetEditAdvSalaryList(int empy_id)
        {
            var salary = _deduction.GetEditAdvSalaryList(empy_id);
            string result = JsonConvert.SerializeObject(salary);
            return Ok(result);
        }

        [HttpDelete("DelAdvSalary")]
        public IActionResult DelAdvSalary(int empy_id, string Vch, int SrNo)
        {
            var data = _deduction.DelAdvSalary(empy_id, Vch, SrNo);
            return Ok(data);
        }


        // Loan Status

        [HttpGet("GetLoanStatus")]
        public IActionResult GetLoanStatus(string type, bool status)
        {
            var data = _deduction.GetLoanStatus(type, status);
            return Ok(data);
        }

        [HttpPost("UpdateLoanStatus")]
        public IActionResult UpdateLoanStatus(List<LoanStatusVM> status)
        {
            var data = _deduction.UpdateLoanStatus(status);
            return Ok(data);
        }

        // Insurance Entry

        [HttpPost("SaveInsrnceLoan")]
        public IActionResult SaveInsrnceLoan([FromBody] Tblinsuranceloan requestData)
        {
            var result = _deduction.SaveInsrnceLoan(requestData);
            return Ok(result);
        }


        [HttpGet("GetEditInsrnceLoan")]
        public IActionResult GetEditInsrnceLoan(int empy_id)
        {
            var Loan = _deduction.GetEditInsrnceLoan(empy_id);
            string result = JsonConvert.SerializeObject(Loan);
            return Ok(result);
        }


        [HttpDelete("DelInsrnceLoan")]
        public IActionResult DelInsrnceLoan(int empy_id, string Vch, int SrNo)
        {
            var data = _deduction.DelInsrnceLoan(empy_id, Vch, SrNo);
            return Ok(data);
        }

        #endregion

        #region Incentives

        [HttpPost("SaveEmpIncentive")]
        public IActionResult SaveEmpIncentive([FromBody] Tblinsentive requestData)
        {
            var data = _incentives.SaveEmpIncentive(requestData);
            return Ok(data);
        }



        [HttpGet("GetEditIncentive")]
        public IActionResult GetEditIncentive(int empy_id)
        {
            var incentive = _incentives.GetEditIncentive(empy_id);
            string result = JsonConvert.SerializeObject(incentive);
            return Ok(result);
        }

        [HttpDelete("deleteIncentive")]
        public IActionResult deleteIncentive(int empy_id, int SrNo)
        {
            var data = _incentives.deleteIncentive(empy_id, SrNo);
            return Ok(data);
        }

        // Arrears

        [HttpPost("SaveEmpArrears")]
        public IActionResult SaveEmpArrears([FromBody] TblArrear requestData)
        {
            var data = _incentives.SaveEmpArrears(requestData);
            return Ok(data);
        }

        [HttpGet("GetEditArrearsList")]
        public IActionResult GetEditArrearsList(int empy_id)
        {
            var incentive = _incentives.GetEditArrearsList(empy_id);
            string result = JsonConvert.SerializeObject(incentive);
            return Ok(result);
        }

        [HttpDelete("deleteEmpArrears")]
        public IActionResult deleteEmpArrears(int empy_id, int SrNo)
        {
            var data = _incentives.deleteEmpArrears(empy_id, SrNo);
            return Ok(data);
        }


        // Leave Enchasement

        [HttpPost("SaveLeaveEnchasment")]
        public IActionResult SaveLeaveEnchasment([FromBody] TblLvEnchasment requestData)
        {
            var data = _incentives.SaveLeaveEnchasment(requestData);
            return Ok(data);
        }


        [HttpGet("GetEditLeaveEnchasment")]
        public IActionResult GetEditLeaveEnchasment(int empy_id)
        {
            var enchasement = _incentives.GetEditLeaveEnchasment(empy_id);
            string result = JsonConvert.SerializeObject(enchasement);
            return Ok(result);
        }

        [HttpDelete("deleteLeaveEnchasment")]
        public IActionResult deleteLeaveEnchasment(int empy_id, int SrNo)
        {
            var data = _incentives.deleteLeaveEnchasment(empy_id, SrNo);
            return Ok(data);
        }

        // Yearly Bonus

        [HttpPost("SaveYearlyBonus")]
        public IActionResult SaveYearlyBonus([FromBody] TblYearlybonu requestData)
        {
            var data = _incentives.SaveYearlyBonus(requestData);
            return Ok(data);
        }

        [HttpGet("GetEditBonusList")]
        public IActionResult GetEditBonusList(int empy_id)
        {
            var enchasement = _incentives.GetEditBonusList(empy_id);
            string result = JsonConvert.SerializeObject(enchasement);
            return Ok(result);
        }

        [HttpDelete("DelYearlyBonus")]
        public IActionResult DelYearlyBonus(int empy_id, int SrNo)
        {
            var data = _incentives.DelYearlyBonus(empy_id, SrNo);
            return Ok(data);
        }

        [HttpPost("SaveEmployeeOvertime")]
        public IActionResult SaveEmployeeOvertime([FromBody] TblOverTime requestData)
        {
            var data = _incentives.SaveEmployeeOvertime(requestData);
            return Ok(data);
        }

        // Overtime 

        [HttpGet("GetEditOvertime")]
        public IActionResult GetEditOvertime(int empy_id)
        {
            var overtime = _incentives.GetEditOvertime(empy_id);

            return Ok(overtime);
        }

        [HttpDelete("DelOvertime")]
        public IActionResult DelOvertime(int empy_id, int SrNo)
        {
            var data = _incentives.DelOvertime(empy_id, SrNo);
            return Ok(data);
        }

        #endregion

        #region Employee Leaves

        // Leaves Entry
        [HttpGet("GetLeaveType")]
        public IActionResult GetLeaveType()
        {
            var List = _leaves.GetLeaveType();
            string result = JsonConvert.SerializeObject(List);
            return Ok(result);
        }



        [HttpPost("SaveLeavesEntry")]
        public IActionResult SaveLeavesEntry([FromBody] TblleavesEntry requestData)
        {
            var data = _leaves.SaveLeavesEntry(requestData);
            return Ok(data);
        }

        [HttpGet("GetEditEmpLeaves")]
        public IActionResult GetEditEmpLeaves(int empy_id)
        {
            var List = _leaves.GetEditEmpLeaves(empy_id);
            return Ok(List);
        }


        [HttpDelete("DelLeaves")]
        public IActionResult DelLeaves(int empy_id, int SrNo)
        {
            var data = _leaves.DelLeaves(empy_id, SrNo);
            return Ok(data);
        }

        #endregion

        #region Salary Calculation

        // Salary Type

        [HttpPost("SaveSalaryType")]
        public IActionResult SaveSalaryType([FromBody] TblSalaryType requestData)
        {
            var data = _salaryCalculation.SaveSalaryType(requestData);
            return Ok(data);
        }

        [HttpGet("GetSalaryTypeList")]
        public IActionResult GeSalaryTypeList()
        {
            var data = _salaryCalculation.GetSalaryTypeList();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);

        }


        [HttpDelete("DelSalaryType")]
        public IActionResult DelSalaryType(int Id)
        {
            var data = _salaryCalculation.DelSalaryType(Id);
            return Ok(data);
        }

        // Salary Payables

        [HttpGet("getSalaryPayables")]
        public IActionResult getSalaryPayables(int Month, int year)
        {
            var Loan = _salaryCalculation.getSalaryPayables(Month, year);
            string result = JsonConvert.SerializeObject(Loan);
            return Ok(result);
        }


        #endregion

        #region Audit 

        [HttpGet("GetVouchers")]
        public IActionResult GetVouchers(string type)
        {
            var data = _audit.GetVouchers(type);
            return Ok(data);

        }


        [HttpPost("UpdateVoucherStatus")]
        public IActionResult UpdateVoucherStatus(List<VoucherApprovalVM> status)
        {
            var data = _audit.UpdateVoucherStatus(status);
            return Ok(data);
        }

        // Allow Same Leaves

        [HttpGet("AllowSameLeave")]
        public IActionResult AllowSameLeave(int empy_id)
        {
            var data = _audit.AllowSameLeave(empy_id);
            return Ok(data);
        }

        #endregion
    }
}
