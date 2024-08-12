import { Component } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';
import { environment } from 'src/environment/environmemt';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-flock-performance',
  templateUrl: './flock-performance.component.html',
  styleUrls: ['./flock-performance.component.css'],
})
export class FlockPerformanceComponent {

  basePath = environment.basePath;
  cmpId = localStorage.getItem('cmpId');
  cmpImage: any = localStorage.getItem('Logo');
  cmpName: any = localStorage.getItem('CmpName');
  cmpLogo: string = '';

  jobNo: any;
  isLoginPage: boolean = false;
  expList: any[] = []
  saleList: any[] = []

  totalWtKg: any;

  row: any;
  totalExp: any;
  totalExpCostBird: any;
  totalExpCostKg: any;
  totalExpPercent: any;
  totalSale: any;
  totalSalePercent: any;
  totalProfitLoss: any;
  liveability: any;
  fcr: any;
  ebi: any;

  //LEDGER
  fromDate: Date;
  toDate: Date;
  outputCode: any;
  outputName: any;

  constructor(
    private apiService: ApiService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
  ) {}

  async ngOnInit() {
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
    this.activatedRoute.queryParams.subscribe((params) => {
      this.jobNo = params.jobNo;
    });

    this.cmpLogo = `${this.basePath}/Companies/${this.cmpName}/CompanyLogo/${this.cmpImage}`;
    this.getPerformanceDetail();
  }

  getPerformanceDetail() {
    try {
      this.apiService
        .getDataById('Sale/PerformanceReport', { jobNo: this.jobNo })
        .subscribe((data) => {
          if(data.length == 0){
            return;
          }
          this.row = data[0];
          this.expList = data.filter((x) => x.HEAD == 'EXPENSE');
          this.saleList = data.filter((x) => x.HEAD == 'SALE');
          this.calculateTotals()
        });
    } catch (err) {
      console.log(err);
    } finally {
    }
  }

  calculateTotals() {
    this.totalWtKg = ((this.row.TOTALCHICKS - this.row.MORTALITY) * this.row.AVGWEIGHT) / 1000;

    this.totalExp = this.expList.reduce(
      (total: any, item: any) => total + (item.AMOUNT || 0),
      0
    );
    
    this.totalExpCostBird = this.expList.reduce(
      (total: any, item: any) => total + (item.AMOUNT / this.row.TOTALCHICKS || 0),
      0
    );

    this.totalExpCostKg = this.expList.reduce(
      (total: any, item: any) => total + (item.AMOUNT / this.totalWtKg || 0),
      0
    );

    this.totalExpPercent = this.expList.reduce(
      (total: any, item: any) => total + (item.AMOUNT / this.totalExp * 100 || 0),
      0
    );

    this.totalSale = this.saleList.reduce(
      (total: any, item: any) => total + (item.AMOUNT || 0),
      0
    );
    
    this.totalSalePercent = this.saleList.reduce(
      (total: any, item: any) => total + (item.AMOUNT / this.totalSale * 100 || 0),
      0
    );

    this.totalProfitLoss = (this.totalExp - this.totalSale);

    this.liveability = (this.row.TOTALCHICKS - this.row.MORTALITY) / this.row.TOTALCHICKS * 100;
    this.fcr = this.totalWtKg / this.row.TOTALFEED
    debugger;
    this.ebi = ((this.row.AVGWEIGHT / 41.79) * (this.liveability)) / ((50 / this.fcr) * 10);
  }

  onClickLedger(code: any, name: any){
    this.outputCode = code;
    this.outputName = name;
    this.fromDate = new Date(2020, 0, 1);
    this.toDate = new Date(2030, 0, 1);
  }
}
