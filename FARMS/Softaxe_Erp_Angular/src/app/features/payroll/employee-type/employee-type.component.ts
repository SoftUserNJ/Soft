import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-employee-type',
  templateUrl: './employee-type.component.html',
  styleUrls: ['./employee-type.component.css']
})
export class EmployeeTypeComponent {

  
  EmpTypeForm!: FormGroup;
  EmpTypeList: any = [];
  isShow = false;
  isReadOnly: boolean = true;
  editMode: boolean = true;
  isShowPage: boolean = true;
  editSno: any = '';
  editModeSno: boolean = false;
  selectedRow: number | null = null;
  btnAdd:string = 'Add';
  Disabled: boolean;

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
    this.EmpTypeForm = this.fb.group({

      empTypeId: [''],
      empTypeName: [''],

    });
  }

  
  ngOnInit() 
  { 
    this.formInit();
    this.disableFields();
    this.getEmpTypeList();
  }

  onClickNew() { debugger
    this.isShow = true;
    this.editMode = false;
    this.enableFields();
  }

  disableFields() {
    this.Disabled = true;
  }

  resetForm() {
    this.EmpTypeForm.get('empTypeName')?.patchValue('');
  
  }

  enableFields() {
    this.Disabled = false;
  }


  onClickRefresh() {
    this.isShow = false;
    this.resetForm();
    this.disableFields();
  }

  getEmpTypeList() {
    this.apiService.getData('Payroll/GetEmpTypeList').subscribe((data)=>{
      this.voucherList = data;
    })
}

onClickSave() { 
  debugger;

  const form = this.EmpTypeForm.value;

  if (!form.empTypeName) {
    this.tostr.warning('Enter Employee Type');
    return;
  }

  const empTypeId = this.editMode ? form.empTypeId : 0;

  const dataToSave = {
    EmployeeType: form.empTypeName,
    Id: empTypeId,
  };

  this.apiService
    .saveData('Payroll/SaveEmployeeType', dataToSave)
    .subscribe((result) => {
      if (result === true || result === 'true') {
        this.tostr.success('Save Successfully');
        this.onClickRefresh();
        this.getEmpTypeList();
      } else {
        this.tostr.error('Please Save Again');
      }
    });
}

editEmployeeType(row: any) { debugger

  this.isShow = true;

  this.enableFields();
  this.editModeSno = true;
  this.editSno = row.sno;
  this.editMode = true;
  
  this.EmpTypeForm.get('empTypeName')?.patchValue(row.EmployeeType);
  this.EmpTypeForm.get('empTypeId')?.patchValue(row.Id);
 
}

deleteEmployeeType(Id: any): void { debugger
  const confirmDelete = confirm('Are you sure you want to delete this Employee Type?');

  if (confirmDelete == true) {
    const obj = {
      Id: Id,
    };

    this.apiService.deleteData('Payroll/DelEmployeeType', obj).subscribe({
      next: (data) => {
        if (data == 'true' || data == true) {
          this.tostr.success('Delete Successfully');
          this.getEmpTypeList();
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
