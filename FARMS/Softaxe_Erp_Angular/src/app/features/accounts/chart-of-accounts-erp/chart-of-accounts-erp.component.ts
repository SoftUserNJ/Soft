import {
  Component,
  ElementRef,
  ViewChild,
  ChangeDetectorRef,
} from '@angular/core';
import { ApiService } from 'src/app/services/api.service';
import { ChartOfAccount, COALevel } from 'src/app/models/ChartofAccount';
import { DatePipe } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { CommonService } from 'src/app/services/common.service';
declare const $: any;

@Component({
  selector: 'app-chart-of-accounts-erp',
  templateUrl: './chart-of-accounts-erp.component.html',
  styleUrls: ['./chart-of-accounts-erp.component.css']
})
export class ChartOfAccountsErpComponent {

  isShowPage: boolean = true;
  cmpId: any = localStorage.getItem('cmpId');
  locWise: any = localStorage.getItem('locWise');
  locId: any = localStorage.getItem('locId');
  locName: any = localStorage.getItem('locName');

  l1Code = '';
  l2Code = '';
  l3Code = '';
  l4Code = '';
  l5Code = '';

  selectedLevel1: any;
  selectedLevel2: any;
  selectedLevel3: any;
  selectedLevel4: any;
  selectedLevel5: any;

  coaLvl1: COALevel[] = [];
  coaLvl2: COALevel[] = [];
  coaLvl3: COALevel[] = [];
  coaLvl4: COALevel[] = [];
  coaLvl5: COALevel[] = [];
  locationList4: any[] = [];
  locationList5: any[] = [];

  // SEARCH

  chartOfAccount: ChartOfAccount[] = [];
  searchLvl2: COALevel[] = [];
  searchLvl3: COALevel[] = [];
  search1: any;
  search2: any;
  search3: any;
  l1Search = '';
  l2Search = '';
  l3Search = '';
  l4Search = '';
  l5Search = '';

  // LEVEL 1
  level1Name = '';
  level1Code = '';
  isDisabledLevel1: boolean = true;
  isFocusLevel1: boolean = false;
  isShowLevel1: boolean = false;

  // LEVEL 2
  level2Name = '';
  level2Code = '';
  isDisabledLevel2: boolean = true;
  isFocusLevel2: boolean = false;
  isShowLevel2: boolean = false;

  // LEVEL 3
  level3Name = '';
  level3Code = '';
  isDisabledLevel3: boolean = true;
  isFocusLevel3: boolean = false;
  isShowLevel3: boolean = false;

  // LEVEL 4
  level4Name = '';
  level4Code = '';
  level4Tag = '';
  level4Tag1 = '';
  isDisabledLevel4: boolean = true;
  isFocusLevel4: boolean = false;
  isShowLevel4: boolean = false;

  saleCodeDD = null ;
  saleCodeList:any[] = [];
  saleCodeHide:boolean = true;

  // LEVEL 5
  level5Name = '';
  level5Code = '';
  isDisabledLevel5: boolean = true;
  isFocusLevel5: boolean = false;
  isShowLevel5: boolean = false;

  @ViewChild('coaList', { static: false }) coaList!: ElementRef;

  constructor(
    private apiService: ApiService,
    private toast: ToastrService,
    private dp: DatePipe,
    private cdr: ChangeDetectorRef,
    private com: CommonService
  ) {}

  ngOnInit(): void {
    this.getCOAList();
    this.getLevel1();
    this.getLocation();
  }

  togglePages() {
    this.isShowPage = !this.isShowPage;
    if (this.isShowPage == true) {
      this.getCOAList();
      this.refresh();
    } else {
      this.coaLvl2 = [];
      this.coaLvl3 = [];
      this.coaLvl4 = [];
      this.coaLvl5 = [];
      this.selectedLevel1 = null;
      this.selectedLevel2 = null;
      this.selectedLevel3 = null;
      this.selectedLevel4 = null;
      this.selectedLevel5 = null;
    }
  }

  getCOAList() {
    this.com.showLoader();
    this.apiService
      .getData('accounts/getcoa')
      .subscribe((result: ChartOfAccount[]) => {
        this.chartOfAccount = result;
        this.com.hideLoader();
      });
  }

  getLocation() {
    this.apiService
      .getDataById('Admin/GetLocationById', { companyId: this.cmpId })
      .subscribe((data) => {
        let l4 = data;
        l4.CHECKEDL4 = false;
        this.locationList4 = l4;

        let l5 = data;
        l5.CHECKEDL5 = false;
        this.locationList5 = l5;
      });
  }

  getLoc(tag: any) {
    let mapCode;

    if (tag == 'L4') {
      const l = this.locationList4.filter((x) => x.CHECKEDL4 == true);
      mapCode = l.map((item) => item.ID).join(',');
    }

    if (tag == 'L5') {
      const l = this.locationList5.filter((x) => x.CHECKEDL5 == true);
      mapCode = l.map((item) => item.ID).join(',');
    }

    return mapCode;
  }

  columChecked(event: any, tag: any) {
    if (tag == 'l4') {
      if (event.target.checked == true) {
        this.locationList4.forEach((x) => {
          x.CHECKEDL4 = true;
        });
      } else {
        this.locationList4.forEach((x) => {
          x.CHECKEDL4 = false;
        });
      }
    }
    if (tag == 'l5') {
      if (event.target.checked == true) {
        this.locationList5.forEach((x) => {
          x.CHECKEDL5 = true;
        });
      } else {
        this.locationList5.forEach((x) => {
          x.CHECKEDL5 = false;
        });
      }
    }
  }

  selectedLocation(tag: any, code: any) {
    this.locationList4.forEach((x) => {
      x.CHECKEDL4 = false;
    });

    this.locationList5.forEach((x) => {
      x.CHECKEDL5 = false;
    });

    $('.headCheckBox').prop('checked', false);

    if (code) {
      if (tag == 'l4') {
        const l4: any = this.coaLvl4.find((x) => x.code == code);

        l4.MAPCODE.split(',');
        for (let i = 0; i < this.locationList4.length; i++) {
          const item = this.locationList4[i];

          if (l4.MAPCODE.includes(item.ID)) {
            item.CHECKEDL4 = true;
          }
        }
      } else if (tag == 'l5') {
        const l5: any = this.coaLvl5.find((x) => x.code == code);

        l5.MAPCODE.split(',');
        for (let i = 0; i < this.locationList5.length; i++) {
          const item = this.locationList5[i];

          if (l5.MAPCODE.includes(item.ID)) {
            item.CHECKEDL5 = true;
          }
        }
      }
    }
  }

  //====================== LEVEL 1 ======================//

  getLevel1() {
    const obj = {
      status: 'L1',
    };

    this.apiService.getDataById('accounts/GetLevels', obj).subscribe((data) => {
      this.coaLvl1 = data;
    });
  }

  newLevel1() {
    this.refreshLevel1();
    this.isDisabledLevel1 = false;
    this.isShowLevel1 = true;
    this.isFocusLevel1 = true;
  }

  refreshLevel1() {
    this.level1Name = '';
    this.level1Code = '';
    this.isDisabledLevel1 = true;
    this.isShowLevel1 = false;
  }

  createUpdateLevel1() {
    if (this.level1Name.trim() == '' || this.level1Name == null) {
      this.toast.warning('Enter Level1 Name...');
      return;
    }

    try {
      this.com.showLoader();

      const obj = {
        code: this.level1Code,
        name: this.level1Name,
        status: 'L1',
        dtNow: new Date(),
      };

      this.apiService
        .saveData('accounts/SaveUpdateLevels', obj)
        .subscribe((result) => {
          if (result == true || result == 'true') {
            this.getLevel1();
            this.toast.success('Level 1 Save Successfully');
            this.refreshLevel1();
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

  editLevel1(code: any, name: any, notChange: boolean): void {
    if (notChange) {
      this.toast.info('Cannot Edit Level 1');
      return;
    }

    this.level1Name = name;
    this.level1Code = code;
    this.isDisabledLevel1 = false;
    this.isShowLevel1 = true;
  }

  deleteLevel1(code: any, notChange: boolean): void {
    if (notChange) {
      this.toast.info('Cannot Delete Level 1');
      return;
    }

    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (!confirmDelete) {
      return;
    }

    const obj = {
      code: code,
      status: 'L1',
      dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
    };

    try {
      this.com.showLoader();

      this.apiService.deleteData('accounts/DeleteLevels', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.getLevel1();
            this.toast.success('Level 1 Delete Successfully');
            this.refreshLevel1();
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

  //===================== LEVEL 2 ========================//

  onLevel1Change(event: any) {
    if (event == undefined) {
      this.coaLvl2 = [];
      this.coaLvl3 = [];
      this.coaLvl4 = [];
      this.coaLvl5 = [];
      this.selectedLevel2 = null;
      this.selectedLevel3 = null;
      this.selectedLevel4 = null;
      this.selectedLevel5 = null;
      this.l1Code = '0';
      this.l2Code = '0';
      this.l3Code = '0';
      this.l4Code = '0';
      this.l5Code = '0';
      return;
    }

    this.l1Code = event.code;

    var obj = {
      code: event.code,
      status: 'L2',
    };

    this.apiService.getDataById('accounts/GetLevels', obj).subscribe((data) => {
      this.coaLvl2 = data;
    });

    this.coaLvl3 = [];
    this.coaLvl4 = [];
    this.coaLvl5 = [];
    this.selectedLevel2 = null;
    this.selectedLevel3 = null;
    this.selectedLevel4 = null;
    this.selectedLevel5 = null;
  }

  newLevel2() {
    this.refreshLevel2();
    this.isDisabledLevel2 = false;
    this.isShowLevel2 = true;
    this.isFocusLevel2 = true;
  }

  refreshLevel2() {
    this.level2Name = '';
    this.level2Code = '';
    this.isDisabledLevel2 = true;
    this.isShowLevel2 = false;
  }

  createUpdateLevel2() {
    if (this.l1Code == '0' || this.l1Code == '') {
      this.toast.warning('Select Level1...');
      return;
    }

    if (this.level2Name.trim() == '' || this.level2Name == null) {
      this.toast.warning('Enter Level2 Name...');
      return;
    }

    try {
      this.com.showLoader();
      const obj = {
        preLvl: this.l1Code,
        code: this.level2Code,
        name: this.level2Name,
        status: 'L2',
        dtNow: new Date(),
      };

      this.apiService
        .saveData('accounts/SaveUpdateLevels', obj)
        .subscribe((result) => {
          if (result == true || result == 'true') {
            this.onLevel1Change({ code: obj.preLvl });
            this.toast.success('Level 2 Save Successfully');
            this.refreshLevel2();
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

  editLevel2(code: any, name: any, notChange: boolean): void {
    if (notChange) {
      this.toast.info('Cannot Edit Level 2');
      return;
    }

    this.level2Name = name;
    this.level2Code = code;
    this.isDisabledLevel2 = false;
    this.isShowLevel2 = true;
  }

  deleteLevel2(code: any, notChange: boolean): void {
    if (notChange) {
      this.toast.info('Cannot Delete Level 2');
      return;
    }

    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (!confirmDelete) {
      return;
    }

    try {
      this.com.showLoader();

      const obj = {
        code: code,
        status: 'L2',
        dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      };

      this.apiService.deleteData('accounts/DeleteLevels', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.onLevel1Change({ code: this.l1Code });
            this.toast.success('Level 2 Delete Successfully');
            this.refreshLevel2();
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

  //===================== LEVEL 3 ========================//

  onLevel2Change(event: any) {
    if (event == undefined) {
      this.coaLvl3 = [];
      this.coaLvl4 = [];
      this.coaLvl5 = [];
      this.selectedLevel3 = null;
      this.selectedLevel4 = null;
      this.selectedLevel5 = null;
      this.l2Code = '0';
      this.l3Code = '0';
      this.l4Code = '0';
      this.l5Code = '0';
      return;
    }

    this.l2Code = event.code;

    var obj = {
      code: event.code,
      status: 'L3',
    };

    this.apiService.getDataById('accounts/GetLevels', obj).subscribe((data) => {
      this.coaLvl3 = data;
    });

    this.coaLvl4 = [];
    this.coaLvl5 = [];
    this.selectedLevel3 = null;
    this.selectedLevel4 = null;
    this.selectedLevel5 = null;
  }

  newLevel3() {
    this.refreshLevel3();
    this.isDisabledLevel3 = false;
    this.isShowLevel3 = true;
    this.isFocusLevel3 = true;
  }

  refreshLevel3() {
    this.level3Name = '';
    this.level3Code = '';
    this.isDisabledLevel3 = true;
    this.isShowLevel3 = false;
  }

  createUpdateLevel3() {
    if (this.l1Code == '0' || this.l1Code == '') {
      this.toast.warning('Select Level1...');
      return;
    }

    if (this.l2Code == '0' || this.l2Code == '') {
      this.toast.warning('Select Level2...');
      return;
    }

    if (this.level3Name.trim() == '' || this.level3Name == null) {
      this.toast.warning('Enter Level3 Name...');
      return;
    }

    try {
      this.com.showLoader();

      const obj = {
        preLvl: this.l2Code,
        code: this.level3Code,
        name: this.level3Name,
        status: 'L3',
        dtNow: new Date(),
      };

      this.apiService
        .saveData('accounts/SaveUpdateLevels', obj)
        .subscribe((result) => {
          if (result == true || result == 'true') {
            this.onLevel2Change({ code: obj.preLvl });
            this.toast.success('Level 3 Save Successfully');
            this.refreshLevel3();
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

  editLevel3(code: any, name: any, notChange: boolean): void {
    if (notChange) {
      this.toast.info('Cannot Edit Level 3');
      return;
    }

    this.level3Name = name;
    this.level3Code = code;
    this.isDisabledLevel3 = false;
    this.isShowLevel3 = true;
  }

  deleteLevel3(code: any, notChange: boolean): void {
    if (notChange) {
      this.toast.info('Cannot Delete Level 3');
      return;
    }

    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (!confirmDelete) {
      return;
    }

    try {
      this.com.showLoader();
      const obj = {
        code: code,
        status: 'L3',
        dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      };

      this.apiService.deleteData('accounts/DeleteLevels', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.onLevel2Change({ code: this.l2Code });
            this.toast.success('Level 3 Delete Successfully');
            this.refreshLevel3();
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

  //===================== LEVEL 4 ========================//

  onLevel3Change(event: any) {
    if (event == undefined) {
      this.coaLvl4 = [];
      this.coaLvl5 = [];
      this.selectedLevel4 = null;
      this.selectedLevel5 = null;
      this.l3Code = '0';
      this.l4Code = '0';
      this.l5Code = '0';
      return;
    }

    this.l3Code = event.code;
    var obj = {
      code: event.code,
      status: 'L4',
    };

    this.apiService.getDataById('accounts/GetLevels', obj).subscribe((data) => {
      this.coaLvl4 = data;
    });

    this.coaLvl5 = [];
    this.selectedLevel4 = null;
    this.selectedLevel5 = null;
  }

  newLevel4() {
    this.refreshLevel4();
    this.isDisabledLevel4 = false;
    this.isShowLevel4 = true;
    this.isFocusLevel4 = true;

    const l4Loc = this.locationList4.find((x) => x.ID == this.locId)
    l4Loc.CHECKEDL4 = true;
  }

  refreshLevel4() {
    this.level4Name = '';
    this.level4Code = '';
    this.level4Tag = '';
    this.level4Tag1 = '';
    this.isDisabledLevel4 = true;
    this.isShowLevel4 = false;
    this.selectedLocation('l4', '');

    this.saleCodeHide = true;
    this.saleCodeList = [];
    this.saleCodeDD = null;
  }

  createUpdateLevel4() {

    if (this.l1Code == '0' || this.l1Code == '') {
      this.toast.warning('Select Level1...');
      return;
    }

    if (this.l2Code == '0' || this.l2Code == '') {
      this.toast.warning('Select Level2...');
      return;
    }

    if (this.l3Code == '0' || this.l3Code == '') {
      this.toast.warning('Select Level3...');
      return;
    }

    if (this.level4Name.trim() == '' || this.level4Name == null) {
      this.toast.warning('Enter Level4 Name...');
      return;
    }

    if ((this.level4Tag == 'S' || this.level4Tag == 's') && this.saleCodeDD == null) {
      this.toast.warning('Select Sale Code...');
      return;
    }

    try {
      this.com.showLoader();

      const obj = {
        preLvl: this.l3Code,
        code: this.level4Code,
        name: this.level4Name,
        tag: this.level4Tag,
        tag1: this.level4Tag1,
        saleCode: this.saleCodeDD == null ? "" : this.saleCodeDD,
        status: 'L4',
        mapCode: this.getLoc('L4'),
        dtNow: new Date(),
      };

      this.apiService
        .saveData('accounts/SaveUpdateLevels', obj)
        .subscribe((result) => {
          if (result == true || result == 'true') {
            this.onLevel3Change({ code: obj.preLvl });
            this.toast.success('Level 4 Save Successfully');

            this.refreshLevel4();
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

  editLevel4(i: any): void {

    if (i.notChange) {
      this.toast.info('Cannot Edit Level 4');
      return;
    }

    this.selectedLocation('l4', i.code);

    this.level4Name = i.name;
    this.level4Code = i.code;
    this.level4Tag = i.tag;
    this.level4Tag1 = i.tag1;
    this.saleCodeDD = i.saleCode;
    this.isDisabledLevel4 = false;
    this.isShowLevel4 = true;
    this.getSaleCode();
    // if(i.tag == 'S' || i.tag == 's'){

    //   this.apiService.getDataById('Common/GetLevel4CodeNameByTag', { tag: 'J' }).subscribe((data) => {
    //   this.saleCodeList = data;
    //   this.saleCodeDD = i.saleCode;
    //   this.saleCodeHide = false;
    //   this.saleCodeHide = true;
    //   });

    // }
    // else{
    //   this.saleCodeList = [];
    //   this.saleCodeDD = undefined;
    //   this.saleCodeHide = true;
    // }

  }

  deleteLevel4(code: any, notChange: boolean): void {
    if (notChange) {
      this.toast.info('Cannot Delete Level 4');
      return;
    }

    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (!confirmDelete) {
      return;
    }

    try {
      this.com.showLoader();

      const obj = {
        code: code,
        status: 'L4',
        dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      };

      this.apiService.deleteData('accounts/DeleteLevels', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.onLevel3Change({ code: this.l3Code });
            this.toast.success('Level 4 Delete Successfully');
            this.refreshLevel4();
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

  getSaleCode() {

    if (this.level4Tag.toLowerCase() == 's' || this.level4Tag.toLowerCase() == 'j') {

      let tag = (this.level4Tag.toLowerCase() == 's') ? 'J' : (this.level4Tag.toLowerCase() == 'j') ? 'S' : ''

      this.saleCodeHide = false;
      this.apiService.getDataById('Common/GetLevel4CodeNameByTag', { tag: tag }).subscribe((data) => {
        this.saleCodeList = data;
      });
    } else {
      this.saleCodeHide = true;
      this.saleCodeList = [];
    }
  }

  //===================== LEVEL 5 ========================//

  onLevel4Change(event: any) {
    if (event == undefined) {
      this.coaLvl5 = [];
      this.selectedLevel5 = null;
      this.l4Code = '0';
      this.l5Code = '0';
      return;
    }
    this.l4Code = event.code;

    var obj = {
      code: event.code,
      status: 'L5',
    };

    this.apiService.getDataById('accounts/GetLevels', obj).subscribe((data) => {
      this.coaLvl5 = data;
    });

    this.selectedLevel5 = null;
  }

  newLevel5() {
    this.refreshLevel5();
    this.isDisabledLevel5 = false;
    this.isShowLevel5 = true;
    this.isFocusLevel5 = true;
    const l5Loc = this.locationList5.find((x) => x.ID == this.locId)
    l5Loc.CHECKEDL5 = true;
  }

  refreshLevel5() {
    this.level5Name = '';
    this.level5Code = '';
    this.isDisabledLevel5 = true;
    this.isShowLevel5 = false;
    this.selectedLocation('l5', '');
    const l4Tag = this.coaLvl4.find((x) => x.code == this.selectedLevel4)
    const tag = ["J", "S", "D", "C"];
    if(tag.includes(l4Tag.tag1)){
      this.toast.warning("Cant Add Level 5 Account, Plz Change Level 4 Account")
      return;
    }
    $("#level5Modal").modal('show');
  }
  createUpdateLevel5() {
    if (this.l1Code == '0' || this.l1Code == '') {
      this.toast.warning('Select Level1...');
      return;
    }

    if (this.l2Code == '0' || this.l2Code == '') {
      this.toast.warning('Select Level2...');
      return;
    }

    if (this.l3Code == '0' || this.l3Code == '') {
      this.toast.warning('Select Level3...');
      return;
    }

    if (this.l4Code == '0' || this.l4Code == '') {
      this.toast.warning('Select Level3...');
      return;
    }

    if (this.level5Name.trim() == '' || this.level5Name == null) {
      this.toast.warning('Enter Level5 Name...');
      return;
    }

    const l4: any = this.coaLvl4.find((x) => x.code == this.l4Code);
    let mapCode = this.getLoc('L5');

    if(l4.MAPCODE){
      if(!(l4.MAPCODE).includes(mapCode)){
          this.toast.warning(`You can only select these ${l4.MAPCODE} ...`);
        return;
      }
    }

    try {
      this.com.showLoader();

      const obj = {
        preLvl: this.l4Code,
        code: this.level5Code,
        name: this.level5Name,
        status: 'L5',
        mapCode: mapCode,
        dtNow: new Date(),
      };

      this.apiService
        .saveData('accounts/SaveUpdateLevels', obj)
        .subscribe((result) => {
          if (result == true || result == 'true') {
            this.onLevel4Change({ code: obj.preLvl });
            this.toast.success('Level 5 Save Successfully');
            this.refreshLevel5();
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

  editLevel5(code: any, name: any, notChange: boolean): void {
    if (notChange) {
      this.toast.info('Cannot Edit Level 5');
      return;
    }

    this.selectedLocation('l5', code);

    this.level5Name = name;
    this.level5Code = code;
    this.isDisabledLevel5 = false;
    this.isShowLevel5 = true;
  }

  deleteLevel5(code: any, notChange: boolean): void {
    if (notChange) {
      this.toast.info('Cannot Delete Level 5');
      return;
    }

    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (!confirmDelete) {
      return;
    }

    try {
      this.com.showLoader();

      const obj = {
        code: code,
        status: 'L5',
        dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      };

      this.apiService.deleteData('accounts/DeleteLevels', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.onLevel4Change({ code: this.l4Code });
            this.toast.success('Level 5 Delete Successfully');
            this.refreshLevel5();
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

  onLevel5Change(event: any) {
    this.l5Code = event.code;
  }

  editCoaList(
    l1: any,
    l2: any,
    l3: any,
    l4: any,
    l5: any,
    name: any,
    notChange: boolean
  ): void {
    if (notChange) {
      this.toast.info('Cannot Edit Level 5');
      return;
    }

    try {
      this.com.showLoader();
      this.togglePages();
      $('#level5Modal').modal('show');
      this.onLevel1Change({ code: l1 });
      this.onLevel2Change({ code: l1 + l2 });
      this.onLevel3Change({ code: l1 + l2 + l3 });
      this.onLevel4Change({ code: l1 + l2 + l3 + l4 });
      this.selectedLevel1 = l1;
      this.selectedLevel2 = l1 + l2;
      this.selectedLevel3 = l1 + l2 + l3;
      this.selectedLevel4 = l1 + l2 + l3 + l4;
      this.selectedLevel5 = l1 + l2 + l3 + l4 + l5;
      this.newLevel5();
      this.cdr.detectChanges();
      this.level5Name = name;
      this.level5Code = l1 + l2 + l3 + l4 + l5;
      this.selectedLocation('l5', l1 + l2 + l3 + l4 + l5);
      this.com.hideLoader();
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  refresh() {
    this.searchLvl2 = [];
    this.searchLvl3 = [];
    this.search1 = null;
    this.search2 = null;
    this.search3 = null;
    this.l1Search = '';
    this.l2Search = '';
    this.l3Search = '';
    this.l4Search = '';
    this.l5Search = '';
    this.searchGrid();
  }

  //==================== Search Filter ===================//

  onL1Change(event: any) {
    this.l1Search = event == undefined ? '' : event.name;
    this.searchLvl2 = [];
    this.searchLvl3 = [];
    this.search2 = null;
    this.search3 = null;
    this.l2Search = '';
    this.l3Search = '';
    this.searchGrid();

    if (event == undefined) {
      return;
    }

    var obj = {
      code: event.code,
      status: 'L2',
    };

    this.apiService.getDataById('accounts/GetLevels', obj).subscribe((data) => {
      this.searchLvl2 = data;
    });
  }

  onL2Change(event: any) {
    this.l2Search = event == undefined ? '' : event.name;
    this.searchLvl3 = [];
    this.search3 = null;
    this.l3Search = '';
    this.searchGrid();

    if (event == undefined) {
      return;
    }

    var obj = {
      code: event.code,
      status: 'L3',
    };

    this.apiService.getDataById('accounts/GetLevels', obj).subscribe((data) => {
      this.searchLvl3 = data;
    });
  }

  onL3Change(event: any) {
    if (event == undefined) {
      this.l3Search = '';
      this.searchGrid();
    } else {
      this.l3Search = event.name;
      this.searchGrid();
    }
  }

  onInput() {
    this.searchGrid();
  }

  searchGrid() {
    const tableElement = this.coaList.nativeElement;
    const rows = tableElement.querySelectorAll('tr');

    rows.forEach((row: HTMLTableRowElement) => {
      let isShow = true;

      if (
        row.querySelector('.l1Name')?.textContent != this.l1Search &&
        this.l1Search.length > 0
      ) {
        isShow = false;
      }

      if (
        row.querySelector('.l2Name')?.textContent != this.l2Search &&
        this.l2Search.length > 0
      ) {
        isShow = false;
      }

      if (
        row.querySelector('.l3Name')?.textContent != this.l3Search &&
        this.l3Search.length > 0
      ) {
        isShow = false;
      }

      if (isShow) {
        const l4Name = row
          .querySelector('.l4Name')
          ?.textContent?.toLocaleLowerCase();
        if (
          l4Name !== null &&
          l4Name !== undefined &&
          l4Name.indexOf(this.l4Search.toLowerCase()) >= 0
        ) {
          isShow = true;
        } else {
          isShow = false;
        }
      }

      if (isShow) {
        const l5Name = row
          .querySelector('.l5Name')
          ?.textContent?.toLocaleLowerCase();
        if (
          l5Name !== null &&
          l5Name !== undefined &&
          l5Name.indexOf(this.l5Search.toLowerCase()) >= 0
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

  export(fileName: any, formate: any) {
    this.com.ExportFiles(fileName, formate);
  }

  onClickPrint(){
    let url = `ChartOfAccount?level1=${this.search1 ?? '%'}&level2=${this.search2 ?? '%'}&level3=${this.search3 ?? '%'}&compId=${this.cmpId}&locId=${this.locId ?? '%'}&LocName=${this.locName}`;
    this.com.viewReport(url);
  }
}
