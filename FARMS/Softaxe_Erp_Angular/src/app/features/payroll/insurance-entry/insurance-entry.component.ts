import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-insurance-entry',
  templateUrl: './insurance-entry.component.html',
  styleUrls: ['./insurance-entry.component.css']
})
export class InsuranceEntryComponent {

  constructor(
    private tostr: ToastrService,
    private fb: FormBuilder,
    private apiService: ApiService,
    private dp: DatePipe
  ) {
    
  }

  @ViewChild('voucherLists', { static: false }) voucherLists!: ElementRef;

  InsrnceLoanForm!: FormGroup;
  InsrnceLoanList: any = [];
  isShow = false;
  isReadOnly: boolean = true;
  editMode: boolean = true;
  editSno: any = '';
  editModeSno: boolean = false;
  selectedRow: number | null = null;
  btnAdd:string = 'Add';
  EmployeeList : any[] = [];
  voucherList: any[] = [];
  Level5List: any[] = [];
  CutInsrnceLoanList: any[] = [];
  Disabled: boolean;
  VchDisable: boolean = false;



  ngOnInit() {

    this.formInit();
    this.disableFields();
    this.getEmployeeList();
    this.getLevel5Accounts();
  }

  formInit() {
    this.InsrnceLoanForm = this.fb.group({
      id: [''],
      startDate: [new Date()],
      SrNo: [''],
      empName: [undefined],
      remarks: [''],
      amount: [''],
      mnthlyInstlment: [''],
      tmpStop: [''],
      engineNo: [''],
      vehNo: [''],
      chasisNo: [''],
      finEntry: [''],
      level5Accounts: [undefined],
      vchType: [undefined],

    });
  }

  getEmployeeList() {
    this.apiService.getData('Payroll/GetEmployees').subscribe((data)=>{
      this.EmployeeList = data;
    })
  }

  getLevel5Accounts() {
    this.apiService.getData('Payroll/getLevel5Accounts').subscribe((data)=>{
      this.Level5List = data;
    })
  }

  onClickNew() {
    this.isShow = true;
    this.editMode = false;
    this.enableFields();
  }

  onClickRefresh() {
    this.isShow = false;
    this.resetForm();
    this.InsrnceLoanList = [];
    this.VchDisable = false;
    this.disableFields();
  }


  disableFields() {
    this.Disabled = true;
  }



  enableFields() {
    this.Disabled = false;
  }

  
  resetForm() {
    this.InsrnceLoanForm.get('empName')?.patchValue(undefined);
    this.InsrnceLoanForm.get('remarks')?.patchValue('');
    this.InsrnceLoanForm.get('SrNo')?.patchValue('');
    this.InsrnceLoanForm.get('amount')?.patchValue('');
    this.InsrnceLoanForm.get('balnce')?.patchValue('');
    this.InsrnceLoanForm.get('tmpStop')?.patchValue('');
    this.InsrnceLoanForm.get('chasisNo')?.patchValue('');
    this.InsrnceLoanForm.get('OpBlnce')?.patchValue('');
    this.InsrnceLoanForm.get('vehNo')?.patchValue('');
    this.InsrnceLoanForm.get('finEntry')?.patchValue('');
    this.InsrnceLoanForm.get('level5Accounts')?.patchValue(undefined);
    this.InsrnceLoanForm.get('engineNo')?.patchValue('');
    this.InsrnceLoanForm.get('mnthlyInstlment')?.patchValue('');
    this.InsrnceLoanForm.get('startDate')?.patchValue(new Date());
  }

  editItem(row: any) {


    if(row.sent){
      this.tostr.warning('Voucher Approved!');
          return;
      }
  
    this.btnAdd = 'Update'

    this.editModeSno = true;
    this.editSno = row.sno;
    this.VchDisable = true;
    this.editMode = true;
    this.InsrnceLoanForm.get('remarks')?.patchValue(row.remarks);
    this.InsrnceLoanForm.get('empName')?.patchValue(row.EmpyId);
    this.InsrnceLoanForm.get('amount')?.patchValue(row.amount);
    this.InsrnceLoanForm.get('balnce')?.patchValue(row.balnce);
    this.InsrnceLoanForm.get('tmpStop')?.patchValue(row.tmpStop);
    this.InsrnceLoanForm.get('mnthlyInstlment')?.patchValue(row.mnthlyInstlment);
    this.InsrnceLoanForm.get('startDate')?.patchValue(row.startDate);
    this.InsrnceLoanForm.get('engineNo')?.patchValue(row.engineNo);
    this.InsrnceLoanForm.get('vehNo')?.patchValue(row.vehNo);
    this.InsrnceLoanForm.get('vchType')?.patchValue(row.Vch);
    this.InsrnceLoanForm.get('OpBlnce')?.patchValue(row.OpBlnce);
    this.InsrnceLoanForm.get('SrNo')?.patchValue(row.srno);
    this.InsrnceLoanForm.get('chasisNo')?.patchValue(row.chasisNo);
    this.InsrnceLoanForm.get('finEntry')?.patchValue(row.FinEntry);
    this.InsrnceLoanForm.get('level5Accounts')?.patchValue(row.accountCode);
  }

  
  onClickSave() {
    const form = this.InsrnceLoanForm.value;

   const finEntry = form.finEntry == "" ? false : form.finEntry

    if(finEntry == true && form.level5Accounts  == null){
      this.tostr.warning('Please Select Account');
      return;
    }

    if (!form.amount) {
      this.tostr.warning('Enter Loan Amount');
      return;
    }

    if(form.vchType == null || form.vchType == undefined){
      this.tostr.warning('Select Vch Type');
      return;
    }

    if(form.empName == null || form.empName == undefined){
      this.tostr.warning('Select Employee');
      return;
    }

   let Nom  = (form.amount/form.mnthlyInstlment);
    if (Nom % 1 > 0) {
      Nom = Math.round(Nom);
    }

    let vchNo = this.editMode ? this.InsrnceLoanForm.get('SrNo')?.value : 0;
  
    const dataToSave = {
      Srno: vchNo,
      Vch: form.vchType,
      Stdate: this.dp.transform(form.startDate, 'yyyy-MM-dd' ),
      Remarks: form.remarks,
      Noofmnth: Nom,
      EmpyId: form.empName,
      Loanamt: form.amount,
      Instamt: form.mnthlyInstlment,
      Active: form.tmpStop == "" ? false : form.tmpStop,
      sent: form.sent,
      AccountCode: form.level5Accounts,
      FinEntry:  finEntry,
      Vehicleno: form.vehNo,
      Chasisno: form.chasisNo,
      Engineno: form.engineNo



    };
    this.apiService
      .saveData('Payroll/SaveInsrnceLoan', dataToSave)
      .subscribe((result) => {
        if (result === true || result === 'true') {
          this.tostr.success('Save Successfully');
          this.onClickRefresh();
        } else {
          this.tostr.error('Please Save Again');
        }
      });
  }


  editLoan(event: any): void {
    // this.onClickRefresh();
    // this.isShow = true;
    // this.editMode = true;
    this.InsrnceLoanList = [];
    const obj = {
      empy_id: event.empy_id
    };

    this.apiService
      .getDataById('Payroll/GetEditInsrnceLoan', obj)
      .subscribe((data) => {
        
        if (!data || data.length === 0) {
          this.tostr.info("No Records Found");
          this.enableFields();
          return;
        }

        this.enableFields();
        data.forEach((item: any) => {
          
          // this.InsrnceLoanForm.get('id')?.patchValue(item.Id);

          let form = item;
          form.empName = item.name;
          form.EmpyId = item.empy_id;
          form.startDate = this.dp.transform(item.stdate, 'yyyy-MM-dd');
          form.amount = item.loanamt;
          form.sno = item.srno;
          form.Nom = item.noofmnth;
          form.mnthlyInstlment = item.instamt;
          form.remarks = item.remarks;
          form.tmpStop = item.Active;
          form.sent = item.sent;
          form.vchType = item.Vch;
          form.finEntry = item.FinEntry;
          form.level5Accounts = item.accountCode;
          form.vehNo = item.Vehicleno;
          form.OpBlnce = item.opening;
          form.engineNo = item.Engineno;
          form.chasisNo = item.Chasisno;

          this.InsrnceLoanList.push(form);
        });
      });
  }


  deleteLoan(srno:any, Vch: any,EmpyId: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');


    if (confirmDelete == true) {
      const obj = {
        empy_id: EmpyId,
        SrNo: srno,
        Vch: Vch
      };

      this.apiService.deleteData('Payroll/DelInsrnceLoan', obj).subscribe({
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
