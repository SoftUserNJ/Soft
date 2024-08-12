import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';
import { ReportModalComponent } from 'src/app/features/report-modal/report-modal.component';
import { AuthService } from 'src/app/services/auth.service';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-daily-sale-activity-report',
  templateUrl: './daily-sale-activity-report.component.html',
  styleUrls: ['./daily-sale-activity-report.component.css']
})
export class DailySaleActivityReportComponent {

  fromDate: Date;
  toDate: Date;
  today: Date;
  modalRef: BsModalRef;
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

  openReportModal() {
    debugger;
    const FromDate = this.dp.transform(this.fromDate, 'yyyy/MM/dd');
    const ToDate = this.dp.transform(this.toDate, 'yyyy/MM/dd');
    const finid = this.auth.finId();
    const locid = this.auth.locId();
    const cmpId = this.auth.cmpId();

    let url = `DailySaleActivityReport?FromDate=${FromDate}&ToDate=${ToDate}&finid=${finid}&locid=${locid}&cmpId=${cmpId}&PreparedBy=${this.auth.username()}&CmpName=${this.auth.cmpName()}&CmpAdr=${this.auth.cmpAdr()}&CmpLogo=${this.auth.cmpLogo()}`;

    const modalConfig: ModalOptions = {
      class: 'custom-modal-width',
      initialState: {
        reportUrl: url,
      },
    };

    this.modalRef = this.modalService.show(ReportModalComponent, modalConfig);
  }
}
