import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-sales-margin-status',
  templateUrl: './sales-margin-status.component.html',
  styleUrls: ['./sales-margin-status.component.css'],
})
export class SalesMarginStatusComponent implements OnInit {
  fromDate: Date;
  toDate: Date;
  groupList: any[] = [];
  subGroupList: any[] = [];
  groupName: any = 'Main Area';
  subGroupName: any = 'Sub Area';
  group: any;
  subGroup: any;
  status: any = 'party';

  constructor(
    private apiService: ApiService,
    private dp: DatePipe,
    private auth: AuthService,
    private com: CommonService,
  ) {
    const today = new Date();
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  }

  openReportModal() {
    const fromDate = this.dp.transform(this.fromDate, 'yyyy/MM/dd');
    const toDate = this.dp.transform(this.toDate, 'yyyy/MM/dd');

    let groupId = 0;
    let groupId1 = 99999;
    let subGroupId = 0;
    let subGroupId1 = 99999;

    if (this.group) {
      groupId = this.group;
      groupId1 = this.group;
    }
    if (this.subGroup) {
      subGroupId = this.subGroup;
      subGroupId1 = this.subGroup;
    }

    let url = "";

    if (this.status == 'party') {
      url = `CostParty?FromDate=${fromDate}&ToDate=${toDate}&FDate=${fromDate}&TDate=${toDate}&GroupId=${groupId}&GroupId1=${groupId1}&SubGroupId=${subGroupId}&SubGroupId1=${subGroupId1}&Verify=%&CompId=${this.auth.cmpId()}&LocId=${this.auth.locId()}`;
    } else if (this.status == 'category') {
      url = `CostCategory?FromDate=${fromDate}&ToDate=${toDate}&FDate=${fromDate}&TDate=${toDate}&GroupId=${groupId}&GroupId1=${groupId1}&SubGroupId=${subGroupId}&SubGroupId1=${subGroupId1}&Verify=%&CompId=${this.auth.cmpId()}&LocId=${this.auth.locId()}`;
    } else if (this.status == 'product') {
      url = `CostProduct?FromDate=${fromDate}&ToDate=${toDate}&FDate=${fromDate}&TDate=${toDate}&GroupId=${groupId}&GroupId1=${groupId1}&SubGroupId=${subGroupId}&SubGroupId1=${subGroupId1}&Verify=%&CompId=${this.auth.cmpId()}&LocId=${this.auth.locId()}`;
    } else if (this.status == 'teamwise') {
      url = `CostSaleTeam?FromDate=${fromDate}&ToDate=${toDate}&FDate=${fromDate}&TDate=${toDate}&GroupId=${groupId}&GroupId1=${groupId1}&SubGroupId=${subGroupId}&SubGroupId1=${subGroupId1}&Verify=%&CompId=${this.auth.cmpId()}&LocId=${this.auth.locId()}`;
    }
    this.com.viewReport(url);
  }

  ngOnInit(): void {
    this.getGroup();
  }

  getGroup() {
    this.resetList();
    let url = '';

    if (this.status == 'party') {
      url = 'Sale/GetMainArea';
    } else if (this.status == 'category' || this.status == 'product') {
      url = 'Inventory/GetCategory';
    } else if (this.status == 'teamwise') {
      url = 'Sale/SaleManagerList';
    }

    this.apiService.getData(url).subscribe((data) => {
      this.groupList = data;
    });
  }

  onChangeGroup() {
    let url = '';
    let obj: any = {};

    if (!this.group) {
      this.subGroupList = [];
      this.subGroup = null;
      return;
    }

    if (this.status == 'party') {
      url = 'Sale/GetSubArea';
      obj.mainAreaId = this.group;
    } else if (this.status == 'category' || this.status == 'product') {
      url = 'Inventory/GetBrand';
      obj.categoryId = this.group;
    } else if (this.status == 'teamwise') {
      url = 'Sale/GetOrderTakerBySM';
      obj.saleManagerId = this.group;
    }

    this.apiService.getDataById(url, obj).subscribe((data) => {
      this.subGroupList = data;
    });
  }

  onRadioChange(name: string) {
    this.status = name;

    if (name == 'party') {
      this.groupName = 'Main Area';
      this.subGroupName = 'Sub Area';
    } else if (name == 'category' || name == 'product') {
      this.groupName = 'Category';
      this.subGroupName = 'Brand';
    } else if (name == 'teamwise') {
      this.groupName = 'Sale Manager';
      this.subGroupName = 'Order Taker';
    }
    this.getGroup();
  }

  resetList() {
    this.group = null;
    this.subGroupList = [];
    this.subGroup = null;
  }
}
