import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-lab-test-type',
  templateUrl: './lab-test-type.component.html',
  styleUrls: ['./lab-test-type.component.css']
})
export class LabTestTypeComponent {

  constructor(
    private fb: FormBuilder,
    private tostr: ToastrService,
    private apiService: ApiService,
    private dp: DatePipe,
    private com: CommonService,

  ) {
    const today = new Date();
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  }

  @ViewChild('voucherLists', { static: false }) voucherLists!: ElementRef;

  fromDate: any;
  toDate: any;
  showLoader:boolean = false;

  LabTestTypeList: any = [];
  LabTestNo: number = 0;
  LabTestName: any;
  TestUOM: any;

  isShowBtn: boolean = false;
  readonly: boolean = false;

  btnSave: string = 'Save';



  ngOnInit() {
    this.showLoader = true;
    this.getLabTestType();
    this.refreshLabTestType();
  }

  refreshLabTestType() {
    this.LabTestNo = 0;
    this.LabTestName = '';
    this.TestUOM = '';

    this.readonly = true;
    this.isShowBtn = false;
  }

  newLabTestType() {
    this.refreshLabTestType();

    this.readonly = false;
    this.isShowBtn = true;
    this.btnSave = 'Save';

  }


  getLabTestType() {
    try {

      if (this.showLoader) {
        this.com.showLoader();
      }
      else {
        this.com.hideLoader();
      }
      this.apiService.getData('Purchase/GetLabTestTypeList').subscribe((data) => {
        this.LabTestTypeList = data;

        this.com.hideLoader();
        this.showLoader = false;
      });

    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  createUpdateLabTestType() {
    if (this.LabTestName == '') {
      this.tostr.warning('Enter Main Type Name ....!');
      return;
    }

    try {
      this.com.showLoader();

      const obj = {
        LabTestNo: this.LabTestNo,
        LabTestName: this.LabTestName,
        TestUOM: this.TestUOM

      };

      this.apiService
        .saveObj('Purchase/AddUpdateLabTestType', obj)
        .subscribe((result) => {
          this.com.hideLoader();

          if (result == true || result == 'true') {
            this.tostr.success('Save Successfully');
            this.refreshLabTestType();
            this.getLabTestType();
          } else {
            this.tostr.error('Please Save Again');
          }
        });

    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  editLabTestType(data: any) {
    this.LabTestNo = data.LabTestNo;
    this.LabTestName = data.LabTestName;
    this.TestUOM = data.TestUOM;
    this.readonly = false;
    this.isShowBtn = true;
    this.btnSave = 'Update';

  }

  deleteLabTestType(LabTestNo: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {

      try {
        this.com.showLoader();
        const obj = {
          LabTestNo: LabTestNo,
        };
        this.apiService.deleteData('Purchase/DeleteLabTestType', obj).subscribe({
          next: (data) => {
            this.com.hideLoader();

            if (data == 'true' || data == true) {
              this.getLabTestType();
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

      } catch (err) {
        this.com.hideLoader();
        console.log(err);
      } finally {
        //this.com.hideLoader();
      }
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
