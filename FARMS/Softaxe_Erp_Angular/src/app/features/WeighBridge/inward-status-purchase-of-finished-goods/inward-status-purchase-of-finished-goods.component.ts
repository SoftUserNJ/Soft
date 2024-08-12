import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-inward-status-purchase-of-finished-goods',
  templateUrl: './inward-status-purchase-of-finished-goods.component.html',
  styleUrls: ['./inward-status-purchase-of-finished-goods.component.css']
})
export class InwardStatusPurchaseOfFinishedGoodsComponent {

  @ViewChild('voucherLists', { static: false }) voucherLists!: ElementRef;

  fromDate: Date;
  toDate: Date;
  SaleStatus: FormGroup;
  partyList: any [] = [];
  costCenterList: any [] = [];
  itemList: any [] = [];
  SaleStatusList: any [] = [];
  GoDownList: any [] = [];
  Disabled: boolean;
  constructor(
    private apiService: ApiService,
    private dp: DatePipe,
    private fb: FormBuilder

  ) {
    const today = new Date();
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  }

  ngOnInit() {
    this.getSaleSatusList();
    // this.formInit();
    // this.getParty();
    // this.getItemList();
    // this.getCostCenter();
    // this.getGodowns();
  }

  // formInit() {
  //   this.SaleStatus = this.fb.group({
  //     vchType:['GP-OUT'],
  //     costcenter: [undefined],
  //     toDate :[new Date()],
  //     fromDate: [new Date()],
  //     itemList : [undefined],
  //     partyList: [undefined],
  //     doRangeFrom: [''],
  //     doRangeTO: [''],
  //     VehNo: [''],
  //     waitToEnter: [''],
  //     inForloading: [''],
  //     outAfterLoading: [''],
  //     sumQty: [''],
  //     godown: [undefined],
  //     inwardType: ['ALL'],
  //     month: [''],
  //     arrivalNo: [''],
  //     arrivalNoTO: [''],
  //     LCNO: [''],
  //     remarks: [''],

  //   });
  // }

  getParty() {
    this.apiService
      .getDataById('Common/GetDirectL4L5CodeNamesByL4Tag', {l4Tag: 'C'})
      .subscribe((data) => {
        this.partyList = data;
      });
  }

  getCostCenter() {
    this.apiService
      .getData('Common/GetCostCenter')
      .subscribe((data) => {
        this.costCenterList = data;
      });
  }

  getGodowns() {
    this.apiService
      .getData('Common/GetGodowns')
      .subscribe((data) => {
        this.GoDownList = data;
      });
  }

    
  getItemList() {
    this.apiService
    .getDataById('Common/GetDirectL4L5CodeNamesByL4Tag', {l4Tag: 'S'})
    .subscribe((data) => {
      this.itemList = data;
    });
  }

  getSaleSatusList() {  
    let sumQty = 0;

    const obj = {
      fromDate: this.dp.transform(this.fromDate, 'yyyy-MM-dd'),
      toDate: this.dp.transform(this.toDate, 'yyyy-MM-dd'),
    };

    this.apiService
      .getDataById('WeighBridge/GetOutwardbyDate', obj)
      .subscribe((data) => {

        this.SaleStatusList = data;

        
        data.forEach(item => {
          sumQty += item.Qty;
        });
        // this.SaleStatus.get('sumQty').setValue(sumQty);

      });
  }
  
  // searchVehicleNo(event: Event): void {
  //   const searchTerm = (event.target as HTMLInputElement).value.toLowerCase();
  //   const rows = document.querySelectorAll('.StatusList tbody tr');
  
  //   rows.forEach((row: HTMLTableRowElement) => {
  //     const rowData = row.textContent?.toLowerCase() || '';
  
  //     if (rowData.includes(searchTerm)) {
  //       row.style.display = '';
  //     } else {
  //       row.style.display = 'none';
  //     }
  //   });
  // }

  
  searchGrid(event: any): void {
    const tableElement = this.voucherLists.nativeElement;
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

  rowHighLight(event: any) {
    // Get the clicked row
    const clickedRow = event.target.closest('tr');

    // Add 'HighLightRow' class to all td elements in the clicked row
    const tds = clickedRow.querySelectorAll('td');
    tds.forEach((td) => {
      td.classList.add('HighLightRow');
    });

    // Remove 'HighLightRow' class from other rows
    const allRows = document.querySelectorAll('tr');
    allRows.forEach((row) => {
      if (row !== clickedRow) {
        const otherRowTds = row.querySelectorAll('td');
        otherRowTds.forEach((td) => {
          td.classList.remove('HighLightRow');
        });
      }
    });
  }

  // filter(){
  //   const obj = {
  //     fromDate: this.dp.transform(this.SaleStatus.get('fromDate').value,'yyyy-MM-dd'),
  //     toDate: this.dp.transform(this.SaleStatus.get('toDate').value,'yyyy-MM-dd')
  //   };
  //   this.apiService
  //   .getDataById('Weighbridge/GetOutwardbyDate', obj)
  //   .subscribe((data) => {
  //     this.SaleStatusList = data;
  //   });
  // }

}
