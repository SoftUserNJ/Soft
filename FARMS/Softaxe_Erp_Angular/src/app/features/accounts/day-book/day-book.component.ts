import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-day-book',
  templateUrl: './day-book.component.html',
  styleUrls: ['./day-book.component.css'],
})
export class DayBookComponent {
  locName = localStorage.getItem('locName');
  isDayClose = localStorage.getItem('dayClose');
  distributionPos = localStorage.getItem('distributionPos');
  date: Date = new Date();
  locationList: any[] = [];
  locId: any = null;
  isDisableLoc: boolean = false;
  isDisableDate: boolean = false;
  constructor(
    private apiService: ApiService,
    private auth: AuthService,
    private dp: DatePipe,
    private com: CommonService
  ) {}

  async ngOnInit() {
    this.locationList = await this.com.getLocation();

    if (this.distributionPos != 'ERP') {
      if (this.auth.locId() == 'HO') {
        this.isDisableLoc = false;
      } else {
        this.isDisableLoc = true;
      }

      this.locId = this.auth.locId();
    } else {
      if (this.auth.locId() != 'HO') {
        this.isDisableLoc = true;
        this.locId = this.auth.locId();
      }
    }

    if (this.isDayClose == 'true') {
      this.isDisableDate = true;
      this.getDaybookDate();
    }
  }

  getDaybookDate() {
    this.apiService
      .getDataById('Utilities/LastCloseDate', { locId: this.locId })
      .subscribe((data) => {
        const date = data[0].DATE.split('/');
        this.date = new Date(date[2], date[1] - 1, date[0]);
      });
  }

  openReportModal(action: string) {
    const date = this.dp.transform(this.date, 'yyyy/MM/dd');

    if (this.locId == null) {
      this.locName = 'Over All';
    } else {
      this.locName = this.locationList.find((x) => x.ID == this.locId).NAME;
    }

    let url = '';
    if (action === 'cash') {
      url = `DayBook1?FromDate=${date}&ToDate=${date}&CloseDate=${date}&VchType=CR,CP,CP-FREIGHT&CompId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${
        this.locId ?? '%'
      }&LocName=${this.locName}`;
    } else if (action === 'bank') {
      url = `DayBook1?FromDate=${date}&ToDate=${date}&CloseDate=${date}&VchType=BR,BP&Closed=&CompId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${
        this.locId ?? '%'
      }&LocName=${this.locName}`;
    } else if (action === 'close') {
      url = `DayBook1?FromDate=${date}&ToDate=${date}&CloseDate=${date}&VchType=CR,CP,BR,BP,CP-FREIGHT&Closed=***CLOSED***&CompId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${
        this.locId ?? '%'
      }&LocName=${this.locName}`;
      this.saveDayClose();
    }
    this.com.viewReport(url);
  }

  saveDayClose() {
    let date = new Date(
      this.date.getFullYear(),
      this.date.getMonth(),
      this.date.getDate() + 1
    );

    var obj = {
      dayClose: this.dp.transform(date, 'yyyy/MM/dd'),
      dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      status: 'Close',
      locId: this.locId,
    };

    this.apiService
      .saveObj('Utilities/SaveUpdateDayClose', obj)
      .subscribe((data) => {
        if (data == true || data == 'true') {
          this.getDaybookDate();
        } else {
        }
      });
  }
}
