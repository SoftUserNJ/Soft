import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-print-voucher-range-wise',
  templateUrl: './print-voucher-range-wise.component.html',
  styleUrls: ['./print-voucher-range-wise.component.css'],
})
export class PrintVoucherRangeWiseComponent {
  fromDate: Date;
  toDate: Date;
  isDateDisable: boolean = true;

  fromNo: any = '';
  toNo: any = '';
  isVchNoDisable: boolean = false;

  vchList: any[] = [];
  vchType: any = null;

  locationList: any[] = [];
  locId: any = null;

  constructor(
    private auth: AuthService,
    private dp: DatePipe,
    private tostr: ToastrService,
    private com: CommonService,
    private apiService: ApiService
  ) {
    const today = new Date();
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  }

  async ngOnInit() {
    this.getVchTypes();
    this.locationList = await this.com.getLocation();
    this.locId = this.auth.locId();
  }

  getVchTypes() {
    this.apiService.getData('Accounts/GetVchTypes').subscribe((data) => {
      this.vchList = data;
    });
  }

  onClickNumber() {
    this.isDateDisable = true;
    this.isVchNoDisable = false;
  }

  onClickDate() {
    this.isDateDisable = false;
    this.isVchNoDisable = true;
    this.fromNo = '';
    this.toNo = '';
  }

  onClickPrint() {
    let vchType: any = '';
    let fromDate: any = '2000/01/01';
    let toDate = '3000/01/01';
    let fromNo = '0';
    let toNo = '999999999';

    if (this.isDateDisable == true) {
      fromNo = this.fromNo;
      toNo = this.toNo;

      if (fromNo == '') {
        this.tostr.warning('Enter From Number...!');
        return;
      }
      if (toNo == '') {
        this.tostr.warning('Enter To Number...!');
        return;
      }

      if (fromNo > toNo) {
        this.tostr.warning('From Number & To Number is Not Correct...!');
        return;
      }

      fromDate = '2000/01/01';
      toDate = '3000/01/01';
    }

    if (this.isVchNoDisable == true) {
      fromDate = this.dp.transform(this.fromDate, 'yyyy/MM/dd');
      toDate = this.dp.transform(this.toDate, 'yyyy/MM/dd');

      if (this.fromDate == undefined) {
        this.tostr.warning('Select From Date...!');
        return;
      }

      if (this.toDate == undefined) {
        this.tostr.warning('Select To Date...!');
        return;
      }

      if (this.fromDate > this.toDate) {
        this.tostr.warning('From Date & To Date Selection is Wrong...!');
        return;
      }

      fromNo = '0';
      toNo = '999999999';
    }

    if (this.vchType == null) {
      this.vchList.forEach((x) => {
        vchType += x.vchtype + ',';
      });
    } else {
      vchType = this.vchType;
    }

    let url = `PrintVoucherRangeWise?DateFrom=${fromDate}&DateTo=${toDate}&VchType=${vchType}&VchNoFrom=${fromNo}&VchNoTo=${toNo}&CompId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${
      this.locId ?? '%'
    }`;
    this.com.viewReport(url);
  }
}
