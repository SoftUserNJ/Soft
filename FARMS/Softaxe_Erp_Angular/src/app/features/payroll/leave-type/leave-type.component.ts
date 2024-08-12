import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-leave-type',
  templateUrl: './leave-type.component.html',
  styleUrls: ['./leave-type.component.css']
})
export class LeaveTypeComponent {

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
    this.getHrSetupList();
    this.disableFields();
  }

  HrSetupForm!: FormGroup;
  isShow = false;
  isReadOnly: boolean = true;
  editMode: boolean = true;
  editSno: any = '';
  editModeSno: boolean = false;
  selectedRow: number | null = null;
  btnAdd:string = 'Add';
  Disabled:boolean;

  formInit() 
  { 
    this.HrSetupForm = this.fb.group({

      HrSetupNo: [''],
      name: [''],
      amount: [''],
      type: [undefined],
      category: [undefined]

    });
      
      
  }
  

  
  resetForm() {
    this.HrSetupForm.get('type')?.patchValue(undefined);
    this.HrSetupForm.get('category')?.patchValue(undefined);
    this.HrSetupForm.get('name')?.patchValue('');
    this.HrSetupForm.get('amount')?.patchValue('');
    this.HrSetupForm.get('HrSetupNo')?.patchValue('');
  }

  onClickNew() {
    this.isShow = true;
    this.editMode = false;
    this.enableFields();
  }

  onClickRefresh() {
    this.isShow = false;
    this.resetForm();
    this.disableFields();
  }


  disableFields() {
    this.Disabled = true;
  }

  enableFields() {
    this.Disabled = false;
  }




  onlyNumeric(event: any): void {
    const inputVal = event.target.value;
    // Replace non-numeric characters using a regular expression
    const cleanValue = inputVal.replace(/[^0-9]/g, '');
    // Update the form control value with the cleaned numeric value
    this.HrSetupForm.patchValue({ quantity: cleanValue }, { emitEvent: false });
  }


  rowHighLight(event: any) {
    // Get the clicked row
    const clickedRow = event.target.closest('tr');
  
    // Add 'HighLightRow' class to all td elements in the clicked row
    const tds = clickedRow.querySelectorAll('td');
    tds.forEach(td => {
      td.classList.add('HighLightRow');
    });
  
    // Remove 'HighLightRow' class from other rows
    const allRows = document.querySelectorAll('tr');
    allRows.forEach(row => {
      if (row !== clickedRow) {
        const otherRowTds = row.querySelectorAll('td');
        otherRowTds.forEach(td => {
          td.classList.remove('HighLightRow');
        });
      }
    });
  }


  onClickSave() {

    const form = this.HrSetupForm.value;
  
    if (!form.name) {
      this.tostr.warning('Enter Leave Name');
      return;
    }

    if (!form.amount) {
      this.tostr.warning('Enter Leave Amount');
      return;
    }

    if(form.type == null || form.type == undefined){
      this.tostr.warning('Select Leave Type');
      return;
    }

    if(form.category == null || form.category == undefined){
      this.tostr.warning('Select Leave Category');
      return;
    }

    let vchNo = this.editMode ? this.HrSetupForm.get('HrSetupNo')?.value : 0;
  
    const dataToSave = {
      HrSetupId: vchNo,  
      name: form.name,
      type: form.type,
      category: form.category,
      amount: form.amount,
    };
  
    this.apiService
      .saveData('Payroll/SaveHrSetup', dataToSave)
      .subscribe((result) => {
        if (result === true || result === 'true') {
          this.tostr.success('Save Successfully');
          this.onClickRefresh();
          this.getHrSetupList();
        } else {
          this.tostr.error('Please Save Again');
        }
      });
  }
  

  getHrSetupList() {
      this.apiService.getData('Payroll/GetHrSetupList').subscribe((data)=>{
        this.voucherList = data;
      })
  }

  deleteSetup(HrSetupId: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {
      const obj = {
        vchNo: HrSetupId,
      };

      this.apiService.deleteData('Payroll/DelHRSetup', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.tostr.success('Delete Successfully');
            this.getHrSetupList();
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



  editSetup(row: any) {

    this.isShow = true;
  
    this.enableFields();
    this.editModeSno = true;
    this.editSno = row.sno;
    this.editMode = true;
    
    this.HrSetupForm.get('name')?.patchValue(row.Name);
    this.HrSetupForm.get('HrSetupNo')?.patchValue(row.HrSetupId);
    this.HrSetupForm.get('category')?.patchValue(row.Category);
    this.HrSetupForm.get('amount')?.patchValue(row.Amount);
    this.HrSetupForm.get('type')?.patchValue(row.Type);
   
  }

  searchGrid(event: any): void {
    const tableElement = this.voucherLists.nativeElement;
    const rows = tableElement.querySelectorAll('tr');
  
    rows.forEach((row: HTMLTableRowElement) => {
      let isShow = true;
  
      if (
        row.textContent &&
        row.textContent
          .toLowerCase()
          .indexOf(event.target.value.toLowerCase()) > -1
      ) {
        isShow = true;
      } else {
        isShow = false;
      }
  
      if (isShow) {
        row.style.display = '';
      } else {
        row.style.display = 'none';
      }
    });
  }
}
