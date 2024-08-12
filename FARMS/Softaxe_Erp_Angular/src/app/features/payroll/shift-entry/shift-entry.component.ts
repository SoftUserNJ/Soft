import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-shift-entry',
  templateUrl: './shift-entry.component.html',
  styleUrls: ['./shift-entry.component.css']
})
export class ShiftEntryComponent {


  ShiftForm!: FormGroup;
  ShiftList: any = [];
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
    this.ShiftForm = this.fb.group({

      shiftId: [''],
      shiftName: [''],

    });
  }

  ngOnInit() 
  { 
    this.formInit();
    this.disableFields();
    this.getShiftList();
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
      this.ShiftForm.get('shiftName')?.patchValue('');
    
    }
  
    enableFields() {
      this.Disabled = false;
    }


    onClickRefresh() {
      this.isShow = false;
      this.resetForm();
      this.disableFields();
    }

    getShiftList() {
      this.apiService.getData('Payroll/GetShiftList').subscribe((data)=>{
        this.voucherList = data;
      })
  }

  onClickSave() {
  
    const form = this.ShiftForm.value;
  
    if (!form.shiftName) {
      this.tostr.warning('Enter Shift Name');
      return;
    }
  
    const shiftId = this.editMode ? form.shiftId : 0;
  
    const dataToSave = {
      Shift: form.shiftName,
      Id: shiftId,
    };
  
    this.apiService
      .saveData('Payroll/SaveShift', dataToSave)
      .subscribe((result) => {
        if (result === true || result === 'true') {
          this.tostr.success('Save Successfully');
          this.onClickRefresh();
          this.getShiftList();
        } else {
          this.tostr.error('Please Save Again');
        }
      });
  }

  editShift(row: any) {

    this.isShow = true;

    this.enableFields();
    this.editModeSno = true;
    this.editSno = row.sno;
    this.editMode = true;
    
    this.ShiftForm.get('shiftName')?.patchValue(row.Shift);
    this.ShiftForm.get('shiftId')?.patchValue(row.Id);
   
  }
  
  deleteShift(Id: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this Shift?');

    if (confirmDelete == true) {
      const obj = {
        Id: Id,
      };

      this.apiService.deleteData('Payroll/DelShift', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.tostr.success('Delete Successfully');
            this.getShiftList();
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
