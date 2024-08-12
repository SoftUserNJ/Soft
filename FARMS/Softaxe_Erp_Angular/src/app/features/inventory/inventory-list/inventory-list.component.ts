import { Component } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';
import { DatePipe } from '@angular/common';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-inventory-list',
  templateUrl: './inventory-list.component.html',
  styleUrls: ['./inventory-list.component.css'],
})
export class InventoryListComponent {
  locName = localStorage.getItem('locName');
  distributionPos = localStorage.getItem('distributionPos');
  fromDate: Date;
  toDate: Date;

  locationList: any[] = [];
  isDisableLoc: boolean = false;
  locId: any = null;

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

  onViewReport() {
    const fromDate = this.dp.transform(this.fromDate, 'yyyy/MM/dd');
    const toDate = this.dp.transform(this.toDate, 'yyyy/MM/dd');

    let groupId = 0;
    let groupId1 = 9999;
    let subGroupId = 0;
    let subGroupId1 = 9999;

    if (this.categoryId) {
      groupId = this.categoryId;
      groupId1 = this.categoryId;
    }

    if (this.brandId) {
      subGroupId = this.brandId;
      subGroupId1 = this.brandId;
    }

    if (this.locId == null) {
      this.locName = 'Over All';
    } else {
      this.locName = this.locationList.find((x) => x.ID == this.locId).NAME;
    }

    let url = `InventoryList?Groupid=${groupId}&Groupid1=${groupId1}&SubGroupid=${subGroupId}&SubGroupid1=${subGroupId1}&FromDate=${fromDate}&ToDate=${toDate} &FDate=${fromDate}&TDate=${toDate}&locid=${
      this.locId ?? '%'
    }&comp_id=${this.auth.cmpId()}&finid=${this.auth.finId()}&Verify=%&LocName=${
      this.locName
    }`;
    this.com.viewReport(url);
  }

  category = [];
  categoryId: any = null;
  brand = [];
  brandId: any = null;

  async ngOnInit() {
    this.GetProductCategory();

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
  }

  GetProductCategory() {
    this.apiService.getData('Inventory/GetCategory').subscribe((data) => {
      this.category = data;
    });
  }

  onClearCategory() {
    this.brand = [];
    this.brandId = undefined;
  }

  onChangeCategory() {
    if (!this.categoryId) {
      return;
    }

    var obj = { categoryId: this.categoryId };
    this.apiService.getDataById('Inventory/GetBrand', obj).subscribe((data) => {
      this.brand = data;
      this.brandId = undefined;
    });
  }
}
