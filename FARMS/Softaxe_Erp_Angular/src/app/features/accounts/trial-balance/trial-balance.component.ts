import { ApiService } from 'src/app/services/api.service';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { DatePipe } from '@angular/common';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-trial-balance',
  templateUrl: './trial-balance.component.html',
  styleUrls: ['./trial-balance.component.css'],
})
export class TrialBalanceComponent {
  locName = localStorage.getItem('locName');
  distributionPos = localStorage.getItem('distributionPos');
  obDebit: number = 0;
  obCredit: number = 0;
  debit: number = 0;
  credit: number = 0;
  cbDebit: number = 0;
  cbCredit: number = 0;
  search = '';
  consolidated: boolean = true;
  detailed: boolean = false;
  control: boolean = false;
  trialBalanceList: any[] = [];
  fromDate: Date;
  toDate: Date;

  locationList: any[] = [];
  isDisableLoc: boolean = false;
  locId: any = null;

  outputCode: any;
  outputName: any;

  @ViewChild('trialList', { static: false }) trialList!: ElementRef;

  constructor(
    private apiService: ApiService,
    private dp: DatePipe,
    private auth: AuthService,
    private com: CommonService
  ) {
    const today = new Date();
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  }

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
    this.getTrialBalance();
  }

  openReportModal() {
    const filterBy = this.getSelectedRadio();
    const fromDate = this.dp.transform(this.fromDate, 'yyyy/MM/dd');
    const toDate = this.dp.transform(this.toDate, 'yyyy/MM/dd');

    if (this.locId == null) {
      this.locName = 'Over All';
    } else {
      this.locName = this.locationList.find((x) => x.ID == this.locId).NAME;
    }

    let url = `TrailBalance?FromDate=${fromDate}&ToDate=${toDate}&FDate=${fromDate}&TDate=${toDate}&filterBy=${filterBy}&Names=${
      this.search ?? '%'
    }&compId=${this.auth.cmpId()}&finId=${this.auth.finId()}&locId=${
      this.locId ?? '%'
    }&Verify=%&LocName=${this.locName}`;
    this.com.viewReport(url);
  }

  getTrialBalance() {
    try {
      this.com.showLoader();

      const obj = {
        fromDate: this.dp.transform(this.fromDate, 'yyyy/MM/dd'),
        toDate: this.dp.transform(this.toDate, 'yyyy/MM/dd'),
        filterBy: this.getSelectedRadio(),
        locId: this.locId ?? '%',
      };

      this.apiService
        .getDataById('Accounts/GetTrialBalance', obj)
        .subscribe((data) => {
          this.trialBalanceList = data;
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

  onClickFilter(event: any) {
    this.consolidated = false;
    this.detailed = false;
    this.control = false;

    if (event.target.value == 'Consolidated') {
      this.consolidated = true;
    } else if (event.target.value == 'Detailed') {
      this.detailed = true;
    } else if (event.target.value == 'Control') {
      this.control = true;
    }
    this.getTrialBalance();
  }

  public getSelectedRadio(): string {
    if (this.consolidated) {
      return 'Consolidated';
    } else if (this.control) {
      return 'Control';
    } else if (this.detailed) {
      return 'Detailed';
    } else {
      return '';
    }
  }

  onSearchInput(event: any) {
    this.search = event.target.value;
    this.searchGrid();
  }

  searchGrid(): void {
    const tableElement = this.trialList.nativeElement;
    const rows = tableElement.querySelectorAll('tr');

    let obCredit = 0;
    let obDebit = 0;
    let credit = 0;
    let debit = 0;
    let cbCredit = 0;
    let cbDebit = 0;

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
        obDebit += parseFloat(
          row.querySelector('.obDebit')?.textContent!.replace(/,/g, '')
        );
        obCredit += parseFloat(
          row.querySelector('.obCredit')?.textContent!.replace(/,/g, '')
        );
        debit += parseFloat(
          row.querySelector('.debit')?.textContent!.replace(/,/g, '')
        );
        credit += parseFloat(
          row.querySelector('.credit')?.textContent!.replace(/,/g, '')
        );
        cbDebit += parseFloat(
          row.querySelector('.cbDebit')?.textContent!.replace(/,/g, '')
        );
        cbCredit += parseFloat(
          row.querySelector('.cbCredit')?.textContent!.replace(/,/g, '')
        );
        row.style.display = '';
      } else {
        row.style.display = 'none';
      }
    });

    this.obDebit = obDebit;
    this.obCredit = obCredit;
    this.debit = debit;
    this.credit = credit;
    this.cbDebit = cbDebit;
    this.cbCredit = cbCredit;
  }

  export(fileName: any, formate: any) {
    this.com.ExportFiles(fileName, formate);
  }

  onClickLedger(code: any, name: any) {
    this.outputCode = code;
    this.outputName = name;
  }
}
