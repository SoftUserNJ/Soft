import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { environment } from '../../../../environment/environmemt';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-post-dated-cheque',
  templateUrl: './post-dated-cheque.component.html',
  styleUrls: ['./post-dated-cheque.component.css'],
})
export class PostDatedChequeComponent {
  basePath = environment.basePath;
  pdChequeList: any[] = [];
  pdBank: any[] = [];
  pdParty: any[] = [];
  searchBank = '';
  searchParty = '';
  all: boolean = true;
  today: boolean = false;
  deposit: boolean = false;
  cleared: boolean = false;
  bounced: boolean = false;
  fromDate: Date;
  toDate: Date;
  dpDate: Date;
  srcPdfImg: any;
  file: File | null = null;
  row: any;
  fileList: any;
  isPdf: boolean = false;

  @ViewChild('pdChequeLists', { static: false })
  private pdChequeLists!: ElementRef;
  constructor(
    private apiService: ApiService,
    private toast: ToastrService,
    private dp: DatePipe,
    private toastr: ToastrService,
    private com: CommonService
  ) {
    const today = new Date();
    this.dpDate = today;
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  }

  ngOnInit() {
    this.getPDChequeList();
    this.getPDBank();
    this.getPDParty();
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
    let item = this.row.TYPENO.split('-');

    let formData = new FormData();
    formData.append('vchNo', item[1].toString());
    formData.append('vchType', item[0]);
    formData.append('file', this.file);

    this.apiService
      .saveData('Accounts/FileUpload', formData)
      .subscribe((result) => {
        if (result == true || result == 'true') {
          this.toastr.success('Save Successfully');
          this.getFile(this.row);
          this.onRefreshModel();
        } else {
          this.toastr.error('Please Save Again');
        }
      });
  }

  getFile(item: any) {
    var obj = {
      vchType: item.TYPENO.split('-')[0],
      vchNo: item.TYPENO.split('-')[1],
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

  getPDBank() {
    this.apiService.getData('Sale/GetPDBank').subscribe((data) => {
      this.pdBank = data;
    });
  }

  getPDParty() {
    this.apiService.getData('Sale/GetPDParty').subscribe((data) => {
      this.pdParty = data;
    });
  }

  onDepositChange(item: any) {
    if (item.DEPOSIT) {
      item.CLEARED = false;
      item.BOUNCED = false;
    }
  }

  onClearedChange(item: any) {
    if (item.CLEARED) {
      item.DEPOSIT = false;
      item.BOUNCED = false;
    }
  }

  onBouncedChange(item: any) {
    if (item.BOUNCED) {
      item.DEPOSIT = false;
      item.CLEARED = false;
    }
  }

  getPDChequeList() {
    try {
      this.com.showLoader();

      const status = this.getSelectedRadio();
      const obj = {
        fromDate:
          status === 'today'
            ? this.dp.transform(this.dpDate, 'yyyy/MM/dd')
            : this.dp.transform(this.fromDate, 'yyyy/MM/dd'),
        toDate:
          status === 'today'
            ? this.dp.transform(this.dpDate, 'yyyy/MM/dd')
            : this.dp.transform(this.toDate, 'yyyy/MM/dd'),
        status: status,
      };
      this.apiService
        .getDataById('Sale/GetPDChequeList', obj)
        .subscribe((data) => {
          this.pdChequeList = data;
          setTimeout(() => {
            this.searchGrid();
          }, 300);
          this.com.hideLoader();
        });
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  savePDCheque() {
    try {
      this.com.showLoader();
      const pdCheque: any[] = this.pdChequeList.map((data: any) => ({
        vchtype: data.TYPENO.split('-')[0],
        vchno: data.TYPENO.split('-')[1],
        deposit: data.DEPOSIT,
        cleared: data.CLEARED,
        bounced: data.BOUNCED,
      }));

      this.apiService
        .saveData('Sale/SavePDCheque', pdCheque)
        .subscribe((result) => {
          if (result == true || result == 'true') {
            this.toast.success('Save Successfully');
            this.getPDChequeList();
            this.com.hideLoader();
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

  onClickFilter(event: any) {
    this.all = false;
    this.today = false;
    this.deposit = false;
    this.cleared = false;
    this.bounced = false;

    if (event.target.value == 'all') {
      this.all = true;
    } else if (event.target.value == 'today') {
      this.today = true;
    } else if (event.target.value == 'deposit') {
      this.deposit = true;
    } else if (event.target.value == 'cleared') {
      this.cleared = true;
    } else if (event.target.value == 'bounced') {
      this.bounced = true;
    }
    this.getPDChequeList();
  }

  public getSelectedRadio(): string {
    if (this.all) {
      return 'all';
    } else if (this.today) {
      return 'today';
    } else if (this.deposit) {
      return 'deposit';
    } else if (this.cleared) {
      return 'cleared';
    } else if (this.bounced) {
      return 'bounced';
    } else {
      return '';
    }
  }

  onChangeBank(event: any) {
    this.searchBank = event.NAMES;
    this.searchGrid();
  }

  onChangeParty(event: any) {
    this.searchParty = event.NAMES;
    this.searchGrid();
  }

  onClearBank() {
    this.searchBank = '';
    this.searchGrid();
  }

  onClearParty() {
    this.searchParty = '';
    this.searchGrid();
  }

  searchGrid() {
    const tableElement = this.pdChequeLists.nativeElement;
    const rows = tableElement.querySelectorAll('tr');

    rows.forEach((row: HTMLTableRowElement) => {
      let isShow = true;

      if (
        row.querySelector('.bank')?.textContent != this.searchBank &&
        this.searchBank.length > 0
      ) {
        isShow = false;
      }

      if (
        row.querySelector('.party')?.textContent != this.searchParty &&
        this.searchParty.length > 0
      ) {
        isShow = false;
      }

      if (isShow) {
        row.style.display = '';
      } else {
        row.style.display = 'none';
      }
    });
  }
  export(fileName: any, formate: any) {
    this.com.ExportFiles(fileName, formate);
  }
}
