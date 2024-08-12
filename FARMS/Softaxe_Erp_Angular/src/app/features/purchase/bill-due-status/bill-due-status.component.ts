import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { environment } from '../../../../environment/environmemt';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-bill-due-status',
  templateUrl: './bill-due-status.component.html',
  styleUrls: ['./bill-due-status.component.css'],
})
export class BillDueStatusComponent {
  @ViewChild('invoice') invoice!: ElementRef<HTMLInputElement>;

  basePath = environment.basePath;
  all: boolean = true;
  pending: boolean = false;
  overdue: boolean = false;
  fromDate: Date;
  toDate: Date;
  invoiceList: any[] = [];
  groupList: any[] = [];
  companyList: any[] = [];
  groupId: number = 0;
  companyId: number = 0;
  cmpDisable: boolean = true;
  isMulti: boolean = true;
  search = '';
  totalDisAmt: number = 0;
  totalOtherCredit: number = 0;
  totalNetDue: number = 0;
  totalRecAmount: number = 0;
  totalBalance: number = 0;
  srcPdfImg: any;
  file: File | null = null;
  row: any;
  fileList: any[] = [];
  recAmountList: any[] = [];
  isPdf: boolean = false;
  totalRecAmt: any;

  locationList: any[] = [];
  isDisableLoc: boolean = false;
  locId: any = null;
  cmpId = localStorage.getItem('cmpId');
  distributionPos = localStorage.getItem('distributionPos');

  constructor(
    private apiService: ApiService,
    private datePipe: DatePipe,
    private toastr: ToastrService,
    private auth: AuthService,
    private com: CommonService
  ) {
    const today = new Date();
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  }

  onViewReport(item: any) {
    const dPart = item.INVDATE.split('/');
    const vchDate = this.datePipe.transform(
      new Date(dPart[2], dPart[1] - 1, dPart[0]),
      'yyyy/MM/dd'
    );
    let url = `PurchaseInvoice?VchNoFrom=${item.INVOICENO}&VchNoTo=${
      item.INVOICENO
    }&VchType=${item.VCHTYPE}&fromDate=${vchDate}&toDate=${vchDate}&CmpId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}`;
    this.com.viewReport(url);
  }

  async ngOnInit() {
    await this.getGroupList();

    this.locationList = await this.com.getLocation();
    if(this.distributionPos != "ERP"){
      if (this.auth.locId() == 'HO') {
        this.locId = this.auth.locId();
        this.isDisableLoc = false;
      } else {
        this.locId = this.auth.locId();
        this.isDisableLoc = true;
      }
    }

    await this.getInvoice();
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
    formData.append('vchNo', item.INVOICENO.toString());
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
    const obj = {
      vchType: item.VCHTYPE,
      vchNo: item.INVOICENO,
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
    this.fileList = [];
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

  onClickRecModal(item: any) {
    const obj = {
      invoiceNo: item.INVOICENO,
      vchType: 'PI',
    };

    this.apiService
      .getDataById('Purchase/GetRecAmount', obj)
      .subscribe((result) => {
        this.recAmountList = result;

        this.totalRecAmt = result.reduce(
          (total, item) => total + item.RECAMOUNT,
          0
        );
        this.onClickAmountRow(this.recAmountList[0]);
      });
  }

  onClickAmountRow(item: any) {
    this.onRefreshModel();

    const obj = {
      vchType: item.VCHTYPE,
      vchNo: item.VCHNO,
    };

    this.apiService
      .getDataById('Accounts/GetFiles', obj)
      .subscribe((result) => {
        if (result.length == 0) {
          return;
        }
        this.onViewFile(result[0]);
      });
  }

  async getGroupList() {
    const data = await this.apiService.getData('Accounts/GetGroup').toPromise();

    this.groupList = data;
    this.groupId = this.groupList[0].id;
    this.isMulti = this.groupList[0].IsMulti;

    await this.getComanyList();
  }

  async getComanyList() {
    const data = await this.apiService
      .getDataById('Accounts/GetCompany', { groupId: this.groupId })
      .toPromise();
    this.companyList = data;

    if (this.companyList.length > 1 && this.isMulti == true) {
      this.cmpDisable = false;
    }

    this.companyId = parseInt(this.cmpId);
  }

  onClickFilter(event: any) {
    this.all = false;
    this.pending = false;
    this.overdue = false;

    if (event.target.value == 'all') {
      this.all = true;
    } else if (event.target.value == 'pending') {
      this.pending = true;
    } else if (event.target.value == 'overdue') {
      this.overdue = true;
    }
    this.getInvoice();
  }

  public getSelectedRadio(): string {
    if (this.all) {
      return 'all';
    } else if (this.pending) {
      return 'pending';
    } else if (this.overdue) {
      return 'overdue';
    } else {
      return '';
    }
  }

  async getInvoice() {
    try {
      this.com.showLoader();
      const obj = {
        fromDate: this.datePipe.transform(this.fromDate, 'yyyy/MM/dd'),
        toDate: this.datePipe.transform(this.toDate, 'yyyy/MM/dd'),
        status: this.getSelectedRadio(),
        cmpId: this.companyId,
        locId: this.locId ?? '%',
      };

      const data = await this.apiService
        .getDataById('Purchase/GetBillDueStatus', obj)
        .toPromise();
      this.invoiceList = data;
      setTimeout(() => {
        this.searchGrid();
      }, 100);
      this.com.hideLoader();
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  onInput(event: any) {
    this.search = event.target.value;
    this.searchGrid();
  }

  searchGrid(): void {
    const tableElement = this.invoice.nativeElement;
    const rows = tableElement.querySelectorAll('tr');

    let totalDisAmt = 0;
    let totalOtherCredit = 0;
    let totalNetDue = 0;
    let totalRecAmount = 0;
    let totalBalance = 0;

    rows.forEach((row: HTMLTableRowElement) => {
      let isShow = true;

      if (
        row.textContent &&
        row.textContent.toLowerCase().indexOf(this.search.toLowerCase()) > -1
      ) {
        isShow = true;
      } else {
        isShow = false;
      }

      if (isShow) {
        totalDisAmt += parseFloat(
          row.querySelector('.discountAmt')?.textContent!.replace(/,/g, '')
        );
        totalOtherCredit += parseFloat(
          row.querySelector('.otherCredit')?.textContent!.replace(/,/g, '')
        );
        totalNetDue += parseFloat(
          row.querySelector('.netDue')?.textContent!.replace(/,/g, '')
        );
        totalRecAmount += parseFloat(
          row.querySelector('.recAmt')?.textContent!.replace(/,/g, '')
        );
        totalBalance += parseFloat(
          row.querySelector('.balance')?.textContent!.replace(/,/g, '')
        );
        row.style.display = '';
      } else {
        row.style.display = 'none';
      }
    });

    this.totalDisAmt = totalDisAmt;
    this.totalOtherCredit = totalOtherCredit;
    this.totalNetDue = totalNetDue;
    this.totalRecAmount = totalRecAmount;
    this.totalBalance = totalBalance;
  }

  export(fileName: any, formate: any) {
    this.com.ExportFiles(fileName, formate);
  }
}
