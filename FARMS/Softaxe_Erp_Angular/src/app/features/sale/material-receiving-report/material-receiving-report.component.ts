import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';
import { environment } from 'src/environment/environmemt';

@Component({
  selector: 'app-material-receiving-report',
  templateUrl: './material-receiving-report.component.html',
  styleUrls: ['./material-receiving-report.component.css']
})
export class MaterialReceivingReportComponent {
  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;

  fromDate: Date;
  toDate: Date;

  typesList = [
    { vlaue: 'CP', name: 'CP' },
    { vlaue: 'CP-FREIGHT', name: 'CP-FREIGHT' },
    { vlaue: 'CR', name: 'CR' },
    { vlaue: 'BP', name: 'BP' },
    { vlaue: 'BR', name: 'BR' },
  ];

  basePath = environment.basePath;

  partyList: any[] = [];
  bankCashList: any[] = [];
  groupList: any[] = [];
  companyList: any[] = [];
  voucherList: any[] = [];
  locationList: any = [];
  bags: number = 0;
  gross: number = 0;
  tare: number = 0;
  ourNet: number = 0;
  BiltyWeight: number = 0;
  bardana: number = 0;
  moist: number = 0;
  other: number = 0;
  PayableWeight: number = 0;
  StockWeight: number = 0;
  Freight: number = 0;
  party = '';
  bankCash = '';
  type = '';
  cmpDisable: boolean = true;
  isMulti: boolean = true;
  srcPdfImg: any;
  file: File | null = null;
  row: any;
  fileList: any;
  isPdf: boolean = false;
  cmpId = localStorage.getItem('cmpId');

  totalBags: number = 0;
  totalGross:number = 0;
  totalTare:number = 0;
  totalNet :number = 0;
  totalBiltyWt :number = 0;
  TotalBardana :number = 0;
  Totalmoist :number = 0;
  TotalOther :number = 0;
  TotalPayableWeight :number = 0;
  TotalStockWeight :number = 0;
  TotalFreight :number = 0;

  @ViewChild('PRStatus', { static: false }) PRStatus!: ElementRef;
  constructor(
    private apiService: ApiService,
    private dp: DatePipe,
    private toastr: ToastrService,
    private auth: AuthService,
    private com: CommonService
  ) {
    const today = new Date();
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  }

  ngOnInit() {
    this.getVoucherList();
  
  }




  getVoucherList() {

    this.totalBags = 0;
    this.totalGross = 0;
    this.totalTare = 0;
    this.totalNet = 0;
    this.totalBiltyWt = 0;
    this.TotalBardana = 0;
    this.TotalPayableWeight = 0;
    this.TotalStockWeight = 0;
    this.TotalFreight = 0;

    try {
      this.com.showLoader();
      this.voucherList = [];
      const obj = {
        fromDate: this.dp.transform(this.fromDate, 'yyyy/MM/dd'),
        toDate: this.dp.transform(this.toDate, 'yyyy/MM/dd'),
      };

      this.apiService
        .getDataById('Sale/GetMRRDetail', obj)
        .subscribe((data) => {
          console.log(data)
          this.voucherList = data;

         data.forEach((item: any) => { 
          this.totalBags += item.Bags;
          this.totalGross += item.FirstWeight;
          this.totalTare += item.SecWeight;
          this.totalNet += (item.FirstWeight-item.SecWeight);
          this.totalBiltyWt += (item.BiltWt);
          this.TotalBardana += (item.BagsDED);
          this.TotalPayableWeight += (item.PayableWT);
          this.TotalStockWeight += (item.PayableWT1);
          this.TotalFreight += (item.Freight);

         });

          this.com.hideLoader();
        });
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  search(event: any): void {
    const tableElement = this.PRStatus.nativeElement;
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

  // searchGrid(): void {
  //   const tableElement = this.PRStatus.nativeElement;
  //   const rows = tableElement.querySelectorAll('tr');

  //   let totalBags = 0;
  //   let totalBiltyWt = 0;
  //   let totalGross = 0;
  //   let totalNet = 0;
  //   let totalTare = 0;
  //   let TotalBardana = 0;
  //   let TotalFreight = 0;
  //   let Totalmoist = 0;
  //   let TotalOther = 0;
  //   let TotalPayableWeight = 0;
  //   let TotalStockWeight = 0;

  //   rows.forEach((row: HTMLTableRowElement) => {
  //     let isShow = true;

  //     if (
  //       row.querySelector('.partyCode')?.textContent != this.party &&
  //       this.party.length > 0
  //     ) {
  //       isShow = false;
  //     }

  //     if (
  //       row.querySelector('.bcCode')?.textContent != this.bankCash &&
  //       this.bankCash.length > 0
  //     ) {
  //       isShow = false;
  //     }

  //     if (
  //       row.querySelector('.vchType')?.textContent != this.type &&
  //       this.type.length > 0
  //     ) {
  //       isShow = false;
  //     }

  //     if (isShow) {
  //       tax += parseFloat(
  //         row.querySelector('.tax')?.textContent!.replace(/,/g, '')
  //       );
  //       bankReceived += parseFloat(
  //         row.querySelector('.BR')?.textContent!.replace(/,/g, '')
  //       );
  //       bankPayment += parseFloat(
  //         row.querySelector('.BP')?.textContent!.replace(/,/g, '')
  //       );
  //       cashReceived += parseFloat(
  //         row.querySelector('.CR')?.textContent!.replace(/,/g, '')
  //       );
  //       cashPayment += parseFloat(
  //         row.querySelector('.CP')?.textContent!.replace(/,/g, '')
  //       );
  //       creditReceived += parseFloat(
  //         row.querySelector('.CreditCardR')?.textContent!.replace(/,/g, '')
  //       );
  //       creditPayment += parseFloat(
  //         row.querySelector('.CreditCardP')?.textContent!.replace(/,/g, '')
  //       );
  //       row.style.display = '';
  //     } else {
  //       row.style.display = 'none';
  //     }
  //   });

  //   this.totalBags = totalBags;
  //   this.totalBiltyWt = totalBiltyWt;
  //   this.totalGross = totalGross;
  //   this.totalNet = totalNet;
  //   this.totalTare = totalTare;
  //   this.TotalBardana = TotalBardana;
  //   this.TotalFreight = TotalFreight;
  //   this.Totalmoist = Totalmoist;
  //   this.TotalOther = TotalOther;
  //   this.TotalPayableWeight = TotalOther;
  //   this.TotalStockWeight = TotalStockWeight;

  // }


  PrintMRR() {
   let FromDate = this.dp.transform(this.fromDate, 'yyyy/MM/dd');
    let ToDate = this.dp.transform(this.toDate, 'yyyy/MM/dd');

    let url = `RptMaterialReceived?CatFrom=0&CatTo=999999999&CmpId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&FromDate=${FromDate}&Ifrom=0&Ito=99999999999999&LocId=U1&Pfrom=0
    &Pto=99999999999999&ToDate=${ToDate}&VchFrom=0&VchTo=999999999&VchType=RP-RAW`;
    this.com.viewReport(url);
  }

  
ReceiveingReportSlip(Vchno:any, VchDate:any, VehicleNo:any, Gpno:any) {

  let url = `ReceivingOfGoods?VchType=RP-RAW&VchNo=${Vchno}&FinId=${this.auth.finId()}&LocId=U1&FromDate=${VchDate}&VehNo=${VehicleNo}&GpNo=${Gpno}&CmpId=${this.auth.cmpId()}`;
  this.com.viewReport(url);
  }
  
  SecondWeightSlip(Vchno:any, VchDate:any, VehicleNo:any, Gpno:any) {
  
  let url = `SecondWeightSlip?VchType=RP-RAW&VchNo=${Vchno}&FinId=${this.auth.finId()}&LocId=U1&FromDate=${VchDate}&VehNo=${VehicleNo}&GpNo=${Gpno}&CmpId=${this.auth.cmpId()}`;
  this.com.viewReport(url);
  }
  
}
