import { Component, ElementRef, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { DatePipe } from '@angular/common';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-account-openings',
  templateUrl: './account-openings.component.html',
  styleUrls: ['./account-openings.component.css'],
})
export class AccountOpeningsComponent {
  debit: number = 0;
  credit: number = 0;
  acccount_list: any = [];
  all: boolean = true;
  customer: boolean = false;
  supplier: boolean = false;
  search = '';
  stockValue = '';
  date: Date;

  @ViewChild('AccountOpening', { static: false }) AccountOpening!: ElementRef;

  constructor(
    private toast: ToastrService,
    private apiService: ApiService,
    private dp: DatePipe,
    private com: CommonService,
    private auth: AuthService
  ) {}

  ngOnInit() {
    this.getAccountOp();
  }

  onViewReport() {
    const fromDate = this.dp.transform(this.date, 'yyyy/MM/dd');
    const toDate = this.dp.transform(this.date, 'yyyy/MM/dd');

    let url = `PrintVoucherRangeWise?DateFrom=${fromDate}&DateTo=${toDate}&VchType=JV-OP&VchNoFrom=1&VchNoTo=1&CompId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}`;
    this.com.viewReport(url);
  }

  getAccountOp() {
    try {
      this.com.showLoader();

      this.apiService.getData('Accounts/GetAccountOP').subscribe((data) => {
        this.acccount_list = data;
        this.stockValue = parseFloat(data[0].STOCKVALUE).toFixed(2);
        this.date = data[0].VCHDATE;
        setTimeout(() => {
          this.searchGrid();
        }, 100);
        this.com.hideLoader();
      });
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  onDebitChange(data: any) {
    if (data.DEBIT) {
      data.CREDIT = 0;
    }
    this.searchGrid();
  }

  onCreditChange(data: any) {
    if (data.CREDIT) {
      data.DEBIT = 0;
    }
    this.searchGrid();
  }

  saveAccountOp() {
    const mylist = this.acccount_list.filter(
      (x: any) =>
        (x.DEBIT != 0 && x.DEBIT != '' && x.DEBIT != null) ||
        (x.CREDIT != 0 && x.CREDIT != '' && x.CREDIT != null)
    );

    try {
      this.com.showLoader();

      const account: any[] = mylist.map((data: any) => ({
        code: data.CODE,
        dtNow: new Date(),
        debit: data.DEBIT,
        credit: data.CREDIT,
      }));

      this.apiService
        .saveData('Accounts/SaveAccountOP', account)
        .subscribe((result) => {
          if (result == true || result == 'true') {
            this.com.hideLoader();
            this.toast.success('Save Successfully');
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

  onClickFilter(event: any): string {
    this.all = false;
    this.customer = false;
    this.supplier = false;
    let selectedOption: string = '';

    if (event.target.value == 'all') {
      this.all = true;
      selectedOption = 'all';
    } else if (event.target.value == 'customer') {
      this.customer = true;
      selectedOption = 'customer';
    } else if (event.target.value == 'supplier') {
      this.supplier = true;
      selectedOption = 'supplier';
    }

    this.searchGrid();
    return selectedOption;
  }

  onSearchInput(event: any) {
    this.search = event.target.value;
    this.searchGrid();
  }

  searchGrid(): void {
    const tableElement = this.AccountOpening.nativeElement;
    const rows = tableElement.querySelectorAll('tr');

    let debit = 0;
    let credit = 0;

    rows.forEach((row: HTMLTableRowElement) => {
      let isShow = true;

      if (this.customer) {
        if (row.querySelector('.tag')?.textContent == 'D') {
          isShow = true;
        } else {
          isShow = false;
        }
      }

      if (this.supplier) {
        if (row.querySelector('.tag')?.textContent == 'C') {
          isShow = true;
        } else {
          isShow = false;
        }
      }

      if (isShow) {
        if (
          row.textContent &&
          row.textContent.toLowerCase().indexOf(this.search.toLowerCase()) > -1
        ) {
          isShow = true;
        } else {
          isShow = false;
        }
      }
      if (isShow) {
        const debitInput = row.querySelector('.debit') as HTMLInputElement;
        const creditInput = row.querySelector('.credit') as HTMLInputElement;

        let myDebit = parseFloat(debitInput.value);
        let mycredit = parseFloat(creditInput.value);
        debit += isNaN(myDebit) ? 0 : myDebit;
        credit += isNaN(mycredit) ? 0 : mycredit;

        row.style.display = '';
      } else {
        row.style.display = 'none';
      }
    });
    this.debit = debit;
    this.credit = credit;
  }

  onClickUpdateBs(){
    try {
      this.com.showLoader();

      this.apiService
        .saveObj('Accounts/UpdateBalanceSheet', {})
        .subscribe((r) => {
          if (r == true || r == 'true') {
            this.toast.success('Balance Update Successfully');
            this.com.hideLoader();
            //this.onClickRefresh();
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

  export(fileName: any, formate: any) {
    this.com.ExportFiles(fileName, formate);
  }
}
