import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';
@Component({
  selector: 'app-job-wise-due-status',
  templateUrl: './job-wise-due-status.component.html',
  styleUrls: ['./job-wise-due-status.component.css'],
})
export class JobWiseDueStatusComponent {
  @ViewChild('costList', { static: false }) costList!: ElementRef;
  fromDate: Date;
  toDate: Date;
  party = '';
  jonNo: any = '';
  costCentre: any = '';
  farm: any = '';
  balanceList: any[] = [];
  totalOpening: any;
  totalDebit: any;
  totalCredit: any;
  totalClosing: any;

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
    this.getBalanceList();
  }

  getBalanceList() {
    try {
      this.com.showLoader();
      const obj = {
        fromDate: this.datePipe.transform(this.fromDate, 'yyyy/MM/dd'),
        toDate: this.datePipe.transform(this.toDate, 'yyyy/MM/dd'),
      };

      this.apiService
        .getDataById('Sale/GetJobDueStatus', obj)
        .subscribe((data) => {
          this.balanceList = data;
          setTimeout(() => {
            this.searchGrid();
          }, 100);
          this.com.hideLoader();
        });
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
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

      if (isShow) {
        const jobNo = row
          .querySelector('.jobNo')
          ?.textContent?.toLocaleLowerCase();
        if (
          jobNo !== null &&
          jobNo !== undefined &&
          jobNo.indexOf(this.jonNo.toLowerCase()) >= 0
        ) {
          isShow = true;
        } else {
          isShow = false;
        }
      }

      if (isShow) {
        const costCentre = row
          .querySelector('.costCentre')
          ?.textContent?.toLocaleLowerCase();
        if (
          costCentre !== null &&
          costCentre !== undefined &&
          costCentre.indexOf(this.costCentre.toLowerCase()) >= 0
        ) {
          isShow = true;
        } else {
          isShow = false;
        }
      }
      if (isShow) {
        const farmName = row
          .querySelector('.farmName')
          ?.textContent?.toLocaleLowerCase();
        if (
          farmName !== null &&
          farmName !== undefined &&
          farmName.indexOf(this.farm.toLowerCase()) >= 0
        ) {
          isShow = true;
        } else {
          isShow = false;
        }
      }
      if (isShow) {
        const party = row
          .querySelector('.party')
          ?.textContent?.toLocaleLowerCase();
        if (
          party !== null &&
          party !== undefined &&
          party.indexOf(this.party.toLowerCase()) >= 0
        ) {
          isShow = true;
        } else {
          isShow = false;
        }
      }
      // if (
      //   row.textContent &&
      //   row.textContent.toLowerCase().indexOf(this.party.toLowerCase()) > -1
      // ) {
      //   isShow = true;
      // } else {
      //   isShow = false;
      // }

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

    let url = `AccountLedger?FromAccountCode=${i.CODE}&ToAccountCode=${
      i.CODE
    }&FromDate=${fromDate}&ToDate=${toDate}&FDate=${fromDate}&TDate=${toDate}&FromBnf=0&ToBnf=9999999&LocId=${
      i.LOCID
    }&Comp_id=${this.auth.cmpId()}&VchrType=%&Verify=%&JobNo=${
      i.JOBID
    }&LocName=${i.COSTCENTRE}`;
    this.com.viewReport(url);
  }

  export(fileName: any, formate: any) {
    this.com.ExportFiles(fileName, formate);
  }
}
