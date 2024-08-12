import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-allow-same-leave',
  templateUrl: './allow-same-leave.component.html',
  styleUrls: ['./allow-same-leave.component.css']
})
export class AllowSameLeaveComponent {
  constructor(
    private tostr: ToastrService,
    private fb: FormBuilder,
    private apiService: ApiService,
    private dp: DatePipe
  )
  {

  }


  SameLeaveForm!: FormGroup;
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

  EmployeeList: any[]= [];

  getEmployeeList() {
    this.apiService.getData('Payroll/GetEmployees').subscribe((data)=>{
      this.EmployeeList = data;
    })
  }


  formInit() 
  { 
    this.SameLeaveForm = this.fb.group({

      empName: [undefined],
      empy_id: [''],

    });
  }

  ngOnInit() 
  {
    this.formInit();
    this.disableFields();
    this.getEmployeeList();

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
    this.SameLeaveForm.get('empName')?.patchValue('');
    this.SameLeaveForm.get('empy_id')?.patchValue('');
  
  }

    onClickNew() {
      this.isShow = true;
      this.enableFields();
    }


    InputEmployee(event:any) {
      const selectedEmployeeId: number = parseInt(event.target.value, 10);
          const selectedEmployee = this.EmployeeList.find(employee => employee.empy_id === selectedEmployeeId);
          if (selectedEmployee) {
            this.SameLeaveForm.patchValue({ empName: selectedEmployee.empy_id });
          }
          else{
            this.tostr.info('No Employee Found');  
          }
    
    }
    
    SelectEmployee(event:any) {
            this.SameLeaveForm.patchValue({ empy_id: event.empy_id });
    }

    onClickLeaves(){

      const empName = this.SameLeaveForm.get('empName').value;
  
      if (empName === null || empName === undefined) {
        this.tostr.warning('Select Employee First');
        return;
      }
  
        const obj = {
          empy_id : empName
        };
  
        this.apiService.getDataById('Payroll/AllowSameLeave', obj).subscribe({
          next: (data) => {
            if (data == 'true' || data == true) {
              this.tostr.success('Applied Successfully');
              this.onClickRefresh();
            } else if (data == 'false' || data == false) {
              this.tostr.error('Apply Again');
            }
          },
          error: (error) => {
            this.tostr.info(error.error.text);
          },
        });
      
    }

    onClickEOBI(){

    }

    
}
