import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-bank-entry',
  templateUrl: './bank-entry.component.html',
  styleUrls: ['./bank-entry.component.css']
})
export class BankEntryComponent {

  constructor(
    private tostr: ToastrService,
    private fb: FormBuilder,
    private apiService: ApiService,
    private dp: DatePipe
  )
  {

  }

  BankForm!: FormGroup;
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
    this.getBankList();
  }

  formInit() 
  { 
    this.BankForm = this.fb.group({

      bankId: [''],
      bankName: [''],
      branchCode: [''],
      accNo: [''],
      address: [''],

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
    this.BankForm.get('bankName')?.patchValue('');
    this.BankForm.get('bankId')?.patchValue('');
    this.BankForm.get('branchCode')?.patchValue('');
    this.BankForm.get('accNo')?.patchValue('');
    this.BankForm.get('address')?.patchValue('');
  
  }

  enableFields() {
    this.Disabled = false;
  }

  getBankList() {
    this.apiService.getData('Payroll/GetBankList').subscribe((data)=>{
      this.voucherList = data;
    })
}

onClickRefresh() {
  this.isShow = false;
  this.resetForm();
  this.disableFields();
}


onClickSave() {

  const form = this.BankForm.value;

  if (!form.bankName) {
    this.tostr.warning('Enter Bank Name');
    return;
  }

  if (!form.branchCode) {
    this.tostr.warning('Enter Branch Code');
    return;
  }

  const bankId = this.editMode ? form.bankId : 0;

  const dataToSave = {
    Bank: form.bankName,
    Id: bankId,
    BranchCode: form.branchCode,
    Address: form.address,
    AccNo: form.accNo
  };

  this.apiService
    .saveData('Payroll/SaveBank', dataToSave)
    .subscribe((result) => {
      if (result === true || result === 'true') {
        this.tostr.success('Save Successfully');
        this.onClickRefresh();
        this.getBankList();
      } else {
        this.tostr.error('Please Save Again');
      }
    });
}



editBank(row: any) {

  this.isShow = true;

  this.enableFields();
  this.editModeSno = true;
  this.editSno = row.sno;
  this.editMode = true;
  
  this.BankForm.get('bankName')?.patchValue(row.bankName);
  this.BankForm.get('bankId')?.patchValue(row.bankId);
  this.BankForm.get('branchCode')?.patchValue(row.branchCode);
  this.BankForm.get('accNo')?.patchValue(row.accNo);
  this.BankForm.get('address')?.patchValue(row.address);
 
}

deleteBank(bankId: any): void {
  const confirmDelete = confirm('Are you sure you want to delete this Bank?');

  if (confirmDelete == true) {
    const obj = {
      Id: bankId,
    };

    this.apiService.deleteData('Payroll/DelBank', obj).subscribe({
      next: (data) => {
        if (data == 'true' || data == true) {
          this.tostr.success('Delete Successfully');
          this.getBankList();
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
