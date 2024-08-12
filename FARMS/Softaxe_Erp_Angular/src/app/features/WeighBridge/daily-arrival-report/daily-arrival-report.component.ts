import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { parse } from '@devexpress/analytics-core/analytics-criteria';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-daily-arrival-report',
  templateUrl: './daily-arrival-report.component.html',
  styleUrls: ['./daily-arrival-report.component.css']
})
export class DailyArrivalReportComponent {
  @ViewChild('voucherLists', { static: false }) voucherLists!: ElementRef;

  fromDate: Date;
  toDate: Date;
  arrivalList: any[] = [];
  filteredArrivalList: any[] = [];

  searchQuery: string = '';

  constructor(
    private apiService: ApiService,
    private dp: DatePipe,
    private com: CommonService,
    private auth: AuthService,

  ) {
    const today = new Date();
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  }

  ngOnInit() {
    this.getArrivalList();
  }
  getArrivalList() {

    const obj = {
      fromDate: this.dp.transform(this.fromDate, 'yyyy-MM-dd'),
      toDate: this.dp.transform(this.toDate, 'yyyy-MM-dd'),
    };

    this.apiService
      .getDataById('WeighBridge/GetOutwardbyDate', obj)
      .subscribe((data) => {

        data.forEach((item: any) => {
          let form: any = item;

          const wt = item.firstweight ?? 0 - item.secweight ?? 0;
          const bags = item.Bags ?? 0;
          var avgPB = bags == 0 ? 0 : wt/ bags;

          if(isNaN(avgPB)){ avgPB = 0; }
          else if(avgPB == Infinity){ avgPB = 0; }

          form.avgPB = parseFloat(avgPB.toFixed(2));
          this.arrivalList.push(form);
        });

        this.filteredArrivalList = [...this.arrivalList];
        this.total();

      });
  }


  // Totals
  totalGrossParty: number = 0;
  totalTareParty: number = 0;
  totalNetParty: number = 0;
  totalGrossOur: number = 0;
  totalTareOur: number = 0;
  totalNetOur: number = 0;
  totalAvg: number = 0;
  totalDiff: number = 0;
  totalBags: number = 0;
  totalLab: number = 0;
  totalOth: number = 0;
  totalParty: number = 0;
  totalOur: number = 0;
  totalTaken: number = 0;
  totalFreight: number = 0;
  totalRcv: number = 0;
  totalRej: number = 0;
  totalAcp: number = 0;

  filterData() {
    this.filteredArrivalList = this.arrivalList.filter((item) => {
      for (const key in item) {
        if (typeof item[key] === 'string' && item[key].toLowerCase().includes(this.searchQuery.toLowerCase())) {
          return true;
        }
      }
      return false;
    });
    this.total();
  }

  total() {
    this.totalGrossParty = this.filteredArrivalList.reduce((total, item) => total + item.Gross, 0);
    this.totalTareParty = this.filteredArrivalList.reduce((total, item) => total + item.Tare, 0);
    this.totalNetParty = this.filteredArrivalList.reduce((total, item) => total + item.Gross ?? 0 - item.Tare ?? 0, 0);
    this.totalGrossOur = this.filteredArrivalList.reduce((total, item) => total + item.firstweight, 0);
    this.totalTareOur = this.filteredArrivalList.reduce((total, item) => total + item.secweight, 0);
    this.totalNetOur = this.filteredArrivalList.reduce((total, item) => total + (item.firstweight ?? 0 - item.secweight ?? 0), 0);
    this.totalAvg = this.filteredArrivalList.reduce((total, item) => total + item.avgPB, 0);
    this.totalDiff = this.filteredArrivalList.reduce((total, item) => total + ((item.firstweight ?? 0 - item.secweight ?? 0) - (item.Gross ?? 0 - item.Tare ?? 0)), 0);
    this.totalBags = this.filteredArrivalList.reduce((total, item) => total + item.SBags, 0);
    this.totalLab = this.filteredArrivalList.reduce((total, item) => total + item.LabDed, 0);
    this.totalOth = this.filteredArrivalList.reduce((total, item) => total + 0, 0);
    this.totalParty = this.filteredArrivalList.reduce((total, item) => total + item.PayableWt1, 0);
    this.totalOur = this.filteredArrivalList.reduce((total, item) => total + item.PayableWt, 0);
    this.totalTaken = this.filteredArrivalList.reduce((total, item) => total + item.PayableWt1, 0);
    this.totalFreight = this.filteredArrivalList.reduce((total, item) => total + item.Freight, 0);
    this.totalRcv = this.filteredArrivalList.reduce((total, item) => total + item.Bags, 0);
    this.totalRej = this.filteredArrivalList.reduce((total, item) => total + item.RejBags, 0);
    this.totalAcp = this.filteredArrivalList.reduce((total, item) => total + ((item.Bags ?? 0) - (item.RejBags ?? 0)), 0);
  }

  // total() {

  //   const rows = this.voucherLists.nativeElement.querySelectorAll('tr');

  //   let totalGrossParty = 0;
  //   let totalTareParty = 0;
  //   let totalNetParty = 0;
  //   let totalGrossOur = 0;
  //   let totalTareOur = 0;
  //   let totalNetOur = 0;
  //   let totalAvg = 0;
  //   let totalDiff = 0;
  //   let totalBags = 0;
  //   let totalLab = 0;
  //   let totalOth = 0;
  //   let totalParty = 0;
  //   let totalOur = 0;
  //   let totalTaken = 0;
  //   let totalFreight = 0;
  //   let totalRcv = 0;
  //   let totalRej = 0;
  //   let totalAcp = 0;

  //   rows.forEach((row: HTMLTableRowElement) => {

  //     const totalGrossPartyInput = row.querySelector('.totalGrossParty') as HTMLInputElement;
  //     const totalTarePartyInput = row.querySelector('.totalTareParty') as HTMLInputElement;
  //     const totalNetPartyInput = row.querySelector('.totalNetParty') as HTMLInputElement;
  //     const totalGrossOurInput = row.querySelector('.totalGrossOur') as HTMLInputElement;
  //     const totalTareOurInput = row.querySelector('.totalTareOur') as HTMLInputElement;
  //     const totalNetOurInput = row.querySelector('.totalNetOur') as HTMLInputElement;
  //     const totalAvgInput = row.querySelector('.totalAvg') as HTMLInputElement;
  //     const totalDiffInput = row.querySelector('.totalDiff') as HTMLInputElement;
  //     const totalBagsInput = row.querySelector('.totalBags') as HTMLInputElement;
  //     const totalLabInput = row.querySelector('.totalLab') as HTMLInputElement;
  //     const totalOthInput = row.querySelector('.totalOth') as HTMLInputElement;
  //     const totalPartyInput = row.querySelector('.totalParty') as HTMLInputElement;
  //     const totalOurInput = row.querySelector('.totalOur') as HTMLInputElement;
  //     const totalTakenInput = row.querySelector('.totalTaken') as HTMLInputElement;
  //     const totalFreightInput = row.querySelector('.totalFreight') as HTMLInputElement;
  //     const totalRcvInput = row.querySelector('.totalRcv') as HTMLInputElement;
  //     const totalRejInput = row.querySelector('.totalRej') as HTMLInputElement;
  //     const totalAcpInput = row.querySelector('.totalAcp') as HTMLInputElement;


  //     // Add NaN check before parsing and summing
  //     if (totalGrossPartyInput && !isNaN(parseFloat(totalGrossPartyInput.innerText.replace(/,/g, '')))) {
  //       totalGrossParty += parseFloat(totalGrossPartyInput.innerText.replace(/,/g, ''));
  //     }

  //     if (totalTarePartyInput && !isNaN(parseFloat(totalTarePartyInput.innerText.replace(/,/g, '')))) {
  //       totalTareParty += parseFloat(totalTarePartyInput.innerText.replace(/,/g, ''));
  //     }

  //     if (totalNetPartyInput && !isNaN(parseFloat(totalNetPartyInput.innerText.replace(/,/g, '')))) {
  //       totalNetParty += parseFloat(totalNetPartyInput.innerText.replace(/,/g, ''));
  //     }

  //     if (totalGrossOurInput && !isNaN(parseFloat(totalGrossOurInput.innerText.replace(/,/g, '')))) {
  //       totalGrossOur += parseFloat(totalGrossOurInput.innerText.replace(/,/g, ''));
  //     }

  //     if (totalTareOurInput && !isNaN(parseFloat(totalTareOurInput.innerText.replace(/,/g, '')))) {
  //       totalTareOur += parseFloat(totalTareOurInput.innerText.replace(/,/g, ''));
  //     }

  //     if (totalNetOurInput && !isNaN(parseFloat(totalNetOurInput.innerText.replace(/,/g, '')))) {
  //       totalNetOur += parseFloat(totalNetOurInput.innerText.replace(/,/g, ''));
  //     }

  //     if (totalAvgInput && !isNaN(parseFloat(totalAvgInput.innerText.replace(/,/g, '')))) {
  //       totalAvg += parseFloat(totalAvgInput.innerText.replace(/,/g, ''));
  //     }

  //     if (totalDiffInput && !isNaN(parseFloat(totalDiffInput.innerText.replace(/,/g, '')))) {
  //       totalDiff += parseFloat(totalDiffInput.innerText.replace(/,/g, ''));
  //     }

  //     if (totalBagsInput && !isNaN(parseFloat(totalBagsInput.innerText.replace(/,/g, '')))) {
  //       totalBags += parseFloat(totalBagsInput.innerText.replace(/,/g, ''));
  //     }

  //     if (totalLabInput && !isNaN(parseFloat(totalLabInput.innerText.replace(/,/g, '')))) {
  //       totalLab += parseFloat(totalLabInput.innerText.replace(/,/g, ''));
  //     }

  //     if (totalOthInput && !isNaN(parseFloat(totalOthInput.innerText.replace(/,/g, '')))) {
  //       totalOth += parseFloat(totalOthInput.innerText.replace(/,/g, ''));
  //     }

  //     if (totalPartyInput && !isNaN(parseFloat(totalPartyInput.innerText.replace(/,/g, '')))) {
  //       totalParty += parseFloat(totalPartyInput.innerText.replace(/,/g, ''));
  //     }

  //     if (totalOurInput && !isNaN(parseFloat(totalOurInput.innerText.replace(/,/g, '')))) {
  //       totalOur += parseFloat(totalOurInput.innerText.replace(/,/g, ''));
  //     }

  //     if (totalTakenInput && !isNaN(parseFloat(totalTakenInput.innerText.replace(/,/g, '')))) {
  //       totalTaken += parseFloat(totalTakenInput.innerText.replace(/,/g, ''));
  //     }

  //     if (totalFreightInput && !isNaN(parseFloat(totalFreightInput.innerText.replace(/,/g, '')))) {
  //       totalFreight += parseFloat(totalFreightInput.innerText.replace(/,/g, ''));
  //     }

  //     if (totalRcvInput && !isNaN(parseFloat(totalRcvInput.innerText.replace(/,/g, '')))) {
  //       totalRcv += parseFloat(totalRcvInput.innerText.replace(/,/g, ''));
  //     }

  //     if (totalRejInput && !isNaN(parseFloat(totalRejInput.innerText.replace(/,/g, '')))) {
  //       totalRej += parseFloat(totalRejInput.innerText.replace(/,/g, ''));
  //     }

  //     if (totalAcpInput && !isNaN(parseFloat(totalAcpInput.innerText.replace(/,/g, '')))) {
  //       totalAcp += parseFloat(totalAcpInput.innerText.replace(/,/g, ''));
  //     }

  //   });

  //   // this.qtySum = qtySum;
  //   // this.rateSum = rateSum;
  //   // this.amountSum = amountSum;
  //   // this.saleTaxSum = saleTaxSum;
  //   // this.taxAmountSum = parseFloat(taxAmountSum.toFixed(2));
  //   // this.netAmountSum = parseFloat(netAmountSum.toFixed(2));

  //   this.totalGrossParty = totalGrossParty;
  //   this.totalTareParty = totalTareParty;
  //   this.totalNetParty = totalNetParty;
  //   this.totalGrossOur = totalGrossOur;
  //   this.totalTareOur = totalTareOur;
  //   this.totalNetOur = totalNetOur;
  //   this.totalAvg = totalAvg;
  //   this.totalDiff = totalDiff;
  //   this.totalBags = totalBags;
  //   this.totalLab = totalLab;
  //   this.totalOth = totalOth;
  //   this.totalParty = totalParty;
  //   this.totalOur = totalOur;
  //   this.totalTaken = totalTaken;
  //   this.totalFreight = totalFreight;
  //   this.totalRcv = totalRcv;
  //   this.totalRej = totalRej;
  //   this.totalAcp = totalAcp;
  // }



  // searchGrid(event: any): void {
  //   const tableElement = this.voucherLists.nativeElement;
  //   const rows = tableElement.querySelectorAll('tr');

  //   rows.forEach((row: HTMLTableRowElement) => {
  //     let isShow = true;

  //     if (
  //       row.textContent &&
  //       row.textContent
  //         .toLowerCase()
  //         .indexOf(event.target.value.toLowerCase()) > -1
  //     ) {
  //       isShow = true;
  //     } else {
  //       isShow = false;
  //     }

  //     if (isShow) {
  //       row.style.display = '';
  //     } else {
  //       row.style.display = 'none';
  //     }
  //   });

  //   setTimeout(() => {
  //     this.total();
  //   }, 100);
  // }


  printReport() {
    const fromDate = this.dp.transform(this.fromDate, 'yyyy/MM/dd');
    const toDate = this.dp.transform(this.toDate, 'yyyy/MM/dd');
    const url = `DailyArrivalRpt?FromDate=${fromDate}&ToDate=${toDate}&CmpId=${this.auth.cmpId()}&CmpName=${this.auth.cmpName()}&CmpAddress=${this.auth.cmpAdr()}&LocName=${this.auth.locId()}&FinId=${this.auth.finId()}`;
    this.com.viewReport(url);
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

}

