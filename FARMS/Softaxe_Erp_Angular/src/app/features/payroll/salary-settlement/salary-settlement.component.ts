import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-salary-settlement',
  templateUrl: './salary-settlement.component.html',
  styleUrls: ['./salary-settlement.component.css']
})
export class SalarySettlementComponent {

  constructor(
    private fb: FormBuilder,
    private tostr: ToastrService,
    private apiService: ApiService,
    private dp: DatePipe
  ) {
    const today = new Date();
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  }

  ngOnInit() {
    this.formInit();
    this.getDepartmentList();
    this.getDesignationList();
    this.getSalaryTypeList();
    this.getEmpTypeList();
    this.getSalaryReasonList();
    this.getEmployeeList();
    this.disableFields();
    //this.getSettlementNo();
   this.getSalarySettlementLabels();
  }

  // List Page

  @ViewChild('voucherLists', { static: false }) voucherLists!: ElementRef;
  voucherList: any[] = [];
  fromDate: Date;
  toDate: Date;

  SalarySetlementForm!: FormGroup;
  SalarySetlementList: any = [];
  isShow = false;
  isReadOnly: boolean = true;
  editMode: boolean = true;
  editSno: any = '';
  editModeSno: boolean = false;
  selectedRow: number | null = null;
  btnAdd:string = 'Add';
  document: File | null = null;
  selectedDocument: any = '';
  file: any;
  Disabled:boolean;


  formInit() {
    this.SalarySetlementForm = this.fb.group({
      entryDate: [new Date()],
      hireDate: [new Date()],
      joinDate: [new Date()],
      SrNo: [''],
      empName: [undefined],
      department: [undefined],
      designation: [undefined],
      type: [undefined],
      SalaryType: [undefined],
      reason: [undefined],
      grossSalary: [''],
      cashSalary: [''],
      bankSalary: [''],
      basic: [''],
      grade: [''],
      pf: [''],
      lvl2: [''],
      lvl3: [''],
      lvl4: [''],
      lvl5: [''],
      lvl6: [''],
      lvl7: [''],
      remarks: [''],
      netSalary: [''],
      active1: [''],
      s1: [''],
      s2: [''],
      s3: [''],
      s4: [''],
      s5: [''],
      s6: [''],
      s7: ['']
    });
  }

  // Employee

  EmployeeList: any = [];

  // Designation

  DesignationList: any = [];
  DesignationName: any;
  isDisabledDesignation: boolean = true;
  isShowDesignation: boolean = false;
  DesignationId: number = 0;

  newDesignation() {
    this.refreshDesignation();
    this.isDisabledDesignation = false;
    this.isShowDesignation = true;
  }

  refreshDesignation() {
    this.DesignationId = 0;
    this.DesignationName = '';
    this.isDisabledDesignation = true;
    this.isShowDesignation = false;
  }

  createUpdateDesignation() {
    if (this.DesignationName == '') {
      this.tostr.warning('Enter Designation Name ....!');
      return;
    }

    const obj = {
      Id: this.DesignationId,
      Designation: this.DesignationName,
    };

    this.apiService
      .saveObj('Payroll/SaveDesignation', obj)
      .subscribe((result) => {
        if (result == true || result == 'true') {
          this.tostr.success('Save Successfully');
          this.refreshDesignation();
          this.getDesignationList();
        } else {
          this.tostr.error('Please Save Again');
        }
      });
  }

  getDesignationList() {
    this.apiService.getData('Payroll/GetDesignationList').subscribe((data)=>{
      this.DesignationList = data;
    })
}

getEmployeeList() {
  this.apiService.getData('Payroll/GetEmployees').subscribe((data)=>{
    this.EmployeeList = data;
  })
}


editDesignation(Id: any, Designation: any) {
  this.DesignationId = Id;
  this.DesignationName = Designation;
  this.isDisabledDesignation = false;
  this.isShowDesignation = true;
}

deleteDesignation(Id: any): void {
  const confirmDelete = confirm('Are you sure you want to delete this Designation?');

  if (confirmDelete == true) {
    const obj = {
      Id: Id,
    };
    this.apiService.deleteData('Payroll/DelDesignation', obj).subscribe({
      next: (data) => {
        if (data == 'true' || data == true) {
          this.getDesignationList();
          this.tostr.success('Delete Successfully');
        } else if (data == 'false' || data == false) {
          this.tostr.error('Delete Again');
        } else {
          this.tostr.warning('In Used');
        }
      },
      error: (error) => {
        this.tostr.info(error.error.text);
      },
    });
  }
}

  // ---------------------------------------------------------------------- //

  // Department

  DepartmentList: any = [];
  DepartMentName: any;
  isDisabledDepartment: boolean = true;
  isShowDepartment: boolean = false;
  DepartMentId: number = 0;

  newDepartment() {
    this.refreshDepartment();
    this.isDisabledDepartment = false;
    this.isShowDepartment = true;
  }

  refreshDepartment() {
    this.DepartMentId = 0;
    this.DepartMentName = '';
    this.isDisabledDepartment = true;
    this.isShowDepartment = false;
  }

  createUpdateDepartment() {
    if (this.DepartMentName == '') {
      this.tostr.warning('Enter Department Name ....!');
      return;
    }

    const obj = {
      Id: this.DepartMentId,
      Department: this.DepartMentName,
    };

    this.apiService
      .saveObj('Payroll/SaveDepartment', obj)
      .subscribe((result) => {
        if (result == true || result == 'true') {
          this.tostr.success('Save Successfully');
          this.refreshDepartment();
          this.getDepartmentList();
        } else {
          this.tostr.error('Please Save Again');
        }
      });
  }

  getDepartmentList() {
    this.apiService.getData('Payroll/GetDepartmentList').subscribe((data)=>{
      this.DepartmentList = data;
    })
}

editDepartment(Id: any, Department: any) {
  this.DepartMentId = Id;
  this.DepartMentName = Department;
  this.isDisabledDepartment = false;
  this.isShowDepartment = true;
}

deleteDepartment(Id: any): void {
  const confirmDelete = confirm('Are you sure you want to delete this department?');

  if (confirmDelete == true) {
    const obj = {
      Id: Id,
    };
    this.apiService.deleteData('Payroll/DelDepartment', obj).subscribe({
      next: (data) => {
        if (data == 'true' || data == true) {
          this.getDepartmentList();
          this.tostr.success('Delete Successfully');
        } else if (data == 'false' || data == false) {
          this.tostr.error('Delete Again');
        } else {
          this.tostr.warning('In Used');
        }
      },
      error: (error) => {
        this.tostr.info(error.error.text);
      },
    });
  }
}

// ----------------------------------------------------------//

  // Salary Type

  SalaryTypeList: any = [];
  SalaryTypeName: any;
  isDisabledSalaryType: boolean = true;
  isShowSalaryType: boolean = false;
  SalaryTypeId: number = 0;

  newSalaryType() {
    this.refreshSalaryType();
    this.isDisabledSalaryType = false;
    this.isShowSalaryType = true;
  }

  refreshSalaryType() {
    this.SalaryTypeId = 0;
    this.SalaryTypeName = '';
    this.isDisabledSalaryType = true;
    this.isShowSalaryType = false;
  }

  createUpdateSalaryType() {
    if (this.SalaryTypeName == '') {
      this.tostr.warning('Enter Salary Type ....!');
      return;
    }

    const obj = {
      Id: this.SalaryTypeId,
      SalaryType: this.SalaryTypeName,
    };

    this.apiService
      .saveObj('Payroll/SaveSalaryType', obj)
      .subscribe((result) => {
        if (result == true || result == 'true') {
          this.tostr.success('Save Successfully');
          this.refreshSalaryType();
          this.getSalaryTypeList();
        } else {
          this.tostr.error('Please Save Again');
        }
      });
  }

  getSalaryTypeList() {
    this.apiService.getData('Payroll/GetSalaryTypeList').subscribe((data)=>{
      this.SalaryTypeList = data;
    })
}

editSalaryType(Id: any, SalaryType: any) {
  this.SalaryTypeId = Id;
  this.SalaryTypeName = SalaryType;
  this.isDisabledSalaryType = false;
  this.isShowSalaryType = true;
}

deleteSalaryType(Id: any): void {
  const confirmDelete = confirm('Are you sure you want to delete this Salary Type?');

  if (confirmDelete == true) {
    const obj = {
      Id: Id,
    };
    this.apiService.deleteData('Payroll/DelSalaryType', obj).subscribe({
      next: (data) => {
        if (data == 'true' || data == true) {
          this.getSalaryTypeList();
          this.tostr.success('Delete Successfully');
        } else if (data == 'false' || data == false) {
          this.tostr.error('Delete Again');
        } else {
          this.tostr.warning('In Used');
        }
      },
      error: (error) => {
        this.tostr.info(error.error.text);
      },
    });
  }
}


// --------------------------------------------------------------//

// Employee Type

EmpTypeList: any = [];
EmpTypeName: any;
isDisabledEmpType: boolean = true;
isShowEmpType: boolean = false;
EmpTypeId: number = 0;

newEmpType() {
  this.refreshEmpType();
  this.isDisabledEmpType = false;
  this.isShowEmpType = true;
}

refreshEmpType() {
  this.EmpTypeId = 0;
  this.EmpTypeName = '';
  this.isDisabledEmpType = true;
  this.isShowEmpType = false;
}

createUpdateEmpType() {
  if (this.EmpTypeName == '') {
    this.tostr.warning('Enter Employee Type ....!');
    return;
  }

  const obj = {
    Id: this.EmpTypeId,
    EmployeeType: this.EmpTypeName,
  };

  this.apiService
    .saveObj('Payroll/SaveEmployeeType', obj)
    .subscribe((result) => {
      if (result == true || result == 'true') {
        this.tostr.success('Save Successfully');
        this.refreshEmpType();
        this.getEmpTypeList();
      } else {
        this.tostr.error('Please Save Again');
      }
    });
}

getEmpTypeList() {
  this.apiService.getData('Payroll/GetEmpTypeList').subscribe((data)=>{
    this.EmpTypeList = data;
  })
}

editEmpType(Id: any, EmployeeType: any) {
this.EmpTypeId = Id;
this.EmpTypeName = EmployeeType;
this.isDisabledEmpType = false;
this.isShowEmpType = true;
}

deleteEmpType(Id: any): void {
const confirmDelete = confirm('Are you sure you want to delete this Employee Type?');

if (confirmDelete == true) {
  const obj = {
    Id: Id,
  };
  this.apiService.deleteData('Payroll/DelEmployeeType', obj).subscribe({
    next: (data) => {
      if (data == 'true' || data == true) {
        this.getEmpTypeList();
        this.tostr.success('Delete Successfully');
      } else if (data == 'false' || data == false) {
        this.tostr.error('Delete Again');
      } else {
        this.tostr.warning('In Used');
      }
    },
    error: (error) => {
      this.tostr.info(error.error.text);
    },
  });
}
}

// ---------------------------------------------------------- //

  // Salary Reason

SalaryReasonList: any = [];
SalaryReasonName: any;
isDisabledSalaryReason: boolean = true;
isShowSalaryReason: boolean = false;
SalaryReasonId: number = 0;

newSalaryReason() {
  this.refreshSalaryReason();
  this.isDisabledSalaryReason = false;
  this.isShowSalaryReason = true;
}

refreshSalaryReason() {
  this.SalaryReasonId = 0;
  this.SalaryReasonName = '';
  this.isDisabledSalaryReason = true;
  this.isShowSalaryReason = false;
}

createUpdateSalaryReason() {
  if (this.SalaryReasonName == '') {
    this.tostr.warning('Enter Reason....!');
    return;
  }

  const obj = {
    Id: this.SalaryReasonId,
    Reason: this.SalaryReasonName,
  };

  this.apiService
    .saveObj('Payroll/SaveSalaryReason', obj)
    .subscribe((result) => {
      if (result == true || result == 'true') {
        this.tostr.success('Save Successfully');
        this.refreshSalaryReason();
        this.getSalaryReasonList();
      } else {
        this.tostr.error('Please Save Again');
      }
    });
}

getSalaryReasonList() {
  this.apiService.getData('Payroll/GetSalaryReasonList').subscribe((data)=>{
    this.SalaryReasonList = data;
  })
}

editSalaryReason(Id: any, Reason: any) {
this.SalaryReasonId = Id;
this.SalaryReasonName = Reason;
this.isDisabledSalaryReason = false;
this.isShowSalaryReason = true;
}

deleteSalaryReason(Id: any): void {
const confirmDelete = confirm('Are you sure you want to delete this Reason?');

if (confirmDelete == true) {
  const obj = {
    Id: Id,
  };
  this.apiService.deleteData('Payroll/DelSalaryReason', obj).subscribe({
    next: (data) => {
      if (data == 'true' || data == true) {
        this.getSalaryReasonList();
        this.tostr.success('Delete Successfully');
      } else if (data == 'false' || data == false) {
        this.tostr.error('Delete Again');
      } else {
        this.tostr.warning('In Used');
      }
    },
    error: (error) => {
      this.tostr.info(error.error.text);
    },
  });
}
}

// -------------------------------------------------------- //


  // togglePages() {
  //   this.isShowPage = !this.isShowPage;
  //   if (this.isShowPage) {

  //   }
  // }

  resetForm() {
    this.SalarySetlementForm.get('SalaryType')?.patchValue(undefined);
    this.SalarySetlementForm.get('department')?.patchValue(undefined);
    this.SalarySetlementForm.get('designation')?.patchValue(undefined);
    this.SalarySetlementForm.get('type')?.patchValue(undefined);
    this.SalarySetlementForm.get('reason')?.patchValue(undefined);
    this.SalarySetlementForm.get('empName')?.patchValue(undefined);
    this.SalarySetlementForm.get('grossSalary')?.patchValue('');
    this.SalarySetlementForm.get('cashSalary')?.patchValue('');
    this.SalarySetlementForm.get('bankSalary')?.patchValue('');
    this.SalarySetlementForm.get('netSalary')?.patchValue('');
    this.SalarySetlementForm.get('pf')?.patchValue('');
    this.SalarySetlementForm.get('basic')?.patchValue('');
    this.SalarySetlementForm.get('lvl2')?.patchValue('');
    this.SalarySetlementForm.get('lvl3')?.patchValue('');
    this.SalarySetlementForm.get('lvl4')?.patchValue('');
    this.SalarySetlementForm.get('lvl5')?.patchValue('');
    this.SalarySetlementForm.get('lvl6')?.patchValue('');
    this.SalarySetlementForm.get('lvl7')?.patchValue('');
    this.SalarySetlementForm.get('remarks')?.patchValue('');
    this.SalarySetlementForm.get('grade')?.patchValue('');
    this.SalarySetlementForm.get('active1')?.patchValue('');
    this.SalarySetlementForm.get('joinDate')?.patchValue(new Date());
    this.SalarySetlementForm.get('hireDate')?.patchValue(new Date());
    this.SalarySetlementForm.get('entryDate')?.patchValue(new Date());

  }


  onClickNew() {
    this.isShow = true;
    this.editMode = false;
    this.enableFields();
  }

  enableFields() {
    this.Disabled = false;
  }

  disableFields() {
    this.Disabled = true;
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



  onDocumentSelected(event: any) {
    const inputElement = event.target;
    if (inputElement.files && inputElement.files.length > 0) {
      this.document = inputElement.files[0];
    } else {
      this.document = null;
    }
  }

  editItem(row: any) {

    this.btnAdd = 'Update'
    this.editMode = true;
    this.editModeSno = true;
    this.SalarySetlementForm.get('empName')?.patchValue(row.empName);
    this.SalarySetlementForm.get('department')?.patchValue(row.deptId);
    this.SalarySetlementForm.get('designation')?.patchValue(row.desgnId);
    this.SalarySetlementForm.get('type')?.patchValue(row.typeId);
    this.SalarySetlementForm.get('SalaryType')?.patchValue(row.SalaryTypeId);
    this.SalarySetlementForm.get('netSalary')?.patchValue(row.netSalary);
    this.SalarySetlementForm.get('grossSalary')?.patchValue(row.grossSalary);
    this.SalarySetlementForm.get('cashSalary')?.patchValue(row.cashSalary);
    this.SalarySetlementForm.get('bankSalary')?.patchValue(row.bankSalary);
    this.SalarySetlementForm.get('remarks')?.patchValue(row.remarks);
    this.SalarySetlementForm.get('pf')?.patchValue(row.pf);
    this.SalarySetlementForm.get('grade')?.patchValue(row.grade);
    this.SalarySetlementForm.get('reason')?.patchValue(row.reason);
    this.SalarySetlementForm.get('active1')?.patchValue(row.active1);
    this.SalarySetlementForm.get('joinDate')?.patchValue(row.joinDate);
    this.SalarySetlementForm.get('hireDate')?.patchValue(row.hireDate);
    this.SalarySetlementForm.get('entryDate')?.patchValue(row.entryDate);
    this.SalarySetlementForm.get('empName')?.patchValue(row.EmpyId);
    this.SalarySetlementForm.get('basic')?.patchValue(row.bsalary);
    this.SalarySetlementForm.get('lvl2')?.patchValue(row.Level2);
    this.SalarySetlementForm.get('lvl3')?.patchValue(row.Level3);
    this.SalarySetlementForm.get('lvl4')?.patchValue(row.Level4);
    this.SalarySetlementForm.get('lvl5')?.patchValue(row.Level5);
    this.SalarySetlementForm.get('lvl6')?.patchValue(row.Level6);
    this.SalarySetlementForm.get('lvl7')?.patchValue(row.Level7);
    this.SalarySetlementForm.get('SrNo')?.patchValue(row.SrNo);

  }


  CalculateSalary() {
    const grossSalary = this.SalarySetlementForm.get('grossSalary').value;

    const basicValue = (grossSalary / 100) * this.SalarySetlementForm.get('s1').value;
    this.SalarySetlementForm.get('basic').setValue(Number(basicValue.toFixed(0)));

    const lvl2Value = (grossSalary / 100) * this.SalarySetlementForm.get('s2').value;
    this.SalarySetlementForm.get('lvl2').setValue(Number(lvl2Value.toFixed(0)));

    const lvl3Value = (grossSalary / 100) * this.SalarySetlementForm.get('s3').value;
    this.SalarySetlementForm.get('lvl3').setValue(Number(lvl3Value.toFixed(0)));

    const lvl4Value = (grossSalary / 100) * this.SalarySetlementForm.get('s4').value;
    this.SalarySetlementForm.get('lvl4').setValue(Number(lvl4Value.toFixed(0)));

    const lvl5Value = (grossSalary / 100) * this.SalarySetlementForm.get('s5').value;
    this.SalarySetlementForm.get('lvl5').setValue(Number(lvl5Value.toFixed(0)));

    const lvl6Value = (grossSalary / 100) * this.SalarySetlementForm.get('s6').value;
    this.SalarySetlementForm.get('lvl6').setValue(Number(lvl6Value.toFixed(0)));

    const lvl7Value = (grossSalary / 100) * this.SalarySetlementForm.get('s7').value;
    this.SalarySetlementForm.get('lvl7').setValue(Number(lvl7Value.toFixed(0)));

    const netSalary = basicValue + lvl2Value + lvl3Value + lvl4Value + lvl5Value + lvl6Value + lvl7Value;
    this.SalarySetlementForm.get('netSalary').setValue(Number(netSalary.toFixed(0)));
}


  onClickRefresh() {
    this.isShow = false;
    this.resetForm();
    this.SalarySetlementList = [];
    this.disableFields();
  }


  onClickSave() {

    const form = this.SalarySetlementForm.value;
  
    if (!form.pf) {
      this.tostr.warning('Enter P/F');
      return;
    }

    if (!form.grossSalary) {
      this.tostr.warning('Enter Gross Salary');
      return;
    }

    if (!form.bankSalary) {
      this.tostr.warning('Enter Bank Salary');
      return;
    }

    if (!form.cashSalary) {
      this.tostr.warning('Enter Cash Salary');
      return;
    }
  
    if(form.empName == null || form.empName == undefined){
      this.tostr.warning('Select Employee');
      return;
    }

    if(form.SalaryType == null || form.SalaryType == undefined){
      this.tostr.warning('Select Salary Type');
      return;
    }

    if(form.reason == null || form.reason == undefined){
      this.tostr.warning('Select Salary reason');
      return;
    }

    if(form.type == null || form.type == undefined){
      this.tostr.warning('Select Employe Type');
      return;
    }

    if(form.designation == null || form.designation == undefined){
      this.tostr.warning('Select Designation');
      return;
    }


    
    if(form.department == null || form.department == undefined){
      this.tostr.warning('Select Department');
      return;
    }
  
  
    let vchNo = this.editMode ? this.SalarySetlementForm.get('SrNo')?.value : 0;
  
    const dataToSave = {
      SrNo: vchNo,
      JoinDate: this.dp.transform(form.joinDate, 'yyyy-MM-dd'),
      HireDate: this.dp.transform(form.hireDate,'yyyy-MM-dd'),
      EmpyId: form.empName,
      DeptId: form.department,
      DesgId: form.designation,
      EmpyType: form.type,
      Through: form.SalaryType,
      Grade: form.grade,
      Reasons: form.reason,
      Gsalary: form.grossSalary,
      remarks: form.remarks,
      Netsalary: form.netSalary,
      Active: form.active1 == "" ? false : form.active1,
      Banksalary: form.bankSalary,
      Cashsalary: form.cashSalary,
      Bsalary: form.basic,
      Level2: form.lvl2,
      Level3: form.lvl3,
      Level4: form.lvl4,
      Level5: form.lvl5,
      Level6: form.lvl6,
      Level7: form.lvl7,
    };
  
    this.apiService
      .saveData('Payroll/AddUpdateSalryStlmnt', dataToSave)
      .subscribe((result) => {
        if (result === true || result === 'true') {
          this.tostr.success('Save Successfully');
          this.onClickRefresh();
        } else {
          this.tostr.error('Please Save Again');
        }
      });
  }


  getSalarySettlementLabels() {
    this.apiService.getData('Payroll/GetSalarySettlementLabels')
      .subscribe((data) => {

      
        this.SalarySetlementForm.get('s1').patchValue(data[0].p1);
        this.SalarySetlementForm.get('s2').patchValue(data[0].p2);
        this.SalarySetlementForm.get('s3').patchValue(data[0].p3);
        this.SalarySetlementForm.get('s4').patchValue(data[0].p4);
        this.SalarySetlementForm.get('s5').patchValue(data[0].p5);
        this.SalarySetlementForm.get('s6').patchValue(data[0].p6);
        this.SalarySetlementForm.get('s7').patchValue(data[0].p7);
      });
  }


  editSalary(event: any): void {


    this.SalarySetlementList = [];

    const obj = {
      empy_id: event.empy_id,
    };

    this.apiService
      .getDataById('Payroll/GetEditSalrySetlment', obj)
      .subscribe((data) => {

        if (!data || data.length === 0) {
          this.tostr.info("No Records Found");
          this.enableFields();
          return;
        }
        
        this.enableFields();
        this.SalarySetlementForm.get('department').setValue(data[0].dept_Id);
        data.forEach((item: any) => {
          this.SalarySetlementForm
            .get('entryDate')
            ?.patchValue(
              new Date(
                item.trdate.split('/')[2],
                item.trdate.split('/')[1] - 1,
                item.trdate.split('/')[0]
              )
            );
            this.SalarySetlementForm
            .get('hireDate')
            ?.patchValue(
              new Date(
                item.hire_date.split('/')[2],
                item.hire_date.split('/')[1] - 1,
                item.hire_date.split('/')[0]
              )
            );
            this.SalarySetlementForm
            .get('joinDate')
            ?.patchValue(
              new Date(
                item.join_date.split('/')[2],
                item.join_date.split('/')[1] - 1,
                item.join_date.split('/')[0]
              )
            );
          //this.SalarySetlementForm.get('SrNo')?.patchValue(item.SrNo);
          let form = item;
          form.sno = item.SrNo;
          form.empName = item.Name;
          form.EmpyId = item.empy_id;
          form.deptId = item.dept_Id;
          form.department = item.Department;
          form.desgnId = item.desg_Id;
          form.designation = item.Designation;
          form.typeId = item.TypeId;
          form.type = item.EmployeeType;
          form.SalaryTypeId = item.SalaryTypeId;
          form.SalaryType = item.SalaryType;
          form.grossSalary = item.gsalary;
          form.netSalary = item.netsalary;
          form.reason = item.reasons;
          let JoinDate = this.dp.transform(item.join_date, 'yyyy-MM-dd');
          let HireDate = this.dp.transform(item.hire_date,'yyyy-MM-dd');
          let EntryDate = this.dp.transform(item.trdate, 'yyyy-MM-dd');
          form.joinDate = JoinDate;
          form.hireDate = HireDate;
          form.entryDate = EntryDate;
          form.cashSalary = item.CASHSALARY;
          form.bankSalary = item.banksalary;
          form.basic = item.bsalary;
          form.lvl2 = item.Level2;
          form.lvl3 = item.Level3;
          form.lvl4 = item.Level4;
          form.lvl5 = item.Level5;
          form.lvl6 = item.Level6;
          form.lvl7 = item.Level7;
          form.grade = item.grade;
          form.active1 = item.Active;
          this.SalarySetlementList.push(form);
        });
      });
  }


  deleteSalary(SrNo:any, EmpyId:any): void {
    const confirmDelete = confirm('Are you sure you want to delete this Salary?');

    if (confirmDelete == true) {
      const obj = {
        empy_id: EmpyId,
        SrNo: SrNo
      };

      this.apiService.deleteData('Payroll/DelSalrySetlment', obj).subscribe({
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
