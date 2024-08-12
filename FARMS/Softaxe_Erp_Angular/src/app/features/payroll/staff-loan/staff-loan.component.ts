import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-staff-loan',
  templateUrl: './staff-loan.component.html',
  styleUrls: ['./staff-loan.component.css']
})
export class StaffLoanComponent {

  constructor(
    private tostr: ToastrService,
    private fb: FormBuilder,
    private apiService: ApiService,
    private dp: DatePipe
  ) {
    
  }

  @ViewChild('voucherLists', { static: false }) voucherLists!: ElementRef;

  StaffLoanForm!: FormGroup;
  StaffLoanList: any = [];
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
  Disabled:boolean;
  VchDisable:boolean = false;


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
    this.getLevel5Accounts();
  }

  formInit() {
    this.StaffLoanForm = this.fb.group({
      id: [''],
      startDate: [new Date()],
      SrNo: [''],
      empName: [undefined],
      remarks: [''],
      amount: [''],
      mnthlyInstlment: [''],
      balnce: [''],
      tmpStop: [''],
      vchType:[undefined],
      level5Accounts: [undefined],
      finEntry: ['']

    });
  }

  getLevel5Accounts() {
    this.apiService.getData('Payroll/getLevel5Accounts').subscribe((data)=>{
      this.Level5List = data;
    })
  }

  getEmployeeList() {
    this.apiService.getData('Payroll/GetEmployees').subscribe((data)=>{
      this.EmployeeList = data;
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
    this.StaffLoanList = [];
    this.disableFields();
  }


  disableFields() {
    this.Disabled = true;
  }



  enableFields() {
    this.Disabled = false;
  }

  
  resetForm() {
    this.StaffLoanForm.get('empName')?.patchValue(undefined);
    this.StaffLoanForm.get('remarks')?.patchValue('');
    this.StaffLoanForm.get('SrNo')?.patchValue('');
    this.StaffLoanForm.get('amount')?.patchValue('');
    this.StaffLoanForm.get('balnce')?.patchValue('');
    this.StaffLoanForm.get('tmpStop')?.patchValue('');
    this.StaffLoanForm.get('mnthlyInstlment')?.patchValue('');
    this.StaffLoanForm.get('vchType')?.patchValue(undefined);
    this.StaffLoanForm.get('finEntry')?.patchValue('');
    this.StaffLoanForm.get('level5Accounts')?.patchValue(undefined);
    this.StaffLoanForm.get('startDate')?.patchValue(new Date());
  }

  editItem(row: any) {


    if(row.sent){
      this.tostr.warning('Voucher Approved!');
          return;
      }
  
    this.btnAdd = 'Update'
    this.editMode = true;
    this.editModeSno = true;
    this.VchDisable = true;
    this.StaffLoanForm.get('remarks')?.patchValue(row.remarks);
    this.StaffLoanForm.get('empName')?.patchValue(row.EmpyId);
    this.StaffLoanForm.get('amount')?.patchValue(row.amount);
    this.StaffLoanForm.get('balnce')?.patchValue(row.balnce);
    this.StaffLoanForm.get('tmpStop')?.patchValue(row.tmpStop);
    this.StaffLoanForm.get('mnthlyInstlment')?.patchValue(row.mnthlyInstlment);
    this.StaffLoanForm.get('vchType')?.patchValue(row.vchType);
    this.StaffLoanForm.get('startDate')?.patchValue(row.startDate);
    this.StaffLoanForm.get('SrNo')?.patchValue(row.srno);
    this.StaffLoanForm.get('finEntry')?.patchValue(row.FinEntry);
    this.StaffLoanForm.get('level5Accounts')?.patchValue(row.accountCode);
  }


  onClickSave() {

    const form = this.StaffLoanForm.value;

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

    let vchNo = this.editMode ? this.StaffLoanForm.get('SrNo')?.value : 0;
  
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
      FinEntry:  finEntry
    };
    this.apiService
      .saveData('Payroll/SaveStaffLoan', dataToSave)
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
    this.StaffLoanList = [];
    const obj = {
      empy_id: event.empy_id,
    };

    this.apiService
      .getDataById('Payroll/GetEditLoan', obj)
      .subscribe((data) => {
        
        if (!data || data.length === 0) {
          this.tostr.info("No Records Found");
          this.enableFields();
          return;
        }

        this.enableFields();
        data.forEach((item: any) => {
          
          // this.StaffLoanForm.get('id')?.patchValue(item.Id);

          let form = item;
          form.vchType = item.Vch;
          form.empName = item.name;
          form.EmpyId = item.empy_id;
          form.startDate = this.dp.transform(item.stdate, 'yyyy-MM-dd');
          form.amount = item.loanamt;
          form.mnthlyInstlment = item.instamt;
          form.sno = item.srno;
          form.Nom = item.noofmnth;
          form.finEntry = item.FinEntry;
          form.Level5List = item.accountCode;
          form.remarks = item.remarks;
          form.tmpStop = item.Active;
          form.sent = item.sent

          this.StaffLoanList.push(form);
        });
      });
  }


  deleteLoan(srno:any, Vch:any, EmpyId:any, sent:any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if(sent){
      this.tostr.warning('Voucher Approved!');
          return;
      }

    if (confirmDelete == true) {
      const obj = {
        empy_id: EmpyId,
        Vch: Vch,
        SrNo: srno
      };

      this.apiService.deleteData('Payroll/DelStaffLoan', obj).subscribe({
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
