import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-salary-days',
  templateUrl: './salary-days.component.html',
  styleUrls: ['./salary-days.component.css']
})
export class SalaryDaysComponent {
  constructor(
    private tostr: ToastrService,
    private fb: FormBuilder,
    private apiService: ApiService,
    private dp: DatePipe
  )
  {

  }


  SalaryDaysForm!: FormGroup;
  isShow = false;
  isReadOnly: boolean = true;
  editMode: boolean = true;
  isShowPage: boolean = true;
  editSno: any = '';
  editModeSno: boolean = false;
  selectedRow: number | null = null;
  btnAdd:string = 'Add';
  YearList:number[] = [];
  Disabled: boolean;



  formInit() 
  { 
    this.SalaryDaysForm = this.fb.group({

      SalaryDays:[''],
      srno: [''],

    });
  }

  ngOnInit() 
  { 
    this.formInit();
    this.disableFields();
    this.getSalaryDays();
  }

  disableFields() {
    this.Disabled = true;
  }

  enableFields() {
    this.Disabled = false;
  }

  onClickRefresh() {
    this.isShow = false;
    this.resetForm();
    this.disableFields();
  }

  resetForm() {
    this.SalaryDaysForm.get('srno')?.patchValue('');
    this.SalaryDaysForm.get('SalaryDays')?.patchValue('');
  
  }

    onClickNew() {
      this.isShow = true;
      this.enableFields();
    }

    onClickSave() {
    
      const form = this.SalaryDaysForm.value;
    

      if (form.SalaryDays === 0 || form.SalaryDays === undefined || form.SalaryDays == null) {
        this.tostr.warning('Enter Salary Days....!');
        return;
      }

      if(form.srno == 0 || form.srno == undefined || form.srno == null) {
        form.srno= 1
      }

    
      const dataToSave = {
        srno: form.srno,
        SalaryDays: form.SalaryDays
      };
    
      this.apiService
        .saveData('Payroll/SaveSalaryDays', dataToSave)
        .subscribe((result) => {
          if (result === true || result === 'true') {
            this.tostr.success('Save Successfully');
            this.onClickRefresh();
            this.getSalaryDays();
          } else {
            this.tostr.error('Please Save Again');
          }
        });
    }

    getSalaryDays() {
      this.apiService.getData('Payroll/GetSalaryDays').subscribe((data)=>{
        this.SalaryDaysForm.get('srno').setValue(data[0].srno);
        this.SalaryDaysForm.get('SalaryDays').setValue(data[0].SalaryDays);
      });
  }
  
}
