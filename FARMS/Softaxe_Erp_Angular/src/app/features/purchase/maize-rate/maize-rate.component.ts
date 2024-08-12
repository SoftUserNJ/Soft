import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-maize-rate',
  templateUrl: './maize-rate.component.html',
  styleUrls: ['./maize-rate.component.css']
})
export class MaizeRateComponent {

  
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

  MaizeRateList: any = [];
  LabTestNo: number = 0;
  LabTestName: any;
  TestUOM: any;

  isShowBtn: boolean = false;
  readonly: boolean = false;

  btnSave: string = 'Save';

  ItemList: any = [];
  UOMList: any = [];
  Item: any;
  VchNo: number;
  FromDate: any;
  ToDate: any;
  Rate: any;
  Moisture: any;
  uom: any;


  


  ngOnInit() {
    this.showLoader = true;
    this.getMaizeItem();
    this.getUOM();
    this.getMaizeRateList();
    this.refresh();
  }

  getMaizeItem() {
    this.apiService.getData('Purchase/GetMaizeItem').subscribe((data) => {
      this.ItemList = data;
    })
  }
  getUOM() {
    this.apiService.getData('Common/GetUom').subscribe((data) => {
      this.UOMList = data;
    })
  }

  getMaizeVchNo() {
    this.apiService.getData('Purchase/GetMaizeVchNo').subscribe((data) => {
      this.VchNo = data[0].VchNo;
    })
  }

  refresh() {
    this.getMaizeVchNo();
    this.Item = undefined;
    this.uom = undefined;
    this.FromDate = new Date();
    this.ToDate = new Date();
    this.Rate = '';
    this.Moisture = '';

    this.readonly = true;
    this.isShowBtn = false;
  }

  newMaizeRate() {
    this.refresh();

    this.readonly = false;
    this.isShowBtn = true;
    this.btnSave = 'Save';

  }


  getMaizeRateList() {
    try {

      if (this.showLoader) {
        this.com.showLoader();
      }
      else {
        this.com.hideLoader();
      }
      this.apiService.getData('Purchase/GetMaizeRateList').subscribe((data) => {
        this.MaizeRateList = data;

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

  createUpdateMaizeRate() {
    if (this.Item == undefined) {
      this.tostr.warning('Select Item ....!');
      return;
    }

    if (this.Rate == '') {
      this.tostr.warning('Enter Rate ....!');
      return;
    }

    if (this.uom == undefined) {
      this.tostr.warning('Select UOM ....!');
      return;
    }

    try {
      this.com.showLoader();

      const obj = {
        vchno: this.VchNo,
        itemCode: this.Item,
        Moisture: this.Moisture,
        uom: this.uom,
        FromDate: this.dp.transform(this.fromDate, 'yyyy-MM-dd'),
        ToDate: this.dp.transform(this.ToDate, 'yyyy-MM-dd'),
        Rate: this.Rate

      };

      this.apiService
        .saveObj('Purchase/AddUpdateMaizeRate', obj)
        .subscribe((result) => {
          this.com.hideLoader();

          if (result == true || result == 'true') {
            this.tostr.success('Save Successfully');
            this.refresh();
            this.getMaizeRateList();
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

  editMaizeRate(data: any) {
    this.VchNo = data.VchNo;
    this.Item = data.ItemCode;
    this.FromDate = data.FromDate;
    this.ToDate = data.ToDate;
    this.Rate = data.Rate;
    this.Moisture = data.Moisture;

    this.readonly = false;
    this.isShowBtn = true;
    this.btnSave = 'Update';

  }

  deleteMaizeRate(VchNo: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {

      try {
        this.com.showLoader();
        const obj = {
          VchNo: VchNo,
        };
        this.apiService.deleteData('Purchase/DeleteMaizeRate', obj).subscribe({
          next: (data) => {
            this.com.hideLoader();

            if (data == 'true' || data == true) {
              this.getMaizeRateList();
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
