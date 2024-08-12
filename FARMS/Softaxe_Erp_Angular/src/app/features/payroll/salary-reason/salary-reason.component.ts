import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-salary-reason',
  templateUrl: './salary-reason.component.html',
  styleUrls: ['./salary-reason.component.css']
})
export class SalaryReasonComponent {

  SalaryReasonForm!: FormGroup;
  SalaryReasonList: any = [];
  isShow = false;
  isReadOnly: boolean = true;
  editMode: boolean = true;
  isShowPage: boolean = true;
  editSno: any = '';
  editModeSno: boolean = false;
  selectedRow: number | null = null;
  btnAdd:string = 'Add';
  Disabled:boolean;

  constructor(
    private tostr: ToastrService,
    private fb: FormBuilder,
    private apiService: ApiService,
    private dp: DatePipe
  )
  {

  }

  @ViewChild('voucherLists', { static: false }) voucherLists!: ElementRef;
  voucherList: any[] = [];

  formInit() 
  { 
    this.SalaryReasonForm = this.fb.group({

      salaryReasonId: [''],
      salaryReasonName: [''],

    });
  }

  
  ngOnInit() 
  { 
    this.formInit();
    this.disableFields();
    this.getSalaryReasonList();
  }

  onClickNew() {
    this.isShow = true;
    this.editMode = false;
    this.enableFields();
  }

  disableFields() {
    this.Disabled = true;
  }

  resetForm() {
    this.SalaryReasonForm.get('salaryReasonName')?.patchValue('');
  
  }

  enableFields() {
    this.Disabled = false;
  }


  onClickRefresh() {
    this.isShow = false;
    this.resetForm();
    this.disableFields();
  }

  getSalaryReasonList() {
    this.apiService.getData('Payroll/GetSalaryReasonList').subscribe((data)=>{
      this.voucherList = data;
    })
}

onClickSave() {

  const form = this.SalaryReasonForm.value;

  if (!form.salaryReasonName) {
    this.tostr.warning('Enter Salary Reason');
    return;
  }

  const salaryReasonId = this.editMode ? form.salaryReasonId : 0;

  const dataToSave = {
    Reason: form.salaryReasonName,
    Id: salaryReasonId,
  };

  this.apiService
    .saveData('Payroll/SaveSalaryReason', dataToSave)
    .subscribe((result) => {
      if (result === true || result === 'true') {
        this.tostr.success('Save Successfully');
        this.onClickRefresh();
        this.getSalaryReasonList();
      } else {
        this.tostr.error('Please Save Again');
      }
    });
}

editSalaryReason(row: any) {

  this.isShow = true;

  this.enableFields();
  this.editModeSno = true;
  this.editSno = row.sno;
  this.editMode = true;
  
  this.SalaryReasonForm.get('salaryReasonName')?.patchValue(row.Reason);
  this.SalaryReasonForm.get('salaryReasonId')?.patchValue(row.Id);
 
}

deleteSalaryReason(Id: any): void {
  const confirmDelete = confirm('Are you sure you want to delete this Salary Reason?');

  if (confirmDelete == true) {
    const obj = {
      Id: Id,
    };

    this.apiService.deleteData('Payroll/DelSalaryReason', obj).subscribe({
      next: (data) => {
        if (data == 'true' || data == true) {
          this.tostr.success('Delete Successfully');
          this.getSalaryReasonList();
        } else if (data == 'false' || data == false) {
          this.tostr.error('Delete Again');
        }
      },
      error: (error) => {
        this.tostr.info(error.error.text);
      },
    });
  }
}
}
