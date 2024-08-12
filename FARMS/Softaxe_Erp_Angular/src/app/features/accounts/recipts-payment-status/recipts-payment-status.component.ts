import { Component, ElementRef, ViewChild } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';
import { DatePipe } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { environment } from '../../../../environment/environmemt';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-recipts-payment-status',
  templateUrl: './recipts-payment-status.component.html',
  styleUrls: ['./recipts-payment-status.component.css'],
})
export class ReciptsPaymentStatusComponent {
  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;

  fromDate: Date;
  toDate: Date;

  typesList = [
    { vlaue: 'CP', name: 'CP' },
    { vlaue: 'CP-FREIGHT', name: 'CP-FREIGHT' },
    { vlaue: 'CR', name: 'CR' },
    { vlaue: 'BP', name: 'BP' },
    { vlaue: 'BR', name: 'BR' },
  ];

  basePath = environment.basePath;

  partyList: any[] = [];
  bankCashList: any[] = [];
  groupList: any[] = [];
  companyList: any[] = [];
  voucherList: any[] = [];
  tax: number = 0;
  bankReceived: number = 0;
  bankPayment: number = 0;
  cashReceived: number = 0;
  cashPayment: number = 0;
  creditReceived: number = 0;
  creditPayment: number = 0;
  groupId: number = 0;
  companyId: number = 0;
  party = '';
  bankCash = '';
  type = '';
  cmpDisable: boolean = true;
  isMulti: boolean = true;
  srcPdfImg: any;
  file: File | null = null;
  row: any;
  fileList: any;
  isPdf: boolean = false;
  cmpId = localStorage.getItem('cmpId');

  @ViewChild('PRStatus', { static: false }) PRStatus!: ElementRef;
  constructor(
    private apiService: ApiService,
    private dp: DatePipe,
    private toastr: ToastrService,
    private auth: AuthService,
    private com: CommonService
  ) {
    const today = new Date();
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  }

  ngOnInit() {
    this.getGroupList();
    this.getPartyList();
    this.getBankCashList();
  }

  openReportModal(item: any) {
    var dp = item.VCHDATE.split('/');
    const date = this.dp.transform(
      new Date(dp[2], dp[1] - 1, dp[0]),
      'yyyy/MM/dd'
    );

    let url = `PrintVoucherRangeWise?DateFrom=${date}&DateTo=${date}&VchType=${
      item.VCHTYPE
    }&VchNoFrom=${item.VCHNO}&VchNoTo=${
      item.VCHNO
    }&CompId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}`;
    this.com.viewReport(url)
   
  }

  onClickModel(item: any) {
    this.row = item;
    this.onRefreshModel();
    this.getFile(item);
  }

  onFileSelected(event: any) {
    this.file = event.target.files[0];
    if (this.file && this.file.type === 'application/pdf') {
      this.isPdf = true;
    } else {
      this.isPdf = false;
    }

    this.srcPdfImg = URL.createObjectURL(event.target.files[0]);
  }

  onClickUpload() {
    let item = this.row;

    let formData = new FormData();
    formData.append('vchNo', item.VCHNO.toString());
    formData.append('vchType', item.VCHTYPE);
    formData.append('file', this.file);

    this.apiService
      .saveData('Accounts/FileUpload', formData)
      .subscribe((result) => {
        if (result == true || result == 'true') {
          this.toastr.success('Save Successfully');
          this.getFile(item);
          this.onRefreshModel();
        } else {
          this.toastr.error('Please Save Again');
        }
      });
  }

  getFile(item: any) {
    var obj = {
      vchType: item.VCHTYPE,
      vchNo: item.VCHNO,
    };

    this.apiService
      .getDataById('Accounts/GetFiles', obj)
      .subscribe((result) => {
        if (result.length == 0) {
          this.fileList = [];
          return;
        }
        this.fileList = result;
        this.onViewFile(result[0])
      });
  }

  onRemoveFile(item: any) {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (!confirmDelete) {
      return;
    }

    var obj = {
      name: item.name,
      path: item.path,
    };

    this.apiService
      .deleteData('Accounts/DeleteFile', obj)
      .subscribe((result) => {
        if (result == true || result == 'true') {
          this.getFile(this.row);
          this.onRefreshModel();
          this.toastr.success('Delete Successfully');
        } else {
          this.toastr.error('Please Delete Again');
        }
      });
  }

  onRefreshModel() {
    this.file = null;
    this.isPdf = false;
    this.srcPdfImg = '';
  }

  onViewFile(item: any) {
    let ext = item.name.substring(item.name.lastIndexOf('.') + 1);

    if (ext.toLowerCase() == 'pdf') {
      this.isPdf = true;
    } else {
      this.isPdf = false;
    }

    this.srcPdfImg = this.basePath + '/' + item.path + '/' + item.name;
  }

  getPartyList() {
    this.apiService.getData('Accounts/GetPRParty').subscribe((data) => {
      this.partyList = data;
    });
  }

  getBankCashList() {
    this.apiService.getData('Accounts/GetPRPBankCash').subscribe((data) => {
      this.bankCashList = data;
    });
  }

  getGroupList() {
    this.apiService.getData('Accounts/GetGroup').subscribe((data) => {
      this.groupList = data;
      this.groupId = this.groupList[0].id;
      this.isMulti = this.groupList[0].IsMulti;
      this.getComanyList();
    });
  }

  getComanyList() {
    this.apiService
      .getDataById('Accounts/GetCompany', { groupId: this.groupId })
      .subscribe((data) => {
        this.companyList = data;
        this.companyId = parseInt(this.cmpId);
        
        if (this.companyList.length > 1 && this.isMulti == true) {
          this.cmpDisable = false;
        }

        this.getVoucherList();
      });
  }

  getVoucherList() {
    try {
      this.com.showLoader();
      const obj = {
        fromDate: this.dp.transform(this.fromDate, 'yyyy/MM/dd'),
        toDate: this.dp.transform(this.toDate, 'yyyy/MM/dd'),
        companyId: this.companyId,
      };

      this.apiService
        .getDataById('Accounts/GetPaymentReceipts', obj)
        .subscribe((data) => {
          this.voucherList = data;
          console.log(data)
          setTimeout(() => {
            this.searchGrid();
          }, 100);
          this.com.hideLoader();
        });
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  onChangeParty(event: any) {
    this.party = event.code;
    this.searchGrid();
  }

  onChangeBankCash(event: any) {
    this.bankCash = event.code;
    this.searchGrid();
  }

  onChangeTypes(event: any) {
    this.type = event.vlaue;
    this.searchGrid();
  }

  onClearParty() {
    this.party = '';
    this.searchGrid();
  }

  onClearBakCash() {
    this.bankCash = '';
    this.searchGrid();
  }

  onClearTypes() {
    this.type = '';
    this.searchGrid();
  }

  searchGrid(): void {
    const tableElement = this.PRStatus.nativeElement;
    const rows = tableElement.querySelectorAll('tr');

    let tax = 0;
    let bankReceived = 0;
    let bankPayment = 0;
    let cashReceived = 0;
    let cashPayment = 0;
    let creditReceived = 0;
    let creditPayment = 0;

    rows.forEach((row: HTMLTableRowElement) => {
      let isShow = true;

      if (
        row.querySelector('.partyCode')?.textContent != this.party &&
        this.party.length > 0
      ) {
        isShow = false;
      }

      if (
        row.querySelector('.bcCode')?.textContent != this.bankCash &&
        this.bankCash.length > 0
      ) {
        isShow = false;
      }

      if (
        row.querySelector('.vchType')?.textContent != this.type &&
        this.type.length > 0
      ) {
        isShow = false;
      }

      if (isShow) {
        tax += parseFloat(
          row.querySelector('.tax')?.textContent!.replace(/,/g, '')
        );
        bankReceived += parseFloat(
          row.querySelector('.BR')?.textContent!.replace(/,/g, '')
        );
        bankPayment += parseFloat(
          row.querySelector('.BP')?.textContent!.replace(/,/g, '')
        );
        cashReceived += parseFloat(
          row.querySelector('.CR')?.textContent!.replace(/,/g, '')
        );
        cashPayment += parseFloat(
          row.querySelector('.CP')?.textContent!.replace(/,/g, '')
        );
        creditReceived += parseFloat(
          row.querySelector('.CreditCardR')?.textContent!.replace(/,/g, '')
        );
        creditPayment += parseFloat(
          row.querySelector('.CreditCardP')?.textContent!.replace(/,/g, '')
        );
        row.style.display = '';
      } else {
        row.style.display = 'none';
      }
    });

    this.tax = tax;
    this.bankReceived = bankReceived;
    this.bankPayment = bankPayment;
    this.cashReceived = cashReceived;
    this.cashPayment = cashPayment;
    this.creditReceived = creditReceived;
    this.creditPayment = creditPayment;
  }
  
  export(fileName: any, formate: any) {
    this.com.ExportFiles(fileName, formate);
  }
}
