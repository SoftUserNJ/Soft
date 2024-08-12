import { Component, ViewChild, ElementRef } from '@angular/core';
import { DatePipe } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { environment } from '../../../../environment/environmemt';
import { CommonService } from 'src/app/services/common.service';
declare const $: any;

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
})
export class DashboardComponent {
  @ViewChild('partyPositionTable', { static: false })
  partyPositionTable!: ElementRef;
  @ViewChild('ledgerLists', { static: false }) ledgerLists!: ElementRef;
  @ViewChild('pcLists', { static: false }) pcLists!: ElementRef;

  @ViewChild('approveVoucher', { static: false }) approveVoucher!: ElementRef;
  @ViewChild('varifyVoucher', { static: false }) varifyVoucher!: ElementRef;
  @ViewChild('auditVoucher', { static: false }) auditVoucher!: ElementRef;

  basePath = environment.basePath;
  cmpImage: any = localStorage.getItem('Logo');
  cmpName: any = localStorage.getItem('CmpName');
  isApproval = localStorage.getItem('approvalSystem');
  isDashboard = localStorage.getItem('dashboard');
  mobApp = localStorage.getItem('mobApp');

  cmpLogo: string = '';

  verifyVoucherList: any[] = [];
  approveVoucherList: any[] = [];
  auditVoucherList: any[] = [];

  isApprove: boolean = false;
  isVerify: boolean = false;
  isAudit: boolean = false;

  // HEADER BOX
  verify: any = 0;
  approve: any = 0;
  audit: any = 0;
  costProfitMargin: any = 0;
  profitAndLoss: any = 0;

  // PURCHASE ACTIVITY
  products: any = 0;
  stockAmount: any = 0;
  purchase: any = 0;
  purchaseAmount: any = 0;
  purchaseBillDueSatuts: any = 0;
  totalPurchaseBillDueSatuts: any = 0;

  // SALE ACTIVITY
  pendingOrders: any = 0;
  pendingOrdersAmount: any = 0;
  deliverOrders: any = 0;
  deliverOrdersAmount: any = 0;
  sale: any = 0;
  saleAmount: any = 0;
  saleBillDueStatus: any = 0;
  totalSaleBillDueStatus: any = 0;

  // BANK CASH ACTIVITY
  receipts: any = 0;
  totalReceipts: any = 0;
  payment: any = 0;
  totalPayment: any = 0;
  receiptsPayment: any = 0;
  trailBalance: any = 0;

  // TRIAL DIFFRENCE
  trialBalanceList: any[] = [];
  totalDebit: any = 0;
  totalCredit: any = 0;
  totalDff: any = 0;

  // EXPIRE MIN LEVEL PRODUCT
  minLvlProductList: any[] = [];
  expireProductList: any[] = [];

  // PARTY POSTION
  partyPositionList: any = [];
  customer: boolean = true;
  supplier: boolean = false;
  balOpen: any = 0;
  balSalePurchase: any = 0;
  balRecPaid: any = 0;
  balCurrent: any = 0;
  searchPartyPostion: string = '';

  // PROFIT & LOSS
  fromDate: Date;
  toDate: Date;

  // COST PROFIT & MARGIN
  fromDate1: Date;
  toDate1: Date;
  costProfitList: any[] = [];
  costStatus: any = '';
  searchCost: any = '';
  costLength: any = 0;
  costSale: any = 0;
  costSalePer: any = 0;
  costRecAmt: any = 0;
  costRecAmtPer: any = 0;
  cost: any = 0;
  costPer: any = 0;
  costDiscount: any = 0;
  costDiscountPer: any = 0;
  costMargin: any = 0;
  costMarginPer: any = 0;

  vchList: any[] = [];
  approveType: string = null;
  approveNo: any = '';

  vchList2: any[] = [];
  approveType2: any = null;
  approveNo2: any = '';

  vchList3: any[] = [];
  approveType3: any = null;
  approveNo3: any = '';

  locationList: any[] = [];
  locId: any;
  isDisableLoc: boolean = false;

  firslLoad : boolean = true

  usersList: any [] = [];
  userId: any = null;

  outputCode: any;
  outputName: any;

  constructor(
    private apiService: ApiService,
    private tostr: ToastrService,
    private dp: DatePipe,
    private auth: AuthService,
    private com: CommonService
  ) {
    const today = new Date();
    const fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    const toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
    this.fromDate = fromDate;
    this.fromDate1 = fromDate;
    this.toDate = toDate;
    this.toDate1 = toDate;
  }

  openReport(item: any) {
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
    }&CompId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${
      item.LOCID
    }`;
    this.com.viewReport(url);
  }

  async ngOnInit() {
    this.cmpLogo = `${this.basePath}/Companies/${this.cmpName}/CompanyLogo/${this.cmpImage}`;
    if (this.isDashboard == 'true') {
      this.locationList = await this.com.getLocation();
      if (this.auth.locId() == 'HO') {
        this.isDisableLoc = false;
      } else {
        this.isDisableLoc = true;
      }
      this.locId = this.auth.locId();

      this.getDashboard();
      this.getVoucherStatus();
      this.getVchTypes();
      this.getMinLvlProduct();
      this.getExpireProduct();
      this.getPartyPosition();
      this.getUserList();
    }
  }

  getUserList() {
    this.apiService
      .getDataById('Auth/GetUsersList', { locId: (this.locId == 'HO') ? '%' : this.locId })
      .subscribe((data) => {
        this.usersList = data;
      });
  }

  getVchTypes() {
    this.apiService.getData('Accounts/GetVchTypes').subscribe((data) => {
      this.vchList = data;
      this.vchList2 = data;
      this.vchList3 = data;
    });
  }

  getVoucherStatus() {
    this.apiService
      .getDataById('Accounts/GetVchData', { locId: this.locId, userId: this.userId })
      .subscribe((data) => {
        if(this.firslLoad){
          this.verify = data.verify.filter(
            (x) => x.LOCID == this.auth.locId()
          ).length;
          this.approve = data.approval.filter(
            (x) => x.LOCID == this.auth.locId()
          ).length;
          this.audit = data.audit.filter(
            (x) => x.LOCID == this.auth.locId()
          ).length;
          this.firslLoad = false;
        }

        this.verifyVoucherList = data.verify;
        this.approveVoucherList = data.approval;
        this.auditVoucherList = data.audit;

        this.approveType = null;
        this.approveNo = '';
        this.approveType2 = null;
        this.approveNo2 = '';
        this.approveType3 = null;
        this.approveNo3 = '';
      });
  }

  onClickVouchers(tag: any) {
    if (tag == 'approve') {
      this.isApprove = true;
      this.isVerify = false;
      this.isAudit = false;
    } else if (tag == 'verify') {
      this.isApprove = false;
      this.isVerify = true;
      this.isAudit = false;
    } else if (tag == 'audit') {
      this.isApprove = false;
      this.isVerify = false;
      this.isAudit = true;
    } else {
      this.isApprove = true;
      this.isVerify = true;
      this.isAudit = true;
    }

    $('#voucherModal').modal('show');
  }

  searchVoucher(tag: any) {
    let tableElement: any;
    let vtype: any = '';
    let vno: any = '';

    if (tag == 'approve') {
      tableElement = this.approveVoucher.nativeElement;
      if (this.approveType) {
        vtype = this.approveType.trim();
      }
      vno = this.approveNo.toLowerCase();
    }
    if (tag == 'verify') {
      tableElement = this.varifyVoucher.nativeElement;
      if (this.approveType2) {
        vtype = this.approveType2.trim();
      }
      vno = this.approveNo2.toLowerCase();
    }
    if (tag == 'audit') {
      tableElement = this.auditVoucher.nativeElement;
      if (this.approveType3) {
        vtype = this.approveType3.trim();
      }
      vno = this.approveNo3.toLowerCase();
    }

    const rows = tableElement.querySelectorAll('tr');
    rows.forEach((row: HTMLTableRowElement) => {
      let isShow = true;

      if (
        row.querySelector('.vchtype')?.textContent != vtype &&
        vtype.length > 0
      ) {
        isShow = false;
      }

      if (isShow) {
        if (
          row.querySelector('.vchno')?.textContent.toLowerCase().indexOf(vno) >
          -1
        ) {
          isShow = true;
        } else {
          isShow = false;
        }
      }

      if (isShow) {
        row.style.display = '';
      } else {
        row.style.display = 'none';
      }
    });
  }

  getDashboard() {
    this.apiService.getData('Dashboard/GetDashboard').subscribe((result) => {
      const data = result[0];

      this.costProfitMargin = data.CostProfitMargin;
      this.profitAndLoss = data.ProfitandLoss;

      // PURCHASE ACTIVITY
      this.products = data.TotalProduct;
      this.stockAmount = data.StockAmount;
      this.purchase = data.TotalPurchaseInvoice;
      this.purchaseAmount = data.TotalPurchaseAmount;
      this.purchaseBillDueSatuts = data.TotalPurchaseDueInvoice;
      this.totalPurchaseBillDueSatuts = data.TotalPurchaseDue;

      // SALE ACTIVITY
      this.pendingOrders = data.TotalPendingDO;
      this.pendingOrdersAmount = data.TotalPendingDOAmt;
      this.deliverOrders = data.TotalDelDO;
      this.deliverOrdersAmount = data.TotalDelDOAmount;
      this.sale = data.TotalSaleInvoice;
      this.saleAmount = data.TotalSaleAmount;
      this.saleBillDueStatus = data.TotalSaleDueInvoice;
      this.totalSaleBillDueStatus = data.TotalSaleDue;

      // BANK CASH ACTIVITY
      this.receipts = data.Receipts;
      this.totalReceipts = data.TotalReceipts;
      this.payment = data.Payment;
      this.totalPayment = data.TotalPayment;
      this.receiptsPayment = data.ReceiptsPayment;
      this.trailBalance = data.TrialBalance;
    });
  }

  onClickTrial(trial: any) {
    this.totalDebit = 0;
    this.totalCredit = 0;
    this.totalDff = 0;

    if (trial != 0) {
      this.apiService.getData('Dashboard/TrialDifference').subscribe((data) => {
        this.trialBalanceList = data;

        data.forEach((x) => {
          this.totalDebit += x.DEBIT;
          this.totalCredit += x.CREDIT;
          this.totalDff += x.DIFF;
        });

        $('#trialModal').modal('show');
      });
    }
  }

  getPartyPosition() {
    let tag = 'customer';

    if (this.customer) {
      tag = 'customer';
    } else if (this.supplier) {
      tag = 'supplier';
    }

    this.apiService
      .getDataById('Dashboard/GetPartyPosition', { tag: tag })
      .subscribe((data) => {
        this.partyPositionList = data;
        this.searchPartyPostion = '';
        setTimeout(() => {
          this.searchGrid();
        }, 100);
      });
  }

  onClickFilter(status: any) {
    this.customer = false;
    this.supplier = false;
    if (status == 'customer') {
      this.customer = true;
      this.supplier = false;
    } else if (status == 'supplier') {
      this.customer = false;
      this.supplier = true;
    }
    this.getPartyPosition();
  }

  searchGrid(): void {
    const tableElement = this.partyPositionTable.nativeElement;
    const rows = tableElement.querySelectorAll('tr');

    this.balOpen = 0;
    this.balSalePurchase = 0;
    this.balRecPaid = 0;
    this.balCurrent = 0;

    rows.forEach((row: HTMLTableRowElement) => {
      let isShow = true;

      if (
        row.textContent &&
        row.textContent
          .toLowerCase()
          .indexOf(this.searchPartyPostion.toLowerCase()) > -1
      ) {
        isShow = true;
      } else {
        isShow = false;
      }

      if (isShow) {
        row.style.display = '';
        this.balOpen += parseFloat(
          row.querySelector('.balOpen')?.textContent!.replace(/,/g, '')
        );
        this.balSalePurchase += parseFloat(
          row.querySelector('.balSalePurchase')?.textContent!.replace(/,/g, '')
        );
        this.balRecPaid += parseFloat(
          row.querySelector('.balRecPaid')?.textContent!.replace(/,/g, '')
        );
        this.balCurrent += parseFloat(
          row.querySelector('.balCurrent')?.textContent!.replace(/,/g, '')
        );
      } else {
        row.style.display = 'none';
      }
    });
  }

  getMinLvlProduct() {
    this.apiService.getData('Dashboard/MinLvlProduct').subscribe((data) => {
      this.minLvlProductList = data;
    });
  }

  getExpireProduct() {
    this.apiService.getData('Dashboard/ExpireProducts').subscribe((data) => {
      this.expireProductList = data;
    });
  }

  onViewProfitLoss() {
    const fromDate = this.dp.transform(this.fromDate, 'yyyy/MM/dd');
    const toDate = this.dp.transform(this.toDate, 'yyyy/MM/dd');

    let groupId = 0;
    let groupId1 = 99999;
    let subGroupId = 0;
    let subGroupId1 = 99999;

    let url = `CostCategory?FromDate=${fromDate}&ToDate=${toDate}&FDate=${fromDate}&TDate=${toDate}&GroupId=${groupId}&GroupId1=${groupId1}&SubGroupId=${subGroupId}&SubGroupId1=${subGroupId1}&Verify=%&CompId=${this.auth.cmpId()}&LocId=${this.auth.locId()}`;
    this.com.viewReport(url);
  }

  async onClickCProfit() {
    if (this.costStatus == '') {
      this.tostr.warning('Select Area/Party & Product Wise...!');
    }

    if (this.costStatus) {
      await this.onClickCP(this.costStatus);
    }
  }

  async onClickCP(tag: any) {
    let columnName = '';
    let groupBy = '';

    if (tag == 'area') {
      columnName = 'V.MAINAREA, V.SUBAREA';
      groupBy = 'V.MAINAREA, V.SUBAREA';
    } else if (tag == 'areaParty') {
      columnName = 'V.MAINAREA, V.SUBAREA, V.PARTY';
      groupBy = 'V.MAINAREA, V.SUBAREA, V.PARTY ';
    } else if (tag == 'areaProduct') {
      columnName = 'V.MAINAREA, V.SUBAREA, V.PRODUCT';
      groupBy = 'V.MAINAREA, V.SUBAREA, V.PRODUCT';
    } else if (tag == 'areaCategory') {
      columnName = 'V.MAINAREA, V.SUBAREA, V.CATAGORY';
      groupBy = 'V.MAINAREA, V.SUBAREA, V.CATAGORY';
    } else if (tag == 'areaSaleMan') {
      columnName = 'V.MAINAREA, V.SUBAREA, V.SALEMAN';
      groupBy = 'V.MAINAREA, V.SUBAREA, V.SALEMAN';
    } else if (tag == 'party') {
      columnName = 'V.PARTY';
      groupBy = 'V.PARTY';
    } else if (tag == 'partyArea') {
      columnName = 'V.PARTY, V.MAINAREA, V.SUBAREA';
      groupBy = 'V.PARTY, V.MAINAREA, V.SUBAREA';
    } else if (tag == 'partyAreaProduct') {
      columnName = 'V.PARTY, V.MAINAREA, V.SUBAREA, V.PRODUCT';
      groupBy = 'V.PARTY, V.MAINAREA, V.SUBAREA, V.PRODUCT';
    } else if (tag == 'partyAreaCategory') {
      columnName = 'V.PARTY, V.MAINAREA, V.SUBAREA, V.CATAGORY';
      groupBy = 'V.PARTY, V.MAINAREA, V.SUBAREA, V.CATAGORY';
    } else if (tag == 'partyAreaSaleTeam') {
      columnName = 'V.PARTY, V.MAINAREA, V.SUBAREA, V.SALEMAN';
      groupBy = 'V.PARTY, V.MAINAREA, V.SUBAREA, V.SALEMAN';
    } else if (tag == 'product') {
      columnName = 'V.PRODUCT';
      groupBy = 'V.PRODUCT';
    } else if (tag == 'productArea') {
      columnName = 'V.PRODUCT, V.MAINAREA, V.SUBAREA';
      groupBy = 'V.PRODUCT, V.MAINAREA, V.SUBAREA';
    } else if (tag == 'productAreaParty') {
      columnName = 'V.PRODUCT, V.MAINAREA, V.SUBAREA, V.PARTY';
      groupBy = 'V.PRODUCT, V.MAINAREA, V.SUBAREA, V.PARTY';
    } else if (tag == 'catrgory') {
      columnName = 'V.CATAGORY';
      groupBy = 'V.CATAGORY';
    } else if (tag == 'catrgoryArea') {
      columnName = 'V.CATAGORY, V.MAINAREA, V.SUBAREA';
      groupBy = 'V.CATAGORY, V.MAINAREA, V.SUBAREA';
    } else if (tag == 'saleTeam') {
      columnName = 'V.SALEMAN';
      groupBy = 'V.SALEMAN';
    }

    let obj = {
      columnName: columnName,
      groupBy: groupBy,
      fromDate: this.dp.transform(this.fromDate1, 'yyyy/MM/dd'),
      toDate: this.dp.transform(this.toDate1, 'yyyy/MM/dd'),
    };

    const data = await this.apiService
      .getDataById('Dashboard/GetCostStatus', obj)
      .toPromise();
    this.costStatus = tag;
    this.costProfitList = data;
    this.searchCost = '';
    this.costLength = columnName.split(',').length + 1;

    setTimeout(() => {
      this.searchGridPC();
    }, 70);
  }

  searchGridPC() {
    const tableElement = this.pcLists.nativeElement;
    const rows = tableElement.querySelectorAll('tr');

    this.costSale = 0;
    this.costSalePer = 0;
    this.costRecAmt = 0;
    this.costRecAmtPer = 0;
    this.cost = 0;
    this.costPer = 0;
    this.costDiscount = 0;
    this.costDiscountPer = 0;
    this.costMargin = 0;
    this.costMarginPer = 0;

    rows.forEach((row: HTMLTableRowElement) => {
      let isShow = true;

      if (
        row.textContent &&
        row.textContent.toLowerCase().indexOf(this.searchCost.toLowerCase()) >
          -1
      ) {
        isShow = true;
      } else {
        isShow = false;
      }

      if (isShow) {
        this.costSale += parseFloat(
          row.querySelector('.sale')?.textContent!.replace(/,/g, '')
        );
        this.costRecAmt += parseFloat(
          row.querySelector('.recamt')?.textContent!.replace(/,/g, '')
        );
        this.cost += parseFloat(
          row.querySelector('.cost')?.textContent!.replace(/,/g, '')
        );
        this.costDiscount += parseFloat(
          row.querySelector('.discount')?.textContent!.replace(/,/g, '')
        );
        this.costMargin += parseFloat(
          row.querySelector('.margin')?.textContent!.replace(/,/g, '')
        );
        row.style.display = '';
      } else {
        row.style.display = 'none';
      }
    });

    rows.forEach((row: HTMLTableRowElement) => {
      if (row.style.display !== 'none') {
        // Sale
        var rowSaleAmt = parseFloat(
          row.querySelector('.sale')?.textContent!.replace(/,/g, '')
        );
        row.querySelector('.saleper').innerHTML = (
          (rowSaleAmt / this.costSale) *
          100
        )
          .toFixed(2)
          .toString();
        this.costSalePer += (rowSaleAmt / this.costSale) * 100;

        // Receive
        var rowRecAmt = parseFloat(
          row.querySelector('.recamt')?.textContent!.replace(/,/g, '')
        );
        row.querySelector('.recamtper').innerHTML = (
          (rowRecAmt / rowSaleAmt) *
          100
        )
          .toFixed(2)
          .toString();
        this.costRecAmtPer = (this.costRecAmt / this.costSale) * 100;

        // Cost
        var rowCostAmt = parseFloat(
          row.querySelector('.cost')?.textContent!.replace(/,/g, '')
        );
        row.querySelector('.costper').innerHTML = (
          (rowCostAmt / rowSaleAmt) *
          100
        )
          .toFixed(2)
          .toString();
        this.costPer = (this.cost / this.costSale) * 100;

        // Discount
        var rowDiscountAmt = parseFloat(
          row.querySelector('.discount')?.textContent!.replace(/,/g, '')
        );
        row.querySelector('.discountper').innerHTML = (
          (rowDiscountAmt / rowSaleAmt) *
          100
        )
          .toFixed(2)
          .toString();
        this.costDiscountPer = (this.costDiscount / this.costSale) * 100;

        // Margin
        var rowMarginAmt = parseFloat(
          row.querySelector('.margin')?.textContent!.replace(/,/g, '')
        );
        row.querySelector('.marginper').innerHTML = (
          (rowMarginAmt / rowSaleAmt) *
          100
        )
          .toFixed(2)
          .toString();
        this.costMarginPer = (this.costMargin / this.costSale) * 100;
      }
    });
  }

  onClickLedger(code: any, name: any){
    this.outputCode = code;
    this.outputName = name;
  }
  
}
