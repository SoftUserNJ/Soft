import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';
import { ReportModalComponent } from 'src/app/features/report-modal/report-modal.component';
import { AuthService } from 'src/app/services/auth.service';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-purchase-contract-report',
  templateUrl: './purchase-contract-report.component.html',
  styleUrls: ['./purchase-contract-report.component.css'],
})
export class PurchaseContractReportComponent {
  constructor(
    private apiService: ApiService,
    private dp: DatePipe,
    private modalService: BsModalService,
    private auth: AuthService,
    private com: CommonService
  ) {
    const today = new Date();
    this.today = today;
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  }

  modalRef: BsModalRef;
  fromDate: Date;
  toDate: Date;
  today: Date;
  selectedVchType = 'PO-PUR';
  poNoFrom: string = '';
  poNoTo: string = '';

  purContRptList: any[] = [];
  purDetailList: any[] = [];
  partyList: any[] = [];
  costCenterList: any[] = [];
  itemList: any[] = [];

  status: boolean = false;
  detail: boolean = false;
  pending: boolean = false;
  canceled: boolean = false;
  detailReportStatus: boolean = false;
  reportStatus: string = 'status';

  ngOnInit() {
    this.getParty();
    this.getItemList();
    this.getCostCenter();
    this.getPurchaseContractReport('status');
  }

  onClickFilter(event: any) {
    this.status = false;
    this.detail = false;
    this.pending = false;
    this.canceled = false;
    this.detailReportStatus = false;

    if (event.target.value == 'status') {
      this.status = true;
    } else if (event.target.value == 'detail') {
      this.detail = true;
    } else if (event.target.value == 'pending') {
      this.pending = true;
    } else if (event.target.value == 'canceled') {
      this.canceled = true;
    } else if (event.target.value == 'detailReportStatus') {
      this.detailReportStatus = true;
    }
  }

  getParty() {
    const obj = {
      tag: 'C',
    };

    this.apiService
      .getDataById('Reports/GetLevel5List', obj)
      .subscribe((data) => {
        this.partyList = data;
      });
  }

  getCostCenter() {
    this.apiService.getData('Reports/GetCostCenter').subscribe((data) => {
      this.costCenterList = data;
    });
  }

  getItemList() {
    this.apiService.getData('Reports/GetItemList').subscribe((data) => {
      this.itemList = data;
    });
  }

  getPurDetail(data: any) {

    const vchDate = new Date(
      data.podate.split('/')[2],
      data.podate.split('/')[1] - 1,
      data.podate.split('/')[0]
    );

    const obj = {
      vchNo: data.pono,
      vchDate: this.dp.transform(vchDate, 'yyyy/MM/dd'),
      vchType: 'PO-PUR',
    };

    // this.apiService.getDataById('Reports/GetPurDetail', obj).subscribe((data) => {
    //   this.purDetailList = data;
    // });
  }

  getPurchaseContractReport(tag: any) {
    this.reportStatus = tag;

    const obj = {
      VchType: 'PO-PUR',
      PFrom: this.poNoFrom == '' ? '0' : this.poNoFrom,
      PTo: this.poNoTo == '' ? '99999999999999' : this.poNoTo,
      IFrom: '00000000000000',
      ITo: '99999999999999',
      FinId: this.auth.finId(),
      LocId: this.auth.locId(),
      FromDate: this.dp.transform(this.fromDate, 'yyyy/MM/dd'),
      ToDate: this.dp.transform(this.toDate, 'yyyy/MM/dd'),
      Category: '%',
      cancel: '%',
    };

    this.apiService
      .getDataById('Reports/GetPurchaseContractReport', obj)
      .subscribe((data) => {
        this.purContRptList = data;
      });
  }

  onViewReport() {
    const VchType = 'PO-PUR';
    const PoNo = this.poNoFrom == '' ? '1' : this.poNoFrom;
    const PFrom = this.poNoFrom == '' ? '0' : this.poNoFrom;
    const PTo = this.poNoTo == '' ? '99999999999999' : this.poNoTo;
    const IFrom = '00000000000000';
    const ITo = '99999999999999';
    const FromDate = this.dp.transform(this.fromDate, 'yyyy/MM/dd');
    const ToDate = this.dp.transform(this.toDate, 'yyyy/MM/dd');
    const Category = '%';
    const cancel = '%';

    let url = '';
    if (this.reportStatus == 'status') {
      url = `PurchaseContractReport?VchType=${VchType}&PFrom=${PFrom}&PTo=${PTo}&IFrom=${IFrom}&ITo=${ITo}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}&FromDate=${FromDate}&ToDate=${ToDate}&Category=${Category}&cancel=${cancel}&PreparedBy=${this.auth.username()}&CmpName=${this.auth.cmpName()}&CmpAdr=${this.auth.cmpAdr()}&CmpCont=${this.auth.cmpCont()}&CmpLogo=${this.auth.cmpLogo()}`;
    } else if (this.reportStatus == 'detail') {
      url = `PurchaseDetailReport?VchType=${VchType}&PoNo=${PoNo}&PFrom=${PFrom}&PTo=${PTo}&IFrom=${IFrom}&ITo=${ITo}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}&PreparedBy=${this.auth.username()}&CmpName=${this.auth.cmpName()}&CmpAdr=${this.auth.cmpAdr()}&CmpCont=${this.auth.cmpCont()}&CmpLogo=${this.auth.cmpLogo()}`;
    } else if (this.reportStatus == 'pending') {
      url = `PurchaseContractPendingReport?VchType=${VchType}&PFrom=${PFrom}&PTo=${PTo}&IFrom=${IFrom}&ITo=${ITo}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}&FromDate=${FromDate}&ToDate=${ToDate}&Category=${Category}&cancel=${cancel}&PreparedBy=${this.auth.username()}&CmpName=${this.auth.cmpName()}&CmpAdr=${this.auth.cmpAdr()}&CmpCont=${this.auth.cmpCont()}&CmpLogo=${this.auth.cmpLogo()}`;
    } else if (this.reportStatus == 'canceled') {
      url = `PurchaseContractCanceledReport?VchType=${VchType}&PFrom=${PFrom}&PTo=${PTo}&IFrom=${IFrom}&ITo=${ITo}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}&FromDate=${FromDate}&ToDate=${ToDate}&Category=${Category}&PreparedBy=${this.auth.username()}&CmpName=${this.auth.cmpName()}&CmpAdr=${this.auth.cmpAdr()}&CmpCont=${this.auth.cmpCont()}&CmpLogo=${this.auth.cmpLogo()}`;
    } else if (this.reportStatus == 'detailReportStatus') {
      url = `PurchaseContractStatusReport?VchType=${VchType}&PFrom=${PFrom}&PTo=${PTo}&IFrom=${IFrom}&ITo=${ITo}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}&FromDate=${FromDate}&ToDate=${ToDate}&Category=${Category}&PreparedBy=${this.auth.username()}&CmpName=${this.auth.cmpName()}&CmpAdr=${this.auth.cmpAdr()}&CmpCont=${this.auth.cmpCont()}&CmpLogo=${this.auth.cmpLogo()}`;
    }

    const modalConfig: ModalOptions = {
      class: 'custom-modal-width',
      initialState: {
        reportUrl: url,
      },
    };

    this.modalRef = this.modalService.show(ReportModalComponent, modalConfig);
  }

  printPo(event: any) {
    const url = `PrintPurContract?VchNo=${event.pono}&VchType=PO-Pur&CmpId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}&CmpName=${this.auth.cmpName()}&CmpAdr=${this.auth.cmpAdr()}`;
    this.com.viewReport(url);
  }
}
