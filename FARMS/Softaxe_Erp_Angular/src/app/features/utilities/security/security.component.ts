import { Component, ElementRef, ViewChild } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';
import { ToastrService } from 'ngx-toastr';
import { DatePipe } from '@angular/common';
import { AuthService } from 'src/app/services/auth.service';

declare const $: any;

@Component({
  selector: 'app-security',
  templateUrl: './security.component.html',
  styleUrls: ['./security.component.css'],
})
export class SecurityComponent {
  constructor(
    private apiService: ApiService,
    private tostr: ToastrService,
    private auth: AuthService,
    private dp: DatePipe
  ) {}

  isDayClose = localStorage.getItem('dayClose');
  isApprovalSystem = localStorage.getItem('approvalSystem');

  // User Permissions
  locationList: any[] = [];
  ngLocation: any = null;
  getUsersList: any[] = [];
  ngUser: any = null;
  allowDashboard: boolean = false;

  // Voucher Authentication
  @ViewChild('tblDetailvt', { static: false }) tblDetailvt!: ElementRef;
  vtUsers: any = null;
  voucherTypes: any[] = [];
  allChk: boolean = false;

  // Month Close
  monthClose: any;
  monthOpen: any = null;
  autoDayClosing: any;

  // Day Close
  dayClose: any;

  ngOnInit(): void {
    this.getLocation();
    this.getMonthClose();
    
    this.Menu();

    $('body').on('click', '.chkMain', function () {
      if ($(this).is(':checked')) {
        $(this)
          .closest('li')
          .find('li')
          .each(function () {
            $(this).find('input').prop('checked', true);
          });
      } else {
        $(this)
          .closest('li')
          .find('li')
          .each(function () {
            $(this).find('input').prop('checked', false);
          });
      }
    });

    this.dayClose = new Date();
    this.autoDayClosing = new Date();
  }

  async getLocation() {
    const result = await this.apiService
      .getDataById('Admin/GetLocationById', { companyId: this.auth.cmpId() })
      .toPromise();
    this.locationList = result;
    this.ngLocation = this.auth.locId();
    this.getUsers();
    this.getDayClose();
  }

  getUsers() {
    if (this.ngLocation == null) {
      this.getUsersList = [];
      this.ngUser = null;
      return;
    }
    
    this.apiService
    .getDataById('Auth/GetUsersList', { locId: this.ngLocation })
    .subscribe((data) => {
      this.getUsersList = data;
      this.ngUser = parseInt(localStorage.getItem('userId'));
      this.onUserChange();
    });
  }

  onAllowDashboard(event: any) {
    if (this.ngUser == null) {
      event.target.checked = false;
      this.tostr.warning('Select User...!');
      return;
    }
    var obj = { userId: this.ngUser, status: this.allowDashboard };
    this.apiService
      .saveObj('Utilities/AllowDashboard', obj)
      .subscribe((data) => {
        if (data == true || data == 'true') {
          this.tostr.success('Update Successfully');
        } else {
          this.tostr.error('Please Save Again');
        }
      });
  }

  onUserChange() {
    this.clearChk();

    if (this.ngUser == null) {
      this.allowDashboard = false;
      return;
    }

    this.apiService
      .getDataById('Utilities/GetAllowForm', { userId: this.ngUser })
      .subscribe((data) => {
        this.allowDashboard = data.dashboard;
        $.each(data.allowForms, function (i, item) {
          $('#myUL')
            .find('ul')
            .find('li')
            .each(function () {
              var menuId = $(this).find('input').attr('id');

              if (menuId == item.Menuid) {
                $(this).find('input').prop('checked', true);

                $(this)
                  .closest('.parentli')
                  .find('.chkMain')
                  .prop('checked', true);
              }
            });
        });
      });
  }

  //================ Get Saved Permissions Check Boxes ===================

  
  Menu() {
    var myhtml = '';
    $('.securityLi').each(function () {
      var mainMenu = $(this).find('span').text();

      myhtml += `<li class="mb-1 parentli"><input type="checkbox" class="chkMain"><span class="caret">${mainMenu}</span>`;
      myhtml += '<ul class="nested ps-3">';

      $(this)
        .find('li')
        .each(function () {
          var subMenu;
          if($(this).text() == 'File Maintain Department SetupDesignation SetupHolidays SetupLeaves SettingTSR Setting Type SettingShift SettingReason SettingSalary Setllement LabelsSet Salary DaysSet Month and YearsAllow Provident Fund and EOBIOvertime Calculation Formula'){
            subMenu = 'File Maintain';
          } else if($(this).text() == 'TSR Setting Type SettingShift SettingReason Setting'){
            subMenu = 'TSR Setting';
          } else if($(this).text() == 'Employee Information Employee SetupEmployee SalaryEmployee Family Detail'){
            subMenu = 'Employee Information';
          } else if($(this).text() == 'Provident Fund ModuleBank EntryBank ReceiptProvident FundProvident LaonBank Amount ApprovalProvident Laon ApprovalProvident Fund LedgerProvident Fund ReportProvident Fund Profit'){
            subMenu = 'Provident Fund Module';
          } else if($(this).text() == 'Employee Deduction Income TaxE.O.B.IStaff LoanAdvance SalaryVehicle LoanInsurance EntryOther DeductionLoan Status'){
            subMenu = 'Employee Deduction';
          } else if($(this).text() == 'Deduction Report Employee Income Tax, Provident, EOBI, ReportEmployee Advance Salary Deduction ReportEmployee Loan ReportEmployee Loan LedgerEmployee All Deduction Report'){
            subMenu = 'Deduction Report';
          } else if($(this).text() == 'Incentive Employee IncentiveArrearsYearly BonusEmployee Bonus ReportLeaves Enchasement PaymentOverTimeOverTime Report Day WiseLeaves Enchasement Payment ReportIncentive ReportYearly Bonus and Arrears ReportOvertime Amount ReportLoan AdjustmentMake Leaves EnchasementLeave Enchasement Report'){
            subMenu = 'Incentive Employee';
          } else if($(this).text() == 'Employee Leaves LeavesLeave Balance ReportAttendance SystemYearly Leaves ReportAttendance Sheet'){
            subMenu = 'Employee Leaves';
          } else if($(this).text() == 'Salary Calculation Salary SheetPay SlipSalary Type'){
            subMenu = 'Salary Calculation';
          } else if($(this).text() == 'Reports Employee Listing All Employee Salary ReportAdvanced Salary ReportOther Deduction ReportMonthly deduction ReportVehicle Loan ReportProvident Loan ReportInsurance Loan ReportStaff Loan Balance ReportVehicle Loan Balance ReportProvident Loan Balance ReportInsurance Loan Balance ReportDepartment ListingDesignation Listing'){
            subMenu = 'Reports';
          } else if($(this).text() == 'Audit Voucher ApprovalAllow Leaves SameMake Payment VoucherMake Payable VoucherMake Salary Paid VoucherPaid Voucher'){
            subMenu = 'Audit';
          }
          else{
            subMenu = $(this).text();
          }
           
          var subMenuId = $(this).attr('id');
          myhtml += `<li class="d-flex align-items-center"><input class="me-1" type="checkbox" id="${subMenuId}"> ${subMenu}</li>`;
        });

      myhtml += '</ul>';
      myhtml += '</li>';
    });

    $('#myUL').html(myhtml);

    this.treeView();
  }

  //================ Tree View Check Boxes ===================

  treeView() {
    const togglers: NodeListOf<Element> = document.querySelectorAll('.caret');

    for (let i = 0; i < togglers.length; i++) {
      togglers[i].addEventListener('click', function (this: HTMLElement) {
        const nested = this.parentElement?.querySelector('.nested');
        if (nested) {
          nested.classList.toggle('active');
        }
        this.classList.toggle('caret-down');
      });
    }
  }

  //================ Clear Check Boxes ===================

  clearChk() {
    $('#myUL')
      .find('ul')
      .find('li')
      .each(function () {
        $(this).find('input').prop('checked', false);
        $(this).closest('.parentli').find('.chkMain').prop('checked', false);
      });
  }

  SavePermissionsMenu() {
    if (this.ngUser == null) {
      this.tostr.warning('Select User...!');
      return;
    }

    const body = [];

    document.querySelectorAll('#myUL ul li').forEach((li: HTMLLIElement) => {
      const menuId = li.querySelector('input')?.getAttribute('id');
      if (li.querySelector('input')?.checked) {
        const obj = {
          menuid: menuId,
          userid: this.ngUser,
        };

        body.push(obj);
      }
    });
    this.apiService
      .save('Utilities/SaveUpdateAllowForm', { userId: this.ngUser }, body)
      .subscribe((result) => {
        if (result == true || result == 'true') {
          this.tostr.success('Save Successfully');
        } else {
          this.tostr.error('Please Save Again');
        }
      });
  }

  //============================ VOUCHER TYPE AUTH ========================//
  getVchTypes() {
    if (this.vtUsers == null) {
      this.voucherTypes = [];
      this.allChk = false;
      return;
    }

    this.apiService
      .getDataById('Utilities/GetVchData', { userId: this.vtUsers })
      .subscribe((data) => {
        if (data[0].VCHTYPE == '') {
          this.tostr.warning('Data not found against selected user');
          return;
        } else {
          this.voucherTypes = data;
        }
      });
  }

  rowChecked(event: any, data: any) {
    if (event.target.checked == true) {
      data.CANFEED = true;
      data.CANVERIFY = true;
      data.CANUNVERIFY = true;
      data.CANAPPROVE = true;
      data.CANUNAPPROVE = true;
      data.CANAUDIT = true;
      data.CANUNAUDIT = true;
    } else {
      data.CANFEED = false;
      data.CANVERIFY = false;
      data.CANUNVERIFY = false;
      data.CANAPPROVE = false;
      data.CANUNAPPROVE = false;
      data.CANAUDIT = false;
      data.CANUNAUDIT = false;
    }
  }

  allChecked(event: any) {
    if (event.target.checked == true) {
      this.allChk = true;
      this.voucherTypes.forEach((x) => {
        x.CANFEED = true;
        x.CANVERIFY = true;
        x.CANUNVERIFY = true;
        x.CANAPPROVE = true;
        x.CANUNAPPROVE = true;
        x.CANAUDIT = true;
        x.CANUNAUDIT = true;
      });
    } else {
      this.allChk = false;
      this.voucherTypes.forEach((x) => {
        x.CANFEED = false;
        x.CANVERIFY = false;
        x.CANUNVERIFY = false;
        x.CANAPPROVE = false;
        x.CANUNAPPROVE = false;
        x.CANAUDIT = false;
        x.CANUNAUDIT = false;
      });
    }
  }

  columChecked(event: any, colum: any) {
    if (event.target.checked == true) {
      this.voucherTypes.forEach((x) => {
        if (colum == 'feed') {
          x.CANFEED = true;
        } else if (colum == 'verify') {
          x.CANVERIFY = true;
        } else if (colum == 'unverify') {
          x.CANUNVERIFY = true;
        } else if (colum == 'appove') {
          x.CANAPPROVE = true;
        } else if (colum == 'unapprove') {
          x.CANUNAPPROVE = true;
        } else if (colum == 'audit') {
          x.CANAUDIT = true;
        } else if (colum == 'unaudit') {
          x.CANUNAUDIT = true;
        }
      });
    } else {
      this.voucherTypes.forEach((x) => {
        if (colum == 'feed') {
          x.CANFEED = false;
        } else if (colum == 'verify') {
          x.CANVERIFY = false;
        } else if (colum == 'unverify') {
          x.CANUNVERIFY = false;
        } else if (colum == 'appove') {
          x.CANAPPROVE = false;
        } else if (colum == 'unapprove') {
          x.CANUNAPPROVE = false;
        } else if (colum == 'audit') {
          x.CANAUDIT = false;
        } else if (colum == 'unaudit') {
          x.CANUNAUDIT = false;
        }
      });
    }
  }

  //================= SAVE VCHTYPES ====================//

  SaveVoucherType() {
    if (this.vtUsers == null) {
      this.tostr.warning('Please Select a User');
      return;
    }

    if (this.voucherTypes.length == 0) {
      this.tostr.warning('There is no voucher for save');
      return;
    }

    const obj = this.voucherTypes.map((data) => ({
      vchType: data.VCHTYPE,
      stopEntry: data.CANFEED,
      canVerify: data.CANVERIFY,
      canUnVerify: data.CANUNVERIFY,
      canApprove: data.CANAPPROVE,
      canUnApprove: data.CANUNAPPROVE,
      canAudit: data.CANAUDIT,
      canUnAudit: data.CANUNAUDIT,
      uId: this.vtUsers,
    }));

    this.apiService
      .saveData('Utilities/SaveAllowVchType', obj)
      .subscribe((data) => {
        if (data == true || data == 'true') {
          this.tostr.success('Save Successfully');
        } else {
          this.tostr.error('Please Save Again');
        }
      });
  }

  //====================== Search Voucher Types ====================//

  onVchSearch(event: any): void {
    const tableElement = this.tblDetailvt.nativeElement;
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

  //======================= SAVE MONTH CLOSE ======================//

  async getMonthClose() {
    const data = await this.apiService
      .getData('Utilities/LastCloseMonth')
      .toPromise();

    if(data.length == 0){
      return
    }
    let month = data[0];
    this.monthClose = month.MonthClosingDate;
    this.monthOpen = this.dp.transform(month.MonthOpening, 'MM');
  }

  saveMonthClose() {
    let monthClosing = this.monthClose;
    let monthOpen = this.monthOpen;

    if (!monthClosing) {
      monthClosing = 0;
    }
    if (monthOpen == null) {
      monthOpen = 0;
    }

    var obj = {
      monthClose: parseInt(monthClosing),
      monthOpen: parseInt(monthOpen),
      autoClose: this.dp.transform(this.autoDayClosing, 'yyyy/MM/dd') ,
    };

    this.apiService
      .saveObj('Utilities/SaveUpdateMonthClose', obj)
      .subscribe((data) => {
        if (data == true || data == 'true') {
          this.tostr.success('Save Successfully');
        } else {
          this.tostr.error('Please Save Again');
        }
      });
  }

  //====================== Save Day Close ====================//

  async getDayClose() {

    if (this.ngLocation == null) {
      return;
    }

    const date = await this.apiService
      .getDataById('Utilities/LastCloseDate', {locId: this.ngLocation})
      .toPromise();
    let dp = date[0].DATE.split('/');
    this.dayClose = new Date(dp[2], dp[1] - 1, dp[0]);
  }

  saveDayClose() {

    if (this.ngLocation == null) {
      this.tostr.warning("Select Location...")
      return;
    }

    var obj = {
      dayClose: this.dp.transform(this.dayClose, 'yyyy/MM/dd'),
      dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      status: "Open",
      locId: this.ngLocation,
    };

    this.apiService
      .saveObj('Utilities/SaveUpdateDayClose', obj)
      .subscribe((data) => {
        if (data == true || data == 'true') {
          this.tostr.success('Save Successfully');
        } else {
          this.tostr.error('Please Save Again');
        }
      });
  }
}
