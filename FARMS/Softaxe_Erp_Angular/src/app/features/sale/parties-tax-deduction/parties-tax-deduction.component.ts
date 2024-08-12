import { Component } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';
import { DatePipe } from '@angular/common';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-parties-tax-deduction',
  templateUrl: './parties-tax-deduction.component.html',
  styleUrls: ['./parties-tax-deduction.component.css'],
})
export class PartiesTaxDeductionComponent {
  fromDate: Date;
  toDate: Date;

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

  mainArea: any[] = [];
  mainAreaId: any = null;

  subArea: any[] = [];
  subAreaId: any;

  customerList: any[] = [];
  customer: any;

  detail: boolean = true;
  summary: boolean = false;

  locName = localStorage.getItem('locName');
  locationList: any[] = [];
  isDisableLoc: boolean = false;
  locId: any = null;

  onViewReport() {
    const fromDate = this.dp.transform(this.fromDate, 'yyyy/MM/dd');
    const toDate = this.dp.transform(this.toDate, 'yyyy/MM/dd');

    let groupId = 0;
    let groupId1 = 9999;
    let subGroupId = 0;
    let subGroupId1 = 9999;
    let fromCode = 0;
    let toCode = 99999999999999;

    if (this.mainAreaId) {
      groupId = this.mainAreaId;
      groupId1 = this.mainAreaId;
    }

    if (this.subAreaId) {
      subGroupId = this.subAreaId;
      subGroupId1 = this.subAreaId;
    }

    if (this.customer) {
      fromCode = this.customer;
      toCode = this.customer;
    }

    if (this.locId == null) {
      this.locName = 'Over All';
    } else {
      this.locName = this.locationList.find((x) => x.ID == this.locId).NAME;
    }

    let url = '';

    if (this.detail) {
      url = `PartiesTaxDeduction?fromDate=${fromDate}&toDate=${toDate}&FDate=${fromDate}&TDate=${toDate}&fromCode=${fromCode}&toCode=${toCode}&fromGroupId=${groupId}&toGroupId=${groupId1}&fromSubGroupId=${subGroupId}&toSubGroupId=${subGroupId1}&cmpId=${this.auth.cmpId()}&finId=${this.auth.finId()}&locId=${
        this.locId ?? '%'
      }&LocName=${this.locName}`;
    }

    if (this.summary) {
      url = `PartiesTaxDeductionSummary?fromDate=${fromDate}&toDate=${toDate}&FDate=${fromDate}&TDate=${toDate}&fromCode=${fromCode}&toCode=${toCode}&cmpId=${this.auth.cmpId()}&finId=${this.auth.finId()}&locId=${
        this.locId ?? '%'
      }&LocName=${this.locName}`;
    }
    this.com.viewReport(url);
  }

  async ngOnInit() {
    this.GetMainArea();
    this.getCustomerList();

    this.locationList = await this.com.getLocation();
    if (this.auth.locId() == 'HO') {
      this.locId = this.auth.locId();
      this.isDisableLoc = false;
    } else {
      this.locId = this.auth.locId();
      this.isDisableLoc = true;
    }
  }

  GetMainArea() {
    this.apiService.getData('Sale/GetMainArea').subscribe((data) => {
      this.mainArea = data;
    });
  }

  onClearMainArea() {
    this.subArea = [];
    this.subAreaId = undefined;
  }

  onChangeMainArea() {
    if (!this.mainAreaId) {
      return;
    }

    var obj = { mainAreaId: this.mainAreaId };
    this.apiService.getDataById('Sale/GetSubArea', obj).subscribe((data) => {
      this.subArea = data;
      this.subAreaId = undefined;
    });
  }

  getCustomerList() {
    this.apiService.getData('Sale/GetCustomerLedger').subscribe((data) => {
      this.customerList = data;
    });
  }

  onClickSatus(status: any) {
    if (status == 'detail') {
      this.detail = true;
      this.summary = false;
    } else if (status == 'summary') {
      this.detail = false;
      this.summary = true;
    }
  }
}
