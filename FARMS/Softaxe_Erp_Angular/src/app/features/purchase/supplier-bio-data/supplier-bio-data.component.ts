import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { DatePipe } from '@angular/common';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-supplier-bio-data',
  templateUrl: './supplier-bio-data.component.html',
  styleUrls: ['./supplier-bio-data.component.css'],
})
export class SupplierBioDataComponent {
  isDisabled: boolean = true;
  isShowPage: boolean = true;
  supplierForm!: FormGroup;

  // SUPPLIER
  supplierList: any[] = [];
  level4: any[] = [];
  L4Code: any = null;
  isShow = false;

  // SEARCH
  supplierSearch = '';
  all: boolean = true;
  active: boolean = false;
  inActive: boolean = false;

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

  @ViewChild('supplierLists', { static: false }) supplierLists!: ElementRef;

  constructor(
    private fb: FormBuilder,
    private tostr: ToastrService,
    private apiService: ApiService,
    private dp: DatePipe,
    private com: CommonService
  ) {}
  ngOnInit() {
    this.getSupplier();
    this.getMainArea();
    this.formInit();
  }

  formInit() {
    this.supplierForm = this.fb.group({
      code: [''],
      name: [''],
      address: [''],
      city: [''],
      contact: [''],
      phone: [''],
      email: [''],
      postalCode: [''],
      mainAreaId: [null],
      subAreaId: [null],
      commission: [0],
      creditLimit: [0],
      inActive: false,
      tag: ['supplier'],
      dtNow: [new Date()],
      ntn: [''],
      strn: [''],
      AccNo:[''],
    });
  }

  togglePages() {

    if(this.L4Code == null){
      this.tostr.warning("Select Main Account...")
      return;
    }
    this.isShowPage = !this.isShowPage;
    if (!this.isShowPage) {
      this.onClickRefresh();
    }
    else{
      setTimeout(() => {
        this.searchGrid();
      }, 0);
    }
  }

  getSupplier() {
    this.apiService.getData('Purchase/GetSupplier').subscribe((data) => {
      this.supplierList = data.party;
      this.level4 = data.l4;
      setTimeout(() => {
        this.searchGrid();
      }, 50);
    });
  }

  onClickNew() {
    this.onClickRefresh();
    this.isShow = true;
    this.isDisabled = false;
  }

  onClickRefresh() {
    this.supplierForm.reset();
    this.isShow = false;
    this.isDisabled = true;
  }

  onClickSave() {
    let body = this.supplierForm.value;
    body.commission = body.commission == null ? 0 : body.commission;
    body.creditLimit = body.creditLimit == null ? 0 : body.creditLimit;
    body.dtNow = new Date();
    body.tag = 'supplier';
    body.L4Code = this.L4Code;
    body.inActive = body.inActive == null ? false : body.inActive;

    if (body.L4Code == null) {
      this.tostr.warning('Select Main Account...!');
      return;
    }

    if (body.name == null) {
      this.tostr.warning('Enter Supplier Name....!');
      return;
    }

    if (body.mainAreaId == null) {
      this.tostr.warning('Select Main Area....!');
      return;
    }

    if (body.subAreaId == null) {
      this.tostr.warning('Select Sub Area....!');
      return;
    }

    if (body.address == null) {
      this.tostr.warning('Enter Address....!');
      return;
    }

    if (body.city == null) {
      this.tostr.warning('Enter City....!');
      return;
    }

    if (body.postalCode == null) {
      this.tostr.warning('Enter Postal Code....!');
      return;
    }

    if (body.contact == null) {
      this.tostr.warning('Enter Contact....!');
      return;
    }

    if (body.phone == null) {
      this.tostr.warning('Enter Phone....!');
      return;
    }

    if (body.email == null) {
      this.tostr.warning('Enter Email....!');
      return;
    }

    try {
      this.com.showLoader();

      this.apiService.saveData('Purchase/AddUpdateSupplier', body).subscribe(
        (result) => {
          if (result == true || result == 'true') {
            this.tostr.success('Save Successfully');
            this.onClickRefresh();
            this.getSupplier();
            this.com.hideLoader();
          } else {
            this.com.hideLoader();
            this.tostr.error('Please Save Again');
          }
        },
        (error) => {
          this.com.hideLoader();
          this.tostr.error('On Err');
        }
      );
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  editSupplier(data: any) {
    this.L4Code = data.code.substring(0, 9)
    this.togglePages();
    this.onClickNew();

    this.supplierForm.get('code')?.setValue(data.code);
    this.supplierForm.get('name')?.setValue(data.name);
    this.supplierForm.get('address')?.setValue(data.address);
    this.supplierForm.get('city')?.setValue(data.city);
    this.supplierForm.get('contact')?.setValue(data.contact);
    this.supplierForm.get('phone')?.setValue(data.phone);
    this.supplierForm.get('email')?.setValue(data.email);
    this.supplierForm.get('postalCode')?.setValue(data.postalCode);
    this.supplierForm.get('mainAreaId')?.setValue(data.mainAreaId);
    this.onChangeMainAera({ id: data.mainAreaId });
    this.supplierForm.get('subAreaId')?.setValue(data.subAreaId);
    this.supplierForm.get('commission')?.setValue(data.commission);
    this.supplierForm.get('creditLimit')?.setValue(data.creditLimit);
    this.supplierForm.get('inActive')?.setValue(data.inActive);

    this.supplierForm.get('ntn')?.setValue(data.Ntn);
    this.supplierForm.get('strn')?.setValue(data.Strn);
    this.supplierForm.get('AccNo')?.setValue(data.AccNo);
    

    this.supplierForm.get('tag')?.setValue('supplier');
  }

  deleteSupplier(code: any) {
    const confirmDelete = confirm('Are you sure you want to delete this item?');
    if (!confirmDelete) {
      return;
    }

    try {
      this.com.showLoader();
      const obj = {
        code: code,
        dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      };

      this.apiService.deleteData('Purchase/DeleteSupplier', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.getSupplier();
            this.tostr.success(' Delete Successfully');
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

  //====================== FILTER ====================//

  onClickFilter(event: any) {
    this.all = false;
    this.active = false;
    this.inActive = false;

    if (event.target.value == 'all') {
      this.all = true;
    } else if (event.target.value == 'active') {
      this.active = true;
    } else if (event.target.value == 'inActive') {
      this.inActive = true;
    }
    this.searchGrid();
  }

  onSearchSupplier(event: any) {
    this.supplierSearch = event.target.value;
    this.searchGrid();
  }

  searchGrid() {
    const tableElement = this.supplierLists.nativeElement;
    const rows = tableElement.querySelectorAll('tr');

    rows.forEach((row: HTMLTableRowElement) => {
      let isShow = true;

      if(this.L4Code != null){
        if (
          row.querySelector('.code')?.textContent.substring(0, 9) != this.L4Code &&
          this.L4Code.length > 0
          ) {isShow = false;}
      }

      if (this.active) {
        if (row.querySelector('.active')?.textContent == 'true') {
          isShow = true;
        } else {
          isShow = false;
        }
      }

      if (this.inActive) {
        if (row.querySelector('.active')?.textContent == 'false') {
          isShow = true;
        } else {
          isShow = false;
        }
      }

      if (isShow) {
        if (
          row.textContent &&
          row.textContent
            .toLowerCase()
            .indexOf(this.supplierSearch.toLowerCase()) > -1
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

  //======================= MAIN AREA =======================//

  getMainArea() {
    this.apiService.getData('Purchase/GetMainArea').subscribe((data) => {
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
        .saveObj('Purchase/AddUpdateMainArea', obj)
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

    if (!confirmDelete) {
      return;
    }
    try {
      this.com.showLoader();
      const obj = {
        id: id,
        dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      };

      this.apiService.deleteData('Purchase/DeleteMainArea', obj).subscribe({
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

  //======================= SUB AREA =======================//

  onChangeMainAera(event: any) {
    this.supplierForm.get('subAreaId')?.patchValue(null);

    if (event == undefined) {
      this.subAreaList = [];
    }

    this.mainAreaId = event.id;
    var obj = {
      mainAreaId: event.id,
    };

    this.apiService
      .getDataById('Purchase/GetSubArea', obj)
      .subscribe((data) => {
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
        .saveObj('Purchase/AddUpdateSubArea', obj)
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

    if (!confirmDelete) {
      return;
    }
    try {
      this.com.showLoader();
      const obj = {
        mainAreaId: this.mainAreaId,
        id: id,
        dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      };

      this.apiService.deleteData('Purchase/DeleteSubArea', obj).subscribe({
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

  export(fileName: any, formate: any) {
    this.com.ExportFiles(fileName, formate);
  }
}
