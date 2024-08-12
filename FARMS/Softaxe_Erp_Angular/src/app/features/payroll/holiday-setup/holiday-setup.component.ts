import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-holiday-setup',
  templateUrl: './holiday-setup.component.html',
  styleUrls: ['./holiday-setup.component.css']
})
export class HolidaySetupComponent {

  constructor(
    private tostr: ToastrService,
    private fb: FormBuilder,
    private apiService: ApiService,
    private dp: DatePipe
  )
  {

  }

  HolidaySetupForm!: FormGroup;
  HolidaySetupList: any = [];
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
    this.getHolidayList();
  }


  formInit() 
  { 
    this.HolidaySetupForm = this.fb.group({

      fromDate: [new Date()],
      toDate: [new Date()],
      description: [''],
      Id: [''],

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
    this.HolidaySetupForm.get('fromDate').setValue(new Date());
    this.HolidaySetupForm.get('toDate').setValue(new Date());
    this.HolidaySetupForm.get('description').setValue('');
  
  }

  enableFields() { 
    this.Disabled = false;
  }

  onClickRefresh() {
    this.isShow = false;
    this.resetForm();
    this.disableFields();
  }

  
  getHolidayList() {
    this.apiService.getData('Payroll/GetHolidayList').subscribe((data)=>{
      this.voucherList = data;
    })
}

  onClickSave() {
  
    const form = this.HolidaySetupForm.value;

    if (form.fromDate == null || form.fromDate == 0) {
      this.tostr.warning('Enter From Date....!');
      return;
    }

    if (form.toDate == null || form.toDate == 0) {
      this.tostr.warning('Enter To Date....!');
      return;
    }
  
    if (!form.description) {
      this.tostr.warning('Enter Description....!');
      return;
    }
  
    const Id = this.editMode ? form.Id : 0;
  
    const dataToSave = {
      Holiday: form.description,
      fromDate: this.dp.transform(  
        this.HolidaySetupForm.get('fromDate')?.value,
        'yyyy-MM-dd'
      ),
      toDate: this.dp.transform(  
        this.HolidaySetupForm.get('toDate')?.value,
        'yyyy-MM-dd'
      ),
      Id: Id,
    };
  
    this.apiService
      .saveData('Payroll/SaveHoliday', dataToSave)
      .subscribe((result) => {
        if (result === true || result === 'true') {
          this.tostr.success('Save Successfully');
          this.onClickRefresh();
          this.getHolidayList();
        } else {
          this.tostr.error('Please Save Again');
        }
      });
  }

  editHoliday(row: any) { debugger

    this.isShow = true;
  
    this.enableFields();
    this.editModeSno = true;
    this.editSno = row.sno;
    this.editMode = true;
    
    this.HolidaySetupForm.get('description')?.patchValue(row.Holiday);
    this.HolidaySetupForm.get('Id')?.patchValue(row.Id);
    this.HolidaySetupForm.get('toDate')?.patchValue(row.To_Date);
    this.HolidaySetupForm.get('fromDate')?.patchValue(row.From_Date);
   
  }


  

  deleteHoliday(Id: any): void { debugger
    const confirmDelete = confirm('Are you sure you want to delete this Holiday?');
  
    if (confirmDelete == true) {
      const obj = {
        Id: Id,
      };
  
      this.apiService.deleteData('Payroll/DelHoliday', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.tostr.success('Delete Successfully');
            this.getHolidayList();
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
