import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import * as moment from 'moment';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-salary-sheet',
  templateUrl: './salary-sheet.component.html',
  styleUrls: ['./salary-sheet.component.css']
})
export class SalarySheetComponent {


  constructor(
    private tostr: ToastrService,
    private fb: FormBuilder,
    private apiService: ApiService,
    private dp: DatePipe,
    private com: CommonService,
    private auth: AuthService,
  ) {
    
  }

  @ViewChild('voucherLists', { static: false }) voucherLists!: ElementRef;

  SalarySheetForm!: FormGroup;
  isShow = false;
  isReadOnly: boolean = true;
  editMode: boolean = true;
  editSno: any = '';
  editModeSno: boolean = false;
  selectedRow: number | null = null;
  btnAdd:string = 'Add';
  EmployeeList : any[] = [];
  voucherList: any[] = [];
  DepartmentList: any = [];
  LocationList: any = [];
  SalaryTypeList: any = [];
  YearList: any = [];
  DayA:number;
  year: number;
  monthName: string;
  monthNumber: number;



  ngOnInit() {

    this.formInit();
    this.disableFields();
    this.getEmployeeList();
    this.getDepartment();
    this.getLocation();
    this.getSalaryTypeList();
    this.getYearMonth();
    this.setLastDayOfMonth();
  }

  formInit() {
    this.SalarySheetForm = this.fb.group({
      days: [''],
      fromDateDW: [new Date()],
      toDateDW: [new Date()],
      salaryDate: [new Date()],
      DW: [''],
      summary: [''],
      executive: [''],
      production: [''],
      format2Sheet: [''],
      format3Sheet: [''],
      productionWo: [''],
      format3DW: [''],
      admnstrationStaff: [''],
      format2Bank: [''],
      format4: [''],
      format5: [''],
      year: [undefined],
      location: [undefined],
      SalaryType: [undefined],
      empName: [undefined],
      department: [undefined],

    });
  }

  getDepartment() {
    this.apiService.getData('PayRoll/GetDepartment').subscribe((data) => {
      this.DepartmentList = data;
    });
  }

  getEmployeeList() {
    this.apiService.getData('PayRoll/GetEmployees').subscribe((data)=>{
      this.EmployeeList = data;
    })
  }

  getYearMonth() {
    this.apiService.getData('PayRoll/GetMonthYear').subscribe((data) => {
      this.YearList = data;
    });
  }


  getLocation() {
    this.apiService.getData('PayRoll/GetLocation').subscribe((data) => {
      this.LocationList = data;
    });
  }

  setLastDayOfMonth() {
    const currentDate = new Date();
    const lastDayOfMonth = new Date(currentDate.getFullYear(), currentDate.getMonth() + 1, 0);
    this.SalarySheetForm.get('salaryDate').setValue(lastDayOfMonth);
  }

  getSalaryTypeList() {
    this.apiService.getData('PayRoll/GetSalaryTypeList').subscribe((data)=>{
      this.SalaryTypeList = data;
    })
}

setMonthAndDays() {
  
  var year = this.SalarySheetForm.get('year').value;
  var month = this.SalarySheetForm.get('salaryDate').value;
  const monthName = moment(month).format('MMMM');
  const monthNo = moment(month).month() + 1;
  const daysInMonth = moment(month).daysInMonth();
    this.DayA = daysInMonth;
    this.year = year;
    this.monthNumber = monthNo;
  const formattedValue = `${monthName} - ${year} - Days ${daysInMonth}`;
  this.monthName = `${monthName} - ${year}`;
  this.SalarySheetForm.get('days').setValue(formattedValue);
}

onClearYear() {
  this.SalarySheetForm.get('days').setValue('');
}

  onClickNew() {
    this.isShow = true;
    this.editMode = false;
    this.enableFields();
  }

  onClickRefresh() {
    this.isShow = false;
    this.resetForm();
    this.disableFields();
  }


  disableFields() {
  }



  enableFields() {

  }

  
  resetForm() {

  }

  resetFormOnAdd() {

  }

  openReportModal() {
    let form = this.SalarySheetForm.value;
  if(form.year == null || form.year == undefined ){
    this.tostr.info('Select Year');
    return;
  }
  
  const lastDateOfMonth = moment().endOf('month');
  const trDate = lastDateOfMonth.format('M/DD/YYYY');
  let url = `EmpSalarySheet?cal=0&comp_id=${this.auth.cmpId()}&Crit=0&Crit1=9999&CritD=0&CritD1=99999&DayA=${this.DayA}&EO=true&Loc=%&LocId=${this.auth.locId()}&month=${this.monthNumber}&Pro=true&PRP=%&Salary=%&trDate=${trDate}&year=${this.year}&monthName=${this.monthName}`;

  this.com.viewReport(url);
}
  

CalculatePrint() {
  let form = this.SalarySheetForm.value;
  if(form.year == null || form.year == undefined ){
    this.tostr.info('Select Year');
    return;
  }

  const lastDateOfMonth = moment().endOf('month');
  const trDate = lastDateOfMonth.format('M/DD/YYYY');
  let url = `EmpSalarySheet?cal=1&comp_id=${this.auth.cmpId()}&Crit=0&Crit1=9999&CritD=0&CritD1=99999&DayA=${this.DayA}&EO=true&Loc=%&LocId=${this.auth.locId()}&month=${this.monthNumber}&Pro=true&PRP=%&Salary=%&trDate=${trDate}&year=${this.year}&monthName=${this.monthName}`;

  this.com.viewReport(url);
}

}
