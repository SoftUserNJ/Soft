import { Component, ViewChild, ElementRef, Input, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { environment } from '../../../../environment/environmemt';
import { CommonService } from 'src/app/services/common.service';
declare const $: any;

@Component({
  selector: 'app-finished-goods-entry',
  templateUrl: './finished-goods-entry.component.html',
  styleUrls: ['./finished-goods-entry.component.css']
})
export class FinishedGoodsEntryComponent {

  @ViewChild('InvoiceList', { static: false }) InvoiceList!: ElementRef;
  
  basePath = environment.basePath;
  saleInvoiceForm!: FormGroup;

  // PRODUCT
  locationList: any[] = [];
  uomList: any[] = [];
  categoryList: any[] = [];
  category: any = null;
  productName: string = '';
  barCode: any = '';
  productList: any[] = [];

  // INVOICE LIST
  fromDate: Date;
  toDate: Date;
  invoiceList: any[] = [];

  // PRODUCT MODAL
  mProduct: any;
  mDescriptiion: any;
  mCategory: any;
  mBrand: any;
  mMadeIn: any;
  mUom: any;
  mSku: any;
  mWarehouse: any;
  mRack: any;
  mShelf: any;
  mExpiry: any;
  mStock: any;
  mTax: any;
  mDiscount: any;
  mRate: any;
  mStatus: any;
  mImage: any;

  // TERMS
  termsDays = '';
  termsId = 0;
  termsList: any[] = [];
  isDisabledTerms: boolean = true;
  isShowTerms: boolean = false;

  // Qty MODAL
  balance: any;
  onQty: number;
  location: any;
  subParty: any;
  batchNo: number;
  expiryDate: Date;
  saleRate: number;
  amount: any;

  // PAYMENT
  bankCashList: any[] = [];
  paymentList: any[] = [];
  isDisabledPayment: boolean = true;
  isShowPayment: boolean = false;
  bankCash = '';
  totalPayment = '';
  payment = '';
  chequeNo = '';
  today: Date;
  chequeDate: Date;
  paymentDate: Date;

  // OTHER
  SR: boolean = false;
  locList: any[] = [];
  netDue: any;
  productRow: any;
  appendedData: any[] = [];
  isNewClick: boolean = false;
  isDisabled: boolean = true;
  isTaxDisable: boolean = false;
  formStatus: string = '';
  isPrint: boolean = false;
  paymentMethod: any = 'Cash';

  // PARTY
  partyMainList: any[] = [];
  partySubList: any[] = [];
  tblSubPartyList: any[] = [];
  partyDiscounts: any[] = [];

  pAddress:string = '';
  pCity:string = '';
  pNTN:string = '';
  pRateDiff:string = '';

  // CURRENCY
  currencyList: any[] = [];

  // GODOWNS
  godownsList: any[] = [];

  constructor(
    private apiService: ApiService,
    private fb: FormBuilder,
    private tostr: ToastrService,
    private dp: DatePipe,
    private auth: AuthService,
    private com: CommonService,
  ) {
    const today = new Date();
    this.today = today;
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  }

  onClickPrint(tag: any) {
    let url = '';
    const vchNo = this.saleInvoiceForm.get('invNo').value;
    const vchType = this.saleInvoiceForm.get('vchType').value;
    const vchDate = this.dp.transform(
      this.saleInvoiceForm.get('vchDate').value,
      'yyyy/MM/dd'
    );

    if (tag === 'invoice') {
      if (vchType == 'SP') {
        url = `SaleInvoice?VchNoFrom=${vchNo}&VchNoTo=${vchNo}&VchType=${vchType}&fromDate=${vchDate}&toDate=${vchDate}&CmpId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}`;
      }
      if (vchType == 'SR') {
        url = `SaleReturn?VchNoFrom=${vchNo}&VchNoTo=${vchNo}&VchType=${vchType}&fromDate=${vchDate}&toDate=${vchDate}&CmpId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}`;
      }
    } else if (tag === 'loading') {
      url = `SaleLoading?VchNo=${vchNo}&VchType=${vchType}&CompId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}`;
    } else if (tag == 'vch') {
      url = `PrintVoucherRangeWise?DateFrom=${vchDate}&DateTo=${vchDate}&VchType=${vchType}&VchNoFrom=${vchNo}&VchNoTo=${vchNo}&CompId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}`;
    }
    this.com.viewReport(url);
  }

  onClickRowReport(invoiceNo: string, invDate: any, tag: string) {
    let url = '';

    const dParts = invDate.split('/');
    const invdate = this.dp.transform(
      new Date(dParts[2], dParts[1] - 1, dParts[0]),
      'yyyy/MM/dd'
    );
    const vchType = this.saleInvoiceForm.get('vchType').value;

    if (tag === 'invoice') {
      if (vchType == 'SP') {
        url = `SaleInvoice?VchNoFrom=${invoiceNo}&VchNoTo=${invoiceNo}&VchType=${vchType}&fromDate=${invdate}&toDate=${invdate}&CmpId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}`;
      }
      if (vchType == 'SR') {
        url = `SaleReturn?VchNoFrom=${invoiceNo}&VchNoTo=${invoiceNo}&VchType=${vchType}&fromDate=${invdate}&toDate=${invdate}&CmpId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}`;
      }
    } else if (tag === 'loading') {
      url = `SaleLoading?VchNo=${invoiceNo}&VchType=${vchType}&CompId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}`;
    }
    this.com.viewReport(url);
  }

  ngOnInit() {

    this.formInit();
    this.getUoms();
    this.getLocation();
    this.getLoc();
    this.getCategory();
    this.getTerms();
    this.getCurrency();
    this.getGodowns();
    this.getPartyMain();
    this.resetForm();
  }

  formInit() {
    this.saleInvoiceForm = this.fb.group({
      invNo: [0],
      vchType: ['Do-Sales'],
      vchDate: [new Date()],
      locId: [''],
      currency: [undefined],
      creditDebit: ['Credit'],
      godown: [undefined],
      partyMain: [undefined],
      partySub: [undefined],
      party: [null],
      grossAmount: [0],
      discount: [0],
      discountAmt: [0],
      otherCredit: [0],
      remarks: [''],
      shipment: [0],
      fTax: [0],
      fTaxAmt: [0],
      whTax: [0],
      whTaxAmt: [0],
      totalDue: [0],
      recAmount: [0],
      retAmount: [0],
      termsDay: ['30'],
    });

    this.saleInvoiceForm.get('grossAmount').disable();
    this.saleInvoiceForm.get('fTaxAmt').disable();
    this.saleInvoiceForm.get('whTaxAmt').disable();
    this.saleInvoiceForm.get('totalDue').disable();
    this.saleInvoiceForm.get('retAmount').disable();
  }

  resetForm() {
    this.category = null;
    this.productName = '';
    this.formStatus = '';
    this.barCode = '';
    this.netDue = '0.00';
    this.isPrint = false;
    this.isNewClick = false;
    this.SR = false;
    this.productList = [];
    this.appendedData = [];
    this.saleInvoiceForm.reset();
    this.saleInvoiceForm.get('invNo').setValue(0);
    this.saleInvoiceForm.get('vchType').setValue('Do-Sales');
    this.saleInvoiceForm.get('vchDate').setValue(new Date());
    this.saleInvoiceForm.get('creditDebit').setValue('Credit');
    this.saleInvoiceForm.get('currency').setValue(undefined);
    this.saleInvoiceForm.get('locId').setValue(this.auth.locId());
    this.saleInvoiceForm.get('grossAmount').setValue(0);
    this.saleInvoiceForm.get('discount').setValue(0);
    this.saleInvoiceForm.get('discountAmt').setValue(0);
    this.saleInvoiceForm.get('otherCredit').setValue(0);
    this.saleInvoiceForm.get('shipment').setValue(0);
    this.saleInvoiceForm.get('fTax').setValue(0);
    this.saleInvoiceForm.get('fTaxAmt').setValue(0);
    this.saleInvoiceForm.get('whTax').setValue(0);
    this.saleInvoiceForm.get('whTaxAmt').setValue(0);
    this.saleInvoiceForm.get('totalDue').setValue(0);
    this.saleInvoiceForm.get('recAmount').setValue(0);
    this.saleInvoiceForm.get('retAmount').setValue(0);
    this.saleInvoiceForm.get('termsDay').setValue(30);
  }

  onClickNew() {
    this.isDisabled = false;
    this.isNewClick = true;
    this.formStatus = 'New';
    this.getMax();
  }

  onClickRefresh() {
    this.isDisabled = true;
    this.resetForm();
  }

  async getCurrency() {
    const result = await this.apiService
      .getData('Sale/GetCurrency')
      .toPromise();
    this.currencyList = result;
  }

  async getGodowns() {
    const result = await this.apiService
      .getData('Common/GetGodowns')
      .toPromise();

    this.godownsList = result;
  }

  async getTblSubParty(event:any) {
    const result = await this.apiService
      .getDataById('Common/GetSubParty', {code : event.CODE})
      .toPromise();

    this.tblSubPartyList = result;
  }

  async getPartyMain() {
    const result = await this.apiService
      .getDataById('Common/GetLevel4CodeNameByTag', {tag:'D'})
      .toPromise();
    this.partyMainList = result;
  }

  onPartyMainChange(event:any){
    this.getPartySub(event.CODE);
  }

  async getPartySub(code) {
    const result = await this.apiService
      .getDataById('Common/GetLevel5CodeNameByL4Code',{code:code})
      .toPromise();
    this.partySubList = result;
  }

  onPartySubChange(event:any){

    if(event == undefined){
      this.pAddress = '';
      this.pCity = '';
      this.pNTN = '';
      this.pRateDiff = '';
    }
    else{
      const p = this.partySubList.find((p) => p.CODE == event.CODE);

      this.pAddress = p.Address;
      this.pCity = p.City;
      this.pNTN = p.NTN +'/'+ p.CNIC;
      this.pRateDiff = p.RateDiff;

      this.getTblSubParty({CODE:event.CODE})
      this.getPartyDisc({CODE:event.CODE})
    }
  }

  async getPartyDisc(event:any){
   
      const result = await this.apiService
        .getDataById('Sale/GetPartyDisc', {code : event.CODE})
        .toPromise();
      this.partyDiscounts = result;

  }

  async getUoms() {
    const result = await this.apiService
      .getData('Inventory/GetProductUoms')
      .toPromise();
    this.uomList = result;
  }

  async getLocation() {
    const result = await this.apiService
      .getData('Inventory/GetLocation')
      .toPromise();
    this.locationList = result;
  }

  async getLoc() {
    const result = await this.apiService
      .getDataById('Admin/GetLocationById', { companyId: this.auth.cmpId() })
      .toPromise();
    this.locList = result;
    this.saleInvoiceForm.get('locId').setValue(this.auth.locId());
  }

  getInvoices() {
    const obj = {
      fromDate: this.dp.transform(this.fromDate, 'yyyy/MM/dd'),
      toDate: this.dp.transform(this.toDate, 'yyyy/MM/dd'),
      vchType: this.saleInvoiceForm.get('vchType').value,
    };

    this.apiService
      .getDataById('Sale/GetDoList', obj)
      .subscribe((data) => {
        this.invoiceList = data;
      });
  }

  isRound(value: any) {
    return this.com.roundVal(value);
  }

  onSearchInput(event: any): void {
    const tableElement = this.InvoiceList.nativeElement;
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

  onClickSP(event: any): void {
    if (this.isNewClick) {
      event.preventDefault();
      return;
    }

    this.saleInvoiceForm.get('vchType').setValue('SP');
    this.SR = false;
  }

  onClickSR(event: any): void {
    if (this.isNewClick) {
      event.preventDefault();
      return;
    }

    this.saleInvoiceForm.get('vchType').setValue('SR');
    this.SR = true;
  }

  getMax() {
    const obj = {
      vchType: this.saleInvoiceForm.get('vchType').value,
    };

    this.apiService
      .getDataById('Sale/GetDoMaxNo', obj)
      .subscribe((data) => {
        this.saleInvoiceForm.get('invNo')?.patchValue(data[0].VCHNO);
      });
  }

  getCategory() {

    this.apiService.getData('Inventory/GetCategory').subscribe((result) => {
      this.categoryList = result;
    });
  }

  onChangeCategory(event: any) {
    if (event == undefined) {
      this.productList = [];
      return;
    }
    this.getProductList(event.id, '', '');
  }

  onInputSearchProduct(event: any) {
    if (event.target.value.length == 0) {
      this.productList = [];
      return;
    }
    if (event.target.value.length > 3) {
      this.getProductList(0, event.target.value, '');
    }
  }

  onInputSearchBarcode(event: any) {
    if (event.target.value.length == 0) {
      this.productList = [];
      return;
    }
    this.getProductList(0, '', event.target.value);
  }

  getProductList(categoryId: any, productName: any, barcode: any) {
    const obj = {
      categoryId: categoryId,
      productName: productName,
      barCode: barcode,
      vchType: this.saleInvoiceForm.get('vchType').value,
      invoiceDate: this.dp.transform(new Date(), 'yyyy/MM/dd'),
    };

    this.apiService
      .getDataById('Sale/GetDoProductList', obj)
      .subscribe((data) => {
        this.productList = data;
        data.forEach((x, i) => {
          x.UomList = this.uomList.filter(
            (z) => z.CODE.slice(-5) === x.CODE.slice(-5)
          );
          data[i] = x;
        });
        this.productList = data;
      });
  }

  onChangeUom(i: any, uoms: any[]) {
    const uom = uoms.find((z) => z.UOMID == i.target.value);
    const index = this.productList.findIndex(
      (x) => x.CODE.slice(-5) === uom.CODE.slice(-5)
    );
    const product = this.productList.find(
      (x) => x.CODE.slice(-5) === uom.CODE.slice(-5)
    );
    product.MAXRATE = uom.MAXRARE;
    product.MINRATE = uom.MINRATE;
    product.UOM = uom.UOM;
    product.UOMID = uom.UOMID;
    product.PACKING = uom.PACKING;
    product.BASEPACKING = uom.BASPACKING;

    this.productList[index] = product;
  }

  onClickPlusQty(i: any) {
    const index = this.appendedData.findIndex(
      (x) =>
        x.CODE.slice(-5) == i.CODE.slice(-5) &&
        x.UOMID == i.UOMID &&
        x.SID == i.SID
    );

    this.productRow = { ...i };
    this.onQty = 1;
    this.saleRate = i.MAXRATE;
    this.balance = i.BALANCE;
    this.amount = this.com.roundVal((this.onQty * this.saleRate).toFixed(2));

    if (index !== -1) {
      this.onQty = this.appendedData[index].QTY;
    }
    $('#QtyModal').modal('show');

    setTimeout(() => {
      $('.onQty').select();
    }, 200);
  }

  onClickMinusQty(i: any) {
    const index = this.appendedData.findIndex(
      (x) =>
        x.CODE.slice(-5) == i.CODE.slice(-5) &&
        x.UOMID == i.UOMID &&
        x.SID == i.SID
    );

    if (this.appendedData[index].QTY > 1) {
      this.appendedData[index].QTY =
        parseFloat(this.appendedData[index].QTY) - 1;
    }

    this.calculation();
  }

  onClickPlus(i: any) {
    this.productRow = { ...i };
    let dp = i.EXPIRYDATE.split('/');
    this.onQty = i.QTY;
    this.location = i.SID;
    this.batchNo = i.BATCHNO;
    this.expiryDate = new Date(dp[2], dp[1] - 1, dp[0]);
    this.saleRate = i.MAXRATE;
    this.balance = i.BALANCE;
    this.amount = this.com.roundVal((this.onQty * this.saleRate).toFixed(2));
    $('#QtyModal').modal('show');

    setTimeout(() => {
      $('.onQty').select();
    }, 100);
  }

  onInputModalCla(e: any, tag: any) {
    let row = this.productRow;
    const u = row.UomList.find((x) => x.UOMID == row.UOMID);

    if (
      u.PACKING == '1' &&
      this.onQty >= parseFloat(u.BASPACKING) &&
      u.PACKING != parseFloat(u.BASPACKING)
    ) {
      this.onQty = parseFloat(u.BASPACKING) - 1;
      if (tag == 'Q') {
        e.target.value = parseFloat(u.BASPACKING) - 1;
      }
    }

    let qty =
      parseFloat(this.getNum(this.onQty)) * parseFloat(this.getNum(u.PACKING));
    if (qty > parseFloat(row.STOCK)) {
      this.onQty = Math.floor(parseFloat(row.STOCK) / parseFloat(u.PACKING));

      if (tag == 'Q') {
        e.target.value = Math.floor(
          parseFloat(row.STOCK) / parseFloat(u.PACKING)
        );
      }
    }

    this.amount = this.com.roundVal((this.onQty * this.saleRate).toFixed(2));
  }

  onClickRemove(i: any, index: any) {
    const confirmDelete = confirm('Are you sure you want to delete this item?');
    if (!confirmDelete) {
      return;
    }
    if (index !== -1) {
      this.appendedData.splice(index, 1);
    }
    this.calculation();
  }

  appendData() {
    if (this.onQty < 0) {
      return;
    }

    let row: any = this.productRow;
    const index = this.appendedData.findIndex(
      (x) =>
        x.CODE.slice(-5) == row.CODE.slice(-5) &&
        x.UOMID == row.UOMID &&
        x.EXPIRYDATE == row.EXPIRYDATE &&
        x.SID == row.SID &&
        x.BATCHNO == row.BATCHNO
    );

    row.QTY = this.onQty;
    row.RATE = this.saleRate;
    row.OLDRATE = this.saleRate;
    row.EXPIRYDATE = this.dp.transform(this.expiryDate, 'dd/MM/yyyy');
    row.SID = this.location;
    row.SUBPARTY = this.subParty;
    row.BATCHNO = this.batchNo;

    if (index !== -1) {
      row.QTY = this.onQty;
      row.RETQTY = this.appendedData[index].RETQTY;
      this.appendedData[index] = row;
      $('#QtyModal').modal('hide');
      this.calculation();
      return;
    }

    row.RETQTY = 0;
    this.appendedData.push(row);

    console.log(this.appendedData)
    $('#QtyModal').modal('hide');
    this.calculation();

  }

  onInputDisAmt() {
    const disAmt = this.saleInvoiceForm.get('discountAmt').value;
    const grossAmount = this.saleInvoiceForm.get('grossAmount').value;

    if (disAmt < 0 || disAmt == '' || disAmt == null) {
      this.saleInvoiceForm.get('discountAmt').patchValue(0);
    }

    if (parseFloat(disAmt) > parseFloat(grossAmount)) {
      this.saleInvoiceForm.get('discountAmt').patchValue(grossAmount);
    }

    var disPercent = (parseFloat(disAmt) / parseFloat(grossAmount)) * 100;
    this.saleInvoiceForm
      .get('discount')
      .patchValue(this.com.roundVal(disPercent.toFixed(2)));
    this.calculation();
  }

  onInputDis() {
    const discount = this.saleInvoiceForm.get('discount').value;
    const grossAmount = this.saleInvoiceForm.get('grossAmount').value;

    if (discount < 0 || discount == '' || discount == null) {
      this.saleInvoiceForm.get('discount').patchValue(0);
    }

    if (discount > 100) {
      this.saleInvoiceForm.get('discount').patchValue(100);
    }

    var disAmt = this.com.roundVal(
      ((parseFloat(grossAmount) * parseFloat(discount)) / 100).toFixed(2)
    );
    this.saleInvoiceForm.get('discountAmt').patchValue(disAmt);
    this.calculation();
  }

  onInputQty(event: any, i: any, index: any) {
    if (i.QTY < 0 || i.QTY == null) {
      event.target.value = 0;
      i.QTY = 0;
    }

    // const x = i.UomList.find((x) => x.UOMID == i.UOMID);

    // i.PACKING = x.PACKING;
    // i.BASEPACKING = x.BASPACKING;

    if (
      i.PACKING == '1' &&
      parseFloat(i.QTY) >= parseFloat(i.BASEPACKING) &&
      i.PACKING != parseFloat(i.BASEPACKING)
    ) {
      event.target.value = parseFloat(i.BASEPACKING) - 1;
      i.QTY = parseFloat(i.BASEPACKING) - 1;
    }

    const vchType = this.saleInvoiceForm.get('vchType').value;
    if (vchType == 'SP') {
      let stockArray = this.getStock(i.CODE, i.EXPIRYDATE, i.LOCATION, index);
      let stockInHand =
        parseFloat(this.getNum(i.STOCK)) +
        parseFloat(this.getNum(stockArray[1]));
      let stockAvalible =
        parseFloat(this.getNum(stockInHand)) -
        parseFloat(this.getNum(stockArray[0]));
      let qty =
        parseFloat(this.getNum(i.QTY)) * parseFloat(this.getNum(i.PACKING));
      if (qty > stockAvalible) {
        event.target.value =
          Math.floor(stockAvalible / parseFloat(i.PACKING)) <= 0
            ? 0
            : Math.floor(stockAvalible / parseFloat(i.PACKING));
        i.QTY =
          Math.floor(stockAvalible / parseFloat(i.PACKING)) <= 0
            ? 0
            : Math.floor(stockAvalible / parseFloat(i.PACKING));
      }
    }

    if (i.SLABRATE != 0) {
      if (
        parseFloat(i.QTY) >= parseFloat(i.FROMQTY) &&
        parseFloat(i.QTY) <= parseFloat(i.TOQTY)
      ) {
        i.RATE = i.SLABRATE;
      } else if (parseFloat(i.QTY) > parseFloat(i.TOQTY)) {
        i.RATE = i.ABOVESLABRATE;
      } else {
        i.RATE = i.OLDRATE;
      }
    }

    this.calculation();
  }

  getStock(code: any, expiry: any, location: any, index: any) {
    let stock = 0;
    let retStock = 0;

    this.appendedData.forEach((x, i) => {
      if (x.CODE == code && x.EXPIRYDATE == expiry && x.LOCATION == location) {
        let retQty = 0;
        if (
          parseFloat(this.getNum(x.PACKING)) == 0 ||
          parseFloat(this.getNum(x.PACKING)) == 1
        ) {
          retStock += parseFloat(this.getNum(retQty)) * 1;
        } else {
          retStock +=
            parseFloat(this.getNum(retQty)) *
            parseFloat(this.getNum(x.PACKING));
        }

        if (i != index) {
          if (
            parseFloat(this.getNum(x.PACKING)) == 0 ||
            parseFloat(this.getNum(x.PACKING)) == 1
          ) {
            stock += parseFloat(this.getNum(x.QTY)) * 1;
          } else {
            stock +=
              parseFloat(this.getNum(x.QTY)) *
              parseFloat(this.getNum(x.PACKING));
          }
        }
      }
    });
    return [stock, retStock];
  }

  rowDis(i: any){
    if (parseFloat(i.DISCOUNT) > 100) {
      i.DISCOUNT = 100;
    }
    i.DISAMOUNT = this.com.roundVal(((parseFloat(i.VALUE) * parseFloat(i.DISCOUNT)) / 100).toFixed(2));
    this.calculation();
  }

  rowDisAmt(i: any){
    if (parseFloat(i.DISAMOUNT) > parseFloat(i.VALUE)) {
      i.DISAMOUNT = i.VALUE;
    }
    i.DISCOUNT = ((parseFloat(i.DISAMOUNT) / parseFloat(i.VALUE)) * 100).toFixed(2);
    this.calculation();
  }

  calculation() {
    let grossAmount = 0;
    let productValue = 0;

    this.appendedData.forEach((x) => {
      x.NETQTY = this.getNum(x.QTY) - this.getNum(x.RETQTY);
      x.VALUE = this.com.roundVal(
        this.getNum(parseFloat(x.NETQTY) * parseFloat(x.RATE)).toFixed(2)
      );
      productValue += parseFloat(x.VALUE);

      if (parseFloat(x.SALETAX) > 100) {
        x.SALETAX = 100;
      }
      x.SALETAXAMT = this.com.roundVal(
        this.getNum(
          (parseFloat(x.SALETAX) *
            (this.getNum(x.VALUE) - this.getNum(x.DISAMOUNT))) /
            100
        ).toFixed(2)
      );
      x.NETVALUE = this.com.roundVal(
        (
          parseFloat(this.getNum(x.VALUE)) -
          parseFloat(this.getNum(x.DISAMOUNT)) +
          parseFloat(this.getNum(x.SALETAXAMT))
        ).toFixed(2)
      );
      grossAmount += parseFloat(x.NETVALUE);
    });

    this.saleInvoiceForm
      .get('grossAmount')
      .setValue(this.com.roundVal(grossAmount.toFixed(2)));

    const otherCredit = this.saleInvoiceForm.get('otherCredit').value;
    const shipment = this.saleInvoiceForm.get('shipment').value;
    const discountAmt = this.saleInvoiceForm.get('discountAmt').value;
    const fTax = this.saleInvoiceForm.get('fTax').value;
    const whTax = this.saleInvoiceForm.get('whTax').value;

    let due = this.com.roundVal(
      ((grossAmount + parseFloat(shipment)) - (parseFloat(discountAmt) + parseFloat(otherCredit))).toFixed(2)
    );

    let fTaxAmt = this.com.roundVal(
      ((parseFloat(fTax) * productValue) / 100).toFixed(2)
    );
    this.saleInvoiceForm.get('fTaxAmt').setValue(fTaxAmt);

    let whTaxAmt =
      (parseFloat(whTax) * (parseFloat(due) + parseFloat(fTaxAmt))) / 100;
    this.saleInvoiceForm
      .get('whTaxAmt')
      .setValue(this.com.roundVal(whTaxAmt.toFixed(2)));

    let totalDue = this.com.roundVal(
      (parseFloat(due) + parseFloat(fTaxAmt) + whTaxAmt).toFixed(2)
    );
    this.saleInvoiceForm
      .get('totalDue')
      .setValue(this.com.roundVal(parseFloat(totalDue).toFixed(2)));

    let recAmount = this.saleInvoiceForm.get('recAmount').value;

    if (parseFloat(recAmount) > totalDue) {
      this.saleInvoiceForm
        .get('retAmount')
        .setValue(
          this.com.roundVal((parseFloat(recAmount) - totalDue).toFixed(2))
        );
      this.netDue = 0;
    } else {
      this.saleInvoiceForm.get('retAmount').setValue(0);
      this.netDue = this.com.roundVal(
        this.getNum(totalDue - parseFloat(recAmount)).toFixed(2)
      );
    }
  }


  onClickSave(): void {

    let body = this.saleInvoiceForm.value;
    body.invNo = this.saleInvoiceForm.get('invNo').value;
    body.locId = this.saleInvoiceForm.get('locId').value;
    body.grossAmount = this.saleInvoiceForm.get('grossAmount').value;
    body.fTax = this.saleInvoiceForm.get('fTax').value;
    body.fTaxAmt = this.saleInvoiceForm.get('fTaxAmt').value;
    body.whTax = this.saleInvoiceForm.get('whTax').value;
    body.whTaxAmt = this.saleInvoiceForm.get('whTaxAmt').value;
    body.totalDue = this.saleInvoiceForm.get('totalDue').value;
    body.retAmount = this.saleInvoiceForm.get('retAmount').value;

    if (this.appendedData.length == 0) {
      this.tostr.warning('Add Product....!');
      return;
    }

    if (body.locId == null || body.locId == undefined) {
      this.tostr.warning('Select Location...!');
      return;
    }

    if (body.currency == null || body.currency == undefined) {
      this.tostr.warning('Select Currency...!');
      return;
    }

    if (body.partyMain == null || body.partyMain == undefined) {
      this.tostr.warning('Select Party Main...!');
      return;
    }

    if (body.partySub == null || body.partySub == undefined) {
      this.tostr.warning('Select Party Sub...!');
      return;
    }

    if (body.grossAmount == null || body.grossAmount == undefined) {
      this.tostr.warning('Make Gross Amount greater than 0...!');
      return;
    }

    try {
      this.com.showLoader();

      let vDate = this.dp.transform(body.vchDate, 'yyyy-MM-dd');
      const dp: any = vDate.split('-');
      const dueDate = new Date(
        dp[0],
        dp[1] - 1,
        parseInt(dp[2]) + parseInt(body.termsDay)
      );

      const invoice: any[] = this.appendedData.map((data) => ({
        doNo: (body.invNo).toString(),
        DoDate: this.dp.transform(body.vchDate, 'yyyy-MM-dd'),
        DueDate: this.dp.transform(dueDate, 'yyyy-MM-dd'),
        GodownId: body.godown == undefined || body.godown == null ? 0 : parseInt(body.godown),
        CurrencyId: parseInt(body.currency),
        PaymentType: (body.creditDebit).toString(),
        PartyCode: body.partySub,
        PartyName: this.partySubList.find((x) => x.CODE == body.partySub).NAME,
        ProductCode: data.CODE,
        Qty: (data.QTY).toString(),
        Rate: (data.RATE).toString(),
        UomId: data.UOMID.toString(),
        Remarks: body.remarks,
        dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      }));

      this.apiService
        .saveData('Sale/AddDO', invoice)
        .subscribe((result) => {
          if (result == true || result == 'true') {
            this.tostr.success('Save Successfully');
            this.getInvoices();
            this.formStatus = 'Edit';
            this.isPrint = true;
            this.com.hideLoader();
            this.onClickRefresh();
          } else {
            this.com.hideLoader();
            this.tostr.error('Please Save Again');
          }
        });
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      this.com.hideLoader();
    }
  }

  deleteInvoice(invNo: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (!confirmDelete) {
      return;
    }

    try {
      this.com.showLoader();
      const obj = {
        vchno: invNo,
        vchType: this.saleInvoiceForm.get('vchType').value,
        dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      };

      this.apiService.deleteData('Sale/DeleteDo', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.getInvoices();
            this.onClickRefresh();
            this.com.hideLoader();
            this.tostr.success('Delete Successfully');
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
      this.com.hideLoader();
    }
  }


  async editInvoice(invNo: any) {

    try {
      this.com.showLoader();

      const obj = {
        vchNo: invNo,
        vchType: this.saleInvoiceForm.get('vchType').value,
      };

      const data = await this.apiService
        .getDataById('Sale/EditDo', obj)
        .toPromise();

      this.isPrint = true;
      this.isDisabled = false;
      this.isNewClick = true;
      this.formStatus = 'Edit';
      this.appendedData = [];

      let fr = data[0];

      this.saleInvoiceForm.get('invNo')?.patchValue(fr.VCHNO);

      this.saleInvoiceForm
        .get('vchDate')
        ?.patchValue(
          new Date(
            fr.VCHDATE.split('/')[2],
            fr.VCHDATE.split('/')[1] - 1,
            fr.VCHDATE.split('/')[0]
          )
        );

       this.saleInvoiceForm.get('locId')?.patchValue(fr.LOCID);
       this.saleInvoiceForm.get('currency')?.patchValue(fr.CURRENCYID);
       this.saleInvoiceForm.get('creditDebit')?.patchValue(fr.BVCHTYPE);
       this.saleInvoiceForm.get('godown')?.patchValue(fr.GODOWNID);
       this.saleInvoiceForm.get('partyMain')?.patchValue(fr.PARTYCODE.substring(0, 9));
       await this.getPartySub(fr.PARTYCODE.substring(0, 9))
       this.saleInvoiceForm.get('partySub')?.patchValue(fr.PARTYCODE);

      data.forEach((item: any) =>  {

        this.appendedData.push(item);

        this.calculation();
        this.com.hideLoader();
      });
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      this.com.hideLoader();
    }

    $('.autoClose').click();
  }



  getPaymentList() {
    const obj = {
      invNo: this.saleInvoiceForm.get('invNo')?.value,
      invType: this.saleInvoiceForm.get('vchType')?.value,
    };

    this.apiService
      .getDataById('Sale/OldPaymentList', obj)
      .subscribe((data) => {
        this.paymentList = data;
      });
  }

  getBankCashList() {
    this.apiService.getData('Accounts/GetPRPBankCash').subscribe((data) => {
      this.bankCashList = data;
    });
  }

  saveBankCash(): void {
    try {
      this.com.showLoader();
      const obj = {
        BankCash: this.bankCash,
        partyCode: '00123',
        VchDate: this.dp.transform(this.paymentDate, 'yyyy/MM/dd'),
        Payment: this.payment,
        TotalPayment: this.payment,
        ChequeNo: this.chequeNo,
        ChequeDate: this.dp.transform(this.chequeDate, 'yyyy/MM/dd'),
        VchType: 'CP',
        vchno: '1021',
        status: 'New',
        invNo: this.saleInvoiceForm.get('invNo')?.value,
        invType: this.saleInvoiceForm.get('vchType')?.value,
      };

      this.apiService.saveObj('Sale/SaveBankCash', obj).subscribe((result) => {
        if (result == true || result == 'true') {
          this.tostr.success('Save Successfully');
          this.com.hideLoader();
          this.onClickRefresh();
        } else {
          this.tostr.error('Please Save Again');
          this.com.hideLoader();
        }
      });
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  deleteBankCash(item: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {
      try {
        this.com.showLoader();
        const obj = {
          Vchno: item.vchno,
          Vchtype: item.vchtype,
          amount: item.amount,
          invoiceno: this.saleInvoiceForm.get('invNo')?.value,
          invType: this.saleInvoiceForm.get('vchType')?.value,
          dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
        };

        this.apiService.deleteData('Sale/DeletePayment', obj).subscribe({
          next: (data) => {
            if (data == 'true' || data == true) {
              this.getPaymentList();
              this.tostr.success('Delete Successfully');
              this.com.hideLoader();
            } else if (data == 'false' || data == false) {
              this.tostr.error('Delete Again');
              this.com.hideLoader();
            }
          },
          error: (error) => {
            this.tostr.info(error.error.text);
            this.com.hideLoader();
          },
        });
      } catch (err) {
        this.com.hideLoader();
        console.log(err);
      } finally {
        //this.com.hideLoader();
      }
    }
  }

  refreshPayment() {
    this.payment = '';
    this.totalPayment = '';
    this.chequeNo = '';
    this.paymentDate = new Date();
    this.chequeDate = new Date();
    this.bankCash = undefined;
    this.isDisabledPayment = true;
    this.isShowPayment = false;
  }

  newPayment() {
    this.refreshPayment();
    this.isDisabledPayment = false;
    this.isShowPayment = true;
  }

  // =================== OTHER FUNCTIONS ==================//

  getNum(val) {
    if (val == '') {
      val = 0;
    }
    if (isNaN(val) || val == Infinity) {
      return 0;
    }
    return val;
  }

  onLength(event: any, length: number) {
    if (event.target.value > length) {
      event.target.value = length;
    }
  }

  onZero(e: any) {
    if (e.target.value == '' || e.target.value == null) {
      e.target.value = 0;
    }
  }

  //======================= PRODUCT DETAIL MODAL =====================//

  openProductModal(i: any) {
    $('#PDModal').modal('show');

    this.mProduct = i.PRODUCT;
    this.mDescriptiion = i.DES;
    this.mCategory = i.CATEGORY;
    this.mBrand = i.BRAND;
    this.mMadeIn = i.MADEIN;
    this.mUom = i.UOM;
    this.mSku = i.LOCATION;
    this.mWarehouse = i.GODOWNNAME;
    this.mRack = i.RACKNAME;
    this.mShelf = i.SHELFNAME;
    this.mExpiry = i.EXPIRY;
    this.mStock = i.STOCK;
    this.mTax = i.SALETAX;
    this.mDiscount = i.DISCOUNT;
    this.mRate = i.MAXRATE;
    this.mStatus = i.INACTIVE;
    this.mImage = this.basePath + i.IMAGE;
  }

  //======================= TERMS =====================//

  getTerms() {
    this.apiService.getData('Purchase/GetTerms').subscribe((data) => {
      this.termsList = data;
    });
  }

  createUpdateTerms() {
    const obj = {
      id: this.termsId,
      name: this.termsDays,
      dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
    };

    if (this.termsDays == '' || this.termsDays == null) {
      this.tostr.warning('Enter Days....!');
      return;
    }

    this.apiService
      .saveObj('Purchase/AddUpdateTerms', obj)
      .subscribe((result) => {
        if (result == true || result == 'true') {
          this.getTerms();
          this.refreshTerms();
          this.tostr.success('Save Successfully');
        } else {
          this.tostr.error('Please Save Again');
        }
      });
  }

  editTerms(id: any, name: any, alias: any): void {
    this.termsDays = name;
    this.termsId = id;
    this.isDisabledTerms = false;
    this.isShowTerms = true;
  }

  deleteTerms(id: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {
      const obj = {
        id: id,
        dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      };

      this.apiService.deleteData('Purchase/DeleteTerms', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.getTerms();
            this.refreshTerms();
            this.tostr.success('Delete Successfully');
          } else if (data == 'false' || data == false) {
            this.tostr.error('Delete Again');
          }
        },
        error: (error) => {
          this.tostr.info(error.error.text);
        },
      });
    }
  }

  refreshTerms() {
    this.termsDays = '';
    this.termsId = 0;
    this.isDisabledTerms = true;
    this.isShowTerms = false;
  }

  newTerms() {
    this.refreshTerms();
    this.isDisabledTerms = false;
    this.isShowTerms = true;
  }

}
