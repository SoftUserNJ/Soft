import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-employee-incentive',
  templateUrl: './employee-incentive.component.html',
  styleUrls: ['./employee-incentive.component.css']
})
export class EmployeeIncentiveComponent {
  constructor(
    private tostr: ToastrService,
    private fb: FormBuilder,
    private apiService: ApiService,
    private dp: DatePipe
  ) {
    
  }

  @ViewChild('voucherLists', { static: false }) voucherLists!: ElementRef;

  EmpIncentiveForm!: FormGroup;
  EmpIncentiveList: any = [];
  isShow = false;
  isReadOnly: boolean = true;
  editMode: boolean = true;
  editSno: any = '';
  editModeSno: boolean = false;
  selectedRow: number | null = null;
  btnAdd:string = 'Add';
  EmployeeList : any[] = [];
  voucherList: any[] = [];
  Disabled:boolean;


  ngOnInit() {

    this.formInit();
    this.disableFields();
    this.getEmployeeList();
  }

  formInit() {
    this.EmpIncentiveForm = this.fb.group({

      Date: [new Date()],
      SrNo: [''],
      empName: [undefined],
      remarks: [''],
      telephone: [''],
      petrol: [''],
      tadafood: [''],
      security: [''],
      maintaince: [''],
      house: [''],
      medical: [''],
      gym: [''],
      family: [''],
      totalIncntive: [''],

    });
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
    this.EmpIncentiveList = [];
    this.disableFields();
  }


  disableFields() {
   this.Disabled = true;
  }



  enableFields() {
    this.Disabled = false;
  }

  
  resetForm() {
    this.EmpIncentiveForm.get('empName')?.patchValue(undefined);
    this.EmpIncentiveForm.get('remarks')?.patchValue('');
    this.EmpIncentiveForm.get('SrNo')?.patchValue('');
    this.EmpIncentiveForm.get('telephone')?.patchValue('');
    this.EmpIncentiveForm.get('petrol')?.patchValue('');
    this.EmpIncentiveForm.get('tadafood')?.patchValue('');
    this.EmpIncentiveForm.get('security')?.patchValue('');
    this.EmpIncentiveForm.get('maintaince')?.patchValue('');
    this.EmpIncentiveForm.get('house')?.patchValue('');
    this.EmpIncentiveForm.get('medical')?.patchValue('');
    this.EmpIncentiveForm.get('gym')?.patchValue('');
    this.EmpIncentiveForm.get('family')?.patchValue('');
    this.EmpIncentiveForm.get('totalIncntive')?.patchValue('');
    this.EmpIncentiveForm.get('Date')?.patchValue(new Date());
  }

  resetFormOnAdd() {
    this.EmpIncentiveForm.get('remarks')?.patchValue('');
    this.EmpIncentiveForm.get('SrNo')?.patchValue('');
    this.EmpIncentiveForm.get('telephone')?.patchValue('');
    this.EmpIncentiveForm.get('petrol')?.patchValue('');
    this.EmpIncentiveForm.get('tadafood')?.patchValue('');
    this.EmpIncentiveForm.get('security')?.patchValue('');
    this.EmpIncentiveForm.get('maintaince')?.patchValue('');
    this.EmpIncentiveForm.get('house')?.patchValue('');
    this.EmpIncentiveForm.get('medical')?.patchValue('');
    this.EmpIncentiveForm.get('gym')?.patchValue('');
    this.EmpIncentiveForm.get('family')?.patchValue('');
    this.EmpIncentiveForm.get('totalIncntive')?.patchValue('');
    this.EmpIncentiveForm.get('Date')?.patchValue(new Date());
  }

  
  // form.totalIncntive = (
  //   (form.telephone || 0) +
  //   (form.petrol || 0) +
  //   (form.tadafood || 0) +
  //   (form.security || 0) +
  //   (form.maintaince || 0) +
  //   (form.house || 0) +
  //   (form.medical || 0) +
  //   (form.gym || 0) +
  //   (form.family || 0)
  // );


  editItem(row: any) {

    this.btnAdd = 'Update'
    this.editMode = true;
    this.editModeSno = true;

    this.EmpIncentiveForm.get('remarks')?.patchValue(row.remarks);
    this.EmpIncentiveForm.get('empName')?.patchValue(row.EmpyId);
    this.EmpIncentiveForm.get('telephone')?.patchValue(row.telephone);
    this.EmpIncentiveForm.get('petrol')?.patchValue(row.petrol);
    this.EmpIncentiveForm.get('tadafood')?.patchValue(row.tadafood);
    this.EmpIncentiveForm.get('security')?.patchValue(row.security);
    this.EmpIncentiveForm.get('maintaince')?.patchValue(row.maintaince);
    this.EmpIncentiveForm.get('house')?.patchValue(row.house);
    this.EmpIncentiveForm.get('medical')?.patchValue(row.medical);
    this.EmpIncentiveForm.get('gym')?.patchValue(row.gym);
    this.EmpIncentiveForm.get('family')?.patchValue(row.family);
    this.EmpIncentiveForm.get('totalIncntive')?.patchValue(row.totalIncntive);
    this.EmpIncentiveForm.get('Date')?.patchValue(row.Date);
    this.EmpIncentiveForm.get('SrNo')?.patchValue(row.srno);
  }


  
onClickSave() {

  const form = this.EmpIncentiveForm.value;

  if(form.empName == null || form.empName == undefined){
    this.tostr.warning('Select Employee');
    return;
  }

  let vchNo = this.editMode ? this.EmpIncentiveForm.get('SrNo')?.value : 0;

  const dataToSave = {
    Srno: vchNo,
      Trdate: this.dp.transform(form.Date ,'yyyy-MM-dd' ),
      remarks: form.remarks,
      Tel: form.telephone || 0,
      Pet: form.petrol || 0,
      Tada: form.tadafood || 0,
      Maint: form.maintaince || 0,
      Other: form.house || 0,
      Gym: form.gym || 0,
      Medical: form.medical || 0,
      Family: form.family || 0,
      Total: form.totalIncntive || 0,
      EmpyId: form.empName,
      Security: form.security || 0
  };





  this.apiService
    .saveData('Payroll/SaveEmpIncentive', dataToSave)
    .subscribe((result) => {
      if (result === true || result === 'true') {
        this.tostr.success('Save Successfully');
        this.onClickRefresh();
      } else {
        this.tostr.error('Please Save Again');
      }
    });
}


  editIncentive(event: any): void {
    this.EmpIncentiveList = [];
    const obj = {
      empy_id: event.empy_id
    };

    this.apiService
      .getDataById('Payroll/GetEditIncentive', obj)
      .subscribe((data) => {

        if (!data || data.length === 0) {
          this.tostr.info("No Records Found");
          this.enableFields();
          return;
        }


        this.enableFields();
        data.forEach((item: any) => {
          let form = item;

          form.empName = item.EmpName;
          form.EmpyId = item.empy_id;
          form.Date = this.dp.transform(item.trdate, 'yyyy-MM-dd');
          form.telephone = item.tel;
          form.SrNo = item.srno;
          form.petrol = item.pet;
          form.tadafood = item.tada;
          form.remarks = item.Remarks;
          form.house = item.other;
          form.totalIncntive = item.total;
          form.gym = item.gym;
          form.medical = item.medical;
          form.family = item.family;
          form.security = item.security;
          form.maintaince = item.maint;

          this.EmpIncentiveList.push(form);
        });
      });
  }

  deleteIncentive(srno:any, EmpyId:any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {
      const obj = {
        empy_id: EmpyId,
        SrNo: srno
      };

      this.apiService.deleteData('Payroll/deleteIncentive', obj).subscribe({
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
