import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { ApiService } from 'src/app/services/api.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-product-sale-issue',
  templateUrl: './product-sale-issue.component.html',
  styleUrls: ['./product-sale-issue.component.css']
})
export class ProductSaleIssueComponent {
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
  customerList: any[] = [];
  customer: any;
  categoryList: any[] = [];
  category: any;
  productList: any[] = [];
  filterProduct: any[] = [];
  product: any;
  vehicleList: any[] = [];
  vehicle: any;
  isIssue: string = 'sale'

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

    let partyCode = '%';
    let productCode = '%';
    let fromCategory = 0;
    let toCategory = 9999;
    let vehicleNo = '%';
    let jobNo = '';
    let rptType = 'Sale Detail';

    if(this.vehicle){
      vehicleNo = this.vehicle
    }
    
    if(this.isIssue == 'sale'){
      rptType = 'Sale Detail'
    }
    else if(this.isIssue == 'issue'){
      rptType = 'Issue For JobNo'
    }
    else{
      rptType = 'Sale / Issue'
    }

    if (this.jobNo) {
      jobNo = this.JobList.find((x) => x.ID == this.jobNo).NAME;
    }

    if(this.customer){
      partyCode = this.customer;
    }

    if(this.category){
      fromCategory = this.category;
      toCategory = this.category;
    }

    if(this.product){
      if(this.isIssue){
        productCode = '1' + this.product.substring(1, 14);
      }
      else{
        productCode = this.product;
      }
    }

    if (this.locId == null) {
      this.locName = 'Over All';
    } else {
      this.locName = this.locationList.find((x) => x.ID == this.locId).NAME;
    }

    let url = `ProductSaleIssue?fromDate=${fromDate}&toDate=${toDate}&FDate=${fromDate}&TDate=${toDate}&cmpId=${this.auth.cmpId()}&finId=${this.auth.finId()}&locId=${
      this.locId ?? '%'
    }&LocName=${this.locName}&jobNo=${this.jobNo ?? '%'}&JobNoName=${jobNo}&PartyCode=${partyCode}&ProductCode=${productCode}&FromCategory=${fromCategory}&ToCategory=${toCategory}&RptType=${rptType}&vehicleNo=${vehicleNo}&isIssue=${this.isIssue}`;

    this.com.viewReport(url);
  }

  async ngOnInit() {
    this.getCustomerList();
    this.getCategoryList();
    this.getProductList();
    this.getVehicle();

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

  getCustomerList() {
    this.apiService.getDataById('Accounts/GetAccountsList', {module: "customersupplier"}).subscribe((data) => {
      this.customerList = data;
    });
  }

  getCategoryList() {
    this.apiService.getData('Inventory/GetCategory').subscribe((data) => {
      this.categoryList = data;
    });
  }

  getProductList() {
    this.apiService.getDataById('Inventory/GetProductRateUpdate', {isStock: false}).subscribe((data) => {
      this.productList = data;
      this.filterProduct = data;
    });
  }

  getVehicle() {
    this.apiService.getData('Sale/GetVehicleNo').subscribe((data) => {
      this.vehicleList = data;
    });
  }

  onChangeCategory(){
    this.product = null;
    if (this.category != null) {
      this.filterProduct = this.productList.filter((x) => x.CATEGORYID == this.category);
    } else {
      this.filterProduct = this.productList;
    }
  }

  rptStatus(status: string){
    this.isIssue = status;
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