import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { ReportModalComponent } from 'src/app/features/report-modal/report-modal.component';
import { ApiService } from 'src/app/services/api.service';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-purchase-contract-range-wise-report',
  templateUrl: './purchase-contract-range-wise-report.component.html',
  styleUrls: ['./purchase-contract-range-wise-report.component.css'],
})
export class PurchaseContractRangeWiseReportComponent {
  fromDate: Date;
  toDate: Date;
  today: Date;

  selectedVchType = 'PO-PUR';
  modalRef: BsModalRef;
  poNoFrom: string = '';
  poNoTo: string = '';
  purContDetList: any[] = [];
  partyList: any[] = [];
  costCenterList: any[] = [];
  itemList: any[] = [];

  constructor(
    private apiService: ApiService,
    private dp: DatePipe,
    private modalService: BsModalService,
    private auth: AuthService
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
    const obj = {
      tag: 'C',
    };

    this.apiService.getDataById('Test/GetLevel5List', obj).subscribe((data) => {
      this.partyList = data;
    });
  }

  getCostCenter() {
    this.apiService.getData('Test/GetCostCenter').subscribe((data) => {
      this.costCenterList = data;
    });
  }

  getItemList() {
    this.apiService.getData('Test/GetItemList').subscribe((data) => {
      this.itemList = data;
    });
  }

  openPurDetailReport() {
    const VchType = 'PO-PUR';
    const VchnoFrom = 6;
    const PFrom = this.poNoFrom;
    const PTo = this.poNoTo;
    const IFrom = '00000000000000';
    const ITo = '99999999999999';

    let url = `PurchaseDetailReport?VchType=${VchType}&VchnoFrom=${VchnoFrom}&PFrom=${PFrom}&PTo=${PTo}&IFrom=${IFrom}&ITo=${ITo}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}&PreparedBy=${this.auth.username()}&CmpName=${this.auth.cmpName()}&CmpAdr=${this.auth.cmpAdr()}&CmpCont=${this.auth.cmpCont()}&CmpLogo=${this.auth.cmpLogo()}`;

    const modalConfig: ModalOptions = {
      class: 'custom-modal-width',
      initialState: {
        reportUrl: url,
      },
    };

    this.modalRef = this.modalService.show(ReportModalComponent, modalConfig);
  }
}
