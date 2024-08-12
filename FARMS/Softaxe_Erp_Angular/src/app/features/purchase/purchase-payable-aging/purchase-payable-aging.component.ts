import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-purchase-payable-aging',
  templateUrl: './purchase-payable-aging.component.html',
  styleUrls: ['./purchase-payable-aging.component.css'],
})
export class PurchasePayableAgingComponent {
  date: Date;

  constructor(
    private apiService: ApiService,
    private tostr: ToastrService,
    private dp: DatePipe,
    private com: CommonService,
    private auth: AuthService
  ) {
    this.date = new Date();
  }

  mainArea: any[] = [];
  mainAreaId: any = null;
  subArea: any[] = [];
  subAreaId: any;
  areaWise: boolean = true;

  txtD1: any;
  txtD2: any;
  txtD3: any;
  txtD4: any;
  txtD5: any;
  txtD6: any;
  txtD7: any;

  locName = localStorage.getItem('locName');
  locationList: any[] = [];
  isDisableLoc: boolean = false;
  locId: any = null;

  onViewReport() {
    const date = this.dp.transform(this.date, 'yyyy/MM/dd');
    let groupId = 0;
    let groupId1 = 9999;
    let subGroupId = 0;
    let subGroupId1 = 9999;
    let code = 0;
    let code1 = 99999999999999;

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

    let url = `Aging?FromDate=${date}&ToDate=${date}&FromGroupId=${groupId}&ToGroupId=${groupId1}&FromSubGroupId=${subGroupId}&ToSubGroupId=${subGroupId1}&FromCode=${code}&ToCode=${code1}&AreaWise=${
      this.areaWise
    }&CompId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${
      this.locId ?? '%'
    }&filterBy=Supplier&LocName=${this.locName}`;
    this.com.viewReport(url);
  }

  async ngOnInit() {
    this.GetMainArea();
    this.GetAging();

    this.locationList = await this.com.getLocation();
    if (this.auth.locId() == 'HO') {
      this.locId = this.auth.locId();
      this.isDisableLoc = false;
    } else {
      this.locId = this.auth.locId();
      this.isDisableLoc = true;
    }
  }

  GetAging() {
    this.apiService.getData('Purchase/GetAging').subscribe((data) => {
      this.txtD1 = data[0].D1;
      this.txtD2 = data[0].D2;
      this.txtD3 = data[0].D3;
      this.txtD4 = data[0].D4;
      this.txtD5 = data[0].D5;
      this.txtD6 = data[0].D6;
      this.txtD7 = data[0].D7;
    });
  }

  SaveDays() {
    try {
      this.com.showLoader();
      const obj = {
        d1: this.txtD1,
        d2: this.txtD2,
        d3: this.txtD3,
        d4: this.txtD4,
        d5: this.txtD5,
        d6: this.txtD6,
        d7: this.txtD7,
        dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      };

      this.apiService
        .saveObj('Purchase/SaveUpdateAging', obj)
        .subscribe((data) => {
          if (data == true || data == 'true') {
            this.GetMainArea();
            this.GetAging();
            this.com.hideLoader();
            this.tostr.success('Save Successfully');
          } else {
            this.com.hideLoader();
            this.tostr.error('Please Save Again');
          }
        });
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
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
}
