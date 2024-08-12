import { Component, ViewChild, ElementRef } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';
import { ActivatedRoute } from '@angular/router';
import { NgSelectComponent } from '@ng-select/ng-select';

@Component({
  selector: 'app-stock-transfer',
  templateUrl: './stock-transfer.component.html',
  styleUrls: ['./stock-transfer.component.css'],
})
export class StockTransferComponent {
  costCenter = localStorage.getItem('costCenter');
  JobList: any[] = [];

  fromDate: Date;
  toDate: Date;
  stockTransferList: any[] = [];
  stockTransferForm!: FormGroup;
  productList: any[] = [];
  fromLocationList: any[] = [];
  uomList: any[] = [];
  toLocationList: any[] = [];
  vchno: any;
  vchDate: Date;
  stockTransfer_List: any[] = [];
  isRowEdit: boolean = false;
  isShowPage: boolean = true;
  isNew: boolean = false;
  isNewClick = false;
  isPageDisabled: boolean = true;
  isPrint: boolean = false;
  otherLocation: boolean = false;
  tag: any = 'same';
  @ViewChild('stockTransferLists', { static: false })
  stockTransferLists!: ElementRef;

  constructor(
    private apiService: ApiService,
    private fb: FormBuilder,
    private toast: ToastrService,
    private dp: DatePipe,
    private com: CommonService,
    private auth: AuthService
  ) {
    const today = new Date();
    this.vchDate = today;
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  }

  openRowReport(item: any) {
    const vchno = item.VCHNO;
    const vchtype = item.VCHTYPE;

    let url = `TransferLoad?VchType=${vchtype}&VchNo=${vchno}&CompId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}`;
    this.com.viewReport(url);
  }

  onViewReport() {
    const vchno = this.vchno;

    let url = `TransferLoad?VchType=STK-TRF&VchNo=${vchno}&CompId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}`;
    this.com.viewReport(url);
  }

  async ngOnInit() {
    this.formInit();
    this.getStockTransferList();
    this.getProductList();
    this.onClickRefresh();

    this.JobList = await this.com.getJobList(true);
  }

  formInit() {
    this.stockTransferForm = this.fb.group({
      sno: [0],
      vchNo: [6],
      vchDate: this.vchDate,
      productCode: [null],
      locFromId: [null],
      fromJobNo: [null],
      expiryDate: [''],
      uomId: [null],
      qty: [''],
      locToId: [null],
      toJobNo: [null],
      status: [''],
      dtNow: new Date(),
    });

    this.stockTransferForm.get('fromJobNo').disable();
    this.stockTransferForm.get('toJobNo').disable();
  }

  togglePages() {
    this.isShowPage = !this.isShowPage;
    if (this.isShowPage) {
      this.onClickRefresh();
    }
  }

  getStockTransferList() {
    const obj = {
      fromDate: this.dp.transform(this.fromDate, 'yyyy/MM/dd'),
      toDate: this.dp.transform(this.toDate, 'yyyy/MM/dd'),
    };

    this.apiService
      .getDataById('Inventory/GetTransferList', obj)
      .subscribe((data) => {
        this.stockTransferList = data;
      });
  }

  onClickRefresh() {
    this.resetForm();
    this.isPageDisabled = true;
    this.isNewClick = false;
    this.isNew = false;
    this.stockTransfer_List = [];
    this.vchno = '';
    this.vchDate = new Date();
    this.isPrint = false;
  }

  resetForm() {
    this.fromLocationList = [];
    this.uomList = [];
    this.toLocationList = [];
    this.stockTransferForm.get('productCode')?.patchValue(null);
    this.stockTransferForm.get('locFromId')?.patchValue(null);
    this.stockTransferForm.get('uomId')?.patchValue(null);
    this.stockTransferForm.get('qty')?.patchValue('');
    this.stockTransferForm.get('locToId')?.patchValue(null);
    this.isRowEdit = false;
  }

  onClickSameLocation(event: any): void {
    if (this.isNewClick) {
      event.preventDefault();
      return;
    }

    this.otherLocation = false;
    this.tag = 'same';
  }

  onClickOtherLocation(event: any): void {
    if (this.isNewClick) {
      event.preventDefault();
      return;
    }

    this.otherLocation = true;
    this.tag = 'other';
  }

  onClickNew() {
    let result = this.com.isStopEntry('STK-TRF');
    if (!result) {
      this.toast.info('You are not allowed');
      return;
    }

    this.getMaxNumber();
    this.isPageDisabled = false;
    this.isNewClick = true;
    this.isNew = true;
    this.isRowEdit = false;
  }

  getMaxNumber() {
    this.apiService.getData('Inventory/GetSTMax').subscribe((data) => {
      this.vchno = data[0].vchno;
    });
  }

  getProductList() {
    this.apiService
      .getDataById('Inventory/GetProductList', { isStock: true })
      .subscribe((data) => {
        this.productList = data;
      });
  }

  async onChangeProduct(event: any) {
    if (event == null) {
      this.onClearProduct();
      return;
    }

    const obj = {
      code: event.code,
      vchNo: this.vchno,
      vchType: 'STK-TRF',
    };

    try {
      const fLocdata = await this.apiService
        .getDataById('Inventory/GetSTFromLocation', obj)
        .toPromise();
      if (fLocdata.length > 0) {
        this.stockTransferForm.get('locFromId')?.setValue(fLocdata[0].SHELFID);
        this.fromLocationList = fLocdata;
      }

      const uomData = await this.apiService
        .getDataById('Inventory/GetProductUom', obj)
        .toPromise();
      if (uomData.length > 0) {
        this.stockTransferForm.get('uomId')?.setValue(uomData[0].UOMID);
        this.uomList = uomData;
      }

      if(!this.isRowEdit){
        this.onChangeFromLoc(fLocdata[0]);
      }

    } catch (error) {
      console.error('An error occurred:', error);
    }
  }

  expId: any;
  async onChangeFromLoc(e: any) {

    if(e.SHELFID){
      this.expId = e.SHELFID + '-' + e.EXPIRYDATE;
    }
    else{
      this.expId = e.locFromId + '-' + e.expiryDate;
    }

    const data = await this.apiService
      .getDataById('Inventory/GetSTToLocation', {
        id: e.SHELFID ?? e.locFromId,
        tag: this.tag,
      })
      .toPromise();
    //this.stockTransferForm.get('locToId')?.patchValue(null);
    this.toLocationList = data;
    this.OnInputQty();
  }

  onClearProduct() {
    this.stockTransferForm.get('locFromId')?.patchValue(null);
    this.stockTransferForm.get('uomId')?.patchValue(null);
    this.stockTransferForm.get('locToId')?.patchValue(null);
    this.fromLocationList = [];
    this.uomList = [];
    this.toLocationList = [];
  }

  OnInputQty() {

    const loc = this.fromLocationList.find(
      (option) => option.SHELFID + '-' + option.EXPIRYDATE == this.expId
    );

    const uom = this.uomList.find(
      (option) => option.UOMID === this.stockTransferForm.get('uomId')?.value
    );

    let basePacking = uom.BASPACKING;
    let packing = uom.PACKING;

    let myValue = this.stockTransferForm.get('qty')?.value;
    let qty = parseFloat(myValue) * parseFloat(packing);
    let myQty = Math.floor(parseFloat(loc.BALANCE) / parseFloat(packing));

    if (
      packing == 1 &&
      parseFloat(myValue) >= parseFloat(basePacking) &&
      packing != parseFloat(basePacking)
    ) {
      if (myQty < basePacking) {
        if (myQty < 0) {
          this.stockTransferForm.get('qty')?.setValue(0);
        } else {
          this.stockTransferForm.get('qty')?.setValue(myQty);
        }
      } else {
        this.stockTransferForm
          .get('qty')
          ?.setValue(parseFloat(basePacking) - 1);
      }
    } else if (qty > loc.BALANCE) {
      if (myQty < 0) {
        this.stockTransferForm.get('qty')?.setValue(0);
      } else {
        this.stockTransferForm.get('qty')?.setValue(myQty);
      }
    }
  }

  appendData(): void {
    if (this.tag != 'same') {
      if (this.stockTransfer_List.length == 1) {
        if (!this.isRowEdit) {
          this.toast.warning('you just add one product');
          return;
        }
      }
    }

    let form = this.stockTransferForm.value;

    if (form.productCode == '' || form.productCode == null) {
      this.toast.warning('Select Product....!');
      return;
    }

    if (form.locFromId == '' || form.locFromId == null) {
      this.toast.warning('Select From Location....!');
      return;
    }
    if (form.uomId == '' || form.uomId == null) {
      this.toast.warning('Select UOM....!');
      return;
    }

    if (form.qty == '' || form.qty == null) {
      this.toast.warning('Enter Qty....!');
      return;
    }

    if (form.locToId == '' || form.locToId == null) {
      this.toast.warning('Select To Location....!');
      return;
    }

    const product = this.productList.find(
      (option) => option.code === form.productCode
    );

    if (product) {
      form.productName = product.name;
    }

    const fLocation = this.fromLocationList.find(
      (option) => option.SHELFID === form.locFromId
    );

    if (fLocation) {
      form.fLocation = fLocation.LOCATION;
      form.expiryDate = fLocation.EXPIRYDATE;
    }

    const uom = this.uomList.find((option) => option.UOMID === form.uomId);

    if (uom) {
      form.uom = uom.UOM;
    }

    const tLocation = this.toLocationList.find(
      (option) => option.id === form.locToId
    );

    if (tLocation) {
      form.tLocation = tLocation.name;
    }

    const fjobName = this.JobList.find((x) => x.ID == form.fromJobNo);
    form.fromJobName = fjobName != undefined ? fjobName.NAME : '';

    const tjobName = this.JobList.find((x) => x.ID == form.toJobNo);
    form.toJobName = tjobName != undefined ? tjobName.NAME : '';

    const added = this.stockTransfer_List.filter(
      (item) =>
        item.productCode == form.productCode &&
        item.locFromId == form.locFromId &&
        item.expiryDate == form.expiryDate &&
        item.sno != form.sno
    );
    if (added.length > 0) {
      this.toast.info('This Product Already Added....!');
      return;
    }

    if (this.isRowEdit) {
      const index = this.stockTransfer_List.findIndex(
        (row) => row.sno === form.sno
      );
      if (index !== -1) {
        this.stockTransfer_List[index] = form;
        this.isRowEdit = false;
        this.resetForm();
        return;
      }
    }

    form.sno = this.stockTransfer_List.length + 1;
    this.stockTransfer_List.push(form);
    this.resetForm();
  }

  async editRow(row: any) {
    this.isRowEdit = true;
    this.stockTransferForm.get('sno')?.setValue(row.sno);
    this.stockTransferForm.get('productCode')?.setValue(row.productCode);
    await this.onChangeProduct({ code: row.productCode });
    await this.onChangeFromLoc(row);
    this.stockTransferForm.get('locFromId')?.setValue(row.locFromId);
    this.stockTransferForm.get('uomId')?.setValue(parseInt(row.uomId));
    this.stockTransferForm.get('qty')?.setValue(row.qty);
    this.stockTransferForm.get('locToId')?.setValue(row.locToId);
    this.stockTransferForm.get('fromJobNo')?.setValue(row.fromJobNo);
    this.stockTransferForm.get('toJobNo')?.setValue(row.toJobNo);
  }

  removeRow(row: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');
    if (!confirmDelete) {
      return;
    }
    const indexToRemove = this.stockTransfer_List.findIndex(
      (item) => item.productName === row.productName
    );
    if (indexToRemove !== -1) {
      this.stockTransfer_List.splice(indexToRemove, 1);
    }
  }

  onClickSave() {
    let result = this.com.isStopEntry('STK-TRF');
    if (!result) {
      this.toast.info('You are not allowed');
      return;
    }

    if (this.stockTransfer_List.length == 0) {
      this.toast.warning('Add Product First...');
      return;
    }

    try {
      this.com.showLoader();

      const transfer: any[] = this.stockTransfer_List.map((data) => ({
        vchNo: this.vchno,
        vchDate: this.dp.transform(this.vchDate, 'yyyy-MM-dd'),
        productCode: data.productCode,
        locFromId: data.locFromId,
        fromJobNo: data.fromJobNo ?? 0,
        expiryDate: this.dp.transform(
          new Date(
            data.expiryDate.split('/')[2],
            data.expiryDate.split('/')[1] - 1,
            data.expiryDate.split('/')[0]
          ),
          'yyyy-MM-dd'
        ),
        uomId: data.uomId.toString(),
        qty: data.qty,
        locToId: data.locToId,
        toJobNo: data.toJobNo ?? 0,
        status: this.isNew == true ? 'new' : 'edit',
        tag: this.tag,
        dtNow: new Date(),
      }));

      this.apiService
        .saveData('Inventory/SaveUpdateTransfer', transfer)
        .subscribe((r) => {
          if (r.status == true || r.status == 'true') {
            this.toast.success('Save Successfully');
            this.vchno = r.vchNo;
            this.getStockTransferList();
            this.isPrint = true;
            this.isNew = false;
            this.com.hideLoader();
            //this.onClickRefresh();
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

  editStockTransfer(VCHNO: any): void {
    let result = this.com.isStopEntry('STK-TRF');
    if (!result) {
      this.toast.info('You are not allowed');
      return;
    }

    try {
      this.com.showLoader();
      this.isPageDisabled = false;
      this.isNewClick = true;
      this.isNew = false;

      this.apiService
        .getDataById('Inventory/EditTransfer', { vchNo: VCHNO })
        .subscribe((data) => {
          data.forEach((item: any) => {
            let form = this.stockTransferForm.value;
            form.sno = this.stockTransfer_List.length + 1;
            (form.productCode = item.CODE),
              (form.productName = item.NAMES),
              (form.locFromId = item.SHELFID),
              (form.fLocation = item.FROMLOCATION),
              (form.fromJobNo = item.FROMJOBNO),
              (form.fromJobName = item.FROMJOBNAME),
              (form.qty = item.QTY),
              (form.uomId = item.UOM),
              (form.uom = item.UOMNAME),
              (form.locToId = item.TOSHELFNO),
              (form.tLocation = item.TOLOCATION),
              (form.toJobNo = item.TOJOBNO),
              (form.toJobName = item.TOJOBNAME),
              (form.expiryDate = item.EXPDATE),
              this.stockTransfer_List.push(form);
            this.resetForm();

            if (item.TAG == 'other') {
              this.otherLocation = true;
            } else {
              this.otherLocation = false;
            }
            this.tag = item.TAG;
            this.vchno = item.VCHNO;
            this.vchDate = new Date(
              item.VCHDATE.split('/')[2],
              item.VCHDATE.split('/')[1] - 1,
              item.VCHDATE.split('/')[0]
            );
          });
          this.isPrint = true;
          this.togglePages();
          this.com.hideLoader();
        });
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      this.com.hideLoader();
    }
  }

  deleteStockTransfer(vchNo: any): void {
    let result = this.com.isStopEntry('STK-TRF');
    if (!result) {
      this.toast.info('You are not allowed');
      return;
    }

    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (!confirmDelete) {
      return;
    }
    try {
      this.com.showLoader();
      this.apiService
        .deleteData('Inventory/DeleteTransfer', {
          vchNo: vchNo,
          dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
        })
        .subscribe({
          next: (data) => {
            if (data == 'true' || data == true) {
              this.toast.success('Delete Successfully');
              this.getStockTransferList();
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

  searchGrid(event: any): void {
    const tableElement = this.stockTransferLists.nativeElement;
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

  export(fileName: any, formate: any) {
    this.com.ExportFiles(fileName, formate);
  }
}
