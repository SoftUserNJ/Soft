import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-cost-centre',
  templateUrl: './cost-centre.component.html',
  styleUrls: ['./cost-centre.component.css'],
})
export class CostCentreComponent {
  plusDisabled: string = 'disabled';

  ngFarmId: any = null;
  ngShareType: any = "A";
  ngJobNo: any = null;

  // COST CENTRE
  costCentreId: number = 0;
  costCentreName: any;
  costCommission: any;
  rent: any;
  rentInstallment: any;
  comType: any = 'A';
  userId: any;
  isDisabledCostCentre: boolean = true;
  isShowCostCentre: boolean = false;

  // JOB NO
  jobNoId: any;
  jobNo: any;
  jobStartDate: any;
  jobEndDate: any;
  remarks: any;
  days: any;
  totalChicks: any;
  weight: any;
  expense: any;
  finished: any = false;
  isDisabledJobNo: boolean = true;
  isShowJobNo: boolean = false;
  editMode: boolean = false;

  // Drop Down
  costCentreList: any = [];
  jobNoList: any = [];
  jobNoListFilter: any = [];
  accountList: any = [];
  usersList: any[] = [];
  appendAccountList: any = [];

  filterBy: string = 'Started';
  locId: any = localStorage.getItem('locId');
  jobNoStartDate: string;
  jobNoEndDate: string;
  jobNoRemarks: string;

  account: any = null;
  commission: any = 0;

  constructor(
    private tostr: ToastrService,
    private apiService: ApiService,
    private dp: DatePipe
  ) {}

  ngOnInit() {
    this.getCostCentre();
    this.getShareHolder();
    this.getUserList();
  }

  getCostCentre() {
    this.apiService.getData('Sale/GetCostCentre').subscribe((data) => {
      this.costCentreList = data;
    });
  }

  getShareHolder() {
    this.apiService.getData('Sale/GetShareHolderList').subscribe((data) => {
      this.accountList = data;
    });
  }

  getUserList() {
    this.apiService
      .getDataById('Auth/GetUsersList', { locId: (this.locId == 'HO') ? '%' : this.locId })
      .subscribe((data) => {
        this.usersList = data;
      });
  }

  // #region COST CENTRE

  newCostCentre() {
    this.refreshCostCentre();
    this.isDisabledCostCentre = false;
    this.isShowCostCentre = true;

    setTimeout(() => {
      document.getElementById('costCentreName').focus();
    }, 200);
  }

  saveCostCentre() {
    if (this.costCentreName == '') {
      this.tostr.warning('Enter Farm Name ....!');
      return;
    }

    const obj = {
      id: this.costCentreId,
      name: this.costCentreName,
      commission: this.costCommission,
      comType: this.comType,
      rent: this.rent,
      rentInst: this.rentInstallment,
      userId: this.userId ?? 0,
    };

    this.apiService.saveObj('Sale/SaveCostCentre', obj).subscribe((result) => {
      if (result == true || result == 'true') {
        this.tostr.success('Save Successfully');
        this.refreshCostCentre();
        this.getCostCentre();
      } else {
        this.tostr.error('Please Save Again');
      }
    });
  }

  refreshCostCentre() {
    this.costCentreId = 0;
    this.costCentreName = '';
    this.costCommission = '';
    this.rent = '';
    this.rentInstallment = '';
    this.comType = 'A';
    this.userId = null;
    this.isDisabledCostCentre = true;
    this.isShowCostCentre = false;
  }

  editCostCentre(item: any) {
    this.costCentreId = item.CostcentreId;
    this.costCentreName = item.CostcentreName;
    this.costCommission = item.comm;
    this.rent = item.Rent;
    this.rentInstallment = item.RentInst;
    this.comType = item.ComType;
    this.userId = item.UserId;
    this.isDisabledCostCentre = false;
    this.isShowCostCentre = true;
  }

  deleteCostCentre(item: any) {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {
      const obj = {
        id: item.CostcentreId,
      };
      this.apiService.deleteData('Sale/DeleteCostCentre', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.getCostCentre();
            this.tostr.success('Delete Successfully');
          } else if (data == 'false' || data == false) {
            this.tostr.error('Delete Again');
          } else {
            this.tostr.warning('In Used');
          }
        },
        error: (error) => {
          this.tostr.info(error.error.text);
        },
      });
    }
  }

  // #endregion

  getJobNo() {
    if (this.ngFarmId == null) {
      this.jobNoList = [];
      this.ngJobNo = null;
      this.onJobNoChange(undefined);

      this.appendAccountList = [];
      this.account = null;
      this.commission = 0;
      return;
    }

    this.apiService
      .getDataById('Sale/GetJobNoById', { costCentreId: this.ngFarmId })
      .subscribe((data) => {
        this.jobNoList = data;
        this.finishedFilter();
      });
  }

  // #region JOB NO

  newJobNo() {
    this.refreshJobNo();
    this.isDisabledJobNo = false;
    this.isShowJobNo = true;
    this.daysCalculation();
    setTimeout(() => {
      document.getElementById('jobNo').focus();
    }, 200);
  }

  saveJobNo() {
    if (this.ngFarmId == null) {
      this.tostr.warning('Select Cost Centre ....!');
      return;
    }

    if (this.jobNo == '' || this.jobNo == 0) {
      this.tostr.warning('Enter Flock No ....!');
      return;
    }

    if (this.editMode == false) {
      const j = this.jobNoList.find((i) => i.JOBNO == this.jobNo);

      if (j != undefined) {
        this.tostr.info('Flock no already exist ....!');
        return;
      }
    }

    const sDate = this.jobStartDate
      ? this.dp.transform(this.jobStartDate, 'yyyy/MM/dd')
      : '';
    const eDate = this.jobEndDate
      ? this.dp.transform(this.jobEndDate, 'yyyy/MM/dd')
      : '';

    if (sDate == '' || sDate == '') {
      this.tostr.warning('Select Start Date ....!');
      return;
    }

    if (eDate == '' || eDate == '') {
      this.tostr.warning('Select End Date ....!');
      return;
    }

    const obj = {
      id: this.jobNoId == '' ? 0 : this.jobNoId,
      jobNo: parseInt(this.jobNo),
      startDate: sDate,
      endDate: eDate,
      remarks: this.remarks,
      totalChicks: this.totalChicks == '' ? 0 : this.totalChicks,
      weight: this.weight == '' ? 0 : this.weight,
      expense: this.expense == '' ? 0 : this.expense,
      days: this.days == '' ? 0 : this.days,
      finished: this.finished,
      costcenterId: this.ngFarmId,
    };
    
    this.apiService.saveObj('Sale/SaveJobNo', obj).subscribe((result) => {
      if (result == true || result == 'true') {
        this.tostr.success('Save Successfully');
        this.refreshJobNo();
        this.getJobNo();
      } else {
        this.tostr.error('Please Save Again');
      }
    });
  }

  refreshJobNo() {
    if (this.ngFarmId == null) {
      this.tostr.warning('Select Farm...!');
      return;
    }

    this.jobNoId = '';
    this.jobNo = '';
    this.jobStartDate = new Date();
    this.jobEndDate = this.addDays(new Date(), 45);
    this.remarks = '';
    this.totalChicks = '';
    this.weight = '';
    this.expense = '';
    this.isDisabledJobNo = true;
    this.isShowJobNo = false;
    this.editMode = false;
    this.days = '';
    this.finished = false;
  }

  editJobNo(item: any) {
    this.editMode = true;
    this.jobNoId = item.ID;
    this.jobNo = item.JOBNO;

    const s = item.STARTDATE.split('/');
    this.jobStartDate = new Date(s[2], s[1] - 1, s[0]);

    const e = item.ENDDATE.split('/');
    this.jobEndDate = new Date(e[2], e[1] - 1, e[0]);

    this.remarks = item.REMARKS;
    this.days = item.DAYS;
    this.finished = item.FINISHED;
    this.totalChicks = item.TOTALCHICKS;
    this.weight = item.WEIGHT;
    this.expense = item.EXPENSE;
    this.isDisabledJobNo = false;
    this.isShowJobNo = true;
  }

  deleteJobNo(item: any) {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {
      const obj = {
        id: item.ID,
        costCentreId: item.COSTCENTREID,
      };

      this.apiService.deleteData('Sale/DeleteJobNo', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.tostr.success('Delete Successfully');
            this.getJobNo();
          } else if (data == 'false' || data == false) {
            this.tostr.error('Delete Again');
          }
          else{
            this.tostr.info(data);
          }
        },
        error: (error) => {
          this.tostr.info("It cannot be deleted");
        },
      });
    }
  }

  // #endregion

  onJobNoChange(event: any) {
    this.jobNoStartDate = event ? event.STARTDATE : '';
    this.jobNoEndDate = event ? event.ENDDATE : '';
    this.jobNoRemarks = event ? event.REMARKS : '';
    this.days = event ? event.DAYS : '';
  }

  filterRadio(event: any) {
    this.filterBy = event.target.value;
    this.finishedFilter();
  }

  finishedFilter() {
    if (this.filterBy == 'Finished') {
      this.jobNoListFilter = this.jobNoList.filter((x) => x.FINISHED == true);
    }

    if (this.filterBy == 'Started') {
      this.jobNoListFilter = this.jobNoList.filter(
        (x) =>
          x.FINISHED != true
      );
    }
  }

  addDays(date: Date, days: any) {
    var result = new Date(date);
    result.setDate(result.getDate() + days);
    return result;
  }

  daysCalculation() {
    const s = this.jobStartDate;
    const e = this.jobEndDate;
    if (!s || !e) {
      this.days = '';
      return;
    }

    const startDate = new Date(s);
    const endDate = new Date(e);
    startDate.setHours(0, 0, 0, 0);
    endDate.setHours(0, 0, 0, 0);

    const oneDay = 24 * 60 * 60 * 1000;
    const startDays = Math.floor(startDate.getTime() / oneDay);
    const endDays = Math.floor(endDate.getTime() / oneDay);

    const days = endDays - startDays + 1;
    this.days = days;
  }

  //#region SHARE HOLDER

  addShareHolder() {
    if (this.account == null) {
      this.tostr.warning('Select Account...!');
      return;
    }

    if (this.commission == 0 || this.commission == '' || this.commission == undefined ) {
      this.tostr.warning('Enter Commission...!');
      return;
    }

    let obj: any = {};
    obj.code = this.account;
    obj.name = this.accountList.find((x) => x.code == this.account).name;
    obj.comm = this.commission;
    this.appendAccountList.push(obj);
  }

  onClickShareHolder() {
    if (this.ngFarmId == null) {
      this.tostr.warning('Select Farm...!');
      return;
    }

    this.apiService.getDataById('Sale/GetShareHolders', { id: this.ngFarmId}).subscribe((data) => {
      this.appendAccountList = data;
    });
  }

  deleteAccount(i: any) {
    if (i !== -1) {
      this.appendAccountList.splice(i, 1);
    }
  }

  saveShareHolder(){

    if(this.appendAccountList.length == 0){
      this.tostr.warning("First Add Account...!");
      return;
    }

    const invoice: any[] = this.appendAccountList.map((data) => ({
      Code: data.code,
      Share: data.comm,
      FarmId: this.ngFarmId,
      ShareType: this.ngShareType,
    }));

    this.apiService
      .saveData('Sale/SaveShareHolder', invoice)
      .subscribe((result) => {
        if (result == true || result == 'true') {
          this.tostr.success('Save Successfully');
        } else {
          this.tostr.error('Please Save Again');
        }
      });
  }

  //#endregion
}
