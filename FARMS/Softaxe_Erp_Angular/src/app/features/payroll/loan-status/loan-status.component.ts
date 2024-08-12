import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-loan-status',
  templateUrl: './loan-status.component.html',
  styleUrls: ['./loan-status.component.css']
})
export class LoanStatusComponent implements OnInit {

  constructor(
    private tostr: ToastrService,
    private apiService: ApiService,
    private fb: FormBuilder,
  ) {}
  LoanStatusForm!: FormGroup;
  running: boolean;
  tmpStop: boolean;
  StatusData: any[] = [];
  status: boolean = true;


  ngOnInit() {
    this.formInit();
  }

  formInit() {
    this.LoanStatusForm = this.fb.group({
      running: [],
      stop: [],
      type:[undefined]

    });
  }
  changeStatus(event:any): void{
    let form = this.LoanStatusForm.value;
    if(form.type == null || form.type == undefined)
    {
      this.tostr.info('Select Loan Type');
      return;
    }

    if (event.target.id == 'running' && event.target.checked == true) {
      this.status = true;
    } else if (event.target.id == 'stop' && event.target.checked == true) {
      this.status = false;
    }
    
    this.GetVouchers();
  
}

GetVouchers(){

  this.StatusData = [];
  const type = this.LoanStatusForm.get('type').value;
  const obj = {
    type: type,
    status: this.status
  };

  this.apiService
    .getDataById('Payroll/GetLoanStatus', obj)
    .subscribe((data) => {
      
      if (!data || data.length === 0) {
        this.tostr.info("No Records Found");
        return;
      }

      data.forEach((item: any) => {
        let form = item;
        this.StatusData.push(form);
        
      });

    });
}

OnClearType() {
  this.StatusData = [];
  this.LoanStatusForm.get('type').setValue(undefined);
}

updateSentStatus(event: any, item: any) {
  item.Active = event.target.checked;
}


onClickSave() {
  if (this.StatusData.length == 0) {
    this.tostr.warning('Incomplete Transaction...');
    return;
  }


  const voucher: any[] = this.StatusData.map((data) => ({
    EmpyId: data.empy_id,
    Active: data.Active,
    Type: data.Type
  }));

  this.apiService
    .saveData('Payroll/UpdateLoanStatus', voucher)
    .subscribe((result) => {
      if (result == true || result == 'true') {
        this.tostr.success('Save Successfully');
        this.OnClearType();
      } else {
        this.tostr.error('Please Save Again');
      }
    });
}



}
