import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-party-balance-status',
  templateUrl: './party-balance-status.component.html',
  styleUrls: ['./party-balance-status.component.css'],
})
export class PartyBalanceStatusComponent {
  fromDate: Date;
  toDate: Date;

  mainArea: any[] = [];
  mainAreaId: any = null;
  subArea: any[] = [];
  subAreaId: any;
  balance: boolean = true;
  sale: boolean = false;

  distributionPos = localStorage.getItem('distributionPos');
  locName = localStorage.getItem('locName');
  locationList: any[] = [];
  isDisableLoc: boolean = false;
  locId: any = null;

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
    this.GetMainArea();

    this.locationList = await this.com.getLocation();

    if(this.distributionPos != "ERP"){
      if (this.auth.locId() == 'HO') {
        this.locId = this.auth.locId();
        this.isDisableLoc = false;
      } else {
        this.locId = this.auth.locId();
        this.isDisableLoc = true;
      }
    }
  }

  GetMainArea() {
    this.apiService.getData('Purchase/GetMainArea').subscribe((data) => {
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
    this.apiService
      .getDataById('Purchase/GetSubArea', obj)
      .subscribe((data) => {
        this.subArea = data;
        this.subAreaId = undefined;
      });
  }

  onViewReport() {
    const fromDate = this.dp.transform(this.fromDate, 'yyyy/MM/dd');
    const toDate = this.dp.transform(this.toDate, 'yyyy/MM/dd');

    const fdp: any[] = fromDate.split('/');
    const fromLastDate = this.dp.transform(
      new Date(fdp[0], fdp[1] - 2, fdp[2]),
      'yyyy/MM/dd'
    );

    const tdp: any[] = toDate.split('/');
    const toLastDate = this.dp.transform(
      new Date(tdp[0], tdp[1] - 2, tdp[2]),
      'yyyy/MM/dd'
    );

    let groupId = 0;
    let groupId1 = 9999;
    let subGroupId = 0;
    let subGroupId1 = 9999;

    if (this.mainAreaId) {
      groupId = this.mainAreaId;
      groupId1 = this.mainAreaId;
    }

    if (this.subAreaId) {
      subGroupId = this.subAreaId;
      subGroupId1 = this.subAreaId;
    }

    if (this.locId == null) {
      this.locName = 'Over All';
    } else {
      this.locName = this.locationList.find((x) => x.ID == this.locId).NAME;
    }

    let url = '';

    if (this.balance) {
      url = `PartyPosition?FromDate=${fromDate}&ToDate=${toDate}&FLMDate=${fromLastDate}&TLMDate=${toLastDate}&FDate=${fromDate}&TDate=${toDate}&Groupid=${groupId}&Groupid1=${groupId1}&SubGroupid=${subGroupId}&SubGroupid1=${subGroupId1}&mainAccount=%&L4Tag=C&comp_id=${this.auth.cmpId()}&FinId=${this.auth.finId()}&locid=${
        this.locId ?? '%'
      }&LocName=${this.locName}`;
      //url = `PartyPosition?FromDate=${fromDate}&ToDate=${toDate}&FDate=${fromDate}&TDate=${toDate}&Groupid=${groupId}&Groupid1=${groupId1}&SubGroupid=${subGroupId}&SubGroupid1=${subGroupId1}&VchType=PI&L4Tag=C&VchR=BP&VchP=CP&comp_id=${this.auth.cmpId()}&FinId=${this.auth.finId()}&locid=${this.auth.locId()}&Verify=%&Approvel=%`;
    } else if (this.sale) {
      url = `PartyPosition1?FromDate=${fromDate}&ToDate=${toDate}&FDate=${fromDate}&TDate=${toDate}&Groupid=${groupId}&Groupid1=${groupId1}&SubGroupid=${subGroupId}&SubGroupid1=${subGroupId1}&VchType=RP-RAW&L4Tag=C&VchR=BP&VchP=CP&comp_id=${this.auth.cmpId()}&FinId=${this.auth.finId()}&locid=${
        this.locId ?? '%'
      }&Verify=%&Approvel=%&LocName=${this.locName}`;
    }

    this.com.viewReport(url);
  }

  onClickSatus(status: any) {
    if (status == 'balance') {
      this.balance = true;
      this.sale = false;
    } else if (status == 'sale') {
      this.balance = false;
      this.sale = true;
    }
  }
}
