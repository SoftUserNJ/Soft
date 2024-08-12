import { Component, ViewChild, ElementRef } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-avail-service',
  templateUrl: './avail-service.component.html',
  styleUrls: ['./avail-service.component.css'],
})
export class AvailServiceComponent {
  @ViewChild('availServiceLists', { static: false })
  availServiceLists!: ElementRef;

  availServiceForm!: FormGroup;

  isShowPage: boolean = true;
  isRowEdit: boolean = false;
  isDisabledTP: boolean = true;
  isShowTP: boolean = false;

  inProcess: boolean = true;
  due: boolean = false;
  clear: boolean = false;
  all: boolean = false;

  productId = 0;
  qtyInput: number = 1;
  product = '';
  costRate = '';
  saleRate = '';
  total: any;
  row: any = {};
  mainArea = '';
  timePeriod = '';
  showFirstDiv = true;

  isDisabledTerms: boolean = true;
  isShowTerms: boolean = false;
  termsDays = '';
  termsId = 0;

  mainAreaId = 0;
  mainAreaName = '';
  subAreaId = 0;
  subAreaName = '';

  availServiceList: any[] = [];
  availService_List: any[] = [];
  piProductList: any[] = [];
  serviceProdList: any[] = [];
  category: any[] = [];
  appendedData: any[] = [];
  customerList: any[] = [];
  mainAreaList: any[] = [];
  servicesList: any[] = [];
  services: any[] = [];
  subAreaList: any[] = [];
  products: any[] = [];
  bankCash: any[] = [];
  uomList: any[] = [];
  

  totalNetDue: number = 0;
  totalRecAmt: number = 0;
  totalBalance: number = 0;
  netDue: number;

  rate: any;
  tax: any;
  transNo: any;
  qty: number = 1;
  tp: any;

  fromDate: Date;
  toDate: Date;
  today: Date;
  productTaxAmount: any;
  totalDisc: any;
  totalDueAmount: number;
  discPerc: any;

  constructor(
    private apiService: ApiService,
    private fb: FormBuilder,
    private toastr: ToastrService,
    private dp: DatePipe
  ) {
    const today = new Date();
    this.today = today;
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  }

  ngOnInit() {
    this.formInit();
    this.getAvailService();
    this.calculateTotals();
    this.getAllDropDowns();
    this.disableFields();
  }

  formInit() {
    this.availServiceForm = this.fb.group({
      transNo: [''],
      transDate: [''],
      billingDate: [''],
      customerCode: [''],
      customerName: [''],
      customerContact: [''],
      mainAreaId: [''],
      subAreaId: [''],
      totalBill: [0],
      discount: [0],
      discountAmount: [0],
      remarks: [''],
      dueDateId: [0],
      totalDue: [''],
      paidAmount: [0],
      returnAmount: [0],
      paymentMethod: [''],
      netDue: [0],
      spVoucher: [0],
      status: [''],
      productNameId: [0],
      productName: [''],
      qty: [1],
      service: [''],
      serviceCode: [''],
      costRate: [0],
      productRate: [0],
      productTax: [0],
      productTaxAmount: [0],
      total: [0],
      stockCode: [''],
      productRemarks: [''],
      godownId: [0],
      rackId: [0],
      shelId: [0],
      uomId: [0],
      expDate: [''],
      dtNow: [''],
      totalAmount: [0],
      value: [0],
      category: [''],
      catProdRate: [''],
    });
    this.availServiceForm.get('transDate')?.setValue(new Date());
    this.availServiceForm.get('billingDate')?.patchValue(new Date());
    this.availServiceForm.get('totalAmount').disable;
    this.availServiceForm.get('totalDue')?.disable;
    this.availServiceForm.get('discountAmount').disable;
    this.availServiceForm.get('customerCode')?.patchValue(undefined);
    this.availServiceForm.get('service')?.patchValue(undefined);
    this.availServiceForm.get('category')?.patchValue(undefined);
    this.availServiceForm.get('productName')?.patchValue(undefined);
    this.availServiceForm.get('mainAreaId')?.patchValue(undefined);
    this.availServiceForm.get('subAreaId')?.patchValue(undefined);
  }

  togglePages() {
    this.isShowPage = !this.isShowPage;
    if (this.isShowPage) {
      this.onClickRefresh();
    }
  }

  toggleDivs() {
    this.showFirstDiv = !this.showFirstDiv;
  }

  getAllDropDowns() {
    this.getUoms();
    this.getTransNo();
    this.getCustomerList();
    this.getMainAreaList();
    this.getServiceProduct();
    this.getCategory();
    this.getServices();
    this.getBankCash();
  }

  disableFields(){
    this.availServiceForm.get('productTaxAmount')?.setValue(1);
    this.availServiceForm.get('transNo')?.disable();
    this.availServiceForm.get('totalBill')?.disable();
    this.availServiceForm.get('totalDue')?.disable();
    this.availServiceForm.get('paidAmount')?.disable();
  }

  
  onLength(event: any, length) {
    if (event.target.value > length) {
      event.target.value = length;
    }
  }

  public getSelectedRadio(): string {
    if (this.all) {
      return 'all';
    } else if (this.due) {
      return 'due';
    } else if (this.clear) {
      return 'clear';
    } else if (this.inProcess) {
      return 'inProcess';
    } else {
      return '';
    }
  }

  onClickFilter(event: any) {
    this.all = false;
    this.due = false;
    this.clear = false;
    this.inProcess = false;

    if (event.target.value == 'all') {
      this.all = true;
    } else if (event.target.value == 'due') {
      this.due = true;
    } else if (event.target.value == 'clear') {
      this.clear = true;
    } else if (event.target.value == 'inProcess') {
      this.inProcess = true;
    }
    this.getAvailService();
  }


  getAvailService() {
    
    const obj = {
      fromDate: this.dp.transform(this.fromDate, 'yyyy/MM/dd'),
      toDate: this.dp.transform(this.toDate, 'yyyy/MM/dd'),
      filterBy: this.getSelectedRadio() || 'inProcess',
    };

    this.apiService
      .getDataById('Sale/GetAvailService', obj)
      .subscribe((data) => {
        this.availServiceList = data;
        this.calculateTotals();
      });
  }

  onChangeUom(i: any, uoms: any[]) {
    
    const uom = uoms.find((z) => z.UOMID == i.target.value);
    const index = this.piProductList.findIndex(
      (x) => x.CODE.slice(-5) === uom.CODE.slice(-5)
    );
    const product = this.piProductList.find(
      (x) => x.CODE.slice(-5) === uom.CODE.slice(-5)
    );
    product.MAXRATE = uom.MAXRARE;
    product.UOM = uom.UOM;
    product.UOMID = uom.UOMID;
    this.piProductList[index] = product;
  }

  async getUoms() {
    const result = await this.apiService
      .getData('Inventory/GetProductUoms')
      .toPromise();
    this.uomList = result;
  }

  getTransNo() {
    
    this.apiService.getData('Sale/GetMaxService').subscribe((data) => {
      this.availServiceForm.get('transNo')?.patchValue(data[0].TRANSNO);
    });
  }

  getCustomerList() {
    this.apiService.getData('Sale/GetCustomer').subscribe((data) => {
      this.customerList = data;
    });
  }

  getCategory() {
    this.apiService.getData('Inventory/GetCategory').subscribe((result) => {
      this.category = result;
    });
  }

  onChangeCategory(event: any) {
    this.getPIProductList(event.id, '', '');
    this.piProductList = [];
  }

  onInputSearchProduct(event: any) {
    if (event.target.value.length > 3) {
      this.getPIProductList(0, event.target.value, '');
    }
  }

  getPIProductList(categoryId: any, productName: any, barcode: any) {
    
    const obj = {
      categoryId: categoryId,
      productName: productName,
      barCode: barcode,
      vchType: 'PR',
      vchDate: '2023-12-5'
    };
    this.apiService
      .getDataById('Purchase/PIProductList', obj)
      .subscribe((data) => {
        this.piProductList = data;
        data.forEach((x, i) => {
          x.UomList = this.uomList.filter(
            (z) => z.CODE.slice(-5) === x.CODE.slice(-5)
          );
          data[i] = x;
        });
      });
  }

  appendDataToOtherTable(item: any) {
    
    let row = item;
    row.QTY = 1;
    row.TAX = 0;
    this.qtyInput = 1;
    this.appendedData.push(row);
    //item = this.piProductList[0];
    this.calculateTotal(row) 
  }

  appendData(): void {
    
    let form = this.availServiceForm.value;
    //form.productName = this.productName;
    if (this.isRowEdit) {
      const index = this.availService_List.findIndex(
        (row) => row.sno === form.sno
      );
      if (index !== -1) {
        this.availService_List[index] = form;
        this.isRowEdit = false;
        return;
      }
    }

    
    form.sno = this.availService_List.length + 1;
    this.availService_List.push(form);
  }

  calculateTotal(i: any) {
    
    i.VALUE = i.QTY * i.MAXRATE;
    i.TAXAMOUNT = (i.TAX / 100) * i.VALUE;
    i.TOTAL =  i.VALUE +  i.TAXAMOUNT

    this.calculateTotals();
  }

  calculateTotals() {
    
    this.totalNetDue = this.availServiceList.reduce(
      (total: any, item: any) => total + (item.TOTALBILL || 0),
      0
    );

    this.totalRecAmt = this.availServiceList.reduce(
      (total: any, item: any) => total + (item.RECAMT || 0),
      0
    );

    this.totalBalance = this.availServiceList.reduce(
      (total: any, item: any) => total + (item.BALANCE || 0),
      0
    );

    const grossAmount = this.appendedData.reduce(
      (sum, item) => sum + item.TOTAL,
      0
    );
    this.availServiceForm
      .get('totalAmount')
      ?.patchValue(grossAmount.toFixed(2));
    this.availServiceForm
      .get('totalDue')
      ?.patchValue(grossAmount.toFixed(2));

    this.totalDisc = (this.availServiceForm.get('discount').value / 100) * this.availServiceForm.get('totalAmount').value
    this.availServiceForm.get('discountAmount')?.setValue( this.totalDisc.toFixed(2));
    this.totalDueAmount = this.availServiceForm.get('totalAmount').value - this.totalDisc;
    this.availServiceForm.get('totalDue')?.setValue(this.totalDueAmount.toFixed(2));
    const totalDueAmountAsString = this.totalDueAmount.toFixed(2);
    this.netDue = parseFloat(totalDueAmountAsString);
    this.availServiceForm.get('netDue')?.setValue(this.netDue);
    
    // this.discPerc = (this.availServiceForm.get('discountAmount').value / this.availServiceForm.get('totalAmount').value) * 100;
    // this.availServiceForm.get('discount')?.setValue( this.discPerc.toFixed(2));
  }

  getServices() {
    this.apiService.getData('Sale/GetServices').subscribe((result) => {
      this.services = result;
    });
  }

  onChangeService(event: any) {
    

    this.availServiceForm.patchValue({
      productRate: event.RATE,
      value: this.qty * event.RATE,
      productTax: event.TAX,
      productTaxAmount: (event.RATE / 100) * event.TAX,
      total: event.RATE + (event.RATE / 100) * event.TAX,
    });
  }

  onClearService(){
    this.availServiceForm.patchValue({
      productRate: '',
      value: '',
      productTax: '',
      productTaxAmount: '',
      total: '',
      totalAmount: '',
    });
  }

  onChangeProduct(event: any) {
    

    this.availServiceForm.patchValue({
      //productRate: event.Max,
      value: event.SaleRate,
      productTax: event.TAX,
      productTaxAmount: (event.RATE / 100) * event.TAX,
      total: event.RATE + (event.RATE / 100) * event.TAX,
    });
  }

  onInputQty(){
    const rate =  this.availServiceForm.get('productRate')?.value
    const qty =  this.availServiceForm.get('qty')?.value
    const taxAmount =  this.availServiceForm.get('productTaxAmount')?.value
  
    this.availServiceForm.get('totalAmount')?.setValue((parseFloat(rate) * parseFloat(qty)).toFixed(2)  );
  }

  getMainAreaList() {
    this.apiService.getData('Sale/GetMainArea').subscribe((data) => {
      this.mainAreaList = data;
    });
  }

  createUpdateMainArea() {
    const obj = {
      id: this.mainAreaId,
      name: this.mainAreaName,
      dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
    };

    this.apiService
      .saveObj('Sale/AddUpdateMainArea', obj)
      .subscribe((result) => {
        if (result == true || result == 'true') {
          this.getMainAreaList();
          this.toastr.success('Save Successfully');
          this.refreshMA();
        } else {
          this.toastr.error('Please Save Again');
        }
      });
  }

  editMainArea(id: any, name: any): void {
    this.mainAreaName = name;
    this.mainAreaId = id;
    this.isDisabledTP = false;
    this.isShowTP = true;
  }

  deleteMainArea(id: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {
      const obj = {
        id: id,
        dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      };

      this.apiService.deleteData('Sale/DeleteMainArea', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.getMainAreaList();
            this.toastr.success('Delete Successfully');
          } else if (data == 'false' || data == false) {
            this.toastr.error('Delete Again');
          }
        },
        error: (error) => {
          this.toastr.info(error.error.text);
        },
      });
    }
  }

  onChangeMainAera(event: any) {
    this.mainAreaId = event.id;
    var obj = {
      mainAreaId: event.id,
    };

    this.apiService.getDataById('Sale/GetSubArea', obj).subscribe((data) => {
      this.subAreaList = data;
    });
  }

  onClearMainArea() {
    this.subAreaList = [];
    this.availServiceForm.get('subAreaId')?.patchValue(null);
  }

  createUpdateSubArea() {
    const obj = {
      mainAreaId: this.mainAreaId,
      id: this.subAreaId,
      name: this.subAreaName,
      dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
    };

    this.apiService
      .saveObj('Sale/AddUpdateSubArea', obj)
      .subscribe((result) => {
        if (result == true || result == 'true') {
          this.onChangeMainAera({ id: obj.mainAreaId });
          this.toastr.success('Save Successfully');
          this.refreshSA();
        } else {
          this.toastr.error('Please Save Again');
        }
      });
  }

  editSubArea(id: any, name: any): void {
    this.subAreaName = name;
    this.subAreaId = id;
    this.isDisabledTP = false;
    this.isShowTP = true;
  }

  deleteSubArea(id: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {
      const obj = {
        mainAreaId: this.mainAreaId,
        id: id,
        dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      };

      this.apiService.deleteData('Sale/DeleteSubArea', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.onChangeMainAera({ id: obj.mainAreaId });
            this.toastr.success('Delete Successfully');
            this.refreshSA();
          } else if (data == 'false' || data == false) {
            this.toastr.error('Delete Again');
          }
        },
        error: (error) => {
          this.toastr.info(error.error.text);
        },
      });
    }
  }

  getServiceProduct() {
    this.apiService.getData('Sale/GetServiceProduct').subscribe((data) => {
      this.serviceProdList = data;
    });
  }

  createUpdateProducts() {
    
    const obj = {
      id: this.productId,
      name: this.product,
      costRate: this.costRate,
      saleRate: this.saleRate,
    };

    this.apiService
      .saveObj('Sale/SaveUpdateServiceProduct', obj)
      .subscribe((result) => {
        if (result == true || result == 'true') {
          this.getServiceProduct();
          this.toastr.success('Save Successfully');
          this.refreshSP();
        } else {
          this.toastr.error('Please Save Again');
        }
      });
  }

  editProduct(id: any, name: any, saleRate: any, costRate: any): void {
    this.product = name;
    this.productId = id;
    this.costRate = saleRate;
    this.saleRate = costRate;
    this.isDisabledTP = false;
    this.isShowTP = true;
  }

  deleteProduct(Id: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {
      const obj = {
        id: Id,
        dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      };

      this.apiService.deleteData('Sale/DelServiceProduct', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.getServiceProduct();
            this.toastr.success('Delete Successfully');
          } else if (data == 'false' || data == false) {
            this.toastr.error('Delete Again');
          }
        },
        error: (error) => {
          this.toastr.info(error.error.text);
        },
      });
    }
  }

  getBankCash() {
    this.apiService.getData('Sale/GetPDBank').subscribe((data) => {
      this.bankCash = data;
    });
  }

  deleteService(TRANSNO: any) {
    
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {
      const obj = {
        id: TRANSNO,
      };

      this.apiService.deleteData('Sale/DelAvailService', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.getAvailService();
            this.toastr.success('Delete Successfully');
          } else if (data == 'false' || data == false) {
            this.toastr.error('Delete Again');
          }
        },
        error: (error) => {
          this.toastr.info(error.error.text);
        },
      });
    }
  }

  onClickSave(): void {
    

    let body = this.availServiceForm.value;
  
    const availService: any[] = this.appendedData.map((data) => ({
      TransNo : this.availServiceForm.get('transNo').value,
      TransDate : this.dp.transform(body.transDate, 'yyyy-MM-dd'),
      BillingDate: this.dp.transform(body.billingDate, 'yyyy-MM-dd'),
      DtNow :this.dp.transform(new Date, 'yyyy-MM-dd'),
      ExpDate: this.dp.transform(new Date(data.EXPIRYDATE.split("/")[2], data.EXPIRYDATE.split("/")[1] - 1, data.EXPIRYDATE.split("/")[0]), 'yyyy-MM-dd'),
      CustomerCode: body.customerCode,
      CustomerName: body.customerName,
      CustomerContact: body.customerContact.toString(),
      MainAreaId: body.mainAreaId,
      SubAreaId: body.subAreaId,
      TotalBill: body.totalAmount,
      Discount: body.discount,
      DiscountAmount: body.discountAmount,
      Remarks: body.remarks,
      DueDateId: 15,
      TotalDue:  parseFloat(this.availServiceForm.get('totalDue').value),
      PaidAmount: 5000,
      ReturnAmount: parseFloat(this.availServiceForm.get('totalDue').value),
      PaymentMethod: 'Cash',
      NetDue: body.netDue,
      SPVoucher: 1000,
      Status:   'New',

      ProductNameId: this.availServiceForm.get('productName').value,
      ProductName: data.PRODUCT,
      Qty: data.QTY,
      Service: this.services.find((x) => x.CODE == body.service).NAME,
      ServiceCode: body.service,
      CostRate:  data.RATE,
      ProductRate: data.MAXRATE,
      ProductTax: data.TAX,
      ProductTaxAmount: data.TAXAMOUNT,
      Total: body.totalAmount,
      StockCode: data.STOCK.toString(),
      ProductRemarks: body.productRemarks,
      GodownId: data.GID,
      RackId: data.RID,
      ShelId: data.SID,
      UomId: data.UOMID,
    }));
    this.apiService
      .saveData('Sale/SaveAvailService', availService)
      .subscribe(
        (result) => {
          if (result == true || result == 'true') {
            this.toastr.success('Save Successfully');
            this.getTransNo();
            this.onClickRefresh();
          } else {
            this.toastr.error('Please Save Again');
          }
        },
        (error) => {
          this.toastr.error('Error');
        }
      );
  }

  async editAvailService(TRANSNO:any) {
    

    const obj = {
      id: TRANSNO,
    };

  const data = await this.apiService
      .getDataById('Sale/EditService', obj)
      .toPromise();

    this.isPageDisabled = false;
    this.isPageEnabled = true;
    this.appendedData = [];
    // data.forEach((item: any) => {
    //   this.availServiceForm
    //     .get('transDate')
    //     ?.patchValue(
    //       new Date(
    //         item.VCHDATE.split('/')[2],
    //         item.VCHDATE.split('/')[1] - 1,
    //         item.VCHDATE.split('/')[0]
    //       )
    //     );
    //   this.availServiceForm
    //     .get('billingDate')
    //     ?.patchValue(
    //       new Date(
    //         item.VCHDATE.split('/')[2],
    //         item.VCHDATE.split('/')[1] - 1,
    //         item.VCHDATE.split('/')[0]
    //       )
    //     );
      // this.availServiceForm.get('transNo')?.patchValue(item.VCHNO);
      // this.availServiceForm.get('billingDate')?.patchValue(item.DELIVERIBOY);
      // this.availServiceForm.get('transDate ')?.patchValue(item.ORDERTAKERID);
      // this.availServiceForm.get('customerCode ')?.patchValue(item.PARTYCODE.substring(9, 14));
      // this.availServiceForm.get('customerName ')?.patchValue(item.CUSTOMERNAME);
      // this.availServiceForm.get('customerContact ')?.patchValue(item.CUSTOMERCONTACT);
      // this.availServiceForm.get('mainAreaId')?.patchValue(item.NDISCOUNT);
      // this.availServiceForm.get('subAreaId')?.patchValue(item.DISCOUNTAMT);
      // this.availServiceForm.get('service')?.patchValue(item.OTHERCREDIT);
      // this.availServiceForm.get('discount')?.patchValue(item.REMARKS);
      // this.availServiceForm.get('discountAmount')?.patchValue(item.SHIPMENT);
      // this.availServiceForm.get('productRemarks')?.patchValue(item.RECAMOUNT);
      // this.availServiceForm.get('crRemarks')?.patchValue(item.CRREMARKS);
      // this.availServiceForm.get('returnAmt')?.patchValue(item.RETURNAMT);
      // this.availServiceForm.get('termsDay')?.patchValue(item.TERMS);

      // let ticks: number = new Date().getTime();
      // item.sno = ticks;
      // item.DISCOUNT = item.PRODUCTDISCOUNT
      // item.SALETAX = item.SALETAXRATE
      // item.UOM = item.UOMNAME
      // item.MAXRATE = item.RATE
      // this.appendedData.push(item);
      // this.calculateTotal(item);
    // });


    // /$('.autoClose').click();
  }



  removeRow(row: any): void {
    
    const confirmDelete = confirm('Are you sure you want to delete this item?');
    if (!confirmDelete) {
      return;
    }
    const indexToRemove = this.appendedData.findIndex(
      (item) => item.PRODUCT === row.PRODUCT
    );
    if (indexToRemove !== -1) {
      this.appendedData.splice(indexToRemove, 1);
    }
    this.calculateTotals();
  }

  resetForm() {
    this.piProductList = [];
    this.appendedData = [];
    this.availServiceForm.get('customerCode')?.patchValue(undefined);
    this.availServiceForm.get('customerName')?.patchValue(undefined);
    this.availServiceForm.get('customerContact')?.patchValue('');
    this.availServiceForm.get('mainAreaId')?.patchValue(undefined);
    this.availServiceForm.get('subAreaId')?.patchValue(undefined);
    this.availServiceForm.get('service')?.patchValue(undefined);
    this.availServiceForm.get('productName')?.patchValue(undefined);
    this.availServiceForm.get('value')?.patchValue(undefined);
    this.availServiceForm.get('category')?.patchValue(undefined);
    this.availServiceForm.get('productRate')?.patchValue('');
    this.availServiceForm.get('productTax')?.patchValue('');
    this.availServiceForm.get('productTaxAmount')?.patchValue('');
    this.availServiceForm.get('productRemarks')?.patchValue('');
    this.availServiceForm.get('total')?.patchValue('');
    this.availServiceForm.get('totalAmount')?.patchValue('');
    this.availServiceForm.get('qty')?.patchValue('');
    this.availServiceForm.get('totalBill')?.patchValue('');
    this.availServiceForm.get('discount')?.patchValue('');
    this.availServiceForm.get('discountAmount')?.patchValue('');
    this.availServiceForm.get('remarks')?.patchValue('');
    this.availServiceForm.get('totalDue')?.patchValue('');
    this.availServiceForm.get('paidAmount')?.patchValue('');
    this.availServiceForm.get('returnAmount')?.patchValue('');
    this.availServiceForm.get('paymentMethod')?.patchValue('');
  }

  searchGrid(event: any): void {
    const tableElement = this.availServiceLists.nativeElement;
    const rows = tableElement.querySelectorAll('tr');
    let amount = 0;

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

  isPageEnabled: boolean = false;
  isPageDisabled: boolean = true;

  enablePage() {
    this.isPageDisabled = false;
  }

  onClickRefresh() {
    this.isPageDisabled = true;
  }

  newMainArea() {
    this.refreshMA();
    this.isDisabledTP = false;
    this.isShowTP = true;
  }

  refreshMA() {
    this.mainAreaName = '';
    this.isDisabledTP = true;
    this.isShowTP = false;
  }

  newServiceProduct() {
    this.refreshSP();
    this.isDisabledTP = false;
    this.isShowTP = true;
  }

  refreshSP() {
    this.product = '';
    this.costRate = '';
    this.saleRate = '';
    this.isDisabledTP = true;
    this.isShowTP = false;
  }

  newSubArea() {
    this.refreshSA();
    this.isDisabledTP = false;
    this.isShowTP = true;
  }

  refreshSA() {
    this.subAreaName = '';
    this.isDisabledTP = true;
    this.isShowTP = false;
  }
}
