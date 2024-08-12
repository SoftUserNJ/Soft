import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-leave-enchasement',
  templateUrl: './leave-enchasement.component.html',
  styleUrls: ['./leave-enchasement.component.css']
})
export class LeaveEnchasementComponent {

  constructor(
    private tostr: ToastrService,
    private fb: FormBuilder,
    private apiService: ApiService,
    private dp: DatePipe,
    private auth: AuthService
  ) {
    
  }

  @ViewChild('voucherLists', { static: false }) voucherLists!: ElementRef;
  voucherList: any[] = [];

  LeaveEnchsmntForm!: FormGroup;
  LeaveEnchsmntList: any = [];
  isShow = false;
  isReadOnly: boolean = true;
  editMode: boolean = true;
  editSno: any = '';
  editModeSno: boolean = false;
  selectedRow: number | null = null;
  btnAdd:string = 'Add';
  EmployeeList : any[] = [];
  YearList : any[] = [];
  Disabled:boolean;



  ngOnInit() {

    this.formInit();
    this.disableFields();
    this.getEmployeeList();
    this.getYearMonth();
  }

  formInit() {
    this.LeaveEnchsmntForm = this.fb.group({
      SrNo: [''],
      empName: [undefined],
      year: [undefined],
      remarks: [''],
      Date: [new Date()],
      reference: [''],
      GrossSalary: [''],
      percentage: [''],
      LeaveEnchment: [''],
      Leaves: [''],
      Paid: [''],
      Balance: [''],
      ITax: [''],
      EOBI: [''],
      Loan: [''],
      VLoan: [''],
      PF: [''],
      NetBonus: ['']

    });
  }


  
  onClickRefresh() {
    this.isShow = false;
    this.resetForm();
    this.LeaveEnchsmntList = [];
    this.disableFields();
    this.btnAdd = 'Add';

  }

  disableFields() {
  this.Disabled = true;
  }

  
  getYearMonth() {
    this.apiService.getData('Payroll/GetMonthYear').subscribe((data) => {
      this.YearList = data;
    });
  }



  enableFields() {
    this.Disabled = false;
  }

  
  resetForm() {
    this.LeaveEnchsmntForm.get('empName')?.patchValue(undefined);
    this.LeaveEnchsmntForm.get('remarks')?.patchValue('');
    this.LeaveEnchsmntForm.get('SrNo')?.patchValue('');
    this.LeaveEnchsmntForm.get('reference')?.patchValue('');
    this.LeaveEnchsmntForm.get('Date')?.patchValue(new Date());
    this.LeaveEnchsmntForm.get('Date')?.patchValue(new Date());
    this.LeaveEnchsmntForm.get('GrossSalary').patchValue('');
    this.LeaveEnchsmntForm.get('percentage').patchValue('');
    this.LeaveEnchsmntForm.get('LeaveEnchment').patchValue('');
    this.LeaveEnchsmntForm.get('Leaves').patchValue('');
    this.LeaveEnchsmntForm.get('Paid').patchValue('');
    this.LeaveEnchsmntForm.get('Balance').patchValue('');
    this.LeaveEnchsmntForm.get('ITax').patchValue('');
    this.LeaveEnchsmntForm.get('EOBI').patchValue('');
    this.LeaveEnchsmntForm.get('Loan').patchValue('');
    this.LeaveEnchsmntForm.get('VLoan').patchValue('');
    this.LeaveEnchsmntForm.get('PF').patchValue('');
    this.LeaveEnchsmntForm.get('NetBonus').patchValue('');
  }

  resetFormOnAdd() {
    this.LeaveEnchsmntForm.get('remarks')?.patchValue('');
    this.LeaveEnchsmntForm.get('SrNo')?.patchValue('');
    this.LeaveEnchsmntForm.get('reference')?.patchValue('');
    this.LeaveEnchsmntForm.get('Date')?.patchValue(new Date());
    this.LeaveEnchsmntForm.get('GrossSalary').patchValue('');
    this.LeaveEnchsmntForm.get('percentage').patchValue('');
    this.LeaveEnchsmntForm.get('LeaveEnchment').patchValue('');
    this.LeaveEnchsmntForm.get('Leaves').patchValue('');
    this.LeaveEnchsmntForm.get('Paid').patchValue('');
    this.LeaveEnchsmntForm.get('Balance').patchValue('');
    this.LeaveEnchsmntForm.get('ITax').patchValue('');
    this.LeaveEnchsmntForm.get('EOBI').patchValue('');
    this.LeaveEnchsmntForm.get('Loan').patchValue('');
    this.LeaveEnchsmntForm.get('VLoan').patchValue('');
    this.LeaveEnchsmntForm.get('PF').patchValue('');
    this.LeaveEnchsmntForm.get('NetBonus').patchValue('');
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
  this.btnAdd = 'Update'

  this.editModeSno = true;
  this.editMode = true;

  this.LeaveEnchsmntForm.get('empName')?.patchValue(row.EmpyId);
  this.LeaveEnchsmntForm.get('remarks')?.patchValue(row.remarks);
  this.LeaveEnchsmntForm.get('SrNo')?.patchValue(row.srno);
  this.LeaveEnchsmntForm.get('reference')?.patchValue(row.reference);
  this.LeaveEnchsmntForm.get('Date')?.patchValue(row.Date);
  this.LeaveEnchsmntForm.get('GrossSalary').patchValue(row.GrossSalary);
  this.LeaveEnchsmntForm.get('percentage').patchValue(row.percentage);
  this.LeaveEnchsmntForm.get('LeaveEnchment').patchValue(row.LeaveEnchment);
  this.LeaveEnchsmntForm.get('Leaves').patchValue(row.Leaves);
  this.LeaveEnchsmntForm.get('Paid').patchValue(row.Paid);
  this.LeaveEnchsmntForm.get('Balance').patchValue(row.Balance);
  this.LeaveEnchsmntForm.get('ITax').patchValue(row.ITax);
  this.LeaveEnchsmntForm.get('EOBI').patchValue(row.EOBI);
  this.LeaveEnchsmntForm.get('Loan').patchValue(row.Loan);
  this.LeaveEnchsmntForm.get('VLoan').patchValue(row.VLoan);
  this.LeaveEnchsmntForm.get('PF').patchValue(row.PF);
  this.LeaveEnchsmntForm.get('NetBonus').patchValue(row.NetBonus);
}

onClickSave() {

  const form = this.LeaveEnchsmntForm.value;

  if(form.empName == null || form.empName == undefined){
    this.tostr.warning('Select Employee');
    return;
  }

  let vchNo = this.editMode ? this.LeaveEnchsmntForm.get('SrNo')?.value : 0;

  const dataToSave = {
    Srno: vchNo,
    Stdate: this.dp.transform(form.Date, 'yyyy-MM-dd'),
    Remarks: form.remarks,
    Reference: form.reference,
    EmpyId: form.empName,
    Grosssalary:form.GrossSalary,
    Percentage: form.percentage,
    Lv:form.Leaves,
    Lvpaid:form.Paid,
    Lvbalance:form.Balance,
    Bamount:form.NetBonus,
    Itax: form.ITax,
    Eobi: form.EOBI,
    Pf: form.PF,
    Loan: form.Loan,
    Vloan: form.VLoan,
    Bonus: form.NetBonus,

  };

  this.apiService
    .saveData('Payroll/SaveLeaveEnchasment', dataToSave)
    .subscribe((result) => {
      if (result === true || result === 'true') {
        this.tostr.success('Save Successfully');
        this.onClickRefresh();
      } else {
        this.tostr.error('Please Save Again');
      }
    });
}


GetEditLeaveEnchasment(event: any): void {
  this.LeaveEnchsmntList = [];
  const obj = {
    empy_id: event.empy_id
  };

  this.apiService.getDataById('Payroll/GetEditLeaveEnchasment', obj)
    .subscribe((data) => {
      this.enableFields();
      if (data && data.length > 0) {
        data.forEach((item: any) => {

          let form = item;

          form.empName = item.EmpName;
          form.EmpyId = item.empy_id;
          form.Date = this.dp.transform(item.stdate, 'yyyy-MM-dd');
          form.reference = item.Reference;
          form.SrNo = item.srno;
          form.GrossSalary = item.GROSSSALARY;
          form.percentage = item.Percentage;
          form.Leaves = item.LV;
          form.Paid = item.LVPAID;
          form.Balance = item.LVBALANCE;
          form.NetBonus = item.BAMOUNT;
          form.Bonus = item.BAMOUNT;
          form.ITax = item.ITAX;
          form.EOBI = item.EOBI;
          form.PF = item.PF;
          form.Loan = item.Loan;
          form.VLoan = item.vloan;
          form.remarks = item.Remarks;

          this.LeaveEnchsmntList.push(form);
        });
      } else {
        this.tostr.info('No record founds');
      }
    });
}


deleteDeduction(srno:any, EmpyId:any): void {
  const confirmDelete = confirm('Are you sure you want to delete this Deduction?');

  if (confirmDelete == true) {
    const obj = {
      empy_id: EmpyId,
      SrNo: srno
    };

    this.apiService.deleteData('Payroll/deleteLeaveEnchasment', obj).subscribe({
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

}
