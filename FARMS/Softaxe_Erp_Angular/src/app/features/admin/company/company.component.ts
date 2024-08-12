import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { DatePipe } from '@angular/common';
import { ApiService } from 'src/app/services/api.service';
import { CommonService } from 'src/app/services/common.service';
import { environment } from 'src/environment/environmemt';

@Component({
  selector: 'app-company',
  templateUrl: './company.component.html',
  styleUrls: ['./company.component.css'],
})
export class CompanyComponent {
  companyForm!: FormGroup;
  basePath = environment.basePath;
  
  groupList: any[] = [];
  companyList: any[] = [];
  PontsList: any[] = [];
  ShiftList: any[] = [];
  
  isShowPage: boolean = false;
  isDisabled: boolean = true;

  finFromDate: Date;
  finToDate: Date;
  companyImage: File | null = null;
  selectedImage: any = '';
  file: any;
  isFarm: boolean = false;

  erpItems: any[] = [
    { id: 'Distribution', name: 'Distribution' },
    { id: 'ERP', name: 'ERP' },
    { id: 'Farm', name: 'Farm' },
  ];

  commissonItems: any[] = [
    { id: 'Recovery', name: 'Recovery' },
    { id: 'Sale', name: 'Sale' },
    { id: 'Slab', name: 'Slab' },
  ];

  mbileAppItem: any[] = [
    { id: 'Local Order', name: 'Local Order' },
    { id: 'MobileApp', name: 'Mobile App' },
    { id: 'Regular', name: 'Regular' },
  ];
  
  locationWiseItem: any[] = [
    { id: 'Fix Location', name: 'Fix Location' },
    { id: 'Location Wise', name: 'Location Wise' },
  ];
  
  reportItem: any[] = [
    { id: 0, name: 0 },
    { id: 1, name: 1 },
  ];

  constructor(
    private apiService: ApiService,
    private fb: FormBuilder,
    private dp: DatePipe,
    private tostr: ToastrService,
    private com: CommonService
  ) {
    const today = new Date();
    this.finFromDate = new Date(today.getFullYear(), 0, 1);
    this.finToDate = new Date(today.getFullYear(), 13 - 1, 0);
  }

  ngOnInit(): void {
    this.formInit();
    this.getCompanyList();
    this.getGroupList();
    this.getPointsList();
    this.getShiftList();
  }

  formInit() {
    this.companyForm = this.fb.group({
      GroupId: [null],
      CompId: [0],
      Date: [new Date()],
      CompanyName: [''],
      ShortName: [''],
      Country: [''],
      City: [''],
      Address: [''],
      OwnerName: [''],
      Email: [''],
      Contact: [''],
      Ntn: [''],
      StkAdj: [''],
      ShipmentSale: [''],
      ShipmentPurchase: [''],
      OtherCreditSale: [''],
      OtherCreditPurchase: [''],
      DiscountSale: [''],
      DiscountPurchase: [''],
      AccountOpening: [''],
      CostofSale: [''],
      Tax1: [''],
      Tax2: [''],
      FTax: [''],
      WhTax: [''],
      InputSaleTax: [''],
      OtherSaleTax: [''],
      FinFromDate: [this.finFromDate],
      FinToDate: [this.finToDate],
      Currency: [''],
      Symbol: [''],
      Commission: [null],
      PosDistribution: [null],
      MobApp: ['Regular'],
      LocationWise: ['Location Wise'],
      ReportFormat: [0],
      FurtherTax: [0],
      WhFiler: [0],
      WhNonFiler: [0],
      Tax: false,
      LedgerDetail: false,
      SystemApproval: false,
      DayClose: false,
      MonthClose: false,
      GL: false,
      CommCustomer: false,
      CommSupplier: false,
      ProDisSale: false,
      ProDisPurchase: false,
      BillWiseControl: false,
      CreditLimit: false,
      PartyDisAlw: false,
      TaxOnProduct: false,
      SaleRapComm: false,
      LoadParty: false,
      Services: false,
      ProByCategory: false,
      Stock: false,
      StockExpiry: false,
      CostCenter: false,
      JobWise: false,
      Aging: false,
      ExportDetails: false,
      PoMust: false,
      RoundVal: false,
      IsBroiler: false,
      Islayers: false,
      IsHatchery: false,
    });
  }

  togglePages() {
    this.isShowPage = !this.isShowPage;
    if (this.isShowPage == true) {
      this.onClickRefresh();
    }
  }

  getCompanyList() {
    this.apiService.getData('Admin/GetCompanyList').subscribe((data) => {
      this.companyList = data;
    });
  }

  getGroupList() {
    this.apiService.getData('Admin/GetCompanyGroup').subscribe((data) => {
      this.groupList = data;
    });
  }

  onClickNew() {
    this.onClickRefresh();
    this.companyForm.get('FinFromDate')?.setValue(this.finFromDate);
    this.companyForm.get('FinToDate')?.setValue(this.finToDate);
    this.companyForm.get('Date')?.setValue(new Date());
    this.isDisabled = false;
    this.getNumebr();
  }
  
  getNumebr(){
    this.apiService.getData('Admin/GetNumber').subscribe((data) => {
      this.companyForm.get('CompId')?.setValue(data.max);
      this.companyForm.get('StkAdj')?.setValue(data.code.StkAdjustmentCode);
      this.companyForm.get('ShipmentSale')?.setValue(data.code.ShipmentSaleCode);
      this.companyForm.get('ShipmentPurchase')?.setValue(data.code.ShipmentPurchaseCode);
      this.companyForm.get('OtherCreditSale')?.setValue(data.code.OtherCreditCodeSale);
      this.companyForm.get('OtherCreditPurchase')?.setValue(data.code.OtherCreditCodePurchase);
      this.companyForm.get('DiscountSale')?.setValue(data.code.DiscountCodeSale);
      this.companyForm.get('DiscountPurchase')?.setValue(data.code.DiscountCodePurchase);
      this.companyForm.get('AccountOpening')?.setValue(data.code.AccountOpningCode);
      this.companyForm.get('CostofSale')?.setValue(data.code.CostofSale);
      this.companyForm.get('Tax1')?.setValue(data.code.Tax1Code);
      this.companyForm.get('Tax2')?.setValue(data.code.Tax2Code);
    });
  }

  onClickRefresh() {
    this.companyForm.reset();
    this.formInit();
    this.isDisabled = true;
    this.selectedImage = "";
    this.companyImage = null;
    this.file = "";
  }

  dateFormate(date: any) {
    let dp = date.split('-');
    return new Date(dp[0], dp[1] - 1, dp[2]);
  }

  onChangeErp(){
    let frm = this.companyForm.get('PosDistribution')?.value;

    if(frm == "Farm"){
      this.isFarm = true;
    }
    else{
      this.companyForm.get('IsBroiler')?.setValue(false);
      this.companyForm.get('Islayers')?.setValue(false);
      this.companyForm.get('IsHatchery')?.setValue(false);
      this.isFarm = false;
    }
  }

  getCompanyDetail(item: any) {
    const obj = {
      id: item.CompanyId,
    };

    this.apiService
      .getDataById('Admin/GetCompanyDetail', obj)
      .subscribe((data) => {
        let cmp = data[0];
        this.companyForm.get('GroupId')?.setValue(cmp.GRPID);
        this.companyForm.get('CompId')?.setValue(cmp.CMP_ID);
        this.companyForm.get('Date')?.setValue(this.dateFormate(cmp.DATE));
        this.companyForm.get('CompanyName')?.setValue(cmp.CMP_NAME);
        this.companyForm.get('ShortName')?.setValue(cmp.SHORTNAME);
        this.companyForm.get('Country')?.setValue(cmp.COUNTRY);
        this.companyForm.get('City')?.setValue(cmp.CMP_CITY);
        this.companyForm.get('Address')?.setValue(cmp.CMP_ADR);
        this.companyForm.get('OwnerName')?.setValue(cmp.OWNERNAME);
        this.companyForm.get('Email')?.setValue(cmp.EMAIL);
        this.companyForm.get('Contact')?.setValue(cmp.CONTACT);
        this.companyForm.get('Ntn')?.setValue(cmp.NTN);
        this.companyForm.get('StkAdj')?.setValue(cmp.STKADJUSTMENTCODE);
        this.companyForm.get('ShipmentSale')?.setValue(cmp.SHIPMENTSALECODE);
        this.companyForm.get('ShipmentPurchase')?.setValue(cmp.SHIPMENTPURCHASECODE);
        this.companyForm.get('OtherCreditSale')?.setValue(cmp.OTHERCREDITCODESALE);
        this.companyForm.get('OtherCreditPurchase')?.setValue(cmp.OTHERCREDITCODEPURCHASE);
        this.companyForm.get('DiscountSale')?.setValue(cmp.DISCOUNTCODESALE);
        this.companyForm.get('DiscountPurchase')?.setValue(cmp.DISCOUNTCODEPURCHASE);
        this.companyForm.get('AccountOpening')?.setValue(cmp.ACCOUNTOPNINGCODE);
        this.companyForm.get('CostofSale')?.setValue(cmp.COSTOFSALE);
        this.companyForm.get('Tax1')?.setValue(cmp.TAX1CODE);
        this.companyForm.get('Tax2')?.setValue(cmp.TAX2CODE);
        this.companyForm.get('FTax')?.setValue(cmp.FTAXCODE);
        this.companyForm.get('WhTax')?.setValue(cmp.WHTCODE);
        this.companyForm.get('InputSaleTax')?.setValue(cmp.INPUTSALETAX);
        this.companyForm.get('OtherSaleTax')?.setValue(cmp.OTHERSALETAX);
        this.companyForm.get('FurtherTax')?.setValue(cmp.FURTHERTAX);
        this.companyForm.get('FinFromDate')?.setValue(this.dateFormate(cmp.FROMDATE));
        this.companyForm.get('FinToDate')?.setValue(this.dateFormate(cmp.TODATE));
        this.companyForm.get('Currency')?.setValue(cmp.CURRENCY);
        this.companyForm.get('Symbol')?.setValue(cmp.CURRENCYSYMBOL);
        this.companyForm.get('Commission')?.setValue(cmp.COMMISSION);
        this.companyForm.get('PosDistribution')?.setValue(cmp.DISTRIBUTIONPOS);
        this.companyForm.get('MobApp')?.setValue(cmp.MOBAPP);
        this.companyForm.get('LocationWise')?.setValue(cmp.LOCATIONWISE);
        this.companyForm.get('ReportFormat')?.setValue(cmp.REPORTFORMAT);
        this.companyForm.get('WhFiler')?.setValue(cmp.WHFILER);
        this.companyForm.get('WhNonFiler')?.setValue(cmp.WHNONFILER);
        this.companyForm.get('Tax')?.setValue(cmp.TAX);
        this.companyForm.get('SystemApproval')?.setValue(cmp.APPROVALSYSTEM);
        this.companyForm.get('MonthClose')?.setValue(cmp.MONTHCLOSE);
        this.companyForm.get('DayClose')?.setValue(cmp.DAYCLOSE);
        this.companyForm.get('LedgerDetail')?.setValue(cmp.LEDGER);
        this.companyForm.get('RoundVal')?.setValue(cmp.ROUNDVAL);
        this.companyForm.get('CommCustomer')?.setValue(cmp.COMMISSIONCUSTOMER);
        this.companyForm.get('CommSupplier')?.setValue(cmp.COMMISSIONSUPPLIER);
        this.companyForm.get('ProDisSale')?.setValue(cmp.PRODUCTDISCOUNTSALE);
        this.companyForm.get('ProDisPurchase')?.setValue(cmp.PRODUCTDISCOUNTPURCHASE);
        this.companyForm.get('BillWiseControl')?.setValue(cmp.BILLWISECONTROL);
        this.companyForm.get('CreditLimit')?.setValue(cmp.CREDITLIMIT);
        this.companyForm.get('PartyDisAlw')?.setValue(cmp.PARTYDISCOUNTALLOWED);
        this.companyForm.get('TaxOnProduct')?.setValue(cmp.TAXONPRODUCT);
        this.companyForm.get('SaleRapComm')?.setValue(cmp.SALERAPCOMMISSION);
        this.companyForm.get('LoadParty')?.setValue(cmp.LOADPARTY);
        this.companyForm.get('Services')?.setValue(cmp.SERVICE);
        this.companyForm.get('ProByCategory')?.setValue(cmp.PRODUCTBYCATEGORY);
        this.companyForm.get('Stock')?.setValue(cmp.STOCK);
        this.companyForm.get('StockExpiry')?.setValue(cmp.STOCKEXPIRY);
        this.companyForm.get('CostCenter')?.setValue(cmp.COSTCENTERCONTROL);
        this.companyForm.get('JobWise')?.setValue(cmp.JOBWISECONTROL);
        this.companyForm.get('Aging')?.setValue(cmp.AGING);
        this.companyForm.get('ExportDetails')?.setValue(cmp.EXPORTDETAIL);
        this.companyForm.get('PoMust')?.setValue(cmp.POMUST);
        this.companyForm.get('GL')?.setValue(cmp.GL);
        this.companyForm.get('IsBroiler')?.setValue(cmp.ISBROILER);
        this.companyForm.get('Islayers')?.setValue(cmp.ISLAYERS);
        this.companyForm.get('IsHatchery')?.setValue(cmp.ISHATCHERY);
        this.selectedImage = this.basePath + data[0].LOGOPATH;
        this.isShowPage = true;
        this.isDisabled = false;

        this.onChangeErp();
      });
  }

  onClickSave() {
    try {
      let body = this.companyForm.value;
      body.Date = this.dp.transform(body.Date, 'yyyy/MM/dd');
      body.FinFromDate = this.dp.transform(body.FinFromDate, 'yyyy/MM/dd');
      body.FinToDate = this.dp.transform(body.FinToDate, 'yyyy/MM/dd');
      body.Ntn = ((body.Ntn == null) ? "" : body.Ntn);
      body.FurtherTax = ((body.FurtherTax == null) ? 0 : body.FurtherTax);
      body.WhFiler = ((body.WhFiler == null) ? 0 : body.WhFiler);
      body.WhNonFiler = ((body.WhNonFiler == null) ? 0 : body.WhNonFiler);
      
      if (body.GroupId == null) {
        this.tostr.warning('Select Group...');
        return;
      }

      if (body.CompanyName == null) {
        this.tostr.warning('Enter Company Name...');
        return;
      }

      if (body.ShortName == null) {
        this.tostr.warning('Enter Short Name...');
        return;
      }

      if (body.Country == null) {
        this.tostr.warning('Enter Country...');
        return;
      }
      
      if (body.City == null) {
        this.tostr.warning('Enter City...');
        return;
      }

      if (body.Address == null) {
        this.tostr.warning('Enter Address...');
        return;
      }

      if (body.OwnerName == null) {
        this.tostr.warning('Enter OwnerName...');
        return;
      }

      if (body.Email == null) {
        this.tostr.warning('Enter Email...');
        return;
      }

      if (body.Contact == null) {
        this.tostr.warning('Enter Contact...');
        return;
      }

      if (body.Currency == null) {
        this.tostr.warning('Enter Currency...');
        return;
      }

      if (body.Symbol == null) {
        this.tostr.warning('Enter Currency Symbol...');
        return;
      }

      if (body.PosDistribution == null) {
        this.tostr.warning('Select ERP/Distribution...');
        return;
      }

      if (body.PosDistribution == "Farm") {
        if(body.IsBroiler != true && body.Islayers != true && body.IsHatchery != true){
          this.tostr.warning('Select Broiler/Layers/Hatchery...');
          return;
        }
      }

      let formData = new FormData();
      for (const key of Object.keys(body)) {
        formData.append(key, body[key]);
      }

      formData.append('Image', this.companyImage!);

      this.com.showLoader();
      
      this.apiService
        .saveData('Admin/AddUpdateCompany', formData)
        .subscribe(
          (result) => {
            if (result == true || result == 'true') {
              this.tostr.success('Save Successfully');
              this.onClickRefresh();
              this.getCompanyList();
              this.com.hideLoader();
            } else {
              this.tostr.error('Please Save Again');
              this.com.hideLoader();
            }
          },
          (error) => {
            this.tostr.error('On Err');
            this.com.hideLoader();
          }
        );
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  deleteCompany(item: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');
    if (!confirmDelete) {
      return;
    }

    try {
      this.com.showLoader();

      let obj = {
        groupId: item.GroupId,
        companyid: item.CompanyId,
      };

      this.apiService.deleteData('Admin/DeleteCompany', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.onClickRefresh();
            this.tostr.success('Company Delete Successfully');
            this.getCompanyList();
            this.com.hideLoader();
          } else if (data == 'false' || data == false) {
            this.com.hideLoader();
            this.tostr.error('Delete Again');
          }
        },
        error: (error) => {
          this.com.hideLoader();
          this.tostr.info(error.error.text);
        },
      });
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
    }
  }

  onFileSelected(event: any) {
    this.companyImage = event.target.files[0];
    if (this.companyImage) {
      this.selectedImage = URL.createObjectURL(event.target.files[0]);
    }
  }





















  isInputDisabled: boolean = true;
  isDisabled1: boolean = true;



  CompanyDetail: any[] = [];
  // CompanyDetail1:any;
  fDate: Date;
  tDate: Date;
  date: Date;

  

  email: string;
  cmp_name: string = '';


  onClikHome() {
    this.isShowPage = false;
  }

  enableInput() {
    this.isInputDisabled = false;
  }

  disableInput() {
    this.isInputDisabled = true;
  }

  enableFields() {
    this.isDisabled1 = false;
  }

  disableFields() {
    this.isDisabled1 = true;
  }

  

  pointsList: any[] = [];

  pointsName = '';
  pointsId = 0;

  isDisabledPoints: boolean = true;
  isShowPoints: boolean = false;

  refreshPoints() {
    this.pointsName = '';
    this.pointsId = 0;
    this.isDisabledPoints = true;
    this.isShowPoints = false;
  }

  newPoints() {
    this.refreshPoints();
    this.isDisabledPoints = false;
    this.isShowPoints = true;
  }

  getPointsList() {
    this.apiService.getData('Admin/GetPoints').subscribe((data) => {
      this.pointsList = data;
      console.log('Points', data);
    });
  }

  createUpdatePoints() {
    const obj = {
      id: this.pointsId,
      name: this.pointsName,
      dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
    };

    if (this.pointsName == '' || this.pointsName == null) {
      this.tostr.warning('Enter Name....!');
      return;
    }

    this.apiService
      .saveObj('Admin/AddUpdatePoints', obj)
      .subscribe((result) => {
        if (result == true || result == 'true') {
          this.getPointsList();
          this.tostr.success('Save Successfully');
          this.refreshPoints();
        } else {
          this.tostr.error('Please Save Again');
        }
      });
  }

  editPoints(id: any, name: any): void {
    this.pointsName = name;
    this.pointsId = id;
    this.isDisabledPoints = false;
    this.isShowPoints = true;
  }

  deletePoints(Id: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {
      const obj = {
        id: Id,
        dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      };

      this.apiService.deleteData('Admin/DeletePoints', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.getPointsList();
            this.tostr.success('Delete Successfully');
            this.refreshPoints();
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

  //  Shift Modal

  shiftList: any[] = [];

  shiftName = '';
  fTime = '';
  toTime = '';
  shiftId = 0;

  isDisabledShift: boolean = true;
  isShowShift: boolean = false;

  refreshShift() {
    this.shiftId = 0;
    this.shiftName = '';
    this.fTime = '';
    this.toTime = '';
    this.isDisabledShift = true;
    this.isShowShift = false;
  }

  newShift() {
    this.refreshShift();
    this.isDisabledShift = false;
    this.isShowShift = true;
  }

  getShiftList() {
    this.apiService.getData('Admin/GetShift').subscribe((data) => {
      this.shiftList = data;
      console.log('shift', data);
    });
  }

  createUpdateShift() {
    const obj = {
      id: this.shiftId,
      name: this.shiftName,
      fromTime: this.fTime,
      toTime: this.toTime,
      dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
    };

    if (this.shiftName == '' || this.shiftName == null) {
      this.tostr.warning('Enter Name....!');
      return;
    }

    this.apiService.saveObj('Admin/AddUpdateShift', obj).subscribe((result) => {
      if (result == true || result == 'true') {
        this.getShiftList();
        this.tostr.success('Save Successfully');
        this.refreshShift();
      } else {
        this.tostr.error('Please Save Again');
      }
    });
  }

  editShift(id: any, name: any, fTime: any, tTime: any): void {
    this.shiftId = id;
    this.shiftName = name;
    this.fTime = fTime;
    this.toTime = tTime;
    this.isDisabledShift = false;
    this.isShowShift = true;
  }

  deleteShift(Id: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {
      const obj = {
        id: Id,
        dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      };

      this.apiService.deleteData('Admin/DeleteShift', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.getShiftList();
            this.tostr.success('Delete Successfully');
            this.refreshShift();
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

  export(fileName: any, formate: any) {
    this.com.ExportFiles(fileName, formate);
  }
}
