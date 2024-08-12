import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-designation-entry',
  templateUrl: './designation-entry.component.html',
  styleUrls: ['./designation-entry.component.css']
})
export class DesignationEntryComponent {

  constructor(
    private tostr: ToastrService,
    private fb: FormBuilder,
    private apiService: ApiService,
    private dp: DatePipe
  )
  {

  }

  DesignationForm!: FormGroup;
  DesignationList: any = [];
  isShow = false;
  isReadOnly: boolean = true;
  editMode: boolean = true;
  isShowPage: boolean = true;
  editSno: any = '';
  editModeSno: boolean = false;
  selectedRow: number | null = null;
  btnAdd:string = 'Add';
  Disabled:boolean;

  
  @ViewChild('voucherLists', { static: false }) voucherLists!: ElementRef;
  voucherList: any[] = [];

  ngOnInit() 
  { 
    this.formInit();
    this.disableFields();
    this.getDesignationList();
  }

  formInit() 
  { 
    this.DesignationForm = this.fb.group({

      desgnId: [''],
      desgnName: [''],

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
    this.DesignationForm.get('desgnName')?.patchValue('');
    this.DesignationForm.get('desgnId')?.patchValue('');
  
  }

  enableFields() {
    this.Disabled = false;
  }

  getDesignationList() {
    this.apiService.getData('Payroll/GetDesignationList').subscribe((data)=>{
      this.voucherList = data;
    })
}

onClickRefresh() {
  this.isShow = false;
  this.resetForm();
  this.disableFields();
}


onClickSave() {

  const form = this.DesignationForm.value;

  if (!form.desgnName) {
    this.tostr.warning('Enter Designation Name');
    return;
  }

  const desgnId = this.editMode ? form.desgnId : 0;

  const dataToSave = {
    Designation: form.desgnName,
    Id: desgnId,
  };

  this.apiService
    .saveData('Payroll/SaveDesignation', dataToSave)
    .subscribe((result) => {
      if (result === true || result === 'true') {
        this.tostr.success('Save Successfully');
        this.onClickRefresh();
        this.getDesignationList();
      } else {
        this.tostr.info('Please Save Again');
      }
    });
}



editDesignation(row: any) {

  this.isShow = true;

  this.enableFields();
  this.editModeSno = true;
  this.editSno = row.sno;
  this.editMode = true;
  
  this.DesignationForm.get('desgnName')?.patchValue(row.Designation);
  this.DesignationForm.get('desgnId')?.patchValue(row.Id);
 
}

deleteDesignation(Id: any): void {
  const confirmDelete = confirm('Are you sure you want to delete this Designation?');

  if (confirmDelete == true) {
    const obj = {
      Id: Id,
    };

    this.apiService.deleteData('Payroll/DelDesignation', obj).subscribe({
      next: (data) => {
        if (data == 'true' || data == true) {
          this.tostr.success('Delete Successfully');
          this.getDesignationList();
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
