import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-freight-payments',
  templateUrl: './freight-payments.component.html',
  styleUrls: ['./freight-payments.component.css'],
})
export class FreightPaymentsComponent {
  constructor(
    private fb: FormBuilder,
    private tostr: ToastrService,
    private apiService: ApiService,
    private dp: DatePipe
  ) {
    const today = new Date();
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  }

  //List Page
  @ViewChild('voucherLists', { static: false }) voucherLists!: ElementRef;

  voucherList: any[] = [];
  fromDate: Date;
  toDate: Date;

  //Form Page
  FreightPaymentsForm!: FormGroup;
  isShow = false;
  isReadOnly: boolean = true;
  editMode: boolean = true;
  isShowPage: boolean = true;
  btnAdd: string = 'Add';

  detailsList: any = [];

  mainAccount: any = [];
  subAccount: any = [];

  ngOnInit() {
    this.formInit();
    this.getPaymentList();
    this.getVchFec('');
    this.getAccountMain();
  }

  formInit() {
    this.FreightPaymentsForm = this.fb.group({
      vchDate: [new Date()],
      fromDate1: [this.fromDate],
      toDate1: [this.toDate],
    });
  }

  resetForm() {
    // this.FreightPaymentsForm.get('sample1').patchValue('');
    // this.FreightPaymentsForm.get('sample2').patchValue('');
    // this.FreightPaymentsForm.get('sample3').patchValue('');
    // this.FreightPaymentsForm.get('VehNo').patchValue('');
    // this.FreightPaymentsForm.get('arrvNo').patchValue('');
    // this.FreightPaymentsForm.get('BagsIn').patchValue('');
    // this.FreightPaymentsForm.get('remarks').patchValue('');
  }

  getPaymentList() {
    // const obj = {
    //   fromDate: this.dp.transform(this.fromDate, 'yyyy-MM-dd'),
    //   toDate: this.dp.transform(this.toDate, 'yyyy-MM-dd'),
    // };
    // this.apiService
    //   .getDataById('Purchase/GetLabsList', obj)
    //   .subscribe((data) => {
    //     this.voucherList = data;
    //   });
  }

  getAccountMain() {
    this.apiService.getData('Common/GetLevel4').subscribe((data) => {
      this.mainAccount = data;
    });
  }

  getAccountSub(event: any) {
    this.apiService
      .getDataById('Common/GetLevel5CodeNameByL4Code', { code: event.CODE })
      .subscribe((data) => {
        this.subAccount = data;
      });
  }

  getVchFec(tag: any) {
    const form = this.FreightPaymentsForm.value;
    const obj = {
      vchDate: this.dp.transform(form.vchDate, 'yyyy-MM-dd'),
      fromDate: this.dp.transform(form.fromDate1, 'yyyy-MM-dd'),
      toDate: this.dp.transform(form.toDate1, 'yyyy-MM-dd'),
      tag: tag,
    };

    this.apiService.getDataById('Accounts/GetVchFec', obj).subscribe((data) => {
      debugger;
      this.detailsList = data;
    });
  }

  onActionClick(event: any, tag) {
    debugger;
  }

  onClickSave() {
    // const form = this.FreightPaymentsForm.value;
    // if (!form.arrvNo) {
    //   this.tostr.warning('Select Arrvial First....!');
    //   return;
    // }
    // let LabTestNo = this.editMode ? this.FreightPaymentsForm.get('labNo')?.value : 0;
    // let visuallyAccepted: boolean = false;
    // let visuallyRejected: boolean = false;
    // const voucher = {
    //   LabTestNo: LabTestNo,
    //   ResDate: form.Vchdate,
    //   BagsIn: form.BagsIn,
    //   VisAcc: visuallyAccepted,
    //   VisRej: visuallyRejected,
    //   ArrivalNo: form.arrvNo,
    //   VehicleNo: form.VehNo,
    //   Remarks: form.remarks,
    //   Test1: form.sample1,
    //   Test2: form.sample2,
    //   Test3: form.sample3,
    // }
    // this.apiService
    //   .saveData('Purchase/SaveLabFirstSample', voucher)
    //   .subscribe((result) => {
    //     if (result == true || result == 'true') {
    //       this.tostr.success('Save Successfully');
    //       this.onClickRefresh();
    //       this.getPaymentList();
    //     } else {
    //       this.tostr.error('Please Save Again');
    //     }
    //   });
  }

  async editLab(LabTestNo: any) {
    this.onClickRefresh();
    this.isShow = true;
    this.editMode = true;

    const obj = {
      LabTestNo: LabTestNo,
    };

    await this.apiService
      .getDataById('Purchase/GetEditLab', obj)
      .subscribe((data) => {
        this.togglePages();
        data.forEach((item: any) => {
          this.FreightPaymentsForm.patchValue({
            Vchdate: item.ResDate,
            labNo: item.LabTestNo,
            arrvNo: item.ArrivalNo,
            vchType: item.VchType,
            BagsIn: item.BagsIn,
            VehNo: item.VehicleNo,
            sample1: item.Test1,
            sample2: item.Test2,
            sample3: item.Test3,
            remarks: item.Remarks,
          });
        });
      });
  }

  deleteLab(LabTestNo: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {
      const obj = {
        LabTestNo: LabTestNo,
      };

      this.apiService.deleteData('Purchase/DelLab', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.tostr.success('Delete Successfully');
            this.getPaymentList();
          } else if (data == 'false' || data == false) {
            this.tostr.error('Delete Again');
          }
        },
        error: (error) => {
          this.tostr.info(error.error.text);
        },
      });
    }
  }

  onClickNew() {
    this.isShow = true;
    this.editMode = false;
  }

  onClickRefresh() {
    this.isShow = false;
    this.resetForm();
  }

  togglePages() {
    this.isShowPage = !this.isShowPage;
    if (this.isShowPage) {
      this.onClickRefresh();
    }
  }

  searchGrid(event: any): void {
    const tableElement = this.voucherLists.nativeElement;
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
