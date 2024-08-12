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
  selector: 'app-delivery-order',
  templateUrl: './delivery-order.component.html',
  styleUrls: ['./delivery-order.component.css']
})

export class DeliveryOrderComponent {
  @ViewChild('InvoiceList', { static: false }) InvoiceList!: ElementRef;
  
  basePath = environment.basePath;
  isShowPage: boolean = true;
  saleInvoiceForm!: FormGroup;
  costCenter = localStorage.getItem('costCenter');
  JobList: any[] = [];
  jobNo: any = null;
  formTag: any = '';
  isCategory: boolean = false;
  rowIndex: any;

  // PRODUCT
  locationList: any[] = [];
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
  selectedStatus: string = 'unsent';

  // TERMS
  termsDays = '';
  termsId = 0;
  termsList: any[] = [];
  isDisabledTerms: boolean = true;
  isShowTerms: boolean = false;

  // SUB PARTY
  subParty = '';
  subPartyId = 0;
  subPartyList: any[] = [];
  isDisabledSubParty: boolean = true;
  isShowSubParty: boolean = false;

  // Qty MODAL
  balance: any;
  onQty: number;
  location: any;
  sParty: any = null;
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

  isPartySelected: boolean = false;

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

  Dis1: string = "";
  Dis2: string = "";
  Dis3: string = "";
  Dis4: string = "";
  Dis5: string = "";
  Dis6: string = "";
  RateDiff: any = 0;
  TotalDiscount: number = 0;

  DES1CODE: any = 0;
  DES2CODE: any = 0;
  DES3CODE: any = 0;
  DES4CODE: any = 0;
  DES5CODE: any = 0;
  DES6CODE: any = 0;

  PRODUCTSALECODE: any;
  PRODUCTSTOCKCODE: any;



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
    this.getInvoices();
    this.getCustomer();
    this.getDiscodes();
    this.getLocation();
    this.getCategory();
    this.getTerms();
    
  }

  togglePages() {
    this.isShowPage = !this.isShowPage;
    this.onClickNew();
    if (this.isShowPage) {
      this.onClickRefresh();
    }
  }

  formInit() {
    this.saleInvoiceForm = this.fb.group({
      doNo: [0],
      invNo: [0],
      vchType: ['SP'],
      vchDate: [new Date()],
      locId: [''],
      party: [null],
      walkingName: [0],
      walkingContact: [0],
      grossAmount: [0],
      discount1: [0],
      discountAmt1: [0],
      discount2: [0],
      discountAmt2: [0],
      discount4: [0],
      discount3: [0],
      discountAmt3: [0],
      discountAmt4: [0],
      discount5: [0],
      discountAmt5: [0],
      discount6: [0],
      discountAmt6: [0],
      otherCredit: [0],
      remarks: [''],
      shipment: [0],
      fTax: [0],
      fTaxAmt: [0],
      whTax: [0],
      whTaxAmt: [0],
      totalDiscount: [0],
      recAmount: [0],
      remarksAmountPaid: [''],
      retAmount: [0],
      termsDay: ['30'],
      saleRemarks: [''],
      Formula1:[''],
      Formula2:[''],
      Formula3:[''],
      Formula4:[''],
      Formula5:[''],
      Formula6:['']
    });

    this.saleInvoiceForm.get('doNo').disable();
    this.saleInvoiceForm.get('invNo').disable();
    this.saleInvoiceForm.get('locId').disable();
    this.saleInvoiceForm.get('grossAmount').disable();
    //this.saleInvoiceForm.get('fTax').disable();
    this.saleInvoiceForm.get('fTaxAmt').disable();
    //this.saleInvoiceForm.get('whTax').disable();
    this.saleInvoiceForm.get('whTaxAmt').disable();
    this.saleInvoiceForm.get('totalDiscount').disable();
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
    // this.saleInvoiceForm.get('discount').setValue(0);
    // this.saleInvoiceForm.get('discountAmt').setValue(0);
    this.saleInvoiceForm.get('otherCredit').setValue(0);
    this.saleInvoiceForm.get('shipment').setValue(0);
    this.saleInvoiceForm.get('fTax').setValue(0);
    this.saleInvoiceForm.get('fTaxAmt').setValue(0);
    this.saleInvoiceForm.get('whTax').setValue(0);
    this.saleInvoiceForm.get('whTaxAmt').setValue(0);
    this.saleInvoiceForm.get('totalDiscount').setValue(0);
    this.saleInvoiceForm.get('recAmount').setValue(0);
    this.saleInvoiceForm.get('retAmount').setValue(0);
    this.saleInvoiceForm.get('termsDay').setValue(30);
    this.saleInvoiceForm.get('party').enable();
    this.sParty = undefined
  }

  async onClickNew() {
    let type = this.saleInvoiceForm.get('vchType').value;
    let result = this.com.isStopEntry(type);
    
    if(!result){
      this.tostr.info("You are not allowed")
      return;
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

  async getLocation() {
    const result = await this.apiService
      .getData('Inventory/GetLocation')
      .toPromise();
    this.locationList = result;
  }

  getInvoices() {
    const obj = {
      fromDate: this.dp.transform(this.fromDate, 'yyyy/MM/dd'),
      toDate: this.dp.transform(this.toDate, 'yyyy/MM/dd'),
      vchType: this.saleInvoiceForm.get('vchType').value,
    };
    this.com.showLoader();
    this.apiService
      .getDataById('Sale/GetInvoiceList', obj)
      .subscribe((data) => {
        if (this.selectedStatus === 'unsent') {
          this.invoiceList = data.filter(item => !item.Sent);
          this.com.hideLoader();
        } else if (this.selectedStatus === 'sent') {
          this.invoiceList = data.filter(item => item.Sent);
          this.com.hideLoader();
        } else {
          this.invoiceList = data;
          this.com.hideLoader();
        }
      });
  }

  onStatusChange(event: any) {
    this.selectedStatus = event.target.value;
    this.getInvoices();
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

  
  async getPartyTc() {
   let Party = this.saleInvoiceForm.get('party').value;
   const obj = {
    Party: Party.toString()
  };
  await   this.apiService.getDataById('Sale/GetPartyTC', obj).subscribe((result) => {
    this.saleInvoiceForm.get('discount1').setValue(result[0].Disc1);
    this.saleInvoiceForm.get('discount2').setValue(result[0].Disc2);
    this.saleInvoiceForm.get('discount3').setValue(result[0].Disc3);
    this.saleInvoiceForm.get('discount4').setValue(result[0].Disc4);
    this.saleInvoiceForm.get('discount5').setValue(result[0].Disc5);
    this.saleInvoiceForm.get('discount6').setValue(result[0].Disc6);
    this.saleInvoiceForm.get('Formula1').setValue(result[0].Formula1);
    this.saleInvoiceForm.get('Formula2').setValue(result[0].Formula2);
    this.saleInvoiceForm.get('Formula3').setValue(result[0].Formula3);
    this.saleInvoiceForm.get('Formula4').setValue(result[0].Formula4);
    this.saleInvoiceForm.get('Formula5').setValue(result[0].Formula5);
    this.saleInvoiceForm.get('Formula6').setValue(result[0].Formula6);
  });
   
  }

  onChangeCategory(event: any) {
   
    if (event == undefined) {
      this.productList = [];
      return;
    }
    if(this.saleInvoiceForm.get('party').value == undefined){
      this.tostr.info("Select Customer");
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
      party: this.saleInvoiceForm.get('party').value
    };

    this.apiService
      .getDataById('Sale/GetProductList', obj)
      .subscribe((data) => {
        this.productList = data;
        console.log(data);
      });
  }

  getOrderTakerList() {
    this.apiService.getData('Sale/GetOrderTakerList').subscribe((data) => {
      this.orderTakerList = data;
    });
  }

  getCustomer() {
    this.apiService.getDataById(`Sale/GetCustomer`, {status: true}).subscribe((data) => {
      this.customerList = data.party ?? data;
    });
  }

  getDiscodes() {
    this.apiService.getData(`Sale/GetDisCodes`).subscribe((data) => {
      this.Dis1 = data[0].DES1;
      this.Dis2 = data[0].DES2;
      this.Dis3 = data[0].DES3;
      this.Dis4 = data[0].DES4;
      this.Dis5 = data[0].DES5;
      this.Dis6 = data[0].DES6;
      this.DES1CODE = data[0].CODE1
      this.DES2CODE = data[0].CODE2
      this.DES3CODE = data[0].CODE3
      this.DES4CODE = data[0].CODE4
      this.DES5CODE = data[0].CODE5
      this.DES6CODE = data[0].CODE6

    });
  }

  onClickPlusQty(i: any) {

    console.log(i)

    const index = this.appendedData.findIndex(
      (x) =>
        x.PRODUCTSALECODE == i.PRODUCTSALECODE &&
        x.UOMID == i.UOMID &&
        x.EXPIRYDATE == i.EXPIRYDATE &&
        x.SID == i.SID
    );

    this.productRow = { ...i };
    this.onQty = 1;
    let dp = i.EXPIRYDATE.split('/');
    this.location = i.SID;
    this.expiryDate = new Date(dp[2], dp[1] - 1, dp[0]);
    this.saleRate = i.MAXRATE;
    this.balance = i.STOCK;
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
    }, 100);
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

  onInputModalCla(e: any) {
    let row = this.productRow;

    if(e.target.value > row.STOCK){
      e.target.value = row.STOCK
    }
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

 
  appendData() {

    if(this.sParty == undefined){
      this.tostr.info("Select Sub Party");
      return;
    }

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

    const index = this.appendedData.findIndex(
      (x) =>
        x.PRODUCTSALECODE == row.PRODUCTSALECODE &&
        x.UOMID == row.UOMID &&
        x.EXPIRYDATE == row.EXPIRYDATE &&
        x.SID == row.SID &&
        x.BATCHNO == row.BATCHNO &&
        x.NETQTY == row.NETQTY
    );

    row.QTY = this.onQty;
    row.RATE = this.saleRate;
    row.DIFF = 0;
    row.NETRATE = 0;
    row.EXPIRYDATE = this.dp.transform(this.expiryDate, 'dd/MM/yyyy');
    row.SID = this.location;
    row.BATCHNO = this.batchNo;
    row.SUBPARTY = this.sParty;
    row.PRODUCTSALECODE;
    row.PRODUCTSTOCKCODE;
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

    console.log(this.appendedData);
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
    const discount1 = this.saleInvoiceForm.get('discount1').value;
    const discount2 = this.saleInvoiceForm.get('discount2').value;
    const discount3 = this.saleInvoiceForm.get('discount3').value;
    const discount4 = this.saleInvoiceForm.get('discount4').value;
    const discount5 = this.saleInvoiceForm.get('discount5').value;
    const discount6 = this.saleInvoiceForm.get('discount6').value;
    const grossAmount = this.saleInvoiceForm.get('grossAmount').value;
    const Formula1 = this.saleInvoiceForm.get('Formula1').value;
    const Formula2 = this.saleInvoiceForm.get('Formula2').value;
    const Formula3 = this.saleInvoiceForm.get('Formula3').value;
    const Formula4 = this.saleInvoiceForm.get('Formula4').value;
    const Formula5 = this.saleInvoiceForm.get('Formula5').value;
    const Formula6 = this.saleInvoiceForm.get('Formula6').value;
    var disAmt1 = 0;
    var disAmt2 = 0;
    var disAmt3 = 0;
    var disAmt4 = 0;
    var disAmt5 = 0;
    var disAmt6 = 0;

    // if (discount < 0 || discount == '' || discount == null) {
    //   this.saleInvoiceForm.get('discount').patchValue(0);
    // }

    // if (discount > 100) {
    //   this.saleInvoiceForm.get('discount').patchValue(100);
    // }
    if(Formula1 == 'A'){
       disAmt1 = this.com.roundVal(
        ((parseFloat(grossAmount) * parseFloat(discount1)) / 100).toFixed(2)
      );
      this.saleInvoiceForm.get('discountAmt1').patchValue(disAmt1);
    } else{
      disAmt1 = this.com.roundVal(
        ((this.totalQty * parseFloat(discount1))).toFixed(2)
      );
      this.saleInvoiceForm.get('discountAmt1').patchValue(disAmt1);
    }


    if(Formula2 == 'A'){
      disAmt2 = this.com.roundVal(
       ((parseFloat(grossAmount) * parseFloat(discount2)) / 100).toFixed(2)
     );
     this.saleInvoiceForm.get('discountAmt2').patchValue(disAmt2);
   } else{
     disAmt2 = this.com.roundVal(
       ((this.totalQty * parseFloat(discount2))).toFixed(2)
     );
     this.saleInvoiceForm.get('discountAmt2').patchValue(disAmt2);
   }
   
    if(Formula3 == 'A'){
     disAmt3 = this.com.roundVal(
      ((parseFloat(grossAmount) * parseFloat(discount3)) / 100).toFixed(2)
    );
    this.saleInvoiceForm.get('discountAmt3').patchValue(disAmt3);
  } else{
    disAmt3 = this.com.roundVal(
      ((this.totalQty * parseFloat(discount3))).toFixed(2)
    );
    this.saleInvoiceForm.get('discountAmt3').patchValue(disAmt3);
  }


  if(Formula4 == 'A'){
    disAmt4 = this.com.roundVal(
     ((parseFloat(grossAmount) * parseFloat(discount4)) / 100).toFixed(2)
   );
   this.saleInvoiceForm.get('discountAmt4').patchValue(disAmt4);
 } else{
   disAmt4 = this.com.roundVal(
     ((this.totalQty * parseFloat(discount4))).toFixed(2)
   );
   this.saleInvoiceForm.get('discountAmt4').patchValue(disAmt4);
 }

 if(Formula5 == 'A'){
  disAmt5 = this.com.roundVal(
   ((parseFloat(grossAmount) * parseFloat(discount5)) / 100).toFixed(2)
 );
 this.saleInvoiceForm.get('discountAmt5').patchValue(disAmt5);
} else{
 disAmt5 = this.com.roundVal(
   ((this.totalQty * parseFloat(discount5))).toFixed(2)
 );
 this.saleInvoiceForm.get('discountAmt5').patchValue(disAmt5);
}


if(Formula6 == 'A'){
  disAmt6 = this.com.roundVal(
   ((parseFloat(grossAmount) * parseFloat(discount6)) / 100).toFixed(2)
 );
 this.saleInvoiceForm.get('discountAmt6').patchValue(disAmt6);
} else{
 disAmt6 = this.com.roundVal(
   ((this.totalQty * parseFloat(discount6))).toFixed(2)
 );
 this.saleInvoiceForm.get('discountAmt6').patchValue(disAmt6);
}

    if(this.isDiscount){
      this.calculation();
    }
  }

  onInputQty(event: any, i: any, index: any) {
    if (i.QTY < 0 || i.QTY == null) {
      event.target.value = 0;
      i.QTY = 0;
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

      allTotalQty += x.QTY;

      x.DIFF = this.RateDiff;
      x.NETRATE = x.RATE + x.DIFF

      x.VALUE = this.com.roundVal(
        this.getNum(parseFloat(x.QTY) * parseFloat(x.NETRATE)).toFixed(2)
      );
      productValue += parseFloat(x.VALUE);

 

      if (parseFloat(x.SALETAX) > 100) {
        x.SALETAX = 100;
      }
      x.SALETAXAMT = this.com.roundVal(
        this.getNum(
          (parseFloat(x.SALETAX) *
            (this.getNum(x.VALUE))) /
            100
        ).toFixed(2)
      );
      x.NETVALUE = this.com.roundVal(
        (
          parseFloat(this.getNum(x.VALUE))  +
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
    //const shipment = this.saleInvoiceForm.get('shipment').value;
    const discountAmt1 = this.saleInvoiceForm.get('discountAmt1').value;
    const discountAmt2 = this.saleInvoiceForm.get('discountAmt2').value;
    const discountAmt3 = this.saleInvoiceForm.get('discountAmt3').value;
    const discountAmt4 = this.saleInvoiceForm.get('discountAmt4').value;
    const discountAmt5 = this.saleInvoiceForm.get('discountAmt5').value;
    const discountAmt6 = this.saleInvoiceForm.get('discountAmt6').value;
    const fTax = this.saleInvoiceForm.get('fTax').value;
    const whTax = this.saleInvoiceForm.get('whTax').value;
    const totalDiscount = this.saleInvoiceForm.get('totalDiscount').value;

    let TotalDiscount = this.com.roundVal(
      ((parseFloat(discountAmt1) + parseFloat(discountAmt2) + parseFloat(discountAmt3)
       + parseFloat(discountAmt4) + parseFloat(discountAmt5) + parseFloat(discountAmt6))).toFixed(2)
       );

       let due = this.com.roundVal(
        ((parseFloat(fTax) + parseFloat(whTax))).toFixed(2)
       );


       this.saleInvoiceForm.get('totalDiscount').setValue(TotalDiscount);
       this.netDue =  grossAmount - TotalDiscount;

    // let due = this.com.roundVal(
    //   ((grossAmount) - (parseFloat(discountAmt1) + parseFloat(otherCredit))).toFixed(2)
    // );

    // let fTaxAmt = this.com.roundVal(
    //   ((parseFloat(fTax) * productValue) / 100).toFixed(2)
    // );
    // this.saleInvoiceForm.get('fTaxAmt').setValue(fTaxAmt);

    // let whTaxAmt =
    //   (parseFloat(whTax) * (parseFloat(due) + parseFloat(fTaxAmt))) / 100;
    // this.saleInvoiceForm
    //   .get('whTaxAmt')
    //   .setValue(this.com.roundVal(whTaxAmt.toFixed(2)));

    // let totalDue = this.com.roundVal(
    //   (parseFloat(due) + parseFloat(fTaxAmt) + whTaxAmt).toFixed(2)
    // );
    // this.saleInvoiceForm
    //   .get('totalDue')
    //   .setValue(this.com.roundVal(parseFloat(totalDue).toFixed(2)));

    // let recAmount = this.saleInvoiceForm.get('recAmount').value;

    // if (parseFloat(recAmount) > totalDue) {
    //   this.saleInvoiceForm
    //     .get('retAmount')
    //     .setValue(
    //       this.com.roundVal((parseFloat(recAmount) - totalDue).toFixed(2))
    //     );
    //   this.netDue = 0;
    // } else {
    //   this.saleInvoiceForm.get('retAmount').setValue(0);
    //   this.netDue = this.com.roundVal(
    //     this.getNum(totalDue - parseFloat(recAmount)).toFixed(2)
    //   );
    // }
  }

  onChangeParty() {
    this.isPartySelected = true;
    const cus = this.customerList.find(
      (x) => x.code == this.saleInvoiceForm.get('party').value
    );
    console.log(cus);

    if (cus) {
      this.RateDiff = cus.ratediff;
    } else {
      this.RateDiff = 0;

    }
     this.getPartyTc()

    this.onInputDis();

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
    body.totalDiscount = this.saleInvoiceForm.get('totalDiscount').value;
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

    // if (body.deliveryBoy == null || body.deliveryBoy == undefined) {
    //   this.tostr.warning('Select Delivery Person...!');
    //   return;
    // }

    // if (body.orderTaker == null || body.orderTaker == undefined) {
    //   this.tostr.warning('Select Order Taker...!');
    //   return;
    // }

    if (body.party == null || body.party == undefined) {
      this.tostr.warning('Select Party....!');
      return;
    }

    if (body.grossAmount == null || body.grossAmount == undefined) {
      this.tostr.warning('Make Gross Amount greater than 0...!');
      return;
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
        //DeliveryBoy: body.deliveryBoy,
       // OrderTaker: body.orderTaker,
        PartyCode: body.party,
        PartyName: this.customerList.find((x) => x.code == body.party).name,
        //WalkingName: body.walkingName,
       // WalkingContact: body.walkingContact,
        ProductName: data.PRODUCT,
        SaleQty: data.QTY,
        //RetQty: data.RETQTY,
        //NetQty: data.NETQTY,
        Rate: data.RATE,
        RateDiff: this.RateDiff,
        //OldRate: data.OLDRATE,
        //ProductDis: data.DISCOUNT,
        //ProductDisAmt: data.DISAMOUNT,
        SaleTax: data.SALETAX,
        SaleTaxAmt: data.SALETAXAMT,
        NetValue: data.NETVALUE,
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
        Discount1: body.discount1,
        DiscountAmt1: body.discountAmt1,
        Discount2: body.discount2,
        DiscountAmt2: body.discountAmt2,
        Discount3: body.discount3,
        DiscountAmt3: body.discountAmt3,
        Discount4: body.discount4,
        DiscountAmt4: body.discountAmt4,
        Discount5: body.discount5,
        DiscountAmt5: body.discountAmt5,
        Discount6: body.discount6,
        DiscountAmt6: body.discountAmt6,
        //OtherCredit: body.otherCredit,
        Remarks: body.remarks,
       // Shipment: body.shipment,
        //RecAmount: body.recAmount,
        NetDue: this.netDue,
        VehicleNo: data.BATCHNO,
      //  ReturnAmt: body.retAmount,
       // TaxP: taxP,
        TermsDays: body.termsDay.toString(),
        FTax: body.fTax,
        FTaxAmt: body.fTaxAmt,
        WHT: body.whTax,
        WHTAmt: body.whTaxAmt,
        Des: 'Sale ' + (body.vchType == 'SP' ? 'Invoice' : 'Return'),
        Status: this.formStatus,
        //PaymentMethod: this.paymentMethod,
        //PaymentMethodRmk: body.remarksAmountPaid,
        SaleRemarks: body.saleRemarks,
        //JobNo: (this.formTag == "GetCustomer") ? (data.JOBNO ?? 0) : (this.jobNo1 ?? 0),
        JobNo: this.jobNo ?? 0,
        NoStock: data.NOSTOCK,
       // Tag: this.formTag,
        dtNow: new Date(),
        GrossAmount: this.saleInvoiceForm.get('grossAmount').value,
        TotalDiscount: this.saleInvoiceForm.get('totalDiscount').value,
        NetRate: data.NETRATE,
        Value: data.VALUE,
        SubPartyId: data.SUBPARTY,
        ProductSaleCode: data.PRODUCTSALECODE,
        ProductStockCode: data.PRODUCTSTOCKCODE,
        DES1CODE: this.DES1CODE || 0,
        DES2CODE: this.DES2CODE || 0,
        DES3CODE: this.DES3CODE || 0,
        DES4CODE: this.DES4CODE || 0,
        DES5CODE: this.DES5CODE || 0,
        DES6CODE: this.DES6CODE || 0,
        totalQty: this.totalQty,
      }));

      console.log(invoice);

      this.apiService
        .saveData('Sale/SaveUpdateDeliveryOrder', invoice)
        .subscribe((r) => {
          if (r.status == true || r.status == 'true') {
            this.com.hideLoader();
            this.tostr.success('Save Successfully');
            this.saleInvoiceForm.get('invNo').patchValue(r.vchNo);
            this.getInvoices();
            this.formStatus = 'Edit';
            this.isPrint = true;
            
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

  deleteInvoice(invNo: any, Approve:any): void {

    if(Approve == true){
      this.tostr.info("Voucher Approved!");
      return;
    }

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
            this.com.hideLoader();
            this.getInvoices();
            this.onClickRefresh();
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

  async editInvoice(invNo: any, invDate: any, Approve:any) {

    if(Approve == true){
      this.tostr.info("Voucher Approved!");
      return;
    }

    let type = this.saleInvoiceForm.get('vchType').value;
    let result = this.com.isStopEntry(type);

    if(!result){
      this.tostr.info("You are not allowed")
      return;
    }

    this.togglePages();
    
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
      this.sParty = data[0].SubPartyId;
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
       

        this.saleInvoiceForm.get('discount1').setValue(data[0].Disc1);
        this.saleInvoiceForm.get('discount2').setValue(data[0].Disc2);
        this.saleInvoiceForm.get('discount3').setValue(data[0].Disc3);
        this.saleInvoiceForm.get('discount4').setValue(data[0].Disc4);
        this.saleInvoiceForm.get('discount5').setValue(data[0].Disc5);
        this.saleInvoiceForm.get('discount6').setValue(data[0].Disc6);
        

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


  //======================= DELIVERY PERSON =====================//

  getSubParty() {
    this.apiService.getDataById('Sale/GetSubPartyByCode', {code: this.saleInvoiceForm.get('party').value}).subscribe((data) => {
      this.subPartyList = data;
    });
  }

  createUpdateSubParty() {

    try {

      let party = this.saleInvoiceForm.get('party').value;
      if(party == null){
        this.tostr.warning("Select Party...!")
        return;
      }

      this.com.showLoader();
      const obj = {
        id: this.subPartyId,
        code: this.saleInvoiceForm.get('party').value,
        name: this.subParty,
        dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      };

      if (this.subParty == '' || this.subParty == null) {
        this.tostr.warning('Enter Sub Party....!');
        return;
      }

      this.apiService
        .saveObj('Sale/SaveUpdateSubParty', obj)
        .subscribe((result) => {
          if (result == true || result == 'true') {
            this.getSubParty();
            this.refreshSubParty();
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

  editSubParty(id: any, name: any): void {
    this.subParty = name;
    this.subPartyId = id;
    this.isDisabledSubParty = false;
    this.isShowSubParty = true;
  }

  deleteSubParty(id: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {
      try {
        this.com.showLoader();

        const obj = {
          id: id,
          dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
        };

        this.apiService.deleteData('Sale/DeleteSubParty', obj).subscribe({
          next: (data) => {
            if (data == 'true' || data == true) {
              this.getSubParty();
              this.refreshSubParty();
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

  refreshSubParty() {
    this.subParty = '';
    this.subPartyId= 0;
    this.isDisabledSubParty = true;
    this.isShowSubParty = false;
  }

  newSubParty() {
    this.refreshSubParty();
    this.isDisabledSubParty = false;
    this.isShowSubParty = true;
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

  onClickPlusSubParty(){
    let party = this.saleInvoiceForm.get('party').value;
    if(party == null){
      this.tostr.warning("Select Party...!")
      return;
    }

    $('#SubPartyModal').modal('show');
  }

  export(fileName: any, formate: any) {
    this.com.ExportFiles(fileName, formate);
  }


  SentVoucher(event: any, INVOIVENO: any, Approve: any) {
    if(Approve == false){
      this.tostr.info("Approve Voucher To Send");
      return;
    }
    try {
        const isChecked = event.target.checked;
        const obj = {
            isChecked: isChecked,
            VchNo: INVOIVENO
        };
        console.log(obj);
        this.com.showLoader();

        this.apiService.saveData('Sale/SentDoStatus', obj)
            .subscribe((result) => {
                this.com.hideLoader();
                if (result === true || result === 'true') {
                    this.tostr.success('Save Successfully');
                    this.getInvoices();
                } else {
                    this.tostr.error('Please Save Again');
                }
            }, (error) => {
                this.com.hideLoader();
                console.error(error);
                this.tostr.error('Please try again.');
            });
    } catch (err) {
        this.com.hideLoader();
        console.error(err);
        this.tostr.error('An unexpected error occurred. Please try again.');
    }
}

    
}