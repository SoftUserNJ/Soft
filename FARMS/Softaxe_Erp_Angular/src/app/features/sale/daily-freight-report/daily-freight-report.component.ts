import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-daily-freight-report',
  templateUrl: './daily-freight-report.component.html',
  styleUrls: ['./daily-freight-report.component.css']
})
export class DailyFreightReportComponent {

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