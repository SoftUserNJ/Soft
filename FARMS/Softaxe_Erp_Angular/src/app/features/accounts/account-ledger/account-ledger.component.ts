import { Component, ElementRef, ViewChild } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';
import { DatePipe } from '@angular/common';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';
@Component({
  selector: 'app-account-ledger',
  templateUrl: './account-ledger.component.html',
  styleUrls: ['./account-ledger.component.css'],
})
export class AccountLedgerComponent {
  costCenter = localStorage.getItem('costCenter');
  locName = localStorage.getItem('locName');
  distributionPos = localStorage.getItem('distributionPos');
  fromDate: Date;
  toDate: Date;
  accountList: any[] = [];
  locationList: any[] = [];
  JobList: any[] = [];
  filterList: any[] = [];
  locId: any = null;
  jobNo: any = null;
  search = '';
  all: boolean = true;
  bank: boolean = false;
  cash: boolean = false;
  other: boolean = false;
  accountCode = '';
  isDisableLoc: boolean = false;

  outputCode: any;
  outputName: any;

  @ViewChild('AccountList', { static: false }) AccountList!: ElementRef;
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

  onClickView(Tage: String) {
    const fromDate = this.dp.transform(this.fromDate, 'yyyy/MM/dd');
    const toDate = this.dp.transform(this.toDate, 'yyyy/MM/dd');
    const nowDate = this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss');
    this.accountSelected();

    if (this.locId == null) {
      this.locName = 'Over All';
    } else {
      this.locName = this.locationList.find((x) => x.ID == this.locId).NAME;
    }

    let jobName = '';
    if (this.jobNo != null) {
      jobName = this.JobList.find((x) => x.ID == this.jobNo).NAME;
    }

    let url = '';

    if (Tage == 'View') {
      url = `AccountLedger?FromAccountCode=${this.accountCode}&ToAccountCode=${
        this.accountCode
      }&FromDate=${fromDate}&ToDate=${toDate}&FDate=${fromDate}&TDate=${toDate}&FromBnf=0&ToBnf=9999999&LocId=${
        this.locId ?? '%'
      }&Comp_id=${this.auth.cmpId()}&VchrType=%&Verify=%&JobNo=${
        this.jobNo ?? '%'
      }&JobName=${jobName}&LocName=${this.locName}`;
    } else {
      url = `Detailledger?FromAccountCode=${this.accountCode}&ToAccountCode=${
        this.accountCode
      }&FromDate=${fromDate}&ToDate=${toDate}&FDate=${fromDate}&TDate=${toDate}&FromBnf=0&ToBnf=9&LocId=${
        this.locId ?? '%'
      }&Comp_id=${this.auth.cmpId()}&VchrType=%&AppOnly=%&Area=%&EntryType=%&Filter=%&ExclEntryType=%&FlockNo=%&LocName=${
        this.locName
      }`;
    }
    this.com.viewReport(url);
  }

  async ngOnInit() {
    this.getAccountList();
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

  getAccountList() {
    try {
      this.com.showLoader();

      this.apiService
        .getData('Accounts/GetAccountLedgerList')
        .subscribe((data) => {
          this.accountList = data;
          this.com.hideLoader();
        });
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  onClickChk(event: any) {
    const tableElement = this.AccountList.nativeElement;
    const rows = tableElement.querySelectorAll('tr');

    if (event.target.checked == true) {
      rows.forEach((row: HTMLTableRowElement) => {
        const checkbox = row.querySelector(
          'input[type="checkbox"]'
        ) as HTMLInputElement;
        checkbox.checked = true;
      });
    } else {
      rows.forEach((row: HTMLTableRowElement) => {
        const checkbox = row.querySelector(
          'input[type="checkbox"]'
        ) as HTMLInputElement;
        checkbox.checked = false;
      });
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

  accountSelected() {
    this.accountCode = '';
    const tableElement = this.AccountList.nativeElement;
    const rows = tableElement.querySelectorAll('tr');
    rows.forEach((row: HTMLTableRowElement) => {
      const checkbox = row.querySelector(
        'input[type="checkbox"]'
      ) as HTMLInputElement;
      if (checkbox.checked) {
        this.accountCode += row.querySelector('.code').textContent + ',';
      }
    });
  }

  onClickFilter(event: any) {
    this.all = false;
    this.bank = false;
    this.cash = false;
    this.other = false;

    if (event.target.value == 'all') {
      this.all = true;
    } else if (event.target.value == 'bank') {
      this.bank = true;
    } else if (event.target.value == 'cash') {
      this.cash = true;
    } else if (event.target.value == 'other') {
      this.other = true;
    }
    this.searchGrid();
  }

  onSearchInput(event: any) {
    this.search = event.target.value;
    this.searchGrid();
  }

  searchGrid(): void {
    const tableElement = this.AccountList.nativeElement;
    const rows = tableElement.querySelectorAll('tr');

    rows.forEach((row: HTMLTableRowElement) => {
      let isShow = true;

      if (this.bank) {
        if (row.querySelector('.tag')?.textContent == 'H1') {
          isShow = true;
        } else {
          isShow = false;
        }
      }

      if (this.cash) {
        if (row.querySelector('.tag')?.textContent == 'H') {
          isShow = true;
        } else {
          isShow = false;
        }
      }

      if (this.other) {
        if (!['H', 'H1'].includes(row.querySelector('.tag')?.textContent!)) {
          isShow = true;
        } else {
          isShow = false;
        }
      }

      if (isShow) {
        if (
          row.textContent &&
          row.textContent.toLowerCase().indexOf(this.search.toLowerCase()) > -1
        ) {
          isShow = true;
        } else {
          isShow = false;
        }
      }
      if (isShow) {
        row.style.display = '';
      } else {
        row.style.display = 'none';
      }
    });
  }

  onClickLedger(code: any, name: any) {
    this.outputCode = code;
    this.outputName = name;
  }
}
