import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-employee-family',
  templateUrl: './employee-family.component.html',
  styleUrls: ['./employee-family.component.css']
})
export class EmployeeFamilyComponent {

  constructor(
    private tostr: ToastrService,
    private fb: FormBuilder,
    private apiService: ApiService,
    private dp: DatePipe
  ) {
    const today = new Date();
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  }

  @ViewChild('voucherLists', { static: false }) voucherLists!: ElementRef;
  voucherList: any[] = [];
  EmployeeList: any[] = [];
  fromDate: Date;
  toDate: Date;

  EmpFamilyForm!: FormGroup;
  EmpFamilyList: any = [];
  isShow = false;
  isReadOnly: boolean = true;
  editMode: boolean = true;
  editSno: any = '';
  editModeSno: boolean = false;
  selectedRow: number | null = null;
  btnAdd:string = 'Add';
  Disabled:boolean;

  ngOnInit() {

    this.formInit();
    this.getEmployeeList();
    this.disableFields();

  }

  formInit() {
    this.EmpFamilyForm = this.fb.group({
      gender: [undefined],
      cnicNo: [''],
      Id: [''],
      empName: [undefined],
      childName: ['']

    });
  }

  onClickNew() {
    this.isShow = true;
    this.editMode = false;
    this.enableFields();
  }


  // togglePages() {
  //   this.isShowPage = !this.isShowPage;
  //   if (this.isShowPage) {
  //     this.onClickRefresh()
  //   }
  // }

  getEmployeeList() {
    this.apiService.getData('Payroll/GetEmployees').subscribe((data)=>{
      this.EmployeeList = data;
    })
  }

  resetForm() {
    this.EmpFamilyForm.get('empName')?.patchValue(undefined);
    this.EmpFamilyForm.get('childName')?.patchValue('');
    this.EmpFamilyForm.get('cnicNo')?.patchValue('');
    this.EmpFamilyForm.get('gender')?.patchValue(undefined);
  }

  resetFormOnAdd() {

    this.EmpFamilyForm.get('childName')?.patchValue('');
    this.EmpFamilyForm.get('cnicNo')?.patchValue('');
    this.EmpFamilyForm.get('gender')?.patchValue(undefined);
  }

  onAdd() {

    let form = this.EmpFamilyForm.value;

    if (form.empName === null || form.empName === undefined) {
      this.tostr.warning('Select Employee Name....!');
      return;
    }


    if (form.gender === null || form.gender === undefined) {
      this.tostr.warning('Select Gender....!');
      return;
    }

    if (form.childName === '' || form.childName === "0") {
      this.tostr.warning('Enter Child Name');
      return;
    }

    let EmpName = this.EmployeeList.find((i) => i.empy_id === form.empName);
    form.EmpName = EmpName.name;
    form.EmpId = EmpName.empy_id
    
    if (this.editModeSno) {
      const index = this.EmpFamilyList.findIndex(
        (row) => row.sno === this.editSno
      );
      if (index !== -1) {
        form.sno = this.editSno;
        this.EmpFamilyList[index] = form;
        this.editModeSno = false;
        this.editSno = '';
        this.btnAdd = 'Add'
        this.resetFormOnAdd();
        return;
      }
    }

    form.sno = this.EmpFamilyList.length + 1;
    this.EmpFamilyList.push(form);
    this.resetFormOnAdd();
  }

  onClickRefresh() {
    this.isShow = false;
    this.resetForm();
    this.EmpFamilyList = [];
    this.disableFields();
  }

  disableFields() {
    this.Disabled = true;
  }
  
  enableFields() {
    this.Disabled = false;
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
    this.EmpFamilyForm.get('empName')?.patchValue(row.EmpId);
    this.EmpFamilyForm.get('childName')?.patchValue(row.childName);
    this.EmpFamilyForm.get('cnicNo')?.patchValue(row.cnicNo);
    this.EmpFamilyForm.get('gender')?.patchValue(row.gender);
  }

  deleteItem(row: any) {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (!confirmDelete) {
      return;
    }

    const indexToRemove = this.EmpFamilyList.findIndex(
      (item) => item.sno === row.sno
    );
    if (indexToRemove !== -1) {
      this.EmpFamilyList.splice(indexToRemove, 1);
    }
  }


  // getEmpFamilyList() {
  //   this.apiService.getData('EmpFamily/GetEmpFamilyList')
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


  onClickSave() {
    if (this.EmpFamilyList.length == 0) {
      this.tostr.warning('Incomplete Transaction...');
      return;
    }

   // let Id = this.editMode ? this.EmpFamilyForm.get('Id')?.value : 0;


    const voucher: any[] = this.EmpFamilyList.map((data) => ({
      EmpyId: data.EmpId,
      Name: data.childName,
      Cnic: data.cnicNo,
      Gender: data.gender,
      SrNo: data.sno
    }));

    this.apiService
      .saveData('Payroll/AddUpdateEmpFamily', voucher)
      .subscribe((result) => {
        if (result == true || result == 'true') {
          this.tostr.success('Save Successfully');
          this.onClickRefresh();
        } else {
          this.tostr.error('Please Save Again');
        }
      });
  }


  editEmpFamily(event: any): void {
    // this.onClickRefresh();
    // this.isShow = true;
    // this.editMode = true;
    this.EmpFamilyList = [];
    const obj = {
      empy_id: event.empy_id,
    };

    this.apiService
      .getDataById('Payroll/GetEditEmpFamily', obj)
      .subscribe((data) => {

        if (!data || data.length === 0) {
          this.tostr.info("No Records Found");
          this.enableFields();
          return;
        }

        
        this.enableFields();
        data.forEach((item: any) => {


        
          let form = item;
          form.Id = item.Id;
          form.sno = item.SrNo
          form.EmpId = item.empy_id;
          form.EmpName = item.EmpName;
          form.childName = item.Name;
          form.gender = item.Gender;
          form.cnicNo = item.CNIC;
         
          this.EmpFamilyList.push(form);
        });
      });
  }

  deleteEmpFamily(): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');
    var empy_id = this.EmpFamilyForm.get('empName')?.value;

    if (empy_id === null || empy_id === undefined) {
      this.tostr.warning('Select Employee to Delete....!');
      return;
    }

    if (confirmDelete == true) {
      const obj = {
        empy_id: empy_id,
      };

      this.apiService.deleteData('Payroll/DelEmpFamily', obj).subscribe({
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
