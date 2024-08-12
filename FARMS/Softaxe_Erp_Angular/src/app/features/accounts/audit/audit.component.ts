import { Component, ViewChild, ElementRef } from '@angular/core';
import { DatePipe } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-audit',
  templateUrl: './audit.component.html',
  styleUrls: ['./audit.component.css'],
})
export class AuditComponent {
  @ViewChild('approvalVchLists', { static: false })
  approvalVchLists!: ElementRef;
  @ViewChild('vchDetailLists', { static: false }) vchDetailLists!: ElementRef;
  costCenter = localStorage.getItem('costCenter')
  isVerify: boolean = false;
  isDateWise: boolean = true;
  totalDebit: number = 0;
  totalCredit: number = 0;
  selectedLocation: string;
  searchVchType = '';
  vchNoSearch = '';
  searchTable = '';
  fromDate: Date;
  toDate: Date;
  approvalVchList: any[] = [];
  vchTypeList: any[] = [];
  vchDetailList: any[] = [];
  topMultiData: any[] = [];

  usersList: any[] = [];
  userId: any = null;

  locationList: any[] = [];
  locId: any;
  isDisableLoc: boolean = false;

  constructor(
    private apiService: ApiService,
    private tostr: ToastrService,
    private dp: DatePipe,
    private auth: AuthService,
    private com: CommonService
  ) {
    const today = new Date();
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date();
  }

  async ngOnInit() {
    this.locationList = await this.com.getLocation();
    if (this.auth.locId() == 'HO') {
      this.isDisableLoc = false;
    } else {
      this.isDisableLoc = true;
    }
    this.locId = this.auth.locId();
    this.getVchApproval();
    this.getVchTypes();
    this.getTopMultiData();
    this.getUserList();
  }

  getUserList() {
    this.apiService
      .getDataById('Auth/GetUsersList', { locId: (this.locId == 'HO') ? '%' : this.locId })
      .subscribe((data) => {
        this.usersList = data;
      });
  }

  openReport(item: any) {
    let d = item.VCHDATE.split('/');
    const fromDate = this.dp.transform(
      new Date(d[2], d[1] - 1, d[0]),
      'yyyy/MM/dd'
    );
    const toDate = this.dp.transform(
      new Date(d[2], d[1] - 1, d[0]),
      'yyyy/MM/dd'
    );

    let url = `PrintVoucherRangeWise?DateFrom=${fromDate}&DateTo=${toDate}&VchType=${
      item.VCHTYPE
    }&VchNoFrom=${item.VCHNO}&VchNoTo=${
      item.VCHNO
    }&CompId=${this.auth.cmpId()}&FinId=${item.FINID}&LocId=${item.LOCID}`;

    this.com.viewReport(url);
  }

  onClickFilter(event: any) {
    this.isVerify = false;

    if (event.target.value == 'unapprove') {
      this.isVerify = false;
    } else if (event.target.value == 'approve') {
      this.isVerify = true;
    }
    this.getVchApproval();
  }

  toggleSelectAll(event: any) {
    for (const item of this.approvalVchList) {
      item.VOUCHER = event.target.checked ? 1 : 0;
    }
  }

  getVchApproval() {
    let fromDate;
    let toDate;

    if (this.isDateWise) {
      fromDate = this.dp.transform(this.fromDate, 'yyyy/MM/dd');
      toDate = this.dp.transform(this.toDate, 'yyyy/MM/dd');
    } else {
      fromDate = '2001/01/01';
      toDate = '2050/12/30';
    }

    const status = this.isVerify == false ? 'unapprove' : 'approve';
    const tag = 'audit';

    const obj = {
      fromDate: fromDate,
      toDate: toDate,
      status: status,
      tag: tag,
      locId: this.locId,
      userId: this.userId,
    };

    this.apiService
      .getDataById('Accounts/GetVchApproval', obj)
      .subscribe((data) => {
        this.approvalVchList = data;
        setTimeout(() => {
          this.searchGrid();
        }, 10);
      });
  }

  getVchDetail(item: any) {
    const dp = item.VCHDATE.split('/');

    const obj = {
      vchType: item.VCHTYPE,
      vchNo: item.VCHNO,
      locId: item.LOCID,
      vchDate: this.dp.transform(
        new Date(dp[2], dp[1] - 1, dp[0]),
        'yyyy/MM/dd'
      ),
    };

    this.apiService
      .getDataById('Accounts/GetVchDetail', obj)
      .subscribe((data) => {
        this.vchDetailList = data;
        this.calculateTotals();
      });
  }

  calculateTotals() {
    this.totalDebit = this.vchDetailList.reduce(
      (total: any, item: any) => total + (item.DEBIT || 0),
      0
    );

    this.totalCredit = this.vchDetailList.reduce(
      (total: any, item: any) => total + (item.CREDIT || 0),
      0
    );
  }

  getVchTypes() {
    this.apiService
      .getDataById('Accounts/GetAllowVchType', { tag: 'audit' })
      .subscribe((data) => {
        this.vchTypeList = data;
      });
  }

  getTopMultiData() {
    this.apiService.getData('Accounts/TopMultiData').subscribe((data) => {
      this.topMultiData = data.location.map((location) => ({
        locId: location.LocId,
        locName: location.LocName,
        combinedLabel: `${location.LocId} - ${location.LocName}`,
      }));
      this.selectedLocation = this.topMultiData[0].combinedLabel;
    });
  }

  onClickSave() {
    let mylist;
    if (this.isVerify == false) {
      mylist = this.approvalVchList.filter((x: any) => x.VOUCHER == true);
    } else {
      mylist = this.approvalVchList.filter((x: any) => x.VOUCHER == false);
    }

    try {
      this.com.showLoader();
      const vchVerify: any[] = mylist.map((data: any) => ({
        voucher: this.isVerify == false ? 1 : 0,
        vchType: data.VCHTYPE,
        vchNo: data.VCHNO,
        locId: data.LOCID,
        finId: data.FINID,
        tag: 'audit',
        dtNow: new Date(),
      }));

      this.apiService
        .saveData('Accounts/SaveVouchersApproval', vchVerify)
        .subscribe((result) => {
          if (result == true || result == 'true') {
            this.com.hideLoader();
            this.tostr.success('Save Successfully');
          } else {
            this.com.hideLoader();
            this.tostr.error('Please Save Again');
          }
          this.getVchApproval();
        });
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    }
  }

  onChangeVchType(event: any) {
    if (event == undefined) {
      this.searchVchType = '';
      return;
    }

    this.searchVchType = event.VCHTYPE.trim();
    this.searchGrid();
  }

  onClearVchType() {
    this.searchVchType = '';
    this.searchGrid();
  }

  onSearchInput(event: any) {
    this.vchNoSearch = event.target.value;
    this.searchGrid();
  }

  searchGrid() {
    const tableElement = this.approvalVchLists.nativeElement;
    const rows = tableElement.querySelectorAll('tr');

    rows.forEach((row: HTMLTableRowElement) => {
      let isShow = true;

      if (
        row.querySelector('.vchType')?.textContent != this.searchVchType &&
        this.searchVchType.length > 0
      ) {
        isShow = false;
      }

      if (
        row.querySelector('.vchNo')?.textContent != this.vchNoSearch &&
        this.vchNoSearch.length > 0
      ) {
        isShow = false;
      }

      if (isShow) {
        row.style.display = '';
      } else {
        row.style.display = 'none';
      }
    });
  }

  onInput(event: any) {
    this.searchTable = event.target.value;
    this.searchTableGrid();
  }

  searchTableGrid() {
    const tableElement = this.vchDetailLists.nativeElement;
    const rows = tableElement.querySelectorAll('tr');

    let debit = 0;
    let credit = 0;

    rows.forEach((row: HTMLTableRowElement, index: number) => {
      let isShow = true;

      if (index === rows.length - 1) {
        return;
      }

      if (
        row.textContent &&
        row.textContent.toLowerCase().indexOf(this.searchTable.toLowerCase()) >
          -1
      ) {
        isShow = true;
      } else {
        isShow = false;
      }

      if (isShow) {
        debit += parseFloat(
          row.querySelector('.debit')?.textContent!.replace(/,/g, '')
        );
        credit += parseFloat(
          row.querySelector('.credit')?.textContent!.replace(/,/g, '')
        );
        row.style.display = '';
      } else {
        row.style.display = 'none';
      }
    });

    this.totalDebit = debit;
    this.totalCredit = credit;
  }
}