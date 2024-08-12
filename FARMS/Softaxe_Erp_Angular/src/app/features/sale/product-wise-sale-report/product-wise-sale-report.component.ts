import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';
import { ReportModalComponent } from 'src/app/features/report-modal/report-modal.component';
import { AuthService } from 'src/app/services/auth.service';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-product-wise-sale-report',
  templateUrl: './product-wise-sale-report.component.html',
  styleUrls: ['./product-wise-sale-report.component.css']
})
export class ProductWiseSaleReportComponent {

  
  modalRef: BsModalRef;
  fromDate: Date;
  toDate: Date;
  today: Date;

  selectedVchType = 'PO-PUR';
  poNoFrom: string = '';
  poNoTo: string = '';

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

  openProdWiseSaleReport() {
    debugger;
    const VchType = 'PO-PUR';
    const InwardType = 'PO-PUR';
    const VchNoFrom = '1';
    const VchNoTo = '99999999999999';
    const CatFrom = 0;
    const CatTo = 999;
    const PFrom = '00000000000000';
    const PTo = '99999999999999';
    const IFrom = '00000000000000';
    const ITo = '99999999999999';
    const FromDate = this.dp.transform(this.fromDate, 'yyyy/MM/dd');
    const ToDate = this.dp.transform(this.toDate, 'yyyy/MM/dd');

    let url = `ProductWiseSaleReport?VchType=${VchType}&InwardType=${InwardType}&VchNoFrom=${VchNoFrom}&VchNoTo=${VchNoTo}&CatFrom=${CatFrom}&CatTo=${CatTo}&PFrom=${PFrom}&PTo=${PTo}&IFrom=${IFrom}&ITo=${ITo}&FromDate=${FromDate}&ToDate=${ToDate}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}&PreparedBy=${this.auth.username()}&CmpName=${this.auth.cmpName()}&CmpAdr=${this.auth.cmpAdr()}&CmpCont=${this.auth.cmpCont()}&CmpLogo=${this.auth.cmpLogo()}`;

    const modalConfig: ModalOptions = {
      class: 'custom-modal-width',
      initialState: {
        reportUrl: url,
      },
    };

    this.modalRef = this.modalService.show(ReportModalComponent, modalConfig);
  }

  openProdPartyWiseSaleReport() {
    debugger;
    const VchType = 'PO-PUR';
    const InwardType = 'PO-PUR';
    const VchNoFrom = '1';
    const VchNoTo = '999';
    const CatFrom = '0';
    const CatTo = '999';
    const PFrom = '00000000000000';
    const PTo = '99999999999999';
    const IFrom = '00000000000000';
    const ITo = '99999999999999';
    const FromDate = this.dp.transform(this.fromDate, 'yyyy/MM/dd');
    const ToDate = this.dp.transform(this.toDate, 'yyyy/MM/dd');

    let url = `ProductPartyWiseSaleReport?VchType=${VchType}&InwardType=${InwardType}&VchNoFrom=${VchNoFrom}&VchNoTo=${VchNoTo}&CatFrom=${CatFrom}&CatTo=${CatTo}&PFrom=${PFrom}&PTo=${PTo}&IFrom=${IFrom}&ITo=${ITo}&FromDate=${FromDate}&ToDate=${ToDate}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}&PreparedBy=${this.auth.username()}&CmpName=${this.auth.cmpName()}&CmpAdr=${this.auth.cmpAdr()}&CmpCont=${this.auth.cmpCont()}&CmpLogo=${this.auth.cmpLogo()}`;

    const modalConfig: ModalOptions = {
      class: 'custom-modal-width',
      initialState: {
        reportUrl: url,
      },
    };

    this.modalRef = this.modalService.show(ReportModalComponent, modalConfig);
  }
}
