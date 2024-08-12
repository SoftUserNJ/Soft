import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-profit-and-loss-comparison',
  templateUrl: './profit-and-loss-comparison.component.html',
  styleUrls: ['./profit-and-loss-comparison.component.css'],
})
export class ProfitAndLossComparisonComponent {
  costCenter = localStorage.getItem('costCenter');
  locName = localStorage.getItem('locName');
  distributionPos = localStorage.getItem('distributionPos');
  fromDate: Date;
  toDate: Date;
  locationList: any[] = [];
  JobList: any[] = [];
  filterList: any[] = [];
  locId: any = null;
  jobNo: any = null;
  isDisableLoc: boolean = false;
  constructor(
    private auth: AuthService,
    private dp: DatePipe,
    private com: CommonService
  ) {
    const today = new Date();
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  }

  openReportModal() {
    let fromDate = this.dp.transform(this.fromDate, 'yyyy/MM/dd');
    let toDate = this.dp.transform(this.toDate, 'yyyy/MM/dd');
    let jobName = "";

    if (this.locId == null) {
      this.locName = 'Over All';
    } else {
      this.locName = this.locationList.find((x) => x.ID == this.locId).NAME;
    }

    if(this.jobNo != null){
      fromDate = "2000/01/01"
      toDate = "3000/01/01"
      jobName = this.filterList.find((x) => x.ID == this.jobNo).NAME;
    }

    let url = `ProfitLoss?FromDate=${fromDate}&ToDate=${toDate}&FDate=${fromDate}&TDate=${toDate}&CostofSale=${this.auth.cos()}&DiscountonSale=${this.auth.ds()}&OtherCreditSale=${this.auth.os()}&LocId=${
      this.locId ?? '%'
    }&CompId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&Verify=%&JobNo=${
      this.jobNo ?? '%'
    }&JobName=${jobName}&LocName=${this.locName}`;
    this.com.viewReport(url);
  }

  async ngOnInit() {
    if (this.costCenter == 'true') {
      this.JobList = await this.com.getJobList(false);
    }
    this.locationList = await this.com.getLocation();

    if (this.distributionPos != 'ERP') {
      if (this.auth.locId() == 'HO') {
        this.isDisableLoc = false;
      } else {
        this.isDisableLoc = true;
      }

      this.locId = this.auth.locId();
      this.onChangeLoc();
    } else {
      if (this.auth.locId() != 'HO') {
        this.isDisableLoc = true;
        this.locId = this.auth.locId();
      }
    }
  }

  onChangeLoc() {
    this.jobNo = null;
    if (this.locId != null && this.locId != 'HO') {
      this.filterList = this.JobList.filter((x) => x.LOCID == this.locId);
    } else {
      this.filterList = this.JobList;
    }
  }
}
