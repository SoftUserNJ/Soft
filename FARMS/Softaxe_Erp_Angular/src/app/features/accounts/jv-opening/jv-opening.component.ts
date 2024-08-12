import { Component, ViewChild, ElementRef } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { DatePipe } from '@angular/common';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-jv-opening',
  templateUrl: './jv-opening.component.html',
  styleUrls: ['./jv-opening.component.css']
})

export class JvOpeningComponent {
  @ViewChild('voucherLists', { static: false }) voucherLists!: ElementRef;
  constructor(
    private apiService: ApiService,
    private fb: FormBuilder,
    private toast: ToastrService,
    private dp: DatePipe,
    private auth: AuthService,
    private com: CommonService,
  ) {
    const today = new Date();
    this.vchDate = today;
  }
  costCenter = localStorage.getItem('costCenter')

  isShowPage: boolean = true;
  vchDate: Date;
  addData: any[] = [];
  voucherList: any[] = [];
  filteredData: any[] = [];
  JobList: any[] = [];
  voucherForm!: FormGroup;
  acc_head: any = [];
  voucher_list: any[] = [];
  account: any;
  isEditMode: boolean = false;
  selectedRow: any;
  totalDebit: number = 0;
  totalCredit: number = 0;
  totalqty: number = 0;
  totalAmount: any;
  accountName: any;
  ngName = '';
  isRowEdit: boolean = false;
  isNew: boolean = false;
  private field1Changing = false;
  private field2Changing = false;
  isPrint: boolean = false;

  async ngOnInit() {
    this.getFinDate();
    this.getJVList();
    this.getAccountHead();
    this.formInit();
    this.inputDisable();
    this.calculateTotals();
    this.JobList = await this.com.getJobList(true);

  }

  openReportModal(item: any) {
    let d = item.VCHDATE.split('/');
    const fromDate = this.dp.transform(
      new Date(d[2], d[1] - 1, d[0]),
      'yyyy/MM/dd'
    );
    const toDate = this.dp.transform(
      new Date(d[2], d[1] - 1, d[0]),
      'yyyy/MM/dd'
    );

    let url = `PrintVoucherRangeWise?DateFrom=${fromDate}&DateTo=${toDate}&VchType=${
      item.VCHTYPE
    }&VchNoFrom=${item.VCHNO}&VchNoTo=${
      item.VCHNO
    }&CompId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}`;
    this.com.viewReport(url);
  }

  printReport() {
    const vchDate = this.voucherForm.get('vchDate')?.value;
    const vchNo = this.voucherForm.get('vchNo')?.value;

    const fromDate = this.dp.transform(vchDate, 'yyyy/MM/dd');
    const toDate = this.dp.transform(vchDate, 'yyyy/MM/dd');

    let url = `PrintVoucherRangeWise?DateFrom=${fromDate}&DateTo=${toDate}&VchType=JV-OP&VchNoFrom=${vchNo}&VchNoTo=${vchNo}&CompId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}`;
    this.com.viewReport(url);
  }

  formInit() {
    this.voucherForm = this.fb.group({
      sno: [0],
      vchNo: [0],
      vchDate: [this.vchDate],
      description: [''],
      account: [null],
      accountName: [''],
      debit: [null],
      credit: [null],
      qty: [''],
      status: [''],
      jobNo: [null],
    });

    this.voucherForm.get('vchDate')?.disable();
    this.voucherForm.get('vchNo')?.disable();
  }

  togglePages() {
    this.isShowPage = !this.isShowPage;
    if (this.isShowPage) {
      this.onClickRefresh();
      this.voucherForm.get('jobNo')?.patchValue(null);
    }
  }

  onChangeAccount(event: any) {
    this.accountName = event.name;
  }

  inputDisable() {
    this.voucherForm.get('debit')?.valueChanges.subscribe((value) => {
      if (!this.field1Changing) {
        this.field1Changing = true;

        if (value) {
          this.voucherForm.get('credit')?.setValue(0, { emitEvent: false });
          this.voucherForm.get('credit')?.disable();
        } else {
          this.voucherForm.get('credit')?.enable();
        }

        this.field1Changing = false;
      }
    });

    this.voucherForm.get('credit')?.valueChanges.subscribe((value) => {
      if (!this.field2Changing) {
        this.field2Changing = true;

        if (value) {
          this.voucherForm.get('debit')?.setValue(0, { emitEvent: false });
          this.voucherForm.get('debit')?.disable();
        } else {
          this.voucherForm.get('debit')?.enable();
        }

        this.field2Changing = false;
      }
    });
  }

  getFinDate(){
    this.apiService.getData('Accounts/GetFinDate').subscribe((data) => {
      this.voucherForm.get('vchDate').patchValue(data);
    });
  }

  getJVList() {
    const obj = {
      fromDate: '2000/01/01',
      toDate: '3000/01/01',
      vchType: "JV-OP",
    };
    this.apiService.getDataById('Accounts/GetJV', obj).subscribe((data) => {
      this.voucherList = data;
    });
  }

  getAccountHead() {
    this.apiService.getData('Accounts/GetAccountHead').subscribe((data) => {
      this.acc_head = data;
    });
  }

  calculateTotals() {
    this.totalDebit = this.voucher_list.reduce(
      (total: any, item: any) => total + (item.debit || 0),
      0
    );

    this.totalCredit = this.voucher_list.reduce(
      (total: any, item: any) => total + (item.credit || 0),
      0
    );

    this.totalqty = this.voucher_list.reduce(
      (total: any, item: any) => total + (item.qty || 0),
      0
    );
  }

  appendData(): void {
    let form = this.voucherForm.value;
    form.vchDate = this.dp.transform(form.vchDate, 'dd/MM/yyyy');

    if (form.credit == null || form.credit === '') {
      form.credit = 0;
    }

    if (form.debit == null || form.debit === '') {
      form.debit = 0;
    }

    if (form.vchNo == null || form.vchNo === '') {
      form.vchNo = 0;
    }

    if (form.account == '' || form.account == null) {
      this.toast.warning('Select Account Head...');
      return;
    }

    if (form.description == '' || form.description == null) {
      this.toast.warning('Enter Description...');
      return;
    }
    if (
      (form.credit == null || form.credit == '') &&
      (form.debit == null || form.debit == '')
    ) {
      this.toast.warning('Enter Credit/Debit...');
      return;
    }

    form.accountName = this.acc_head.find((x) => x.code == form.account).name;
    const jobName = this.JobList.find((x) => x.ID == form.jobNo)
    form.jobName = (jobName != undefined) ? jobName.NAME : '';

    if (this.isRowEdit) {
      const index = this.voucher_list.findIndex((row) => row.sno === form.sno);
      if (index !== -1) {
        this.voucher_list[index] = form;
        this.isRowEdit = false;
        this.resetForm();
        this.calculateTotals();
        return;
      }
    }

    form.sno = this.voucher_list.length + 1;
    this.voucher_list.push(form);

    this.resetForm();
    this.calculateTotals();
  }

  editRow(row: any) {
    this.voucherForm.get('sno')?.patchValue(row.sno);
    this.voucherForm.get('account')?.patchValue(row.account);
    this.voucherForm.get('description')?.patchValue(row.description);
    this.voucherForm.get('jobNo')?.patchValue(row.jobNo);
    this.voucherForm.get('credit')?.patchValue(row.credit);
    this.voucherForm.get('debit')?.patchValue(row.debit);
    this.voucherForm.get('qty')?.patchValue(row.qty);
    this.accountName = row.accountName;
    this.isRowEdit = true;
  }

  removeRow(row: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (!confirmDelete) {
      return;
    }

    const indexToRemove = this.voucher_list.findIndex(
      (item) => item.account === row.account
    );
    if (indexToRemove !== -1) {
      this.voucher_list.splice(indexToRemove, 1);
    }

    this.calculateTotals();
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

  resetForm() {
    this.voucherForm.get('sno')?.patchValue(0);
    this.voucherForm.get('account')?.patchValue(undefined);
    this.voucherForm.get('description')?.patchValue('');
    this.voucherForm.get('debit')?.patchValue('');
    this.voucherForm.get('credit')?.patchValue('');
    this.voucherForm.get('qty')?.patchValue('');
    this.isRowEdit = false;
  }

  saveJV(): void {

    let result = this.com.isStopEntry("JV-OP");
    if(!result){
      this.toast.info("You are not allowed")
      return;
    }
    
    if (this.voucher_list.length == 0) {
      this.toast.warning('Add Voucher First...');
      return;
    }
    
    try {
      this.com.showLoader();

      const voucher: any[] = this.voucher_list.map((data) => ({
        vchNo: this.voucherForm.get('vchNo')?.value,
        vchType: "JV-OP",
        vchDate: this.dp.transform(
          this.voucherForm.get('vchDate')?.value,
          'yyyy-MM-dd'
        ),
        description: data.description,
        account: data.account,
        debit: data.debit.toString(),
        credit: data.credit.toString(),
        qty: data.qty == ''  || data.qty == null ? 0 : data.qty,
        jobNo: data.jobNo ?? 0,
        jobName: data.jobName,
        status: ((this.isNew == true) ? 'new' : 'edit'),
        dtNow: new Date(),
      }));

      this.apiService
        .saveData('Accounts/SaveUpdateJV', voucher)
        .subscribe((r) => {
          if (r.status == true || r.status == 'true') {
            this.toast.success('Save Successfully');
            this.voucherForm.get('vchNo')?.patchValue(r.vchNo);
            this.getJVList();
            this.isPrint = true;
            this.isNew = false;
            this.com.hideLoader();
            //this.onClickRefresh();
          } else {
            this.com.hideLoader();
            this.toast.error('Please Save Again');
          }
        });
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  editJV(vchNo: any): void {

    let result = this.com.isStopEntry("JV-OP");
    if(!result){
      this.toast.info("You are not allowed")
      return;
    }

    this.isDisabled = false;
    this.isNewClick = true;
    this.isNew = false;

    try {
      this.com.showLoader();

      const obj = {
        vchNo: vchNo,
        vchType: "JV-OP",
      };

      this.apiService.getDataById('Accounts/EditJV', obj).subscribe((data) => {
        data.forEach((item: any) => {
          this.voucherForm
            .get('vchDate')
            ?.patchValue(
              new Date(
                item.VCHDATE.split('/')[2],
                item.VCHDATE.split('/')[1] - 1,
                item.VCHDATE.split('/')[0]
              )
            );
          this.voucherForm.get('vchNo')?.patchValue(item.VCHNO);
          let form = this.voucherForm.value;
          form.sno = this.voucher_list.length + 1;
          (form.description = item.DESCRP),
            (form.account = item.CODE),
            (form.accountName = item.NAMES),
            (form.jobNo = item.JOBID),
            (form.jobName = item.JOBNAME),
            (form.debit = item.DEBIT),
            (form.credit = item.CREDIT),
            (form.qty = item.QTY),
            this.voucher_list.push(form);
        });
        this.calculateTotals();
        this.togglePages();
        this.isPrint = true;
        this.com.hideLoader();
      });
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  deleteJV(VCHNO: any, VCHTYPE: any): void {

    let result = this.com.isStopEntry(VCHTYPE);
    if(!result){
      this.toast.info("You are not allowed")
      return;
    }

    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (!confirmDelete) {
      return;
    }

    try {
      this.com.showLoader();
      const obj = {
        vchNo: VCHNO,
        vchType: VCHTYPE,
        dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      };

      this.apiService.deleteData('accounts/DeleteJV', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.toast.success('Delete Successfully');
            this.getJVList();
            this.com.hideLoader();
          } else if (data == 'false' || data == false) {
            this.com.hideLoader();
            this.toast.error('Delete Again');
          }
        },
        error: (error) => {
          this.com.hideLoader();
          this.toast.info(error.error.text);
        },
      });
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  isDisabled: boolean = true;
  isNewClick = false;

  onClickRefresh() {
    this.isDisabled = true;
    this.isNewClick = false;
    this.isNew = false;
    this.isPrint = false;
    this.resetForm();
    this.voucher_list = [];
    this.calculateTotals();
    this.voucherForm.get('sno')?.patchValue(0);
    this.voucherForm.get('vchNo')?.patchValue(0);
    this.voucherForm.get('account')?.patchValue(undefined);
  }

  onClickNew() {

    let result = this.com.isStopEntry("JV-OP");
    if(!result){
      this.toast.info("You are not allowed")
      return;
    }

    this.isDisabled = false;
    this.isNewClick = true;
    this.isNew = true;
    this.getMaxNumber();
  }

  getMaxNumber() {
    this.apiService
      .getDataById('accounts/GetMaxNumber', { vchType: 'JV-OP' })
      .subscribe((data) => {
        this.voucherForm.get('vchNo')?.patchValue(data[0].VCHNO);
      });
  }

  onClickUpdateBs(){
    try {
      this.com.showLoader();

      this.apiService
        .saveObj('Accounts/UpdateBalanceSheet', {})
        .subscribe((r) => {
          if (r == true || r == 'true') {
            this.toast.success('Balance Update Successfully');
            this.com.hideLoader();
            //this.onClickRefresh();
          } else {
            this.com.hideLoader();
            this.toast.error('Please Save Again');
          }
        });
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  export(fileName: any, formate: any) {
    this.com.ExportFiles(fileName, formate);
  }
  
}