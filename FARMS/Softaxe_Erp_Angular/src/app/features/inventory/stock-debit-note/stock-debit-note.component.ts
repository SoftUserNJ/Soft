import { Component, ViewChild, ElementRef } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-stock-debit-note',
  templateUrl: './stock-debit-note.component.html',
  styleUrls: ['./stock-debit-note.component.css'],
})
export class StockDebitNoteComponent {
  @ViewChild('stockDebitLists', { static: false }) stockDebitLists!: ElementRef;
  costCenter = localStorage.getItem('costCenter')
  JobList: any[] = [];

  stockDebitForm!: FormGroup;
  stockDebitList: any[] = [];
  stockDebit_List: any[] = [];
  productList: any[] = [];
  uomList: any[] = [];
  locationList: any[] = [];
  vchno: any;
  isRowEdit: boolean = false;
  isShowPage: boolean = true;
  isNew: boolean = false;
  fromDate: Date;
  toDate: Date;
  vchDate: Date;
  expiryDate: any;
  isNewClick = false;
  isPageEnabled: boolean = false;
  isPageDisabled: boolean = true;
  isPrint: boolean = false;

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
    this.expiryDate = today;
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  }

  printReport() {
    const vchDate = this.vchDate;
    const vchNo = this.vchno;

    const fromDate = this.dp.transform(vchDate, 'yyyy/MM/dd');
    const toDate = this.dp.transform(vchDate, 'yyyy/MM/dd');

    let url = `PrintVoucherRangeWise?DateFrom=${fromDate}&DateTo=${toDate}&VchType=STK-DR&VchNoFrom=${vchNo}&VchNoTo=${vchNo}&CompId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}`;
    this.com.viewReport(url);
  }

  openRowReport(item: any) {
    var dp = item.vchDate.split('/');
    const date = this.dp.transform(
      new Date(dp[2], dp[1] - 1, dp[0]),
      'yyyy/MM/dd'
    );

    let url = `PrintVoucherRangeWise?DateFrom=${date}&DateTo=${date}&VchType=${
      item.vchType
    }&VchNoFrom=${item.vchno}&VchNoTo=${
      item.vchno
    }&CompId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}`;
    this.com.viewReport(url);
  }

  async ngOnInit() {
    this.getStockDebitList();
    this.getProductList();
    this.formInit();
    this.stockDebitForm.get('expiryDate')?.disable();
    this.JobList = await this.com.getJobList(true);
  }

  formInit() {
    this.stockDebitForm = this.fb.group({
      sno: [0],
      vchNo: [0],
      vchDate: [''],
      vchType: [''],
      productCode: [null],
      locationId: [null],
      expiryDate: [this.expiryDate],
      uomId: [null],
      qty: [''],
      remarks: [''],
      jobNo: [null],
      status: ['New'],
      tag: ['Debit'],
      dtNow: [''],
    });
  }

  getStockDebitList() {
    const obj = {
      fromDate: this.dp.transform(this.fromDate, 'yyyy/MM/dd'),
      toDate: this.dp.transform(this.toDate, 'yyyy/MM/dd'),
    };

    this.apiService
      .getDataById('Inventory/GetDebitNoteList', obj)
      .subscribe((data) => {
        this.stockDebitList = data;
      });
  }

  togglePages() {
    this.isShowPage = !this.isShowPage;
    if (this.isShowPage) {
      this.onClickRefresh();
      this.stockDebitForm.get('jobNo')?.patchValue(null);
    }
  }

  getMaxNumber() {
    this.apiService.getData('Inventory/GetDNMax').subscribe((data) => {
      this.vchno = data[0].vchno;
    });
  }

  getProductList() {
    this.apiService.getDataById('Inventory/GetProductList', {isStock: true}).subscribe((data) => {
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
      vchType: 'STK-DR',
    };

    try {
      const location = await this.apiService
        .getDataById('Inventory/GetSTFromLocation', obj)
        .toPromise();

      this.locationList = location;
      this.stockDebitForm.get('locationId')?.setValue(location[0].SHELFID);
      this.onLocationChange();

      const uomData = await this.apiService
        .getDataById('Inventory/GetProductUom', obj)
        .toPromise();
      this.uomList = uomData;
      this.stockDebitForm.get('uomId')?.setValue(uomData[0].UOMID);
    } catch (error) {
      console.error('An error occurred:', error);
    }
  }

  onClearProduct() {
    this.stockDebitForm.get('locationId')?.patchValue(null);
    this.stockDebitForm.get('uomId')?.patchValue(null);
    this.locationList = [];
    this.uomList = [];
  }

  onClickRefresh() {
    this.isPageDisabled = true;
    this.isNewClick = false;
    this.isNew = false;
    this.vchno = '';
    this.vchDate = new Date();
    this.resetForm();
    this.stockDebit_List = [];
    this.isPrint = false;
  }

  onClickNew() {
    let result = this.com.isStopEntry("STK-DR");
    if(!result){
      this.toast.info("You are not allowed")
      return;
    }
    this.onClickRefresh();
    this.isPageDisabled = false;
    this.isNewClick = true;
    this.isNew = true;
    this.getMaxNumber();
  }

  resetForm() {
    this.stockDebitForm.get('productCode')?.patchValue(null);
    this.stockDebitForm.get('locationId')?.patchValue(null);
    this.stockDebitForm.get('expiryDate')?.patchValue(new Date());
    this.stockDebitForm.get('uomId')?.patchValue(null);
    this.stockDebitForm.get('qty')?.patchValue('');
    this.stockDebitForm.get('remarks')?.patchValue('');
    this.locationList = [];
    this.uomList = [];
    this.isRowEdit = false;
  }

  async onLocationChange() {
    let loc = this.stockDebitForm.get('locationId')?.value;
    const location = this.locationList.find((item) => item.SHELFID === loc);

    if (location) {
      let exp = location.EXPIRYDATE.split('/');
      this.stockDebitForm
        .get('expiryDate')
        ?.setValue(new Date(exp[2], exp[1] - 1, exp[0]));
    }

    this.OnInputQty();
  }

  OnInputQty() {
    const loc = this.locationList.find(
      (option) =>
        option.SHELFID === this.stockDebitForm.get('locationId')?.value
    );

    if (!loc) {
      return;
    }

    const uom = this.uomList.find(
      (option) => option.UOMID === this.stockDebitForm.get('uomId')?.value
    );

    if (!uom) {
      return;
    }
    let basePacking = uom.BASPACKING;
    let packing = uom.PACKING;

    let myValue = this.stockDebitForm.get('qty')?.value;
    let qty = parseFloat(myValue) * parseFloat(packing);
    let myQty = Math.floor(parseFloat(loc.BALANCE) / parseFloat(packing));

    if (
      packing == 1 &&
      parseFloat(myValue) >= parseFloat(basePacking) &&
      packing != parseFloat(basePacking)
    ) {
      if (myQty < basePacking) {
        if (myQty < 0) {
          this.stockDebitForm.get('qty')?.setValue(0);
        } else {
          this.stockDebitForm.get('qty')?.setValue(myQty);
        }
      } else {
        this.stockDebitForm.get('qty')?.setValue(parseFloat(basePacking) - 1);
      }
    } else if (qty > loc.BALANCE) {
      if (myQty < 0) {
        this.stockDebitForm.get('qty')?.setValue(0);
      } else {
        this.stockDebitForm.get('qty')?.setValue(myQty);
      }
    }
  }

  appendData(): void {
    let form = this.stockDebitForm.value;

    if (form.productCode == '' || form.productCode == null) {
      this.toast.warning('Select Product....!');
      return;
    }

    if (form.locationId == '' || form.locationId == null) {
      this.toast.warning('Select Location....!');
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

    if (form.remarks == '' || form.remarks == null) {
      this.toast.warning('Enter Remarks....!');
      return;
    }

    const product = this.productList.find(
      (option) => option.code === form.productCode
    );
    if (product) {
      form.productName = product.name;
    }

    const location = this.locationList.find(
      (option) => option.SHELFID === form.locationId
    );
    if (location) {
      form.location = location.LOCATION;
      form.expiryDate = location.EXPIRYDATE;
    }

    const uom = this.uomList.find((option) => option.UOMID === form.uomId);
    if (uom) {
      form.uomName = uom.UOM;
    }

    const jobName = this.JobList.find((x) => x.ID == form.jobNo)
    form.jobName = (jobName != undefined) ? jobName.NAME : '';

    if (this.isRowEdit) {
      const index = this.stockDebit_List.findIndex(
        (row) => row.sno === form.sno
      );
      if (index !== -1) {
        this.stockDebit_List[index] = form;
        this.isRowEdit = false;
        this.resetForm();
        return;
      }
    }

    form.sno = this.stockDebit_List.length + 1;
    this.stockDebit_List.push(form);
    this.resetForm();
  }

  async editRow(row: any) {
    await this.onChangeProduct({ code: row.productCode });
    this.stockDebitForm.get('sno')?.patchValue(row.sno);
    this.stockDebitForm.get('productCode')?.patchValue(row.productCode);
    this.stockDebitForm.get('locationId')?.patchValue(row.locationId);
    this.onLocationChange();
    //this.stockDebitForm.get('expiryDate')?.patchValue(row.expiryDate);
    this.stockDebitForm.get('uomId')?.patchValue(parseInt(row.uomId));
    this.stockDebitForm.get('jobNo')?.patchValue(row.jobNo);
    this.stockDebitForm.get('qty')?.patchValue(row.qty);
    this.stockDebitForm.get('remarks')?.patchValue(row.remarks);
    this.isRowEdit = true;
  }

  removeRow(row: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');
    if (!confirmDelete) {
      return;
    }
    const indexToRemove = this.stockDebit_List.findIndex(
      (item) => item.productCode === row.productCode
    );
    if (indexToRemove !== -1) {
      this.stockDebit_List.splice(indexToRemove, 1);
    }
  }

  saveDebitNote() {

    let result = this.com.isStopEntry("STK-DR");
    if(!result){
      this.toast.info("You are not allowed")
      return;
    }
    
    if (this.stockDebit_List.length == 0) {
      this.toast.warning('Add Voucher First...');
      return;
    }

    try {
      this.com.showLoader();

      const debit: any[] = this.stockDebit_List.map((data) => ({
        vchNo: this.vchno,
        vchDate: this.dp.transform(this.vchDate, 'yyyy-MM-dd'),
        vchType: data.vchType,
        productCode: data.productCode,
        locationId: data.locationId,
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
        remarks: data.remarks,
        status: this.isNew == true ? 'new' : 'edit',
        tag: data.tag,
        jobNo: data.jobNo ?? 0,
        dtNow: new Date(),
      }));

      this.apiService
        .saveData('Inventory/SaveUpdateDebitNote', debit)
        .subscribe((r) => {
          if (r.status == true || r.status == 'true') {
            this.toast.success('Save Successfully');
            this.vchno = r.vchNo
            this.getStockDebitList();
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

  editDebitNote(vchno: any): void {

    let result = this.com.isStopEntry("STK-DR");
    if(!result){
      this.toast.info("You are not allowed")
      return;
    }

    try {
      this.com.showLoader();

      this.isPageDisabled = false;
      this.isNewClick = true;
      this.isNew = false;

      this.apiService
        .getDataById('Inventory/EditDebitNote', { vchNo: vchno })
        .subscribe((data) => {
          data.forEach((item: any) => {
            let form = this.stockDebitForm.value;
            form.sno = this.stockDebit_List.length + 1;
            (form.productCode = item.CODE),
              (form.productName = item.NAMES),
              (form.locationId = item.SHELFID),
              (form.location = item.LOCATION),
              (form.qty = item.QTY),
              (form.uomId = item.UOM),
              (form.uomName = item.UOMNAME),
              (form.expiryDate = item.EXPDATE),
              (form.remarks = item.DESCRIPTION),
              (form.jobNo = item.JOBNO),
              (form.jobName = item.JOBNAME),
              this.stockDebit_List.push(form);
            this.resetForm();
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
      //this.com.hideLoader();
    }
  }

  deleteDebitNote(vchno: any): void {

    let result = this.com.isStopEntry("STK-DR");
    if(!result){
      this.toast.info("You are not allowed")
      return;
    }

    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (!confirmDelete) {
      return;
    }
    try {
      this.com.showLoader();
      const obj = {
        vchNo: vchno,
        dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      };

      this.apiService.deleteData('Inventory/DeleteDebitNote', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.toast.success('Delete Successfully');
            this.getStockDebitList();
            this.com.hideLoader();
          } else if (data == 'false' || data == false) {
            this.toast.error('Delete Again');
            this.com.hideLoader();
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
    const tableElement = this.stockDebitLists.nativeElement;
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
