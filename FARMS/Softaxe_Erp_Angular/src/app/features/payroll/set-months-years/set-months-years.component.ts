import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-set-months-years',
  templateUrl: './set-months-years.component.html',
  styleUrls: ['./set-months-years.component.css']
})
export class SetMonthsYearsComponent {
  constructor(
    private tostr: ToastrService,
    private fb: FormBuilder,
    private apiService: ApiService,
    private dp: DatePipe
  )
  {

  }


  MonthYearsForm!: FormGroup;
  isShow = false;
  isReadOnly: boolean = true;
  editMode: boolean = true;
  isShowPage: boolean = true;
  editSno: any = '';
  editModeSno: boolean = false;
  selectedRow: number | null = null;
  btnAdd:string = 'Add';
  YearList:number[] = [];
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


  formInit() 
  { 
    this.MonthYearsForm = this.fb.group({

      month: [undefined],
      year: [undefined],
      finId: [''],

    });
  }

  ngOnInit() 
  { 
    this.formInit();
    this.generateYears();
    this.disableFields();
    this.getMonthYear();
  }


  generateYears() {
   
    const currentYear = new Date().getFullYear();

    for (let year = currentYear; year <= 2050; year++) {
      this.YearList.push(year);
    }
  }

  disableFields() {
   this.Disabled = true;
  }

  enableFields() {
    this.Disabled = false;
  }

  onClickRefresh() {
    this.isShow = false;
    this.resetForm();
    this.disableFields();
  }

  resetForm() {
    this.MonthYearsForm.get('month')?.patchValue('');
    this.MonthYearsForm.get('year')?.patchValue('');
  
  }

    onClickNew() {
      this.isShow = true;
      this.enableFields();
    }

    onClickSave() {
    
      const form = this.MonthYearsForm.value;
    
      if (form.year === null || form.year === undefined) {
        this.tostr.warning('Select Year....!');
        return;
      }

      if (form.month === null || form.month === undefined) {
        this.tostr.warning('Select Month....!');
        return;
      }

      if(form.finId == 0 || form.finId == undefined || form.finId == null) {
        form.finId= 1
      }

    
      const dataToSave = {
        mnth: form.month,
        year: form.year,
        finID: form.finId
      };
    
      this.apiService
        .saveData('Payroll/SaveMonthYear', dataToSave)
        .subscribe((result) => {
          if (result === true || result === 'true') {
            this.tostr.success('Save Successfully');
            this.onClickRefresh();
            this.getMonthYear();
          } else {
            this.tostr.error('Please Save Again');
          }
        });
    }

    getMonthYear() {
      this.apiService.getData('Payroll/GetMonthYear').subscribe((data)=>{
        this.MonthYearsForm.get('month').setValue(data[0].mnth);
        this.MonthYearsForm.get('year').setValue(data[0].year);
        this.MonthYearsForm.get('finId').setValue(data[0].finID);
      });
  }
  
}
