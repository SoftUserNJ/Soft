import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-vouchers-approval',
  templateUrl: './vouchers-approval.component.html',
  styleUrls: ['./vouchers-approval.component.css']
})
export class VouchersApprovalComponent {

  constructor(
    private tostr: ToastrService,
    private fb: FormBuilder,
    private apiService: ApiService,
    private dp: DatePipe
  ) {
   
  }

  @ViewChild('voucherLists', { static: false }) voucherLists!: ElementRef;

  VoucherForm!: FormGroup;
  VoucherAppList: any = [];
  VoucherAppListFiltered : any = [];
  isShow = false;
  selectedRow: number | null = null;
  btnAdd:string = 'Add';
  voucherList: any[] = [];


  unapproved: boolean = true;
  approved: boolean = false;

  
  months = [
    { id: 1, name: 'January' },
    { id: 2, name: 'February' },
    { id: 3, name: 'March' },
    { id: 4, name: 'April' },
    { id: 5, name: 'May' },
    { id: 6, name: 'June' },
    { id: 7, name: 'July' },
    { id: 8, name: 'August' },
    { id: 9, name: 'September' },
    { id: 10, name: 'October' },
    { id: 11, name: 'November' },
    { id: 12, name: 'December' },
  ];



  ngOnInit() {

    this.formInit();
    this.getMonthYear();
    this.setCurrentDate();
  }

  formInit() {
    this.VoucherForm = this.fb.group({
      fromDate: [new Date()],
      toDate: [new Date()],
      apprvAll: [''],
      selectDate: [''],
      month: [undefined],
      vchType:[undefined],
      unapproved: [],
      approved: []

    });
  }

  onClickRefresh() {
    this.isShow = false;
    this.VoucherAppList = [];
    this.VoucherAppListFiltered = [];
    this.VoucherForm.get('vchType').reset();
  }


  getMonthYear() {
    this.apiService.getData('Payroll/GetMonthYear').subscribe((data) => {
      const year = data[0].year;
      this.VoucherForm.get('month').setValue(data[0].mnth);
  
      this.months = this.months.map((month) => ({
        id: month.id,
        name: `${month.name} - ${year}`
      }));
    });
  }
  
   setCurrentDate() {
    const today = new Date();
    this.VoucherForm.get('fromDate').setValue(new Date(today.getFullYear(), today.getMonth(), 1))  ;
    this.VoucherForm.get('toDate').setValue(new Date(today.getFullYear(), today.getMonth() + 1, 0)) ;
  }


  onClickFilter(event: any) {
    this.unapproved = false;
    this.approved = false;

    if (event.target.value == 'unapproved') {
      this.unapproved = true;
    } else if (event.target.value == 'approved') {
      this.approved = true;
    }
    this.searchGrid();
  }

  searchGrid() {
    if (this.approved) {
      
      this.VoucherAppListFiltered = this.VoucherAppList.filter(item => item.sent === true);
    } else if (this.unapproved) {
      
      this.VoucherAppListFiltered = this.VoucherAppList.filter(item => item.sent === false);
    } else {
      
      this.VoucherAppListFiltered = this.VoucherAppList;
    }
  }
  


  SelectVoucher(event: any): void {

    this.VoucherAppList = [];

    const obj = {
      type: event
    };

    this.apiService
      .getDataById('Payroll/GetVouchers', obj)
      .subscribe((data) => {

        if (!data || data.length === 0) {
          this.tostr.info("No Records Found");
          return;
        }
        
        data.forEach((item: any) => {

          let form = item;
          form.empy_id = item.empy_id;
          form.tumbid = item.tumbid;
          form.EmpName = item.EmpName;
          form.VchNo = item.VchNo;
          form.VchDate = this.dp.transform(item.VchDate, 'yyyy-MM-dd');
          form.Amount = item.Amount;
          form.Remarks = item.Remarks;
          form.Type = item.Type;
          form.sent = item.sent ?? false;

          this.VoucherAppList.push(form);
        });
        this.searchGrid();
      });
  }

  
  toggleApproveAll() {
    const isChecked = this.VoucherForm.get('apprvAll').value;
    this.VoucherAppListFiltered.forEach(item => {
        item.sent = isChecked;
    });
}


toggleDateRange() {
  const selectDateChecked = this.VoucherForm.get('selectDate').value;
  if(selectDateChecked)
  {
    const from = this.VoucherForm.get('fromDate').value;
    const to = this.VoucherForm.get('toDate').value;
  
    this.VoucherAppListFiltered = this.VoucherAppList.filter(item => {
      const vchDate = new Date(item.VchDate);
      return vchDate >= from && vchDate <= to;
    });

  }
  else{
  this.searchGrid();

}

}


onClickSave() {
    if (this.VoucherAppList.length == 0) {
      this.tostr.warning('Incomplete Transaction...');
      return;
    }


    const voucher: any[] = this.VoucherAppListFiltered.map((data) => ({
      SrNo: data.VchNo,
      EmpyId: data.empy_id,
      VchType: data.Type,
      IsApproved: data.sent

    }));

    this.apiService
      .saveData('Payroll/UpdateVoucherStatus', voucher)
      .subscribe((result) => {
        if (result == true || result == 'true') {
          this.tostr.success('Save Successfully');
          this.onClickRefresh();
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
    tds.forEach(td => {
      td.classList.add('HighLightRow');
    });
  
    // Remove 'HighLightRow' class from other rows
    const allRows = document.querySelectorAll('tr');
    allRows.forEach(row => {
      if (row !== clickedRow) {
        const otherRowTds = row.querySelectorAll('td');
        otherRowTds.forEach(td => {
          td.classList.remove('HighLightRow');
        });
      }
    });
  }

  updateSentStatus(event: any, data: any) {
    data.sent = event.target.checked;
  }
  
  
}
