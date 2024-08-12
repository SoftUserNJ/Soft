import { Component } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';
import { DatePipe } from '@angular/common';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-product-rate-comparison',
  templateUrl: './product-rate-comparison.component.html',
  styleUrls: ['./product-rate-comparison.component.css'],
})
export class ProductRateComparisonComponent {
  fromDate: Date;
  toDate: Date;

  constructor(
    private apiService: ApiService,
    private com: CommonService,
    private dp: DatePipe,
    private auth: AuthService
  ) {
    const today = new Date();
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  }

  category = [];
  categoryId: any = null;

  brand = [];
  brandId: any = null;


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

    let url = `InventoryListSale?Groupid=${groupId}&Groupid1=${groupId1}&SubGroupid=${subGroupId}&SubGroupid1=${subGroupId1}&FromDate=${fromDate}&ToDate=${toDate} &FDate=${fromDate}&TDate=${toDate}&LocId=${this.auth.locId()}&CompId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&Verify=%`;
    this.com.viewReport(url);
  }

  ngOnInit(): void {
    this.GetProductCategory();
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
