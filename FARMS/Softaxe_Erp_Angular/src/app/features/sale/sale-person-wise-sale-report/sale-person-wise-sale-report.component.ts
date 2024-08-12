import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-sale-person-wise-sale-report',
  templateUrl: './sale-person-wise-sale-report.component.html',
  styleUrls: ['./sale-person-wise-sale-report.component.css']
})
export class SalePersonWiseSaleReportComponent {

  fromDate: Date;
  toDate: Date;
  today: Date;
  
  constructor(
    private apiService: ApiService,
    private dp: DatePipe
  ) {
    const today = new Date();
    this.today = today;
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  }

}
