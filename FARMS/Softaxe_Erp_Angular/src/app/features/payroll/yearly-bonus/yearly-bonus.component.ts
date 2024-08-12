import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-yearly-bonus',
  templateUrl: './yearly-bonus.component.html',
  styleUrls: ['./yearly-bonus.component.css']
})
export class YearlyBonusComponent {


  constructor(
    private tostr: ToastrService,
    private fb: FormBuilder,
    private apiService: ApiService,
    private dp: DatePipe
  ) {
    
  }

  @ViewChild('voucherLists', { static: false }) voucherLists!: ElementRef;
  voucherList: any[] = [];

  YearlyBonusForm!: FormGroup;
  YearlyBonusList: any = [];
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
    this.YearlyBonusForm = this.fb.group({
      SrNo: [''],
      empName: [undefined],
      remarks: [''],
      Date: [new Date()],
      reference: [''],
      GrossSalary: [''],
      percentage: [''],
      LeaveEnchment: [''],
      Leaves: [''],
      Paid: [''],
      Bonus: [''],
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
    this.YearlyBonusList = [];
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
   this.YearlyBonusForm.get('SrNo').patchValue('');
    this.YearlyBonusForm.get('empName').patchValue(undefined);
    this.YearlyBonusForm.get('remarks').patchValue('');
    this.YearlyBonusForm.get('Date').patchValue(new Date());
    this.YearlyBonusForm.get('reference').patchValue('');
    this.YearlyBonusForm.get('GrossSalary').patchValue('');
    this.YearlyBonusForm.get('percentage').patchValue('');
    this.YearlyBonusForm.get('LeaveEnchment').patchValue('');
    this.YearlyBonusForm.get('Leaves').patchValue('');
    this.YearlyBonusForm.get('Paid').patchValue('');
    this.YearlyBonusForm.get('ITax').patchValue('');
    this.YearlyBonusForm.get('EOBI').patchValue('');
    this.YearlyBonusForm.get('Loan').patchValue('');
    this.YearlyBonusForm.get('VLoan').patchValue('');
    this.YearlyBonusForm.get('PF').patchValue('');
    this.YearlyBonusForm.get('Bonus').patchValue('');
    this.YearlyBonusForm.get('NetBonus').patchValue('');
  }

  resetFormOnAdd() {
    this.YearlyBonusForm.get('SrNo').patchValue('');
    this.YearlyBonusForm.get('remarks').patchValue('');
    this.YearlyBonusForm.get('Date').patchValue(new Date());
    this.YearlyBonusForm.get('reference').patchValue('');
    this.YearlyBonusForm.get('GrossSalary').patchValue('');
    this.YearlyBonusForm.get('percentage').patchValue('');
    this.YearlyBonusForm.get('LeaveEnchment').patchValue('');
    this.YearlyBonusForm.get('Leaves').patchValue('');
    this.YearlyBonusForm.get('Paid').patchValue('');
    this.YearlyBonusForm.get('ITax').patchValue('');
    this.YearlyBonusForm.get('EOBI').patchValue('');
    this.YearlyBonusForm.get('Loan').patchValue('');
    this.YearlyBonusForm.get('VLoan').patchValue('');
    this.YearlyBonusForm.get('PF').patchValue('');
    this.YearlyBonusForm.get('Bonus').patchValue('');
    this.YearlyBonusForm.get('NetBonus').patchValue('');
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
  this.YearlyBonusForm.get('SrNo')?.patchValue(row.srno);
  this.YearlyBonusForm.get('empName')?.patchValue(row.EmpyId);
  this.YearlyBonusForm.get('Date')?.patchValue(row.Date);
  this.YearlyBonusForm.get('reference')?.patchValue(row.reference);
  this.YearlyBonusForm.get('NetBonus')?.patchValue(row.NetBonus);
  this.YearlyBonusForm.get('remarks')?.patchValue(row.remarks);
  this.YearlyBonusForm.get('GrossSalary')?.patchValue(row.GrossSalary);
  this.YearlyBonusForm.get('percentage')?.patchValue(row.percentage);
  this.YearlyBonusForm.get('Bonus')?.patchValue(row.Bonus);
  this.YearlyBonusForm.get('ITax')?.patchValue(row.ITax);
  this.YearlyBonusForm.get('EOBI')?.patchValue(row.EOBI);
  this.YearlyBonusForm.get('Loan')?.patchValue(row.Loan);
  this.YearlyBonusForm.get('VLoan')?.patchValue(row.VLoan);
  this.YearlyBonusForm.get('PF')?.patchValue(row.PF);
  this.YearlyBonusForm.get('PF')?.patchValue(row.PF);

}

onClickSave() {

  const form = this.YearlyBonusForm.value;

  if(form.empName == null || form.empName == undefined){
    this.tostr.warning('Select Employee');
    return;
  }

  let vchNo = this.editMode ? this.YearlyBonusForm.get('SrNo')?.value : 0;

  const dataToSave = {
 
    SrNo: vchNo,
    StDate: this.dp.transform(form.Date,'yyyy-MM-dd'),
    Remarks: form.remarks,
    Reference: form.reference,
    EmpyId: form.empName,
    YearlyBonus: form.NetBonus
  };

  this.apiService
    .saveData('Payroll/SaveYearlyBonus', dataToSave)
    .subscribe((result) => {
      if (result === true || result === 'true') {
        this.tostr.success('Save Successfully');
        this.onClickRefresh();
      } else {
        this.tostr.error('Please Save Again');
      }
    });
}

editBonus(event: any): void {
  this.YearlyBonusList = [];
  const obj = {
    empy_id: event.empy_id
  };

  this.apiService.getDataById('Payroll/GetEditBonusList', obj)
    .subscribe((data) => {
      this.enableFields();

      if (data && data.length > 0) {
        data.forEach((item: any) => {

          let form = item;
          form.empName = item.EmpName;
          form.EmpyId = item.empy_id;
          form.Date = this.dp.transform(item.stDate, 'yyyy-MM-dd');
          form.reference = item.Reference;
          form.SrNo = item.srno;
          form.NetBonus = item.YearlyBonus;
          form.remarks = item.Remarks;

          this.YearlyBonusList.push(form);
        });
      } else {
        this.tostr.info('No record found');
      }
    });
}


deleteBonus(srno:any, EmpyId:any): void {
  const confirmDelete = confirm('Are you sure you want to delete this Deduction?');

  if (confirmDelete == true) {
    const obj = {
      empy_id: EmpyId,
      SrNo: srno
    };

    this.apiService.deleteData('Payroll/DelYearlyBonus', obj).subscribe({
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
