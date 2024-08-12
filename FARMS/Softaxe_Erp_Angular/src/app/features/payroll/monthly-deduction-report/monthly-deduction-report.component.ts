import { Component } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { ApiService } from 'src/app/services/api.service';
import { ToastrService } from 'ngx-toastr';
import { DatePipe } from '@angular/common';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-monthly-deduction-report',
  templateUrl: './monthly-deduction-report.component.html',
  styleUrls: ['./monthly-deduction-report.component.css']
})
export class MonthlyDeductionReportComponent {

  constructor(
    private tostr: ToastrService,
    private apiService: ApiService,
    private com:CommonService,
    private auth: AuthService,
    private dp:DatePipe
  )
  {
    const today = new Date();
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  }

  fromDate:Date;
  toDate:Date;
  LocationId:any;
  DepartmentId:number;
  DesignationId:number;
  EmpID:number;
  // empId:any;
  empStatusId:number;


  
  openReportModal() {
   
    const fDate = this.dp.transform(this.fromDate, 'yyyy/MM/dd');
    const toDate = this.dp.transform(this.toDate, 'yyyy/MM/dd');

    let url = `EmpMonthlyDeduction?comp_id=${this.auth.cmpId()}&start_date=${fDate}&end_date=${toDate}`;
  
    // Check and append LocationId
    url += this.LocationId != null && this.LocationId !== undefined ? `&location=${this.LocationId}` : '&location=%';
  
    // Check and append DepartmentId
    url += this.DepartmentId != null && this.DepartmentId !== undefined ? `&department=${this.DepartmentId}` : '&department=%';
  
    // Check and append DesignationId
    url += this.DesignationId != null && this.DesignationId !== undefined ? `&designation=${this.DesignationId}` : '&designation=%';

    url += this.EmpID != null && this.EmpID !== undefined ? `&emp_id=${this.EmpID}` :'&emp_id=%';

    url += this.empStatusId != null && this.empStatusId !== undefined ? `&empstatus=${this.empStatusId}` : '&empstatus=%';
  
    // Check and append empId
    url += this.EmpID != null && this.EmpID !== undefined ? `&emp_id=${this.EmpID}` :'&emp_id=%';
  
  
  
    this.com.viewReport(url);
}




ngOnInit() 
{ 
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



}
