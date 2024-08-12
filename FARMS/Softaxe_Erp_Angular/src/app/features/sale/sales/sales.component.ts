import { Component, ViewChild, ElementRef } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { environment } from '../../../../environment/environmemt';
import { CommonService } from 'src/app/services/common.service';
import { ActivatedRoute } from '@angular/router';
declare const $: any;

@Component({
  selector: 'app-sales',
  templateUrl: './sales.component.html',
  styleUrls: ['./sales.component.css'],
})
export class SalesComponent {
  @ViewChild('InvoiceList', { static: false }) InvoiceList!: ElementRef;
  
  basePath = environment.basePath;
  saleInvoiceForm!: FormGroup;
  costCenter = localStorage.getItem('costCenter');
  JobList: any[] = [];
  jobNo: any = null;
  formTag: any = '';
  isCategory: boolean = false;
  rowIndex: any;

  // PRODUCT
  locationList: any[] = [];
  uomList: any[] = [];
  categoryList: any[] = [];
  category: any = null;
  productName: string = '';
  barCode: any = '';
  productList: any[] = [];
  isDiscount: boolean = false;

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

  // DELIVERY PERSON
  delPer = '';
  delPerId = 0;
  deliveryPerList: any[] = [];
  isDisabledDelPer: boolean = true;
  isShowDelPer: boolean = false;

  // Qty MODAL
  balance: any;
  onQty: number;
  location: any;
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
  orderTakerList: any[] = [];
  customerList: any[] = [];
  locList: any[] = [];
  netDue: any;
  productRow: any;
  appendedData: any[] = [];
  totalQty: any;
  isNewClick: boolean = false;
  isDisabled: boolean = true;
  isTaxDisable: boolean = false;
  formStatus: string = '';
  isPrint: boolean = false;
  paymentMethod: any = 'Cash';

  constructor(
    private apiService: ApiService,
    private fb: FormBuilder,
    private tostr: ToastrService,
    private dp: DatePipe,
    private auth: AuthService,
    private com: CommonService,
    private router: ActivatedRoute
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

  async ngOnInit() {
    this.formInit();
    this.getUoms();
    this.getLocation();
    this.getLoc();
    this.getCategory();
    this.getDeliveryPer();
    this.getOrderTakerList();
    this.getTerms();
    this.router.queryParams.subscribe((params) => {
      this.formTag = params.tag
      this.getCustomer(params.tag);
      this.onClickRefresh();
      if(this.formTag == "GetCustomer"){
        this.isCategory = false;
      }
      else{
        this.isCategory = true;
      }
    });
    //this.getBankCashList();
    //this.getPaymentList();
    
    this.JobList = await this.com.getJobList(true);
  }

  formInit() {
    this.saleInvoiceForm = this.fb.group({
      doNo: [0],
      invNo: [0],
      vchType: ['SP'],
      vchDate: [new Date()],
      locId: [''],
      deliveryBoy: [null],
      orderTaker: [null],
      party: [null],
      walkingName: [0],
      walkingContact: [0],
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
      remarksAmountPaid: [''],
      retAmount: [0],
      termsDay: ['30'],
      saleRemarks: [''],
    });

    this.saleInvoiceForm.get('doNo').disable();
    this.saleInvoiceForm.get('invNo').disable();
    this.saleInvoiceForm.get('locId').disable();
    this.saleInvoiceForm.get('grossAmount').disable();
    //this.saleInvoiceForm.get('fTax').disable();
    this.saleInvoiceForm.get('fTaxAmt').disable();
    //this.saleInvoiceForm.get('whTax').disable();
    this.saleInvoiceForm.get('whTaxAmt').disable();
    this.saleInvoiceForm.get('totalDue').disable();
    this.saleInvoiceForm.get('retAmount').disable();
  }

  resetForm() {
    let vDate = this.saleInvoiceForm.get('vchDate').value;
    this.category = null;
    this.productName = '';
    this.formStatus = '';
    this.barCode = '';
    this.netDue = '0.00';
    this.isPrint = false;
    this.isNewClick = false;
    this.SR = false;
    this.jobNo = null;
    this.productList = [];
    this.appendedData = [];
    this.saleInvoiceForm.reset();
    this.saleInvoiceForm.get('doNo').setValue(0);
    this.saleInvoiceForm.get('invNo').setValue(0);
    this.saleInvoiceForm.get('vchType').setValue('SP');
    this.saleInvoiceForm.get('vchDate').setValue(vDate);
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
    this.saleInvoiceForm.get('party').enable();
  }

  async onClickNew() {
    let type = this.saleInvoiceForm.get('vchType').value;
    let result = this.com.isStopEntry(type);
    
    if(!result){
      this.tostr.info("You are not allowed")
      return;
    }

    if(this.orderTakerList.length > 0){
      this.saleInvoiceForm.get('orderTaker').patchValue(this.orderTakerList[0].id ?? 0)
      this.saleInvoiceForm.get('deliveryBoy').patchValue(this.deliveryPerList[0].id ?? 0)
    }
    this.isDisabled = false;
    this.isNewClick = true;
    this.formStatus = 'New';
    await this.getMax();
  }

  onClickRefresh() {
    this.isDisabled = true;
    this.totalQty = 0;
    this.resetForm();
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
      tag: this.formTag,
    };

    this.apiService
      .getDataById('Sale/GetInvoiceList', obj)
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

  async getMax() {
    const obj = {
      vchType: this.saleInvoiceForm.get('vchType').value,
    };

    const data = await this.apiService.getDataById('Accounts/GetMaxNumber', obj).toPromise();
    this.saleInvoiceForm.get('invNo')?.patchValue(data[0].VCHNO);
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
      .getDataById('Sale/GetProductList', obj)
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

  getOrderTakerList() {
    this.apiService.getData('Sale/GetOrderTakerList').subscribe((data) => {
      this.orderTakerList = data;
    });
  }

  getCustomer(param: any) {
    this.apiService.getDataById(`Sale/${param}`, {status: true}).subscribe((data) => {
      this.customerList = data.party ?? data;
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
        x.EXPIRYDATE == i.EXPIRYDATE &&
        x.SID == i.SID
    );

    this.productRow = { ...i };
    this.onQty = 1;
    let dp = i.EXPIRYDATE.split('/');
    this.location = i.SID;
    this.batchNo = i.BATCHNO;
    this.expiryDate = new Date(dp[2], dp[1] - 1, dp[0]);
    this.saleRate = i.MAXRATE;
    this.balance = i.BALANCE;
    this.amount = this.com.roundVal((this.onQty * this.saleRate).toFixed(2));
    //this.jobNo = i.JOBNO;

    const vchType = this.saleInvoiceForm.get('vchType').value;
    if(vchType == "SR"){
      this.location = this.locationList[0].id
    }

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
        x.EXPIRYDATE == i.EXPIRYDATE &&
        x.SID == i.SID
    );

    if (this.appendedData[index].QTY > 1) {
      this.appendedData[index].QTY =
        parseFloat(this.appendedData[index].QTY) - 1;
    }

    this.calculation();
  }

  onClickPlus(i: any, index: any) {

    this.rowIndex = index;

    this.productRow = { ...i };
    let dp = i.EXPIRYDATE.split('/');
    this.onQty = i.QTY;
    this.location = i.SID;
    this.batchNo = i.BATCHNO;
    this.expiryDate = new Date(dp[2], dp[1] - 1, dp[0]);
    this.saleRate = i.RATE;
    this.balance = i.BALANCE;
    this.amount = this.com.roundVal((this.onQty * this.saleRate).toFixed(2));
    //this.jobNo = i.JOBNO;
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

    let qty = parseFloat(this.getNum(this.onQty)) * parseFloat(this.getNum(u.PACKING));
    if (qty > parseFloat(row.STOCK)) {
      if(!row.NOSTOCK){
        this.onQty = Math.floor(parseFloat(row.STOCK) / parseFloat(u.PACKING));

        if (tag == 'Q') {
          e.target.value = Math.floor(parseFloat(row.STOCK) / parseFloat(u.PACKING));
        }
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

  oldDate: Date;

  onClickInvDate(){
    this.oldDate = this.saleInvoiceForm.get('vchDate').value;
  }

  onChangeVchDate(){
    let date: Date = this.saleInvoiceForm.get('vchDate').value;
    if(this.dp.transform(this.oldDate,"yyyy/MM/dd") > this.dp.transform(date,"yyyy/MM/dd")){
      if(this.appendedData.length > 0){
        this.tostr.warning("Can't Change  Invoice Date")
        this.saleInvoiceForm.get('vchDate').setValue(this.oldDate)
        return ;
      }
    }
  }

  appendData() {

    if (this.onQty < 0) {
      return;
    }
    let row: any = this.productRow;

    if(!row.NOSTOCK){
      if(this.location == null){
        this.tostr.warning("Select Locaion....!")
        return;
      }
    }

    if(this.formTag == 'GetCustomer'){
      if(row.CATEGORY == "Broiler"){
        if(this.jobNo == null){
          this.tostr.warning("Select Job No....!")
          return;
        }
      }
    }

    const index = this.appendedData.findIndex(
      (x) =>
        x.CODE.slice(-5) == row.CODE.slice(-5) &&
        x.UOMID == row.UOMID &&
        x.EXPIRYDATE == row.EXPIRYDATE &&
        x.SID == row.SID &&
        x.BATCHNO == row.BATCHNO &&
        x.NETQTY == row.NETQTY
    );

    row.QTY = this.onQty;
    row.RATE = this.saleRate;
    row.RATEDIFF = this.saleRate;
    row.OLDRATE = this.saleRate;
    row.EXPIRYDATE = this.dp.transform(this.expiryDate, 'dd/MM/yyyy');
    row.SID = this.location;
    row.BATCHNO = this.batchNo;
    //row.JOBNO = this.jobNo;

    if (index !== -1) {
      row.QTY = this.onQty;
      row.RETQTY = this.appendedData[index].RETQTY;
      this.appendedData[index] = row;
      $('#QtyModal').modal('hide');
      this.calculation();
      this.rowDis(row);
      this.rowIndex = -1;
      return;
    }

    row.RETQTY = 0;
    this.appendedData.push(row);
    $('#QtyModal').modal('hide');
    this.calculation();
    this.rowDis(row);
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
    if(this.isDiscount){
      this.calculation();
    }
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

    if(this.isDiscount){
      this.calculation();
    }
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

      if(!i.NOSTOCK){

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
    let allTotalQty = 0;

    let grossAmount = 0;
    let productValue = 0;

    this.appendedData.forEach((x) => {
      x.NETQTY = this.getNum(x.QTY) - this.getNum(x.RETQTY);

      allTotalQty += x.NETQTY;
      x.VALUE = this.com.roundVal(
        this.getNum(parseFloat(x.NETQTY) * parseFloat(x.RATE)).toFixed(2)
      );
      productValue += parseFloat(x.VALUE);

      if (parseFloat(x.DISCOUNT) > 100) {
        x.DISCOUNT = (100).toFixed(0);
      }
      x.DISAMOUNT = this.com.roundVal(
        ((this.getNum(x.DISCOUNT) * this.getNum(x.VALUE)) / 100).toFixed(2)
      );

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

    this.totalQty = allTotalQty;

    this.saleInvoiceForm
      .get('grossAmount')
      .setValue(this.com.roundVal(grossAmount.toFixed(2)));

    this.isDiscount = false

    if(!this.isDiscount){
      this.onInputDis();
    }
    this.isDiscount = true

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

  onChangeParty() {
    const cus = this.customerList.find(
      (x) => x.code == this.saleInvoiceForm.get('party').value
    );

    if(this.formTag == "GetCostCodes"){
      this.saleInvoiceForm.get('party').disable();
      const cat = this.categoryList.find((x) => x.CODE == this.saleInvoiceForm.get('party').value)
      this.category = cat.id
      this.onChangeCategory({id:cat.id})
    }
    else{
      this.saleInvoiceForm.get('party').enable();
    }

    if (cus == undefined) {
      this.saleInvoiceForm.get('discount').patchValue(0);
    } else {
      this.saleInvoiceForm.get('discount').patchValue(cus.commission ?? 0);
    }

    this.onInputDis();

    this.isTaxDisable = false;

    if (cus.saleTax == 'nonfiler') {
      this.saleInvoiceForm
        .get('fTax')
        .setValue(localStorage.getItem('furtherTax'));
    } else if (cus.saleTax == 'filer') {
      this.saleInvoiceForm.get('fTax').setValue(0);
    } else if (cus.saleTax == 'taxfree') {
      this.saleInvoiceForm.get('fTax').setValue(0);
      this.appendedData.forEach((x, i) => {
        x.SALETAX = 0;
      });
      this.isTaxDisable = true;
    }

    if (cus.whTax == 'nonfiler') {
      this.saleInvoiceForm
        .get('whTax')
        .setValue(localStorage.getItem('whNonFiler'));
    } else if (cus.whTax == 'filer') {
      this.saleInvoiceForm
        .get('whTax')
        .setValue(localStorage.getItem('whFiler'));
    } else if (cus.whTax == 'taxfree') {
      this.saleInvoiceForm.get('whTax').setValue(0);
    } else {
      this.saleInvoiceForm.get('whTax').setValue(0);
    }

    this.calculation();
  }

  onChangeJobNo(){
    if(this.formTag != "GetCustomer"){
      let type = this.saleInvoiceForm.get('vchType').value;
      if(type == "SR"){

        if(this.jobNo == null){
          this.saleInvoiceForm.get('discount').setValue(0);
          this.onInputDis();
          return;
        }
        
        let cat = this.categoryList.find((x) => x.id == this.category)
        if(cat.isCommission){
          let job = this.JobList.find((x) => x.ID == this.jobNo);
          this.saleInvoiceForm.get('discount').setValue(job.COMM);
          this.onInputDis();
        }
      }
    }
  }

  onClickSave(): void {

    let type = this.saleInvoiceForm.get('vchType').value;
    let result = this.com.isStopEntry(type);

    if(!result){
      this.tostr.info("You are not allowed")
      return;
    }

    let body = this.saleInvoiceForm.value;
    body.doNo = this.saleInvoiceForm.get('doNo').value;
    body.invNo = this.saleInvoiceForm.get('invNo').value;
    body.party = this.saleInvoiceForm.get('party').value;
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

    if(this.formTag == "GetCostCodes"){
      if(this.jobNo == null){
        this.tostr.warning('Select JobNo...!');
        return;
      }
    }

    if (body.locId == null || body.locId == undefined) {
      this.tostr.warning('Select Location...!');
      return;
    }

    if (body.deliveryBoy == null || body.deliveryBoy == undefined) {
      this.tostr.warning('Select Delivery Person...!');
      return;
    }

    if (body.orderTaker == null || body.orderTaker == undefined) {
      this.tostr.warning('Select Order Taker...!');
      return;
    }

    if (body.party == null || body.party == undefined) {
      this.tostr.warning('Select Party....!');
      return;
    }

    if (body.grossAmount == null || body.grossAmount == undefined) {
      this.tostr.warning('Make Gross Amount greater than 0...!');
      return;
    }

    if (body.vchType == 'SP') {
      if (this.paymentMethod == 'Scan') {
        this.tostr.warning('Payment Method is Selected Wrong...!');
        return;
      }
    } else if (body.vchType == 'SR') {
      if (this.paymentMethod == 'Scan' || this.paymentMethod == 'Debit') {
        this.tostr.warning(
          'Payment Method is Selected Wrong you Just Select Cash...!'
        );
        return;
      }
    }

    if (this.getNum(parseFloat(body.recAmount)) != 0) {
      if (this.paymentMethod == 'Debit') {
        if (body.remarksAmountPaid == '' || body.remarksAmountPaid == null) {
          this.tostr.warning('Enter Remarks Amount Paid...!');
          return;
        }
      }
    }

    try {
      this.com.showLoader();

      let totalNetBill =
        parseFloat(this.getNum(body.grossAmount)) +
        parseFloat(this.getNum(body.whTaxAmt)) +
        parseFloat(this.getNum(body.fTaxAmt)) +
        parseFloat(this.getNum(body.shipment)) -
        (parseFloat(this.getNum(body.discountAmt)) +
          parseFloat(this.getNum(body.otherCredit)));

      let disAmtCredit =
        parseFloat(this.getNum(body.discountAmt)) +
        parseFloat(this.getNum(body.otherCredit));
      let taxP = disAmtCredit / parseFloat(this.getNum(body.grossAmount));

      let vDate = this.dp.transform(body.vchDate, 'yyyy-MM-dd');
      const dp: any = vDate.split('-');
      const dueDate = new Date(
        dp[0],
        dp[1] - 1,
        parseInt(dp[2]) + parseInt(body.termsDay)
      );

      const invoice: any[] = this.appendedData.map((data) => ({
        doNo: body.doNo,
        InvNo: body.invNo,
        Vchtype: body.vchType,
        VchDate: this.dp.transform(body.vchDate, 'yyyy-MM-dd'),
        DueDate: this.dp.transform(dueDate, 'yyyy-MM-dd'),
        LocId: body.locId,
        DeliveryBoy: body.deliveryBoy,
        OrderTaker: body.orderTaker,
        PartyCode: body.party,
        PartyName: this.customerList.find((x) => x.code == body.party).name,
        WalkingName: body.walkingName,
        WalkingContact: body.walkingContact,
        ProductCode: data.CODE,
        ProductName: data.PRODUCT,
        SaleQty: data.QTY,
        RetQty: data.RETQTY,
        NetQty: data.NETQTY,
        Rate: data.RATE,
        RateDiff: data.RATEDIFF,
        OldRate: data.OLDRATE,
        ProductDis: data.DISCOUNT,
        ProductDisAmt: data.DISAMOUNT,
        SaleTax: data.SALETAX,
        SaleTaxAmt: data.SALETAXAMT,
        NetBill: data.NETVALUE,
        Uom: data.UOMID.toString(),
        GID: data.GID ?? 0,
        RID: data.RID ?? 0,
        SID: data.SID ?? 0,
        ExpDate: this.dp.transform(
          new Date(
            data.EXPIRYDATE.split('/')[2],
            data.EXPIRYDATE.split('/')[1] - 1,
            data.EXPIRYDATE.split('/')[0]
          ),
          'yyyy-MM-dd'
        ),
        Discount: body.discount,
        DiscountAmt: body.discountAmt,
        OtherCredit: body.otherCredit,
        Remarks: body.remarks,
        Shipment: body.shipment,
        RecAmount: body.recAmount,
        TotalNetBill: totalNetBill,
        VehicleNo: data.BATCHNO,
        ReturnAmt: body.retAmount,
        TaxP: taxP,
        TermsDays: body.termsDay.toString(),
        FTax: body.fTax,
        FTaxAmt: body.fTaxAmt,
        WHT: body.whTax,
        WHTAmt: body.whTaxAmt,
        Des: 'Sale ' + (body.vchType == 'SP' ? 'Invoice' : 'Return'),
        Status: this.formStatus,
        PaymentMethod: this.paymentMethod,
        PaymentMethodRmk: body.remarksAmountPaid,
        SaleRemarks: body.saleRemarks,
        //JobNo: (this.formTag == "GetCustomer") ? (data.JOBNO ?? 0) : (this.jobNo1 ?? 0),
        JobNo: this.jobNo ?? 0,
        NoStock: data.NOSTOCK,
        Tag: this.formTag,
        dtNow: new Date(),
      }));

      this.apiService
        .saveData('Sale/SaveUpdateInvoice', invoice)
        .subscribe((r) => {
          if (r.status == true || r.status == 'true') {
            this.tostr.success('Save Successfully');
            this.saleInvoiceForm.get('invNo').patchValue(r.vchNo);
            this.getInvoices();
            this.formStatus = 'Edit';
            this.isPrint = true;
            this.com.hideLoader();
            //this.onClickRefresh();
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

  deleteInvoice(invNo: any): void {

    let type = this.saleInvoiceForm.get('vchType').value;
    let result = this.com.isStopEntry(type);

    if(!result){
      this.tostr.info("You are not allowed")
      return;
    }

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

      this.apiService.deleteData('Sale/DeleteInvoice', obj).subscribe({
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
      //this.com.hideLoader();
    }
  }

  async editInvoice(invNo: any, invDate: any) {

    let type = this.saleInvoiceForm.get('vchType').value;
    let result = this.com.isStopEntry(type);

    if(!result){
      this.tostr.info("You are not allowed")
      return;
    }
    
    let dp = invDate.split('/');

    try {
      this.com.showLoader();

      const obj = {
        vchNo: invNo,
        vchType: this.saleInvoiceForm.get('vchType').value,
        invoiceDate: this.dp.transform(
          new Date(dp[2], dp[1] - 1, dp[0]),
          'yyyy/MM/dd'
        ),
      };

      const data = await this.apiService
        .getDataById('Sale/EditInvoice', obj)
        .toPromise();

      this.isPrint = true;
      this.isDisabled = false;
      this.isNewClick = true;
      this.formStatus = 'Edit';
      this.appendedData = [];
      data.forEach((item: any) => {
        this.saleInvoiceForm.get('doNo')?.patchValue(item.DONO);
        this.saleInvoiceForm.get('invNo')?.patchValue(item.VCHNO);

        this.saleInvoiceForm
          .get('vchDate')
          ?.patchValue(
            new Date(
              item.VCHDATE.split('/')[2],
              item.VCHDATE.split('/')[1] - 1,
              item.VCHDATE.split('/')[0]
            )
          );

        this.saleInvoiceForm.get('locId')?.patchValue(item.LOCID);
        this.saleInvoiceForm.get('orderTaker')?.patchValue(item.ORDERTAKERID);
        this.saleInvoiceForm.get('deliveryBoy')?.patchValue(item.DELIVERIBOY);
        this.saleInvoiceForm
          .get('party')
          ?.patchValue(item.PARTYCODE);
        this.saleInvoiceForm.get('walkingName')?.patchValue(item.CUSTOMERNAME);
        this.saleInvoiceForm
          .get('walkingContact')
          ?.patchValue(item.CUSTOMERCONTACT);
        this.saleInvoiceForm.get('discount')?.patchValue(item.NDISCOUNT);
        this.saleInvoiceForm.get('discountAmt')?.patchValue(item.NDISCOUNTAMT);
        this.saleInvoiceForm.get('otherCredit')?.patchValue(item.OTHERCREDIT);
        this.saleInvoiceForm.get('remarks')?.patchValue(item.REMARKS);
        this.saleInvoiceForm.get('shipment')?.patchValue(item.SHIPMENT);
        this.saleInvoiceForm.get('fTax')?.patchValue(item.FURTHERTAX);
        this.saleInvoiceForm.get('fTaxAmt')?.patchValue(item.FURTHERTAXAMT);
        this.saleInvoiceForm.get('whTax')?.patchValue(item.WHT);
        this.saleInvoiceForm.get('whTaxAmt')?.patchValue(item.WHTAMT);
        this.saleInvoiceForm.get('recAmount')?.patchValue(item.RECAMOUNT);
        this.saleInvoiceForm
          .get('remarksAmountPaid')
          ?.patchValue(item.PAYMENTREMARKS);
        this.saleInvoiceForm.get('retAmount')?.patchValue(item.RETURNAMT);
        this.saleInvoiceForm.get('termsDay')?.patchValue(item.TERMS);
        this.saleInvoiceForm.get('saleRemarks')?.patchValue(item.SALEREMARKS);

        this.appendedData.push(item);

        if(this.formTag == "GetCostCodes"){
          this.jobNo = item.T9JOBNO;
        }
        else{
          this.jobNo = item.JOBNO;
        }
        $('.setvaluecash').find('a').removeClass('activePaymnet');
        if (item.PAYMENTTYPE == 'CP' || item.PAYMENTTYPE == 'CR') {
          $('.setvaluecash').find('.cash').addClass('activePaymnet');
        } else if (item.PAYMENTTYPE == 'BP' || item.PAYMENTTYPE == 'BR') {
          $('.setvaluecash').find('.debit').addClass('activePaymnet');
        } else {
          $('.setvaluecash').find('.cash').addClass('activePaymnet');
        }

        this.calculation();
        this.com.hideLoader();
      });
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }

    $('.autoClose').click();
  }

  onClickPayment(e: any, status: any) {
    const element = e.target.closest('.setvaluecash');
    const a = element.querySelectorAll('a, img');
    a.forEach((x) => {
      x.classList.remove('activePaymnet');
    });

    if (e.target.localName == 'a') {
      e.target.classList = 'activePaymnet';
    } else if (e.target.localName == 'img') {
      e.target.parentElement.classList = 'activePaymnet';
    }

    this.paymentMethod = status;
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

  //======================= DELIVERY PERSON =====================//

  getDeliveryPer() {
    this.apiService.getData('Sale/GetDeliveryPerson').subscribe((data) => {
      this.deliveryPerList = data;
    });
  }

  createUpdateDeliveryPer() {
    try {
      this.com.showLoader();
      const obj = {
        id: this.delPerId,
        name: this.delPer,
        dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      };

      if (this.delPer == '' || this.delPer == null) {
        this.tostr.warning('Enter Deliver Person....!');
        return;
      }

      this.apiService
        .saveObj('Sale/AddUpdateDeliveryPerson', obj)
        .subscribe((result) => {
          if (result == true || result == 'true') {
            this.getDeliveryPer();
            this.refreshDelPer();
            this.tostr.success('Save Successfully');
            this.com.hideLoader();
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

  editDelPer(id: any, name: any): void {
    this.delPer = name;
    this.delPerId = id;
    this.isDisabledDelPer = false;
    this.isShowDelPer = true;
  }

  deleteDelPer(id: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {
      try {
        this.com.showLoader();

        const obj = {
          id: id,
          dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
        };

        this.apiService.deleteData('Sale/DeleteDeliveryPerson', obj).subscribe({
          next: (data) => {
            if (data == 'true' || data == true) {
              this.getDeliveryPer();
              this.refreshDelPer();
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

  refreshDelPer() {
    this.delPer = '';
    this.delPerId = 0;
    this.isDisabledDelPer = true;
    this.isShowDelPer = false;
  }

  newDelPer() {
    this.refreshDelPer();
    this.isDisabledDelPer = false;
    this.isShowDelPer = true;
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
