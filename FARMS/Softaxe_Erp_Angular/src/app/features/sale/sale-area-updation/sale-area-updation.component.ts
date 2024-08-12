import { DatePipe } from '@angular/common';
import {
  Component,
  ElementRef,
  OnInit,
  ViewChild,
} from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { CommonService } from 'src/app/services/common.service';
declare const $: any;

@Component({
  selector: 'app-sale-area-updation',
  templateUrl: './sale-area-updation.component.html',
  styleUrls: ['./sale-area-updation.component.css'],
})
export class SaleAreaUpdationComponent implements OnInit {
  // SALE MANAGER
  saleManagerList: any[] = [];
  isDisabledsaleManager: boolean = true;
  isShowsaleManager: boolean = false;
  saleManagerName = '';
  saleManagerId = 0;
  saleManager: any = null;

  // MAIN AREA
  mainAreaList: any[] = [];
  mainAreaName = '';
  mainAreaId = 0;
  isDisabledMainArea = true;
  isShowMainArea = false;

  // SUB AREA
  subAreaList: any[] = [];
  subAreaName = '';
  subAreaId = 0;
  isDisabledSubArea = true;
  isShowSubArea = false;

  // ORDER TAKER
  orderTakerList: any[] = [];
  ordertaker: any = null;
  recovery: any;
  commission: any;
  aboveCommission: any;
  recoveryTarget: any;

  // AREAS LIST
  areaList: any[] = [];
  searchQuery: any = '';
  filer: any = 'all';

  @ViewChild('areaLists', { static: false }) areaLists!: ElementRef;

  constructor(
    private apiService: ApiService,
    private tostr: ToastrService,
    private dp: DatePipe,
    private com: CommonService
  ) {}

  ngOnInit(): void {
    this.getsaleManagerList();
    this.getorderTakerList();
    this.getMainArea();
    this.getAreaList();
  }

  //======================= SALE MANAGER =======================//

  getsaleManagerList() {
    this.apiService.getData('Sale/SaleManagerList').subscribe((data) => {
      this.saleManagerList = data;
    });
  }

  newsaleManager() {
    this.refreshsaleManager();
    this.isDisabledsaleManager = false;
    this.isShowsaleManager = true;
  }

  refreshsaleManager() {
    this.saleManagerName = '';
    this.saleManagerId = 0;
    this.isDisabledsaleManager = true;
    this.isShowsaleManager = false;
  }

  createUpdatesaleManager() {
    if (this.saleManagerName == '' || this.saleManagerName == null) {
      this.tostr.warning('Enter Sale Manager....!');
      return;
    }

    try {
      this.com.showLoader();
      const obj = {
        id: this.saleManagerId,
        name: this.saleManagerName,
        dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      };

      this.apiService
        .saveObj('Sale/AddUpdateSalesManager', obj)
        .subscribe((result) => {
          if (result == true || result == 'true') {
            this.getsaleManagerList();
            this.tostr.success('Save Successfully');
            this.refreshsaleManager();
            this.com.hideLoader();
          } else {
            this.com.hideLoader();
            this.tostr.error('Please Save Again');
          }
        });
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  editsaleManager(id: any, name: any): void {
    this.saleManagerName = name;
    this.saleManagerId = id;
    this.isDisabledsaleManager = false;
    this.isShowsaleManager = true;
  }

  deletesaleManager(id: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (!confirmDelete) {
      return;
    }
    try {
      this.com.showLoader();
      const obj = {
        id: id,
        dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      };

      this.apiService.deleteData('Sale/DeleteSalesManager', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.tostr.success('Delete Successfully');
            this.refreshsaleManager();
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
      //this.com.hideLoader();
    }
  }

  //======================= ORDER TAKER COMMISSION =======================//

  getorderTakerList() {
    this.apiService.getData('Sale/GetOrderTakerList').subscribe((data) => {
      this.orderTakerList = data;
    });
  }

  onClickPopup() {
    if (this.ordertaker == null) {
      this.tostr.warning('Select Order Taker....!');
      return;
    }
    this.apiService
      .getDataById('Sale/GetCommission', { userId: this.ordertaker })
      .subscribe((data) => {
        
        const ot = data[0];
        this.aboveCommission = ot.ABOVECOMMISSION;
        this.commission = ot.COMMISSION;
        this.recovery = ot.RECOVERY;
        this.recoveryTarget = ot.TARGET;

        $('#OrderTakerComissionModal').modal('show');
      });
  }

  onClickSaveCom() {
    try {
      this.com.showLoader();
      var obj = {
        recovery: this.recovery,
        commission: this.commission,
        aboveCommission: this.aboveCommission,
        target: this.recoveryTarget,
        userId: this.ordertaker,
      };

      this.apiService
        .saveObj('Sale/AddUpdateCommission', obj)
        .subscribe((result) => {
          if (result == true || result == 'true') {
            this.tostr.success('Save Successfully');
            this.com.hideLoader();
          } else {
            this.com.hideLoader();
            this.tostr.error('Please Save Again');
          }
        });
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  //======================= MAIN AREA =======================//

  onClearMainArea() {
    this.subAreaList = [];
  }

  getMainArea() {
    this.apiService.getData('Sale/GetMainArea').subscribe((data) => {
      this.mainAreaList = data;
    });
  }

  newMainArea() {
    this.refreshMainArea();
    this.isDisabledMainArea = false;
    this.isShowMainArea = true;
  }

  refreshMainArea() {
    this.mainAreaName = '';
    this.mainAreaId = 0;
    this.isDisabledMainArea = true;
    this.isShowMainArea = false;
  }

  createUpdateMainArea() {
    try {
      this.com.showLoader();
      const obj = {
        id: this.mainAreaId,
        name: this.mainAreaName,
        dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      };

      this.apiService
        .saveObj('Sale/AddUpdateMainArea', obj)
        .subscribe((result) => {
          if (result == true || result == 'true') {
            this.getMainArea();
            this.tostr.success('Save Successfully');
            this.refreshMainArea();
            this.com.hideLoader();
          } else {
            this.com.hideLoader();
            this.tostr.error('Please Save Again');
          }
        });
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  editMainArea(id: any, name: any): void {
    this.mainAreaName = name;
    this.mainAreaId = id;
    this.isDisabledMainArea = false;
    this.isShowMainArea = true;
  }

  deleteMainArea(id: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {
      try {
        this.com.showLoader();
        const obj = {
          id: id,
          dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
        };

        this.apiService.deleteData('Sale/DeleteMainArea', obj).subscribe({
          next: (data) => {
            if (data == 'true' || data == true) {
              this.getMainArea();
              this.tostr.success('Delete Successfully');
              this.refreshMainArea();
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
        //this.com.hideLoader();
      }
    }
  }

  //======================= SUB AREA =======================//

  onChangeMainAera(event: any) {
    if (event == undefined) {
      this.subAreaList = [];
      return;
    }

    this.mainAreaId = event.id;
    var obj = {
      mainAreaId: event.id,
    };

    this.apiService.getDataById('Sale/GetSubArea', obj).subscribe((data) => {
      this.subAreaList = data;
    });
  }

  newSubArea() {
    this.refreshSubArea();
    this.isDisabledSubArea = false;
    this.isShowSubArea = true;
  }

  refreshSubArea() {
    this.subAreaName = '';
    this.subAreaId = 0;
    this.isDisabledSubArea = true;
    this.isShowSubArea = false;
  }

  createUpdateSubArea() {
    try {
      this.com.showLoader();
      const obj = {
        mainAreaId: this.mainAreaId,
        id: this.subAreaId,
        name: this.subAreaName,
        dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      };

      this.apiService
        .saveObj('Sale/AddUpdateSubArea', obj)
        .subscribe((result) => {
          if (result == true || result == 'true') {
            this.onChangeMainAera({ id: obj.mainAreaId });
            this.tostr.success('Save Successfully');
            this.refreshSubArea();
            this.com.hideLoader();
          } else {
            this.com.hideLoader();
            this.tostr.error('Please Save Again');
          }
        });
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  editSubArea(id: any, name: any): void {
    this.subAreaName = name;
    this.subAreaId = id;
    this.isDisabledSubArea = false;
    this.isShowSubArea = true;
  }

  deleteSubArea(id: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {
      try {
        this.com.showLoader();
        const obj = {
          mainAreaId: this.mainAreaId,
          id: id,
          dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
        };

        this.apiService.deleteData('Sale/DeleteSubArea', obj).subscribe({
          next: (data) => {
            if (data == 'true' || data == true) {
              this.onChangeMainAera({ id: obj.mainAreaId });
              this.tostr.success('Delete Successfully');
              this.refreshSubArea();
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
        //this.com.hideLoader();
      }
    }
  }

  // AREA LIST

  getAreaList() {
    this.apiService.getData('Sale/GetAreaList').subscribe((data) => {
      this.areaList = data;
    });
  }

  onChangeOT() {
    if (this.ordertaker == null) {
      this.areaList.forEach((x, i) => {
        x.isSelected = false;
      });
      return;
    }

    this.apiService
      .getDataById('Sale/GetOTAreas', { id: this.ordertaker })
      .subscribe((data) => {
        this.areaList.forEach((x, i) => {
          if (data.find((z) => z.AREAID === x.subAreaId)) {
            x.isSelected = true;
          } else {
            x.isSelected = false;
          }
        });
      });
  }

  selectAll(event: any) {
    if (event.target.checked) {
      this.areaList.forEach((x, i) => {
        x.isSelected = true;
      });
    } else {
      this.areaList.forEach((x, i) => {
        x.isSelected = false;
      });
    }
  }

  onSaveAreaList() {
    if (this.saleManager == null) {
      this.tostr.warning('Select Sale Manager....!');
      return;
    }

    if (this.ordertaker == null) {
      this.tostr.warning('Select Order Taker....!');
      return;
    }

    const list = this.areaList.filter((x) => x.isSelected == true);

    try {
      this.com.showLoader();
      const areas: any[] = list.map((item) => ({
        OrderTakerId: this.ordertaker,
        AreaId: item.subAreaId,
        SaleManagerId: this.saleManager,
        DtNow: new Date(),
      }));

      this.apiService
        .saveData('Sale/AddUpdateOTArea', areas)
        .subscribe((result) => {
          if (result == true || result == 'true') {
            this.com.hideLoader();
            this.tostr.success('Save Successfully');
          } else {
            this.com.hideLoader();
            this.tostr.error('Please Save Again');
          }
        });
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  onFilterChange(tag: any) {
    this.filer = tag;
    this.searchGrid();
  }

  searchGrid() {
    const tableElement = this.areaLists.nativeElement;
    const rows = tableElement.querySelectorAll('tr');

    rows.forEach((row: HTMLTableRowElement) => {
      let isShow = true;

      if (this.filer == 'selected') {
        if (row.querySelector('input').checked) {
          isShow = true;
        } else {
          isShow = false;
        }
      }

      if (this.filer == 'unselected') {
        if (row.querySelector('input').checked) {
          isShow = false;
        } else {
          isShow = true;
        }
      }

      if (isShow) {
        if (
          row.textContent &&
          row.textContent
            .toLowerCase()
            .indexOf(this.searchQuery.toLowerCase()) > -1
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
}
