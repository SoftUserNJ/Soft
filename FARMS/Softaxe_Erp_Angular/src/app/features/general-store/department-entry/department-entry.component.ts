import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-department-entry',
  templateUrl: './department-entry.component.html',
  styleUrls: ['./department-entry.component.css']
})
export class DepartmentEntryComponent {
  
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
  mainDepartMentId: number = 0;
  mainDepartMentName: any;

  isShowBtn:boolean = false;
  readonly:boolean = false;

  btnSave:string = 'Save';



 ngOnInit(){
  this.getMainDepartment();
  this.refreshMainDepartment();
 }

  refreshMainDepartment() {
    this.mainDepartMentId = 0;
    this.mainDepartMentName = '';

    this.readonly = true;
    this.isShowBtn = false;
  }

  newMainDepartment() {
    this.refreshMainDepartment();
    
    this.readonly = false;
    this.isShowBtn = true;
    this.btnSave = 'Save';

  }


  getMainDepartment() {
    this.apiService.getData('GeneralStore/GetMainDepartment').subscribe((data) => {
      this.mainDepartmentList = data;
    });
  }

  createUpdateMainDepartment() {
    if (this.mainDepartMentName == '') {
      this.tostr.warning('Enter Main Department Name ....!');
      return;
    }

    const obj = {
      id: this.mainDepartMentId,
      name: this.mainDepartMentName,
    };

    this.apiService
      .saveObj('GeneralStore/AddUpdateMainDepartment', obj)
      .subscribe((result) => {
        if (result == true || result == 'true') {
          this.tostr.success('Save Successfully');
          this.refreshMainDepartment();
          this.getMainDepartment();
        } else {
          this.tostr.error('Please Save Again');
        }
      });
  }

  editMainDepartment(data:any) {
    this.mainDepartMentId = data.id;
    this.mainDepartMentName = data.name;
    this.readonly = false;
    this.isShowBtn = true;
    this.btnSave = 'Update';

  }

  deleteMainDepartment(id: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {
      const obj = {
        id: id,
      };
      this.apiService.deleteData('GeneralStore/DeleteMainDepartment', obj).subscribe({
        next: (data) => {

          if (data == 'true' || data == true) {
            this.getMainDepartment();
            this.tostr.success('Delete Successfully');
          } else if (data == 'false' || data == false) {
            this.tostr.error('Delete Again');
          } else {
            this.tostr.warning('In Used');
          }
        },
        error: (error) => {
          this.tostr.info(error);
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
