import { DatePipe } from '@angular/common';
import {
  Component,
  ElementRef,
  Input,
  SimpleChanges,
  ViewChild,
} from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';
import { environment } from 'src/environment/environmemt';
declare const $:any;

@Component({
  selector: 'app-file-attachment',
  templateUrl: './file-attachment.component.html',
  styleUrls: ['./file-attachment.component.css'],
})
export class FileAttachmentComponent {
  @Input() code: any = '';
  @Input() name: any = '';
  @Input() fromDate: any;
  @Input() toDate: any;
  @Input() jobNo: any;

  basePath = environment.basePath;
  cmpId = localStorage.getItem('cmpId');
  cmpImage: any = localStorage.getItem('Logo');
  cmpName: any = localStorage.getItem('CmpName');
  cmpLogo: string = '';

  // LEDGER
  @ViewChild('ledgerLists', { static: false }) ledgerLists!: ElementRef;
  fromDate2: Date;
  toDate2: Date;
  ledgerList: any[] = [];
  accountCode: any;
  accountName: any;
  runningTotal: number = 0;
  searchLedger: string = '';

  // ATTACHMENT
  srcPdfImg: any;
  file: File | null = null;
  row: any;
  fileList: any;
  isPdf: boolean = false;

  constructor(
    private apiService: ApiService,
    private toastr: ToastrService,
    private dp: DatePipe,
    private com: CommonService,
    private auth: AuthService
  ) {
    const today = new Date();
    this.fromDate2 = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate2 = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.code || changes.name) {
      this.fromDate2 = this.fromDate ?? this.fromDate2;
      this.toDate2 = this.toDate ?? this.toDate2;
      this.ledgerList = [];
      this.showLedger(this.code, this.name);
    }
  }

  async ngOnInit() {
    this.cmpLogo = `${this.basePath}/Companies/${this.cmpName}/CompanyLogo/${this.cmpImage}`;
  }

  // LEDGER

  showLedger(code: any, name: any) {
    this.accountCode = code;
    this.accountName = name;
    this.onClickLedger(code);
  }

  async onClickLedger(code: any) {
    const obj = {
      fromDate: this.dp.transform(this.fromDate2, 'yyyy/MM/dd'),
      toDate: this.dp.transform(this.toDate2, 'yyyy/MM/dd'),
      account: code,
      jobNo: this.jobNo ?? '%'
    };
    const data = await this.apiService
      .getDataById('Dashboard/GetLedger', obj)
      .toPromise();
    this.searchLedger = '';
    let balance = 0;
    data.forEach((x) => {
      if (x.VchType != 'CB') {
        balance +=
          parseFloat(x.Balance) + parseFloat(x.Debit) - parseFloat(x.Credit);
        x.Balance = balance;
      }
    });
    this.ledgerList = data;
  }

  onSearchLedger(): void {
    const tableElement = this.ledgerLists.nativeElement;
    const rows = tableElement.querySelectorAll('tr');

    rows.forEach((row: HTMLTableRowElement) => {
      let isShow = true;

      if (
        row.textContent &&
        row.textContent.toLowerCase().indexOf(this.searchLedger.toLowerCase()) >
          -1
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

  // VOUCHER
  openReportVoucher(i: any){
    let date = this.dp.transform(i.VchDate, "yyyy/MM/dd")
    let url = `PrintVoucherRangeWise?DateFrom=${date}&DateTo=${date}&VchType=${
      i.VchType.split("-")[0]
    }&VchNoFrom=${i.VchNo}&VchNoTo=${
      i.VchNo
    }&CompId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${i.VchType.split("-")[1]}`;
    this.com.viewReport(url)
  }


  // FILE ATTACHMENT
  onClickModel(item: any) {
    $(".myClose").click();
    
    this.row = item;
    this.onRefreshModel();
    this.getFile(item);
  }

  onClickCloseAtt(){
    $("#LedgerModal").modal('show');
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
    formData.append('vchNo', item.VchNo.toString());
    formData.append('vchType', item.VchType.split("-")[0]);
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
      vchType: item.VchType.split("-")[0],
      vchNo: item.VchNo,
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
}