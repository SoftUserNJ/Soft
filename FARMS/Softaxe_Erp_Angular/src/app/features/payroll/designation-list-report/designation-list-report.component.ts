import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-designation-list-report',
  templateUrl: './designation-list-report.component.html',
  styleUrls: ['./designation-list-report.component.css']
})
export class DesignationListReportComponent {

  
  constructor(
    private tostr: ToastrService,
    private apiService: ApiService,
    private com: CommonService,
    private auth: AuthService,
    private dp:DatePipe,
    private fb: FormBuilder,
  )
  {
    
  }

  RptDesgListForm!: FormGroup;


  
  openReportModal() {

    let form = this.RptDesgListForm.value;

    if (form.LocationId === null || form.LocationId === undefined) {
      this.tostr.warning('Select Location....!');
      return;
    }


    let url = `RptDesignationListing?CmpId=${this.auth.cmpId()}`;

    url += form.LocationId != null && form.LocationId !== undefined ? `&LocId=${form.LocationId}` : '&LocId=%';

    this.com.viewReport(url);
}



formInit() 
{ 
  this.RptDesgListForm = this.fb.group({
    LocationId: [undefined],
  });
}


ngOnInit() 
{ 
  this.formInit();
  this.getLocation();
}

  
  voucherList: any[]=[];
  LocationList: any[]=[];
  DesignationList: any[]=[];
  EmployeeList: any[]=[];
  EmployeeStarusList:any[]=[];

 


getLocation() {
  this.apiService.getData('PayRoll/GetMainLocation').subscribe((data) => {
    this.LocationList = data;
  });
}

}
