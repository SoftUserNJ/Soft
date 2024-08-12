import { Component, ViewChild, ElementRef } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { environment } from 'src/environment/environmemt';
declare const $: any;

@Component({
  selector: 'app-dispatch-order-sal',
  templateUrl: './dispatch-order-sal.component.html',
  styleUrls: ['./dispatch-order-sal.component.css']
})
export class DispatchOrderSalComponent {
  
    @ViewChild('InvoiceList', { static: false }) InvoiceList!: ElementRef;
    @ViewChild('piProductLists', { static: false }) piProductLists!: ElementRef;
  
    basePath = environment.basePath;
  
    // LOCID
    locIdList: any[] = [];
    
    // CURRENCY
    currencyList: any[] = [];
  
    // PARTY
    partyMainList: any = [];
    partySubList: any = [];
    subPartyList: any[] = [];
    partyAddress: String;
    partyCity: String;
    CNIC: String;
  
    // GODOWNS
    godownsList: any[] = [];
  
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
    amount: any;
    maxSaleRate: any;
    minSaleRate: any;
  
    // Add Product
    appendedData: any[] = [];
    totalQty: any;
  
    // BILL
    netDue: any;
  
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
    formStatus: any = '';
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
    cmpDate:Date;
    isPrint: boolean = false;
  
   
    vchtype = [
      {id: 1, name: 'DO-Sale'}
    ]
  
    constructor(
      private apiService: ApiService,
      private fb: FormBuilder,
      private tostr: ToastrService,
      private dp: DatePipe,
    ) {
      const today = new Date();
      this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
      this.cmpDate = new Date(today.getFullYear(), today.getMonth(), 1);
      this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
    }
  
    // onClickPrint(tag: any) {
    //   const nowDate = this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss');
    //   const vchNo = this.purchaseInvoiceForm.get('vchNo').value;
    //   const vchType = this.purchaseInvoiceForm.get('vchType').value;
    //   const vchDate = this.dp.transform(this.purchaseInvoiceForm.get('vchDate').value, 'yyyy/MM/dd');
    //   let url = "";
      
    //   if(tag == "invoice"){
    //     url = `Print-report?vchNo=${vchNo}&vchType=${vchType}&printNo=1&nowDate=${nowDate}`;
    //   }
    //   else if(tag == "loading"){
    //     url = `LDG-report?vchNo=${vchNo}&vchType=${vchType}&printNo=1&nowDate=${nowDate}`;
    //   }
    //   else if(tag == "vch"){
    //     url = `PrintVoucherRangeWise?DateFrom=${vchDate}&DateTo=${vchDate}&VchType=${vchType
    //     }&VchNoFrom=${vchNo}&VchNoTo=${vchNo}&CompId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}`;
    //   }
  
    //   const modalConfig: ModalOptions = {
    //     class: 'custom-modal-width',
    //     initialState: {
    //       reportUrl: url,
    //     },
    //   };
  
    //   this.modalRef = this.modalService.show(ReportModalComponent, modalConfig);
    // }
  
    // onClickRowReport(invoiceNo: string, tag: string) {
    //   let url = '';
  
    //   const nowDate = this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss');
  
    //   if (tag === 'invoice') {
    //     url = `Inv-report?vchNo=${invoiceNo}&vchType=${this.purchaseInvoiceForm.get('vchType').value}&printNo=${this.printNo}&nowDate=${nowDate}`;
    //   } else if (tag === 'loading') {
    //     url = `Inv-report?vchNo=${invoiceNo}&vchType=${this.purchaseInvoiceForm.get('vchType').value}&printNo=${this.printNo}&nowDate=${nowDate}`;
        
    //   }
  
    //   const modalConfig: ModalOptions = {
    //     class: 'custom-modal-width',
    //     initialState: {
    //       reportUrl: url,
    //     },
    //   };
  
    //   this.modalRef = this.modalService.show(ReportModalComponent, modalConfig);
    // }
  
    async ngOnInit() {
      this.formInit();
      await this.getUoms();
      await this.getLocId();
      await this.getCurrency();
      await this.getGodowns();
      await this.getPartyMain();
      await this.getSubParty();
      //await this.getLocation();
      this.getCategory();
      // this.getSupplier();
      // this.getTerms();
      // this.resetForm();
      this.getCategory();
    }
  
    formInit() {
      this.purchaseInvoiceForm = this.fb.group({
        vchNo: [0],
        vchDate: [new Date()],
        vchType: ['DO-Sales'],
        party: [undefined],
        shipmentExpense: [0],
        currencyConversion: [0],
        totalQty: [0],
        grossAmount: [0],
        discount: [0],
        discountAmt: [0],
        otherCredit: [0],
        remarks: [''],
        shipment: [0],
        totalDue: [0],
        recAmount: [0],
        remarksAmountPaid: [''],
        returnAmount: [0],
        termsDay: [0],
        dtNow: new Date(),
  
        locId: [undefined],
        credit: [undefined],
        currecny: [undefined],
        godown: [undefined],
        partyMain: [undefined],
        partySub: [undefined],
        subParty: [undefined],
        
        LocID2: [undefined],
        // partyAddress: [''],
        // partyCity: [''],
        // CNIC: [''],
      });
  
      // this.purchaseInvoiceForm.get('vchNo').disable();
      // this.purchaseInvoiceForm.get('grossAmount').disable();
      // this.purchaseInvoiceForm.get('shipment').disable();
      // this.purchaseInvoiceForm.get('totalDue').disable();
      // this.purchaseInvoiceForm.get('returnAmount').disable();
    }
  
    async getUoms() {
      const result = await this.apiService
        .getData('Test/GetDOProductUoms')
        .toPromise();
      

      this.uomList = result;
    }
  
    async getLocId() {
      const result = await this.apiService
        .getData('Test/GetDoSaleLocation')
        .toPromise();
      this.locIdList = result;
      
    }
  
    async getCurrency() {
      const result = await this.apiService
        .getData('Test/GetDoSaleCurrency')
        .toPromise();
      this.currencyList = result;
    }
    
    async getGodowns() {
      const result = await this.apiService
        .getData('Test/GetDoSaleGodown')
        .toPromise();
      this.godownsList = result;
    }
  
    
    async getPartyMain() {
      this.apiService.getData('Test/GetDoSalePartyMain').subscribe((data) => {
        this.partyMainList = data;
      });
    }
  
    onPartyMainClear() {
      this.purchaseInvoiceForm.get('partyMain')?.patchValue(undefined);
      this.purchaseInvoiceForm.get('partySub')?.patchValue(undefined);
      this.partySubList = [];
      this.onPartySubClear();
    }
  
    getPartySub(event: any) {
      this.purchaseInvoiceForm.get('partySub')?.patchValue(undefined);
      this.partySubList = [];
      this.apiService
        .getDataById('Test/GetDoSalePartySubById', { code: event.CODE })
        .subscribe((data) => {
          this.partySubList = data;
        });
    }
  
    onPartySubClear() {
      this.partyAddress = '';
      this.partyCity = '';
      this.CNIC = '';
    }
  
    partyDetails(event: any){
  
      let pDetails = this.partySubList.find(
        (i) => i.CODE === event.CODE);
  
      this.partyAddress = pDetails.ADDRESS;
      this.partyCity = pDetails.CITY;
      this.CNIC = pDetails.NTN + ' / ' + pDetails.CNIC;
    }
  
    async getSubParty() {
      const result = await this.apiService
        .getData('Test/GetDoSaleSubParty')
        .toPromise();
      this.subPartyList = result;
    }
  
    async getLocation() {
      const result = await this.apiService
        .getData('Inventory/GetLocation')
        .toPromise();
      this.locationList = result;
    }
  
    // getInvoices() {
    //   const obj = {
    //     fromDate: this.dp.transform(this.fromDate, 'yyyy/MM/dd'),
    //     toDate: this.dp.transform(this.toDate, 'yyyy/MM/dd'),
    //     vchType: this.purchaseInvoiceForm.get('vchType').value,
    //   };
  
    //   this.apiService
    //     .getDataById('Purchase/GetPIInvoices', obj)
    //     .subscribe((data) => {
    //       this.invoiceList = data;
    //     });
    // }
  
    // onClickPI(event: any): void {
    //   if (this.isNewClick) {
    //     event.preventDefault();
    //     return;
    //   }
  
    //   this.purchaseInvoiceForm.get('vchType').setValue('PI');
    //   this.PR = false;
    // }
  
    // onClickPR(event: any): void {
    //   if (this.isNewClick) {
    //     event.preventDefault();
    //     return;
    //   }
    //   this.purchaseInvoiceForm.get('vchType').setValue('PR');
    //   this.PR = true;
    // }
  
    getMax() {
      this.apiService
        .getDataById('Test/GetDoSaleVchNo', {
          vchType: this.purchaseInvoiceForm.get('vchType').value,
        })
        .subscribe((data) => {
          
          this.purchaseInvoiceForm.get('vchNo')?.patchValue(data[0].VCHNO);
        });
    }
  
    getCategory() {
      this.apiService.getData('Test/GetCategory').subscribe((result) => {
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
        // vchDate: this.dp.transform(
        //   this.purchaseInvoiceForm.get('vchDate').value,
        //   'yyyy/MM/dd'
        // ),
        // vchType: this.purchaseInvoiceForm.get('vchType').value,
      };
  
      this.apiService
        .getDataById('Test/DOProductList', obj)
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
      this.apiService.getData('Purchase/GetSupplier').subscribe((result) => {
        this.supplierList = [{ id: 0, name: '---Select Supplier---' }];
        this.supplierList.push(...result);
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
      this.expiryDate = new Date(dp[2], dp[1] - 1, dp[0]);
      this.costRate = i.RATE;
      this.amount = (this.onQty * this.costRate).toFixed(2);
      this.maxSaleRate = i.MAXRATE;
      this.minSaleRate = i.MINRATE;
  
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
      this.expiryDate = new Date(dp[2], dp[1] - 1, dp[0]);
      this.costRate = i.RATE;
      this.amount = (this.onQty * this.costRate).toFixed(2);
      this.maxSaleRate = i.MAXRATE;
      this.minSaleRate = i.MINRATE;
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
  
      this.amount = (this.onQty * this.costRate).toFixed(2);
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
          x.SID == row.SID
      );
  
      row.QTY = this.onQty;
      row.PURCHASERATE = this.costRate;
      row.PURCHASEVALUE = row.QTY * this.costRate;
      row.MAXRATE = this.maxSaleRate;
      row.MINRATE = this.minSaleRate;
      row.EXPIRYDATE = this.dp.transform(this.expiryDate, 'dd/MM/yyyy');
      row.SID = this.location;
  
      if (index !== -1) {
        row.QTY = this.onQty;
        row.RETQTY = this.appendedData[index].RETQTY;
        this.appendedData[index] = row;
        $('#QtyModal').modal('hide');
        this.calculation();
        return;
      }
  
      row.RETQTY = 0;
      let ticks: number = new Date().getTime();
      row.sno = ticks;
      this.appendedData.push(row);
      $('#QtyModal').modal('hide');
      this.calculation();
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
        this.purchaseInvoiceForm.get('shipment').setValue(ship.toFixed(2));
      } else {
        upTotalQty = this.getNum(shipExp / totalQty);
        ship = shipExp;
        this.purchaseInvoiceForm.get('shipment').setValue(ship.toFixed(2));
      }
  
      this.appendedData.forEach((x, i) => {
        let rate = x.PURCHASERATE;
        let netQty = x.QTY - x.RETQTY;
        allTotalQty += netQty;
        x.NETQTY = netQty;
        let purchaseRae = upTotalQty * netQty;
        let purRate = netQty * rate + purchaseRae;
        let nPurchaseRate = purRate / netQty;
        x.PURCHASEVALUE = (netQty * rate).toFixed(2);
  
        if (nPurchaseRate != 0) {
          x.RATE = nPurchaseRate.toFixed(2);
        } else {
          x.RATE = rate.toFixed(2);
        }
  
        let value = x.RATE * netQty;
        productValue += value;
        x.VALUE = value.toFixed(2);
  
        if (parseFloat(x.DISCOUNT) > 100) {
          x.DISCOUNT = (100).toFixed(0);
        }
        x.DISCOUNTAMT = ((x.DISCOUNT * value) / 100).toFixed(2);
  
        if (parseFloat(x.SALETAX) > 100) {
          x.SALETAX = 100;
        }
        x.SALETAXAMT = ((x.SALETAX * (value - x.DISCOUNTAMT)) / 100).toFixed(2);
        x.NETVALUE = (
          value -
          parseFloat(x.DISCOUNTAMT) +
          parseFloat(x.SALETAXAMT)
        ).toFixed(2);
  
        grossAmount += parseFloat(this.getNum(x.NETVALUE));
      });
  
      this.totalQty = allTotalQty;
      this.purchaseInvoiceForm
        .get('grossAmount')
        .setValue(grossAmount.toFixed(2));
  
      const otherCredit = this.purchaseInvoiceForm.get('otherCredit').value;
      const shipment = this.purchaseInvoiceForm.get('shipment').value;
      const discountAmt = this.purchaseInvoiceForm.get('discountAmt').value;
  
      let totalDue = (
        grossAmount -
        parseFloat(shipment) -
        (parseFloat(discountAmt) + parseFloat(otherCredit))
      ).toFixed(2);
      this.purchaseInvoiceForm
        .get('totalDue')
        .setValue(parseFloat(totalDue).toFixed(2));
  
      let recAmount = this.purchaseInvoiceForm.get('recAmount').value;
  
      if (parseFloat(recAmount) > parseFloat(totalDue)) {
        this.purchaseInvoiceForm
          .get('returnAmount')
          .setValue((parseFloat(recAmount) - parseFloat(totalDue)).toFixed(2));
        this.netDue = 0;
      } else {
        this.purchaseInvoiceForm.get('returnAmount').setValue(0);
        this.netDue = (parseFloat(totalDue) - parseFloat(recAmount)).toFixed(2);
      }
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
      this.purchaseInvoiceForm.get('discount').patchValue(disPercent.toFixed(2));
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
  
      var disAmt = (
        (parseFloat(grossAmount) * parseFloat(discount)) /
        100
      ).toFixed(2);
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
      let body = this.purchaseInvoiceForm.value;
  
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
  
      const grossAmount = this.purchaseInvoiceForm.get('grossAmount').value;
      const shipment = this.purchaseInvoiceForm.get('shipment').value;
      const returnAmount = this.purchaseInvoiceForm.get('returnAmount').value;
      const vchType = this.purchaseInvoiceForm.get('vchType').value;
      const invNo = this.purchaseInvoiceForm.get('vchNo').value;
  
      let vDate =  this.dp.transform(body.vchDate, 'yyyy-MM-dd');
      const dp: any = vDate.split("-");
      const dueDate = new Date(dp[0], dp[1] -1, parseInt(dp[2]) + parseInt(body.termsDay));
  
      const invoice: any[] = this.appendedData.map((i) => ({
        InvNo: invNo,
        VchType: body.vchType,
        PartyCode: body.party,
        PartyName: this.supplierList.find((x) => x.id == body.party).name,
        ProductCode: i.CODE,
        Remarks: body.remarks,
        Discount: body.discount,
        OtherCredit: body.otherCredit,
        DiscountAmt: body.discountAmt,
        RecAmount: body.recAmount,
        ReturnAmount: returnAmount,
        RetQty: i.RETQTY,
        NetQty: i.NETQTY,
        Uom: i.UOMID,
        L5uom: i.UOMID,
        SID: i.SID,
        SMinRate: i.MINRATE,
        SMaxRate: i.MACRATE,
        Rate: i.RATE,
        PurchaseRate: i.PURCHASERATE,
        SaleTax: i.SALETAX,
        SaleTaxAmt: i.SALETAXAMT,
        NetBill: i.NETVALUE,
        TotalNetBill:
          parseFloat(grossAmount) -
          parseFloat(shipment) -
          parseFloat(body.discountAmt) -
          parseFloat(body.otherCredit),
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
        TermsDays: body.termsDay,
        PaymentMethod: this.paymentMethod,
        PaymentMethodRmk: body.remarksAmountPaid,
        DtNow: new Date(),
      }));
  
      this.apiService
        .saveObj('Purchase/SaveUpdatePurchaseInvoice', {
          invoice: JSON.stringify(invoice),
        })
        .subscribe((result) => {
          if (result == true || result == 'true') {
            this.tostr.success('Save Successfully');
            // this.getInvoices();
            this.formStatus = 'Edit';
            this.isPrint = true;
            //this.onClickRefresh();
          } else {
            this.tostr.error('Please Save Again');
          }
        });
    }
  
    deleteInvoice(invNo: any): void {
      const confirmDelete = confirm('Are you sure you want to delete this item?');
  
      if (confirmDelete == true) {
        const obj = {
          invNo: invNo,
          vchType: this.purchaseInvoiceForm.get('vchType').value,
        };
  
        this.apiService
          .deleteData('Purchase/DeletePurchaseInvoice', obj)
          .subscribe({
            next: (data) => {
              if (data == 'true' || data == true) {
                // this.tostr.success('Delete Successfully');
                // this.getInvoices();
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
  
    async editInvoice(invNo: any) {
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
      data.forEach((item: any) => {
        this.purchaseInvoiceForm
          .get('vchDate')
          ?.patchValue(
            new Date(
              item.VCHDATE.split('/')[2],
              item.VCHDATE.split('/')[1] - 1,
              item.VCHDATE.split('/')[0]
            )
          );
        this.purchaseInvoiceForm.get('vchNo')?.patchValue(item.VCHNO);
        this.purchaseInvoiceForm
          .get('party')
          ?.patchValue(item.PARTYCODE.substring(9, 14));
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
        this.purchaseInvoiceForm.get('otherCredit')?.patchValue(item.OTHERCREDIT);
        this.purchaseInvoiceForm.get('remarks')?.patchValue(item.REMARKS);
        this.purchaseInvoiceForm.get('shipment')?.patchValue(item.SHIPMENT);
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
  
      this.calculation();
  
      $('.autoClose').click();
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
      this.category = undefined;
      this.productName = '';
      this.formStatus = '';
      this.barCode = '';
      this.netDue = '0.00';
      this.isPrint = false;
      this.isNewClick = false;
      this.PR = false;
      this.productList = [];
      this.appendedData = [];
      this.purchaseInvoiceForm.reset();
      this.purchaseInvoiceForm.get('vchNo').setValue(0);
      this.purchaseInvoiceForm.get('vchType').setValue('PI');
      this.purchaseInvoiceForm.get('vchDate').setValue(new Date());
      this.purchaseInvoiceForm.get('shipmentExpense').setValue(0);
      this.purchaseInvoiceForm.get('currencyConversion').setValue(0);
      this.purchaseInvoiceForm.get('totalQty').setValue(0);
      this.purchaseInvoiceForm.get('grossAmount').setValue(0);
      this.purchaseInvoiceForm.get('discount').setValue(0);
      this.purchaseInvoiceForm.get('discountAmt').setValue(0);
      this.purchaseInvoiceForm.get('otherCredit').setValue(0);
      this.purchaseInvoiceForm.get('shipment').setValue(0);
      this.purchaseInvoiceForm.get('totalDue').setValue(0);
      this.purchaseInvoiceForm.get('recAmount').setValue(0);
      this.purchaseInvoiceForm.get('returnAmount').setValue(0);
      this.purchaseInvoiceForm.get('termsDay').setValue(0);
    }
  
    isDisabled: boolean = true;
  
    onClickNew() {
      this.isNewClick = true;
      this.isDisabled = false;
      this.formStatus = 'New';
      this.getMax();
    }
  
    onClickRefresh() {
      this.isDisabled = true;
      //this.resetForm();
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
