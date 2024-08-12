import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-print-booking-report',
  templateUrl: './print-booking-report.component.html',
  styleUrls: ['./print-booking-report.component.css']
})
export class PrintBookingReportComponent {
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
