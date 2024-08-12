import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-provident-loan',
  templateUrl: './provident-loan.component.html',
  styleUrls: ['./provident-loan.component.css']
})
export class ProvidentLoanComponent {

  constructor(
    private tostr: ToastrService,
    private fb: FormBuilder,
    private apiService: ApiService,
    private dp: DatePipe
  ) {
    
  }

  @ViewChild('voucherLists', { static: false }) voucherLists!: ElementRef;

  ProvidentLoanForm!: FormGroup;
  ProvidentLoanList: any = [];
  isShow = false;
  isReadOnly: boolean = true;
  editMode: boolean = true;
  editSno: any = '';
  editModeSno: boolean = false;
  selectedRow: number | null = null;
  btnAdd:string = 'Add';
  EmployeeList : any[] = [];
  voucherList: any[] = [];
  CutProvidentLoanList: any[] = [];
  Level5List: any[] = [];
  Disabled: boolean;
  VchDisable: boolean = false;


  // togglePages() {
  //   this.isShowPage = !this.isShowPage;
  //   if (this.isShowPage) {
  //    this.onClickRefresh();
  //   }
  // }

  ngOnInit() {

    this.formInit();
    this.disableFields();
    this.getEmployeeList();
    this.getLevel5Accounts()
  }

  formInit() {
    this.ProvidentLoanForm = this.fb.group({
      id: [''],
      startDate: [new Date()],
      SrNo: [''],
      empName: [undefined],
      remarks: [''],
      amount: [''],
      mnthlyInstlment: [''],
      tmpStop: [''],
      finEntry: [''],
      level5Accounts: [undefined],
      vchType: [undefined]

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
    this.VchDisable = false;
    this.ProvidentLoanList = [];
    this.disableFields();
  }


  disableFields() {
  this.Disabled = true;
  }



  enableFields() {
    this.Disabled = false;
  }

  
  resetForm() {
    this.ProvidentLoanForm.get('empName')?.patchValue(undefined);
    this.ProvidentLoanForm.get('remarks')?.patchValue('');
    this.ProvidentLoanForm.get('SrNo')?.patchValue('');
    this.ProvidentLoanForm.get('amount')?.patchValue('');
    this.ProvidentLoanForm.get('tmpStop')?.patchValue('');
    this.ProvidentLoanForm.get('mnthlyInstlment')?.patchValue('');
    this.ProvidentLoanForm.get('startDate')?.patchValue(new Date());
    this.ProvidentLoanForm.get('finEntry')?.patchValue('');
    this.ProvidentLoanForm.get('level5Accounts')?.patchValue(undefined);
    this.ProvidentLoanForm.get('SrNo')?.patchValue('');
  }

  editItem(row: any) {


    if(row.sent){
      this.tostr.warning('Voucher Approved!');
          return;
      }
  
    this.btnAdd = 'Update'
    this.editMode = true;
    this.VchDisable = true;
    this.editModeSno = true;
    this.editSno = row.sno;
    this.ProvidentLoanForm.get('remarks')?.patchValue(row.remarks);
    this.ProvidentLoanForm.get('empName')?.patchValue(row.EmpyId);
    this.ProvidentLoanForm.get('amount')?.patchValue(row.amount);
    this.ProvidentLoanForm.get('tmpStop')?.patchValue(row.tmpStop);
    this.ProvidentLoanForm.get('mnthlyInstlment')?.patchValue(row.mnthlyInstlment);
    this.ProvidentLoanForm.get('finEntry')?.patchValue(row.FinEntry);
    this.ProvidentLoanForm.get('level5Accounts')?.patchValue(row.accountCode);
    this.ProvidentLoanForm.get('startDate')?.patchValue(row.startDate);
    this.ProvidentLoanForm.get('SrNo')?.patchValue(row.srno);
  }





    
  onClickSave() {
    const form = this.ProvidentLoanForm.value;

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

    let vchNo = this.editMode ? this.ProvidentLoanForm.get('SrNo')?.value : 0;
  
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


    };
    console.log(dataToSave);
    this.apiService
      .saveData('Payroll/SaveProvidentLoan', dataToSave)
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
    this.ProvidentLoanList = [];
    const obj = {
      empy_id: event.empy_id
    };

    this.apiService
      .getDataById('Payroll/GetEditPLoan', obj)
      .subscribe((data) => {
        
        if (!data || data.length === 0) {
          this.tostr.info("No Records Found");
          this.enableFields();
          return;
        }

        this.enableFields();
        data.forEach((item: any) => {
          
          // this.ProvidentLoanForm.get('id')?.patchValue(item.Id);

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
          form.sent = item.sent
          form.finEntry = item.FinEntry
          form.level5Accounts = item.accountCode

          this.ProvidentLoanList.push(form);
        });
      });
  }


  deleteLoan(srno:any, Vch:any, EmpyId:any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    var empy_id = this.ProvidentLoanForm.get('empName')?.value;
    if (confirmDelete == true) {
      const obj = {
        empy_id: EmpyId,
        SrNo: srno,
        Vch:Vch

      };

      this.apiService.deleteData('Payroll/DelPLoan', obj).subscribe({
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
