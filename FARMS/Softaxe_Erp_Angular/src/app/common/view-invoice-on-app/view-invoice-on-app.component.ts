import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { ReportModalComponent } from 'src/app/features/report-modal/report-modal.component';
@Component({
  selector: 'app-view-invoice-on-app',
  templateUrl: './view-invoice-on-app.component.html',
  styleUrls: ['./view-invoice-on-app.component.css'],
})
export class ViewInvoiceOnAppComponent {
  modalRef: BsModalRef;

  constructor(
    private route: ActivatedRoute,
    private modalService: BsModalService
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      this.onViewReport(params);
    });
  }

  onViewReport(item: any) {
    let url = `SaleInvoice?VchNoFrom=${item.VchNoFrom}&VchNoTo=${item.VchNoTo}&VchType=${item.VchType}&fromDate=${item.fromDate}&toDate=${item.toDate}&CmpId=${item.CmpId}&FinId=${item.FinId}&LocId=${item.LocId}&Logo=${item.Logo}`;
    const modalConfig: ModalOptions = {
      class: 'custom-modal-width ',
      backdrop : 'static',
      keyboard : false,
      initialState: {
        reportUrl: url,
      },
    };
    this.modalRef = this.modalService.show(ReportModalComponent, modalConfig);
    const element = document.querySelector('.close')
    element.remove();
  }
}
