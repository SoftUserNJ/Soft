import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';

import { objectsVisitor } from '@devexpress/analytics-core/analytics-internal';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';
import { FormBuilder, FormGroup } from '@angular/forms';
declare const $: any;





@Component({
  selector: 'app-inward-purchase-status',
  templateUrl: './inward-purchase-status.component.html',
  styleUrls: ['./inward-purchase-status.component.css']
})
export class InwardPurchaseStatusComponent {

  fromDate: Date;
  toDate: Date;
  today: Date;

  inwardPurchaseStatusList: any[] = [];
  filteredinwardPurchaseStatusList: any[] = [];
  processingDetails:any[]=[];
  searchQuery: string = '';
  radioFilter: string = 'allR';

  constructor(
    private apiService: ApiService,
    private dp: DatePipe,
    private auth: AuthService,
    private com: CommonService
    
  ) {
    const today = new Date();
    this.today = today;
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    // this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
    this.toDate = new Date();
  }

  @ViewChild('voucherLists', { static: false }) voucherLists!: ElementRef;

  ngOnInit() {
    this.getInwardPurchaseStatusReport();
  }

  getInwardPurchaseStatusReport() {

    const obj = {
      FromDate: this.dp.transform(this.fromDate, 'yyyy/MM/dd'),
      ToDate: this.dp.transform(this.toDate, 'yyyy/MM/dd'),
    };

    this.apiService
      .getDataById('Reports/GetInwardPurchaseStatusReport', obj)
      .subscribe((data) => {

        this.inwardPurchaseStatusList = [];

        data.forEach((item: any) => {
          let form: any = item;

          form.allR = 'allR';
          form.pendingR = form.FirstWeight != 0 && form.SecWeight == 0 ? 'pendingR' : '';
          form.unloadedR = form.SecWeight != 0 ? 'unloadedR' : '';
          form.rejectedR = form.Reject != 0 ? 'rejectedR' : '';
          form.atGateR = form.FirstWeight == 0 && form.SecWeight == 0 ? 'atGateR' : '';

          this.inwardPurchaseStatusList.push(form);
        });

         this.filteredinwardPurchaseStatusList = [...this.inwardPurchaseStatusList];

        setTimeout(() => {
          this.filterData();
        }, 100);


      });
  }
  
  ReceiveingReportSlip(Vchno:any, VchDate:any, VehicleNo:any, Gpno:any , Locid:any) {

    let url = `ReceivingOfGoods?VchType=RP-RAW&VchNo=${Vchno}&FinId=${this.auth.finId()}&LocId=${Locid}&FromDate=${VchDate}&VehNo=${VehicleNo}&GpNo=${Gpno}&CmpId=${this.auth.cmpId()}`;
    this.com.viewReport(url);
    }
    
    SecondWeightSlip(Vchno:any, VchDate:any, VehicleNo:any, Gpno:any , Locid:any) {
    
    let url = `SecondWeightSlip?VchType=RP-RAW&VchNo=${Vchno}&FinId=${this.auth.finId()}&LocId=${Locid}&FromDate=${VchDate}&VehNo=${VehicleNo}&GpNo=${Gpno}&CmpId=${this.auth.cmpId()}`;
    this.com.viewReport(url);
    }
 
    PrintSlip( Vchno:any, ResultDate:any, VehicleNo:any , Locid:any ) {
      debugger;
      const parts = ResultDate.split('/');
      const date = new Date(`${parts[2]}-${parts[1]}-${parts[0]}`);
      const VchDate = date.toISOString().split('T')[0];
      let url = `LabTestSlip?CmpId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${Locid}&VchDate=${VchDate}&VchNo=${Vchno}&VchType=RP-RAW`;
      this.com.viewReport(url);
  }

  async getProcessing(EntryDate:any , DateIn:any  , DateOut:any  , GateDiffWithIn:any , InDiffWithOut:any ,  GateDiffWithOut:any ) {
    this.processingDetails = [];
    try {
     
     debugger;
   
      const multipleObjects = [
        { Detail: "GATE DATE/TIME", Values: EntryDate , Color:"Balack" },
        { Detail: "IN DATE/TIME", Values: DateIn , Color:"Balack" },
        { Detail: "TIME TAKES", Values: this.mysubstring( GateDiffWithIn,0,5) , Color:"Red" },

        { Detail: "OUT DATE/TIME", Values: DateOut , Color:"Balack" },
        { Detail: "LOADING TIME", Values: this.mysubstring( InDiffWithOut,0,5)   , Color:"Red" },
        { Detail: "COMPLETE PROCESS TIME", Values: this.mysubstring( GateDiffWithOut,0,5)   , Color:"Red" }
      ];
      
      
      multipleObjects.forEach(obj => {
        this.processingDetails.push(obj);
      });
        $('#processingdetailmodel').modal('show');

    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }
  mysubstring(str: string, from: number, to: number): string {
    if (str && str.length >= to) {
      return str.substring(from, to);
    } else {
      return str;
    }
  }


  onRadioChange(event: any) {
    this.radioFilter = event.target.value;
    this.filterData();
  }

  // filterData() {

  //   this.filteredinwardPurchaseStatusList = this.inwardPurchaseStatusList.filter((item) => {
  //     for (const key in item) {

  //       if (typeof item[key] === 'string' && item[key].toLowerCase().includes(this.searchQuery.toLowerCase())) {
  //         return true;
  //       }
  //     }
  //     return false;
  //   });
  // }

  filterData() {
    this.filteredinwardPurchaseStatusList = this.inwardPurchaseStatusList.filter((item) => {
      if (this.radioFilter === 'allR' || item[this.radioFilter] === this.radioFilter) {
        if (this.searchQuery === '' ||
          Object.values(item).some(val => typeof val === 'string' && val.toLowerCase().includes(this.searchQuery.toLowerCase()))) {
          return true;
        }
      }
      return false;
    });

    this.total();
  }


  totalBags: number = 0;
  totalPBags: number = 0;
  totalRejBg: number = 0;
  totalStockWt: number = 0;
  totalDiff: number = 0;
  totalLabded: number = 0;
  totalPayableWt: number = 0;
  totalFreight: number = 0;
  totalBagsDed: number = 0;

  total() {
    this.totalPBags= this.filteredinwardPurchaseStatusList.reduce((total, item) => total + item.Sbags, 0);
    this.totalBags = this.filteredinwardPurchaseStatusList.reduce((total, item) => total + item.Bags, 0);
    this.totalRejBg = this.filteredinwardPurchaseStatusList.reduce((total, item) => total + item.RejBg, 0);



    this.totalStockWt = this.filteredinwardPurchaseStatusList.reduce((total, item) => total + item.StockWt, 0);
    this.totalDiff = this.filteredinwardPurchaseStatusList.reduce((total, item) => total + item.QtDiff, 0);
    this.totalLabded = this.filteredinwardPurchaseStatusList.reduce((total, item) => total + item.Labded, 0);
    this.totalPayableWt = this.filteredinwardPurchaseStatusList.reduce((total, item) => total + item.Payablewt, 0);
    this.totalFreight = this.filteredinwardPurchaseStatusList.reduce((total, item) => total + item.Freight, 0);
    this.totalBagsDed = this.filteredinwardPurchaseStatusList.reduce((total, item) => total + item.Bagsded, 0);
  }

}
