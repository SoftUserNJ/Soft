import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-salary-payable',
  templateUrl: './salary-payable.component.html',
  styleUrls: ['./salary-payable.component.css']
})
export class SalaryPayableComponent {

  
  constructor(
    private tostr: ToastrService,
    private fb: FormBuilder,
    private apiService: ApiService,
    private dp: DatePipe
  ) {
    
  }

  @ViewChild('voucherLists', { static: false }) voucherLists!: ElementRef;
  voucherList: any[] = [];

  SalaryPayableForm!: FormGroup;
  SalaryPayableList: any = [];
  isShow = false;
  isReadOnly: boolean = true;
  editMode: boolean = true;
  editSno: any = '';
  editModeSno: boolean = false;
  selectedRow: number | null = null;
  btnAdd:string = 'Add';
  EmployeeList : any[] = [];
  Disabled:boolean;

  months = [
    { id: 1, name: 'January' },
    { id: 2, name: 'February' },
    { id: 3, name: 'March' },
    { id: 4, name: 'April' },
    { id: 5, name: 'May' },
    { id: 6, name: 'June' },
    { id: 7, name: 'July' },
    { id: 8, name: 'August' },
    { id: 9, name: 'September' },
    { id: 10, name: 'October' },
    { id: 11, name: 'November' },
    { id: 12, name: 'December' },
  ];
  DepartmentList:any[] = [];
  LocationList: any[]=[];
  DesignationList: any[]=[];
  Level5List: any[]=[];

  ngOnInit() {

    this.formInit();
    this.getMonthYear();
    this.disableFields();
    this.getEmployeeList();
    this.getDesignationList();
    this.getLocation();
    this.getDepartmentList() ;
    this.getLevel5Accounts();
  
  }

  formInit() {
    this.SalaryPayableForm = this.fb.group({
      SrNo: [''],
      empName: [undefined],
      Designation: [undefined],
      Department: [undefined],
      Location: [undefined],
      isAllow: [''],
      Date: [new Date()],
      month: [undefined],
      apprvAll : [''],
      level5Accounts: [undefined],
      trDate: [new Date()]

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
    this.SalaryPayableList = [];
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
    this.SalaryPayableForm.get('empName')?.patchValue(undefined);
    this.SalaryPayableForm.get('remarks')?.patchValue('');
    this.SalaryPayableForm.get('SrNo')?.patchValue('');
    this.SalaryPayableForm.get('reference')?.patchValue('');
    this.SalaryPayableForm.get('isAllow')?.patchValue('');
    this.SalaryPayableForm.get('amount')?.patchValue('');
    this.SalaryPayableForm.get('Date')?.patchValue(new Date());
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

getDepartmentList() {

  this.apiService.getData('PayRoll/GetDepartmentList').subscribe((data)=>{
    this.DepartmentList = data;
  })
}


getLocation() {
this.apiService.getData('PayRoll/GetMainLocation').subscribe((data) => {
  this.LocationList = data;
});
}


getDesignationList() {
this.apiService.getData('PayRoll/GetDesignationList').subscribe((data)=>{
  this.DesignationList = data;
})
}

getLevel5Accounts() {
  this.apiService.getData('Payroll/getLevel5Accounts').subscribe((data)=>{
    this.Level5List = data;
  })
}

getMonthYear() {
  this.apiService.getData('Payroll/GetMonthYear').subscribe((data) => {
    const year = data[0].year;
    // this.SalaryPayableForm.get('month').setValue(data[0].mnth);

    this.months = this.months.map((month) => ({
      id: month.id,
      name: `${month.name} - ${year}`
    }));
  });
}

toggleApproveAll() {
  const isChecked = this.SalaryPayableForm.get('apprvAll').value;
  this.SalaryPayableList.forEach(item => {
      item.sent = isChecked;
  });
}



onClickSave() {

  const form = this.SalaryPayableForm.value;

  if (!form.amount) {
    this.tostr.warning('Enter Loan Amount');
    return;
  }

  if(form.empName == null || form.empName == undefined){
    this.tostr.warning('Select Employee');
    return;
  }

  let vchNo = this.editMode ? this.SalaryPayableForm.get('SrNo')?.value : 0;

  const dataToSave = {
    Srno: vchNo,
    StDate: this.dp.transform(form.Date, 'yyyy-MM-dd'),
    Remarks: form.remarks,
    Reference: form.reference,
    EmpyId: form.empName,
    IcomeTaxdeducation: form.amount,
    Active: form.isAllow == "" ? false : form.isAllow,
    sent: form.sent
  };





  this.apiService
    .saveData('Payroll/SaveIncomeTax', dataToSave)
    .subscribe((result) => {
      if (result === true || result === 'true') {
        this.tostr.success('Save Successfully');
        this.onClickRefresh();
      } else {
        this.tostr.error('Please Save Again');
      }
    });
}

getSalaryPayables(event: any): void {
  this.SalaryPayableList = [];
  const year = event.name.split('-')[1].trim();
  const obj = {
    Month: event.id,
    year: year
  };

  this.apiService
    .getDataById('Payroll/getSalaryPayables', obj)
    .subscribe((data) => {
      console.log(data);
      if (!data || data.length === 0) {
        this.tostr.info("No Records Found");
        this.enableFields();
        return;
      }
      this.enableFields();
      data.forEach((item: any) => {
        let form = item;
        form.empy_id = item.empy_id;
        form.empName = item.Name;
        form.Department = item.Department;
        form.Designation = item.Designation;
        form.netsalary = item.netsalary,
        form.basic = item.basic,
        form.gsalary = item.gsalary,
        form.Leave = item.Leave,
        form.Insurance = item.Insurance,
        form.ProvidentLoan = item.ProvidentLoan,
        form.Advance = item.Advance,
        form.Bonus = item.Bonus,
        form.StaffLoan = item.StaffLoan,
        form.VehicleLoan = item.VehicleLoan,
        form.EOBI = item.EOBI,
        form.Level2 = item.Level2,
        form.Level3 = item.Level3,
        form.Level4 = item.Level4,
        form.Level5 = item.Level5,
        form.Level6 = item.Level6,
        form.Level7 = item.Level7,
        form.LocName = item.LocName,

        this.SalaryPayableList.push(form);
      });
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
