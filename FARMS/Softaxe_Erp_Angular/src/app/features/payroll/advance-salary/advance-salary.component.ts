import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-advance-salary',
  templateUrl: './advance-salary.component.html',
  styleUrls: ['./advance-salary.component.css']
})
export class AdvanceSalaryComponent {

  constructor(
    private tostr: ToastrService,
    private fb: FormBuilder,
    private apiService: ApiService,
    private dp: DatePipe
  ) {
    
  }

  @ViewChild('voucherLists', { static: false }) voucherLists!: ElementRef;

  AdvSalaryForm!: FormGroup;
  AdvSalaryList: any = [];
  isShow = false;
  isReadOnly: boolean = true;
  editMode: boolean = true;
  editSno: any = '';
  editModeSno: boolean = false;
  selectedRow: number | null = null;
  btnAdd:string = 'Add';
  EmployeeList : any[] = [];
  Level5List : any[] = [];
  voucherList: any[] = [];
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
  }

  formInit() {
    this.AdvSalaryForm = this.fb.group({
      id: [''],
      startDate: [new Date()],
      SrNo: [''],
      empName: [undefined],
      remarks: [''],
      amount: [''],
      level5Accounts: [undefined],
      reference: [''],
      vchType:[undefined],
      finEntry:[''],

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
    this.getLevel5Accounts();
  }

  onClickRefresh() {
    this.isShow = false;
    this.VchDisable = false;
    this.resetForm();
    this.AdvSalaryList = [];
    this.disableFields();
  }


  disableFields() {
    this.Disabled = true;
  }



  enableFields() {
    this.Disabled = false;
  }

  
  resetForm() {
    this.AdvSalaryForm.get('empName')?.patchValue(undefined);
    this.AdvSalaryForm.get('remarks')?.patchValue('');
    this.AdvSalaryForm.get('SrNo')?.patchValue('');
    this.AdvSalaryForm.get('amount')?.patchValue('');
    this.AdvSalaryForm.get('reference')?.patchValue('');
    this.AdvSalaryForm.get('code')?.patchValue('');
    this.AdvSalaryForm.get('vchType')?.patchValue(undefined);
    this.AdvSalaryForm.get('startDate')?.patchValue(new Date());
  }

  resetFormOnAdd() {

    this.AdvSalaryForm.get('remarks')?.patchValue('');
    this.AdvSalaryForm.get('SrNo')?.patchValue('');
    this.AdvSalaryForm.get('amount')?.patchValue('');
    this.AdvSalaryForm.get('reference')?.patchValue('');
    this.AdvSalaryForm.get('code')?.patchValue('');
    this.AdvSalaryForm.get('startDate')?.patchValue(new Date());
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
    this.editSno = row.sno;
    this.AdvSalaryForm.get('remarks')?.patchValue(row.remarks);
    this.AdvSalaryForm.get('empName')?.patchValue(row.EmpyId);
    this.AdvSalaryForm.get('amount')?.patchValue(row.amount);
    this.AdvSalaryForm.get('reference')?.patchValue(row.reference);
    this.AdvSalaryForm.get('code')?.patchValue(row.code);
    this.AdvSalaryForm.get('vchType')?.patchValue(row.vchType);
    this.AdvSalaryForm.get('startDate')?.patchValue(row.startDate);
    this.AdvSalaryForm.get('SrNo')?.patchValue(row.srno);
    this.AdvSalaryForm.get('finEntry')?.patchValue(row.FinEntry);
    this.AdvSalaryForm.get('level5Accounts')?.patchValue(row.accountCode);
  }

  
  onClickSave() {

    const form = this.AdvSalaryForm.value;

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

    let vchNo = this.editMode ? this.AdvSalaryForm.get('SrNo')?.value : 0;
  
    const dataToSave = {
      SrNo: vchNo,
      Vch: form.vchType,
      Stdate: this.dp.transform(form.startDate, 'yyyy-MM-dd'),
      remarks: form.remarks,
      EmpyId: form.empName,
      AdvanceSalary: form.amount,
      Reference: form.reference,
      sent: form.sent,
      AccountCode: form.level5Accounts,
      FinEntry:  finEntry
    };
    this.apiService
      .saveData('Payroll/SaveAdvanceSalary', dataToSave)
      .subscribe((result) => {
        if (result === true || result === 'true') {
          this.tostr.success('Save Successfully');
          this.onClickRefresh();
        } else {
          this.tostr.error('Please Save Again');
        }
      });
  }

  // getStaffLoanList() {

  //   const obj = {
  //     Type: "Staff Loan"
  //   }
  //   this.apiService.getDataById('EmployeeDeduction/GetLoanList', obj)
  //     .subscribe((data) => { debugger

  //       const uniqueEmpyIds = {};

  //       this.voucherList = data.filter((employee) => {
  //         if (!uniqueEmpyIds[employee.empy_id]) {
  //           uniqueEmpyIds[employee.empy_id] = true;
  //           return true;
  //         }
  //         return false;
  //       });
  //     });
  // }

  editAdvSalary(event: any): void {
    // this.onClickRefresh();
    // this.isShow = true;
    // this.editMode = true;
    this.AdvSalaryList = [];
    const obj = {
      empy_id: event.empy_id,

    };

    this.apiService
      .getDataById('Payroll/GetEditAdvSalaryList', obj)
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
          form.empName = item.EmpName;
          form.EmpyId = item.empy_id;
          form.startDate = this.dp.transform(item.stDate, 'yyyy-MM-dd');
          form.amount = item.AdvanceSalary;
          form.sno = item.srno;
          form.reference = item.Reference,
          form.remarks = item.Remarks;
          form.Level5List = item.accountCode;
          form.finEntry = item.FinEntry;
          form.sent = item.sent;

          this.AdvSalaryList.push(form);
        });
      });
  }

  deleteAdvSalary(srno: any, EmpyId: any, vchType: any, sent:any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');
    if(sent){
      this.tostr.warning('Voucher Approved!');
          return;
      }
      
    if (confirmDelete == true) {
      const obj = {
        SrNo: srno,
        empy_id : EmpyId,
        Vch: vchType,
      };
      console.log(obj)
      this.apiService.deleteData('Payroll/DelAdvSalary', obj).subscribe({
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
