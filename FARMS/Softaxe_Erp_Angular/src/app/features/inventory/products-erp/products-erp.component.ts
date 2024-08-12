import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ApiService } from '../../../services/api.service';
import { ToastrService } from 'ngx-toastr';
import { DatePipe } from '@angular/common';
import { environment } from '../../../../environment/environmemt';
import { CommonService } from '../../../services/common.service';

@Component({
  selector: 'app-products-erp',
  templateUrl: './products-erp.component.html',
  styleUrls: ['./products-erp.component.css']
})
export class ProductsErpComponent {


  isDisabled: boolean = true;
  isShowPage: boolean = true;

  homeCategory: any[] = [];
  homeBrand: any[] = [];
  productsList: any[] = [];
  level4: any[] = [];
  ngCategory: any;
  ngBrand: any;

  basePath = environment.basePath;
  query = '?v=' + Math.random();

  // FILTER
  searchBrand = '';
  searchMain = '';
  productSearch = '';
  all: boolean = true;
  active: boolean = false;
  inActive: boolean = false;
  L4Code: any = null;

  // PRODUCT
  location: any[] = [];
  status = [
    { id: true, name: 'Active' },
    { id: false, name: 'InActive' },
  ];

  productForm!: FormGroup;
  isShow = false;
  productImage: File | null = null;
  selectedImage: any = '';
  file: any;

  // CATEGORY
  category: any[] = [];
  expenseList: any[] = [];
  categoryName = '';
  categoryId: number = 0;
  expiryDays: any = 0;
  expCode: any = null;
  isCommission: any = false;
  isDisabledCategory: boolean = true;
  isShowCategory: boolean = false;
  catrgoryImage: File | null = null;
  selectedCategoryImage: any = '';
  categoryFile: any;

  // BRAND
  brand: any[] = [];
  catId: number = 0;
  brandName = '';
  brandId: number = 0;
  isDisabledBrand: boolean = true;
  isShowBrand: boolean = false;

  // UOM
  uom: any[] = [];
  uomName = '';
  uomId: number = 0;
  isDisabledUom: boolean = true;
  isShowUom: boolean = false;

  // MADE IN
  madeIn: any[] = [];
  madeInName = '';
  madeInId: number = 0;
  isDisabledMadeIn: boolean = true;
  isShowMadeIn: boolean = false;

  @ViewChild('prodList', { static: false }) prodList!: ElementRef;

  constructor(
    private apiService: ApiService,
    private fb: FormBuilder,
    private tostr: ToastrService,
    private dp: DatePipe,
    private com: CommonService
  ) {}

  ngOnInit(): void {
    this.getCategory();
    this.getExpense();
    this.getMainAccount();
    this.getUomList();
    this.getMadeInList();
    this.getLocation();
    this.formInit();
  }

  formInit() {
    this.productForm = this.fb.group({
      saleCode: [''],
      stockCode: [''],
      code: [''],
      category: [0],
      brand: [0],
      name: [''],
      uomId: [0],
      packing: [0],
      shortName: [''],
      saleRate: [0],
      standardWeight: [0],
      itemWeight: [0],
      itemPackedWeight: [0],
      liter: [0],
      discount: [0],
      saleTax: [0],
      madeIn: [0],
      location: [0],
      minimumLevel: [0],
      hsNo: [''],
      status: [false],
      barCode: [''],
      noStock: [false],
      oldRate: [0],
      rate2: [0],
      rate3: [0],
      rate4: [0],
      rate5: [0],
      rate6: [0],
      rate7: [0],
      purchaseRate1: [0],
      purchaseRate2: [0],
      dtNow: [new Date()],
    });
  }

  getProductList() {
    if (this.ngCategory == null) {
      return;
    }

    var obj = {
      categoryId: this.ngCategory,
    };

    this.apiService
      .getDataById('Inventory/GetProducts', obj)
      .subscribe((data) => {
        this.productsList = data;
        this.getHomeBrand(this.ngCategory);

        setTimeout(() => {
          this.searchGrid();
        }, 100);
      });
  }

  getExpense() {
    this.apiService.getData('Inventory/GetExpenseList').subscribe((data) => {
      this.expenseList = data;
    });
  }

  onClearHCategory() {
    this.searchBrand = '';
    this.homeBrand = [];
    this.productsList = [];
    this.ngBrand = null;
    this.searchGrid();
  }

  onClearHBrand() {
    this.searchBrand = '';
    this.searchGrid();
  }

  getHomeBrand(id: any) {
    var obj = {
      categoryId: id,
    };

    this.apiService.getDataById('Inventory/GetBrand', obj).subscribe((data) => {
      this.homeBrand = data;
    });
  }

  onChangeBrand(event: any) {
    this.searchBrand = event.name;
    this.searchGrid();
  }

  onInput() {
    this.searchGrid();
  }

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

  searchGrid() {
    const tableElement = this.prodList.nativeElement;
    const rows = tableElement.querySelectorAll('tr');

    rows.forEach((row: HTMLTableRowElement) => {
      let isShow = true;

      if (
        row.querySelector('.brand')?.textContent != this.searchBrand &&
        this.searchBrand.length > 0
      ) {
        isShow = false;
      }

      if (this.L4Code != null) {
        if (
          row.querySelector('.mainCode')?.textContent != this.L4Code &&
          this.L4Code.length > 0
        ) {
          isShow = false;
        }
      }

      if (isShow) {
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
      }

      if (isShow) {
        if (
          row.textContent &&
          row.textContent
            .toLowerCase()
            .indexOf(this.productSearch.toLowerCase()) > -1
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

  togglePages() {
    if (this.L4Code == null) {
      this.tostr.warning('Select Main Account...');
      return;
    }

    this.isShowPage = !this.isShowPage;
    if (!this.isShowPage) {
      this.onClickRefresh();
    }
  }

  getLocation() {
    this.apiService.getData('Inventory/GetLocation').subscribe((data) => {
      if(data.length > 0){
        this.location = data;
      }
    });
  }

  getMainAccount() {
    this.apiService.getData('Inventory/GetMainAccount').subscribe((data) => {
      this.level4 = data;
    });
  }

  //====================== PRODUCT ======================//

  onClickNew() {
    this.onClickRefresh();

    if(this.location.length > 0){
      this.productForm.get('location')?.setValue(this.location[0].id);
    }
    
    if(this.madeIn.length > 0){
      this.productForm.get('madeIn')?.setValue(this.madeIn[0].id);
    }

    this.productForm.get('status')?.setValue(true);

    if(this.level4.length > 0){
      let code = this.level4.find((x) => x.code == this.L4Code).stockCode
      if(!code)
      {
        this.productForm.get('noStock')?.patchValue(true);
        this.productForm.get('noStock')?.disable();
      }
    }

    this.isShow = true;
    this.isDisabled = false;
  }

  onClickRefresh() {
    this.productForm.reset();
    this.productForm.get('noStock')?.enable();
    this.isShow = false;
    this.isDisabled = true;
    this.selectedImage = '';
    this.productImage = null;
    this.file = '';
  }

  genrateBarCode() {
    let body = this.productForm.value;

    var obj = {
      saleCode: this.L4Code,
      code: body.code,
    };

    this.apiService
      .getDataById('Inventory/GenBarCode', obj)
      .subscribe((data) => {
        this.productForm.get('barCode')?.setValue(data);
      });
  }

  onInputPacking() {
    const packing = this.productForm.get('packing')?.value ?? '';
    this.productForm.get('description')?.setValue(packing + ' Pcs');
  }

  onClickSave() {
    let body = this.productForm.value;

    if (body.category == null) {
      this.tostr.warning('Select Category....!');
      return;
    }

    if (body.brand == null) {
      this.tostr.warning('Select Brand....!');
      return;
    }

    if (body.name == '' || body.name == null) {
      this.tostr.warning('Enter Product Name....!');
      return;
    }

    if (body.uomId == null) {
      this.tostr.warning('Select Unit of Measurment....!');
      return;
    }

    if (body.packing == null || body.packing == 0) {
      this.tostr.warning('Enter Packing....!');
      return;
    }

    if (body.shortName == '' || body.shortName == null) {
        body.shortName = '';
      // this.tostr.warning('Enter Short Name....!');
      // return;
    }

    if (body.saleRate == null || body.saleRate == 0) {
        body.saleRate = 0;
      // this.tostr.warning('Enter Sale Rate....!');
      // return;
    }

    if (body.standardWeight == null || body.standardWeight == 0) {
        body.standardWeight = 0;
      // this.tostr.warning('Enter Standard Weight....!');
      // return;
    }

    if (body.itemWeight == null || body.itemWeight == 0) {
        body.itemWeight = 0
      // this.tostr.warning('Enter Item Weight....!');
      // return;
    }

    if (body.itemPackedWeight == null || body.itemPackedWeight == 0) {
        body.itemPackedWeight = 0;
      // this.tostr.warning('Enter Item Packed Weight....!');
      // return;
    }

    if (body.liter == null || body.liter == 0) {
        body.liter = 0;
      // this.tostr.warning('Enter Liter....!');
      // return;
    }

    if (body.madeIn == null) {
      this.tostr.warning('Select Made In....!');
      return;
    }

    if (body.location == null) {
      this.tostr.warning('Select Location....!');
      return;
    }

    if (body.minimumLevel == null || body.minimumLevel == 0) {
        body.minimumLevel = 0;
      // this.tostr.warning('Enter Minimum Level....!');
      // return;
    }

    if (body.status == null) {
      this.tostr.warning('Select Status....!');
      return;
    }

    

    body.rate2 = body.rate2 ?? 0;
    body.rate3 = body.rate3 ?? 0;
    body.rate4 = body.rate4 ?? 0;
    body.rate5 = body.rate5 ?? 0;
    body.rate6 = body.rate6 ?? 0;
    body.rate7 = body.rate7 ?? 0;
    body.oldRate = body.oldRate ?? 0;
    body.purchaseRate1 = body.purchaseRate1 ?? 0;
    body.purchaseRate2 = body.purchaseRate2 ?? 0;
    body.discount = body.discount ?? 0;
    body.saleTax = body.saleTax ?? 0;
    body.barCode = body.barCode ?? '';
    body.code = body.code ?? '';
    body.stockCode = this.level4.find((x) => x.code == this.L4Code).stockCode;
    body.saleCode = this.L4Code;
    body.noStock = this.productForm.get('noStock').value ?? false;
    body.hsNo = body.hsNo ?? '';
    body.dtNow = this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss');

    try {

      this.com.showLoader();
      let formData = new FormData();
      for (const key of Object.keys(body)) {
        formData.append(key, body[key]);
      }
      formData.append('Picture', this.productImage!);

      this.apiService
        .saveData('Inventory/AddUpdateProduct', formData)
        .subscribe(
          (result) => {
            if (result == true || result == 'true') {
              this.tostr.success('Save Successfully');
              this.onClickRefresh();
              this.getProductList();
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

  deleteProduct(i: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (!confirmDelete) {
      return;
    }
    try {
      this.com.showLoader();

      const obj = {
        stockCode: i.StockCode,
        saleCode: i.SaleCode,
        productName: i.ProductName,
        dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      };

      this.apiService.deleteData('Inventory/DeleteProduct', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.getProductList();
            this.com.hideLoader();
            this.tostr.success('Delete Successfully');
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

  async editProduct(i: any) {
    try {
      this.com.showLoader();

      this.L4Code = i.SaleCode.substring(0, 9);

      let obj = {
        code: i.SaleCode,
      };

      this.togglePages();
      this.onClickNew();

      const data = await this.apiService
        .getDataById('Inventory/EditProduct', obj)
        .toPromise();
      let r = data[0]

      this.productForm.get('saleCode')?.setValue(r.SaleCode);
      this.productForm.get('stockCode')?.setValue(r.StockCode);
      this.productForm.get('code')?.setValue(r.Code);
      this.productForm.get('category')?.setValue(r.CategoryId);
      this.onChangeCatrgory({ id: r.CategoryId });
      this.productForm.get('brand')?.setValue(r.BrandId);
      this.productForm.get('name')?.setValue(r.ProductName);
      this.productForm.get('uomId')?.setValue(r.UomId);
      this.productForm.get('packing')?.setValue(r.Packing);
      this.productForm.get('shortName')?.setValue(r.ShortName);
      this.productForm.get('saleRate')?.setValue(r.SaleRate);
      this.productForm.get('standardWeight')?.setValue(r.StandardWeight);
      this.productForm.get('itemWeight')?.setValue(r.ItemWeight);
      this.productForm.get('itemPackedWeight')?.setValue(r.ItemPackedWeight);
      this.productForm.get('liter')?.setValue(r.Liter);
      this.productForm.get('discount')?.setValue(r.Discount);
      this.productForm.get('saleTax')?.setValue(r.SaleTax);
      this.productForm.get('madeIn')?.setValue(r.CountryId);
      this.productForm.get('location')?.setValue(r.LocationId);
      this.productForm.get('minimumLevel')?.setValue(r.MinLevel);
      this.productForm.get('hsNo')?.setValue(r.HSCode);
      this.productForm.get('status')?.setValue(r.InActive);
      this.productForm.get('barCode')?.setValue(r.BarCode);
      this.productForm.get('noStock')?.setValue(r.NoStock);
      this.productForm.get('oldRate')?.setValue(r.OldRate);
      this.productForm.get('rate2')?.setValue(r.Rate2);
      this.productForm.get('rate3')?.setValue(r.Rate3);
      this.productForm.get('rate4')?.setValue(r.Rate4);
      this.productForm.get('rate5')?.setValue(r.Rate5);
      this.productForm.get('rate6')?.setValue(r.Rate6);
      this.productForm.get('rate7')?.setValue(r.Rate7);
      this.productForm.get('purchaseRate1')?.setValue(r.PurchaseRate1);
      this.productForm.get('purchaseRate2')?.setValue(r.PurchaseRate2);
      this.selectedImage = this.basePath + data[0].Image;

      this.com.hideLoader();
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  //#region =================== CATEGORY ====================//

  getCategory() {
    this.apiService.getData('Inventory/GetCategory').subscribe((result) => {
      this.homeCategory = [{ id: 0, name: 'All' }];
      this.homeCategory.push(...result);
      this.category = result;
    });
  }

  onClearCategory() {
    this.brand = [];
    this.productForm.get('brand')?.patchValue(null);
  }

  newCategory() {
    this.refreshCategory();
    this.isDisabledCategory = false;
    this.isShowCategory = true;
  }

  refreshCategory() {
    this.categoryName = '';
    this.categoryId = 0;
    this.expiryDays = 0;
    this.expCode = null;
    this.isDisabledCategory = true;
    this.isShowCategory = false;
    this.isCommission = false;
    this.selectedCategoryImage = '';
    this.catrgoryImage = null;
    this.categoryFile = '';
  }

  createUpdateCategory() {
    if (this.categoryName == '') {
      this.tostr.warning('Enter Category....!');
      return;
    }
    try {
      this.com.showLoader();

      let formData = new FormData();
      formData.append('id', this.categoryId.toString());
      formData.append('name', this.categoryName);
      formData.append('expiryDays', this.expiryDays ?? 0);
      formData.append('expCode', this.expCode);
      formData.append('isCommission', this.isCommission);
      formData.append('picture', this.catrgoryImage!);
      formData.append(
        'dtNow',
        this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss')!
      );

      this.apiService
        .saveData('Inventory/AddUpdateCategory', formData)
        .subscribe((result) => {
          if (result == true || result == 'true') {
            this.getCategory();
            this.tostr.success('Save Successfully');
            this.refreshCategory();
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

  editCategory(i: any): void {
    this.categoryName = i.name;
    this.categoryId = i.id;
    this.expiryDays = i.expiryDays;
    this.isCommission = i.isCommission;
    this.isDisabledCategory = false;
    this.isShowCategory = true;
    this.expCode = i.CODE ?? null;
    this.selectedCategoryImage = this.basePath + i.Image;
  }

  deleteCategory(id: any): void {
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

      this.apiService.deleteData('Inventory/DeleteCategory', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.getCategory();
            this.tostr.success('Delete Successfully');
            this.refreshCategory();
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

  //#endregion

  //#region =================== BRAND ====================//

  onChangeCatrgory(event: any) {
    this.catId = event.id;

    var obj = {
      categoryId: event.id,
    };

    this.apiService.getDataById('Inventory/GetBrand', obj).subscribe((data) => {
      this.brand = data;
    });
  }

  newBrand() {
    this.refreshBrand();
    this.isDisabledBrand = false;
    this.isShowBrand = true;
  }

  refreshBrand() {
    this.brandName = '';
    this.brandId = 0;
    this.isDisabledBrand = true;
    this.isShowBrand = false;
  }

  createUpdateBrand() {
    if (this.catId == 0) {
      this.tostr.warning('Select Category....!');
      return;
    }

    if (this.brandName == '') {
      this.tostr.warning('Enter Brand....!');
      return;
    }

    try {
      this.com.showLoader();
      const obj = {
        categoryId: this.catId,
        id: this.brandId,
        name: this.brandName,
        dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      };

      this.apiService
        .saveObj('Inventory/AddUpdateBrand', obj)
        .subscribe((result) => {
          if (result == true || result == 'true') {
            this.onChangeCatrgory({ id: obj.categoryId });
            this.tostr.success('Save Successfully');
            this.refreshBrand();
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

  editBrand(id: any, name: any): void {
    this.brandName = name;
    this.brandId = id;
    this.isDisabledBrand = false;
    this.isShowBrand = true;
  }

  deleteBrand(id: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (!confirmDelete) {
      return;
    }
    try {
      this.com.showLoader();
      const obj = {
        categoryId: this.catId,
        id: id,
        dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      };

      this.apiService.deleteData('Inventory/DeleteBrand', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.onChangeCatrgory({ id: obj.categoryId });
            this.tostr.success('Delete Successfully');
            this.refreshBrand();
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

  //#endregion

  //#region =================== UOM ====================//

  getUomList() {
    this.apiService.getData('Inventory/GetUom').subscribe((data) => {
      this.uom = data;
    });
  }

  newUom() {
    this.refreshUom();
    this.isDisabledUom = false;
    this.isShowUom = true;
  }

  refreshUom() {
    this.uomName = '';
    this.uomId = 0;
    this.isDisabledUom = true;
    this.isShowUom = false;
  }

  createUpdateUom() {
    if (this.uomName == '') {
      this.tostr.warning('Enter Uom....!');
      return;
    }
    try {
      this.com.showLoader();
      const obj = {
        id: this.uomId,
        name: this.uomName,
        dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      };
      this.apiService
        .saveObj('Inventory/AddUpdateUom', obj)
        .subscribe((result) => {
          if (result == true || result == 'true') {
            this.getUomList();
            this.tostr.success('Save Successfully');
            this.refreshUom();
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

  editUom(id: any, name: any): void {
    this.uomName = name;
    this.uomId = id;
    this.isDisabledUom = false;
    this.isShowUom = true;
  }

  deleteUom(id: any): void {
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

      this.apiService.deleteData('Inventory/DeleteUom', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.getUomList();
            this.tostr.success('Delete Successfully');
            this.refreshUom();
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

  //#endregion

  //#region =================== MADE IN ====================//

  getMadeInList() {
    this.apiService.getData('Inventory/GetMadeIn').subscribe((data) => {
      if(data.length > 0){
        this.madeIn = data;
      }
    });
  }

  newMadeIn() {
    this.refreshMadeIn();
    this.isDisabledMadeIn = false;
    this.isShowMadeIn = true;
  }

  refreshMadeIn() {
    this.madeInName = '';
    this.madeInId = 0;
    this.isDisabledMadeIn = true;
    this.isShowMadeIn = false;
  }

  createUpdateMadeIn() {
    if (this.madeInName == '') {
      this.tostr.warning('Enter Made In ....!');
      return;
    }

    try {
      this.com.showLoader();

      const obj = {
        id: this.madeInId,
        name: this.madeInName,
        dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      };
      this.apiService
        .saveObj('Inventory/AddUpdateMadeIn', obj)
        .subscribe((result) => {
          if (result == true || result == 'true') {
            this.getMadeInList();
            this.tostr.success('Save Successfully');
            this.refreshMadeIn();
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

  editMadeIn(id: any, name: any): void {
    this.madeInName = name;
    this.madeInId = id;
    this.isDisabledMadeIn = false;
    this.isShowMadeIn = true;
  }

  deleteMadeIn(id: any): void {
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

      this.apiService.deleteData('Inventory/DeleteMadeIn', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.getMadeInList();
            this.tostr.success('Delete Successfully');
            this.refreshMadeIn();
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

  //#endregion

  onFileSelected(event: any) {
    this.productImage = event.target.files[0];
    if (this.productImage) {
      this.selectedImage = URL.createObjectURL(event.target.files[0]);
    }
  }

  onCategoryFile(event: any) {
    this.catrgoryImage = event.target.files[0];
    if (this.catrgoryImage) {
      this.selectedCategoryImage = URL.createObjectURL(event.target.files[0]);
    }
  }

  export(fileName: any, formate: any) {
    this.com.ExportFiles(fileName, formate);
  }
}
