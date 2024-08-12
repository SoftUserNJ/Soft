import { Component, ViewChild, ElementRef } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { environment } from '../../../../environment/environmemt';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';
declare const $: any;

@Component({
  selector: 'app-purchase-invoice',
  templateUrl: './purchase-invoice.component.html',
  styleUrls: ['./purchase-invoice.component.css'],
})
export class PurchaseInvoiceComponent {
  @ViewChild('InvoiceList', { static: false }) InvoiceList!: ElementRef;
  @ViewChild('piProductLists', { static: false }) piProductLists!: ElementRef;

  basePath = environment.basePath;
  costCenter = localStorage.getItem('costCenter');
  

  // PRODUCT
  productName: string = '';
  barCode: any = '';
  categoryList: any[] = [];
  uomList: any[] = [];
  category: any;
  productList: any[] = [];
  locationList: any[] = [];
  productRow: any;

  // Qty Modal
  onQty: number;
  location: any;
  expiryDate: Date;
  costRate: number;
  batchNo: any;
  amount: any;
  maxSaleRate: any;
  minSaleRate: any;

  // DIVISION
  divisionList: any[] = [];
  filterDivision: any[] = [];
  JobList: any[] = [];
  JobList1: any[] = [];
  jobNo: any = null;
  productCode: any;
  costCateogoryCode: any;
  showQty: any;
  dQty: any;
  dIsCommission: any = false;
  dCostRate: any
  dExpDate: any
  dSID: any

  dTotalQty: any = 0;
  dTotalValue: any = 0;
  dTotalCommission: any = 0;
  dTotalNetValue: any = 0;

  // Add Product
  appendedData: any[] = [];
  totalQty: any;

  // BILL
  netDue: any;
  isTaxDisable: boolean = false;

  //Product Modal
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
  mTax: any;
  mDiscount: any;
  mRate: any;
  mStatus: any;
  mImage: any;

  // OTHER
  formStatus: string = '';
  purchaseInvoiceForm!: FormGroup;
  supplierList: any[] = [];
  isNewClick: boolean = false;
  PR: boolean = false;
  paymentMethod = 'Cash';

  // Terms
  termsList: any[] = [];
  isDisabledTerms: boolean = true;
  isShowTerms: boolean = false;
  termsDays = '';
  termsId = 0;

  // Invoice List
  invoiceList: any[] = [];
  fromDate: Date;
  toDate: Date;
  isPrint: boolean = false;

  //FILE UPLOAD
  file: File | null = null;

  constructor(
    private apiService: ApiService,
    private fb: FormBuilder,
    private tostr: ToastrService,
    private dp: DatePipe,
    private auth: AuthService,
    private com: CommonService
  ) {
    const today = new Date();
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  }

  onClickPrint(tag: any) {
    const vchNo = this.purchaseInvoiceForm.get('invNo').value;
    const vchType = this.purchaseInvoiceForm.get('vchType').value;
    const vchDate = this.dp.transform(
      this.purchaseInvoiceForm.get('vchDate').value,
      'yyyy/MM/dd'
    );
    let url = '';

    if (tag == 'invoice') {
      if (vchType == 'PI') {
        url = `SultaniaPurchaseInvoice?VchNoFrom=${vchNo}&VchNoTo=${vchNo}&VchType=${vchType}&fromDate=${vchDate}&toDate=${vchDate}&CmpId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}`;
      }
      if (vchType == 'PR') {
        url = `PurchaseReturn?VchNoFrom=${vchNo}&VchNoTo=${vchNo}&VchType=${vchType}&fromDate=${vchDate}&toDate=${vchDate}&CmpId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}`;
      }
    } else if (tag == 'loading') {
      url = `PurchaseUnloading?VchNo=${vchNo}&VchType=${vchType}&CompId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}`;
    } else if (tag == 'vch') {
      url = `PrintVoucherRangeWise?DateFrom=${vchDate}&DateTo=${vchDate}&VchType=${vchType},SP-RAW&VchNoFrom=${vchNo}&VchNoTo=${vchNo}&CompId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}`;
    }
    this.com.viewReport(url);
  }

  onClickRowReport(invDate: any, invoiceNo: string, tag: string) {
    const vchType = this.purchaseInvoiceForm.get('vchType').value;
    const dPart = invDate.split('/');
    const vchDate = this.dp.transform(
      new Date(dPart[2], dPart[1] - 1, dPart[0]),
      'yyyy/MM/dd'
    );

    let url = '';
    if (tag === 'invoice') {
      if (vchType == 'PI') {
        url = `SultaniaPurchaseInvoice?VchNoFrom=${invoiceNo}&VchNoTo=${invoiceNo}&VchType=${vchType}&fromDate=${vchDate}&toDate=${vchDate}&CmpId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}`;
      }
      if (vchType == 'PR') {
        url = `PurchaseReturn?VchNoFrom=${invoiceNo}&VchNoTo=${invoiceNo}&VchType=${vchType}&fromDate=${vchDate}&toDate=${vchDate}&CmpId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}`;
      }
    } else if (tag === 'loading') {
      url = `PurchaseUnloading?VchNo=${invoiceNo}&VchType=${vchType}&CompId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}`;
    }
    this.com.viewReport(url);
  }

  async ngOnInit() {
    this.formInit();
    await this.getUoms();
    await this.getLocation();
    this.getCategory();
    this.getSupplier();
    this.getTerms();
    this.resetForm();
    this.JobList = await this.com.getJobList(true);
    this.JobList1 = await this.com.getJobList(false);
  }

  formInit() {
    this.purchaseInvoiceForm = this.fb.group({
      invNo: [0],
      vchDate: [new Date()],
      vchType: ['PI'],
      party: [0],
      shipmentExpense: [0],
      currencyConversion: [0],
      totalQty: [0],
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
      returnAmount: [0],
      termsDay: ['30'],
      dtNow: new Date(),
    });

    this.purchaseInvoiceForm.get('invNo').disable();
    this.purchaseInvoiceForm.get('grossAmount').disable();
    this.purchaseInvoiceForm.get('shipment').disable();
    //this.purchaseInvoiceForm.get('fTax').disable();
    this.purchaseInvoiceForm.get('fTaxAmt').disable();
    //this.purchaseInvoiceForm.get('whTax').disable();
    this.purchaseInvoiceForm.get('whTaxAmt').disable();
    this.purchaseInvoiceForm.get('totalDue').disable();
    this.purchaseInvoiceForm.get('returnAmount').disable();
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

  getInvoices() {
    const obj = {
      fromDate: this.dp.transform(this.fromDate, 'yyyy/MM/dd'),
      toDate: this.dp.transform(this.toDate, 'yyyy/MM/dd'),
      vchType: this.purchaseInvoiceForm.get('vchType').value,
    };

    this.apiService
      .getDataById('Purchase/GetPIInvoices', obj)
      .subscribe((data) => {
        this.invoiceList = data;
      });
  }

  isRound(value: any) {
    return this.com.roundVal(value);
  }

  onClickPI(event: any): void {
    if (this.isNewClick) {
      event.preventDefault();
      return;
    }

    this.purchaseInvoiceForm.get('vchType').setValue('PI');
    this.PR = false;
  }

  onClickPR(event: any): void {
    if (this.isNewClick) {
      event.preventDefault();
      return;
    }
    this.purchaseInvoiceForm.get('vchType').setValue('PR');
    this.PR = true;
  }

  async getMax() {
    const obj = {
      vchType: this.purchaseInvoiceForm.get('vchType').value,
    };

    const data = await this.apiService.getDataById('Purchase/GetPIMax', obj).toPromise();
    this.purchaseInvoiceForm.get('invNo')?.patchValue(data[0].VCHNO);
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
    
    if (event.target.value.length > 2) {
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
      vchDate: this.dp.transform(
        this.purchaseInvoiceForm.get('vchDate').value,
        'yyyy/MM/dd'
      ),
      vchType: this.purchaseInvoiceForm.get('vchType').value,
    };

    this.apiService
      .getDataById('Purchase/PIProductList', obj)
      .subscribe((data) => {
        data.forEach((x, i) => {
          x.UomList = this.uomList.filter(
            (z) => z.CODE.slice(-5) === x.CODE.slice(-5)
          );
          data[i] = x;
        });

        this.productList = data;
      });
  }

  getSupplier() {
    this.apiService.getDataById('Purchase/GetSupplier', {status: true}).subscribe((result) => {
      this.supplierList = result.party;
    });
  }

  onChangeUom(i: any, uoms: any[], index: any) {
    const uom = uoms.find((z) => z.UOMID == i.target.value);

    this.productList[index].RATE = uom.PURCHASERATE;
    this.productList[index].UOM = uom.UOM;
    this.productList[index].UOMID = uom.UOMID;
    this.productList[index].PACKING = uom.PACKING;
    this.productList[index].BASEPACKING = uom.BASPACKING;
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
    this.costRate = i.RATE;
    this.amount = this.com.roundVal((this.onQty * this.costRate).toFixed(2));
    this.maxSaleRate = i.MAXRATE;
    this.minSaleRate = i.MINRATE;

    const vchType = this.purchaseInvoiceForm.get('vchType').value;
    if(vchType == "PI"){
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

  onClickPlus(i: any) {
    this.productRow = { ...i };
    let dp = i.EXPIRYDATE.split('/');
    this.onQty = i.QTY;
    this.location = i.SID;
    this.batchNo = i.BATCHNO;
    this.expiryDate = new Date(dp[2], dp[1] - 1, dp[0]);
    this.costRate = i.RATE;
    this.amount = this.com.roundVal((this.onQty * this.costRate).toFixed(2));
    this.maxSaleRate = i.MAXRATE;
    this.minSaleRate = i.MINRATE;
    $('#QtyModal').modal('show');

    setTimeout(() => {
      $('.onQty').select();
    }, 200);
  }

  onClickDivisionModal(i: any) {

    this.filterDivision = this.divisionList.filter((x) => x.PRODUCTCODE == i.CODE);

    this.costCateogoryCode = i.CONCODE;
    this.productCode = i.CODE;
    this.showQty = i.QTY;
    this.dQty = i.QTY;
    this.dIsCommission = i.ISCOMMISSION;
    this.dCostRate = i.RATE;
    let dp = i.EXPIRYDATE.split('/');
    this.dExpDate = new Date(dp[2], dp[1] - 1, dp[0]);
    this.dSID = i.SID;
    this.jobNo = i.JOBNO;
    $('#DivisionModal').modal('show');
    setTimeout(() => {
      $('.onQty').select();
    }, 200);
    this.divisionCalculation();
  }

  appendInDiv() {
    if(this.jobNo == null){
      this.tostr.warning("Select Job No")
      return;
    }

    if(this.dQty == null || this.dQty == 0 || this.dQty == ''){
      this.tostr.warning("Enter Qty");
      return;
    }

    if(this.dCostRate == null || this.dCostRate == 0 || this.dCostRate == ''){
      this.tostr.warning("Enter Cost Rate");
      return;
    }

    const chkDivi = this.divisionList.filter(
      (x) => x.PRODUCTCODE == this.productCode && x.JOBNO == this.jobNo
    );

    if (chkDivi.length > 0) {
      this.tostr.warning('this job is already added');
      return;
    }

    const myQty = this.dTotalQty + this.dQty;
    if(myQty > this.showQty){
      return;
    }

    let obj: any = {};
    (obj.SNO = new Date().getTime()),
    (obj.PRODUCTCODE = this.productCode),
    (obj.COSTCATEGORYCODE = this.costCateogoryCode),
    (obj.JOBNO = this.jobNo),
    (obj.JOBNAME = this.JobList.find((x) => x.ID == this.jobNo).NAME),
    (obj.QTY = this.dQty),
    (obj.RATE = this.dCostRate),
    (obj.VALUE = 0),
    (obj.COMMISSION = (this.dIsCommission == true) ? this.JobList.find((x) => x.ID == this.jobNo).COMM : 0),
    (obj.COMVALUE = 0), 
    (obj.NETVALUE = 0), 
    (obj.EXPIRYDATE = this.dExpDate),
    (obj.SID = this.dSID),
    this.divisionList.push(obj);
    this.filterDivision = this.divisionList.filter((x) => x.PRODUCTCODE == this.productCode);

    this.divisionCalculation();
  }

  divRemoveRow(i: any) {
    const confirmDelete = confirm('Are you sure you want to delete this item?');
    if (!confirmDelete) {
      return;
    }

    const index1 = this.filterDivision.findIndex((x) => x.PRODUCTCODE == i.PRODUCTCODE && x.JOBNO == i.JOBNO);
    const index = this.divisionList.findIndex((x) => x.PRODUCTCODE == i.PRODUCTCODE && x.JOBNO == i.JOBNO);

    if (index !== -1) {
      this.divisionList.splice(index, 1);
    }

    if (index1 !== -1) {
      this.filterDivision.splice(index1, 1);
    }

    this.divisionCalculation();
  }

  divisionCalculation() {
    this.dTotalQty = 0;
    this.dTotalValue = 0;
    this.dTotalCommission = 0;
    this.dTotalNetValue = 0;

    this.filterDivision.filter((x) => x.PRODUCTCODE == this.productCode)
      .forEach((x: any, i) => {
        this.dTotalQty += x.QTY;
        x.VALUE = this.com.roundVal(parseFloat((x.QTY * x.RATE).toFixed(2)));
        this.dTotalValue += x.VALUE;

        const jbNo = this.JobList1.find((z) => z.ID == x.JOBNO);
        if(jbNo.COMTYPE == 'B'){
          x.COMVALUE = this.com.roundVal(parseFloat((x.QTY * x.COMMISSION).toFixed(2)));
        }
        else{
          x.COMVALUE = this.com.roundVal(parseFloat(((x.VALUE * x.COMMISSION) / 100).toFixed(2)));
        }

        this.dTotalCommission += x.COMVALUE;
        x.NETVALUE = this.com.roundVal(parseFloat((x.VALUE - x.COMVALUE).toFixed(2)));
        this.dTotalNetValue += x.NETVALUE;
      });
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

    this.amount = this.com.roundVal((this.onQty * this.costRate).toFixed(2));
  }

  appendData() {
    if (this.onQty < 0) {
      return;
    }

    if(this.location == null){
      this.tostr.warning("Select Locaion....!")
      return;
    }

    let row: any = this.productRow;
    const index = this.appendedData.findIndex(
      (x) =>
        x.CODE.slice(-5) == row.CODE.slice(-5) &&
        x.UOMID == row.UOMID &&
        x.EXPIRYDATE == row.EXPIRYDATE &&
        x.SID == row.SID
    );

    row.QTY = this.onQty;
    row.PURCHASERATE = this.costRate;
    row.PURCHASEVALUE = row.QTY * this.costRate;
    row.MAXRATE = this.maxSaleRate;
    row.MINRATE = this.minSaleRate;
    row.EXPIRYDATE = this.dp.transform(this.expiryDate, 'dd/MM/yyyy');
    row.SID = this.location;
    row.BATCHNO = this.batchNo;

    if (index !== -1) {
      row.QTY = this.onQty;
      row.RETQTY = this.appendedData[index].RETQTY;
      this.appendedData[index] = row;
      $('#QtyModal').modal('hide');
      this.calculation();
      this.onInputDis();
      return;
    }

    row.RETQTY = 0;
    row.sno = new Date().getTime();
    this.appendedData.push(row);
    $('#QtyModal').modal('hide');
    this.calculation();
    this.onInputDis();
  }

  onClickRemove(i: any) {
    const confirmDelete = confirm('Are you sure you want to delete this item?');
    if (!confirmDelete) {
      return;
    }

    const indexToRemove = this.appendedData.findIndex(
      (item) => item.sno === i.sno
    );
    if (indexToRemove !== -1) {
      this.appendedData.splice(indexToRemove, 1);
    }

    const filterRow = this.divisionList.filter((x) => x.PRODUCTCODE == i.CODE);
    filterRow.forEach((x, i) => {
      const index = this.divisionList.findIndex((item) => item.SNO === x.SNO);
      if (index !== -1) {
        this.divisionList.splice(index, 1);
      }
    });

    const filterDiv = this.filterDivision.filter((x) => x.PRODUCTCODE == i.CODE);
    filterDiv.forEach((x, i) => {
      const index = this.filterDivision.findIndex((item) => item.SNO === x.SNO);
      if (index !== -1) {
        this.filterDivision.splice(index, 1);
      }
    });

    this.calculation();
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

    const vchType = this.purchaseInvoiceForm.get('vchType').value;
    if (vchType == 'PR') {
      let stockArray = this.getStock(
        i.STOCKCODE,
        i.EXPIRYDATE,
        i.LOCATION,
        index
      );
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

    this.calculation();
  }

  getStock(code: any, expiry: any, location: any, index: any) {
    let stock = 0;
    let retStock = 0;

    this.appendedData.forEach((x, i) => {
      if (
        x.STOCKCODE == code &&
        x.EXPIRYDATE == expiry &&
        x.LOCATION == location
      ) {
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

  calculation() {
    let allTotalQty = 0;
    let productValue = 0;
    let upTotalQty = 0;
    let ship = 0;
    let grossAmount = 0;

    const shipExp = this.purchaseInvoiceForm.get('shipmentExpense').value;
    const totalQty = this.purchaseInvoiceForm.get('totalQty').value;
    const currencyConversion =
      this.purchaseInvoiceForm.get('currencyConversion').value;

    if (parseFloat(currencyConversion) != 0) {
      upTotalQty = this.getNum((shipExp / totalQty) * currencyConversion);
      ship = this.getNum(shipExp * currencyConversion);
      this.purchaseInvoiceForm
        .get('shipment')
        .setValue(this.com.roundVal(ship.toFixed(2)));
    } else {
      upTotalQty = this.getNum(shipExp / totalQty);
      ship = shipExp;
      this.purchaseInvoiceForm
        .get('shipment')
        .setValue(this.com.roundVal(ship.toFixed(2)));
    }

    this.appendedData.forEach((x, i) => {
      let rate = x.PURCHASERATE;
      let netQty = x.QTY - x.RETQTY;
      allTotalQty += netQty;
      x.NETQTY = netQty;
      let purchaseRae = upTotalQty * netQty;
      let purRate = netQty * rate + purchaseRae;
      let nPurchaseRate = purRate / netQty;
      x.PURCHASEVALUE = this.com.roundVal((netQty * rate).toFixed(2));

      if (nPurchaseRate != 0) {
        x.RATE = nPurchaseRate;
      } else {
        x.RATE = rate;
      }

      let value = x.RATE * netQty;
      productValue += value;
      x.VALUE = this.com.roundVal(value.toFixed(2));

      if (parseFloat(x.DISCOUNT) > 100) {
        x.DISCOUNT = (100).toFixed(0);
      }
      x.DISCOUNTAMT = this.com.roundVal(
        ((x.DISCOUNT * value) / 100).toFixed(2)
      );

      if (parseFloat(x.SALETAX) > 100) {
        x.SALETAX = 100;
      }
      x.SALETAXAMT = this.com.roundVal(
        ((x.SALETAX * (value - x.DISCOUNTAMT)) / 100).toFixed(2)
      );
      x.NETVALUE = this.com.roundVal(
        (value - parseFloat(x.DISCOUNTAMT) + parseFloat(x.SALETAXAMT)).toFixed(
          2
        )
      );

      grossAmount += parseFloat(this.getNum(x.NETVALUE));
    });

    this.totalQty = allTotalQty;
    this.purchaseInvoiceForm.get('grossAmount').setValue(this.com.roundVal(grossAmount.toFixed(2)));

    const disAmt = (grossAmount * this.purchaseInvoiceForm.get('discount').value) / 100;
    this.purchaseInvoiceForm.get('discountAmt').setValue(this.com.roundVal(disAmt.toFixed(2)));

    const otherCredit = this.purchaseInvoiceForm.get('otherCredit').value;
    const shipment = this.purchaseInvoiceForm.get('shipment').value;
    const discountAmt = this.purchaseInvoiceForm.get('discountAmt').value;
    const fTax = this.purchaseInvoiceForm.get('fTax').value;
    const whTax = this.purchaseInvoiceForm.get('whTax').value;

    let due = this.com.roundVal(
      (
        grossAmount +
        parseFloat(shipment) -
        (parseFloat(discountAmt) + parseFloat(otherCredit))
      ).toFixed(2)
    );

    let fTaxAmt = this.com.roundVal(
      ((parseFloat(fTax) * productValue) / 100).toFixed(2)
    );
    this.purchaseInvoiceForm.get('fTaxAmt').setValue(fTaxAmt);

    let whTaxAmt =
      (parseFloat(whTax) * (parseFloat(due) + parseFloat(fTaxAmt))) / 100;
    this.purchaseInvoiceForm
      .get('whTaxAmt')
      .setValue(this.com.roundVal(whTaxAmt.toFixed(2)));

    let totalDue = this.com.roundVal(
      (parseFloat(due) + parseFloat(fTaxAmt) + whTaxAmt).toFixed(2)
    );
    this.purchaseInvoiceForm
      .get('totalDue')
      .setValue(this.com.roundVal(parseFloat(totalDue).toFixed(2)));

    this.purchaseInvoiceForm
      .get('totalDue')
      .setValue(this.com.roundVal(parseFloat(totalDue).toFixed(2)));

    let recAmount = this.purchaseInvoiceForm.get('recAmount').value;

    if (parseFloat(recAmount) > parseFloat(totalDue)) {
      this.purchaseInvoiceForm
        .get('returnAmount')
        .setValue(
          this.com.roundVal(
            (parseFloat(recAmount) - parseFloat(totalDue)).toFixed(2)
          )
        );
      this.netDue = 0;
    } else {
      this.purchaseInvoiceForm.get('returnAmount').setValue(0);
      this.netDue = this.com.roundVal(
        (parseFloat(totalDue) - parseFloat(recAmount)).toFixed(2)
      );
    }
  }

  onChangeParty() {
    const cus = this.supplierList.find(
      (x) => x.code == this.purchaseInvoiceForm.get('party').value
    );

    if (cus == undefined) {
      this.purchaseInvoiceForm.get('discount').patchValue(0);
    } else {
      this.purchaseInvoiceForm.get('discount').patchValue(cus.commission);
    }

    this.onInputDis();

    this.isTaxDisable = false;

    if (cus.saleTax == 'nonfiler') {
      this.purchaseInvoiceForm
        .get('fTax')
        .setValue(localStorage.getItem('furtherTax'));
    } else if (cus.saleTax == 'filer') {
      this.purchaseInvoiceForm.get('fTax').setValue(0);
    } else if (cus.saleTax == 'taxfree') {
      this.purchaseInvoiceForm.get('fTax').setValue(0);
      this.appendedData.forEach((x, i) => {
        x.SALETAX = 0;
      });
      this.isTaxDisable = true;
    }

    if (cus.whTax == 'nonfiler') {
      this.purchaseInvoiceForm
        .get('whTax')
        .setValue(localStorage.getItem('whNonFiler'));
    } else if (cus.whTax == 'filer') {
      this.purchaseInvoiceForm
        .get('whTax')
        .setValue(localStorage.getItem('whFiler'));
    } else if (cus.whTax == 'taxfree') {
      this.purchaseInvoiceForm.get('whTax').setValue(0);
    } else {
      this.purchaseInvoiceForm.get('whTax').setValue(0);
    }

    this.calculation();
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

  onInputDisAmt() {
    const disAmt = this.purchaseInvoiceForm.get('discountAmt').value;
    const grossAmount = this.purchaseInvoiceForm.get('grossAmount').value;

    if (disAmt < 0 || disAmt == '' || disAmt == null) {
      this.purchaseInvoiceForm.get('discountAmt').patchValue(0);
    }

    if (parseFloat(disAmt) > parseFloat(grossAmount)) {
      this.purchaseInvoiceForm.get('discountAmt').patchValue(grossAmount);
    }

    var disPercent = (parseFloat(disAmt) / parseFloat(grossAmount)) * 100;
    this.purchaseInvoiceForm
      .get('discount')
      .patchValue(this.com.roundVal(disPercent.toFixed(2)));
    this.calculation();
  }

  onInputDis() {
    const discount = this.purchaseInvoiceForm.get('discount').value;
    const grossAmount = this.purchaseInvoiceForm.get('grossAmount').value;

    if (discount < 0 || discount == '' || discount == null) {
      this.purchaseInvoiceForm.get('discount').patchValue(0);
    }

    if (discount > 100) {
      this.purchaseInvoiceForm.get('discount').patchValue(100);
    }

    var disAmt = this.com.roundVal(
      ((parseFloat(grossAmount) * parseFloat(discount)) / 100).toFixed(2)
    );
    this.purchaseInvoiceForm.get('discountAmt').patchValue(disAmt);
    this.calculation();
  }

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
    this.mTax = i.SALETAX;
    this.mDiscount = i.DISCOUNT;
    this.mRate = i.RATE;
    this.mStatus = i.INACTIVE;
    this.mImage = this.basePath + i.IMAGE;
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

  onClickSave() {

    let type = this.purchaseInvoiceForm.get('vchType').value;
    let result = this.com.isStopEntry(type);

    if(!result){
      this.tostr.info("You are not allowed")
      return;
    }
    
    let body = this.purchaseInvoiceForm.value;
    body.fTax = this.purchaseInvoiceForm.get('fTax').value;
    body.fTaxAmt = this.purchaseInvoiceForm.get('fTaxAmt').value;
    body.whTax = this.purchaseInvoiceForm.get('whTax').value;
    body.whTaxAmt = this.purchaseInvoiceForm.get('whTaxAmt').value;

    if (this.appendedData.length == 0) {
      this.tostr.warning('Add Product....!');
      return;
    }

    if (body.party == null || body.party == undefined) {
      this.tostr.warning('Select Party....!');
      return;
    }

    let totalQty = this.purchaseInvoiceForm.get('totalQty').value;

    if (parseFloat(totalQty) != 0) {
      if (parseFloat(totalQty) != parseFloat(this.totalQty)) {
        this.tostr.warning('Qty Not Match...!');
        return;
      }
    }

    if (body.vchType == 'PI') {
      if (this.paymentMethod == 'Scan') {
        this.tostr.warning('Payment Method is Selected Wrong...!');
        return;
      }
    } else if (body.vchType == 'PR') {
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

      const grossAmount = this.purchaseInvoiceForm.get('grossAmount').value;
      const shipment = this.purchaseInvoiceForm.get('shipment').value;
      const returnAmount = this.purchaseInvoiceForm.get('returnAmount').value;
      const vchType = this.purchaseInvoiceForm.get('vchType').value;
      const invNo = this.purchaseInvoiceForm.get('invNo').value;

      let vDate = this.dp.transform(body.vchDate, 'yyyy-MM-dd');
      const dp: any = vDate.split('-');
      const dueDate = new Date(
        dp[0],
        dp[1] - 1,
        parseInt(dp[2]) + parseInt(body.termsDay)
      );

      const purchase: any[] = this.appendedData.map((i) => ({
        InvNo: invNo,
        VchType: body.vchType,
        PartyCode: body.party,
        PartyName: this.supplierList.find((x) => x.code == body.party).name,
        ProductCode: i.CODE,
        BatchNo: i.BATCHNO,
        Remarks: body.remarks,
        Discount: body.discount,
        OtherCredit: body.otherCredit,
        DiscountAmt: body.discountAmt,
        RecAmount: body.recAmount,
        ReturnAmount: returnAmount,
        RetQty: i.RETQTY,
        NetQty: i.NETQTY,
        Uom: i.UOMID.toString(),
        L5uom: i.UOMID.toString(),
        SID: i.SID,
        SMinRate: i.MINRATE,
        SMaxRate: i.MAXRATE,
        Rate: i.RATE,
        PurchaseRate: i.PURCHASERATE,
        ProductDis: i.DISCOUNT,
        ProductDisAmt: i.DISCOUNTAMT,
        SaleTax: i.SALETAX,
        SaleTaxAmt: i.SALETAXAMT,
        NetBill: i.NETVALUE,
        TotalNetBill:
          parseFloat(grossAmount) +
          parseFloat(body.whTaxAmt) +
          parseFloat(body.fTaxAmt) +
          parseFloat(shipment) -
          (parseFloat(body.discountAmt) + parseFloat(body.otherCredit)),
        OtherAmount: body.currencyConversion,
        Status: this.formStatus,
        ExpDate: this.dp.transform(
          new Date(
            i.EXPIRYDATE.split('/')[2],
            i.EXPIRYDATE.split('/')[1] - 1,
            i.EXPIRYDATE.split('/')[0]
          ),
          'yyyy-MM-dd'
        ),
        VchDate: this.dp.transform(body.vchDate, 'yyyy-MM-dd'),
        DueDate: this.dp.transform(dueDate, 'yyyy-MM-dd'),
        TotalQty: totalQty,
        Des: vchType == 'PI' ? 'Purchase Invoice' : 'Purchase Return',
        Shipment: shipment,
        OtherShipment: body.shipmentExpense,
        TermsDays: body.termsDay.toString(),
        FTax: body.fTax,
        FTaxAmt: body.fTaxAmt,
        WHT: body.whTax,
        WHTAmt: body.whTaxAmt,
        PaymentMethod: this.paymentMethod,
        PaymentMethodRmk: body.remarksAmountPaid,
        DtNow: new Date(),
      }));

      const division: any[] = this.divisionList.map((i) => ({
        CostCategoryCode: i.COSTCATEGORYCODE, 
        ProductName: this.appendedData.find((x) => x.CODE == i.PRODUCTCODE).PRODUCT,
        ProductCode: i.PRODUCTCODE,
        QTY: i.QTY ?? 0,
        Rate: i.RATE ?? 0, 
        Value: i.VALUE, 
        Commission: i.COMMISSION ?? 0,
        CommissionAmt: i.COMVALUE,
        NetValue: i.NETVALUE,
        JobLocId: this.JobList1.find((x) => x.ID == i.JOBNO).LOCID,
        JobNo: i.JOBNO,
        JobName: i.JOBNAME,
        ExpDate: this.dp.transform(i.EXPIRYDATE, 'yyyy-MM-dd'),
        SID: i.SID
      }));

      this.apiService
        .saveData('Purchase/SaveUpdatePurchaseInvoice', { purchase, division })
        .subscribe(async (r) =>  {
          if (r.status == true || r.status == 'true') {

            if(this.file){
              await this.uploadFile(invNo, vchType);
            }

            this.tostr.success('Save Successfully');
            this.purchaseInvoiceForm.get('invNo').patchValue(r.vchNo);
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

  onChangeFile(e: any){
    this.file = e.target.files[0];
  }

  async uploadFile(vchNo: any, vchType: any) {
    let formData = new FormData();
    formData.append('vchNo', vchNo.toString());
    formData.append('vchType', vchType);
    formData.append('file', this.file);

    const result = this.apiService .saveData('Accounts/FileUpload', formData).toPromise();
    return result
  }

  deleteInvoice(invNo: any): void {

    let type = this.purchaseInvoiceForm.get('vchType').value;
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
        invNo: invNo,
        vchType: this.purchaseInvoiceForm.get('vchType').value,
        dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      };

      this.apiService
        .deleteData('Purchase/DeletePurchaseInvoice', obj)
        .subscribe({
          next: (data) => {
            if (data == 'true' || data == true) {
              this.tostr.success('Delete Successfully');
              this.getInvoices();
              this.com.hideLoader();
              this.onClickRefresh();
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

  async editInvoice(invNo: any) {

    let type = this.purchaseInvoiceForm.get('vchType').value;
    let result = this.com.isStopEntry(type);

    if(!result){
      this.tostr.info("You are not allowed")
      return;
    }

    try {
      this.com.showLoader();
      const obj = {
        invNo: invNo,
        vchType: this.purchaseInvoiceForm.get('vchType').value,
      };

      const data = await this.apiService
        .getDataById('Purchase/EditPIInvoice', obj)
        .toPromise();

      this.isPrint = true;
      this.isDisabled = false;
      this.isNewClick = true;
      this.formStatus = 'Edit';
      this.appendedData = [];
      data.invoice.forEach((item: any) => {
        this.purchaseInvoiceForm
          .get('vchDate')
          ?.patchValue(
            new Date(
              item.VCHDATE.split('/')[2],
              item.VCHDATE.split('/')[1] - 1,
              item.VCHDATE.split('/')[0]
            )
          );
        this.purchaseInvoiceForm.get('invNo')?.patchValue(item.VCHNO);
        this.purchaseInvoiceForm.get('party')?.patchValue(item.PARTYCODE);
        this.purchaseInvoiceForm
          .get('shipmentExpense')
          ?.patchValue(item.SHIPMENTEXPENCE);
        this.purchaseInvoiceForm
          .get('currencyConversion')
          ?.patchValue(item.CURRENCYCONVERSION);
        this.purchaseInvoiceForm.get('totalQty')?.patchValue(item.TOTALQTY);
        this.purchaseInvoiceForm.get('discount')?.patchValue(item.NDISCOUNT);
        this.purchaseInvoiceForm
          .get('discountAmt')
          ?.patchValue(item.NDISCOUNTAMT);
        this.purchaseInvoiceForm
          .get('otherCredit')
          ?.patchValue(item.OTHERCREDIT);
        this.purchaseInvoiceForm.get('remarks')?.patchValue(item.REMARKS);
        this.purchaseInvoiceForm.get('shipment')?.patchValue(item.SHIPMENT);
        this.purchaseInvoiceForm.get('fTax')?.patchValue(item.FURTHERTAX);
        this.purchaseInvoiceForm.get('fTaxAmt')?.patchValue(item.FURTHERTAXAMT);
        this.purchaseInvoiceForm.get('whTax')?.patchValue(item.WHT);
        this.purchaseInvoiceForm.get('whTaxAmt')?.patchValue(item.WHTAMT);
        this.purchaseInvoiceForm.get('recAmount')?.patchValue(item.RECAMOUNT);
        this.purchaseInvoiceForm.get('termsDay')?.patchValue(item.TERMS);

        let ticks: number = new Date().getTime();
        item.sno = ticks;
        this.appendedData.push(item);

        $('.setvaluecash').find('a').removeClass('activePaymnet');
        if (item.PAYMENTTYPE == 'CP' || item.PAYMENTTYPE == 'CR') {
          $('.setvaluecash').find('.cash').addClass('activePaymnet');
        } else if (item.PAYMENTTYPE == 'BP' || item.PAYMENTTYPE == 'BR') {
          $('.setvaluecash').find('.debit').addClass('activePaymnet');
        } else {
          $('.setvaluecash').find('.cash').addClass('activePaymnet');
        }
      });

      this.divisionList = data.division;
      this.calculation();
      //this.divisionCalculation();
      $('.autoClose').click();
      this.com.hideLoader();
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
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

  resetForm() {
    let vDate = this.purchaseInvoiceForm.get('vchDate').value;
    this.category = null;
    this.productName = '';
    this.formStatus = '';
    this.barCode = '';
    this.netDue = '0.00';
    this.isPrint = false;
    this.isNewClick = false;
    this.PR = false;
    this.productList = [];
    this.appendedData = [];
    this.divisionList = [];
    this.filterDivision = [];
    this.purchaseInvoiceForm.reset();
    this.purchaseInvoiceForm.get('invNo').setValue(0);
    this.purchaseInvoiceForm.get('vchType').setValue('PI');
    this.purchaseInvoiceForm.get('vchDate').setValue(vDate);
    this.purchaseInvoiceForm.get('shipmentExpense').setValue(0);
    this.purchaseInvoiceForm.get('currencyConversion').setValue(0);
    this.purchaseInvoiceForm.get('totalQty').setValue(0);
    this.purchaseInvoiceForm.get('grossAmount').setValue(0);
    this.purchaseInvoiceForm.get('discount').setValue(0);
    this.purchaseInvoiceForm.get('discountAmt').setValue(0);
    this.purchaseInvoiceForm.get('otherCredit').setValue(0);
    this.purchaseInvoiceForm.get('shipment').setValue(0);
    this.purchaseInvoiceForm.get('fTax').setValue(0);
    this.purchaseInvoiceForm.get('fTaxAmt').setValue(0);
    this.purchaseInvoiceForm.get('whTax').setValue(0);
    this.purchaseInvoiceForm.get('whTaxAmt').setValue(0);
    this.purchaseInvoiceForm.get('totalDue').setValue(0);
    this.purchaseInvoiceForm.get('recAmount').setValue(0);
    this.purchaseInvoiceForm.get('returnAmount').setValue(0);
    this.purchaseInvoiceForm.get('termsDay').setValue(30);
  }

  isDisabled: boolean = true;

  async onClickNew() {

    let type = this.purchaseInvoiceForm.get('vchType').value;
    let result = this.com.isStopEntry(type);

    if(!result){
      this.tostr.info("You are not allowed")
      return;
    }

    this.isNewClick = true;
    this.isDisabled = false;
    this.formStatus = 'New';
    await this.getMax();
  }

  onClickRefresh() {
    $("#my-file").val('')
    this.isDisabled = true;
    this.totalQty = 0;
    this.file = null;
    this.resetForm();
  }

  // TERMS

  getTerms() {
    this.apiService.getData('Purchase/GetTerms').subscribe((data) => {
      this.termsList = data;
    });
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
          this.tostr.success('Save Successfully');
          this.refreshTerms();
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
            this.tostr.success('Delete Successfully');
            this.refreshTerms();
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
}
