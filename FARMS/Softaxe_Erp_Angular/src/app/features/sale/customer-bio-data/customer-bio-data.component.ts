import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { DatePipe } from '@angular/common';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-customer-bio-data',
  templateUrl: './customer-bio-data.component.html',
  styleUrls: ['./customer-bio-data.component.css'],
})
export class CustomerBioDataComponent {
  isDisabled: boolean = true;
  isShowPage: boolean = true;
  customerForm!: FormGroup;

  // CUSTOMER
  customerList: any[] = [];
  level4: any[] = [];
  L4Code: any = null;
  isShow = false;

  // SEARCH
  customerSearch = '';
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

  @ViewChild('customerLists', { static: false }) customerLists!: ElementRef;

  constructor(
    private fb: FormBuilder,
    private tostr: ToastrService,
    private apiService: ApiService,
    private dp: DatePipe,
    private com: CommonService
  ) {}
  ngOnInit() {
    this.getCustomer();
    this.getMainArea();
    this.formInit();
  }

  formInit() {
    this.customerForm = this.fb.group({
      code: [''],
      name: [''],
      address: [''],
      city: [''],
      email: [''],
      postalCode: [''],
      cnic: [''],
      ntn: [''],
      contact: [''],
      phone: [''],
      commission: [0],
      saleTax: ['nonfiler'],
      whTax: ['nonfiler'],
      mainAreaId: [null],
      subAreaId: [null],
      creditLimit: [0],
      inActive: false,
      tag: ['customer'],
      dtNow: [new Date()],
      ratediff:[0],
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

  async getCustomer() {
    const data = await this.apiService.getData('Sale/GetCustomer').toPromise();
    this.customerList = data.party;
    this.level4 = data.l4;
    setTimeout(() => {
      this.searchGrid();
    }, 50);
  }

  onClickNew() {
    this.onClickRefresh();
    this.isShow = true;
    this.isDisabled = false;
  }

  onClickRefresh() {
    this.customerForm.reset();
    this.customerForm.get('saleTax')?.setValue('nonfiler');
    this.customerForm.get('whTax')?.setValue('nonfiler');
    this.subAreaList = [];
    this.isShow = false;
    this.isDisabled = true;
  }

  onClickSave() {
    let body = this.customerForm.value;
    body.commission = body.commission == null ? 0 : body.commission;
    body.creditLimit = body.creditLimit == null ? 0 : body.creditLimit;
    body.ratediff = body.ratediff == null ? 0 : body.ratediff;
    
    body.dtNow = new Date();
    body.tag = 'customer';
    body.L4Code = this.L4Code;
    body.inActive = body.inActive == null ? false : body.inActive;

    if (body.L4Code == null) {
      this.tostr.warning('Select Main Account...!');
      return;
    }

    if (body.name == null) {
      this.tostr.warning('Enter Customer Name....!');
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

    if (body.cnic == null) {
      this.tostr.warning('Enter CNIC....!');
      return;
    }

    if (body.ntn == null) {
      this.tostr.warning('Enter NTN....!');
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

    if (body.saleTax == null) {
      this.tostr.warning('Select Sale Tax....!');
      return;
    }

    if (body.whTax == null) {
      this.tostr.warning('Select W.H.Tax....!');
      return;
    }

    try {
      this.com.showLoader();

      this.apiService.saveData('Sale/AddUpdateCustomer', body).subscribe(
        (result) => {
          if (result == true || result == 'true') {
            this.tostr.success('Save Successfully');
            this.onClickRefresh();
            this.getCustomer();
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

  async editCustomer(data: any) {
    this.L4Code = data.code.substring(0, 9)
    this.togglePages();
    this.onClickNew();
    this.customerForm.get('code')?.setValue(data.code);
    this.customerForm.get('name')?.setValue(data.name);
    this.customerForm.get('mainAreaId')?.setValue(data.mainAreaId);
    await this.onChangeMainAera({ id: data.mainAreaId });
    this.customerForm.get('subAreaId')?.setValue(data.subAreaId);
    this.customerForm.get('address')?.setValue(data.address);
    this.customerForm.get('city')?.setValue(data.city);
    this.customerForm.get('cnic')?.setValue(data.Nic);
    this.customerForm.get('ntn')?.setValue(data.Ntn);
    this.customerForm.get('contact')?.setValue(data.contact);
    this.customerForm.get('phone')?.setValue(data.phone);
    this.customerForm.get('email')?.setValue(data.email);
    this.customerForm.get('postalCode')?.setValue(data.postalCode);
    this.customerForm.get('saleTax')?.setValue(data.saleTax);
    this.customerForm.get('whTax')?.setValue(data.whTax);
    this.customerForm.get('commission')?.setValue(data.commission);
    this.customerForm.get('creditLimit')?.setValue(data.creditLimit);
    this.customerForm.get('ratediff')?.setValue(data.ratediff);
    
    this.customerForm.get('inActive')?.setValue(data.inActive);
    this.customerForm.get('tag')?.setValue('customer');
  }

  deleteCustomer(code: any) {
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

      this.apiService.deleteData('Sale/DeleteCustomer', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.getCustomer();
            this.com.hideLoader();
            this.tostr.success(' Delete Successfully');
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

  onSearchCustomer(event: any) {
    this.customerSearch = event.target.value;
    this.searchGrid();
  }

  searchGrid() {
    const tableElement = this.customerLists.nativeElement;
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
            .indexOf(this.customerSearch.toLowerCase()) > -1
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

  onClearMainArea() {
    this.subAreaList = [];
    this.customerForm.get('subAreaId')?.patchValue(null);
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
            this.com.hideLoader();
            this.tostr.success('Save Successfully');
            this.refreshMainArea();
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

  //======================= SUB AREA =======================//

  async onChangeMainAera(event: any) {
    this.mainAreaId = event.id;
    var obj = {
      mainAreaId: event.id,
    };

    const data = await this.apiService
      .getDataById('Sale/GetSubArea', obj)
      .toPromise();
    this.customerForm.get('subAreaId')?.patchValue(null);
    this.subAreaList = data;
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

  export(fileName: any, formate: any) {
    this.com.ExportFiles(fileName, formate);
  }
}
