import { Component, ElementRef, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-expense-order',
  templateUrl: './expense-order.component.html',
  styleUrls: ['./expense-order.component.css'],
})
export class ExpenseOrderComponent {
  @ViewChild('expList', { static: false }) expList!: ElementRef;

  expenseList: any[] = [];

  constructor(
    private apiService: ApiService,
    private tostr: ToastrService,
    private com: CommonService
  ) {}

  ngOnInit(): void {
    this.getExpense();
  }

  getExpense() {
    this.apiService.getData('Inventory/GetExpenseList').subscribe((data) => {
      this.expenseList = data;
    });
  }

  onClickSave() {
    const list = this.expenseList.filter(
      (x) => x.OrderByNo != null && x.OrderByNo != ''
    );

    if (list.length == 0) {
      this.tostr.warning('Enter Order No...!');
      return;
    }

    try {
      this.com.showLoader();
      const exp: any[] = list.map((data: any) => ({
        code: data.code,
        orderNo: data.OrderByNo,
      }));

      this.apiService
        .saveObj('Sale/ExpOrderNoUpdate', { vM: JSON.stringify(exp) })
        .subscribe((result) => {
          if (result == true || result == 'true') {
            this.com.hideLoader();
            this.tostr.success('Save Successfully');
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

  searchGrid(event: any): void {
    const tableElement = this.expList.nativeElement;
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
