import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-provident-fund-deduction',
  templateUrl: './provident-fund-deduction.component.html',
  styleUrls: ['./provident-fund-deduction.component.css']
})
export class ProvidentFundDeductionComponent {
  constructor(
    private tostr: ToastrService,
    private fb: FormBuilder,
    private apiService: ApiService,
    private dp: DatePipe
  ) {
    
  }

  @ViewChild('voucherLists', { static: false }) voucherLists!: ElementRef;
  voucherList: any[] = [];

  PFForm!: FormGroup;
  PFList: any = [];
  isShow = false;
  isReadOnly: boolean = true;
  editMode: boolean = true;
  editSno: any = '';
  editModeSno: boolean = false;
  selectedRow: number | null = null;
  btnAdd:string = 'Add';
  EmployeeList : any[] = [];
  Disabled: boolean;



  ngOnInit() {

    this.formInit();
    this.disableFields();
    this.getEmployeeList();
  }

  formInit() {
    this.PFForm = this.fb.group({
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
    this.PFList = [];
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
    this.PFForm.get('empName')?.patchValue(undefined);
    this.PFForm.get('remarks')?.patchValue('');
    this.PFForm.get('SrNo')?.patchValue('');
    this.PFForm.get('reference')?.patchValue('');
    this.PFForm.get('isAllow')?.patchValue('');
    this.PFForm.get('amount')?.patchValue('');
    this.PFForm.get('Date')?.patchValue(new Date());
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
  this.PFForm.get('SrNo')?.patchValue(row.SrNo);
  this.PFForm.get('empName')?.patchValue(row.EmpyId);
  this.PFForm.get('Date')?.patchValue(row.Date);
  this.PFForm.get('reference')?.patchValue(row.reference);
  this.PFForm.get('amount')?.patchValue(row.amount);
  this.PFForm.get('remarks')?.patchValue(row.remarks);
  this.PFForm.get('isAllow')?.patchValue(row.isAllow);
  this.PFForm.get('SrNo')?.patchValue(row.srno);
}




onClickSave() {

  const form = this.PFForm.value;

  if (!form.amount) {
    this.tostr.warning('Enter Loan Amount');
    return;
  }

  if(form.empName == null || form.empName == undefined){
    this.tostr.warning('Select Employee');
    return;
  }

  let vchNo = this.editMode ? this.PFForm.get('SrNo')?.value : 0;

  const dataToSave = {
    Srno: vchNo,
    StDate: this.dp.transform(form.Date, 'yyyy-MM-dd'),
    Remarks: form.remarks,
    Reference: form.reference,
    EmpyId: form.empName,
    PfundDeducation: form.amount,
    Active: form.isAllow == "" ? false : form.isAllow,
    sent: form.sent
  };





  this.apiService
    .saveData('Payroll/SavePfDeduction', dataToSave)
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
  this.PFList = [];
  const obj = {
    empy_id: event.empy_id
  };

  this.apiService
    .getDataById('Payroll/GetEditPfDeductionList', obj)
    .subscribe((data) => {

      if (!data || data.length === 0) {
        this.tostr.info("No Records Found");
        this.enableFields();
        return;
      }
      
      
      //this.togglePages();
      this.enableFields();
      data.forEach((item: any) => {
        
        this.PFForm.get('id')?.patchValue(item.Id);

        let form = item;
        form.empName = item.EmpName;
        form.EmpyId = item.empy_id;
        form.Date = this.dp.transform(item.stDate, 'yyyy-MM-dd');
        form.reference = item.Reference;
        form.SrNo = item.srno;
        form.amount = item.PFundDeducation;
        form.remarks = item.Remarks;
        form.isAllow = item.Active;
        form.sent = item.sent;
       
        
        this.PFList.push(form);
      });
    });
}

deleteDeduction(EmpyId:any, srno:any): void {
  const confirmDelete = confirm('Are you sure you want to delete this Deduction?');

    const obj = {
      empy_id: EmpyId,
      SrNo: srno
    };

    this.apiService.deleteData('Payroll/DelPfDeductions', obj).subscribe({
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

}
