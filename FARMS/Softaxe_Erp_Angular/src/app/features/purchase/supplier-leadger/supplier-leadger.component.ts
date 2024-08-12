import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-supplier-leadger',
  templateUrl: './supplier-leadger.component.html',
  styleUrls: ['./supplier-leadger.component.css'],
})
export class SupplierLeadgerComponent {
  @ViewChild('supplierLedgerLists', { static: false })
  supplierLedgerLists!: ElementRef;
  costCenter = localStorage.getItem('costCenter');
  locName = localStorage.getItem('locName');
  fromDate: Date;
  toDate: Date;
  accountCode = '';
  supplierLedgerList: any[] = [];
  locationList: any[] = [];
  JobList: any[] = [];
  filterList: any[] = [];
  locId: any = null;
  jobNo: any = null;
  isDisableLoc: boolean = false;

  outputCode: any;
  outputName: any;

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

  onClickView(tag: string) {
    const fromDate = this.dp.transform(this.fromDate, 'yyyy/MM/dd');
    const toDate = this.dp.transform(this.toDate, 'yyyy/MM/dd');
    const nowDate = this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss');

    if (this.locId == null) {
      this.locName = 'Over All';
    } else {
      this.locName = this.locationList.find((x) => x.ID == this.locId).NAME;
    }

    let url = '';
    this.accountSelected();
    if (tag == 'view') {
      url = `AccountLedger?FromAccountCode=${this.accountCode}&ToAccountCode=${
        this.accountCode
      }&FromDate=${fromDate}&ToDate=${toDate}&FDate=${fromDate}&TDate=${toDate}&FromBnf=0&ToBnf=9999999&LocId=${
        this.locId ?? '%'
      }&Comp_id=${this.auth.cmpId()}&VchrType=%&Verify=%&JobNo=${
        this.jobNo ?? '%'
      }&LocName=${this.locName}`;
    } else if (tag == 'detail') {
      url = `LedgerDetail?FromAccountCode=${this.accountCode}&ToAccountCode=${
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
    this.getCustomerLedgerList();
    if (this.costCenter == 'true') {
      this.JobList = await this.com.getJobList(false);
    }

    this.locationList = await this.com.getLocation();
    if (this.auth.locId() == 'HO') {
      this.locId = this.auth.locId();
      this.isDisableLoc = false;
    } else {
      this.locId = this.auth.locId();
      this.isDisableLoc = false;
    }
    this.onChangeLoc();
  }

  getCustomerLedgerList() {
    try {
      this.com.showLoader();
      this.apiService
        .getData('Purchase/GetSupplierLedger')
        .subscribe((data) => {
          this.supplierLedgerList = data;
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
    const tableElement = this.supplierLedgerLists.nativeElement;
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
    const tableElement = this.supplierLedgerLists.nativeElement;
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

  searchGrid(event: any): void {
    const tableElement = this.supplierLedgerLists.nativeElement;
    const rows = tableElement.querySelectorAll('tr');

    rows.forEach((row: HTMLTableRowElement) => {
      let isShow = true;

      if (isShow) {
        if (
          row.textContent &&
          row.textContent
            .toLowerCase()
            .indexOf(event.target.value.toLowerCase()) > -1
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

  onClickLedger(code: any, name: any){
    this.outputCode = code;
    this.outputName = name;
  }

}
