import { Component, ElementRef, ViewChild } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';
import { ToastrService } from 'ngx-toastr';
import { DatePipe } from '@angular/common';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-wearhouse',
  templateUrl: './wearhouse.component.html',
  styleUrls: ['./wearhouse.component.css'],
})
export class WearhouseComponent {
  warehouseList: any[] = [];

  wId: number = 0;
  rId: number = 0;

  wName = '';
  rName = '';
  sName = '';

  selectedWarehouse: any;
  selectedRack: any;
  selectedShelf: any;

  // WAREHOUSE
  warehouse: any[] = [];
  isDisabledWarehouse: boolean = true;
  isShowWarehouse: boolean = false;
  warahouseName = '';
  warehouseId = 0;
  warehouseAlias = '';

  // RACK
  rack: any[] = [];
  isDisabledRack: boolean = true;
  isShowRack: boolean = false;
  rackName = '';
  rackId = 0;
  rackAlias = '';

  // SHELF
  shelf: any[] = [];
  isDisabledShelf: boolean = true;
  isShowShelf: boolean = false;
  shelfName = '';
  shelfId = 0;
  shelfAlias = '';

  @ViewChild('warehouseLists') warehouseLists!: ElementRef<HTMLInputElement>;

  constructor(
    private apiService: ApiService,
    private tostr: ToastrService,
    private dp: DatePipe,
    private com: CommonService
  ) {}

  ngOnInit(): void {
    this.getWarehouseList();
    this.getWarehouse();
  }

  getWarehouseList() {
    this.apiService.getData('inventory/GetWarehouseList').subscribe((data) => {
      this.warehouseList = data;
    });
  }

  onClearWarehouse() {
    this.wId = 0;
    this.wName = '';
    this.rId = 0;
    this.rName = '';
    this.sName = '';
    this.rack = [];
    this.shelf = [];
    this.selectedRack = undefined;
    this.selectedShelf = undefined;
    this.searchGrid();
  }

  onClearRack() {
    this.rId = 0;
    this.rName = '';
    this.sName = '';
    this.shelf = [];
    this.selectedShelf = undefined;
    this.searchGrid();
  }

  onClearShelf() {
    this.sName = '';
    this.searchGrid();
  }

  //======================= WAREHOUSE ======================//

  getWarehouse() {
    this.apiService.getData('inventory/Warehouse').subscribe((data) => {
      this.warehouse = data;
    });
  }

  newWarehouse() {
    this.refreshWarehouse();
    this.isDisabledWarehouse = false;
    this.isShowWarehouse = true;
  }

  refreshWarehouse() {
    this.warahouseName = '';
    this.warehouseId = 0;
    this.warehouseAlias = '';
    this.isDisabledWarehouse = true;
    this.isShowWarehouse = false;
  }

  createUpdateWareHouse() {
    const obj = {
      id: this.warehouseId,
      name: this.warahouseName,
      alias: this.warehouseAlias,
      dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
    };

    if (this.warahouseName == '' || this.warahouseName == null) {
      this.tostr.warning('Enter Warehouse....!');
      return;
    }

    if (this.warehouseAlias == '' || this.warehouseAlias == null) {
      this.tostr.warning('Enter Alias....!');
      return;
    }

    try {
      this.com.showLoader();

      this.apiService
        .saveObj('inventory/AddUpdateWarehouse', obj)
        .subscribe((result) => {
          if (result == true || result == 'true') {
            this.getWarehouse();
            this.getWarehouseList();
            this.tostr.success('Save Successfully');
            this.refreshWarehouse();
            setTimeout(() => {
              this.searchGrid();
            }, 10);
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

  editWarehouse(id: any, name: any, alias: any): void {
    this.warahouseName = name;
    this.warehouseId = id;
    this.warehouseAlias = alias;
    this.isDisabledWarehouse = false;
    this.isShowWarehouse = true;
  }

  deleteWarehouse(id: any): void {
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

      this.apiService.deleteData('inventory/DeleteWarehouse', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.getWarehouse();
            this.getWarehouseList();
            this.tostr.success('Delete Successfully');
            this.refreshWarehouse();
            setTimeout(() => {
              this.searchGrid();
            }, 10);
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

  //====================== RACK =======================//

  onWarehouseChange(event: any) {
    this.wId = event.id;
    this.wName = event.name;

    var obj = {
      wId: event.id,
    };

    this.apiService.getDataById('inventory/Rack', obj).subscribe((data) => {
      this.rack = data;

      setTimeout(() => {
        this.searchGrid();
      }, 10);
    });

    this.shelf = [];
    this.selectedRack = undefined;
    this.selectedShelf = undefined;
  }

  newRack() {
    this.refreshRack();
    this.isDisabledRack = false;
    this.isShowRack = true;
  }

  refreshRack() {
    this.rackName = '';
    this.rackId = 0;
    this.rackAlias = '';
    this.isDisabledRack = true;
    this.isShowRack = false;
  }

  createUpdateRack() {
    if (this.wId == 0) {
      this.tostr.warning('Select Warehouse ....!');
      return;
    }

    if (this.rackName == '' || this.rackName == null) {
      this.tostr.warning('Enter Rack....!');
      return;
    }

    if (this.rackAlias == '' || this.rackAlias == null) {
      this.tostr.warning('Enter Alias....!');
      return;
    }

    try {
      this.com.showLoader();
      const obj = {
        wId: this.wId,
        id: this.rackId,
        name: this.rackName,
        alias: this.rackAlias,
        dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      };

      this.apiService
        .saveObj('inventory/AddUpdateRack', obj)
        .subscribe((result) => {
          if (result == true || result == 'true') {
            this.onWarehouseChange({ id: this.wId, name: this.wName });
            this.getWarehouseList();
            this.tostr.success('Save Successfully');
            this.refreshRack();
            setTimeout(() => {
              this.searchGrid();
            }, 10);
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

  editRack(id: any, name: any, alias: any): void {
    this.rackName = name;
    this.rackId = id;
    this.rackAlias = alias;
    this.isDisabledRack = false;
    this.isShowRack = true;
  }

  deleteRack(id: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (!confirmDelete) {
      return;
    }
    try {
      this.com.showLoader();
      const obj = {
        wId: this.wId,
        id: id,
        dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      };

      this.apiService.deleteData('inventory/DeleteRack', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.onWarehouseChange({ id: this.wId, name: this.wName });
            this.getWarehouseList();
            this.tostr.success('Delete Successfully');
            this.refreshRack();
            setTimeout(() => {
              this.searchGrid();
            }, 10);
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

  //========================= SHELF ======================//

  onRackChange(event: any) {
    this.rId = event.id;
    this.rName = event.name;

    var obj = {
      wId: this.wId,
      rId: event.id,
    };

    this.apiService.getDataById('inventory/Shelf', obj).subscribe((data) => {
      this.shelf = data;
      setTimeout(() => {
        this.searchGrid();
      }, 10);
    });

    this.selectedShelf = undefined;
  }

  newShelf() {
    this.refreshShelf();
    this.isDisabledShelf = false;
    this.isShowShelf = true;
  }

  refreshShelf() {
    this.shelfName = '';
    this.shelfId = 0;
    this.shelfAlias = '';
    this.isDisabledShelf = true;
    this.isShowShelf = false;
  }

  createUpdateShelf() {
    if (this.wId == 0) {
      this.tostr.warning('Select Warehouse ....!');
      return;
    }

    if (this.rId == 0) {
      this.tostr.warning('Select Rack ....!');
      return;
    }

    if (this.shelfName == '' || this.shelfName == null) {
      this.tostr.warning('Enter Shelf....!');
      return;
    }

    if (this.shelfAlias == '' || this.shelfAlias == null) {
      this.tostr.warning('Enter Alias....!');
      return;
    }

    try {
      this.com.showLoader();
      const obj = {
        wId: this.wId,
        rId: this.rId,
        id: this.shelfId,
        name: this.shelfName,
        alias: this.shelfAlias,
        dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      };

      this.apiService
        .saveObj('inventory/AddUpdateShelf', obj)
        .subscribe((result) => {
          if (result == true || result == 'true') {
            this.onRackChange({ id: this.rId, name: this.rName });
            this.getWarehouseList();
            this.tostr.success('Save Successfully');
            this.refreshShelf();
            this.com.hideLoader();
            setTimeout(() => {
              this.searchGrid();
            }, 10);
          } else {
            this.tostr.error('Please Save Again');
            this.com.hideLoader();
          }
        });
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  editShelf(id: any, name: any, alias: any): void {
    this.shelfName = name;
    this.shelfId = id;
    this.shelfAlias = alias;
    this.isDisabledShelf = false;
    this.isShowShelf = true;
  }

  deleteShelf(id: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (!confirmDelete) {
      return;
    }
    try {
      this.com.showLoader();
      const obj = {
        wId: this.wId,
        rId: this.rId,
        id: id,
        dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      };

      this.apiService.deleteData('inventory/DeleteShelf', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.onRackChange({ id: this.rId, name: this.rName });
            this.getWarehouseList();
            this.tostr.success('Delete Successfully');
            this.refreshShelf();
            this.com.hideLoader();
            setTimeout(() => {
              this.searchGrid();
            }, 10);
          } else if (data == 'false' || data == false) {
            this.tostr.error('Delete Again');
            this.com.hideLoader();
          }
        },
        error: (error) => {
          this.tostr.info(error.error.text);
          this.com.hideLoader();
        },
      });
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  editRow(item: any) {
    this.onWarehouseChange({ id: item.WID });
    this.onRackChange({ id: item.RID });
    this.selectedWarehouse = item.WID;
    this.selectedRack = item.RID;
    this.selectedShelf = item.SID;

    this.editShelf(item.SID, item.SNAME, item.SALIAS);
  }

  onChangeShelf(event: any) {
    this.sName = event.name;
    this.searchGrid();
  }

  searchGrid(): void {
    const tableElement = this.warehouseLists.nativeElement;
    const rows = tableElement.querySelectorAll('tr');

    rows.forEach((row: HTMLTableRowElement) => {
      let isShow = true;

      if (
        row.querySelector('.Wname')?.textContent != this.wName &&
        this.wName.length > 0
      ) {
        isShow = false;
      }

      if (
        row.querySelector('.Rname')?.textContent != this.rName &&
        this.rName.length > 0
      ) {
        isShow = false;
      }

      if (
        row.querySelector('.Sname')?.textContent != this.sName &&
        this.sName.length > 0
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
