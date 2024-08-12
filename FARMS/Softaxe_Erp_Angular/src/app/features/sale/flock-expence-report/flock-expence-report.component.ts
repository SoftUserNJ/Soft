import { Component } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';
import { DatePipe } from '@angular/common';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-flock-expence-report',
  templateUrl: './flock-expence-report.component.html',
  styleUrls: ['./flock-expence-report.component.css'],
})
export class FlockExpenceReportComponent {
  isLoginPage: boolean = false;
  fromDate: Date;
  toDate: Date;
  locId: any = '%';

  costCentreList: any[] = [];
  costCentreId: any = null;

  farmList: any[] = [];
  row: any;
  farmFilterList: any[] = [];
  isFinished: boolean = false;

  totalQty: any;
  avgRate: any;
  totalAmount: any;
  totalNetProfit: any;

  constructor(
    private apiService: ApiService,
    private dp: DatePipe,
    private com: CommonService,
    private auth: AuthService
  ) {
    const today = new Date();
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  }

  async ngOnInit() {
    //this.cmpLogo = `${this.basePath}/Companies/${this.cmpName}/CompanyLogo/${this.cmpImage}`;
    await this.getFarmRxpese();
    this.getCostCentre();
  }

  getCostCentre() {
    this.apiService.getData('Sale/GetAllCostCentre').subscribe((data) => {
      this.costCentreList = data;
    });
  }

  async getFarmRxpese() {
    try {
      const data = await this.apiService
        .getData('Sale/FarmExpReport')
        .toPromise();
      if (data.length == 0) {
        this.farmList = [];
        return;
      }
      this.row = data[0];
      this.farmList = data;
      this.filterList();
    } catch (err) {
      console.log(err);
    } finally {
    }
  }

  finished(isFin: boolean) {
    this.isFinished = isFin;
    this.filterList();
  }

  filterList() {
    if (this.costCentreId == null) {
      this.farmFilterList = this.farmList.filter(
        (x) => x.FINISHED == this.isFinished
      );
    } else {
      this.farmFilterList = this.farmList.filter(
        (x) =>
          x.FINISHED == this.isFinished && x.COSTCENTREID == this.costCentreId
      );
    }
    this.calculateTotals();
  }

  calculateTotals() {
    this.totalQty = this.farmFilterList.reduce(
      (total: any, i: any) => total + (i.SALEQTY || 0),
      0
    );

    this.totalAmount = this.farmFilterList.reduce(
      (total: any, i: any) => total + (i.SALEAMT || 0),
      0
    );

    this.avgRate = this.totalAmount / this.totalQty;

    this.totalNetProfit = this.farmFilterList.reduce(
      (total: any, i: any) =>
        total +
        (i.CHICKSAMT +
          i.FEEDAMT +
          i.MEDIAMT +
          i.DIESELAMT +
          i.RENTAMT +
          i.SALARIESAMT +
          i.ELECTRICITYAMT +
          i.WOODAMT +
          i.MESSAMT +
          i.SHEDEQUIPMENTAMT +
          i.OTHEREXPENSEAMT) -
        (i.SALEAMT + i.BAGAMT + i.OTHERINCOMEAMT),
      0
    );
  }

  onViewReport(i: any) {
    const fromDate = this.dp.transform(this.fromDate, 'yyyy/MM/dd');
    const toDate = this.dp.transform(this.toDate, 'yyyy/MM/dd');

    let url = `FarmLedger?JobFrom=${i.JOBID}&JobTo=${
      i.JOBID
    }&FromDate=2020/01/01&ToDate=2030/01/01&FDate=2020/01/01&TDate=2030/01/01&LocId=${
      i.LOCID
    }&Comp_id=${this.auth.cmpId()}&FinId=${this.auth.finId()}&JobNo=${
      i.JOBNO
    }&FarmName=${i.COSTCENTRENAME}&LocName=${i.LOCNAME}`;
    this.com.viewReport(url);
  }
}
