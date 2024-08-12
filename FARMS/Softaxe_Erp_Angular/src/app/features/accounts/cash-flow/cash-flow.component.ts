import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-cash-flow',
  templateUrl: './cash-flow.component.html',
  styleUrls: ['./cash-flow.component.css'],
})
export class CashFlowComponent {
  costCenter = localStorage.getItem('costCenter');
  locName = localStorage.getItem('locName');
  fromDate: Date;
  toDate: Date;
  locationList: any[] = [];
  JobList: any[] = [];
  filterList: any[] = [];
  locId: any = null;
  jobNo: any = null;
  isDisableLoc: boolean = false;
  bankCash: any = 'H,H1';

  constructor(
    private auth: AuthService,
    private dp: DatePipe,
    private com: CommonService
  ) {
    const today = new Date();
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  }

  changeTag(tag: any) {
    if (tag == 'both') {
      this.bankCash = 'H,H1';
    }

    if (tag == 'cash') {
      this.bankCash = 'H';
    }

    if (tag == 'bank') {
      this.bankCash = 'H1';
    }
  }

  openReportModal() {
    let fromDate = this.dp.transform(this.fromDate, 'yyyy/MM/dd');
    let toDate = this.dp.transform(this.toDate, 'yyyy/MM/dd');

    let url = `CashFlow?fromDate=${fromDate}&toDate=${toDate}&FDate=${fromDate}&TDate=${toDate}&BankCash=${
      this.bankCash
    }&locId=${
      this.locId ?? '%'
    }&cmpId=${this.auth.cmpId()}&FinId=${this.auth.finId()}`;
    this.com.viewReport(url);
  }

  viewCashBankStatus() {
    let fromDate = this.dp.transform(this.fromDate, 'yyyy/MM/dd');
    let toDate = this.dp.transform(this.toDate, 'yyyy/MM/dd');

    let url = `CashFlowSubRpt?fromDate=${fromDate}&toDate=${toDate}&FDate=${fromDate}&TDate=${toDate}&BankCash=${
      this.bankCash
    }&locId=${
      this.locId ?? '%'
    }&cmpId=${this.auth.cmpId()}&finId=${this.auth.finId()}`;
    this.com.viewReport(url);
  }
}
