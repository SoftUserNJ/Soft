import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-prov-loan-balance-report',
  templateUrl: './prov-loan-balance-report.component.html',
  styleUrls: ['./prov-loan-balance-report.component.css']
})
export class ProvLoanBalanceReportComponent {
  
  
   
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

  ProvLoanBlanaceForm!: FormGroup;

  openReportModal() {
    let form = this.ProvLoanBlanaceForm.value;
    let url = `RptProvLoanBalance?`;

    if (form.empName != null && form.empName !== undefined) {
        url += `empy_id=${form.empName}`;
    }

    if (form.LocationId != null && form.LocationId !== undefined) {
        url += `&LocId=${form.LocationId}`;
    }

    url += `&CompId=${this.auth.cmpId()}`;

    if (form.empStatusId != null && form.empStatusId !== undefined) {
        url += `&EmpStatus=${form.empStatusId}`;
    }

    if (form.DepartmentId != null && form.DepartmentId !== undefined) {
        url += `&departmentId=${form.DepartmentId}`;
    }

    if (form.DesignationId != null && form.DesignationId !== undefined) {
        url += `&designationId=${form.DesignationId}`;
    }

    this.com.viewReport(url);
}




formInit() 
{ 
  this.ProvLoanBlanaceForm = this.fb.group({

    empName: [undefined],
    empy_id: [''],
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
        this.ProvLoanBlanaceForm.patchValue({ empName: selectedEmployee.empy_id });
      }
      else{
        this.tostr.info('No Employee Found');  
      }

}

SelectEmployee(event:any) {
        this.ProvLoanBlanaceForm.patchValue({ empy_id: event.empy_id });
}

}
