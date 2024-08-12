import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-purchase-order-approval',
  templateUrl: './purchase-order-approval.component.html',
  styleUrls: ['./purchase-order-approval.component.css'],
})
export class PurchaseOrderApprovalComponent {
  @ViewChild('voucherLists', { static: false }) voucherLists!: ElementRef;

  fromDate: Date;
  toDate: Date;
  vchType: any = 'PO-Pur';
  search: any = '';
  VoucherAppList: any = [];
  VoucherAppListFiltered: any = [];

  unapproved: boolean = true;
  approved: boolean = false;
  UnAdited: boolean = false;
  Audited: boolean = false;
  UnVerified: boolean = false;
  Verified: boolean = false;
  Pending: boolean = false;
  All: boolean = false;

  constructor(
    private tostr: ToastrService,
    private apiService: ApiService,
    private dp: DatePipe,
    private auth: AuthService,
    private com: CommonService
  ) {
    const today = new Date();
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  }

  ngOnInit() {
    this.GetVoucher();
  }

  GetVoucher(): void {
    if (this.vchType == null) {
      this.tostr.info('Select Voucher Type');
      return;
    }
    const obj = {
      fromDate: this.dp.transform(this.fromDate, 'yyyy/MM/dd'),
      toDate: this.dp.transform(this.toDate, 'yyyy/MM/dd'),
    };

    this.apiService.getDataById('Approval/GetPoVch', obj).subscribe((data) => {
      if (!data || data.length === 0) {
        this.tostr.info('No Records Found');
        return;
      }

      this.VoucherAppList = data;
      this.VoucherAppListFiltered = data;
      this.searchGrid();
    });
  }

  onClickFilter(event: any) {
    this.unapproved = false;
    this.approved = false;
    this.Audited = false;
    this.Verified = false;
    this.UnVerified = false;
    this.Pending = false;
    this.UnAdited = false;
    this.All = false;

    if (event.target.value == 'unapproved') {
      this.unapproved = true;
    } else if (event.target.value == 'approved') {
      this.approved = true;
    } else if (event.target.value == 'UnAdited') {
      this.UnAdited = true;
    } else if (event.target.value == 'Audited') {
      this.Audited = true;
    } else if (event.target.value == 'UnVerified') {
      this.UnVerified = true;
    } else if (event.target.value == 'Verified') {
      this.Verified = true;
    } else if (event.target.value == 'Pending') {
      this.Pending = true;
    } else if (event.target.value == 'All') {
      this.All = true;
    }
    this.searchGrid();
  }

  searchGrid() {
    if (this.approved) {
      this.VoucherAppListFiltered = this.VoucherAppList.filter(
        (item) => item.Aprove === true
      );
    } else if (this.unapproved) {
      this.VoucherAppListFiltered = this.VoucherAppList.filter(
        (item) => item.Aprove === false
      );
    } else if (this.Verified) {
      this.VoucherAppListFiltered = this.VoucherAppList.filter(
        (item) => item.Verify === true
      );
    } else if (this.UnVerified) {
      this.VoucherAppListFiltered = this.VoucherAppList.filter(
        (item) => item.Verify === false
      );
    } else if (this.Audited) {
      this.VoucherAppListFiltered = this.VoucherAppList.filter(
        (item) => item.AuditByN === true
      );
    } else if (this.UnAdited) {
      this.VoucherAppListFiltered = this.VoucherAppList.filter(
        (item) => item.AuditByN === false
      );
    } else if (this.Pending) {
      this.VoucherAppListFiltered = this.VoucherAppList.filter(
        (item) => item.Rcvd === true
      );
    } else {
      this.VoucherAppListFiltered = this.VoucherAppList;
    }
  }

  SearchVch(event: Event): void {
    const searchTerm = (event.target as HTMLInputElement).value.toLowerCase();
    const rows = document.querySelectorAll('.vouchers tbody tr');

    rows.forEach((row: HTMLTableRowElement) => {
      const rowData = row.textContent?.toLowerCase() || '';

      if (rowData.includes(searchTerm)) {
        row.style.display = '';
      } else {
        row.style.display = 'none';
      }
    });
  }

  onClickSave() {
    if (this.VoucherAppList.length == 0) {
      this.tostr.warning('Incomplete Transaction...');
      return;
    }

    try {
      this.com.showLoader();

      const voucher: any[] = this.VoucherAppListFiltered.map((data) => ({
        VchNo: data.VchNo,
        VchType: data.Type,
        IsApproved: data.Aprove,
        IsVerified: data.Verify,
        IsAudited: data.AuditByN,
        IsPending: data.Rcvd,
      }));
      

      this.apiService
        .saveData('Approval/UpdatePOVoucherStatus', voucher)
        .subscribe((result) => {
          if (result == true || result == 'true') {
            this.tostr.success('Save Successfully');
            this.com.hideLoader();
            this.GetVoucher();
          } else {
            this.tostr.error('Please Save Again');
          }
        });
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  toggleAll(tag: any, e: any) {
    if (tag == 'approve') {
      if (e.target.checked) {
        this.VoucherAppListFiltered.forEach((item) => {
          item.Aprove = true;
        });
      } else {
        this.VoucherAppListFiltered.forEach((item) => {
          item.Aprove = false;
        });
      }
    } else if (tag == 'verify') {
      if (e.target.checked) {
        this.VoucherAppListFiltered.forEach((item) => {
          item.Verify = true;
        });
      } else {
        this.VoucherAppListFiltered.forEach((item) => {
          item.Verify = false;
        });
      }
    } else if (tag == 'audit') {
      if (e.target.checked) {
        this.VoucherAppListFiltered.forEach((item) => {
          item.AuditByN = true;
        });
      } else {
        this.VoucherAppListFiltered.forEach((item) => {
          item.AuditByN = false;
        });
      }
    }
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

  onViewReport(event: any) {
    const url = `PrintPurContract?VchNo=${event.VchNo}&VchType=${
      event.VchType
    }&CmpId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}&CmpName=${this.auth.cmpName()}&CmpAdr=${this.auth.cmpAdr()}`;
    debugger;
    this.com.viewReport(url);
  }
}