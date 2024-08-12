import { Component } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { DatePipe } from '@angular/common';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-balance-sheet',
  templateUrl: './balance-sheet.component.html',
  styleUrls: ['./balance-sheet.component.css'],
})
export class BalanceSheetComponent {
  fromDate: Date;
  toDate: Date;

  constructor(
    private auth: AuthService,
    private dp: DatePipe,
    private com: CommonService
  ) {
    const today = new Date();
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  }

  ngOnInit(): void {}

  openReportModal() {
    const fromDate = this.dp.transform(this.fromDate, 'yyyy/MM/dd');
    const toDate = this.dp.transform(this.toDate, 'yyyy/MM/dd');

    let url = `BalanceSheet?FDate=${fromDate}&TDate=${toDate}&LocId=${this.auth.locId()}&comp_id=${this.auth.cmpId()}`;
    this.com.viewReport(url);
  }
}
