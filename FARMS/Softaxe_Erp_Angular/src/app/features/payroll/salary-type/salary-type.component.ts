import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-salary-type',
  templateUrl: './salary-type.component.html',
  styleUrls: ['./salary-type.component.css']
})
export class SalaryTypeComponent {

  SalaryTypeForm!: FormGroup;
  SalaryTypeList: any = [];
  isShow = false;
  isReadOnly: boolean = true;
  editMode: boolean = true;
  isShowPage: boolean = true;
  editSno: any = '';
  editModeSno: boolean = false;
  selectedRow: number | null = null;
  btnAdd:string = 'Add';

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
    this.SalaryTypeForm = this.fb.group({

      salaryTypeId: [''],
      salaryTypeName: [''],

    });
  }

  
  ngOnInit() 
  { 
    this.formInit();
    this.disableFields();
    this.getSalaryTypeList();
  }

  onClickNew() { debugger
    this.isShow = true;
    this.editMode = false;
    this.enableFields();
  }

  disableFields() {
    this.SalaryTypeForm.get('salaryTypeName').disable();
  }

  resetForm() {
    this.SalaryTypeForm.get('salaryTypeName')?.patchValue('');
  
  }

  enableFields() {
    this.SalaryTypeForm.get('salaryTypeName').enable();
  }


  onClickRefresh() {
    this.isShow = false;
    this.resetForm();
    this.disableFields();
  }

  getSalaryTypeList() {
    this.apiService.getData('Payroll/GetSalaryTypeList').subscribe((data)=>{
      this.voucherList = data;
    })
}

onClickSave() { 
  debugger;

  const form = this.SalaryTypeForm.value;

  if (!form.salaryTypeName) {
    this.tostr.warning('Enter Salary Type');
    return;
  }

  const salaryTypeId = this.editMode ? form.salaryTypeId : 0;

  const dataToSave = {
    SalaryType: form.salaryTypeName,
    Id: salaryTypeId,
  };

  this.apiService
    .saveData('Payroll/SaveSalaryType', dataToSave)
    .subscribe((result) => {
      if (result === true || result === 'true') {
        this.tostr.success('Save Successfully');
        this.onClickRefresh();
        this.getSalaryTypeList();
      } else {
        this.tostr.error('Please Save Again');
      }
    });
}

editSalaryType(row: any) { debugger

  this.isShow = true;

  this.enableFields();
  this.editModeSno = true;
  this.editSno = row.sno;
  this.editMode = true;
  
  this.SalaryTypeForm.get('salaryTypeName')?.patchValue(row.SalaryType);
  this.SalaryTypeForm.get('salaryTypeId')?.patchValue(row.Id);
 
}

deleteSalaryType(Id: any): void { debugger
  const confirmDelete = confirm('Are you sure you want to delete this Salary Type?');

  if (confirmDelete == true) {
    const obj = {
      Id: Id,
    };

    this.apiService.deleteData('Payroll/DelSalaryType', obj).subscribe({
      next: (data) => {
        if (data == 'true' || data == true) {
          this.tostr.success('Delete Successfully');
          this.getSalaryTypeList();
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
