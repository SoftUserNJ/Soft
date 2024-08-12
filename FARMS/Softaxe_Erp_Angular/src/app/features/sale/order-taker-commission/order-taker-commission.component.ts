import { Component } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';
import { DatePipe } from '@angular/common';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-order-taker-commission',
  templateUrl: './order-taker-commission.component.html',
  styleUrls: ['./order-taker-commission.component.css'],
})
export class OrderTakerCommissionComponent {
  fromDate: Date;
  toDate: Date;

  constructor(
    private apiService: ApiService,
    private dp: DatePipe,
    private auth: AuthService,
    private com: CommonService,
  ) {
    const today = new Date();
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  }

  orderTaker = [];
  orderTakerId: any = null;

  onViewReport() {
    const fromDate = this.dp.transform(this.fromDate, 'yyyy/MM/dd');
    const toDate = this.dp.transform(this.toDate, 'yyyy/MM/dd');

    let userId = 0;
    let userId1 = 9999;

    if (this.orderTakerId) {
      userId = this.orderTakerId;
      userId1 = this.orderTakerId;
    }

    let url = `OtCommission?FromDate=${fromDate}&ToDate=${toDate}&FDate=${fromDate}&TDate=${toDate}UserId=${userId}&UserId1=${userId1}&CmpId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}`;
    this.com.viewReport(url);
  }

  ngOnInit(): void {
    this.GetOrderTakers();
  }

  GetOrderTakers() {
    this.apiService.getData('Sale/GetOrderTakerList').subscribe((data) => {
      this.orderTaker = data;
    });
  }
}
