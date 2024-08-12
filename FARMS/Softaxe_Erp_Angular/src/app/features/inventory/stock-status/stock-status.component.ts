import { Component } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';
import { environment } from '../../../../environment/environmemt';
import { CommonService } from 'src/app/services/common.service';
declare const $: any;

interface StockData {
  PRODUCT: string;
  DES: string;
  Category: string;
  Brand: string;
  MadeIn: string;
  LASTPURCHASE: string;
  LASTSALE: string;
  MAXRATE: number;
  MINRATE: number;
  RATE: number;
  TAX: number;
  DISCOUNT: number;
  UOM: string;
  SKU: string;
  MINLVL: number;
  INACTIVE: boolean;
  IMAGE: string;
  LOCATION: string;
  EXPIRY: string;
  STOCK: string;
  TOTALSTOCK: string;
}

@Component({
  selector: 'app-stock-status',
  templateUrl: './stock-status.component.html',
  styleUrls: ['./stock-status.component.css'],
})
export class StockStatusComponent {
  stockStatusList: any[] = [];
  filteredStockStatusList: any[] = [];
  stockDetail: any[] = [];
  totalStockAmount: number = 0;
  searchQuery: string = '';
  modalData: StockData = {} as StockData;
  totalStock: any;
  basePath = environment.basePath;

  constructor(private apiService: ApiService, private com: CommonService) {}

  ngOnInit(): void {
    this.getStockStatus();
  }

  getStockStatus() {
    try {
      this.com.showLoader();
      this.apiService.getData('Inventory/GetStockStatus').subscribe((data) => {
        this.stockStatusList = data;
        this.filteredStockStatusList = [...this.stockStatusList];
        this.calculation();
        this.com.hideLoader();
      });
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  calculation() {
    this.totalStockAmount = this.filteredStockStatusList.reduce(
      (total, item) => total + item.STOCKAMT,
      0
    );
  }

  filterData() {
    this.filteredStockStatusList = this.stockStatusList.filter((item) => {
      for (const key in item) {
        if (typeof item[key] === 'string' && item[key].toLowerCase().includes(this.searchQuery.toLowerCase())) {
          return true;
        }
      }
      return false;
    });
    this.calculation();
  }
  

  openDetailModal(item: any) {
    this.apiService
      .getDataById('Inventory/GetStockDetail', { code: item.STOCKCODE })
      .subscribe((data: any) => {
        if (data.length > 0) {
          this.stockDetail = data;
          this.modalData = data[0];
          this.modalData.Category = item.CATEGORY;
          this.modalData.Brand = item.BRAND;
          this.modalData.MadeIn = item.COUNTRY;

          let totalstock = 0;
          data.forEach((item) => {
            totalstock += item.TOTALSTOCK;
          });

          this.totalStock = this.getStock(totalstock, data[0].PACKING);
        }

        $('#StockDetail').modal('show');
      });
  }

  getStock(qty: any, packing: any) {
    let result = '';
    let mqty;
    if (qty < 0) mqty = qty * -1;
    else mqty = qty;
    if (packing === 0) result = mqty.toString();
    else if (mqty >= packing)
      result =
        Math.floor(mqty / packing).toString() +
        ' - ' +
        Math.floor(mqty % packing).toString();
    else result = '0 - ' + mqty.toString();
    if (qty < 0) result = '(' + result + ')';
    return result;
  }

  export(fileName: any, formate: any) {
    this.com.ExportFiles(fileName, formate);
  }
}
