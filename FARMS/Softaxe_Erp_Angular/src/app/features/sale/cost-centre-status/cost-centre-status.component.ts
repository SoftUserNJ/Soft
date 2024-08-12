import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-cost-centre-status',
  templateUrl: './cost-centre-status.component.html',
  styleUrls: ['./cost-centre-status.component.css'],
})
export class CostCentreStatusComponent {
  @ViewChild('costList', { static: false }) costList!: ElementRef;
  fromDate: Date;
  toDate: Date;
  search = '';
  jobList: any[] = [];
  jobNoListFilter: any[] = [];
  totalOpening: any;
  totalDebit: any;
  totalCredit: any;
  totalClosing: any;
  filterBy: any = 'Started';

  constructor(
    private apiService: ApiService,
    private datePipe: DatePipe,
    private auth: AuthService,
    private com: CommonService
  ) {
    const today = new Date();
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  }

  async ngOnInit() {

    this.getCostCentre();
  }

  onClickFilter(event: any) {
    this.filterBy = event.target.value;
    this.finishedFilter();
  }

  finishedFilter() {
    if (this.filterBy == 'All') {
      this.jobNoListFilter = this.jobList;
    }

    if (this.filterBy == 'Started') {
      this.jobNoListFilter = this.jobList.filter(
        (x) =>
          x.FINISHED != true
      );
    }

    if (this.filterBy == 'Finished') {
      this.jobNoListFilter = this.jobList.filter(
        (x) =>
          x.FINISHED == true
      );
    }

    setTimeout(() => {
      this.searchGrid();
    }, 50);
  }

  getCostCentre() {
    try {
      this.com.showLoader();
      const obj = {
        fromDate: this.datePipe.transform(this.fromDate, 'yyyy/MM/dd'),
        toDate: this.datePipe.transform(this.toDate, 'yyyy/MM/dd'),
      };

      this.apiService
        .getDataById('Sale/CostCentreStatus', obj)
        .subscribe((data) => {
          this.jobList = data;
          this.finishedFilter();
          this.com.hideLoader();
        });
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  onSearchInput(event: any) {
    this.search = event.target.value;
    this.searchGrid();
  }

  searchGrid(): void {
    const tableElement = this.costList.nativeElement;
    const rows = tableElement.querySelectorAll('tr');

    this.totalOpening = 0;
    this.totalDebit = 0;
    this.totalCredit = 0;
    this.totalClosing = 0;

    rows.forEach((row: HTMLTableRowElement) => {
      let isShow = true;

      if (
        row.textContent &&
        row.textContent.toLowerCase().indexOf(this.search.toLowerCase()) > -1
      ) {
        isShow = true;
      } else {
        isShow = false;
      }

      if (isShow) {
        this.totalOpening += parseFloat(
          row.querySelector('.opBal')?.textContent!.replace(/,/g, '')
        );
        this.totalDebit += parseFloat(
          row.querySelector('.debit')?.textContent!.replace(/,/g, '')
        );
        this.totalCredit += parseFloat(
          row.querySelector('.credit')?.textContent!.replace(/,/g, '')
        );
        this.totalClosing += parseFloat(
          row.querySelector('.clBal')?.textContent!.replace(/,/g, '')
        );

        row.style.display = '';
      } else {
        row.style.display = 'none';
      }
    });
  }

  onViewReport(i: any) {
    const fromDate = this.datePipe.transform(this.fromDate, 'yyyy/MM/dd');
    const toDate = this.datePipe.transform(this.toDate, 'yyyy/MM/dd');

    let url = `FarmLedger?JobFrom=${i.ID}&JobTo=${
      i.ID
    }&FromDate=${fromDate}&ToDate=${toDate}&FDate=${fromDate}&TDate=${toDate}&LocId=${i.LOCID}&Comp_id=${this.auth.cmpId()}&FinId=${this.auth.finId()}&JobNo=${
      i.JOBNO
    }&FarmName=${i.FARMNAME}&LocName=${i.COSTCENTRE}`;
    this.com.viewReport(url);
  }

  rptFlockPerformance(i: any) {
    let url = `RptFlockPerformance?Id=${i.ID}&FarmName=${i.FARMNAME}&Compid=${this.auth.cmpId()}`;
    this.com.viewReport(url);
  }


  export(fileName: any, formate: any) {
    this.com.ExportFiles(fileName, formate);
  }
}
