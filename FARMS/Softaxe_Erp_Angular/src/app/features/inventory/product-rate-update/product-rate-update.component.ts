import { Component, ElementRef, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-product-rate-update',
  templateUrl: './product-rate-update.component.html',
  styleUrls: ['./product-rate-update.component.css'],
})
export class ProductRateUpdateComponent {
  productList: any[] = [];

  @ViewChild('productLists', { static: false }) productLists!: ElementRef;

  constructor(
    private apiService: ApiService,
    private toast: ToastrService,
    private com: CommonService
  ) {}

  ngOnInit(): void {
    this.getProductRates();
  }

  getProductRates() {
    this.apiService
      .getData('Inventory/GetProductRateUpdate')
      .subscribe((data) => {
        this.productList = data;
      });
  }

  onClickSave() {
    const list = this.productList.filter((x) => x.STATUS == true);

    if (list.length == 0) {
      this.toast.warning('Change any Rate/Qty...!');
      return;
    }
    try {
      this.com.showLoader();
      const productRate: any[] = list.map((data: any) => ({
        Code: data.PRODUCTCODE,
        SaleRate: data.SALERATE,
        FromQty: data.FROMQTY,
        SlabRate: data.SLABRATE,
        ToQty: data.TOQTY,
        AboveSlab: data.ABOVESLABRATE,
        DtNow: new Date(),
      }));
      this.apiService
        .saveData('Inventory/SaveProductRateUpdate', productRate)
        .subscribe((result) => {
          if (result == true || result == 'true') {
            this.productList.forEach((x) => {
              x.STATUS = false;
              this.com.hideLoader();
            });
            this.toast.success('Save Successfully');
            this.com.hideLoader();
          } else {
            this.com.hideLoader();
            this.toast.error('Please Save Again');
          }
        });
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  onInputRate(item: any) {
    item.STATUS = true;
  }

  searchGrid(event: any): void {
    const tableElement = this.productLists.nativeElement;
    const rows = tableElement.querySelectorAll('tr');

    rows.forEach((row: HTMLTableRowElement) => {
      let isShow = true;

      if (
        row.textContent &&
        row.textContent
          .toLowerCase()
          .indexOf(event.target.value.toLowerCase()) > -1
      ) {
        isShow = true;
      } else {
        isShow = false;
      }

      if (isShow) {
        row.style.display = '';
      } else {
        row.style.display = 'none';
      }
    });
  }
  export(fileName: any, formate: any) {
    this.com.ExportFiles(fileName, formate);
  }
}
