import { Component, ElementRef, ViewChild } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';
import { MatSort } from '@angular/material/sort';

@Component({
  selector: 'app-purchase-rate-comparison',
  templateUrl: './purchase-rate-comparison.component.html',
  styleUrls: ['./purchase-rate-comparison.component.css'],
})
export class PurchaseRateComparisonComponent {
  productLists: any[] = [];
  search: string ='';
  amount: number = 0;

  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild('productListsSearch', { static: false })
  productListsSearch!: ElementRef;

  constructor(private apiService: ApiService) {}

  onInputSearchProduct(event: any) {
    const obj = {
      name: event.target.value,
    };

    this.apiService
      .getDataById('Inventory/GetPurchaseRateComparison', obj)
      .subscribe((data) => {
        this.productLists = data;
        setTimeout(() => {
          this.searchGrid();
        }, 100);
      });
  }

  onInputSearchData(event: any) {
    this.search = event.target.value;
    this.searchGrid();
  }

  searchGrid(): void {
    const tableElement = this.productListsSearch.nativeElement;
    const rows = tableElement.querySelectorAll('tr');
    this.amount = 0;

    rows.forEach((row: HTMLTableRowElement) => {
      let isShow = true;

      if (
        row.textContent &&
        row.textContent.toLowerCase().indexOf(this.search.toLowerCase()) > -1
      ) {
        isShow = true;
      } else {
        isShow = false;
      }

      if (isShow) {
        this.amount += parseFloat(row.querySelector('.amount')?.textContent.replace(/,/g, ''));
        row.style.display = '';
      } else {
        row.style.display = 'none';
      }
    });
  }
}
