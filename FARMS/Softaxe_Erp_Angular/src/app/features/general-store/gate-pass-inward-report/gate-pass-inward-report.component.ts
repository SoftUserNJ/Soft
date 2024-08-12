import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-gate-pass-inward-report',
  templateUrl: './gate-pass-inward-report.component.html',
  styleUrls: ['./gate-pass-inward-report.component.css']
})
export class GatePassInwardReportComponent {
  
  fromDate: Date;
  toDate: Date;
  today: Date;

  partyList: any [] = [];
  costCenterList: any [] = [];
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
    this.getCostCenter();
  }

  getParty() {
    debugger;
    const obj = {
      tag: 'C',
    };

    this.apiService
      .getDataById('Test/GetLevel5List', obj)
      .subscribe((data) => {
        this.partyList = data;
        console.log('data', data);
      });
  }

  getCostCenter() {
    debugger;

    this.apiService
      .getData('Test/GetCostCenter')
      .subscribe((data) => {
        this.costCenterList = data;
        console.log('data', data);
      });
  }

}
