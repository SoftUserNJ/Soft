import { DatePipe } from '@angular/common';
import { Component, ViewChild } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-overtime-formula',
  templateUrl: './overtime-formula.component.html',
  styleUrls: ['./overtime-formula.component.css']
})
export class OvertimeFormulaComponent {


  constructor(
    private tostr: ToastrService,
    private fb: FormBuilder,
    private apiService: ApiService,
    private dp: DatePipe
  )
  {

  }

  ngOnInit() 
  { 
    this.getOvertimeFormula();
    this.formuladisable = true;
    this.Checkadisable = true;
  }

  formula: string = '';
  Id: number = 0;
  isShow = false;
  formuladisable: boolean;
  Checkadisable: boolean;

  gSalary: boolean;
  sd: boolean;
  
  onClickNew() {
    this.isShow = true;
    this.formuladisable = false
    this.Checkadisable = false
  }


  updateFormula(value: string, checkbox: HTMLInputElement): void {
    const formulaInput = document.getElementById('formulaInput') as HTMLInputElement;
    const currentFormula = formulaInput.value;
  
    if (checkbox.checked) {
      formulaInput.value = currentFormula + value;
    } else {
      formulaInput.value = currentFormula.replace(value, '');
    }
  }

  onClickSave() {
  
    if (!this.formula) {
      this.tostr.warning('Enter Formula to Save....!');
      return;
    }

    if(this.Id == 0) {
      this.Id = 1
    }



    const dataToSave = {
      Id: this.Id,
      Formula: this.formula,
    };
  
    this.apiService
      .saveData('Payroll/SaveOvertimeFormula', dataToSave)
      .subscribe((result) => {
        if (result === true || result === 'true') {
          this.tostr.success('Save Successfully');
          this.getOvertimeFormula();
          this.formuladisable = true;
          this.Checkadisable = true;
          this.isShow = false;
        } else {
          this.tostr.error('Please Save Again');
        }
      });
  }
  
  getOvertimeFormula() {
    this.apiService.getData('Payroll/GetOvertimeFormula').subscribe((data) => {
      
      if (!data || data.length === 0) {
        this.tostr.info("No Formula Found");
        return;
      }

      this.Id= data[0].id;
      this.formula=  data[0].formula;

    });
  }
}
