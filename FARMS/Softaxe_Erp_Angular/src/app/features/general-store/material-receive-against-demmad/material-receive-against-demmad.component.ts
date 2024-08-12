import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup } from '@angular/forms';
import { parse } from '@devexpress/analytics-core/analytics-criteria';
import { event } from 'jquery';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-material-receive-against-demmad',
  templateUrl: './material-receive-against-demmad.component.html',
  styleUrls: ['./material-receive-against-demmad.component.css']
})
export class MaterialReceiveAgainstDemmadComponent {

  constructor(
    private fb: FormBuilder,
    private tostr: ToastrService,
    private apiService: ApiService,
    private dp: DatePipe
  ) {
    const today = new Date();
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);

    this.fromLDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toLDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  }

  //List Page
  @ViewChild('voucherLists', { static: false }) voucherLists!: ElementRef;
  voucherList: any[] = [];
  fromDate: Date;
  toDate: Date;

  //Form Page
  storePurchaseForm!: FormGroup;
  storePurchaseList: any = [];
  isShow = false;
  isReadOnly: boolean = true;
  isDisabled: boolean = true;
  editMode: boolean = true;
  isShowPage: boolean = true;
  itemCode: any;

  // Below Grid Item
  editModeSno: boolean = false;
  btnAdd: string = 'Add';
  detailGridIndex: any = '';

  //Party
  partyList: any = [];
  partyGroup: any;
  partyBrand: any;

  //Demand List
  @ViewChild('demand', { static: false }) demand!: ElementRef;
  fromLDate: Date;
  toLDate: Date;
  demandList: any = [];
  demandDetail: any = [];

  ngOnInit() {
    this.getRPList();
    this.formInit();
    this.getVchNo();
    this.getParty();
    this.getDemand();
  }

  formInit() {
    this.storePurchaseForm = this.fb.group({
      vchType: ['RP-Store'],
      vchNo: [''],
      date: [new Date()],
      party: [undefined],
      group: [''],
      brand: [''],
      item: [''],
      stockBal: [''],
      demandNo: [0],
      demandQty: [0],
      rcvd: [0],
      rejectedQty: [0],
      rate: [0],
      grossAmount: [0],
      netValue: [0],
      remarks: [''],
      mannualBill: [''],
      taxRate: [0],
      taxAmount: [0],
      carraige: [0],
      freight: [0],
    });
  }

  resetForm() {
    this.storePurchaseForm.get('party')?.setValue(undefined);
    this.storePurchaseForm.get('group')?.setValue('');
    this.storePurchaseForm.get('brand')?.setValue('');
    this.storePurchaseForm.get('item')?.setValue('');
    this.storePurchaseForm.get('stockBal')?.setValue('');
    this.storePurchaseForm.get('demandNo')?.setValue('');
    this.storePurchaseForm.get('demandQty')?.setValue('');
    this.storePurchaseForm.get('rcvd')?.setValue('');
    this.storePurchaseForm.get('rejectedQty')?.setValue('');
    this.storePurchaseForm.get('rate')?.setValue('');
    this.storePurchaseForm.get('grossAmount')?.setValue('');
    this.storePurchaseForm.get('netValue')?.setValue('');
    this.storePurchaseForm.get('remarks')?.setValue('');
    this.storePurchaseForm.get('mannualBill')?.setValue('');
    this.storePurchaseForm.get('taxRate')?.setValue('');
    this.storePurchaseForm.get('taxAmount')?.setValue('');
    this.storePurchaseForm.get('carraige')?.setValue('');
    this.storePurchaseForm.get('freight')?.setValue('');
    this.demandDetail = [];
    this.storePurchaseList = [];
  }

  getDemand() {
    const obj = {
      fromDate: this.dp.transform(this.fromLDate, 'yyyy/MM/dd'),
      toDate: this.dp.transform(this.toLDate, 'yyyy/MM/dd'),
    };
    this.apiService
      .getDataById('GeneralStore/GetStorePurchaseDList', obj)
      .subscribe((data) => {
        this.demandList = data;
      });
  }

  getDemandDetail(event: any) {
    this.resetForm();
    this.rowHighLight(event);
    const clickedRow = event.target.closest('tr');
    const tdClass = clickedRow.querySelector('.vchNo') as HTMLElement;
    this.apiService
      .getDataById('GeneralStore/GetStorePurchaseDDetailList', {
        vchNo: tdClass.textContent,
      })
      .subscribe((data) => {
        this.demandDetail = data;
      });
  }

  demandToFields(event: any) {
    this.rowHighLight(event);
    const clickedRow = event.target.closest('tr');
    const code = clickedRow.querySelector('.code') as HTMLElement;
    const sub = clickedRow.querySelector('.sub') as HTMLElement;
    const iCode = code.textContent + sub.textContent;

    this.apiService
      .getDataById('GeneralStore/GetStorePurchaseStockBlc', { code: iCode })
      .subscribe((data) => {
        this.storePurchaseForm
          .get('stockBal')
          ?.setValue(data.stockBlc[0].STOCKBAL);
        this.storePurchaseForm
          .get('group')
          ?.setValue(data.itemDetail[0].GROUPNAME);
        this.storePurchaseForm
          .get('brand')
          ?.setValue(data.itemDetail[0].BRANDNAME);
      });

    const iDetails = this.demandDetail.find((i) => i.Dmcode + i.Code === iCode);

    this.storePurchaseForm.get('demandNo')?.setValue(iDetails.VCHNO);
    this.storePurchaseForm.get('item')?.setValue(iDetails.Names);
    this.storePurchaseForm.get('demandQty')?.setValue(iDetails.QTY);
    this.storePurchaseForm.get('remarks')?.setValue(iDetails.DesCrp);
    this.storePurchaseForm.get('taxRate')?.setValue(iDetails.SalesTax);
    this.itemCode = iDetails.Dmcode + iDetails.Code;
  }

  getVchNo() {
    this.apiService.getData('GeneralStore/GetStorePurchaseVchNo').subscribe((data) => {
      this.storePurchaseForm.get('vchNo')?.setValue(data[0].VCHNO);
      this.storePurchaseForm.get('date')?.patchValue(new Date());
    });
  }

  getParty() {
    this.apiService
      .getData('GeneralStore/GetStorePurchaseSuppliers')
      .subscribe((data) => {
        this.partyList = data;
      });
  }

  getRPList() {
    const obj = {
      fromDate: this.dp.transform(this.fromDate, 'yyyy/MM/dd'),
      toDate: this.dp.transform(this.toDate, 'yyyy/MM/dd'),
    };

    this.apiService
      .getDataById('GeneralStore/GetStorePurchaseList', obj)
      .subscribe((data) => {
        this.voucherList = data;
      });
  }

  onAdd() {
    let form = this.storePurchaseForm.value;

    if (form.party === null || form.party === undefined) {
      this.tostr.warning('Select Party....!');
      return;
    }

    if (form.demandNo === '' || form.demandNo === null || form.demandNo == 0) {
      this.tostr.warning('DemandNo cannot be empty....!');
      return;
    }

    if (
      form.demandQty === '' ||
      form.demandQty === null ||
      form.demandQty == 0
    ) {
      this.tostr.warning('DemandQty cannot be empty....!');
      return;
    }

    if (form.rcvd === '' || form.rcvd === null || form.rcvd == 0) {
      this.tostr.warning('Enter Rcvd....!');
      return;
    }

    if (form.rate === '' || form.rate === null || form.rate == 0) {
      this.tostr.warning('Enter Rate....!');
      return;
    }

    if (
      form.mannualBill === '' ||
      form.mannualBill === null ||
      form.mannualBill == 0
    ) {
      this.tostr.warning('Enter Mannual Bill#....!');
      return;
    }

    if (form.carraige === '' || form.carraige === null || form.carraige == 0) {
      this.tostr.warning('Enter Carraige....!');
      return;
    }

    if (form.freight === '' || form.freight === null || form.freight == 0) {
      this.tostr.warning('Enter Freight....!');
      return;
    }

    form.itemCode = this.itemCode;
    debugger;
    if (this.editModeSno) {
      if (this.detailGridIndex !== -1) {
        this.storePurchaseList[this.detailGridIndex] = form;
        this.editModeSno = false;
        this.btnAdd = 'Add';
        return;
      }
    }
    this.storePurchaseList.push(form);
  }

  editItem(row: any, i: any) {
    this.btnAdd = 'Update';
    this.detailGridIndex = i;
    this.editModeSno = true;

    this.storePurchaseForm.get('party')?.patchValue(row.party);
    this.storePurchaseForm.get('group')?.patchValue(row.group);
    this.storePurchaseForm.get('brand')?.patchValue(row.brand);
    this.storePurchaseForm.get('item')?.patchValue(row.item);
    this.storePurchaseForm.get('stockBal')?.patchValue(row.stockBal);
    this.storePurchaseForm.get('demandNo')?.patchValue(row.demandNo);
    this.storePurchaseForm.get('demandQty')?.patchValue(row.demandQty);
    this.storePurchaseForm.get('rcvd')?.patchValue(row.rcvd);
    this.storePurchaseForm.get('rate')?.patchValue(row.rate);
    this.storePurchaseForm.get('grossAmount')?.patchValue(row.grossAmount);
    this.storePurchaseForm.get('remarks')?.patchValue(row.remarks);
    this.storePurchaseForm.get('mannualBill')?.patchValue(row.mannualBill);
    this.storePurchaseForm.get('taxRate')?.patchValue(row.taxRate);
    this.storePurchaseForm.get('taxAmount')?.patchValue(row.taxAmount);
    this.storePurchaseForm.get('carraige')?.patchValue(row.carraige);
    this.storePurchaseForm.get('freight')?.patchValue(row.freight);

    this.getRejectedQty();
    this.getGrossAmt();
  }

  deleteItem(index: any) {
    const confirmDelete = confirm('Are you sure you want to delete this item?');
    if (!confirmDelete) {
      return;
    }
    if (index !== -1) {
      this.storePurchaseList.splice(index, 1);
    }
  }

  onClickSave() {
    if (this.storePurchaseList.length == 0) {
      this.tostr.warning('Incomplete Transaction...');
      return;
    }

    let vchNo = this.editMode ? this.storePurchaseForm.get('vchNo')?.value : 0;

    // let vchNo = 0
    // if(this.editMode == true){vchNo = this.requisitionForm.get('transNo')?.value}
    // else {vchNo = 0}

    const voucher: any[] = this.storePurchaseList.map((data) => ({
      vchNo: vchNo,
      vchType: data.vchType,
      date: this.dp.transform(
        this.storePurchaseForm.get('date')?.value,
        'yyyy-MM-dd'
      ),

      party: data.party,
      itemCode: data.itemCode,
      demandNo: parseInt(data.demandNo),
      rcvdQty: parseInt(data.rcvd),
      rate: parseFloat(data.rate),
      grossAmount: data.grossAmount,
      remarks: data.remarks,
      mannualBill: data.mannualBill,
      taxRate: data.taxRate,
      taxAmount: data.taxAmount,
      carriage: data.carraige,
      freight: data.freight,
    }));

    this.apiService
      .saveData('GeneralStore/SaveStorePurchase', voucher)
      .subscribe((result) => {
        if (result == true || result == 'true') {
          this.tostr.success('Save Successfully');
          this.onClickRefresh();
          this.getRPList();
        } else {
          this.tostr.error('Please Save Again');
        }
      });
  }

  editRP(vchNo: any) {
    this.onClickRefresh();
    this.isShow = true;
    this.editMode = true;
    this.isDisabled = false;
    const obj = {
      vchNo: vchNo,
    };
    this.apiService
      .getDataById('GeneralStore/GetEditStorePurchase', obj)
      .subscribe((data) => {
        this.togglePages();
        this.storePurchaseForm.get('vchNo')?.patchValue(vchNo);
        data.forEach((item: any) => {
          this.storePurchaseForm
            .get('date')
            ?.patchValue(
              new Date(
                item.DATE.split('/')[2],
                item.DATE.split('/')[1] - 1,
                item.DATE.split('/')[0]
              )
            );
          //this.storePurchaseForm.get('vchNo')?.patchValue(vchNo);
          let form: any = {};

          form.party = item.MCODE;
          form.group = item.GROUPNAME;
          form.brand = item.BRANDNAME;
          form.item = item.ITEMNAME;
          form.stockBal = item.STOCKBAL;
          form.demandNo = item.DEMANDNO;
          form.demandQty = item.DQTY;
          form.rate = item.RATE;
          form.grossAmount = item.GROSSAMOUNT;
          form.remarks = item.DESCRP;
          form.mannualBill = item.MANNUALBILL;
          form.taxRate = item.TAXRATE;
          form.taxAmount = item.TAXAMOUNT;
          form.carraige = item.CARRIAGE;
          form.freight = item.FREIGHT;
          form.itemCode = item.DMCODE + item.CODE;
          form.rcvd = item.RCVDQTY;

          this.storePurchaseList.push(form);
          console.log(this.storePurchaseList);
        });
      });
  }

  deleteRP(VCHNO: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {
      const obj = {
        vchNo: VCHNO,
      };

      this.apiService.deleteData('GeneralStore/DelStorePurchase', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.tostr.success('Delete Successfully');
            this.getRPList();
            this.getVchNo();
          } else if (data == 'false' || data == false) {
            this.tostr.error('Delete Again');
          }
        },
        error: (error) => {
          this.tostr.info(error.error.text);
        },
      });
    }
  }

  onClickNew() {
    this.isShow = true;
    this.editMode = false;
    this.isDisabled = false;
  }

  onClickRefresh() {
    this.isShow = false;
    this.isDisabled = true;
    this.btnAdd = 'Add';
    this.resetForm();
    this.getVchNo();
  }

  togglePages() {
    this.isShowPage = !this.isShowPage;
    if (this.isShowPage) {
      this.onClickRefresh();
    }
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

  rowHighLight(event: any) {
    // Get the clicked row
    const clickedRow = event.target.closest('tr');

    // Add 'HighLightRow' class to all td elements in the clicked row
    const tds = clickedRow.querySelectorAll('td');
    tds.forEach((td) => {
      td.classList.add('HighLightRow');
    });

    // Remove 'HighLightRow' class from other rows
    const allRows = document.querySelectorAll('tr');
    allRows.forEach((row) => {
      if (row !== clickedRow) {
        const otherRowTds = row.querySelectorAll('td');
        otherRowTds.forEach((td) => {
          td.classList.remove('HighLightRow');
        });
      }
    });
  }

  onlyNumeric(event: any): void {
    const inputVal = event.target.value;
    // Replace non-numeric characters using a regular expression
    let cleanValue = inputVal.replace(/[^0-9.]/g, '');

    // Ensure there is only one dot in the cleaned value
    const dotIndex = cleanValue.indexOf('.');
    if (dotIndex !== -1) {
      const afterDot = cleanValue.substring(dotIndex + 1);
      if (afterDot.includes('.')) {
        // If there is more than one dot after the first one, remove the extra dots
        cleanValue =
          cleanValue.substring(0, dotIndex + 1) + afterDot.replace(/\./g, '');
      }
    }
    if (cleanValue.toString().includes('.')) {
      var decimalPart = cleanValue.toString().split('.')[1];

      if (decimalPart.length > 2) {
        decimalPart = decimalPart.substring(0, 2);
        cleanValue = parseFloat(
          cleanValue.toString().split('.')[0] + '.' + decimalPart
        );
      }
    }

    // Update the form control value with the cleaned numeric value
    const formControlName =
      event.currentTarget.attributes.formcontrolname.nodeValue;

    const formValue = {};
    formValue[formControlName] = cleanValue;

    this.storePurchaseForm.patchValue(formValue, { emitEvent: false });
  }

  searchGridDemand(event: any): void {
    const tableElement = this.demand.nativeElement;
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

  //Calculations

  onRcvdInput(event: any) {
    this.onlyNumeric(event);
    this.getRejectedQty();
  }

  onRateInput(event: any) {
    this.onlyNumeric(event);
    this.getGrossAmt();
  }

  onCarraigeInput(event: any) {
    this.onlyNumeric(event);
    this.getNetAmt();
  }

  getRejectedQty(): void {
    const demandQty: AbstractControl | null =
      this.storePurchaseForm.get('demandQty');
    const rcvd: AbstractControl | null = this.storePurchaseForm.get('rcvd');
    const rejectedQty: AbstractControl | null =
      this.storePurchaseForm.get('rejectedQty');

    // Check if controls exist in the form group
    if (demandQty && rcvd && rejectedQty) {
      // Use the value property to get the numeric value
      const demandQtyValue: number = demandQty.value || 0;
      const rcvdValue: number = rcvd.value || 0;

      // Calculate and set rejectedQty value
      rejectedQty.setValue(demandQtyValue - rcvdValue);
    }
  }

  getGrossAmt(): void {
    const rate: AbstractControl | null = this.storePurchaseForm.get('rate');
    const rcvd: AbstractControl | null = this.storePurchaseForm.get('rcvd');
    const grossAmount: AbstractControl | null =
      this.storePurchaseForm.get('grossAmount');

    // Check if controls exist in the form group
    if (rate && rcvd && grossAmount) {
      // Use the value property to get the numeric value
      const rates: number = rate.value || 0;
      const rcvdValue: number = rcvd.value || 0;

      // Calculate and set grossAmount value
      grossAmount.setValue(rates * rcvdValue);
    }

    this.getSaleTaxAmt();
    this.getNetAmt();
  }

  getSaleTaxAmt(): void {
    const taxAmount: AbstractControl | null =
      this.storePurchaseForm.get('taxAmount');
    const grossAmount: AbstractControl | null =
      this.storePurchaseForm.get('grossAmount');
    const taxRate: AbstractControl | null =
      this.storePurchaseForm.get('taxRate');

    // Check if controls exist in the form group
    if (taxRate && grossAmount && taxAmount) {
      // Use the value property to get the numeric value
      const taxRates: number = taxRate.value || 0;
      const grossAmounts: number = grossAmount.value || 0;

      // Calculate and set grossAmount value
      taxAmount.setValue(grossAmounts * 100 * taxRates);
    }
  }

  getNetAmt(): void {
    const netValueControl = this.storePurchaseForm.get(
      'netValue'
    ) as AbstractControl;
    const grossAmountControl = this.storePurchaseForm.get(
      'grossAmount'
    ) as AbstractControl;
    const taxRateControl = this.storePurchaseForm.get(
      'taxRate'
    ) as AbstractControl;
    const freightControl = this.storePurchaseForm.get(
      'freight'
    ) as AbstractControl;

    // Check if controls exist in the form group
    if (
      netValueControl &&
      grossAmountControl &&
      taxRateControl &&
      freightControl
    ) {
      // Use the value property to get the numeric value
      const taxRate = taxRateControl.value || 0;
      const grossAmount = grossAmountControl.value || 0;
      const freight = freightControl.value || 0;

      // Calculate and set netValue value
      netValueControl.setValue(
        parseFloat(grossAmount) + parseFloat(taxRate) + parseFloat(freight)
      );
    } else {
      console.error('One or more controls not found in the form group.');
    }
  }

}
