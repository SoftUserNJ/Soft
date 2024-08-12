import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-insurance-loan-report',
  templateUrl: './insurance-loan-report.component.html',
  styleUrls: ['./insurance-loan-report.component.css']
})
export class InsuranceLoanReportComponent {

   
  constructor(
    private tostr: ToastrService,
    private apiService: ApiService,
    private auth: AuthService,
    private dp:DatePipe,
    private fb: FormBuilder,
    private com: CommonService
  )
  {
    
  }

  LoanLedgerForm!: FormGroup;

  openReportModal() {

    let form = this.LoanLedgerForm.value;

    if (form.empy_id === null || form.empy_id === undefined) {
      this.tostr.warning('Select Employee....!');
      return;
    }

    if (form.LocationId === null || form.LocationId === undefined) {
      this.tostr.warning('Select Location....!');
      return;
    }


    if (form.DepartmentId === null || form.DepartmentId === undefined) {
      this.tostr.warning('Select Department....!');
      return;
    }

    if (form.DesignationId === null || form.DesignationId === undefined) {
      this.tostr.warning('Select Designation....!');
      return;
    }

    if (form.empStatusId === null || form.empStatusId === undefined) {
      this.tostr.warning('Select Employe Status....!');
      return;
    }




    const FDate = this.dp.transform(form.fromDate, 'yyyy-MM-dd');
    const TDate = this.dp.transform(form.toDate, 'yyyy-MM-dd');

    let url = `InsuranceLoanReport?Fdate=${FDate}&Tdate=${TDate}`;

    url += form.empy_id != null && form.empy_id !== undefined ? `&empy_id=${form.empy_id}` :'&empy_id=%';


    url += form.LocationId != null && form.LocationId !== undefined ? `&LocId=${form.LocationId}` : '&LocId=%';
    
    url += `&CompId=${this.auth.cmpId()}`;

    url += form.empStatusId != null && form.empStatusId !== undefined ? `&EmpStatus=${form.empStatusId}` : '&EmpStatus=%';

    url += form.DepartmentId != null && form.DepartmentId !== undefined ? `&departmentId=${form.DepartmentId}` : '&departmentId=%';


    url += form.DesignationId != null && form.DesignationId !== undefined ? `&designationId=${form.DesignationId}` : '&designationId=%&Logo=';

  this.com.viewReport(url);
}



formInit() 
{ 
  this.LoanLedgerForm = this.fb.group({

    empName: [undefined],
    empy_id: [''],
    fromDate: [new Date()],
    toDate: [new Date()],
    LocationId: [undefined],
    DepartmentId: [undefined],
    DesignationId: [undefined],
    empStatusId: [undefined],
  });
}


ngOnInit() 
{ 
  this.formInit();
  this.getDepartmentList();
  this.getLocation();
  this.getDesignationList();
  this.getEmployeeList();
  this.getEmpStatus();
}

  
  voucherList: any[]=[];
  LocationList: any[]=[];
  DesignationList: any[]=[];
  EmployeeList: any[]=[];
  EmployeeStarusList:any[]=[];

 
  getDepartmentList() {

    this.apiService.getData('PayRoll/GetDepartmentList').subscribe((data)=>{
      this.voucherList = data;
    })
}


getLocation() {
  this.apiService.getData('PayRoll/GetMainLocation').subscribe((data) => {
    this.LocationList = data;
  });
}


getDesignationList() {
  this.apiService.getData('PayRoll/GetDesignationList').subscribe((data)=>{
    this.DesignationList = data;
  })
}

getEmployeeList() {
this.apiService.getData('PayRoll/GetEmployees').subscribe((data)=>{
  this.EmployeeList = data;
})
}

getEmpStatus() {
this.apiService.getData('PayRoll/GetStatus').subscribe((data)=>{
  this.EmployeeStarusList = data;
})
}


InputEmployee(event:any) {
  const selectedEmployeeId: number = parseInt(event.target.value, 10);
      const selectedEmployee = this.EmployeeList.find(employee => employee.empy_id === selectedEmployeeId);
      if (selectedEmployee) {
        this.LoanLedgerForm.patchValue({ empName: selectedEmployee.empy_id });
      }
      else{
        this.tostr.info('No Employee Found');  
      }

}

SelectEmployee(event:any) {
        this.LoanLedgerForm.patchValue({ empy_id: event.empy_id });
}

}
