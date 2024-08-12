import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ApiService } from 'src/app/services/api.service';
import { DatePipe } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/services/auth.service';
import { environment } from '../../../../environment/environmemt';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-sales-payment',
  templateUrl: './sales-payment.component.html',
  styleUrls: ['./sales-payment.component.css'],
})
export class SalesPaymentComponent {
  isShowPage: boolean = true;
  isDisabled: boolean = true;
  isDayClose: any = false;
  dayCloseDate: Date = new Date();

  basePath = environment.basePath;
  lastVchAccount: any = null;
  costCenter = localStorage.getItem('costCenter')
  // DATE
  fromDate: Date;
  toDate: Date;
  vchDate: Date;
  chqDate: Date;
  AccountHead: any[] = [];
  BackCash: any[] = [];
  JobList: any[] = [];
  NGAccountHome: any;
  accountName: any;
  bankcashtName: any;
  VoucherList: any[] = [];
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
  taxList1: any = 0;
  tax2: number = 0;
  tax2Name: string = 'Tax 2';
  taxAmt2: any = 0;
  taxList2: any = 0;
  totalTax: any = 0;
  netAmount: any = 0;
  isPrint: boolean = false;
  srcPdfImg: any;
  file: File | null = null;
  row: any;
  fileList: any;
  isPdf: boolean = false;

  @ViewChild('voucherList', { static: false }) voucherList!: ElementRef;

  constructor(
    private apiService: ApiService,
    private toast: ToastrService,
    private dp: DatePipe,
    private fb: FormBuilder,
    private auth: AuthService,
    private com: CommonService
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
      bankCash: [undefined],
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

  onClickUpload() {
    let item = this.row;

    let formData = new FormData();
    formData.append('vchNo', item.VCHNO.toString());
    formData.append('vchType', item.VCHTYPE);
    formData.append('file', this.file);

    this.apiService
      .saveData('Accounts/FileUpload', formData)
      .subscribe((result) => {
        if (result == true || result == 'true') {
          this.toast.success('Save Successfully');
          this.getFile(item);
          this.onRefreshModel();
        } else {
          this.toast.error('Please Save Again');
        }
      });
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
        this.onViewFile(result[0])
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
          this.toast.success('Delete Successfully');
        } else {
          this.toast.error('Please Delete Again');
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
        .getData('Utilities/LastCloseDate')
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
      .getDataById('Accounts/GetAccountsList', { module: 'customer' })
      .subscribe((data) => {
        this.AccountHead = data;
      });
  }

  async getInvoiceList() {
    const obj = {
      fromDate: this.dp.transform(this.fromDate, 'yyyy/MM/dd'),
      toDate: this.dp.transform(this.toDate, 'yyyy/MM/dd'),
      tag: 'Payment',
      module: 'Customer',
    };

    const data = await this.apiService
      .getDataById('Sale/GetPRVoucher', obj)
      .toPromise();
    this.VoucherList = data;
    setTimeout(() => {
      this.searchGrid({ code: this.NGAccountHome });
    }, 100);
  }

  onClickCashPayment(event: any): void {
    if (this.isNewClick) {
      event.preventDefault();
      return;
    }

    this.bankPayment = false;
    this.vchType = 'CP';
  }

  onClickBankPayment(event: any): void {
    if (this.isNewClick) {
      event.preventDefault();
      return;
    }

    this.bankPayment = true;
    this.vchType = 'BP';
  }

  async getBankCash() {
    const data = await this.apiService
      .getDataById('accounts/GetBankCash', { vchType: this.vchType })
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

  onChangeAccount(event: any) {
    this.accountName = event.name;
  }

  onchangeBankCash(event: any) {
    this.bankcashtName = event.name;
  }

  onClickNew() {
    this.isDisabled = false;
    this.isNewClick = true;

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
    this.vchNo = 0;
    this.vchDate = this.dayCloseDate;
    this.BackCash = [];
    this.addData = [];
    this.resetForm();
    this.paymentForm.get('sno')?.patchValue(0);
    this.paymentForm.get('bankCash')?.patchValue(undefined);
    this.paymentForm.get('mainDes')?.patchValue('');
    this.totalAmount = 0;
    this.tax1 = 0;
    this.taxAmt1 = 0;
    this.taxList1 = 0;
    this.tax2 = 0;
    this.taxAmt2 = 0;
    this.taxList2 = 0;
    this.totalTax = 0;
    this.netAmount = 0;
    this.isPrint = false;
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
      this.toast.warning('Accounts Selection Wrong....!');
      return;
    }

    if (form.bankCash == '' || form.bankCash == null) {
      this.toast.warning('Select Bank/Cash....!');
      return;
    }

    if (form.mainDes == '' || form.mainDes == null) {
      this.toast.warning('Enter Main Description....!');
      return;
    }

    if (form.accountCode == '' || form.accountCode == null) {
      this.toast.warning('Select Account Head....!');
      return;
    }

    if (form.description == '' || form.description == null) {
      this.toast.warning('Enter Descrption....!');
      return;
    }

    if (form.amount == '' || form.amount == null) {
      this.toast.warning('Enter Amount....!');
      return;
    }

    // if (this.vchType == 'BP') {
    //   if (form.chqNo == '' || form.chqNo == null) {
    //     this.toast.warning('Enter Cheque Number....!');
    //     return;
    //   }
    // }

    const added = this.addData.filter(
      (code) => code.accountCode == form.accountCode && code.sno != form.sno
    );
    if (added.length > 0) {
      this.toast.info('This Account Already Added....!');
      return;
    }

    form.accountName = this.AccountHead.find((x) => x.code == form.accountCode).name;
    const jobName = this.JobList.find((x) => x.ID == form.jobNo)
    form.jobName = (jobName != undefined) ? jobName.NAME : '';

    form.chqDate = this.dp.transform(form.chqDate, 'dd/MM/yyyy');
    if (this.isRowEdit) {
      const index = this.addData.findIndex((row) => row.sno === form.sno);
      if (index !== -1) {
        this.addData[index] = form;
        this.isRowEdit = false;
        this.resetForm();
        this.calculation();
        this.onTax({ target: { value: this.tax1 } });
        this.onTax2({ target: { value: this.tax2 } });
        this.paymentForm.get('accountCode').disable();
        return;
      }
    }

    form.sno = this.addData.length + 1;
    this.addData.push(form);
    this.resetForm();
    this.calculation();
    this.onTax({ target: { value: this.tax1 } });
    this.onTax2({ target: { value: this.tax2 } });

    if (this.addData.length > 0) {
      this.paymentForm.get('accountCode').disable();
    } else {
      this.paymentForm.get('accountCode').enable();
    }
  }

  resetForm() {
    this.paymentForm.get('sno')?.patchValue(0);
    this.paymentForm.get('accountCode')?.patchValue(undefined);
    this.paymentForm.get('amount')?.patchValue('');
    this.paymentForm.get('description')?.patchValue('');
    this.paymentForm.get('chqNo')?.patchValue('');
    this.paymentForm.get('chqDate')?.patchValue(new Date());
    this.isRowEdit = false;
    this.paymentForm.get('accountCode').enable();
  }

  editRow(row: any) {
    this.paymentForm.get('sno')?.patchValue(row.sno);
    this.paymentForm.get('accountCode')?.patchValue(row.accountCode);
    this.paymentForm.get('amount')?.patchValue(row.amount);
    this.paymentForm.get('description')?.patchValue(row.description);
    this.paymentForm.get('chqNo')?.patchValue(row.chqNo);
    this.paymentForm.get('jobNo')?.patchValue(row.jobNo);
    const splitDate = row.chqDate.split('/');
    const chqDate = new Date(splitDate[2], splitDate[1] - 1, splitDate[0]);
    this.paymentForm.get('chqDate')?.patchValue(chqDate);
    this.accountName = row.accountName;
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

    if (this.addData.length > 0) {
      this.paymentForm.get('accountCode').disable();
    } else {
      this.paymentForm.get('accountCode').enable();
    }
  }

  onChangeTax(event: any) {
    this.tax1 = event.target.value;
    this.onTax({ target: { value: this.tax1 } });
    this.onTax2({ target: { value: this.tax2 } });
  }

  onChangeTax2(event: any) {
    this.tax2 = event.target.value;
    this.onTax2({ target: { value: this.tax2 } });
  }

  onTax(event: any) {
    if (event.target.value > 100) {
      event.target.value = 100;
    }

    this.taxAmt1 = this.com.roundVal(((this.totalAmount / 100) * event.target.value).toFixed(2));
    this.calculation();
  }

  onTaxAmt(event: any) {
    if (parseFloat(event.target.value) > this.totalAmount) {
      event.target.value = this.totalAmount;
    }

    this.tax1 = this.com.roundVal(((event.target.value / this.totalAmount) * 100).toFixed(2));
    this.calculation();
  }

  onTax2(event: any) {
    if (event.target.value > 100) {
      event.target.value = 100;
    }

    this.taxAmt2 = this.com.roundVal(((this.totalAmount / 100) * event.target.value).toFixed(2));
    this.calculation();
  }

  onTax2Amt(event: any) {
    if (parseFloat(event.target.value) > this.totalAmount) {
      event.target.value = this.totalAmount;
    }

    this.tax2 = this.com.roundVal(((event.target.value / this.totalAmount) * 100).toFixed(2));
    this.calculation();
  }

  calculation() {
    let total: any = 0;
    this.addData.forEach((item) => {
      total += item.amount;
    });

    this.totalAmount = this.com.roundVal(total.toFixed(2));
    this.totalTax = parseFloat(this.taxAmt1) + parseFloat(this.taxAmt2);
    this.netAmount = this.com.roundVal((total - this.totalTax).toFixed(2));
  }

  onClickSave(): void {
    if (this.addData.length == 0) {
      this.toast.warning('First Add Voucher...!');
      return;
    }

    try {
      this.com.showLoader();

      this.lastVchAccount = this.paymentForm.get('bankCash').value;

      const payment: any[] = this.addData.map((item) => ({
        vchNo: this.vchNo,
        vchType: this.vchType,
        vchDate: this.dp.transform(this.vchDate, 'yyyy-MM-dd'),
        bankCashName: this.bankcashtName,
        bankCash: item.bankCash,
        mainDesc: item.mainDes,
        accountHead: item.accountCode,
        accountHeadName: item.accountName,
        jobNo: item.jobNo ?? 0,
        jobName: item.jobName,
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
        module: 'Customer',
        dtNow: new Date(),
      }));

      this.apiService
        .saveData('Sale/SaveUpdatePR', { payment })
        .subscribe((result) => {
          if (result == true || result == 'true') {
            this.toast.success('Save Successfully');
            this.isPrint = true;
            this.isNew = false;
            //this.onClickRefresh();
            this.getInvoiceList();
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

  editAccountPayment(vchNo: any, vchType: any): void {
    try {
      this.com.showLoader();
      
      this.isDisabled = false;
      this.isNewClick = true;
      this.isNew = false;
      this.vchType = vchType;
      this.vchNo = vchNo;

      if (vchType == 'BP') {
        this.bankPayment = true;
      }

      this.getBankCash();

      const obj = {
        vchNo: vchNo,
        vchType: vchType,
      };

      this.apiService.getDataById('Sale/EditPR', obj).subscribe((data) => {
        data.forEach((item: any) => {
          this.paymentForm.get('bankCash')?.patchValue(item.BANKCASHCODE);
          this.bankcashtName = item.BANKCASH;
          this.paymentForm.get('mainDes')?.patchValue(item.MAINDESCRIPTION);
          let form = this.paymentForm.value;
          form.sno = this.addData.length + 1;
          form.accountName = item.NAMES;
          form.accountCode = item.PARTYCODE;
          form.jobNo = item.JOBID;
          form.jobName = item.JOBNAME;
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
        this.paymentForm.get('accountCode').disable();
        this.togglePages();
        this.isPrint = true;
        this.com.hideLoader();
      });
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  deleteAccountPayment(vchNo: any, vchType: any): void {
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

      this.apiService.deleteData('Sale/DeletePR', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.toast.success('Delete Successfully');
            this.getInvoiceList();
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
      this.toast.warning('Enter Tax....!');
      return;
    }

    this.apiService
      .saveObj('Accounts/AddUpdateTax', obj)
      .subscribe((result) => {
        if (result == true || result == 'true') {
          this.toast.success('Save Successfully');
          this.getTax();
          this.refreshTax();
        } else {
          this.toast.error('Please Save Again');
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
            this.toast.success('Delete Successfully');
            this.getTax();
            this.refreshTax();
          } else if (data == 'false' || data == false) {
            this.toast.error('Delete Again');
          }
        },
        error: (error) => {
          this.toast.info(error.error.text);
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
      this.toast.warning('Enter Tax....!');
      return;
    }

    this.apiService
      .saveObj('Accounts/AddUpdateTax', obj)
      .subscribe((result) => {
        if (result == true || result == 'true') {
          this.toast.success('Save Successfully');
          this.getTax2();
          this.refreshTax2();
        } else {
          this.toast.error('Please Save Again');
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
            this.toast.success('Delete Successfully');
            this.getTax2();
            this.refreshTax2();
          } else if (data == 'false' || data == false) {
            this.toast.error('Delete Again');
          }
        },
        error: (error) => {
          this.toast.info(error.error.text);
        },
      });
    }
  }
  export(fileName: any, formate: any) {
    this.com.ExportFiles(fileName, formate);
  }
}
