import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-leaves-entry',
  templateUrl: './leaves-entry.component.html',
  styleUrls: ['./leaves-entry.component.css']
})
export class LeavesEntryComponent {
  constructor(
    private tostr: ToastrService,
    private fb: FormBuilder,
    private apiService: ApiService,
    private dp: DatePipe
  ) {
    
  }

  @ViewChild('voucherLists', { static: false }) voucherLists!: ElementRef;

  LeavesEntryForm!: FormGroup;
  LeavesEntryList: any = [];
  isShow = false;
  isReadOnly: boolean = true;
  editMode: boolean = true;
  editSno: any = '';
  editModeSno: boolean = false;
  selectedRow: number | null = null;
  btnAdd:string = 'Add';
  EmployeeList : any[] = [];
  voucherList: any[] = [];
  LeaveTypeList: any[] = [];
  LeavesRemainingList: any[] = [];
  Disabled:boolean;

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
    this.getLeaveTypeList();

    this.LeavesEntryForm.get('StDate').valueChanges.subscribe(() => this.calculateDays());
    this.LeavesEntryForm.get('EndDate').valueChanges.subscribe(() => this.calculateDays());
  }

  formInit() {
    this.LeavesEntryForm = this.fb.group({
      Date: [new Date()],
      StDate: [new Date()],
      EndDate: [new Date()],
      SrNo: [''],
      empName: [undefined],
      leaveType: [undefined],
      remarks: [''],
      noOfDays: [''],
      totalLeaves: [''],
    });
  }

  getEmployeeList() {
    this.apiService.getData('Payroll/GetEmployees').subscribe((data)=>{
      this.EmployeeList = data;
    })
  }

  getLeaveTypeList() {
    this.apiService.getData('Payroll/GetLeaveType').subscribe((data)=>{
      this.LeaveTypeList = data;
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
    this.LeavesEntryList = [];
    this.LeavesRemainingList = [];
    this.disableFields();
  }


  disableFields() {
    this.Disabled = true;
  }



  enableFields() {
    this.Disabled = false;
  }

  
  resetForm() {
    this.LeavesEntryForm.get('empName')?.patchValue(undefined);
    this.LeavesEntryForm.get('remarks')?.patchValue('');
    this.LeavesEntryForm.get('SrNo')?.patchValue('');
    this.LeavesEntryForm.get('totalLeaves')?.patchValue('');
    this.LeavesEntryForm.get('noOfDays')?.patchValue('');
    this.LeavesEntryForm.get('leaveType')?.patchValue('');
    this.LeavesEntryForm.get('EndDate')?.patchValue(new Date());
    this.LeavesEntryForm.get('Date')?.patchValue(new Date());
    this.LeavesEntryForm.get('StDate')?.patchValue(new Date());
  }

  resetFormOnAdd() {
    this.LeavesEntryForm.get('remarks')?.patchValue('');
    this.LeavesEntryForm.get('SrNo')?.patchValue('');
    this.LeavesEntryForm.get('totalLeaves')?.patchValue('');
    this.LeavesEntryForm.get('noOfDays')?.patchValue('');
    this.LeavesEntryForm.get('leaveType')?.patchValue('');
    this.LeavesEntryForm.get('EndDate')?.patchValue(new Date());
    this.LeavesEntryForm.get('Date')?.patchValue(new Date());
    this.LeavesEntryForm.get('StDate')?.patchValue(new Date());
  }




  editItem(row: any) {

    this.btnAdd = 'Update'
    this.editMode = true;
    this.editModeSno = true;

    this.LeavesEntryForm.get('remarks')?.patchValue(row.remarks);
    this.LeavesEntryForm.get('empName')?.patchValue(row.EmpyId);
    this.LeavesEntryForm.get('totalLeaves')?.patchValue(row.totalLeaves);
    this.LeavesEntryForm.get('noOfDays')?.patchValue(row.noOfDays);
    this.LeavesEntryForm.get('leaveType')?.patchValue(row.leaveType);
    this.LeavesEntryForm.get('Date')?.patchValue(row.Date);
    this.LeavesEntryForm.get('EndDate')?.patchValue(row.EndDate);
    this.LeavesEntryForm.get('StDate')?.patchValue(row.StDate);
    this.LeavesEntryForm.get('SrNo')?.patchValue(row.srno);
  }


    
onClickSave() {

  if (this.LeavesRemainingList.length == 0) {
    this.tostr.warning('No Leaves Allowed For this Employee...');
    return;
  }


  const form = this.LeavesEntryForm.value;

  if(form.empName == null || form.empName == undefined){
    this.tostr.warning('Select Employee');
    return;
  }

  if(form.leaveType == null || form.leaveType == undefined){
    this.tostr.warning('Select Leave Type');
    return;
  }

  if(form.totalLeaves == null || form.totalLeaves == 0){
    this.tostr.warning('Enter Leave Days');
    return;
  }

  let LeaveId = this.LeavesRemainingList.find((i) => i.lv_id === form.leaveType);

  let Total = LeaveId.originalNoOfLvs - form.noOfDays;
  if(Total < 0){
    this.tostr.warning('Number of Leaves Not Allowed');
    return;
  }

  let vchNo = this.editMode ? this.LeavesEntryForm.get('SrNo')?.value : 0;

  const dataToSave = {
    Vch: 'LV-VCH',
    SrNo: vchNo,
    StDate: this.dp.transform(form.StDate, 'yyyy-MM-dd'),
    EndDate: this.dp.transform(form.EndDate, 'yyyy-MM-dd'),
    Date: this.dp.transform(form.Date, 'yyyy-MM-dd'),
    Remarks: form.remarks,
    Nod: form.noOfDays,
    EmpyId: form.empName,
    TotalLeaves: form.totalLeaves,
    LvId : form.leaveType
  };





  this.apiService
    .saveData('Payroll/SaveLeavesEntry', dataToSave)
    .subscribe((result) => {
      if (result === true || result === 'true') {
        this.tostr.success('Save Successfully');
        this.onClickRefresh();
      } else {
        this.tostr.error('Please Save Again');
      }
    });
}

  editLeaves(event: any): void {
    this.LeavesEntryList = [];
    this.LeavesRemainingList = [];


    const obj = {
      empy_id: event.empy_id,
    };

    this.apiService
      .getDataById('Payroll/GetEditEmpLeaves', obj)
      .subscribe((data) => {
        
        this.enableFields();
        data.EmpData.forEach((item: any) => { 

          let form = item;
          form.empName = item.EmpName;
          form.EmpyId = item.empy_id;
          form.Date = this.dp.transform(item.Date, 'yyyy-MM-dd');
          form.EndDate = this.dp.transform(item.EndDate, 'yyyy-MM-dd');
          form.StDate = this.dp.transform(item.stDate, 'yyyy-MM-dd');
          form.noOfDays = item.NOD;
          form.SrNo = item.srno;
          form.leaveType = item.Lv_id;
          form.LeaveName = item.LeaveName;
          form.totalLeaves = item.TotalLeaves;
          form.remarks = item.Remarks;

          this.LeavesEntryList.push(form);
          
        });

        data.LeaveData.forEach((item: any) => { 

          let form = item;
          form.LvName = item.LvName;
          form.lv_id = item.lv_id;
          form.Total = item.Total;
          form.originalNoOfLvs = item.Total;

          this.LeavesRemainingList.push(form);
          
        });


      });
  }

  deleteLeaves(srno:any, EmpyId:any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {
      const obj = {
        empy_id: EmpyId,
        SrNo: srno
      };

      this.apiService.deleteData('Payroll/DelLeaves', obj).subscribe({
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

  calculateDays() {
    const startDate = this.LeavesEntryForm.get('StDate').value;
    const endDate = this.LeavesEntryForm.get('EndDate').value;

    if (startDate && endDate) {
      const start = new Date(startDate);
      const end = new Date(endDate);

      const diffTime = Math.abs(end.getTime() - start.getTime());
      const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));

      this.LeavesEntryForm.get('noOfDays').setValue(diffDays);
      this.LeavesEntryForm.get('totalLeaves').setValue(diffDays);
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
