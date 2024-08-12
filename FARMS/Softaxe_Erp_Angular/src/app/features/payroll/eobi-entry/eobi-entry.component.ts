import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-eobi-entry',
  templateUrl: './eobi-entry.component.html',
  styleUrls: ['./eobi-entry.component.css']
})
export class EobiEntryComponent {


  
  constructor(
    private tostr: ToastrService,
    private fb: FormBuilder,
    private apiService: ApiService,
    private dp: DatePipe
  ) {
    
  }

  @ViewChild('voucherLists', { static: false }) voucherLists!: ElementRef;
  voucherList: any[] = [];

  EOBIForm!: FormGroup;
  EOBIList: any = [];
  isShow = false;
  isReadOnly: boolean = true;
  editMode: boolean = true;
  editSno: any = '';
  editModeSno: boolean = false;
  selectedRow: number | null = null;
  btnAdd:string = 'Add';
  EmployeeList : any[] = [];
  Disabled:boolean;


  ngOnInit() {

    this.formInit();
    this.disableFields();
    this.getEmployeeList();
  }

  formInit() {
    this.EOBIForm = this.fb.group({
      SrNo: [''],
      empName: [undefined],
      remarks: [''],
      Date: [new Date()],
      reference: [''],
      amount: [''],
      isAllow: [''],

    });
  }

  // togglePages() {
  //   this.isShowPage = !this.isShowPage;
  //   if (this.isShowPage) {
  //    this.onClickRefresh();
  //   }
  // }

  
  onClickRefresh() {
    this.isShow = false;
    this.resetForm();
    this.EOBIList = [];
    this.disableFields();
    this.btnAdd = 'Add';

  }

  disableFields() {

    this.Disabled = true;
  }



  enableFields() {
    this.Disabled = false;
  }

  
  resetForm() {
    this.EOBIForm.get('empName')?.patchValue(undefined);
    this.EOBIForm.get('remarks')?.patchValue('');
    this.EOBIForm.get('SrNo')?.patchValue('');
    this.EOBIForm.get('reference')?.patchValue('');
    this.EOBIForm.get('isAllow')?.patchValue('');
    this.EOBIForm.get('amount')?.patchValue('');
    this.EOBIForm.get('Date')?.patchValue(new Date());
  }

onClickNew() {
  this.isShow = true;
  this.editMode = false;
  this.enableFields();
}


getEmployeeList() {
  this.apiService.getData('Payroll/GetEmployees').subscribe((data)=>{
    this.EmployeeList = data;
  })
}



editItem(row: any) {

  if(row.sent){
    this.tostr.warning('Voucher Approved!');
        return;
    }

  this.btnAdd = 'Update'
  this.editMode = true;
  this.editModeSno = true;
  this.editSno = row.sno;
  this.EOBIForm.get('SrNo')?.patchValue(row.SrNo);
  this.EOBIForm.get('empName')?.patchValue(row.EmpyId);
  this.EOBIForm.get('Date')?.patchValue(row.Date);
  this.EOBIForm.get('reference')?.patchValue(row.reference);
  this.EOBIForm.get('amount')?.patchValue(row.amount);
  this.EOBIForm.get('remarks')?.patchValue(row.remarks);
  this.EOBIForm.get('isAllow')?.patchValue(row.isAllow);
  this.EOBIForm.get('SrNo')?.patchValue(row.srno);
}



onClickSave() {

  const form = this.EOBIForm.value;

  if (!form.amount) {
    this.tostr.warning('Enter Loan Amount');
    return;
  }

  if(form.empName == null || form.empName == undefined){
    this.tostr.warning('Select Employee');
    return;
  }

  let vchNo = this.editMode ? this.EOBIForm.get('SrNo')?.value : 0;

  const dataToSave = {
    Srno: vchNo,
    StDate: this.dp.transform(form.Date, 'yyyy-MM-dd'),
    Remarks: form.remarks,
    Reference: form.reference,
    EmpyId: form.empName,
    EobiDeducation: form.amount,
    Active: form.isAllow == "" ? false : form.isAllow,
    sent: form.sent
  };





  this.apiService
    .saveData('Payroll/SaveEOBI', dataToSave)
    .subscribe((result) => {
      if (result === true || result === 'true') {
        this.tostr.success('Save Successfully');
        this.onClickRefresh();
      } else {
        this.tostr.error('Please Save Again');
      }
    });
}

editDeduction(event: any): void {

  this.EOBIList = [];
  const obj = {
    empy_id: event.empy_id
  };

  this.apiService
    .getDataById('Payroll/GetEditEOBITax', obj)
    .subscribe((data) => {
      
      if (!data || data.length === 0) {
        this.tostr.info("No Records Found");
        this.enableFields();
        return;
      }

      this.enableFields();
      data.forEach((item: any) => {
        
        this.EOBIForm.get('id')?.patchValue(item.Id);

        let form = item;
        form.empName = item.EmpName;
        form.EmpyId = item.empy_id;
        form.Date = this.dp.transform(item.stDate, 'yyyy-MM-dd');
        form.reference = item.Reference;
        form.SrNo = item.srno;
        form.amount = item.EobiDeducation;
        form.remarks = item.Remarks;
        form.isAllow = item.Active;
        form.sent = item.sent;
        
        this.EOBIList.push(form);
      });
    });
}


DelEOBITax(EmpyId:any, srno:any): void {
  const confirmDelete = confirm('Are you sure you want to delete this Deduction?');


  if (confirmDelete == true) {
    const obj = {
      empy_id: EmpyId,
      SrNo: srno
    };

    this.apiService.deleteData('Payroll/DelEOBITax', obj).subscribe({
      next: (data) => {
        if (data == 'true' || data == true) {
          this.tostr.success('Delete Successfully');
          this.onClickRefresh();
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
