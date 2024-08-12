import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-finished-goods-production',
  templateUrl: './finished-goods-production.component.html',
  styleUrls: ['./finished-goods-production.component.css']
})
export class FinishedGoodsProductionComponent {

  constructor(
    private tostr: ToastrService,
    private fb: FormBuilder,
    private apiService: ApiService,
    private dp: DatePipe,
    private auth: AuthService,
    private com: CommonService
  ) {
    const today = new Date();
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  }

  @ViewChild('voucherLists', { static: false }) voucherLists!: ElementRef;

  voucherList: any[] = [];
  fromDate: Date;
  toDate: Date;
  showLoader: boolean = false;
  editDisabled: boolean = false;

  vchNo: number = 0;
  itemWt: number = 0;

  level4Code: any = '407001001';
  itemsList: any = [];
  prodLocationList: any = [];
  uomList: any = [];
  locationList: any = [];
  detailsList: any = [];

  isShow = false;
  isShowPage: boolean = true;
  FinishedGoodsForm!: FormGroup;
  editModeSno: boolean = false;
  editMode: boolean = true;
  readonly: boolean = true;

  btnAdd: string = 'Add';
  editSno: any = '';

  ngOnInit() {
    this.showLoader = true;
    this.formInit();
    this.getVouchersList();
    this.getLocationUnit();
    this.getProducts();
    this.getProdLocation();
    this.getUOM();
  }

  formInit() {
    this.FinishedGoodsForm = this.fb.group({
      locationUnit: [this.auth.locId()],
      date: [new Date()],
      product: [undefined],
      prodLocation: [undefined],
      uom: [undefined],
      qty: [''],
      totalWeight: [''],

    });
  }

  resetForm() {
    this.readonly = true;

    this.vchNo = 0;
    this.totalqty = 0;
    this.totalwt = 0;
    this.FinishedGoodsForm.get('locationUnit')?.patchValue(this.auth.locId());
    this.FinishedGoodsForm.get('date')?.patchValue(new Date());
    this.FinishedGoodsForm.get('product')?.patchValue(undefined);
    this.FinishedGoodsForm.get('prodLocation')?.patchValue(undefined);
    this.FinishedGoodsForm.get('uom')?.patchValue(undefined);
    this.FinishedGoodsForm.get('qty')?.patchValue('');
    this.FinishedGoodsForm.get('totalWeight')?.patchValue('');
  }

  async getProducts() {
    const result = await this.apiService
      .getDataById('Common/GetLevel5CodeNameByL4Code', { code: this.level4Code })
      .toPromise();
    this.itemsList = result;
  }

  async getLocationUnit() {
    const result = await this.apiService
      .getData('Common/LocationWithLoc')
      .toPromise();
    this.locationList = result;
  }

  async getProdLocation() {
    const result = await this.apiService
      .getData('Common/GetProductLocation')
      .toPromise();
    this.prodLocationList = result;
  }

  async getUOM() {
    const result = await this.apiService
      .getData('Common/GetUom')
      .toPromise();
    this.uomList = result;
  }

  async getVouchersList() {
    try {
      if (this.showLoader) {
        this.com.showLoader();
      } else {
        this.com.hideLoader();
      }

      const obj = {
        fromDate: this.dp.transform(this.fromDate, 'yyyy/MM/dd'),
        toDate: this.dp.transform(this.toDate, 'yyyy/MM/dd'),
      };

      const result = await this.apiService
        .getDataById('Sale/GetFinishedGoodsProduction', obj)
        .toPromise();

      this.voucherList = result;
      this.com.hideLoader();
      this.showLoader = false;
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  onProductChange(event: any) {
    let item = this.itemsList.find((i) => i.CODE === event.CODE);
    this.FinishedGoodsForm.get('prodLocation')?.patchValue(item.ShelfId);
    this.FinishedGoodsForm.get('uom')?.patchValue(item.Uomid);
    this.FinishedGoodsForm.get('qty')?.patchValue(1);
    this.FinishedGoodsForm.get('totalWeight')?.patchValue(event.Kgs);
    this.itemWt = event.Kgs;
  }

  onQtyInput(event: any) {

    const qty = event.target.value == '' ? 1 : parseFloat(event.target.value);
    var totalWt = this.itemWt * qty;

    if (isNaN(totalWt)) {
      totalWt = this.FinishedGoodsForm.get('totalWeight').value;
    }

    this.FinishedGoodsForm.get('totalWeight')?.patchValue(totalWt);
  }

  itemRefresh() {
    this.FinishedGoodsForm.get('product')?.patchValue(undefined);
    this.FinishedGoodsForm.get('prodLocation')?.patchValue(undefined);
    this.FinishedGoodsForm.get('uom')?.patchValue(undefined);
    this.FinishedGoodsForm.get('qty')?.patchValue('');
    this.FinishedGoodsForm.get('totalWeight')?.patchValue('');
    this.itemWt = 0;
  }

  onAdd() {
    let form = this.FinishedGoodsForm.value;

    let EditExist = true;

    debugger;

    //Duplicate Check
    if (this.editModeSno) {
      const ROwData = this.detailsList.find((row) => row.sno === this.editSno);
      if (ROwData) {
        EditExist = false;
        if (ROwData.product != form.product) {
          EditExist = true;
        }
      }
    }

    if (EditExist == true) {
      const itemDoubleCheck = this.detailsList.find(
        (row) => row.product == form.product
      );
      if (itemDoubleCheck) {
        this.tostr.warning('Item already in table. Select other item....!');
        return;
      }
    }

    if (form.product === undefined || form.product === 'undefined' || form.product === '') {
      this.tostr.warning('Select Product....!');
      return;
    }

    if (form.prodLocation === undefined || form.prodLocation === 'undefined' || form.prodLocation === '') {
      this.tostr.warning('Select Product Location....!');
      return;
    }

    if (form.uom === undefined || form.uom === 'undefined' || form.uom === '') {
      this.tostr.warning('Select UOM....!');
      return;
    }


    if (form.qty === null || form.qty === 'null' || form.qty === '') {
      this.tostr.warning('Enter Quantity....!');
      return;
    }

    let itemName = this.itemsList.find((i) => i.CODE === form.product);

    form.dmcode = form.product.substring(0, 9);
    form.code = form.product.substring(9, 14);
    form.itemName = itemName.NAME;
    form.uomName = this.uomList.find((i) => i.ID === form.uom).UOM;
    form.prodLocationName = this.prodLocationList.find((i) => i.SHELFNO === form.prodLocation).SKU;

    if (this.editModeSno) {
      const index = this.detailsList.findIndex(
        (row) => row.sno === this.editSno
      );
      if (index !== -1) {
        form.sno = this.editSno;
        this.detailsList[index] = form;
        this.editModeSno = false;
        this.editSno = '';
        this.btnAdd = 'Add';
        this.itemRefresh();
        return;
      }
    }

    form.sno = this.detailsList.length + 1;
    this.detailsList.push(form);
    this.itemRefresh();
    this.total();
  }

  editItem(row: any) {
    this.btnAdd = 'Update';

    this.editModeSno = true;
    this.editSno = row.sno;

    this.FinishedGoodsForm.get('product')?.patchValue(row.dmcode + row.code);
    this.FinishedGoodsForm.get('prodLocation')?.patchValue(row.prodLocation);
    this.FinishedGoodsForm.get('uom')?.patchValue(parseInt(row.uom) ?? 0);
    this.FinishedGoodsForm.get('qty')?.patchValue(row.qty);
    this.FinishedGoodsForm.get('totalWeight')?.patchValue(row.totalWeight);

    this.itemWt = row.totalWeight / row.qty;
  }

  deleteItem(index: any) {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (!confirmDelete) {
      return;
    }

    if (index !== -1) {
      this.detailsList.splice(index, 1);
    }

    this.total();

  }

  async onClickSave() {
    if (this.detailsList.length == 0) {
      this.tostr.warning('Incomplete Transaction...');
      return;
    }

    let body = this.FinishedGoodsForm.value;

    // let vchNo = 0
    // if(this.editMode == true){vchNo = this.requisitionForm.get('transNo')?.value}
    // else {vchNo = 0}

    const voucher: any[] = this.detailsList.map((data) => ({
      vchNo: this.vchNo,
      locationUnit: body.locationUnit.toString(),
      date: this.dp.transform(body.date, 'yyyy-MM-dd'),

      dmcode: data.dmcode.toString(),
      code: data.code.toString(),
      prodLocation: data.prodLocation.toString(),
      uom: data.uom.toString(),
      qty: parseInt(data.qty) ?? 0,
      wt: parseFloat(data.totalWeight) ?? 0,
    }));

    try {
      this.com.showLoader();

      this.apiService
        .saveData('Sale/SaveFinishedGoodsProduction', voucher)
        .subscribe((result) => {

          this.com.hideLoader();

          if (result == true || result == 'true') {
            this.tostr.success('Save Successfully');
            this.onClickRefresh();
            this.getVouchersList();
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

  editVouchers(VCHNO: any): void {
    this.onClickRefresh();
    this.isShow = true;
    this.editMode = true;
    this.readonly = false;

    try {
      this.com.showLoader();

      this.apiService
        .getDataById('Sale/GetEditFinishedGoodsProduction', { vchNo: VCHNO })
        .subscribe((data) => {
          this.togglePages();

          const d = data[0];
          this.vchNo = d.vchNo;

          this.FinishedGoodsForm.get('locationUnit')?.patchValue(d.locationUnit);

          this.FinishedGoodsForm.get('date')?.patchValue(
            new Date(
              d.date.split('/')[2],
              d.date.split('/')[1] - 1,
              d.date.split('/')[0]
            )
          );

          data.forEach((item: any) => {
            let form: any = item;
            form.sno = this.detailsList.length + 1;

            this.detailsList.push(form);
          });

          this.total();

          this.com.hideLoader();
        });
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      // this.com.hideLoader();
    }
  }

  deleteVouchers(VCHNO: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {
      try {
        this.com.showLoader();


        this.apiService
          .deleteData('Sale/DelFinishedGoodsProduction', { vchNo: VCHNO })
          .subscribe({
            next: (data) => {
              this.com.hideLoader();

              if (data == 'true' || data == true) {
                this.tostr.success('Delete Successfully');
                this.getVouchersList();
              } else if (data == 'false' || data == false) {
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

  onClickRefresh() {
    this.isShow = false;
    this.resetForm();

    this.detailsList = [];
    this.btnAdd = 'Add';
  }

  togglePages() {
    this.isShowPage = !this.isShowPage;
    if (this.isShowPage) {
    }
  }

  onClickNew() {

    this.isShow = true;
    this.editMode = false;
    this.readonly = false;

    this.vchNo = 0;
    // this.vehicles.nativeElement.focus();
  }

  totalwt:number = 0;
  totalqty:number = 0;

  total(){
    this.totalqty = this.detailsList.reduce((total, item) => total + parseFloat(item.qty), 0);
    this.totalwt = this.detailsList.reduce((total, item) => total + item.totalWeight, 0);
  }

  
  searchGrid(event: any): void {
    const tableElement = this.voucherLists.nativeElement;
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

}