import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { NgSelectModule } from '@ng-select/ng-select';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CalendarModule } from 'primeng/calendar';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { BrowserModule } from '@angular/platform-browser';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatListModule } from '@angular/material/list';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { authGuard } from 'src/app/guards/auth.guard';
import { DepartmentHRComponent } from './department-hr/department-hr.component';
import { DesignationEntryComponent } from './designation-entry/designation-entry.component';
import { HolidaySetupComponent } from './holiday-setup/holiday-setup.component';
import { LeaveTypeComponent } from './leave-type/leave-type.component';
import { EmployeeTypeComponent } from './employee-type/employee-type.component';
import { ShiftEntryComponent } from './shift-entry/shift-entry.component';
import { SalaryReasonComponent } from './salary-reason/salary-reason.component';
import { SalarySettlementLabelsComponent } from './salary-settlement-labels/salary-settlement-labels.component';
import { SalaryDaysComponent } from './salary-days/salary-days.component';
import { SetMonthsYearsComponent } from './set-months-years/set-months-years.component';
import { OvertimeFormulaComponent } from './overtime-formula/overtime-formula.component';
import { EmployeeEntryComponent } from './employee-entry/employee-entry.component';
import { EmployeeFamilyComponent } from './employee-family/employee-family.component';
import { SalarySettlementComponent } from './salary-settlement/salary-settlement.component';
import { BankEntryComponent } from './bank-entry/bank-entry.component';
import { ProvidentFundDeductionComponent } from './provident-fund-deduction/provident-fund-deduction.component';
import { ProvidentLoanComponent } from './provident-loan/provident-loan.component';
import { IncomeTaxComponent } from './income-tax/income-tax.component';
import { EobiEntryComponent } from './eobi-entry/eobi-entry.component';
import { StaffLoanComponent } from './staff-loan/staff-loan.component';
import { AdvanceSalaryComponent } from './advance-salary/advance-salary.component';
import { VehicleLoanComponent } from './vehicle-loan/vehicle-loan.component';
import { InsuranceEntryComponent } from './insurance-entry/insurance-entry.component';
import { OtherDeductionComponent } from './other-deduction/other-deduction.component';
import { LoanStatusComponent } from './loan-status/loan-status.component';
import { ArrearsComponent } from './arrears/arrears.component';
import { EmployeeIncentiveComponent } from './employee-incentive/employee-incentive.component';
import { LeaveEnchasementComponent } from './leave-enchasement/leave-enchasement.component';
import { OvertimeComponent } from './overtime/overtime.component';
import { YearlyBonusComponent } from './yearly-bonus/yearly-bonus.component';
import { LeavesEntryComponent } from './leaves-entry/leaves-entry.component';
import { SalarySheetComponent } from './salary-sheet/salary-sheet.component';
import { SalaryTypeComponent } from './salary-type/salary-type.component';
import { AllowSameLeaveComponent } from './allow-same-leave/allow-same-leave.component';
import { VouchersApprovalComponent } from './vouchers-approval/vouchers-approval.component';
import { EmployeSalaryDetailReportComponent } from './employe-salary-detail-report/employe-salary-detail-report.component';
import { AdvancedSalaryReportComponent } from './advanced-salary-report/advanced-salary-report.component';
import { StaffLoanBalanceReportComponent } from './staff-loan-balance-report/staff-loan-balance-report.component';
import { VehicleLoanReportComponent } from './vehicle-loan-report/vehicle-loan-report.component';
import { InsuranceLoanReportComponent } from './insurance-loan-report/insurance-loan-report.component';
import { IPEDeductionReportComponent } from './ipe-deduction-report/ipe-deduction-report.component';
import { ProvidentLoanReportComponent } from './provident-loan-report/provident-loan-report.component';
import { DesignationListReportComponent } from './designation-list-report/designation-list-report.component';
import { DepartmentListReportComponent } from './department-list-report/department-list-report.component';
import { ProvLoanBalanceReportComponent } from './prov-loan-balance-report/prov-loan-balance-report.component';
import { LoanLedgerComponent } from './loan-ledger/loan-ledger.component';
import { MonthlyDeductionReportComponent } from './monthly-deduction-report/monthly-deduction-report.component';
import { InsuranceLoanBalanceReportComponent } from './insurance-loan-balance-report/insurance-loan-balance-report.component';
import { VehicleLoanBalanceReportComponent } from './vehicle-loan-balance-report/vehicle-loan-balance-report.component';
import { SalaryPayableComponent } from './salary-payable/salary-payable.component';

@NgModule({
  declarations: [
    DepartmentHRComponent,
    DesignationEntryComponent,
    HolidaySetupComponent,
    LeaveTypeComponent,
    EmployeeTypeComponent,
    ShiftEntryComponent,
    SalaryReasonComponent,
    SalarySettlementLabelsComponent,
    SalaryDaysComponent,
    SetMonthsYearsComponent,
    OvertimeFormulaComponent,
    EmployeeEntryComponent,
    EmployeeFamilyComponent,
    SalarySettlementComponent,
    BankEntryComponent,
    ProvidentFundDeductionComponent,
    ProvidentLoanComponent,
    IncomeTaxComponent,
    EobiEntryComponent,
    StaffLoanComponent ,
    AdvanceSalaryComponent,
    VehicleLoanComponent,
    InsuranceEntryComponent,
    OtherDeductionComponent,
    LoanStatusComponent,
    EmployeeIncentiveComponent,
    LeaveEnchasementComponent,
    ArrearsComponent,
    YearlyBonusComponent,
    OvertimeComponent,
    LeavesEntryComponent,
    SalarySheetComponent,
    SalaryTypeComponent,
    VouchersApprovalComponent,
    AllowSameLeaveComponent,
    EmployeSalaryDetailReportComponent,
    AdvancedSalaryReportComponent,
    StaffLoanBalanceReportComponent,
    VehicleLoanReportComponent,
    InsuranceLoanReportComponent,
    IPEDeductionReportComponent,
    ProvidentLoanReportComponent,
    DesignationListReportComponent,
    DepartmentListReportComponent,
    ProvLoanBalanceReportComponent,
    LoanLedgerComponent,
    MonthlyDeductionReportComponent,
    InsuranceLoanBalanceReportComponent,
    VehicleLoanBalanceReportComponent,
    SalaryPayableComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild([
      { path: 'department-form', component: DepartmentHRComponent, canActivate: [authGuard] },
      { path: 'designation-form', component: DesignationEntryComponent, canActivate: [authGuard] },
      { path: 'holiday-setup', component: HolidaySetupComponent, canActivate: [authGuard] },
      { path: 'leave-type-entry', component: LeaveTypeComponent, canActivate: [authGuard] },
      { path: 'employee-type', component: EmployeeTypeComponent, canActivate: [authGuard] },
      { path: 'shift-entry', component: ShiftEntryComponent, canActivate: [authGuard] },
      { path: 'salary-reason', component: SalaryReasonComponent, canActivate: [authGuard] },
      { path: 'salary-settlement-labels', component: SalarySettlementLabelsComponent, canActivate: [authGuard] },
      { path: 'salary-days', component: SalaryDaysComponent, canActivate: [authGuard] },
      { path: 'set-months-years', component: SetMonthsYearsComponent, canActivate: [authGuard] },
      { path: 'overtime-formula', component: OvertimeFormulaComponent, canActivate: [authGuard] },
      { path: 'employee-entry', component: EmployeeEntryComponent, canActivate: [authGuard] },
      { path: 'salary-settlement', component: SalarySettlementComponent, canActivate: [authGuard] },
      { path: 'employee-family', component: EmployeeFamilyComponent, canActivate: [authGuard] },
      { path: 'bank-entry', component: BankEntryComponent, canActivate: [authGuard] },
      { path: 'provident-fund-deduction', component: ProvidentFundDeductionComponent, canActivate: [authGuard] },
      { path: 'provident-loan', component: ProvidentLoanComponent, canActivate: [authGuard] },
      { path: 'income-tax', component: IncomeTaxComponent, canActivate: [authGuard] },
      { path: 'eobi-entry', component: EobiEntryComponent, canActivate: [authGuard] },
      { path: 'staff-loan', component: StaffLoanComponent, canActivate: [authGuard] },
      { path: 'advance-salary', component: AdvanceSalaryComponent, canActivate: [authGuard] },
      { path: 'vehicle-loan', component: VehicleLoanComponent, canActivate: [authGuard] },
      { path: 'insurance-entry', component: InsuranceEntryComponent, canActivate: [authGuard] },
      { path: 'other-deduction', component: OtherDeductionComponent, canActivate: [authGuard] },
      { path: 'loan-status', component: LoanStatusComponent, canActivate: [authGuard] },
      { path: 'employee-incentive', component: EmployeeIncentiveComponent, canActivate: [authGuard] },
      { path: 'leave-enchasement', component: LeaveEnchasementComponent, canActivate: [authGuard] },
      { path: 'arrears', component: ArrearsComponent, canActivate: [authGuard] },
      { path: 'yearly-bonus', component: YearlyBonusComponent, canActivate: [authGuard] },
      { path: 'employee-overtime', component: OvertimeComponent, canActivate: [authGuard] },
      { path: 'leaves-entry', component: LeavesEntryComponent, canActivate: [authGuard] },
      { path: 'salary-sheet', component: SalarySheetComponent, canActivate: [authGuard] },
      { path: 'salary-type', component: SalaryTypeComponent, canActivate: [authGuard] },
      { path: 'allow-same-leave', component: AllowSameLeaveComponent, canActivate: [authGuard] },
      { path: 'vouchers-aprroval', component: VouchersApprovalComponent, canActivate: [authGuard] },
      { path: 'Employee-Salary-Detail-Report', component: EmployeSalaryDetailReportComponent, canActivate: [authGuard] },
      { path: 'Advanced-salary-Report', component: AdvancedSalaryReportComponent, canActivate: [authGuard] },
      { path: 'staff-loan-balance-report', component: StaffLoanBalanceReportComponent, canActivate: [authGuard] },
      { path: 'vehicel-loan-report', component: VehicleLoanReportComponent, canActivate: [authGuard] },
      { path: 'insurance-loan-report', component: InsuranceLoanReportComponent, canActivate: [authGuard] },
      { path: 'ipe-deduction-report', component: IPEDeductionReportComponent, canActivate: [authGuard] },
      { path: 'provident-loan-report', component: ProvidentLoanReportComponent, canActivate: [authGuard] },
      { path: 'designation-list-report', component: DesignationListReportComponent, canActivate: [authGuard] },
      { path: 'department-list-report', component: DepartmentListReportComponent, canActivate: [authGuard] },
      { path: 'prov-loan-balance-report', component: ProvLoanBalanceReportComponent, canActivate: [authGuard] },
      { path: 'employee-loan-ledger', component: ProvLoanBalanceReportComponent, canActivate: [authGuard] },
      { path: 'Monthly-deduction-Report', component: ProvLoanBalanceReportComponent, canActivate: [authGuard] },
      { path: 'insurance-loan-balance-report', component: InsuranceLoanBalanceReportComponent, canActivate: [authGuard] },
      { path: 'vehicle-loan-balance-report', component: VehicleLoanBalanceReportComponent, canActivate: [authGuard] },
      { path: 'salary-payable', component: SalaryPayableComponent, canActivate: [authGuard] },
    

      
    ]),
    NgSelectModule,
    RouterModule,
    FormsModule,
    MatButtonModule,
    MatInputModule,
    MatSelectModule,
    MatToolbarModule,
    MatListModule,
    MatFormFieldModule,
    MatIconModule,
    MatCardModule ,
    MatAutocompleteModule,
    BrowserModule,
    MatDatepickerModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    CalendarModule,
    DatePipe
  ]
})
export class PayrollModule { }
