import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-customer-updation',
  templateUrl: './customer-updation.component.html',
  styleUrls: ['./customer-updation.component.css'],
})
export class CustomerUpdationComponent implements OnInit {
  constructor(
    private tostr: ToastrService,
    private apiService: ApiService,
    private com: CommonService
  ) {}

  customerList: any[] = [];
  @ViewChild('customerLists', { static: false }) customerLists!: ElementRef;

  ngOnInit() {
    this.GetTbaleData();
  }

  GetTbaleData() {
    try {
      this.com.showLoader();
      this.apiService.getData('Sale/GetCustomer').subscribe((data) => {
        this.customerList = data.party;
        this.com.hideLoader();
      });
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  onClickSave() {
    const list = this.customerList.filter((x) => x.STATUS == true);

    if (list.length == 0) {
      this.tostr.warning('Change any Tax...!');
      return;
    }

    try {
      this.com.showLoader();
      const customer: any[] = list.map((data: any) => ({
        Code: data.code,
        SaleTax: data.saleTax,
        WHTax: data.whTax,
        DtNow: new Date(),
      }));

      this.apiService
        .saveObj('Sale/CustomerTaxUpdate', { vM: JSON.stringify(customer) })
        .subscribe((result) => {
          if (result == true || result == 'true') {
            this.customerList.forEach((x) => {
              x.STATUS = false;
            });
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

  onChangeTax(item: any) {
    item.STATUS = true;
  }

  searchGrid(event: any): void {
    const tableElement = this.customerLists.nativeElement;
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

  export(fileName: any, formate: any) {
    this.com.ExportFiles(fileName, formate);
  }
}
