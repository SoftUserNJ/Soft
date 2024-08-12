import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { ApiService } from 'src/app/services/api.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-product-wise-sale',
  templateUrl: './product-wise-sale.component.html',
  styleUrls: ['./product-wise-sale.component.css'],
})
export class ProductWiseSaleComponent {
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

  fromDate: Date;
  toDate: Date;

  categoryList: any[] = [];
  categoryId: any = null;

  mainArea: any[] = [];
  mainAreaId: any = null;

  subArea: any[] = [];
  subAreaId: any;

  isTax: boolean = true;
  tag: any = 'product';

  costCenter = localStorage.getItem('costCenter');
  JobList: any[] = [];
  filterList: any[] = [];

  jobNo: any = null;
  locName = localStorage.getItem('locName');
  locationList: any[] = [];
  isDisableLoc: boolean = false;
  locId: any = null;

  onViewReport() {
    const fromDate = this.dp.transform(this.fromDate, 'yyyy/MM/dd');
    const toDate = this.dp.transform(this.toDate, 'yyyy/MM/dd');

    let fromGroup = 0;
    let toGroup = 9999;
    let fromSubGroup = 0;
    let toSubGroup = 9999;
    let fromCategory = 0;
    let toCategory = 9999;
    let jobNo = "";

    if (this.mainAreaId) {
      fromGroup = this.mainAreaId;
      toGroup = this.mainAreaId;
    }

    if (this.subAreaId) {
      fromSubGroup = this.subAreaId;
      toSubGroup = this.subAreaId;
    }

    if (this.categoryId) {
      fromCategory = this.categoryId;
      toCategory = this.categoryId;
    }

    if(this.jobNo){
      jobNo = this.JobList.find((x) => x.ID == this.jobNo).NAME
    }

    if (this.locId == null) {
      this.locName = 'Over All';
    } else {
      this.locName = this.locationList.find((x) => x.ID == this.locId).NAME;
    }

    let url = '';
    if (this.isTax) {
      if (this.tag == 'product') {
        url = `DailySaleProduct?FromDate=${fromDate}&ToDate=${toDate}&FDate=${fromDate}&TDate=${toDate}&GroupId=${fromGroup}&GroupId1=${toGroup}&SubGroupId=${fromSubGroup}&SubGroupId1=${toSubGroup}&IsTax=true&RptType=Inclusive Tax&CmpId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${
          this.locId ?? '%'
        }&JobNo=${this.jobNo ?? '%'}&LocName=${
          this.locName
        }&FromCategory=${fromCategory}&ToCategory=${toCategory}`;
      }
      if (this.tag == 'party') {
        url = `DailySaleProductPartyAreaWise?FromDate=${fromDate}&ToDate=${toDate}&FDate=${fromDate}&TDate=${toDate}&FromGroup=${fromGroup}&FromSubGroup=${fromSubGroup}&ToSubGroup=${toSubGroup}&ToGroup=${toGroup}&IsTax=true&RptType=Inclusive Tax&CmpId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${
          this.locId ?? '%'
        }&JobNo=${this.jobNo ?? '%'}&LocName=${
          this.locName
        }&FromCategory=${fromCategory}&ToCategory=${toCategory}&JobNoName=${jobNo}`;
      }
    } else if (!this.isTax) {
      if (this.tag == 'product') {
        url = `DailySaleProduct?FromDate=${fromDate}&ToDate=${toDate}&FDate=${fromDate}&TDate=${toDate}&GroupId=${fromGroup}&GroupId1=${toGroup}&SubGroupId=${fromSubGroup}&SubGroupId1=${toSubGroup}&IsTax=false&RptType=Exclusive Tax&CmpId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${
          this.locId ?? '%'
        }&JobNo=${this.jobNo ?? '%'}&LocName=${
          this.locName
        }&FromCategory=${fromCategory}&ToCategory=${toCategory}`;
      }
      if (this.tag == 'party') {
        url = `DailySaleProductPartyAreaWise?FromDate=${fromDate}&ToDate=${toDate}&FDate=${fromDate}&TDate=${toDate}&FromGroup=${fromGroup}&FromSubGroup=${fromSubGroup}&ToSubGroup=${toSubGroup}&ToGroup=${toGroup}&IsTax=false&RptType=Exclusive Tax&CmpId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${
          this.locId ?? '%'
        }&JobNo=${this.jobNo ?? '%'}&LocName=${
          this.locName
        }&FromCategory=${fromCategory}&ToCategory=${toCategory}&JobNoName=${jobNo}`;
      }
    }

    if (this.tag == 'item') {
      url = `ItemWiseSale?fromDate=${fromDate}&toDate=${toDate}&FDate=${fromDate}&TDate=${toDate}&groupId=${fromGroup}&groupId1=${toGroup}&subGroupId=${fromSubGroup}&subGroupId1=${toSubGroup}&fromCategory=${fromCategory}&toCategory=${toCategory}&cmpId=${this.auth.cmpId()}&finId=${this.auth.finId()}&locId=${
        this.locId ?? '%'
      }&LocName=${this.locName}&jobNo=${this.jobNo ?? '%'}&JobNoName=${jobNo}`;
    }

    this.com.viewReport(url);
  }

  async ngOnInit() {
    this.GetMainArea();
    this.getCategory();

    if (this.costCenter == 'true') {
      this.JobList = await this.com.getJobList(false);
    }

    this.locationList = await this.com.getLocation();
    if (this.auth.locId() == 'HO') {
      this.locId = this.auth.locId();
      this.isDisableLoc = false;
    } else {
      this.locId = this.auth.locId();
      this.isDisableLoc = true;
    }

    this.onChangeLoc();
  }

  getCategory() {
    this.apiService.getData('Inventory/GetCategory').subscribe((result) => {
      this.categoryList = result;
    });
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

  onClickInTax(e: any) {
    if (e.target.checked) {
      this.isTax = true;
    }
  }

  onClickExTax(e: any) {
    if (e.target.checked) {
      this.isTax = false;
    }
  }

  onClickRpt(tag: any) {
    this.tag = tag;
  }

  onChangeLoc() {
    this.jobNo = null;
    if (this.locId != null && this.locId != 'HO') {
      this.filterList = this.JobList.filter((x) => x.LOCID == this.locId);
    } else {
      this.filterList = this.JobList;
    }
  }
}
