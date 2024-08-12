import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-sale-print-inv-range-wise',
  templateUrl: './sale-print-inv-range-wise.component.html',
  styleUrls: ['./sale-print-inv-range-wise.component.css']
})
export class SalePrintInvRangeWiseComponent {

  fromDate: Date;
  toDate: Date;
  isDateDisable: boolean = true;

  fromNo: any = '';
  toNo: any = '';
  isVchNoDisable: boolean = false;

  vchType: any = 'SP';

  //this.com.viewReport(url);
  constructor(
    private auth: AuthService,
    private dp: DatePipe,
    private tostr: ToastrService,
    private com: CommonService,
  ) {
    const today = new Date();
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
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
    let fromDate;
    let toDate;
    let fromNo;
    let toNo;

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

    let url = "";

    if (this.vchType == 'SP') {
      url = `SaleInvoice?VchNoFrom=${fromNo}&VchNoTo=${toNo}&VchType=${this.vchType}&fromDate=${fromDate}&toDate=${toDate}&CmpId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}`;
    } else if(this.vchType == 'SR') {
      url = `SaleReturn?VchNoFrom=${fromNo}&VchNoTo=${toNo}&VchType=${this.vchType}&fromDate=${fromDate}&toDate=${toDate}&CmpId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}`;
    }
    
    this.com.viewReport(url);
  }
}
