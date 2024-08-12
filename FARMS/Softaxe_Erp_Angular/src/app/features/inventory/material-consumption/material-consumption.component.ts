import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-material-consumption',
  templateUrl: './material-consumption.component.html',
  styleUrls: ['./material-consumption.component.css']
})
export class MaterialConsumptionComponent {

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

  itemMain: any = [];
  itemsList: any = [];
  prodLocationList: any = [];
  uomList: any = [];
  locationList: any = [];
  detailsList: any = [];

  isShow = false;
  isShowPage: boolean = true;
  MaterialConsumptionForm!: FormGroup;
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
    this.getItemMain();
    this.getProdLocation();
    this.getUOM();
  }

  formInit() {
    this.MaterialConsumptionForm = this.fb.group({
      locationUnit: [this.auth.locId()],
      date: [new Date()],
      itemMain: [undefined],
      product: [undefined],
      uom: [undefined],
      prodLocation: [undefined],
      stock: [''],
      balance: [''],
      consQty: [''],

    });
  }

  resetForm() {
    this.readonly = true;

    this.vchNo = 0;
    this.totalConsQty = 0;
    this.totalStock = 0;
    this.totalBal = 0;

    this.itemsList = [];

    this.MaterialConsumptionForm.get('locationUnit')?.patchValue(this.auth.locId());
    this.MaterialConsumptionForm.get('date')?.patchValue(new Date());
    this.MaterialConsumptionForm.get('product')?.patchValue(undefined);
    this.MaterialConsumptionForm.get('prodLocation')?.patchValue(undefined);
    this.MaterialConsumptionForm.get('uom')?.patchValue(undefined);
    this.MaterialConsumptionForm.get('consQty')?.patchValue('');
  }

  async getItemMain() {
    const result = await this.apiService
      .getDataById('Common/GetLevel4CodeNameByTag', { tag: 'S' })
      .toPromise();
    this.itemMain = result;
  }

  async getProducts(event: any) {
    const result = await this.apiService
      .getDataById('Common/GetLevel5CodeNameByL4Code', { code: event.CODE })
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
        .getDataById('Inventory/GetMaterialConsumption', obj)
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
    this.MaterialConsumptionForm.get('prodLocation')?.patchValue(item.ShelfId);
    this.MaterialConsumptionForm.get('uom')?.patchValue(item.Uomid);

    var obj = {
      date: this.dp.transform(this.MaterialConsumptionForm.get('date')?.value, 'yyyy-MM-dd'),
      itemCode: event.CODE
    };

    try {
      this.apiService
        .getDataById('Inventory/GetMaterialConsumptionBalance', obj)
        .subscribe((result) => {

          this.MaterialConsumptionForm.get('balance')?.patchValue(result[0].BALQTY);
        });
    } catch (err) {
      console.log(err);
    } finally {
    }
  }


  onStockInput(event: any) {
    if (event.target.value == '') {
      this.MaterialConsumptionForm.get('consQty')?.patchValue('');
      return;
    }

    const bal = this.MaterialConsumptionForm.get('balance').value ?? 0;

    if (parseFloat(event.target.value) > parseFloat(bal)) {
      this.MaterialConsumptionForm.get('stock')?.patchValue(bal);
    }



    var total = parseFloat(bal) - parseFloat(event.target.value);
    this.MaterialConsumptionForm.get('consQty')?.patchValue(total);
  }

  itemRefresh() {
    this.MaterialConsumptionForm.get('product')?.patchValue(undefined);
    this.MaterialConsumptionForm.get('prodLocation')?.patchValue(undefined);
    this.MaterialConsumptionForm.get('uom')?.patchValue(undefined);
    this.MaterialConsumptionForm.get('stock')?.patchValue('');
    this.MaterialConsumptionForm.get('balance')?.patchValue('');
    this.MaterialConsumptionForm.get('consQty')?.patchValue('');

    this.totalConsQty = 0;
    this.totalStock = 0;
    this.totalBal = 0;
  }

  onAdd() {
    let form = this.MaterialConsumptionForm.value;

    let EditExist = true;

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

    if (form.itemMain === undefined || form.itemMain === 'undefined' || form.itemMain === '') {
      this.tostr.warning('Select Main....!');
      return;
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


    if (form.stock === null || form.stock === 'null' || form.stock === '') {
      this.tostr.warning('Enter Stock....!');
      return;
    }

    form.itemMainName = this.itemMain.find((i) => i.CODE === form.itemMain).NAME;
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

    this.MaterialConsumptionForm.get('itemMain')?.patchValue(row.itemMain);

    this.getProducts({ CODE: row.itemMain });
    this.MaterialConsumptionForm.get('product')?.patchValue(row.dmcode + row.code);

    this.MaterialConsumptionForm.get('prodLocation')?.patchValue(row.prodLocation);
    this.MaterialConsumptionForm.get('uom')?.patchValue(parseInt(row.uom) ?? 0);
    this.MaterialConsumptionForm.get('stock')?.patchValue(row.stock);
    this.MaterialConsumptionForm.get('balance')?.patchValue(row.balance);
    this.MaterialConsumptionForm.get('consQty')?.patchValue(row.consQty);
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

    let body = this.MaterialConsumptionForm.value;

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
      stock: parseFloat(data.stock) ?? 0,
      balance: parseFloat(data.balance) ?? 0,
      consQty: parseFloat(data.consQty) ?? 0,
    }));

    try {
      this.com.showLoader();

      this.apiService
        .saveData('Inventory/SaveMaterialConsumption', voucher)
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
        .getDataById('Inventory/GetEditMaterialConsumption', { vchNo: VCHNO })
        .subscribe((data) => {
          this.togglePages();

          const d = data[0];
          this.vchNo = d.vchNo;

          this.MaterialConsumptionForm.get('locationUnit')?.patchValue(d.locationUnit);

          this.MaterialConsumptionForm.get('date')?.patchValue(
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
          .deleteData('Inventory/DelMaterialConsumption', { vchNo: VCHNO })
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

  totalStock: number = 0;
  totalBal: number = 0;
  totalConsQty: number = 0;

  total() {
    this.totalStock = this.detailsList.reduce((total, item) => total + item.stock, 0);
    this.totalBal = this.detailsList.reduce((total, item) => total + item.balance, 0);
    this.totalConsQty = this.detailsList.reduce((total, item) => total + item.consQty, 0);
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