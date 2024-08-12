import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { environment } from 'src/environment/environmemt';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-employee-entry',
  templateUrl: './employee-entry.component.html',
  styleUrls: ['./employee-entry.component.css']
})
export class EmployeeEntryComponent {

  constructor(
    private apiService: ApiService,
    private fb: FormBuilder,
    private tostr: ToastrService,
    private dp: DatePipe,
    private com: CommonService,
    private auth: AuthService,
  ) {}

  @ViewChild('EmployeeLists', { static: false }) EmployeeLists!: ElementRef;

  EmployeeForm!: FormGroup;
  isShow = false;
  isShowDocument = false;
  productImage: File | null = null;
  document: File | null = null;
  selectedImage: any = '';
  selectedDocument: any = '';
  file: any;
  editSno: any = '';
  editModeSno: boolean = false;
  isDisabled: boolean = true;
  isShowPage: boolean = true;
  DepartmentList: any = [];
  LocationList: any = [];
  ShiftList: any = [];
  StatusList: any = [];
  basePath = environment.basePath
  query = "?v="+ Math.random();
  btnAdd:string = 'Add';
  EmployeeList: any[] = [];
  LeaveTypeList: any[] = [];
  LeavesEntryList: any[] = [];
  FilteredEmployeeList: any[] = [];
  selectedLocation: any;

  getDepartment() {
    this.apiService.getData('Payroll/GetDepartment').subscribe((data) => {
      this.DepartmentList = data;
    });
  }

  getStatus() {
    this.apiService.getData('Payroll/GetStatus').subscribe((data) => {
      this.StatusList = data;
    });
  }

  getMaxEmpId() {
    
    this.apiService.getData('Payroll/GetMaxEmpId').subscribe((data) => {
      this.EmployeeForm.get('EmpyId').setValue(data[0].empy_id);
    });
  }

  searchEmpList(event: any) {

    if (this.selectedLocation) {
        this.FilteredEmployeeList = this.EmployeeList.filter((item: any) => item.LocatioName === event.CostcentreName);
    } else {
       
        this.FilteredEmployeeList = this.EmployeeList;
    }
}


  getLocation() {
    this.apiService.getData('Payroll/GetLocation').subscribe((data) => {
      this.LocationList = data;
    });
  }

  getShift() {
    this.apiService.getData('Payroll/GetShift').subscribe((data) => {
      this.ShiftList = data;
    });
  }



  ngOnInit() {

    this.formInit();
    this.getEmployeeList();
    this.getDepartment();
    this.getLocation();
    this.getShift();
    this.getLeaveTypeList();
    this.getStatus();
  }

  formInit() {
    this.EmployeeForm = this.fb.group({
      EmpyId: [''],
      name: [''],
      Tumbid: [''],
      Fcnic: [''],
      Fname: [''],
      motherName: [''],
      motherCNIC: [''],
      wifeName: [''],
      wifeCNIC: [''],
      Address1: [''],
      bloodGroup: [''],
      city: [''],
      Ph1: [''],
      Ph2: [''],
      email: [''],
      mob: [''],
      DeptId: [undefined],
      Marital: [undefined],
      gender: [undefined],
      location: [undefined],
      shift: [undefined],
      ntn: [''],
      nic: [''],
      Acctno: [''],
      Eobino: [''],
      Ssno: [''],
      remarks: [''],
      ot: [''],
      LvAmnt: [''],
      leaveType: [undefined],
      active1: [''],
      AppDate: [new Date()],
      BirthDate: [new Date()]
    });
  }

  togglePages() {
    this.isShowPage = !this.isShowPage;
    this.onClickRefresh();
    if (this.isShowPage) {
     
    }
  }

  onFileSelected(event: any) {
    this.productImage = event.target.files[0];
    if (this.productImage) {
      this.selectedImage = URL.createObjectURL(event.target.files[0]);
      console.log(this.selectedImage);
    }
  }
  

  onDocumentSelected(event: any) {
    const inputElement = event.target;
    if (inputElement.files && inputElement.files.length > 0) {
      this.document = inputElement.files[0];
    } else {
      this.document = null;
    }
  }

  onClickNew() {
    this.onClickRefresh();
    this.isShow = true;
    this.isDisabled = false;
    this.getMaxEmpId();
    }

  onClickRefresh() {
    this.EmployeeForm.reset();
    this.isShow = false;
    this.isDisabled = true;
    this.selectedImage = '';
    this.productImage = null;
    this.document = null;
    this.file = '';
    this.LeavesEntryList = [];
  }

  onClickSave() {
    let body = this.EmployeeForm.value;



    body.EmpyId = body.EmpyId == null ? 0 : body.EmpyId;
    body.ot = body.ot == null ? false : body.ot;
  
    body.BirthDate = this.dp.transform(
      this.EmployeeForm.get('BirthDate')?.value,
      'yyyy-MM-dd'
    );
  
    body.AppDate = this.dp.transform(
      this.EmployeeForm.get('AppDate')?.value,
      'yyyy-MM-dd'
    );
  
    if (body.DeptId == null) {
      this.tostr.warning('Select Department....!');
      return;
    }
  
    if (body.location == null) {
      this.tostr.warning('Select Location....!');
      return;
    }
  
    if (body.shift == null) {
      this.tostr.warning('Select Shift....!');
      return;
    }
  
    if (body.name == '' || body.name == null) {
      this.tostr.warning('Enter Employee Name....!');
      return;
    }
  
    if (body.nic == null || body.nic == 0) {
      this.tostr.warning('Enter CNIC Number....!');
      return;
    }
  
    if (body.BirthDate == null || body.BirthDate == 0) {
      this.tostr.warning('Enter Date Of Birth....!');
      return;
    }
  
    if (body.AppDate == null || body.AppDate == 0) {
      this.tostr.warning('Enter Date of Joining....!');
      return;
    }
  
    const modifiedLeavesEntryList = this.LeavesEntryList.map(entry => {
      return {
        LvAmnt: entry.LvAmnt,
        leaveType: entry.leaveType
      };
    });
  
    let formData = new FormData();
    for (const key of Object.keys(body)) {
      formData.append(key, body[key]);
    }
  
    formData.append('LeaveEntryString', JSON.stringify(modifiedLeavesEntryList));
  
    formData.append('Picture', this.productImage!);
    formData.append('Document', this.document!);
  
    for (const key of Object.keys(body)) {
      if (body[key] === '') {
        body[key] = null;
      }
    }


    
    this.apiService.saveData('Payroll/AddUpdateEmployee', formData).subscribe(
      (result) => {
        if (result == true || result == 'true') {
          this.tostr.success('Save Successfully');
          this.onClickRefresh();
          this.getEmployeeList();
        } else {
          this.tostr.error('Please Save Again');
        } 
      },
      (error) => {
        this.tostr.info('Open Level 4 for this Location');
      }
    );
  }
  

  getEmployeeList() {
    
    this.apiService.getData('Payroll/GetEmployeeList').subscribe((data)=>{
      this.EmployeeList = data;
    })
}

getLeaveTypeList() {
  this.apiService.getData('Payroll/GetLeaveType').subscribe((data)=>{
    this.LeaveTypeList = data;
  })
}


editEmployee(empy_id: any) {  
  let obj = {
    empy_id: empy_id
  };

  this.isShowDocument = true;
  this.togglePages();
  this.onClickNew();
  this.apiService
    .getDataById('Payroll/EditEmployee', obj)
    .subscribe((data) => {
      this.EmployeeForm.get('EmpyId')?.setValue(data.EmpData[0].empy_id);
      this.EmployeeForm.get('name')?.setValue(data.EmpData[0].Name);
      this.EmployeeForm.get('Tumbid')?.setValue(data.EmpData[0].tumbid);
      this.EmployeeForm.get('Fname')?.setValue(data.EmpData[0].Fname);
      this.EmployeeForm.get('motherName')?.setValue(data.EmpData[0].MotherName);
      this.EmployeeForm.get('wifeName')?.setValue(data.EmpData[0].WifeName);
      this.EmployeeForm.get('Fcnic')?.setValue(data.EmpData[0].Fcnic);
      this.EmployeeForm.get('motherCNIC')?.setValue(data.EmpData[0].Mothercnic);
      this.EmployeeForm.get('wifeCNIC')?.setValue(data.EmpData[0].Wifecnic);
      this.EmployeeForm.get('Address1')?.setValue(data.EmpData[0].address1);
      this.EmployeeForm.get('DeptId')?.setValue(data.EmpData[0].deptId);
      this.EmployeeForm.get('city')?.setValue(data.EmpData[0].city);
      this.EmployeeForm.get('bloodGroup')?.setValue(data.EmpData[0].BloodGroup);
      this.EmployeeForm.get('Ph2')?.setValue(data.EmpData[0].ph2);
      this.EmployeeForm.get('Ph1')?.setValue(data.EmpData[0].ph1);
      this.EmployeeForm.get('mob')?.setValue(data.EmpData[0].mob);
      this.EmployeeForm.get('email')?.setValue(data.EmpData[0].Email);
      this.EmployeeForm.get('gender')?.setValue(data.EmpData[0].gender);
      this.EmployeeForm.get('location')?.setValue(data.EmpData[0].Location);
      this.EmployeeForm.get('ntn')?.setValue(data.EmpData[0].ntn);
      this.EmployeeForm.get('nic')?.setValue(data.EmpData[0].nic);
      this.EmployeeForm.get('shift')?.setValue(data.EmpData[0].Shift);
      this.EmployeeForm.get('Marital')?.setValue(data.EmpData[0].marital);
      this.EmployeeForm.get('Ssno')?.setValue(data.EmpData[0].SSNO);
      this.EmployeeForm.get('Eobino')?.setValue(data.EmpData[0].EOBINO);
      this.EmployeeForm.get('Acctno')?.setValue(data.EmpData[0].Acctno);
      this.EmployeeForm.get('remarks')?.setValue(data.EmpData[0].remarks);
      this.EmployeeForm.get('ot')?.setValue(data.EmpData[0].ot);
      this.EmployeeForm.get('active1')?.setValue(data.EmpData[0].active1);
      this.EmployeeForm.get('BirthDate')?.patchValue(new Date(data.EmpData[0].birth_date));
      this.EmployeeForm.get('AppDate')?.patchValue(new Date(data.EmpData[0].app_date));
      this.selectedImage = this.basePath + data.EmpData[0].EmpPhoto


      // if (data[0].document !== null) {
      //   this.selectedDocument = this.basePath + data[0].document;
      // } else {
      //   this.selectedDocument = 'javascript:void(0)';
        
      // }

      data.LeaveData.forEach((item: any) => {
        let form = item;
        form.sno = item.lv_id;
        form.leaveTypeName = item.Name;
        form.LvAmnt = item.NoOfLvs;
        form.leaveType = item.lv_id;
        this.LeavesEntryList.push(form);
       });


    });
}


deleteEmployee(empy_id: any): void {
  const confirmDelete = confirm('Are you sure you want to delete this item?');

  if (confirmDelete == true) {
    const obj = {
      empy_id: empy_id  
    };

    this.apiService.deleteData('Payroll/DeleteEmployee', obj).subscribe({
      next: (data) => {
        if (data == 'true' || data == true) {
          this.tostr.success('Delete Successfully');
          this.getEmployeeList();
        } else if (data == 'false' || data == false) {
          this.tostr.error('Delete Again');
        } else if (data == 'Already in Use') {
          this.tostr.info('Already in Use');
        }
      },
      error: (error) => {
        if (error.error && error.error.text) {
          this.tostr.info(error.error.text);
        } else {
          this.tostr.info('Already In Use');
        }
      },
    });
  }
}


searchGrid(event: any): void {
  const tableElement = this.EmployeeLists.nativeElement;
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

// Leave Type Grid


resetForm(){
  this.EmployeeForm.get('leaveType')?.patchValue(undefined);
  this.EmployeeForm.get('LvAmnt')?.patchValue('');
}

onAdd() {

  let form = this.EmployeeForm.value;

  const leaveTypeExists = this.LeavesEntryList.some((entry) => entry.leaveType === form.leaveType);

  if (leaveTypeExists && this.editModeSno == false) {
    this.tostr.warning('Type already Exists');
    return;
  }


  if (form.leaveType === null || form.leaveType === undefined) {
    this.tostr.warning('Select Leave type....!');
    return;
  }

  if (form.LvAmnt === '' || form.LvAmnt === 0 || form.LvAmnt === "0") {
    this.tostr.warning('Enter Leave Amount');
    return;
  }

  let LeaveName = this.LeaveTypeList.find((i) => i.HrSetupId === form.leaveType);

  form.leaveTypeName = LeaveName.Name;

  if (this.editModeSno) {
    const index = this.LeavesEntryList.findIndex(
      (row) => row.sno === this.editSno
    );

    if (index !== -1) {
      form.sno = this.editSno;
      this.LeavesEntryList[index] = form;
      this.editModeSno = false;
      this.editSno = '';
      this.btnAdd = 'Add'
      this.resetForm();
      return;
    }
  }

  form.sno = this.LeavesEntryList.length + 1;
  this.LeavesEntryList.push(form);
  this.resetForm();
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


editItem(row: any) {

  this.btnAdd = 'Update'

  this.editModeSno = true;
  this.editSno = row.sno;
  
  this.EmployeeForm.get('leaveType')?.patchValue(row.leaveType);
  this.EmployeeForm.get('LvAmnt')?.patchValue(row.LvAmnt);
 
}

deleteItem(row: any) {
  const confirmDelete = confirm('Are you sure you want to delete this item?');

  if (!confirmDelete) {
    return;
  }

  const indexToRemove = this.LeavesEntryList.findIndex(
    (item) => item.sno === row.sno
  );
  if (indexToRemove !== -1) {
    this.LeavesEntryList.splice(indexToRemove, 1);
  }
}


// Report

printReport(item:any){ debugger
  let url = `EmployeeDetailForm?emp_id=${item.empy_id}&comp_id=${this.auth.cmpId()}&LocId=${this.auth.locId()}`

 this.com.viewReport(url);
}

}
