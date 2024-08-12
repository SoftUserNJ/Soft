import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-overtime',
  templateUrl: './overtime.component.html',
  styleUrls: ['./overtime.component.css']
})
export class OvertimeComponent {
  constructor(
    private tostr: ToastrService,
    private fb: FormBuilder,
    private apiService: ApiService,
    private dp: DatePipe
  ) {
    
  }

  @ViewChild('voucherLists', { static: false }) voucherLists!: ElementRef;

  EmpOverTimeForm!: FormGroup;
  EmpOverTimeList: any = [];
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
  }

  formInit() {
    this.EmpOverTimeForm = this.fb.group({
      Date: [new Date()],
      SrNo: [''],
      empName: [undefined],
      remarks: [''],
      perHourRate: [''],
      totalHours: [''],
      totalAmnt: [''],
      totalSubHours: [''],
      totalSubAmnt: [''],
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
    this.EmpOverTimeList = [];
    this.disableFields();
  }


  disableFields() {
    this.Disabled = true;
  }



  enableFields() {
    this.Disabled = false;
  }

  
  resetForm() {
    this.EmpOverTimeForm.get('empName')?.patchValue(undefined);
    this.EmpOverTimeForm.get('remarks')?.patchValue('');
    this.EmpOverTimeForm.get('SrNo')?.patchValue('');
    this.EmpOverTimeForm.get('perHourRate')?.patchValue('');
    this.EmpOverTimeForm.get('totalHours')?.patchValue('');
    this.EmpOverTimeForm.get('totalAmnt')?.patchValue('');
    this.EmpOverTimeForm.get('totalSubAmnt')?.patchValue('');
    this.EmpOverTimeForm.get('totalSubHours')?.patchValue('');
    this.EmpOverTimeForm.get('Date')?.patchValue(new Date());
  }


CalculateAmount() { 
  var perHour =   this.EmpOverTimeForm.get('perHourRate')?.value;
  var totalHour =  this.EmpOverTimeForm.get('totalHours')?.value;
  this.EmpOverTimeForm.get('totalAmnt')?.patchValue(totalHour * perHour);
}


    calculateTotalHours() {
      const totalHoursSum = this.EmpOverTimeList.reduce(
        (sum, emp) => sum + (emp.totalHours || 0),
        0
      );
      const totalAmntSum = this.EmpOverTimeList.reduce(
        (sum, emp) => sum + (emp.totalAmnt || 0),
        0
      );
      this.EmpOverTimeForm.get('totalSubHours').setValue(totalHoursSum);
      this.EmpOverTimeForm.get('totalSubAmnt').setValue(totalAmntSum);
    }

  editItem(row: any) {

    this.btnAdd = 'Update'
    this.editMode = true;
    this.editModeSno = true;
    this.EmpOverTimeForm.get('remarks')?.patchValue(row.remarks);
    this.EmpOverTimeForm.get('empName')?.patchValue(row.EmpyId);
    this.EmpOverTimeForm.get('perHourRate')?.patchValue(row.perHourRate);
    this.EmpOverTimeForm.get('totalHours')?.patchValue(row.totalHours);
    this.EmpOverTimeForm.get('totalAmnt')?.patchValue(row.totalAmnt);
    this.EmpOverTimeForm.get('Date')?.patchValue(row.Date);
    this.EmpOverTimeForm.get('SrNo')?.patchValue(row.srno);
  }

  onClickSave() {

    const form = this.EmpOverTimeForm.value;
  
    if (!form.amount) {
      this.tostr.warning('Enter Loan Amount');
      return;
    }
  
    if(form.empName == null || form.empName == undefined){
      this.tostr.warning('Select Employee');
      return;
    }
  
    let vchNo = this.editMode ? this.EmpOverTimeForm.get('SrNo')?.value : 0;
  
    const dataToSave = {
      SrNo: vchNo,
      Stdate: this.dp.transform(form.Date, 'yyyy-MM-dd'),
      remarks: form.remarks,
      TotalHrs: form.totalHours,
      EmpyId: form.empName,
      OverTimeAmount: form.totalAmnt,
      PerHourRate: form.perHourRate
    };
  
  
    this.apiService
      .saveData('Payroll/SaveEmployeeOvertime', dataToSave)
      .subscribe((result) => {
        if (result === true || result === 'true') {
          this.tostr.success('Save Successfully');
          this.onClickRefresh();
        } else {
          this.tostr.error('Please Save Again');
        }
      });
  }


  editOvertime(event: any): void {
    this.EmpOverTimeList = [];


    const obj = {
      empy_id: event.empy_id,
    };

    this.apiService
      .getDataById('Payroll/GetEditOvertime', obj)
      .subscribe((data) => {
        if(data.Gross.length === 0){
          this.tostr.info("Please Enter Gross Salary to Calculate");
          this.EmpOverTimeForm.get('perHourRate').setValue('');
          return;
        }

        if(data.SalarDays.length === 0){
          this.tostr.info("Please Enter Salary Days to Calculate");
          this.EmpOverTimeForm.get('perHourRate').setValue('');
          return;
        }
        
        if(data.Formula.length === 0){
          this.tostr.info("Please Enter Formula to Calculate");
          this.EmpOverTimeForm.get('perHourRate').setValue('');
          return;
        }

        this.EmpOverTimeForm.get('perHourRate').setValue((this.calculatePerHourRate(data)).toFixed(2));
        
        if (!data.EmpData || data.EmpData.length === 0) {
          this.tostr.info("No Records Found");
          this.enableFields();
          this.calculateTotalHours();
          return;
        }

        this.enableFields();
        data.EmpData.forEach((item: any) => {

          let form = item;
          form.empName = item.EmpName;
          form.EmpyId = item.empy_id;
          form.Date = this.dp.transform(item.stDate, 'yyyy-MM-dd');
          form.amount = item.loanamt;
          form.SrNo = item.srno;
          form.totalAmnt = item.OverTimeAmount;
          form.perHourRate = item.PerHourRate;
          form.remarks = item.Remarks;
          form.totalHours = item.TotalHrs;

          this.EmpOverTimeList.push(form);
          
        });

        this.calculateTotalHours();
      });
  }

  calculatePerHourRate(data: any): number {
    const formula = data.Formula[0].formula;
  
    const replacedFormula = formula
      .replace('GSALARY', data.Gross[0].gsalary)
      .replace('SD',data.SalarDays[0].salaryDays);
  
    try {
      return eval(replacedFormula);
    } catch (error) {
      console.error('Error evaluating formula:', error);
      return 0;
    }
  }

  deleteOvertime(EmpyId:any, srno:any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {
      const obj = {
        empy_id: EmpyId,
        SrNo: srno
      };

      this.apiService.deleteData('Payroll/DelOvertime', obj).subscribe({
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
