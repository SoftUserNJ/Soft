import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-other-deduction',
  templateUrl: './other-deduction.component.html',
  styleUrls: ['./other-deduction.component.css']
})
export class OtherDeductionComponent {


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

  resetFormOnAdd() {

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
    console.log('employe', data)
  })
}


// getEOBIList() {

//   const obj = {
//     Type: "EOBI",
//   };
//   this.apiService.getDataById('EmployeeDeduction/GetEmpDeductionList', obj)
//       .subscribe((data) => { debugger

//         const uniqueEmpyIds = {};

//         this.voucherList = data.filter((employee) => {
//           if (!uniqueEmpyIds[employee.empy_id]) {
//             uniqueEmpyIds[employee.empy_id] = true;
//             return true;
//           }
//           return false;
//         });
//       });
// }


onAdd() { 
  // debugger

  let form = this.EOBIForm.value;

  if (form.empName === null || form.empName === undefined) {
    this.tostr.warning('Select Employee....!');
    return;
  }
  if (form.amount === null || form.amount === undefined) {
    this.tostr.warning('Add other deduction....!');
    return;
  }


  let EmployeeName = this.EmployeeList.find((i) => i.empy_id === form.empName);

  form.empName = EmployeeName.name;
  form.EmpyId = EmployeeName.empy_id;

  if (this.editModeSno) {
    const index = this.EOBIList.findIndex(
      (row) => row.sno === this.editSno
    );
    if (index !== -1) {
      form.sno = this.editSno;
      this.EOBIList[index] = form;
      this.editModeSno = false;
      this.editSno = '';
      this.btnAdd = 'Add'
      this.resetFormOnAdd();
      return;
    }
  }

  form.sno = this.EOBIList.length > 0 && this.EOBIList[this.EOBIList.length - 1].sno !== undefined && this.EOBIList[this.EOBIList.length - 1].sno !== null ? this.EOBIList[this.EOBIList.length - 1].sno + 1 : 1;

  this.EOBIList.push(form);
  this.resetFormOnAdd();
}


editItem(row: any) { 

  if(row.sent){
    this.tostr.warning('Voucher Approved!');
        return;
    }

  this.btnAdd = 'Update'

  this.editModeSno = true;
  this.editSno = row.sno;
  this.EOBIForm.get('SrNo')?.patchValue(row.SrNo);
  this.EOBIForm.get('empName')?.patchValue(row.EmpyId);
  this.EOBIForm.get('Date')?.patchValue(row.Date);
  this.EOBIForm.get('reference')?.patchValue(row.reference);
  this.EOBIForm.get('amount')?.patchValue(row.amount);
  this.EOBIForm.get('remarks')?.patchValue(row.remarks);
  this.EOBIForm.get('isAllow')?.patchValue(row.isAllow);
}

deleteItem(row: any) {

  if(row.sent){
    this.tostr.warning('Voucher Approved!');
        return;
    }
  const confirmDelete = confirm('Are you sure you want to delete this item?');

  if (!confirmDelete) {
    return;
  }

  const indexToRemove = this.EOBIList.findIndex(
    (item) => item.sno === row.sno
  );
  if (indexToRemove !== -1) {
    this.EOBIList.splice(indexToRemove, 1);
  }
}



onClickSave() { 
  // debugger
  if (this.EOBIList.length == 0) {
    this.tostr.warning('Incomplete Transaction...');
    return;
  }

  

  //let Id = this.editMode ? this.IncomeTaxForm.get('id')?.value : 0;

  const voucher: any[] = this.EOBIList.map((data) => ({

    SrNo: data.sno,
    Type: "Other",
    StDate: this.dp.transform(data.Date, 'yyyy-MM-dd'),
    Remarks: data.remarks,
    Reference: data.reference,
    EmpyId: data.EmpyId,
    Amount: data.amount,
    Active: data.isAllow == "" ? false : data.isAllow,
    sent: data.sent

  }));
debugger;
  this.apiService
    .saveData('Payroll/SaveDeductions', voucher)
    .subscribe((result) => {
      if (result == true || result == 'true') {
        this.tostr.success('Save Successfully');
        this.onClickRefresh();

      } else {
        this.tostr.error('Please Save Again');
      }
    });
}

editDeduction(event: any): void {
  debugger;
  this.EOBIList = [];
  const obj = {
    empy_id: event.empy_id,
    Type: "Other"
  };

  this.apiService.getDataById('Payroll/GetEditDeductionList', obj)
    .subscribe((data) => {
      this.enableFields();

      // Check if there are records in the response
      if (data && data.length > 0) {
        data.forEach((item: any) => {
          this.EOBIForm.get('id')?.patchValue(item.Id);

          let form = item;
          form.empName = item.EmpName;
          form.EmpyId = item.empy_id;
          form.Date = this.dp.transform(item.stDate, 'yyyy-MM-dd');
          form.reference = item.Reference;
          form.sno = item.srno;
          form.amount = item.Amount;
          form.remarks = item.Remarks;
          form.isAllow = item.Active;
          form.sent = item.sent;

          this.EOBIList.push(form);
        });
      } else {
        console.log('No record found for selected user');
        this.tostr.info('No record found');
      }
    });
}


deleteDeduction(): void {
  const confirmDelete = confirm('Are you sure you want to delete this Deduction?');

 var  empy_id = this.EOBIForm.get('empName')?.value

 if (empy_id === null || empy_id === undefined) {
  this.tostr.warning('Select Employee to Delete....!');
  return;
}
  if (confirmDelete == true) {
    const obj = {
      empy_id: empy_id,
      Type: "Other"
    };

    this.apiService.deleteData('Payroll/DelEmpDeductions', obj).subscribe({
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
