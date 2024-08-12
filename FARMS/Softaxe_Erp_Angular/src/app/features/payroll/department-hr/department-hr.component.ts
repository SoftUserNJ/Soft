import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-department-hr',
  templateUrl: './department-hr.component.html',
  styleUrls: ['./department-hr.component.css']
})
export class DepartmentHRComponent {

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

  ngOnInit() 
  { 
    this.formInit();
    this.disableFields();
    this.getDepartmentList();
  }

  DepartmentForm!: FormGroup;
  DepartmentList: any = [];
  isShow = false;
  isReadOnly: boolean = true;
  editMode: boolean = true;
  isShowPage: boolean = true;
  editSno: any = '';
  editModeSno: boolean = false;
  selectedRow: number | null = null;
  btnAdd:string = 'Add';
  Disabled: boolean;

  formInit() 
  { 
    this.DepartmentForm = this.fb.group({

      deptId: [''],
      deptName: [''],

    });
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
      this.DepartmentForm.get('deptName')?.patchValue('');
    
    }
  
    enableFields() {
      this.Disabled = false;
    }

      onClickSave() {
      
        const form = this.DepartmentForm.value;
      
        if (!form.deptName) {
          this.tostr.warning('Enter Department Name');
          return;
        }
      
        const deptId = this.editMode ? form.deptId : 0;
      
        const dataToSave = {
          Department: form.deptName,
          Id: deptId,
        };
      
        this.apiService
          .saveData('Payroll/SaveDepartment', dataToSave)
          .subscribe((result) => {
            if (result === true || result === 'true') {
              this.tostr.success('Save Successfully');
              this.onClickRefresh();
              this.getDepartmentList();
            } else {
              this.tostr.info('Please Save Again');
            }
          });
      }
    
    onClickRefresh() {
      this.isShow = false;
      this.resetForm();
      this.disableFields();
    }


    getDepartmentList() {
      this.apiService.getData('Payroll/GetDepartmentList').subscribe((data)=>{
        this.voucherList = data;
      })
  }

  editDepartment(row: any) {

    this.isShow = true;

    this.enableFields();
    this.editModeSno = true;
    this.editSno = row.sno;
    this.editMode = true;
    
    this.DepartmentForm.get('deptName')?.patchValue(row.Department);
    this.DepartmentForm.get('deptId')?.patchValue(row.Id);
   
  }
  
  deleteDepartment(Id: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this Department?');

    if (confirmDelete == true) {
      const obj = {
        Id: Id,
      };

      this.apiService.deleteData('Payroll/DelDepartment', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.tostr.success('Delete Successfully');
            this.getDepartmentList();
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
