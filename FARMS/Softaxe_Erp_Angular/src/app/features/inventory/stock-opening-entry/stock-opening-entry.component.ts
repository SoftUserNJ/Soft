import { Component, ViewChild, ElementRef } from '@angular/core';
import { DatePipe } from '@angular/common';
import { ApiService } from 'src/app/services/api.service';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder, FormGroup } from '@angular/forms';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';
declare const $: any;

@Component({
  selector: 'app-stock-opening-entry',
  templateUrl: './stock-opening-entry.component.html',
  styleUrls: ['./stock-opening-entry.component.css'],
})
export class StockOpeningEntryComponent {
  costCenter = localStorage.getItem('costCenter');

  JobList: any[] = [];
  stockOpenList: any[] = [];
  stockOpenForm!: FormGroup;
  locationList: any[] = [];
  productList: any[] = [];
  uomList: any[] = [];
  isDisabled: boolean = true;
  isNewClick = false;
  search = '';
  totalAmount: number = 0;
  openingBalance: number = 0;
  date: any;

  @ViewChild('stockOpenLists', { static: false }) stockOpenLists!: ElementRef;
  constructor(
    private fb: FormBuilder,
    private apiService: ApiService,
    private toast: ToastrService,
    private dp: DatePipe,
    private auth: AuthService,
    private com: CommonService
  ) {}

  onViewReport() {
    let dateP = this.date.split('/');
    let dateObj = new Date(dateP[2], dateP[1] - 1, dateP[0]);
    const date = this.dp.transform(dateObj, 'yyyy/MM/dd');

    let url = `PrintVoucherRangeWise?DateFrom=${date}&DateTo=${date}&VchType=JV-RM&VchNoFrom=1&VchNoTo=1&CompId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}`;
    this.com.viewReport(url);
  }

  async ngOnInit() {
    this.getStockOpening();
    this.formInit();
    this.resetForm();
    this.getLocation();
    this.JobList = await this.com.getJobList(true);
  }

  formInit() {
    this.stockOpenForm = this.fb.group({
      id: [0],
      searchProduct: [''],
      productId: [''],
      stock: [''],
      closingStock: [''],
      rate: [''],
      amount: [''],
      locationId: [null],
      expiryDate: [''],
      uomId: [null],
      uom: [''],
      jobNo: [null],
      dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
    });
  }

  resetForm() {
    this.stockOpenForm.get('closingStock')?.disable();
    this.stockOpenForm.get('amount')?.disable();

    this.stockOpenForm.get('id')?.patchValue(0);
    this.stockOpenForm.get('productId')?.patchValue(null);
    this.stockOpenForm.get('stock')?.patchValue('');
    this.stockOpenForm.get('closingStock')?.patchValue('');
    this.stockOpenForm.get('rate')?.patchValue('');
    this.stockOpenForm.get('amount')?.patchValue('');
    this.stockOpenForm.get('locationId')?.patchValue(null);
    this.stockOpenForm.get('expiryDate')?.patchValue(new Date());
    this.stockOpenForm.get('uomId')?.patchValue(null);
    this.stockOpenForm.get('name')?.patchValue('');
    this.stockOpenForm.get('searchProduct')?.patchValue('');
    this.uomList = [];
    this.productList = [];
  }

  getStockOpening() {
    this.apiService.getData('Inventory/GetSOList').subscribe((data) => {
      if (data.length != 0) {
        this.date = data[0].VCHDATE;
        this.stockOpenList = data;
        setTimeout(() => {
          this.searchGrid();
        }, 200);
      } else {
        this.stockOpenList = [];
      }
    });
  }

  getLocation() {
    this.apiService.getData('Inventory/GetLocation').subscribe((result) => {
      this.locationList = result;
    });
  }

  onSearchProduct(event: any) {
    if (event.target.value.length > 3) {
      let obj = {
        name: event.target.value,
      };

      this.apiService
        .getDataById(`Inventory/GetSearchProductList`, obj)
        .subscribe((data) => {
          this.productList = data;
        });
    }
  }

  async onChangeProduct(event: any) {
    const data = await this.apiService
      .getDataById('Inventory/GetProductDetail', { code: event.code })
      .toPromise();

    this.uomList = data.uom;
    const detail = data.detail[0];
    const expDate = detail.EXPDATE.split('/');

    this.stockOpenForm.patchValue({
      rate: detail.RATE,
      locationId: detail.SHELFID,
      uomId: data.uom[0].UOMID,
      expiryDate: new Date(expDate[2], expDate[1] - 1, expDate[0]),
    });
  }

  onChangeUom(event: any) {
    this.onInputStock();
  }

  onSubmit() {
    const body = this.stockOpenForm.value;

    if (body.productId == null) {
      this.toast.warning('Select Product....!');
      return;
    }

    if (body.rate == null || body.rate == 0) {
      this.toast.warning('Enter Rate....!');
      return;
    }

    if (body.uomId == null || body.uomId == '') {
      this.toast.warning('Select UOM....!');
      return;
    }

    if (body.stock == null || body.stock == 0) {
      this.toast.warning('Enter Stock Qty....!');
      return;
    }

    let closingBal = /[()]/.test(this.stockOpenForm.get('closingStock')?.value);
    if (closingBal) {
      this.toast.warning('Closing Stock in Negative....!');
      return;
    }

    if (body.locationId == null) {
      this.toast.warning('Select Location....!');
      return;
    }
    const uom = this.uomList.find((option) => option.UOMID === body.uomId);

    body.uom = uom.UOM;
    body.dtNow = new Date();
    body.expiryDate = this.dp.transform(body.expiryDate, 'yyyy-MM-dd');
    body.amount = this.stockOpenForm.get('amount').value;
    body.jobNo = body.jobNo == null ? 0 : body.jobNo;
    const jobName = this.JobList.find((x) => x.ID == body.jobNo);
    body.jobName = jobName != undefined ? jobName.NAME : '';

    try {
      this.com.showLoader();

      this.apiService
        .saveData('Inventory/SaveStockOpening', body)
        .subscribe((result) => {
          if (result == true || result == 'true') {
            this.toast.success('Save Successfully');
            this.onClickRefresh();
            this.getStockOpening();
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

  async editStock(item: any) {
    let result = this.com.isStopEntry('JV-RM');
    if (!result) {
      this.toast.info('You are not allowed');
      return;
    }

    try {
      this.com.showLoader();

      const obj = {
        id: item.ID,
        code: item.CODE,
        uomId: item.UOM,
      };

      const data = await this.apiService
        .getDataById('Inventory/EditSOProduct', obj)
        .toPromise();

      this.onClickNew();
      await this.onChangeProduct({ code: data[0].CODE });
      this.productList = [
        {
          code: data[0].CODE,
          name: data[0].PRODUCTNAME,
        },
      ];
      this.stockOpenForm.get('id')?.setValue(data[0].ID);
      this.stockOpenForm.get('productId')?.setValue(data[0].CODE);
      this.stockOpenForm.get('stock')?.setValue(data[0].QTY);
      this.stockOpenForm.get('closingStock')?.setValue(data[0].STOCKBALANCE);
      this.stockOpenForm.get('rate')?.setValue(data[0].RATE);
      this.stockOpenForm.get('amount')?.setValue(data[0].AMOUNT);
      this.stockOpenForm.get('locationId')?.setValue(data[0].LOCATIONID);
      this.stockOpenForm.get('jobNo')?.setValue(data[0].JOBNO);
      const expDate = data[0].EXPIRYDATE.split('/');
      this.stockOpenForm
        .get('expiryDate')
        ?.setValue(new Date(expDate[2], expDate[1] - 1, expDate[0]));
      this.stockOpenForm.get('uomId')?.setValue(parseInt(data[0].UOMID));
      this.openingBalance = data[0].STOCK;

      $('#productModal').modal('show');
      this.onInputStock();
      this.com.hideLoader();
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  deleteStock(item: any): void {
    let result = this.com.isStopEntry('JV-RM');
    if (!result) {
      this.toast.info('You are not allowed');
      return;
    }

    const confirmDelete = confirm('Are you sure you want to delete this item?');

    const expDate = item.EXPIRYDATE.split('/');
    if (!confirmDelete) {
      return;
    }

    try {
      this.com.showLoader();
      const obj = {
        id: item.ID,
        code: item.CODE,
        expDate: this.dp.transform(
          new Date(expDate[2], expDate[1] - 1, expDate[0]),
          'yyyy/MM/dd'
        ),
        location: item.SHELFNO,
        dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      };

      this.apiService.deleteData('Inventory/DeleteOPStock', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.toast.success('Deleted Successfully');
            this.getStockOpening();
            this.com.hideLoader();
          } else if (data == 'false' || data == false) {
            this.com.hideLoader();
            this.toast.error('Delete Again');
          }
        },
        error: (error) => {
          this.com.hideLoader();
          this.toast.info(error.error.text);
        },
      });
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  onInputSearch(event: any) {
    this.search = event.target.value;
    this.searchGrid();
  }

  searchGrid(): void {
    const tableElement = this.stockOpenLists.nativeElement;
    const rows = tableElement.querySelectorAll('tr');
    let amount = 0;

    rows.forEach((row: HTMLTableRowElement) => {
      let isShow = true;

      if (
        row.textContent &&
        row.textContent.toLowerCase().indexOf(this.search.toLowerCase()) > -1
      ) {
        isShow = true;
      } else {
        isShow = false;
      }

      if (isShow) {
        amount += parseFloat(
          row.querySelector('.amount')?.textContent!.replace(/,/g, '')
        );
        row.style.display = '';
      } else {
        row.style.display = 'none';
      }
    });

    this.totalAmount = amount;
  }

  onInputStock() {
    const rate = this.stockOpenForm.get('rate')?.value;
    const qty = this.stockOpenForm.get('stock')?.value;
    this.stockOpenForm
      .get('amount')
      ?.setValue(
        this.com.roundVal((parseFloat(rate) * parseFloat(qty)).toFixed(2))
      );

    const packing = this.uomList.find(
      (option) => option.UOMID === this.stockOpenForm.get('uomId')?.value
    );

    let currentStock = packing.PACKING * qty + this.openingBalance;
    let stock = this.getStock(currentStock, packing.BASPACKING);
    this.stockOpenForm.get('closingStock')?.setValue(stock);
  }

  onClickRefresh() {
    this.resetForm();
    this.isDisabled = true;
    this.isNewClick = false;
    this.openingBalance = 0;
  }

  onClickNew() {
    let result = this.com.isStopEntry('JV-RM');
    if (!result) {
      this.toast.info('You are not allowed');
      return;
    }

    this.onClickRefresh();
    this.isDisabled = false;
    this.isNewClick = true;
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
