import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-day-closing',
  templateUrl: './day-closing.component.html',
  styleUrls: ['./day-closing.component.css'],
})
export class DayClosingComponent {
  constructor(
    private tostr: ToastrService,
    private apiService: ApiService,
    private dp: DatePipe
  ) {}

  txtSearchCashSale: any;
  txtSearchCashPayment: any;
  txtSearchCreditCardSale: any;

  @ViewChild('cashSaleDetail', { static: false }) cashSaleDetail!: ElementRef;
  @ViewChild('cashPaymentDetail', { static: false })
  cashPaymentDetail!: ElementRef;
  @ViewChild('creditCardSaleDetail', { static: false })
  creditCardSaleDetail!: ElementRef;

  footCashSale: any;
  footCashPayment: any;
  footCreditCardSale: any;

  cashSaleList: [];
  cashPaymentList: [];
  cardSaleList: [];

  isParty: boolean = false;
  txtDayClose: boolean = true;
  removeDayClose: boolean = false;

  todayDate: Date;
  lastClosedDate: any;
  txtCashOpening: any = 0;
  txtCashSale: any = 0;
  txtCashPayment: any = 0;
  txtCreditCardSale: any = 0;
  txtTotalSale: any = 0;
  txtCashBalance: any = 0;
  txtTotalCash: any = 0;

  // Shift
  shiftList: any[] = [];
  shiftId: any;

  // Till
  tillList: any[] = [];
  tillId: any;

  // Cash Paid To
  cashPaidToList: any[] = [];
  cashPaidToId: any;

  ngOnInit() {
    this.todayDate = new Date();
    this.getFieldsData();
    this.getAccountsData();
    this.getParty();
    this.getAcc();
  }

  onDateChange() {
    const currentDate = new Date();
    const todayDate = this.todayDate;

    if (todayDate > currentDate) {
      this.todayDate = currentDate;
      this.tostr.warning('Date exceeds!');
    }

    this.getAccountsData();
    this.getAcc();
  }

  onRefresh() {
    this.getAccountsData();
    this.getAcc();
  }

  //======================= Fields Data =======================//

  getFieldsData() {
    this.apiService.getData('Sale/GetDayClosingCash').subscribe((data) => {
      this.shiftList = data.shift;
      this.tillList = data.till;
      if (data.date.closeDate != null) {
        this.lastClosedDate = data.date.closeDate;
        this.btnRemove();
      }
    });
  }

  getParty() {
    this.apiService.getData('Accounts/GetAccountsList').subscribe((data) => {
      this.cashPaidToList = data;
    });
  }

  //======================= Accounts Data =======================//

  getAccountsData() {
    var obj = {
      date: this.dp.transform(this.todayDate, 'yyyy/MM/dd'),
      shift: this.shiftId == undefined ? '' : this.shiftId,
      till: this.tillId == undefined ? '' : this.tillId,
    };

    this.apiService
      .getDataById('Sale/GetDayClosingAccounts', obj)
      .subscribe((data) => {
        this.txtCashOpening = data[0].CASHOP.toFixed(2);
        this.txtCashSale = data[0].CASHSALE.toFixed(2);
        this.txtCashPayment = data[0].CASHPAYMENT.toFixed(2);
        this.txtCreditCardSale = data[0].CREDITCARD.toFixed(2);
        this.calculations();
        this.hideShow();
      });
  }

  //======================= getAcc Data =======================//

  getAcc() {
    this.apiService
      .getDataById('Sale/GetDayClosingAcc', {
        date: this.dp.transform(this.todayDate, 'yyyy/MM/dd'),
      })
      .subscribe((data) => {
        this.cashSaleList = data.sale;
        this.cashPaymentList = data.payment;
        this.cardSaleList = data.creditCard;
      });
  }

  //======================= Save =======================//

  btnSave() {
    if (
      this.txtTotalSale == '0.00' ||
      this.txtTotalSale == '0' ||
      this.txtTotalSale == ''
    ) {
      this.tostr.warning('Make a sale first!');
      return;
    }

    if (this.cashPaidToId == undefined) {
      this.tostr.warning('Select Paid To....!');
      return;
    }

    var obj = {
      date: this.dp.transform(this.todayDate, 'yyyy/MM/dd'),
      paidTo: this.cashPaidToId,
      cash: this.txtTotalSale,
    };

    this.apiService.saveObj('Sale/SaveDayClosingAccounts', obj).subscribe(
      (result) => {
        if (result == true || result == 'true') {
          this.tostr.success('Save Successfully');
          this.getAccountsData();
        } else {
          this.tostr.error('Please Save Again');
        }
      },
      (error) => {
        this.tostr.error('On Err');
      }
    );
  }

  btnDayClose() {
    this.isParty = true;
  }

  hideShow() {
    if (this.removeDayClose == false) {
      const sale = this.txtTotalSale;
      if (sale == '0.00' || sale == '0' || sale == '') {
        this.txtDayClose = false;
        this.isParty = false;
      } else {
        this.txtDayClose = true;
      }
    }
  }

  btnRemove() {
    const lastDate = this.dp.transform(this.lastClosedDate, 'yyyy/MM/dd');
    const currentDate = this.dp.transform(this.todayDate, 'yyyy/MM/dd');
    if (lastDate == currentDate) {
      this.removeDayClose = true;
      this.txtDayClose = false;
      this.isParty = false;
    }
  }

  //======================= calculations =======================//

  calculations() {
    const openingValue = parseFloat(this.txtCashOpening);
    const saleValue = parseFloat(this.txtCashSale);
    const creditCardSaleValue = parseFloat(this.txtCreditCardSale);
    const cashPaymentValue = parseFloat(this.txtCashPayment);

    this.txtTotalCash = (openingValue + saleValue).toFixed(2);
    this.txtTotalSale = (creditCardSaleValue + saleValue).toFixed(2);
    this.txtCashBalance = (this.txtTotalCash - cashPaymentValue).toFixed(2);
  }

  //======================= search =======================//

  onSearchCashSale(event: any) {
    this.txtSearchCashSale = event.target.value;
    this.searchGridCashSale();
  }

  searchGridCashSale(): void {
    const tableElement = this.cashSaleDetail.nativeElement;
    const rows = tableElement.querySelectorAll('tr');

    rows.forEach((row: HTMLTableRowElement) => {
      let isShow = true;

      if (
        row.textContent &&
        row.textContent
          .toLowerCase()
          .indexOf(this.txtSearchCashSale.toLowerCase()) > -1
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

  onSearchCashPayment(event: any) {
    this.txtSearchCashPayment = event.target.value;
    this.searchGridCashPayment();
  }

  searchGridCashPayment(): void {
    const tableElement = this.cashPaymentDetail.nativeElement;
    const rows = tableElement.querySelectorAll('tr');

    rows.forEach((row: HTMLTableRowElement) => {
      let isShow = true;

      if (
        row.textContent &&
        row.textContent
          .toLowerCase()
          .indexOf(this.txtSearchCashPayment.toLowerCase()) > -1
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

  onSearchCardSale(event: any) {
    this.txtSearchCreditCardSale = event.target.value;
    this.searchGridCardSale();
  }

  searchGridCardSale(): void {
    const tableElement = this.creditCardSaleDetail.nativeElement;
    const rows = tableElement.querySelectorAll('tr');

    rows.forEach((row: HTMLTableRowElement) => {
      let isShow = true;

      if (
        row.textContent &&
        row.textContent
          .toLowerCase()
          .indexOf(this.txtSearchCreditCardSale.toLowerCase()) > -1
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
