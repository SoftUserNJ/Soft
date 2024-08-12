import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ApiService } from 'src/app/services/api.service';
import { DatePipe } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/services/auth.service';
import { environment } from '../../../../environment/environmemt';
import { CommonService } from 'src/app/services/common.service';
import { ActivatedRoute, Router } from '@angular/router';
declare const $: any;

@Component({
  selector: 'app-payment',
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.css'],
})
export class PaymentComponent {
  isShowPage: boolean = true;
  isDisabled: boolean = true;
  isDayClose: any = false;
  dayCloseDate: Date = new Date();

  basePath = environment.basePath;
  lastVchAccount: any = null;
  costCenter = localStorage.getItem('costCenter');
  locId = localStorage.getItem('locId');

  tag: any = '';

  // DATE
  fromDate: Date;
  toDate: Date;
  vchDate: Date;
  chqDate: Date;
  AccountHead: any[] = [];
  BackCash: any[] = [];
  JobList: any[] = [];
  NGAccountHome: any;
  bankcashtName: any;
  VoucherList: any[] = [];
  invoiceList: any[] = [];
  vchNo: any;
  vchType: string = 'CP';
  bankPayment: boolean = false;
  addData: any[] = [];
  paymentForm: FormGroup;
  isNewClick: boolean = false;
  isNew: boolean = false;
  isRowEdit: boolean = false;
  totalAmount: number = 0;
  tax1: number = 0;
  tax1Name: string = 'Tax 1';
  taxAmt1: any = 0;
  taxList1: any;
  tax2: number = 0;
  tax2Name: string = 'Tax 2';
  taxAmt2: any = 0;
  taxList2: any;
  totalTax: any = 0;
  netAmount: any = 0;
  previousBalance: any = 0;
  totalAdjustAmount: any = 0;
  totalDueAmount: number = 0;
  totalRecAmount: number = 0;
  totalPenddingAmount: number = 0;
  isAutoAdj: boolean = true;
  isPreviousBal: boolean = true;
  isCalculation: boolean = true;
  isPrint: boolean = false;
  srcPdfImg: any;
  file: File | null = null;
  row: any;
  fileList: any;
  isPdf: boolean = false;

  // DISCOUNT NGMODEL
  invoiceNo: any;
  grossAmount: any;
  discount: any;
  disAmount: any;
  otherCredit: any;
  creditRemarks: any;
  shipment: any;
  netDue: any;

  ntimer: any;
  ntimeout: any = 500;

  @ViewChild('voucherList', { static: false }) voucherList!: ElementRef;

  constructor(
    private apiService: ApiService,
    private dp: DatePipe,
    private fb: FormBuilder,
    private com: CommonService,
    private auth: AuthService,
    private toastr: ToastrService,
    private activatedRoute: ActivatedRoute,
    private router: Router
  ) {
    const today = new Date();
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
    this.vchDate = today;
    this.chqDate = today;
  }

  formInit() {
    this.paymentForm = this.fb.group({
      sno: [0],
      bankCash: [null],
      mainDes: [''],
      accountName: [''],
      accountCode: [null],
      amount: [''],
      description: [''],
      chqNo: [''],
      chqDate: [this.chqDate],
      jobNo: [null],
    });
  }

  printReport() {
    const vchDate = this.vchDate;
    const vchNo = this.vchNo;

    const fromDate = this.dp.transform(vchDate, 'yyyy/MM/dd');
    const toDate = this.dp.transform(vchDate, 'yyyy/MM/dd');

    let url = `PrintVoucherRangeWise?DateFrom=${fromDate}&DateTo=${toDate}&VchType=${
      this.vchType
    }&VchNoFrom=${vchNo}&VchNoTo=${vchNo}&CompId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}`;
    this.com.viewReport(url);
  }

  openRowReport(item: any) {
    var dp = item.VCHDATE.split('/');
    const date = this.dp.transform(
      new Date(dp[2], dp[1] - 1, dp[0]),
      'yyyy/MM/dd'
    );

    let url = `PrintVoucherRangeWise?DateFrom=${date}&DateTo=${date}&VchType=${
      item.VCHTYPE
    }&VchNoFrom=${item.VCHNO}&VchNoTo=${
      item.VCHNO
    }&CompId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}`;
    this.com.viewReport(url);
  }

  onChangeFile(e: any) {
    this.file = e.target.files[0];
  }

  onClickModel(item: any) {
    this.row = item;
    this.onRefreshModel();
    this.getFile(item);
  }

  onFileSelected(event: any) {
    this.file = event.target.files[0];
    if (this.file && this.file.type === 'application/pdf') {
      this.isPdf = true;
    } else {
      this.isPdf = false;
    }

    this.srcPdfImg = URL.createObjectURL(event.target.files[0]);
  }

  async onClickUpload(tag: any) {
    let item = this.row;
    let formData = new FormData();
    formData.append('vchNo', item.VCHNO.toString());
    formData.append('vchType', item.VCHTYPE);
    formData.append('file', this.file);

    const result = await this.apiService
      .saveData('Accounts/FileUpload', formData)
      .toPromise();

    if (tag) {
      if (result == true || result == 'true') {
        this.toastr.success('Save Successfully');
        this.getFile(item);
        this.onRefreshModel();
      } else {
        this.toastr.error('Please Save Again');
      }
    }
  }

  getFile(item: any) {
    var obj = {
      vchType: item.VCHTYPE,
      vchNo: item.VCHNO,
    };

    this.apiService
      .getDataById('Accounts/GetFiles', obj)
      .subscribe((result) => {
        if (result.length == 0) {
          this.fileList = [];
          return;
        }
        this.fileList = result;
        this.onViewFile(result[0]);
      });
  }

  onRemoveFile(item: any) {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (!confirmDelete) {
      return;
    }

    var obj = {
      name: item.name,
      path: item.path,
    };

    this.apiService
      .deleteData('Accounts/DeleteFile', obj)
      .subscribe((result) => {
        if (result == true || result == 'true') {
          this.getFile(this.row);
          this.onRefreshModel();
          this.toastr.success('Delete Successfully');
        } else {
          this.toastr.error('Please Delete Again');
        }
      });
  }

  onRefreshModel() {
    this.file = null;
    this.isPdf = false;
    this.srcPdfImg = '';
  }

  onViewFile(item: any) {
    let ext = item.name.substring(item.name.lastIndexOf('.') + 1);

    if (ext.toLowerCase() == 'pdf') {
      this.isPdf = true;
    } else {
      this.isPdf = false;
    }

    this.srcPdfImg = this.basePath + '/' + item.path + '/' + item.name;
  }

  async ngOnInit() {
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
    this.activatedRoute.queryParams.subscribe((params) => {
      this.tag = params.tag;
    });

    this.isDayClose = this.auth.dayClose();
    this.formInit();
    await this.getDayClose();
    this.getAccountHead();
    await this.getInvoiceList();
    this.getTax();
    this.getTax2();
    this.JobList = await this.com.getJobList(true);
  }

  togglePages() {
    this.isShowPage = !this.isShowPage;
    if (this.isShowPage) {
      this.onClickRefresh();
      this.paymentForm.get('jobNo')?.patchValue(null);
      this.bankPayment = false;
      this.vchType = 'CP';
    }
  }

  async getDayClose() {
    if (this.isDayClose == 'true') {
      const data = await this.apiService
        .getDataById('Utilities/LastCloseDate', { locId: this.locId })
        .toPromise();
      const date = data[0].DATE.split('/');
      this.dayCloseDate = new Date(date[2], date[1] - 1, date[0]);

      this.fromDate = this.dayCloseDate;
      this.toDate = this.dayCloseDate;
      this.vchDate = this.dayCloseDate;
    }
  }

  getAccountHead() {
    this.apiService
      .getDataById('Accounts/GetAccountsList', { module: this.tag })
      .subscribe((data) => {
        this.AccountHead = data;
      });
  }

  async getInvoiceList() {
    const obj = {
      fromDate: this.dp.transform(this.fromDate, 'yyyy/MM/dd'),
      toDate: this.dp.transform(this.toDate, 'yyyy/MM/dd'),
      tag: 'Payment',
      module: this.tag,
    };

    const data = await this.apiService
      .getDataById('Purchase/GetPRVoucher', obj)
      .toPromise();
    this.VoucherList = data;
    setTimeout(() => {
      this.searchGrid({ code: this.NGAccountHome });
    }, 100);
  }

  onClickCashpayment(event: any): void {
    if (this.isNewClick) {
      event.preventDefault();
      return;
    }

    this.bankPayment = false;
    this.vchType = 'CP';
  }

  onClickCpFreight(event: any): void {
    if (this.isNewClick) {
      event.preventDefault();
      return;
    }

    this.bankPayment = null;
    this.vchType = 'CP-FREIGHT';
  }

  onClickBankpayment(event: any): void {
    if (this.isNewClick) {
      event.preventDefault();
      return;
    }

    this.bankPayment = true;
    this.vchType = 'BP';
  }

  async getBankCash() {
    const data = await this.apiService
      .getDataById('accounts/GetBankCash', {
        vchType: this.vchType.substring(0, 2),
      })
      .toPromise();
    this.BackCash = data;
    this.paymentForm.get('bankCash')?.patchValue(data[0].code);
  }

  getMaxNumber() {
    const obj = {
      vchType: this.vchType,
    };

    this.apiService
      .getDataById('accounts/GetMaxNumber', obj)
      .subscribe((data) => {
        this.vchNo = data[0].VCHNO;
      });
  }

  async onChangeAccount(event: any) {
    if (this.tag != 'Supplier') {
      return;
    }

    if (event == undefined) {
      this.invoiceList = [];
      return;
    }

    const data = await this.apiService
      .getDataById('Purchase/InvoiceList', { code: event.code })
      .toPromise();
    this.invoiceList = data;
    this.calculation();
    this.callOldBalance(event.code);
  }

  async callOldBalance(code: any) {
    const oldBal = await this.apiService
      .getDataById('Purchase/CallOldBalance', { code: code })
      .toPromise();
    if (this.isNew) {
      this.previousBalance = this.com.roundVal(
        (oldBal + this.totalDueAmount).toFixed(2)
      );
    } else {
      this.previousBalance = this.com.roundVal(
        oldBal < 0 ? 0 : oldBal.toFixed(2)
      );
    }
  }

  onchangeBankCash(event: any) {
    this.bankcashtName = event.name;
  }

  onClickNew() {
    let result = this.com.isStopEntry(this.vchType);
    if (!result) {
      this.toastr.info('You are not allowed');
      return;
    }

    this.isDisabled = false;
    this.isNewClick = true;
    this.isNew = true;
    if (this.vchType == 'CP') {
      this.paymentForm.get('mainDes')?.patchValue('Cash Payment');
      this.paymentForm.get('description')?.patchValue('Cash Payment');
    } else if (this.vchType == 'BP') {
      this.paymentForm.get('mainDes')?.patchValue('Bank Payment');
      this.paymentForm.get('description')?.patchValue('Bank Payment');
    }

    this.getMaxNumber();
    this.getBankCash();
  }

  onClickRefresh() {
    this.isDisabled = true;
    this.isNewClick = false;
    this.vchNo = '';
    this.vchDate = this.isDayClose == true ? this.dayCloseDate : this.vchDate;
    this.BackCash = [];
    this.addData = [];
    this.resetForm();
    this.paymentForm.get('sno')?.patchValue(0);
    this.paymentForm.get('bankCash')?.patchValue(null);
    this.paymentForm.get('accountCode')?.patchValue(null);
    this.paymentForm.get('mainDes')?.patchValue('');
    this.totalAmount = 0;
    this.totalAdjustAmount = 0;
    this.tax1 = 0;
    this.taxAmt1 = 0;
    this.taxList1 = 0;
    this.tax2 = 0;
    this.taxAmt2 = 0;
    this.taxList2 = 0;
    this.totalTax = 0;
    this.netAmount = 0;
    this.previousBalance = 0;
    this.isAutoAdj = true;
    this.invoiceList = [];
    this.totalDueAmount = 0;
    this.totalRecAmount = 0;
    this.totalPenddingAmount = 0;
    this.isPrint = false;
    this.file = null;
    $('#my-file').val('');
    this.calculation();
  }

  oninputDes(event: any) {
    this.paymentForm.get('mainDes')?.patchValue(event.target.value);
  }

  appendData(): void {
    let form = this.paymentForm.value;

    if (
      form.bankCash == form.accountCode &&
      form.bankCash != '' &&
      form.bankCash != null
    ) {
      this.toastr.warning('Accounts Selection Wrong....!');
      return;
    }

    if (form.bankCash == '' || form.bankCash == null) {
      this.toastr.warning('Select Bank/Cash....!');
      return;
    }

    if (form.mainDes == '' || form.mainDes == null) {
      this.toastr.warning('Enter Main Description....!');
      return;
    }

    if (form.accountCode == '' || form.accountCode == null) {
      this.toastr.warning('Select Account Head....!');
      return;
    }

    if (form.description == '' || form.description == null) {
      this.toastr.warning('Enter Descrption....!');
      return;
    }

    if (form.amount == '' || form.amount == null) {
      this.toastr.warning('Enter Amount....!');
      return;
    }

    // if (this.vchType == 'BP') {
    //   if (form.chqNo == '' || form.chqNo == null) {
    //     this.toastr.warning('Enter Cheque Number....!');
    //     return;
    //   }
    // }

    if (this.tag != 'Account') {
      const added = this.addData.filter(
        (code) => code.accountCode == form.accountCode && code.sno != form.sno
      );
      if (added.length > 0) {
        this.toastr.info('This Account Already Added....!');
        return;
      }
    }

    form.accountName = this.AccountHead.find(
      (x) => x.code == form.accountCode
    ).name;

    form.chqDate = this.dp.transform(form.chqDate, 'dd/MM/yyyy');
    if (this.isRowEdit) {
      const index = this.addData.findIndex((row) => row.sno === form.sno);
      if (index !== -1) {
        this.addData[index] = form;
        this.isRowEdit = false;
        this.resetForm();
        this.calculation();

        if (this.isAutoAdj) {
          this.autoAdjust();
        }

        this.onTax({ target: { value: this.tax1 } });
        this.onTax2({ target: { value: this.tax2 } });
        if (this.tag != 'Account') {
          this.paymentForm.get('accountCode').disable();
        }
        return;
      }
    }

    form.sno = this.addData.length + 1;
    this.addData.push(form);
    this.resetForm();
    this.calculation();
    this.onTax({ target: { value: this.tax1 } });
    this.onTax2({ target: { value: this.tax2 } });

    if (this.tag != 'Account') {
      if (this.addData.length > 0) {
        this.paymentForm.get('accountCode').disable();
      } else {
        this.paymentForm.get('accountCode').enable();
      }
    }

    //this.calculation();
  }

  resetForm() {
    this.paymentForm.get('sno')?.patchValue(0);
    if (this.tag == 'Account') {
      this.paymentForm.get('accountCode')?.patchValue(null);
    }

    this.paymentForm.get('amount')?.patchValue('');
    this.paymentForm.get('description')?.patchValue('');
    this.paymentForm.get('chqNo')?.patchValue('');
    this.paymentForm.get('chqDate')?.patchValue(new Date());
    this.paymentForm.get('accountCode').enable();
    this.isRowEdit = false;
  }

  editRow(row: any) {
    this.paymentForm.get('sno')?.patchValue(row.sno);
    this.paymentForm.get('accountCode')?.patchValue(row.accountCode);
    this.paymentForm.get('amount')?.patchValue(row.amount);
    this.paymentForm.get('description')?.patchValue(row.description);
    this.paymentForm.get('chqNo')?.patchValue(row.chqNo);
    const splitDate = row.chqDate.split('/');
    const chqDate = new Date(splitDate[2], splitDate[1] - 1, splitDate[0]);
    this.paymentForm.get('chqDate')?.patchValue(chqDate);
    this.isRowEdit = true;
    this.paymentForm.get('accountCode').enable();
  }

  removeRow(row: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (!confirmDelete) {
      return;
    }

    const indexToRemove = this.addData.findIndex(
      (item) => item.accountCode === row.accountCode
    );
    if (indexToRemove !== -1) {
      this.addData.splice(indexToRemove, 1);
    }

    this.calculation();
    this.onTax({ target: { value: this.tax1 } });
    this.onTax2({ target: { value: this.tax2 } });

    if (this.tag != 'Account') {
      if (this.addData.length > 0) {
        this.paymentForm.get('accountCode').disable();
      } else {
        this.paymentForm.get('accountCode').enable();
      }
    }
  }

  onChangeTax(event: any) {
    this.tax1 = event.target.value;
    this.onTax({ target: { value: this.tax1 } });
  }

  onChangeTax2(event: any) {
    this.tax2 = event.target.value;
    this.onTax2({ target: { value: this.tax2 } });
  }

  onTax(event: any) {
    if (event.target.value > 100) {
      event.target.value = 100;
    }

    this.taxAmt1 = this.com.roundVal(
      ((this.totalAmount / 100) * event.target.value).toFixed(2)
    );
    this.calculation();
  }

  onTaxAmt(event: any) {
    if (parseFloat(event.target.value) > this.totalAmount) {
      event.target.value = this.totalAmount;
    }

    this.tax1 = this.com.roundVal(
      ((event.target.value / this.totalAmount) * 100).toFixed(2)
    );
    this.calculation();
  }

  onTax2(event: any) {
    if (event.target.value > 100) {
      event.target.value = 100;
    }

    this.taxAmt2 = this.com.roundVal(
      ((this.totalAmount / 100) * event.target.value).toFixed(2)
    );
    this.calculation();
  }
  onTax2Amt(event: any) {
    if (parseFloat(event.target.value) > this.totalAmount) {
      event.target.value = this.totalAmount;
    }

    this.tax2 = this.com.roundVal(
      ((event.target.value / this.totalAmount) * 100).toFixed(2)
    );
    this.calculation();
  }

  onClickAutoAdjust(event: any) {
    if (event.target.checked) {
      this.isAutoAdj = true;
    } else {
      this.isAutoAdj = false;
    }
    this.calculation();
  }

  onClickPreBal(event: any) {
    if (event.target.checked) {
      this.isPreviousBal = true;
    } else {
      this.isPreviousBal = false;
    }
    this.calculation();
  }

  autoAdjust() {
    let totalAmount = parseFloat(this.totalAdjustAmount);

    this.invoiceList.forEach((i, index) => {
      if (totalAmount > 0) {
        if (parseFloat(i.NDUEAMOUNT) < totalAmount) {
          i.RECAMOUNT = this.com.roundVal(i.NDUEAMOUNT.toFixed(2));
          totalAmount = totalAmount - parseFloat(i.NDUEAMOUNT);
        } else {
          i.RECAMOUNT = this.com.roundVal(totalAmount.toFixed(2));
          totalAmount = totalAmount - parseFloat(i.NDUEAMOUNT);
        }
      } else {
        i.RECAMOUNT = 0;
      }

      if (index !== -1) {
        this.invoiceList[index] = i;
      }
    });
  }

  oninputRec(i: any) {
    if (i.RECAMOUNT <= 0) {
      i.RECAMOUNT = 0;
    }

    if (i.RECAMOUNT > i.NDUEAMOUNT) {
      i.RECAMOUNT = this.com.roundVal(parseFloat(i.NDUEAMOUNT).toFixed(2));
    }

    this.invoiceCalculation();

    let netAmt = this.netAmount;
    let totalRecAmt = this.totalRecAmount;

    if (parseFloat(netAmt) < totalRecAmt) {
      let finalValue = totalRecAmt - netAmt;
      i.RECAMOUNT = this.com.roundVal(
        Math.abs(finalValue - i.RECAMOUNT).toFixed(2)
      );
      this.invoiceCalculation();
    }
  }

  invoiceCalculation() {
    this.totalDueAmount = 0;
    this.totalRecAmount = 0;
    this.totalPenddingAmount = 0;

    this.invoiceList.forEach((i, index) => {
      this.totalDueAmount += i.NDUEAMOUNT;
      this.totalRecAmount += parseFloat(i.RECAMOUNT);
      this.totalPenddingAmount += i.NDUEAMOUNT - i.RECAMOUNT;
      i.PENDDING = this.com.roundVal((i.NDUEAMOUNT - i.RECAMOUNT).toFixed(2));

      if (index !== -1) {
        this.invoiceList[index] = i;
      }
    });
  }

  calculation() {
    let amount: any = 0;

    this.addData.forEach((item) => {
      amount += item.amount;
    });

    this.totalAmount = this.com.roundVal(amount.toFixed(2));
    this.totalTax = parseFloat(this.taxAmt1) + parseFloat(this.taxAmt2);
    this.netAmount = this.com.roundVal((amount - this.totalTax).toFixed(2));

    if (this.isAutoAdj) {
      let myAmt = 0;

      if (this.isPreviousBal) {
        myAmt = parseFloat(amount) + parseFloat(this.previousBalance);
      } else {
        myAmt = parseFloat(amount);
      }

      this.totalAdjustAmount = myAmt > 0 ? myAmt : 0;
    } else {
      this.totalAdjustAmount = 0;
    }

    if (!this.isCalculation) {
      this.invoiceCalculation();
      return;
    }

    this.autoAdjust();
    this.invoiceCalculation();
  }

  async onClickDicount(item: any) {
    let data = await this.apiService
      .getDataById('Purchase/GetDisData', {
        vchType: 'PI',
        vchNo: item.INVOICENUMBER,
      })
      .toPromise();
    data = data[0];

    this.invoiceNo = item.INVOICENUMBER;
    this.grossAmount = this.com.roundVal(data.GROSSAMOUNT.toFixed(2));
    this.discount = this.com.roundVal(data.DISCOUNT.toFixed(2));
    this.disAmount = this.com.roundVal(data.DISCOUNTAMT.toFixed(2));
    this.otherCredit = this.com.roundVal(data.OTHERCREDIT.toFixed(2));
    this.creditRemarks = data.REMARKS;
    this.shipment = this.com.roundVal(data.SHIPMENT.toFixed(2));
    this.netDue = this.com.roundVal(data.NETDUE.toFixed(2));
  }

  onInputDiscount(event: any) {
    if (event.target.value < 0 || event.target.value == '') {
      event.target.value = 0;
    }

    if (event.target.value > 100) {
      event.target.value = 100;
    }

    this.disAmount = this.com.roundVal(
      ((this.grossAmount * event.target.value) / 100).toFixed(2)
    );
    this.disCalculate();
  }

  onInputDiscountAmt(event: any) {
    if (event.target.value < 0 || event.target.value == '') {
      event.target.value = 0;
    }

    if (parseFloat(event.target.value) > parseFloat(this.grossAmount)) {
      event.target.value = this.grossAmount;
    }

    this.discount = (event.target.value / this.grossAmount) * 100;
    this.disCalculate();
  }

  disCalculate() {
    this.netDue = this.com.roundVal(
      (
        parseFloat(this.grossAmount) +
        parseFloat(this.shipment) -
        (parseFloat(this.disAmount) + parseFloat(this.otherCredit))
      ).toFixed(2)
    );
  }

  onClickSaveDis() {
    const obj = {
      vchType: 'PI',
      vchNo: this.invoiceNo,
      netDue: this.netDue,
      discount: this.discount,
      disAmount: this.disAmount,
      otherCredit: this.otherCredit,
      remarks: this.creditRemarks,
      partyName: this.AccountHead.find(
        (x) => x.code === this.paymentForm.get('accountCode').value
      ).name,
      dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
    };

    if (this.netDue <= 0) {
      return;
    }

    this.apiService
      .saveObj('Purchase/SaveDiscount', obj)
      .subscribe((result) => {
        if (result == true || result == 'true') {
          if (this.isNew) {
            this.onChangeAccount({
              code: this.paymentForm.get('accountCode').value,
            });
          } else {
            const obj = {
              vchNo: this.vchNo,
              vchType: this.vchType,
              partyCode: this.paymentForm.get('accountCode').value,
              status: 'payment',
            };

            this.editPartyData(obj);
          }
          this.toastr.success('Save Successfully');
        } else {
          this.toastr.error('Please Save Again');
        }
      });
  }

  onClickSave(): void {
    let result = this.com.isStopEntry(this.vchType);
    if (!result) {
      this.toastr.info('You are not allowed');
      return;
    }

    if (this.addData.length == 0) {
      this.toastr.warning('First Add Voucher...!');
      return;
    }

    try {
      this.com.showLoader();

      this.lastVchAccount = this.paymentForm.get('bankCash').value;
      const job = this.JobList.find(
        (x) => x.ID == this.paymentForm.get('jobNo').value
      );

      const payment: any[] = this.addData.map((item) => ({
        vchNo: this.vchNo,
        vchType: this.vchType,
        vchDate: this.dp.transform(this.vchDate, 'yyyy-MM-dd'),
        bankCashName: this.BackCash.find(
          (x) => x.code == this.paymentForm.get('bankCash').value
        ).name,
        bankCash: this.paymentForm.get('bankCash').value,
        mainDesc: this.paymentForm.get('mainDes').value,
        jobNo: this.paymentForm.get('jobNo').value ?? 0,
        jobName: job != undefined ? job.NAME : '',
        jobLocId: job != undefined ? job.LOCID : '',
        accountHead: item.accountCode,
        accountHeadName: item.accountName,
        description: item.description,
        chequeNo: item.chqNo,
        chequeDate: this.dp.transform(
          new Date(
            item.chqDate.split('/')[2],
            item.chqDate.split('/')[1] - 1,
            item.chqDate.split('/')[0]
          ),
          'yyyy-MM-dd'
        ),
        amount: item.amount,
        totalAmount: this.totalAmount,
        tax1Name: this.tax1Name,
        tax1: this.tax1,
        tax1Amount: this.taxAmt1,
        tax2Name: this.tax2Name,
        tax2: this.tax2,
        tax2Amount: this.taxAmt2,
        TotalTaxAmount: this.totalTax,
        netAmount: this.netAmount,
        status: this.isNew == true ? 'new' : 'edit',
        Module: this.tag,
        dtNow: new Date(),
      }));

      const mylist = this.invoiceList.filter(
        (x: any) => x.RECAMOUNT != 0 && x.RECAMOUNT != null
      );

      const invoice: any[] = mylist.map((item) => ({
        InvoiceNo: item.INVOICENUMBER,
        RecAmount: item.RECAMOUNT,
        InvoiceType: item.VCHTYPE,
        InvoiceDate: this.dp.transform(
          new Date(
            item.INVOICEDATE.split('/')[2],
            item.INVOICEDATE.split('/')[1] - 1,
            item.INVOICEDATE.split('/')[0]
          ),
          'yyyy-MM-dd'
        ),
      }));

      this.apiService
        .saveData('Purchase/SaveUpdatePR', { payment, invoice })
        .subscribe(async (r) => {
          if (r.status == true || r.status == 'true') {
            if (this.file) {
              this.row = {
                VCHNO: r.vchNo,
                VCHTYPE: this.vchType,
              };
              await this.onClickUpload(false);
            }
            this.toastr.success('Save Successfully');
            this.vchNo = r.vchNo;
            this.isPrint = true;
            this.isNew = false;
            //this.onClickRefresh();
            this.getInvoiceList();
            this.com.hideLoader();
          } else {
            this.com.hideLoader();
            this.toastr.error('Please Save Again');
          }
        });
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  async editByVchNo(e: any) {
    clearTimeout(this.ntimer);
    if (e.target.value) {
      this.ntimer = setTimeout(async () => {
        this.addData = [];
        await this.editAccountpayment(e.target.value, this.vchType, false);
      }, this.ntimeout);
    }
  }

  async editAccountpayment(vchNo: any, vchType: any, isToggle: boolean) {
    let result = this.com.isStopEntry(vchType);
    if (!result) {
      this.toastr.info('You are not allowed');
      return;
    }

    this.isDisabled = false;
    this.isNewClick = true;
    this.isNew = false;
    this.vchType = vchType;
    this.vchNo = vchNo;

    if (vchType == 'BP') {
      this.bankPayment = true;
    } else if (vchType == 'CP') {
      this.bankPayment = false;
    } else {
      this.bankPayment = null;
    }

    this.getBankCash();

    try {
      this.com.showLoader();

      const obj = {
        vchNo: vchNo,
        vchType: vchType,
        status: 'payment',
        tag: this.tag,
      };

      const data = await this.apiService
        .getDataById('Purchase/EditPR', obj)
        .toPromise();

      if (data.length == 0) {
        this.toastr.info('Voucher Not Found');
        this.onClickRefresh();
        this.com.hideLoader();
        return;
      }

      data.forEach((item: any) => {
        this.paymentForm.get('bankCash')?.patchValue(item.BANKCASHCODE);
        this.paymentForm.get('accountCode')?.patchValue(item.PARTYCODE);
        this.paymentForm.get('jobNo')?.patchValue(item.JOBID);
        this.bankcashtName = item.BANKCASH;
        this.paymentForm.get('mainDes')?.patchValue(item.MAINDESCRIPTION);
        let form = this.paymentForm.value;
        form.sno = this.addData.length + 1;
        form.accountName = item.NAMES;
        form.accountCode = item.PARTYCODE;
        form.amount = item.AMOUNT;
        form.description = item.DESCRIPTION;
        form.chqNo = item.CHQNO;
        form.chqDate = item.CHQDATE;
        this.addData.push(form);
        this.resetForm();
        (this.vchDate = new Date(
          item.VCHDATE.split('/')[2],
          item.VCHDATE.split('/')[1] - 1,
          item.VCHDATE.split('/')[0]
        )),
          (this.tax1 = item.TAX1);
        this.tax2 = item.TAX2;
      });

      this.calculation();
      this.onTax({ target: { value: this.tax1 } });
      this.onTax2({ target: { value: this.tax2 } });
      this.taxList1 = this.tax1;
      this.taxList2 = this.tax2;
      if (this.tag != 'Account') {
        this.paymentForm.get('accountCode').disable();
      }
      this.isPreviousBal = true;
      this.isAutoAdj = true;

      if (this.tag == 'Supplier') {
        const obj1 = {
          vchNo: vchNo,
          vchType: vchType,
          partyCode: data[0].PARTYCODE,
          status: 'payment',
          //tag: this.tag,
        };
        this.editPartyData(obj1);
      }

      if (isToggle) {
        this.togglePages();
      }

      this.isPrint = true;
      this.com.hideLoader();
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  async editPartyData(obj: any) {
    const inv = await this.apiService
      .getDataById('Purchase/EditPartyPR', obj)
      .toPromise();
    this.isCalculation = false;
    this.invoiceList = inv;
    this.calculation();

    this.callOldBalance(obj.partyCode);
    this.calculation();

    this.isCalculation = true;
  }

  deleteAccountpayment(vchNo: any, vchType: any): void {
    let result = this.com.isStopEntry(vchType);
    if (!result) {
      this.toastr.info('You are not allowed');
      return;
    }

    const confirmDelete = confirm('Are you sure you want to delete this item?');
    if (!confirmDelete) {
      return;
    }
    try {
      this.com.showLoader();
      const obj = {
        vchNo: vchNo,
        vchType: vchType,
        dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      };

      this.apiService.deleteData('accounts/DeletePR', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.toastr.success('Delete Successfully');
            this.getInvoiceList();
            this.com.hideLoader();
          } else if (data == 'false' || data == false) {
            this.com.hideLoader();
            this.toastr.error('Delete Again');
          }
        },
        error: (error) => {
          this.com.hideLoader();
          this.toastr.info(error.error.text);
        },
      });
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  searchGrid(event: any): void {
    const tableElement = this.voucherList.nativeElement;
    const rows = tableElement.querySelectorAll('tr');

    if (typeof event === 'undefined') {
      rows.forEach((row: HTMLTableRowElement) => {
        row.style.display = '';
      });
      return;
    }

    rows.forEach((row: HTMLTableRowElement) => {
      let isShow = true;

      if (
        row.querySelector('.partyCode')?.textContent != event.code &&
        event.code.length > 0
      ) {
        isShow = false;
      }

      if (isShow) {
        row.style.display = '';
      } else {
        row.style.display = 'none';
      }
    });
  }

  // TAX 1 CODE

  taxList: any[] = [];
  tax = '';
  isDisabledTax: boolean = true;
  isShowTax: boolean = false;

  refreshTax() {
    this.tax = '';
    this.isDisabledTax = true;
    this.isShowTax = false;
  }

  newTax() {
    this.refreshTax();
    this.isDisabledTax = false;
    this.isShowTax = true;
  }

  getTax() {
    this.apiService
      .getDataById('Accounts/GetTax', { tag: 'tax1' })
      .subscribe((data) => {
        this.taxList = data;
      });
  }

  createUpdateTax() {
    const obj = {
      tax: this.tax,
      tag: 'tax1',
    };

    if (this.tax == '' || this.tax == null) {
      this.toastr.warning('Enter Tax....!');
      return;
    }

    this.apiService
      .saveObj('Accounts/AddUpdateTax', obj)
      .subscribe((result) => {
        if (result == true || result == 'true') {
          this.toastr.success('Save Successfully');
          this.getTax();
          this.refreshTax();
        } else {
          this.toastr.error('Please Save Again');
        }
      });
  }

  editTax(tax: any): void {
    this.tax = tax;
    this.isDisabledTax = false;
    this.isShowTax = true;
  }

  deleteTax(tax: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {
      const obj = {
        tax: tax,
        tag: 'tax1',
      };

      this.apiService.deleteData('Accounts/DeleteTax', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.toastr.success('Delete Successfully');
            this.getTax();
            this.refreshTax();
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

  // TAX 2 CODE

  tax2List: any[] = [];
  tax2s = '';
  isDisabledTax2: boolean = true;
  isShowTax2: boolean = false;

  refreshTax2() {
    this.tax2s = '';
    this.isDisabledTax2 = true;
    this.isShowTax2 = false;
  }

  newTax2() {
    this.refreshTax2();
    this.isDisabledTax2 = false;
    this.isShowTax2 = true;
  }

  getTax2() {
    this.apiService
      .getDataById('Accounts/GetTax', { tag: 'tax2' })
      .subscribe((data) => {
        this.tax2List = data;
      });
  }

  createUpdateTax2() {
    const obj = {
      tax: this.tax2,
      tag: 'tax2',
    };

    if (this.tax2s == '' || this.tax2s == null) {
      this.toastr.warning('Enter Tax....!');
      return;
    }

    this.apiService
      .saveObj('Accounts/AddUpdateTax', obj)
      .subscribe((result) => {
        if (result == true || result == 'true') {
          this.toastr.success('Save Successfully');
          this.getTax2();
          this.refreshTax2();
        } else {
          this.toastr.error('Please Save Again');
        }
      });
  }

  editTax2(tax: any): void {
    this.tax2s = tax;
    this.isDisabledTax2 = false;
    this.isShowTax2 = true;
  }

  deleteTax2(tax: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {
      const obj = {
        tax: tax,
        tag: 'tax2',
      };

      this.apiService.deleteData('Accounts/DeleteTax', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.toastr.success('Delete Successfully');
            this.getTax2();
            this.refreshTax2();
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

  export(fileName: any, formate: any) {
    this.com.ExportFiles(fileName, formate);
  }

  search(event: any): void {
    const tableElement = this.voucherList.nativeElement;
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
}
