import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-gate-pass-inward-approval',
  templateUrl: './gate-pass-inward-approval.component.html',
  styleUrls: ['./gate-pass-inward-approval.component.css'],
})
export class GatePassInwardApprovalComponent {
  constructor(
    private tostr: ToastrService,
    private fb: FormBuilder,
    private apiService: ApiService,
    private dp: DatePipe,
    private com: CommonService,
    private auth: AuthService
  ) {}

  @ViewChild('voucherLists', { static: false }) voucherLists!: ElementRef;

  GPIApprovalForm!: FormGroup;
  VoucherAppList: any = [];
  VoucherAppListFiltered: any = [];
  VoucherAppDetail: any = [];
  isShow = false;
  selectedRow: number | null = null;
  btnAdd: string = 'Add';
  voucherList: any[] = [];
  TotalTare: number = 0;
  TotalGross: number = 0;
  TotalRate: number = 0;
  TotalNet: number = 0;
  TotalQty: number = 0;
  TotalBags: number = 0;

  unapproved: boolean = true;
  approved: boolean = false;
  Rejected: boolean = false;
  All: boolean = false;

  ngOnInit() {
    this.formInit();
    this.setCurrentDate();
    this.SelectVoucher();
  }

  formInit() {
    this.GPIApprovalForm = this.fb.group({
      fromDate: [new Date()],
      toDate: [new Date()],
      vehNo: [''],
      selectDate: [''],
      month: [undefined],
      vchType: ['RP-Raw'],
    });
  }

  onClickRefresh() {
    this.isShow = false;
    this.VoucherAppList = [];
    this.VoucherAppListFiltered = [];
    this.VoucherAppDetail = [];
    this.SelectVoucher();
  }

  setCurrentDate() {
    const today = new Date();
    this.GPIApprovalForm.get('fromDate').setValue(
      new Date(today.getFullYear(), today.getMonth(), 1)
    );
    this.GPIApprovalForm.get('toDate').setValue(
      new Date(today.getFullYear(), today.getMonth() + 1, 0)
    );
  }

  onClickFilter(event: any) {
    
    debugger;
    this.unapproved = false;
    this.approved = false;
    this.Rejected = false;
    this.All = false;

    if (event.target.value == 'unapproved') {
      this.unapproved = true;
    } else if (event.target.value == 'approved') {
      this.approved = true;
    } else if (event.target.value == 'Rejected') {
      this.Rejected = true;
    } else if (event.target.value == 'All') {
      this.All = true;
    }
    this.SelectVoucher();
   // this.searchGrid();
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
    } else if (this.Rejected) {
      this.VoucherAppListFiltered = this.VoucherAppList.filter(
        (item) => item.Reject === true
      );
    } else {
      this.VoucherAppListFiltered = this.VoucherAppList;
    }
  }

  SelectVoucher(): void {
    if (
      this.GPIApprovalForm.get('vchType').value == undefined ||
      this.GPIApprovalForm.get('vchType').value == null
    ) {
      this.tostr.info('Select Voucher Type');
      return;
    }
    const obj = {
      fromDate: this.dp.transform(
        this.GPIApprovalForm.get('fromDate').value,
        'yyyy/MM/dd'
      ),
      toDate: this.dp.transform(
        this.GPIApprovalForm.get('toDate').value,
        'yyyy/MM/dd'
      ),
      unapproved: this.unapproved,
      approved: this.approved,
      rejected: this.Rejected,
      all: this.All,
    };
    this.VoucherAppList = [];
    this.VoucherAppListFiltered=[];

    let totalTare = 0;
    let totalGross = 0;
    let totalNet = 0;
    //this.com.showLoader();
    this.apiService.getDataById('Approval/GetGPIVch', obj).subscribe((data) => {
      if (!data || data.length === 0) {
        return;
      }
      //this.com.hideLoader();
      data.forEach((item: any) => {
        let form = item;
        form.VCHNO = item.VCHNO;
        form.VehicleNo = item.VehicleNo;
        form.Party = item.Party;
        form.Freight = item.Freight;
        form.Gross = item.Gross;
        form.Net = item.Net;
        form.TimeIn = item.TimeIn;
        form.Tare = item.Tare;
        form.TimeOut = item.TimeOut;
        form.VchDate = item.VchDate;
        form.Aprove = item.Aprove ?? false;
        form.Reject = item.Reject ?? false;
        form.Rcvd = item.Rcvd ?? false;

        this.VoucherAppList.push(form);

        totalTare += item.Tare;
        totalGross += item.Gross;
        totalNet += item.Net;
      });
   
      this.TotalTare = totalTare;
      this.TotalGross = totalGross;
      this.TotalNet = totalNet;
        
      this.VoucherAppListFiltered = this.VoucherAppList;
      //this.com.hideLoader();
     // this.searchGrid();
    });

   
  }

  getVchDetail(data: any) {
    this.TotalQty = 0;
    this.TotalBags = 0;
    this.apiService
      .getDataById('Approval/GetGPIVchDetail', { vchNo: data })
      .subscribe((data) => {
        this.VoucherAppDetail = data;
        data.forEach((item: any) => {
          this.TotalQty += item.TOTALSQTY;
          this.TotalBags += item.TOTALBAGS;
        });
      });
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

    const voucher: any[] = this.VoucherAppListFiltered.map((data) => ({
      VchNo: data.VCHNO,
      VchType: this.GPIApprovalForm.get('vchType').value,
      IsApproved: data.Aprove,
      IsRejected: data.Reject,
    }));

    this.apiService
      .saveData('Approval/UpdateGPIVoucherStatus', voucher)
      .subscribe((result) => {
        if (result == true || result == 'true') {
          this.tostr.success('Save Successfully');
          this.onClickRefresh();
          this.SelectVoucher();
        } else {
          this.tostr.error('Please Save Again');
        }
      });
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

  updateApproveStatus(event: any, data: any) {
    data.Aprove = event.target.checked;
  }

  updateTypeStatus(event: any, data: any) {
    data.Rcvd = event.target.checked;
  }

  updateRejectedStatus(event: any, data: any) {
    data.Reject = event.target.checked;
  }


  PrintSlip(ResultDate, ArrivalNo) {
    const parts = ResultDate.split('/');
    const date = new Date(`${parts[2]}-${parts[1]}-${parts[0]}`);
    const VchDate = date.toISOString().split('T')[0];
  
    let url = `LabTestSlip?CmpId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}&VchDate=${VchDate}&VchNo=${ArrivalNo}&VchType=RP-RAW`;
    this.com.viewReport(url);
}
}
