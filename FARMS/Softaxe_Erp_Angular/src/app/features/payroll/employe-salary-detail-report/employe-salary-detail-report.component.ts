import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-employe-salary-detail-report',
  templateUrl: './employe-salary-detail-report.component.html',
  styleUrls: ['./employe-salary-detail-report.component.css']
})
export class EmployeSalaryDetailReportComponent {

  
  constructor(
    private tostr: ToastrService,
    private apiService: ApiService,
    private com: CommonService,
    private auth: AuthService,
    private fb: FormBuilder,
  )
  {
    
  }

  EmpSalaryForm!: FormGroup;


  
  openReportModal() {

    let form = this.EmpSalaryForm.value;

    let url = `EmployeeSalaryDetail?comp_id=${this.auth.cmpId()}&LocId=${this.auth.locId()}`;


    url += form.DepartmentId != null && form.DepartmentId !== undefined ? `&department=${form.DepartmentId}` : '&department=%';
    url += form.DesignationId != null && form.DesignationId !== undefined ? `&designation=${form.DesignationId}` : '&designation=%';

    url += form.empy_id != null && form.empy_id !== undefined ? `&emp_id=${form.empy_id}` :'&emp_id=%';

    url += form.empStatusId != null && form.empStatusId !== undefined ? `&empstatus=${form.empStatusId}` : '&empstatus=%';


    this.com.viewReport(url);
}



formInit() 
{ 
  this.EmpSalaryForm = this.fb.group({

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

    this.apiService.getData('Payroll/GetDepartment').subscribe((data)=>{
      this.voucherList = data;
    })
}


getLocation() {
  this.apiService.getData('Employee/GetLocation').subscribe((data) => {
    this.LocationList = data;
  });
}


getDesignationList() {
  this.apiService.getData('Payroll/GetDepartmentList').subscribe((data)=>{
    this.DesignationList = data;
  })
}

getEmployeeList() {
this.apiService.getData('Payroll/GetEmployees').subscribe((data)=>{
  this.EmployeeList = data;
})
}

getEmpStatus() {
this.apiService.getData('Payroll/GetStatus').subscribe((data)=>{
  this.EmployeeStarusList = data;
})
}


InputEmployee(event:any) {
  const selectedEmployeeId: number = parseInt(event.target.value, 10);
      const selectedEmployee = this.EmployeeList.find(employee => employee.empy_id === selectedEmployeeId);
      if (selectedEmployee) {
        this.EmpSalaryForm.patchValue({ empName: selectedEmployee.empy_id });
      }
      else{
        this.tostr.info('No Employee Found');  
      }

}

SelectEmployee(event:any) {
        this.EmpSalaryForm.patchValue({ empy_id: event.empy_id });
}

  }
  



