import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-salary-settlement-labels',
  templateUrl: './salary-settlement-labels.component.html',
  styleUrls: ['./salary-settlement-labels.component.css']
})
export class SalarySettlementLabelsComponent {
  constructor(
    private tostr: ToastrService,
    private fb: FormBuilder,
    private apiService: ApiService,
    private dp: DatePipe
  )
  {

  }

  SettlementLablesForm!: FormGroup;
  SettlementLablesList: any = [];
  isShow = false;
  isReadOnly: boolean = true;
  editMode: boolean = true;
  isShowPage: boolean = true;
  editSno: any = '';
  editModeSno: boolean = false;
  selectedRow: number | null = null;
  btnAdd:string = 'Add';
  Disabled:boolean;

  @ViewChild('voucherLists', { static: false }) voucherLists!: ElementRef;
  voucherList: any[] = [];

  ngOnInit() 
  { 
    this.formInit();
    this.getSalaryLables();
    this.disableFields();
  }


  formInit() 
  { 
    this.SettlementLablesForm = this.fb.group({
      labelCode: [''],
      allwnce1: [''],
      allwnce1Perc: [''],
      allwnce2: [''],
      allwnce2Perc: [''],
      allwnce3: [''],
      allwnce3Perc: [''],
      allwnce4: [''],
      allwnce4Perc: [''],
      allwnce5: [''],
      allwnce5Perc: [''],
      allwnce6: [''],
      allwnce6Perc: [''],
      allwnce7: [''],
      allwnce7Perc: [''],

    });
  }

  onClickNew() {
    this.isShow = true;
    this.editMode = false;
    this.enableFields();
  }

  disableFields() {
    this.Disabled = true;
  }



  enableFields() {
    this.Disabled = false;
  }

  getSalaryLables() {
    this.apiService.getData('Payroll/GetSalaryLabels').subscribe((data) => {
      this.SettlementLablesForm.get('labelCode')?.setValue(data[0].lableCode);
      this.SettlementLablesForm.get('allwnce1')?.setValue(data[0].lbl1);
      this.SettlementLablesForm.get('allwnce2')?.setValue(data[0].lbl2);
      this.SettlementLablesForm.get('allwnce3')?.setValue(data[0].lbl3);
      this.SettlementLablesForm.get('allwnce4')?.setValue(data[0].lbl4);
      this.SettlementLablesForm.get('allwnce5')?.setValue(data[0].lbl5);
      this.SettlementLablesForm.get('allwnce6')?.setValue(data[0].lbl6);
      this.SettlementLablesForm.get('allwnce7')?.setValue(data[0].lbl7);
      this.SettlementLablesForm.get('allwnce1Perc')?.setValue(data[0].p1);
      this.SettlementLablesForm.get('allwnce2Perc')?.setValue(data[0].p2);
      this.SettlementLablesForm.get('allwnce3Perc')?.setValue(data[0].p3);
      this.SettlementLablesForm.get('allwnce4Perc')?.setValue(data[0].p4);
      this.SettlementLablesForm.get('allwnce5Perc')?.setValue(data[0].p5);
      this.SettlementLablesForm.get('allwnce6Perc')?.setValue(data[0].p6);
      this.SettlementLablesForm.get('allwnce7Perc')?.setValue(data[0].p7);
    });
  }

  onClickSave() {
  
    const form = this.SettlementLablesForm.value;

    if (!form.allwnce1) {
      this.tostr.warning('Enter At Least 1 Level....!');
      return;
    }
    
    if (!form.allwnce1Perc) {
      this.tostr.warning('Enter At Least 1 Value....!');
      return;
    }

    if(form.labelCode == ""  || form.labelCode == 0) {
      form.labelCode = 1
    }

    //const labelCode = this.editMode ? form.labelCode : 0;
  
    const dataToSave = {
      LableCode: form.labelCode,
      Lbl1: form.allwnce1,
      Lbl2: form.allwnce2,
      Lbl3: form.allwnce3,
      Lbl4: form.allwnce4,
      Lbl5: form.allwnce5,
      Lbl6: form.allwnce6,
      Lbl7: form.allwnce7,
      P1: form.allwnce1Perc,
      P2: form.allwnce2Perc,
      P3: form.allwnce3Perc,
      P4: form.allwnce4Perc,
      P5: form.allwnce5Perc,
      P6: form.allwnce6Perc,
      P7: form.allwnce7Perc,
    };
  
    this.apiService
      .saveData('Payroll/SaveLabels', dataToSave)
      .subscribe((result) => {
        if (result === true || result === 'true') {
          this.tostr.success('Save Successfully');
          this.getSalaryLables();
          this.disableFields();
          this.isShow = false;
        } else {
          this.tostr.error('Please Save Again');
        }
      });
  }


  
}
