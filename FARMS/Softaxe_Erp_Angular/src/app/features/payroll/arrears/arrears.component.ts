import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-arrears',
  templateUrl: './arrears.component.html',
  styleUrls: ['./arrears.component.css']
})
export class ArrearsComponent {
  constructor(
    private tostr: ToastrService,
    private fb: FormBuilder,
    private apiService: ApiService,
    private dp: DatePipe
  ) {
    
  }

  @ViewChild('voucherLists', { static: false }) voucherLists!: ElementRef;
  voucherList: any[] = [];

  ArrearsForm!: FormGroup;
  ArrearsList: any = [];
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
    this.ArrearsForm = this.fb.group({

      SrNo: [''],
      empName: [undefined],
      remarks: [''],
      Date: [new Date()],
      pfOnArrears: [''],
      amount: [''],
      reference: ['']

    });
  }
  
  onClickRefresh() {
    this.isShow = false;
    this.resetForm();
    this.ArrearsList = [];
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
    this.ArrearsForm.get('empName')?.patchValue(undefined);
    this.ArrearsForm.get('remarks')?.patchValue('');
    this.ArrearsForm.get('SrNo')?.patchValue('');
    this.ArrearsForm.get('reference')?.patchValue('');
    this.ArrearsForm.get('pfOnArrears')?.patchValue('');
    this.ArrearsForm.get('amount')?.patchValue('');
    this.ArrearsForm.get('Date')?.patchValue(new Date());
  }

  resetFormOnAdd() {

    this.ArrearsForm.get('remarks')?.patchValue('');
    this.ArrearsForm.get('SrNo')?.patchValue('');
    this.ArrearsForm.get('pfOnArrears')?.patchValue('');
    this.ArrearsForm.get('reference')?.patchValue('');
    this.ArrearsForm.get('amount')?.patchValue('');
    this.ArrearsForm.get('Date')?.patchValue(new Date());
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
  this.editMode = true
  this.ArrearsForm.get('SrNo')?.patchValue(row.SrNo);
  this.ArrearsForm.get('empName')?.patchValue(row.EmpyId);
  this.ArrearsForm.get('Date')?.patchValue(row.Date);
  this.ArrearsForm.get('reference')?.patchValue(row.reference);
  this.ArrearsForm.get('amount')?.patchValue(row.amount);
  this.ArrearsForm.get('remarks')?.patchValue(row.remarks);
  this.ArrearsForm.get('pfOnArrears')?.patchValue(row.pfOnArrears);
}

deleteItem(row: any) {
  const confirmDelete = confirm('Are you sure you want to delete this item?');

  if (!confirmDelete) {
    return;
  }

  const indexToRemove = this.ArrearsList.findIndex(
    (item) => item.sno === row.sno
  );
  if (indexToRemove !== -1) {
    this.ArrearsList.splice(indexToRemove, 1);
  }
}


onClickSave() {

  const form = this.ArrearsForm.value;

  if(form.empName == null || form.empName == undefined){
    this.tostr.warning('Select Employee');
    return;
  }

  let vchNo = this.editMode ? this.ArrearsForm.get('SrNo')?.value : 0;

  const dataToSave = {
    SrNo: form.sno,
    Stdate: this.dp.transform(form.Date, 'yyyy-MM-dd'),
    Remarks: form.remarks,
    Reference: form.reference,
    EmpyId: form.empName,
    Arrears: form.amount

  };

  this.apiService
    .saveData('Payroll/SaveEmpArrears', dataToSave)
    .subscribe((result) => {
      if (result === true || result === 'true') {
        this.tostr.success('Save Successfully');
        this.onClickRefresh();
      } else {
        this.tostr.error('Please Save Again');
      }
    });
}


editArrears(event: any): void {

  this.ArrearsList = [];
  const obj = {
    empy_id: event.empy_id
  };

  this.apiService
    .getDataById('Payroll/GetEditArrearsList', obj)
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
        form.Date = this.dp.transform(item.stdate, 'yyyy-MM-dd');
        form.reference = item.Reference;
        form.SrNo = item.srno;
        form.amount = item.Arrears;
        form.remarks = item.Remarks;
        form.pfOnArrears = (((item.Arrears/1.1)/10).toFixed(2));
        
        this.ArrearsList.push(form);
      });
    });
}

deleteArrears(srno:any, EmpyId:any): void {
  const confirmDelete = confirm('Are you sure you want to delete this Deduction?');


  if (confirmDelete == true) {

    const obj = {
      empy_id: EmpyId,
      SrNo: srno
    };

    this.apiService.deleteData('Payroll/deleteEmpArrears', obj).subscribe({
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

calculateArrears() {

  var amount = this.ArrearsForm.get('amount').value;
  amount = ((amount/1.1)/10);
  this.ArrearsForm.get('pfOnArrears').setValue(amount.toFixed(2));
}

}
