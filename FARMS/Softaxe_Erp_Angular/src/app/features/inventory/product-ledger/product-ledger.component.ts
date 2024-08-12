import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-product-ledger',
  templateUrl: './product-ledger.component.html',
  styleUrls: ['./product-ledger.component.css'],
})
export class ProductLedgerComponent {
  locName = localStorage.getItem('locName');
  distributionPos = localStorage.getItem('distributionPos');
  fromDate: Date;
  toDate: Date;
  fromDateExp: Date;
  toDateExp: Date;
  accountList: any[] = [];
  locationList: any[] = [];
  categoryList: any[] = [];
  locList: any[] = [];
  accountCode = '';
  location: any = null;
  category: any = null;
  search: any = '';
  isDisableLoc: boolean = false;
  locId: any = null;
  isExpiryRangeChecked: boolean = false;

  @ViewChild('AccountList', { static: false }) AccountList!: ElementRef;
  constructor(
    private apiService: ApiService,
    private dp: DatePipe,
    private toastr: ToastrService,
    private com: CommonService,
    private auth: AuthService
  ) {
    const today = new Date();
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);

    this.fromDateExp = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDateExp = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  }

  onViewReport() {
    if (this.accountList.length == 0) {
      this.toastr.warning('Search Product Name....');
      return;
    }

    this.accountSelected();

    const fromDate = this.dp.transform(this.fromDate, 'yyyy/MM/dd');
    const toDate = this.dp.transform(this.toDate, 'yyyy/MM/dd');
    let exFromDate = '2000/01/01';
    let exToDate = '3000/01/01';

    if (this.isExpiryRangeChecked) {
      exFromDate = this.dp.transform(this.fromDateExp, 'yyyy/MM/dd');
      exToDate = this.dp.transform(this.toDateExp, 'yyyy/MM/dd');
    }

    const r = this.locationList.find((x) => x.SHELFID == this.location);

    if (this.locId == null) {
      this.locName = 'Over All';
    } else {
      this.locName = this.locList.find((x) => x.ID == this.locId).NAME;
    }

    let url = `ProductLedger?FromAccountCode=${
      this.accountCode
    }&ToAccountCode=${
      this.accountCode
    }&FromDate=${fromDate}&ToDate=${toDate}&FDate=${fromDate}&TDate=${toDate}&FromBnf=0&ToBnf=9999999&LocId=${
      this.locId ?? '%'
    }&CompId=${this.auth.cmpId()}&VchrType=%&Verify=%&ExpiryFromDate=${exFromDate}&ExpiryToDate=${exToDate}&GodownId=${
      this.location ?? '%'
    }&LocationSku=${r == undefined ? '' : r.LOCATION}&LocName=${this.locName}`;

    this.com.viewReport(url);
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

  async ngOnInit() {
    this.locList = await this.com.getLocation();
    this.getCategory();

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

  }

  getCategory() {
    this.apiService.getData('Inventory/GetCategory').subscribe((result) => {
      this.categoryList = result;
    });
  }

  onChangeCategory() {
    this.accountList = [];
    this.locationList = [];
    this.location = null;
    this.search = '';

    if (this.category == null) {
      return;
    }
    this.getProducts('%', this.category);
  }

  onSearchInput() {
    if (this.category == null) {
      if (this.search.length == 0) {
        this.accountList = [];
        this.locationList = [];
        this.location = null;
        return;
      }

      if (this.search.length > 3) {
        this.getProducts(this.search, 0);
      }
    } else {
      const tableElement = this.AccountList.nativeElement;
      const rows = tableElement.querySelectorAll('tr');

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
          row.style.display = '';
        } else {
          row.style.display = 'none';
        }
      });
    }
  }

  getProducts(name: string, category: number) {
    this.apiService
      .getDataById('Inventory/GetProductLedger', {
        name: name,
        category: category,
      })
      .subscribe((data) => {
        this.accountList = data;
      });
  }

  onClickChk(event: any) {
    const tableElement = this.AccountList.nativeElement;
    const rows = tableElement.querySelectorAll('tr');
    this.locationList = [];
    this.location = null;

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

  onClickAccount(event: any, code: any) {
    const tableElement = this.AccountList.nativeElement;
    const rows = tableElement.querySelectorAll('tr');

    let isTrue = event.target.checked;

    rows.forEach((row: HTMLTableRowElement) => {
      const checkbox = row.querySelector(
        'input[type="checkbox"]'
      ) as HTMLInputElement;
      checkbox.checked = false;
    });

    if (isTrue == true) {
      event.target.checked = true;
      this.GetProductLocation(code);
      this.accountCode = code;
    } else {
      event.target.checked = false;
    }
  }

  GetProductLocation(code: any) {
    this.apiService
      .getDataById('Inventory/GetProductLocation', { code: code })
      .subscribe((data) => {
        this.location = null;
        this.locationList = data;
      });
  }
}
