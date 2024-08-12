import { Component } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { ApiService } from 'src/app/services/api.service';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder, FormGroup } from '@angular/forms';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-advanced-salary-report',
  templateUrl: './advanced-salary-report.component.html',
  styleUrls: ['./advanced-salary-report.component.css']
})
export class AdvancedSalaryReportComponent {

  
  constructor(
    private tostr: ToastrService,
    private apiService: ApiService,
    private com: CommonService,
    private auth: AuthService,
    private fb: FormBuilder,
  )
  {
    
  }

  AdvSalaryForm!: FormGroup;


  
  openReportModal() {

    let form = this.AdvSalaryForm.value;

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

    let url = `empAdvanceSalary?comp_id=${this.auth.cmpId()}`;


    url += form.DepartmentId != null && form.DepartmentId !== undefined ? `&department=${form.DepartmentId}` : '&department=%';
    url += form.DesignationId != null && form.DesignationId !== undefined ? `&designation=${form.DesignationId}` : '&designation=%';

    url += form.empy_id != null && form.empy_id !== undefined ? `&emp_id=${form.empy_id}` :'&emp_id=%';

    url += form.empStatusId != null && form.empStatusId !== undefined ? `&empstatus=${form.empStatusId}` : '&empstatus=%';

    url += form.LocationId != null && form.LocationId !== undefined ? `&locId=${form.LocationId}` : '&location=%';

    this.com.viewReport(url);
}



formInit() 
{ 
  this.AdvSalaryForm = this.fb.group({

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
  this.GetMainLocation();
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


GetMainLocation() {
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
        this.AdvSalaryForm.patchValue({ empName: selectedEmployee.empy_id });
      }
      else{
        this.tostr.info('No Employee Found');  
      }

}

SelectEmployee(event:any) {
        this.AdvSalaryForm.patchValue({ empy_id: event.empy_id });
}

 
}
