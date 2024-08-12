import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-finish-goods-status',
  templateUrl: './finish-goods-status.component.html',
  styleUrls: ['./finish-goods-status.component.css']
})
export class FinishGoodsStatusComponent {


  fromDate: Date;
  toDate: Date;
  today: Date;

  partyList: any [] = [];
  costCenterList: any [] = [];
  itemList: any [] = [];
  selectedVchType = 'PO-PUR';
  
  constructor(
    private apiService: ApiService,
    private dp: DatePipe
  ) {
    const today = new Date();
    this.today = today;
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  }

  ngOnInit() {
    this.getParty();
    this.getItemList();
    this.getCostCenter();
  }

  getParty() {
    debugger;
    const obj = {
      tag: 'C',
    };

    this.apiService
      .getDataById('Reports/GetLevel5List', obj)
      .subscribe((data) => {
        this.partyList = data;
        console.log('data', data);
      });
  }

  getCostCenter() {
    debugger;

    this.apiService
      .getData('Reports/GetCostCenter')
      .subscribe((data) => {
        this.costCenterList = data;
        console.log('data', data);
      });
  }

    
  getItemList() {
    this.apiService.getData('Reports/GetItemList').subscribe((data) => {
      this.itemList = data;
      console.log('data', data);
    });
  }

}
