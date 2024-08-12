import { Component, ViewChild, ElementRef } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';
@Component({
  selector: 'app-stock-credit-note',
  templateUrl: './stock-credit-note.component.html',
  styleUrls: ['./stock-credit-note.component.css'],
})
export class StockCreditNoteComponent {
  @ViewChild('stockCreditLists', { static: false }) stockCreditLists!: ElementRef;
  costCenter = localStorage.getItem('costCenter')
  JobList: any[] = [];
  
  stockCreditForm!: FormGroup;
  stockCreditList: any[] = [];
  stockCredit_List: any[] = [];
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

    let url = `PrintVoucherRangeWise?DateFrom=${fromDate}&DateTo=${toDate}&VchType=STK-CR&VchNoFrom=${vchNo}&VchNoTo=${vchNo}&CompId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}`;
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
    this.getStockCreditList();
    this.getLocation();
    this.getProductList();
    this.formInit();
    this.JobList = await this.com.getJobList(true);
  }

  formInit() {
    this.stockCreditForm = this.fb.group({
      sno: [0],
      vchNo: [0],
      vchDate: [''],
      vchType: [''],
      productCode: [undefined],
      locationId: [undefined],
      expiryDate: [this.expiryDate],
      uomId: [undefined],
      qty: [''],
      remarks: [''],
      jobNo: [null],
      status: ['New'],
      tag: ['Credit'],
      dtNow: [''],
    });
  }

  getStockCreditList() {
    const obj = {
      fromDate: this.dp.transform(this.fromDate, 'yyyy/MM/dd'),
      toDate: this.dp.transform(this.toDate, 'yyyy/MM/dd'),
    };

    this.apiService
      .getDataById('Inventory/GetCreditNoteList', obj)
      .subscribe((data) => {
        this.stockCreditList = data;
      });
  }

  togglePages() {
    this.isShowPage = !this.isShowPage;
    if (this.isShowPage) {
      this.onClickRefresh();
    }
  }

  getMaxNumber() {
    this.apiService.getData('Inventory/GetCNMax').subscribe((data) => {
      this.vchno = data[0].vchno;
    });
  }

  getProductList() {
    this.apiService.getDataById('Inventory/GetProductList', {isStock: false}).subscribe((data) => {
      this.productList = data;
    });
  }

  getLocation() {
    this.apiService.getData('Inventory/GetLocation').subscribe((data) => {
      this.locationList = data;
    });
  }

  async onChangeProduct(event: any) {

    if(event == undefined){
      this.onClearProduct();
      return;
    }

    const obj = {
      code: event.code,
      vchno: this.vchno,
    };

    try {
      const lastExp = await this.apiService.getDataById('Inventory/GetProductLastExp', { code: event.code }).toPromise();

      let dParts = (lastExp[0].EXPDATE).split('/');
      this.stockCreditForm.get('expiryDate')?.patchValue(new Date(dParts[2], dParts[1] - 1, dParts[0]));

      const uomData = await this.apiService.getDataById('Inventory/GetProductUom', obj).toPromise();

      this.uomList = uomData;
      this.stockCreditForm.get('uomId')?.setValue(uomData[0].UOMID);
    } catch (error) {
      console.error('An error occurred:', error);
    }
  }

  onClearProduct() {
    this.stockCreditForm.get('locationId')?.patchValue(undefined);
    this.stockCreditForm.get('uomId')?.patchValue(undefined);
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
    this.stockCredit_List = [];
    this.isPrint = false;
  }

  onClickNew() {
    let result = this.com.isStopEntry("STK-CR");
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
    this.stockCreditForm.get('productCode')?.patchValue(undefined);
    this.stockCreditForm.get('locationId')?.patchValue(undefined);
    this.stockCreditForm.get('expiryDate')?.patchValue(new Date());
    this.stockCreditForm.get('uomId')?.patchValue(undefined);
    this.stockCreditForm.get('qty')?.patchValue('');
    this.stockCreditForm.get('remarks')?.patchValue('');
    this.uomList = [];
    this.isRowEdit = false;
  }

  appendData(): void {
    let form = this.stockCreditForm.value;

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

    form.expiryDate = this.dp.transform(form.expiryDate, 'dd/MM/yyyy');
    const product = this.productList.find(
      (option) => option.code === form.productCode
    );
    if (product) {
      form.productName = product.name;
    }

    const location = this.locationList.find(
      (option) => option.id === form.locationId
    );
    if (location) {
      form.location = location.name;
    }

    const uom = this.uomList.find((option) => option.UOMID === form.uomId);
    if (uom) {
      form.uomName = uom.UOM;
    }

    const jobName = this.JobList.find((x) => x.ID == form.jobNo)
    form.jobName = (jobName != undefined) ? jobName.NAME : '';

    if (this.isRowEdit) {
      const index = this.stockCredit_List.findIndex(
        (row) => row.sno === form.sno
      );
      if (index !== -1) {
        this.stockCredit_List[index] = form;
        this.isRowEdit = false;
        this.resetForm();
        return;
      }
    }

    form.sno = this.stockCredit_List.length + 1;
    this.stockCredit_List.push(form);
    this.resetForm();
  }

  async editRow(row: any) {
    await this.onChangeProduct({ code: row.productCode });
    this.stockCreditForm.get('sno')?.patchValue(row.sno);
    this.stockCreditForm.get('productCode')?.patchValue(row.productCode);
    this.stockCreditForm.get('locationId')?.patchValue(row.locationId);
    const splitDate = row.expiryDate.split('/');
    const expDate = new Date(splitDate[2], splitDate[1] - 1, splitDate[0]);
    this.stockCreditForm.get('expiryDate')?.patchValue(expDate);
    this.stockCreditForm.get('uomId')?.patchValue(parseInt(row.uomId));
    this.stockCreditForm.get('jobNo')?.patchValue(row.jobNo);
    this.stockCreditForm.get('qty')?.patchValue(row.qty);
    this.stockCreditForm.get('remarks')?.patchValue(row.remarks);
    this.isRowEdit = true;
  }

  removeRow(row: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');
    if (!confirmDelete) {
      return;
    }
    const indexToRemove = this.stockCredit_List.findIndex(
      (item) => item.productCode === row.productCode
    );
    if (indexToRemove !== -1) {
      this.stockCredit_List.splice(indexToRemove, 1);
    }
  }

  saveCreditNote() {

    let result = this.com.isStopEntry("STK-CR");
    if(!result){
      this.toast.info("You are not allowed")
      return;
    }
    
    if (this.stockCredit_List.length == 0) {
      this.toast.warning('Add Voucher First...');
      return;
    }

    try {
      this.com.showLoader();

    const credit: any[] = this.stockCredit_List.map((data) => ({
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
      dtNow: new Date()
    }));

    this.apiService
      .saveData('Inventory/SaveUpdateCreditNote', credit)
      .subscribe((r) => {
        if (r.status == true || r.status == 'true') {
          this.toast.success('Save Successfully');
          this.vchno = r.vchNo
          this.getStockCreditList();
          this.isNew = false;
          this.isPrint = true;
          this.com.hideLoader();
          //this.onClickRefresh();
        } else {
          this.com.hideLoader();
          this.toast.error('Please Save Again');
        }
      });
    }
    catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  editCreditNote(vchno: any): void {

    let result = this.com.isStopEntry("STK-CR");
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
      .getDataById('Inventory/EditCreditNote', { vchNo: vchno })
      .subscribe((data) => {
        data.forEach((item: any) => {
          let form = this.stockCreditForm.value;
          form.sno = this.stockCredit_List.length + 1;
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
            this.stockCredit_List.push(form);
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

  deleteCreditNote(vchno: any): void {

    let result = this.com.isStopEntry("STK-CR");
    if(!result){
      this.toast.info("You are not allowed")
      return;
    }

    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {
      try {
        this.com.showLoader();
      const obj = {
        vchNo: vchno,
        dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      };

      this.apiService.deleteData('Inventory/DeleteCreditNote', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.toast.success('Delete Successfully');
            this.getStockCreditList();
            this.com.hideLoader();
          } else if (data == 'false' || data == false) {
            this.toast.error('Delete Again');
            this.com.hideLoader();
          }
        },
        error: (error) => {
          this.toast.info(error.error.text);
          this.com.hideLoader();
        },
      });
    }
    catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }
  }

  searchGrid(event: any): void {
    const tableElement = this.stockCreditLists.nativeElement;
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
