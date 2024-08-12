import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-sub-department-entry',
  templateUrl: './sub-department-entry.component.html',
  styleUrls: ['./sub-department-entry.component.css']
})
export class SubDepartmentEntryComponent {

  constructor(
    private fb: FormBuilder,
    private tostr: ToastrService,
    private apiService: ApiService,
    private dp: DatePipe
  ) {
    const today = new Date();
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  }

  @ViewChild('voucherLists', { static: false }) voucherLists!: ElementRef;

  fromDate:any;
  toDate:any;

  mainDepartmentList: any = [];
  subDepartmentList = [];

  mainDepartMentId: any = undefined;
  subDepartMentId: number = 0;
  subDepartMentName: any;

  isShowBtn:boolean = false;
  readonly:boolean = false;

  btnSave:string = 'Save';


  ngOnInit(){
    this.getMainDepartment();
    this.getSubDepartment();
    this.refreshSubDepartment();
   }

   
  refreshSubDepartment() {
    this.mainDepartMentId = undefined;
    this.subDepartMentId = 0;
    this.subDepartMentName = '';

    this.readonly = true;
    this.isShowBtn = false;
  }

  newSubDepartment() {
    this.refreshSubDepartment();
    
    this.readonly = false;
    this.isShowBtn = true;
    this.btnSave = 'Save';

  }

   getMainDepartment() {
    this.apiService.getData('GeneralStore/GetMainDepartment').subscribe((data) => {
      debugger;
      this.mainDepartmentList = data;
    });
  }


  getSubDepartment() {
    this.apiService.getData('GeneralStore/GetSubDepartment').subscribe((data) => {
      this.subDepartmentList = data;
    });
  }

  
  createUpdateSubDepartment() {

    if (this.mainDepartMentId == undefined) {
      this.tostr.warning('Select Main Department ....!');
      return;
    }

    if (this.subDepartMentName == '') {
      this.tostr.warning('Enter Sub Department Name ....!');
      return;
    }

    const obj = {
      id: this.subDepartMentId,
      name: this.subDepartMentName,
      mainDeptId: this.mainDepartMentId,
    };

    this.apiService
      .saveObj('GeneralStore/AddUpdateSubDepartment', obj)
      .subscribe((result) => {
        if (result == true || result == 'true') {
          this.tostr.success('Save Successfully');
          this.refreshSubDepartment();
          this.getSubDepartment();
        } else {
          this.tostr.error('Please Save Again');
        }
      });
  }

  editSubDepartment(data:any) {
    this.subDepartMentId = data.id;
    this.subDepartMentName = data.name;
    this.mainDepartMentId = data.mainDeptId;
    this.readonly = false;
    this.isShowBtn = true;
    this.btnSave = 'Update';

  }

  deleteSubDepartment(data: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {
      const obj = {
        id: data.id,
        mainDeptId: data.mainDeptId,
      };
      this.apiService.deleteData('GeneralStore/DeleteSubDepartment', obj).subscribe({
        next: (data) => {

          if (data == 'true' || data == true) {
            this.tostr.success('Delete Successfully');
            this.getSubDepartment();
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

  onChangeMainDept(event:any){
    this.mainDepartMentId = event.id;
  }
  onClearMainDept(){
    this.mainDepartMentId = undefined;
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
