import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';
declare const $: any;

@Component({
  selector: 'app-sales-by-order',
  templateUrl: './sales-by-order.component.html',
  styleUrls: ['./sales-by-order.component.css'],
})
export class SalesByOrderComponent {
  fromDate: Date;
  toDate: Date;
  searchQuery: string = '';
  status: any = 'doinhand';
  isBilled: boolean = false;
  orderList: any[] = [];
  totalAmount: number = 0;
  totalRecAmount: number = 0;

  map: google.maps.Map;
  marker: google.maps.Marker;

  isShowPage: boolean = false;
  partyName: string = '';
  partyAddress: string = '';
  doNo: number = 0;
  amount: number = 0;
  productList: any[] = [];
  producDetailList: any[] = [];
  nTotalAmount: number = 0;

  row: any;
  isHideInvoice: boolean = true;

  // MAKE INVOICE
  invoiceNo: number = 0;
  invDate: Date;
  termsDays: any = null;
  deliveryPerson: any = null;
  LocId: any;
  termsList: any[] = [];
  locList: any[] = [];
  deliveryPersonList: [] = [];
  finalValue: any = 0;
  isPrint: boolean = false;
  allowSaleTax: any;
  allowWHTtax: any;

  @ViewChild('doLists', { static: false }) doLists!: ElementRef;
  constructor(
    private apiService: ApiService,
    private datePipe: DatePipe,
    private tostr: ToastrService,
    private auth: AuthService,
    private com: CommonService
  ) {
    const today = new Date();
    this.invDate = today;
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  }

  onViewReport(buttonType: any) {
    const invdate = this.datePipe.transform(this.invDate, 'yyyy/MM/dd');

    let url = '';
    if (buttonType === 'invoice') {
      url = `SaleInvoice?VchNoFrom=${this.invoiceNo}&VchNoTo=${
        this.invoiceNo
      }&VchType=SP&fromDate=${invdate}&toDate=${invdate}&CmpId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}`;
    } else if (buttonType === 'loading') {
      url = `SaleLoading?VchNo=${
        this.invoiceNo
      }&VchType=SP&CompId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}`;
    }
    this.com.viewReport(url);
  }

  onViewDoSummary() {
    const fromDate = this.datePipe.transform(this.fromDate, 'yyyy/MM/dd');
    const toDate = this.datePipe.transform(this.toDate, 'yyyy/MM/dd');

    let url = `DoSummary?FDate=${fromDate}&TDate=${toDate}&fromDate=${fromDate}&toDate=${toDate}&cmpId=${this.auth.cmpId()}&finId=${this.auth.finId()}&locId=${this.auth.locId()}`;
    this.com.viewReport(url);
  }

  ngOnInit(): void {
    this.getDOList();
    this.getTermsList();
    this.getLoc();
    this.getDeliveryPerson();
  }

  getTermsList() {
    this.apiService.getData('Purchase/GetTerms').subscribe((data) => {
      this.termsList = data;
    });
  }

  async getLoc() {
    const result = await this.apiService
      .getDataById('Admin/GetLocationById', { companyId: this.auth.cmpId() })
      .toPromise();
    this.locList = result;
    this.LocId = result[0].ID;
  }

  getDeliveryPerson() {
    this.apiService.getData('Sale/GetDeliveryPerson').subscribe((data) => {
      this.deliveryPersonList = data;
    });
  }

  async getDOList() {
    try {
      this.com.showLoader();

      const obj = {
        status: this.status,
        fromDate: this.datePipe.transform(this.fromDate, 'yyyy/MM/dd'),
        toDate: this.datePipe.transform(this.toDate, 'yyyy/MM/dd'),
      };

      const data = await this.apiService
        .getDataById('Sale/GetOrderList', obj)
        .toPromise();
      this.orderList = data;
      setTimeout(() => {
        this.searchGrid();
      }, 50);
      this.com.hideLoader();
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  async onClickDOInHand(status: any) {
    this.status = status;
    await this.getDOList();
    this.isBilled = false;
  }

  async onClickBilled(status: any) {
    this.status = status;
    await this.getDOList();
    this.isBilled = true;
  }

  searchGrid(): void {
    this.totalAmount = 0;
    this.totalRecAmount = 0;

    const tableElement = this.doLists.nativeElement;
    const rows = tableElement.querySelectorAll('tr');

    rows.forEach((row: HTMLTableRowElement) => {
      let isShow = true;

      if (
        row.textContent &&
        row.textContent.toLowerCase().indexOf(this.searchQuery.toLowerCase()) >
          -1
      ) {
        isShow = true;
      } else {
        isShow = false;
      }

      if (isShow) {
        this.totalAmount += parseFloat(
          row.querySelector('.amount')?.textContent!.replace(/,/g, '')
        );
        this.totalAmount += parseFloat(
          row.querySelector('.recAmount')?.textContent!.replace(/,/g, '')
        );
        row.style.display = '';
      } else {
        row.style.display = 'none';
      }
    });
  }

  onClickLocation(x: number, y: number) {
    
    const latLng = new google.maps.LatLng(x, y);
    const mapOptions: google.maps.MapOptions = {
      zoom: 18,
      center: latLng,
      mapTypeId: google.maps.MapTypeId.ROADMAP,
    };

    this.map = new google.maps.Map(
      document.getElementById('map') as HTMLElement,
      mapOptions
    );

    this.marker = new google.maps.Marker({
      position: latLng,
      map: this.map,
      title: 'Your current location!',
    });

    $('#LocationModal').modal('show');
  }

  onClickBack() {
    this.getDOList();
    this.isShowPage = false;
  }

  async onClickInv(i: any) {
    this.row = { ...i };
    this.partyName = i.NAMES;
    this.partyAddress = i.ADDRESS;
    this.doNo = i.DONO;
    this.amount = this.com.roundVal(i.TOALAMOUNT.toFixed(2));
    this.allowSaleTax = i.ALLOWSALETAX;
    this.allowWHTtax = i.ALLOWWHTAX;

    this.isHideInvoice = true;
    const pro = await this.apiService
      .getDataById('Sale/GetOrderDetail', { dono: i.DONO })
      .toPromise();
    this.productList = pro.product;

    let group = 0;
    let groupQty = 0;
    let groupQtyBal = 0;
    let uomId = 0;
    let tQty = 0;
    let qty = 0;

    pro.detail.forEach((x) => {
      let packing = x.PACKING;
      if (group != x.STOCKCODE || uomId != x.UOMID) {
        group = x.STOCKCODE;
        groupQty = x.QTY1;
        uomId = x.UOMID;
        groupQtyBal = groupQty;
        tQty = 0;
      }

      let stock = x.STOCK;
      if (tQty >= groupQty) {
        qty = 0;
      } else {
        if (stock > 0) {
          if (stock - groupQtyBal > 0) {
            qty = groupQtyBal / packing;
          } else {
            qty = stock / packing;
          }
        } else {
          qty = 0;
        }
      }

      if (qty == 0) {
        x.CLASS = 'border-danger';
      }
      groupQtyBal = groupQtyBal - qty * packing;
      x.NQTY = qty;
      tQty = tQty + qty * packing;
    });

    this.producDetailList = pro.detail;
    setTimeout(() => {
      this.calculate();
    }, 100);

    this.isShowPage = true;
    
  }

  onInpuQty(event: any, i: any, index: any) {
    if (i.NQTY < 0 || i.NQTY == null) {
      event.target.value = 0;
    }

    if (
      i.PACKING == '1' &&
      parseFloat(i.NQTY) >= parseFloat(i.BASEPACKING) &&
      i.PACKING != parseFloat(i.BASEPACKING)
    ) {
      event.target.value = parseFloat(i.BASEPACKING) - 1;
    }

    let stockArray = this.getStock(
      i.STOCKCODE,
      i.EXPIRYDATE,
      i.LOCATION,
      index
    );
    let stockInHand =
      parseFloat(this.getNum(i.STOCK)) + parseFloat(this.getNum(stockArray[1]));
    let stockAvalible =
      parseFloat(this.getNum(stockInHand)) -
      parseFloat(this.getNum(stockArray[0]));
    let qty =
      parseFloat(this.getNum(i.NQTY)) * parseFloat(this.getNum(i.PACKING));
    if (qty > stockAvalible) {
      event.target.value =
        Math.floor(stockAvalible / parseFloat(i.PACKING)) <= 0
          ? 0
          : Math.floor(stockAvalible / parseFloat(i.PACKING));
    }

    this.calculate();
  }

  getStock(code: any, expiry: any, location: any, index: any) {
    let stock = 0;
    let retStock = 0;

    this.producDetailList.forEach((x, i) => {
      if (
        x.STOCKCODE == code &&
        x.EXPIRYDATE == expiry &&
        x.LOCATION == location
      ) {
        let retQty = 0;
        // if (parseFloat(this.getNum(x.PACKING)) == 0 || parseFloat(this.getNum(x.PACKING)) == 1) {
        //   retStock += parseFloat(this.getNum(retQty)) * 1;
        // } else {
        //   retStock += parseFloat(this.getNum(retQty)) * parseFloat(this.getNum(x.PACKING));
        // }

        if (i != index) {
          if (
            parseFloat(this.getNum(x.PACKING)) == 0 ||
            parseFloat(this.getNum(x.PACKING)) == 1
          ) {
            stock += parseFloat(this.getNum(x.NQTY)) * 1;
          } else {
            stock +=
              parseFloat(this.getNum(x.NQTY)) *
              parseFloat(this.getNum(x.PACKING));
          }
        }
      }
    });
    return [stock, retStock];
  }

  getNum(val: any) {
    if (val == '') {
      val = 0;
    }
    if (isNaN(val) || val == Infinity) {
      return 0;
    }
    return val;
  }

  calculate() {
    this.nTotalAmount = 0;
    this.producDetailList.forEach((x) => {
      this.nTotalAmount += x.NQTY * x.RATE;
    });

    if(this.auth.cmpId() == "1"){
      if(this.nTotalAmount != this.amount){
        this.isHideInvoice = false;
      }
    }
  }

  getMaxNumber() {
    this.apiService.getData('Sale/getDoMax').subscribe((data) => {
      this.invoiceNo = data[0].INVNO;
    });
  }

  async onClickInvoice(dono: any, invNo: any) {
    
    this.finalValue = 0;
    this.deliveryPerson = null;
    this.isPrint = false;
    const data = await this.checkInvoice(dono, invNo);

    const x = data[0];
    if (x.DUEDAYS != null) {
      this.termsDays = (data[0].DUEDAYS == 0) ? data[0].DUEDAYS : data[0].DUEDAYS.toString();
      this.getMaxNumber();
    } else {
        this.isPrint = true;
        const dp = x.VCHDATE.split('/');
        this.invoiceNo = x.VCHNO;
        this.invDate = new Date(dp[2], dp[1] - 1, dp[0]);
        this.deliveryPerson = x.DELIVERBOY;
        this.termsDays = x.TERMS.toString();
        this.finalValue = x.VALUE;
        this.LocId = x.LOCID;
    }

    $('#invoiceModel').modal({
      backdrop: 'static',
      keyboard: false,
      show: false,
    });
    $('#invoiceModel').modal('show');
  }

  async checkInvoice(dono: any, invNo: any) {
    return await this.apiService
      .getDataById('Sale/CheckOrder', { dono: dono, vchNo: invNo })
      .toPromise();
  }

  onClickMakeInvoice() {
    if (this.LocId == null) {
      this.tostr.warning('Select Location');
      return;
    }

    if (this.deliveryPerson == null) {
      this.tostr.warning('Select Delivery Person');
      return;
    }

    try {
      this.com.showLoader();

      const i = this.row;
      const list = this.producDetailList.filter(
        (x) => x.NQTY != 0 && x.NQTY != null
      );

      let furtherTax: any = 0;
      let futherTaxAmt: any = 0;
      let whTax = 0;
      let value = 0;

      list.forEach((x) => {
        value += this.com.roundVal(parseFloat(x.NQTY) * parseFloat(x.RATE));
      });

      if (this.allowSaleTax == 'nonfiler') {
        furtherTax = localStorage.getItem('furtherTax');
      } else if (this.allowSaleTax == 'filer') {
        furtherTax = 0;
      } else if (this.allowSaleTax == 'taxfree') {
        this.producDetailList.forEach((x) => {
          x.SALETAX = 0;
        });
        furtherTax = 0;
        value = 0;
      }

      if (this.allowWHTtax == 'nonfiler') {
        whTax = parseFloat(localStorage.getItem('whNonFiler'));
      } else if (this.allowWHTtax == 'filer') {
        whTax = parseFloat(localStorage.getItem('whFiler'));
      } else if (this.allowWHTtax == 'taxfree') {
        whTax = 0;
      }
      futherTaxAmt = this.com.roundVal(((value / 100) * furtherTax).toFixed(2));

      let invoice: any[] = [];
      list.forEach((x) => {
        const amount = this.com.roundVal(parseFloat(x.NQTY) * parseFloat(x.RATE));
        const sateTaxAmt = this.com.roundVal((amount / 100) * parseFloat(x.SALETAX));
        const dp = x.EXPIRYDATE.split('/');
        const netBill = this.com.roundVal(amount + sateTaxAmt);

        let vDate = this.datePipe.transform(this.invDate, 'yyyy-MM-dd');
        const dPart: any = vDate.split('-');
        const dueDate = new Date(
          dPart[0],
          dPart[1] - 1,
          parseInt(dPart[2]) + parseInt(this.termsDays)
        );

        const obj = {
          DoNo: this.doNo,
          PartyCode: i.CODE,
          PartyName: i.NAMES,
          DeliveryBoy: this.deliveryPerson,
          OrderTaker: i.ORDERTAKERID,
          ProductCode: x.PRODUCTCODE,
          StockCode: x.STOCKCODE,
          SMaxRate: '0',
          Rate: x.RATE,
          DelQty: x.NQTY,
          SaleTax: x.SALETAX,
          SaleTaxAmt: sateTaxAmt,
          NetBill: netBill,
          InvNo: this.invoiceNo,
          LocId: this.LocId,
          Status: 'new',
          BatchNo: x.BATCHNO,
          ExpiryDate: this.datePipe.transform(
            new Date(dp[2], dp[1] - 1, dp[0]),
            'yyyy-MM-dd'
          ),
          VchDate: this.datePipe.transform(this.invDate, 'yyyy-MM-dd'),
          DueDate: this.datePipe.transform(dueDate, 'yyyy-MM-dd'),
          Uom: x.UOMID.toString(),
          GID: x.GODOWNID,
          RID: x.RACKID,
          SID: x.SHELFID,
          TermsDays: this.termsDays.toString(),
          FTax: furtherTax,
          FTaxAmt: futherTaxAmt,
          WHT: whTax,
          DTNow: new Date(),
        };
        invoice.push(obj);
      });

      this.apiService
        .saveData('Sale/AddUpdateOrder', invoice)
        .subscribe((result) => {
          if (result == true || result == 'true') {
            this.com.hideLoader();
            this.tostr.success('Save Successfully');
            this.getDOList();
            this.isPrint = true;
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

  onClickDelete() {
    var x = confirm('Are you sure you want to delete?');
    if (!x) {
      return;
    }

    if (this.finalValue != 0) {
      this.tostr.warning('Cannot Delete Finalized.');
      return;
    }

    try {
      this.com.showLoader();
      const obj = {
        vchno: this.invoiceNo,
        vchType: 'SP',
        dtNow: this.datePipe.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      };

      this.apiService.deleteData('Sale/DeleteInvoice', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.tostr.success('Delete Successfully');
            $('#invoiceModel').modal('hide');
            this.getDOList();
            this.isShowPage = false;
            this.com.hideLoader();
          } else if (data == 'false' || data == false) {
            this.com.hideLoader();
            this.tostr.error('Delete Again');
          }
        },
        error: (error) => {
          this.com.hideLoader();
          this.tostr.info(error.error.text);
        },
      });
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  onClickUpdate() {
    if (this.deliveryPerson == null) {
      this.tostr.warning('Select Delivery Person...!');
      return;
    }
    try {
      this.com.showLoader();

      let dueDate = this.invDate;
      dueDate.setDate(this.invDate.getDate() + parseInt(this.termsDays));

      const obj = {
        vchDate: this.datePipe.transform(this.invDate, 'yyyy/MM/dd'),
        dueDate: this.datePipe.transform(dueDate, 'yyyy/MM/dd'),
        deliveryPerson: this.deliveryPerson,
        terms: this.termsDays,
        invno: this.invoiceNo,
      };

      this.apiService.saveObj('Sale/UpdateOrder', obj).subscribe((data) => {
        if (data == true || data == 'true') {
          this.com.hideLoader();
          this.tostr.success('Order Updated');
        }
      });
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  export(fileName: any, formate: any) {
    this.com.ExportFiles(fileName, formate);
  }
}
