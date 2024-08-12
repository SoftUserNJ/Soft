import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';
declare const $: any;

@Component({
  selector: 'app-purchase-contract-form',
  templateUrl: './purchase-contract-form.component.html',
  styleUrls: ['./purchase-contract-form.component.css'],
})
export class PurchaseContractFormComponent {
  constructor(
    private fb: FormBuilder,
    private tostr: ToastrService,
    private apiService: ApiService,
    private dp: DatePipe,
    private auth: AuthService,
    private com: CommonService
  ) {
    const today = new Date();
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  }

  //List Page
  @ViewChild('voucherLists', { static: false }) voucherLists!: ElementRef;
  @ViewChild('detailsGrid', { static: false }) detailsGrid!: ElementRef;

  editDisabled: boolean = false;
  voucherList: any[] = [];
  fromDate: Date;
  toDate: Date;
  modalRef: BsModalRef;

  //Form Page
  PurchaseContractForm!: FormGroup;
  detailsList: any = [];
  isShow = false;
  isReadOnly: boolean = true;
  editMode: boolean = true;
  isShowPage: boolean = true;

  isDisabled: boolean;
  isPrint: boolean = false;

  locationList: any = [];
  UomList: any[] = [];
  itemsList: any[] = [];
  partyList: any[] = [];
  incomeTaxList: any[] = [];
  brokerList: any[] = [];
  cropYearList: any[] = [];

  // TERMS
  termsDays = '';
  termsId = 0;
  termsList: any[] = [];
  isDisabledTerms: boolean = true;
  isShowTerms: boolean = false;

  ngOnInit() {
    this.formInit();
    this.getVouchersList();
    this.disableFields();
    this.getUomList();
    this.getItemsList();
    this.getParty();
    this.getBroker();
    //this.getCropYear();
    this.getTerms();
    this.getLocation();
  }

  formInit() {
    this.PurchaseContractForm = this.fb.group({
      vchType: ['PO-Pur'],
      poCompletionDate: [new Date()],
      vchDate: [new Date()],
      vchNo: [''],
      location: [null],
      party: [null],
      broker: [null],
      incomeTax: [null],
      brokerCom: [''],
      brokerUom: [null],
      freightType: [null],
      bType: [null],
      bQty: [''],
      paymentDays: [null],
      invType: [null],
      remarks: [''],

      performaNo: [''],
      performaDate: [new Date()],
      deliveryDate: [new Date()],
      insurance: [''],
      coverDate: [new Date()],
      hsCode: [''],
      coreNoteDate: [new Date()],
    });
  }

  resetForm() {
    this.PurchaseContractForm.get('vchType').patchValue('PO-Pur');
    this.PurchaseContractForm.get('poCompletionDate').patchValue(new Date());
    this.PurchaseContractForm.get('vchDate').patchValue(new Date());
    this.PurchaseContractForm.get('vchNo').patchValue('');
    this.PurchaseContractForm.get('party').patchValue(null);
    this.PurchaseContractForm.get('broker').patchValue(null);
    this.PurchaseContractForm.get('incomeTax').patchValue(null);
    this.PurchaseContractForm.get('brokerCom').patchValue('');
    this.PurchaseContractForm.get('brokerUom').patchValue(null);
    this.PurchaseContractForm.get('freightType').patchValue(null);
    this.PurchaseContractForm.get('bType').patchValue(null);
    this.PurchaseContractForm.get('bQty').patchValue('');
    this.PurchaseContractForm.get('paymentDays').patchValue('30');
    this.PurchaseContractForm.get('invType').patchValue(null);
    this.PurchaseContractForm.get('remarks').patchValue('');
    this.PurchaseContractForm.get('performaNo').patchValue('');
    this.PurchaseContractForm.get('performaDate').patchValue(new Date());
    this.PurchaseContractForm.get('deliveryDate').patchValue(new Date());
    this.PurchaseContractForm.get('insurance').patchValue('');
    this.PurchaseContractForm.get('coverDate').patchValue(new Date());
    this.PurchaseContractForm.get('hsCode').patchValue('');
    this.PurchaseContractForm.get('coreNoteDate').patchValue(new Date());

    this.qtySum = 0;
    this.rateSum = 0;
    this.amountSum = 0;
    this.saleTaxSum = 0;
    this.taxAmountSum = 0;
    this.netAmountSum = 0;
    this.wht = 0;
    this.whtNetAmt = 0;
  }

  async getLocation() {
    const result = await this.apiService
      .getData('Common/LocationWithLoc')
      .toPromise();
      if(result.length > 0){
        this.locationList = result;
        this.PurchaseContractForm.get('location').setValue(result[0].LocID);
      }
  }


  getVchNo() {
    this.apiService
      .getData('Purchase/GetPurchaseContractVchNo')
      .subscribe((data) => {
        this.PurchaseContractForm.get('vchNo').setValue(data[0].VCHNO);
      });
  }

  getItemsList() {
    this.apiService
      .getDataById('Common/GetDirectL4L5CodeNamesByL4Tag', { l4Tag: 'S' })
      .subscribe((data) => {
        this.itemsList = data;
      });
  }

  onItemChange(data: any) {
    const item = this.itemsList.find((x) => x.L5CODE == data.items);
    data.sTax = item.SaleTax;
    data.uom = this.UomList.find((x) => x.ID == item.Uomid).UOM;
    data.rate = item.RATE;
    data.sTax = item.STAX;
    this.calculation(data);
  }

  getUomList() {
    this.apiService.getData('Common/GetUom').subscribe((data) => {
      this.UomList = data;
    });
  }

  getParty() {
    this.apiService
      .getDataById('Common/GetDirectL4L5CodeNamesByL4Tag', { l4Tag: 'C' })
      .subscribe((data) => {
        this.partyList = data;
      });
  }

  // onPartyChange(event: any) {
  //   const WHTAX = this.partyList.find((x) => x.L5CODE == event.L5CODE).WHTAX;
  //   this.incomeTaxList = [{ tax: WHTAX }];
  //   this.PurchaseContractForm.get('incomeTax').patchValue(WHTAX);
  // }

  getBroker() {
    // this.apiService.getDataById('Common/GetDirectL4L5CodeNamesByL4Tag', {l4Tag: 'BR'}).subscribe((data)=>{
    this.apiService
      .getDataById('Common/GetDirectL4L5CodeNamesByL4Tag', { l4Tag: 'D' })
      .subscribe((data) => {
        this.brokerList = data;
      });
  }

  getCropYear() {
    this.apiService.getData('Common/GetCropYear').subscribe((data) => {
      this.cropYearList = data;
    });
  }

  getVouchersList() {
    try {
      this.com.showLoader();

      const obj = {
        fromDate: this.dp.transform(this.fromDate, 'yyyy-MM-dd'),
        toDate: this.dp.transform(this.toDate, 'yyyy-MM-dd'),
      };

      this.apiService
        .getDataById('Purchase/GetPurchaseContractList', obj)
        .subscribe((data) => {
          this.voucherList = data;

          this.com.hideLoader();
        });
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  onAdd() {
    let form = {
      items: null,
      category: null,
      uom: null,
      cropYear: null,
      vehicles: '',
      qty: '',
      rate: '',
      amount: '',
      sTax: '',
      sTaxAmount: '',
      netAmount: '',
    };
    this.detailsList.push(form);
  }

  deleteItem(index: any) {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (!confirmDelete) {
      return;
    }

    if (index !== -1) {
      this.detailsList.splice(index, 1);
    }
  }

  onClickSave() {
    const form = this.PurchaseContractForm.value;

    if (form.party == null) {
      this.tostr.warning('Select Party....!');
      return;
    }

    // if(form.incomeTax == null){
    //   this.tostr.warning('Select Income Tax....!');
    //   return;
    // }

    if (parseFloat(form.brokerCom) > 0 && form.brokerUom == null) {
      this.tostr.warning('Select Broker UOM....!');
      return;
    }

    if (form.freightType == null) {
      this.tostr.warning('Select Freight Type....!');
      return;
    }

    if (form.bType == null) {
      this.tostr.warning('Select B.Type....!');
      return;
    }

    if (form.invType == null) {
      this.tostr.warning('Select Invoice Type....!');
      return;
    }

    let vchNo = this.editMode
      ? this.PurchaseContractForm.get('vchNo')?.value
      : 0;

    const voucher: any[] = this.detailsList.map((data) => ({
      item: data.items ?? '',
      category: data.category ?? '',
      qty: parseFloat(data.qty),
      uom: data.uom ?? '',
      rate: parseFloat(data.rate),
      saleTax: isNaN(parseFloat(data.sTax)) ? 0 : parseFloat(data.sTax),
      amount: data.amount > 0 ? data.amount : 0,
      cropYear: data.cropYear ?? '',
      vehicle: data.vehicles > 0 ? data.vehicles : 0,

      vchNo: vchNo,
      date: this.dp.transform(form.vchDate, 'yyyy-MM-dd'),
      poCompletionDate: this.dp.transform(form.poCompletionDate, 'yyyy-MM-dd'),
      party: form.party,
      broker: form.broker ?? '',
      incomeTax: isNaN(parseFloat(form.incomeTax))
        ? 0
        : parseFloat(form.incomeTax),
      brockerCom: form.brokerCom > 0 ? form.brokerCom : 0,
      brockerUom: form.brokerUom ?? '',
      freightType: form.freightType ?? '',
      bType: form.bType ?? '',
      bQty: isNaN(parseFloat(form.bQty)) ? 0 : parseFloat(form.bQty),
      paymentDays: isNaN(parseInt(form.paymentDays))
        ? 0
        : parseInt(form.paymentDays),
      invType: form.invType ?? '',
      remarks: form.remarks ?? '',
      performaNo: form.performaNo ?? '',
      performaDate: this.dp.transform(form.performaDate, 'yyyy-MM-dd'),
      deliveryDate: this.dp.transform(form.deliveryDate, 'yyyy-MM-dd'),
      insurance: form.insurance ?? '',
      coverDate: this.dp.transform(form.coverDate, 'yyyy-MM-dd'),
      hsCode: form.hsCode ?? '',
      coreNoteDate: this.dp.transform(form.coreNoteDate, 'yyyy-MM-dd'),
    }));

    voucher.forEach((item) => {
      if (!item.item || item.item === null || item.item === '') {
        this.tostr.warning('Select Item....!');
        return;
      }
      if (!item.uom == null || item.uom === null || item.uom === '') {
        this.tostr.warning('Select UOM....!');
        return;
      }
      if (
        !item.qty ||
        isNaN(item.qty) ||
        item.qty === null ||
        item.qty === ''
      ) {
        this.tostr.warning('Enter Qty....!');
        return;
      }
      if (
        !item.rate ||
        isNaN(item.rate) ||
        item.rate === null ||
        item.rate === ''
      ) {
        this.tostr.warning('Enter Rate....!');
        return;
      }
    });
    try {
      this.com.showLoader();

      this.apiService
        .saveData('Purchase/SavePurchaseContract', voucher)
        .subscribe((r) => {
          if (r.status == true || r.status == 'true') {
            this.com.hideLoader();
            this.tostr.success('Save Successfully');
            this.isPrint = true;
            this.editMode = true;
            this.PurchaseContractForm.get('vchNo').patchValue(r.vchno);
            //this.onClickRefresh();
            //this.getVchNo();
            //this.getVouchersList();
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

  edit(vchNo: any): void {
    this.onClickRefresh();
    this.isShow = true;
    this.editMode = true;

    try {
      this.com.showLoader();

      const obj = {
        vchNo: vchNo,
      };

      this.apiService
        .getDataById('Purchase/GetEditPurchaseContract', obj)
        .subscribe((data) => {
          console.log(data);
          this.togglePages();
          this.enableFields();

          const d = data[0];

          if (d.Aprove == 0 || d.Aprove == false || d.Aprove == 'false') {
            this.editDisabled = false;
          }

          if (d.Aprove == 1 || d.Aprove == true || d.Aprove == 'true') {
            this.editDisabled = true;
          }

          const v = this.voucherList.find((x) => x.VCHNO == vchNo);

          this.PurchaseContractForm.get('performaNo').patchValue(v.PERFORMANO);
          this.PurchaseContractForm.get('performaDate')?.patchValue(
            new Date(
              v.PERFORMADATE.split('/')[2],
              v.PERFORMADATE.split('/')[1] - 1,
              v.PERFORMADATE.split('/')[0]
            )
          );

          this.PurchaseContractForm.get('deliveryDate')?.patchValue(
            new Date(
              v.DELIVERYDATE.split('/')[2],
              v.DELIVERYDATE.split('/')[1] - 1,
              v.DELIVERYDATE.split('/')[0]
            )
          );

          this.PurchaseContractForm.get('insurance').patchValue(v.INSURANCE);

          this.PurchaseContractForm.get('coverDate')?.patchValue(
            new Date(
              v.COVERDATE.split('/')[2],
              v.COVERDATE.split('/')[1] - 1,
              v.COVERDATE.split('/')[0]
            )
          );

          this.PurchaseContractForm.get('hsCode')?.patchValue(v.HSCODE);

          this.PurchaseContractForm.get('coreNoteDate')?.patchValue(
            new Date(
              v.CORENOTEDATE.split('/')[2],
              v.CORENOTEDATE.split('/')[1] - 1,
              v.CORENOTEDATE.split('/')[0]
            )
          );

          this.PurchaseContractForm.get('vchNo')?.patchValue(d.vchNo);
          this.PurchaseContractForm.get('party')?.patchValue(d.party);
          this.PurchaseContractForm.get('broker')?.patchValue(d.broker);
          this.PurchaseContractForm.get('brokerCom')?.patchValue(d.brockerCom);
          this.PurchaseContractForm.get('brokerUom')?.patchValue(
            d.brockerUom.trim()
          );
          this.PurchaseContractForm.get('incomeTax')?.patchValue(
            d.incomeTax == 0 ? null : d.incomeTax
          );
          this.PurchaseContractForm.get('freightType')?.patchValue(
            d.freightType
          );
          this.PurchaseContractForm.get('bType')?.patchValue(d.bType);
          this.PurchaseContractForm.get('bQty')?.patchValue(d.bQty);
          this.PurchaseContractForm.get('paymentDays')?.patchValue(
            d.paymentDays.toString()
          );

          this.PurchaseContractForm.get('invType')?.patchValue(d.invType);
          this.PurchaseContractForm.get('remarks')?.patchValue(d.remarks);

          this.PurchaseContractForm.get('vchDate')?.patchValue(
            new Date(
              d.vchDate.split('/')[2],
              d.vchDate.split('/')[1] - 1,
              d.vchDate.split('/')[0]
            )
          );

          this.PurchaseContractForm.get('poCompletionDate')?.patchValue(
            new Date(
              d.poCompDate.split('/')[2],
              d.poCompDate.split('/')[1] - 1,
              d.poCompDate.split('/')[0]
            )
          );

         // this.onPartyChange({ L5CODE: d.party });

          data.forEach((item: any) => {
            let form: any = [];

            form.items = item.item;
            form.category = item.category;
            form.qty = item.qty;
            form.uom = item.uom;
            form.rate = item.rate;
            form.sTax = item.saleTax;
            form.amount = item.amount;
            form.cropYear = item.cropYear;
            form.vehicles = item.vehicles;

            this.calculation(form);
            this.detailsList.push(form);
          });

          this.com.hideLoader();
          this.isPrint = true;
        });
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  delete(vchNo: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {
      try {
        this.com.showLoader();

        const obj = {
          vchNo: vchNo,
        };

        this.apiService
          .deleteData('Purchase/DelPurchaseContract', obj)
          .subscribe({
            next: (data) => {
              this.com.hideLoader();

              if (data == 'true' || data == true) {
                this.tostr.success('Delete Successfully');
                this.getVouchersList();
              } else if (data == 'false' || data == false) {
                this.tostr.error('Delete Again');
              }

              this.com.hideLoader();
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
  }

  onClickNew() {
    this.isShow = true;
    this.editMode = false;
    this.getVchNo();
    this.enableFields();
  }

  onClickRefresh() {
    this.isShow = false;
    this.isPrint = false;
    this.detailsList = [];
    this.editMode = false;
    this.resetForm();
    this.disableFields();
  }

  enableFields() {
    this.isDisabled = false;
    this.PurchaseContractForm.get('vchDate').enable();
    this.PurchaseContractForm.get('performaDate').enable();
    this.PurchaseContractForm.get('deliveryDate').enable();
    this.PurchaseContractForm.get('coverDate').enable();
    this.PurchaseContractForm.get('coreNoteDate').enable();
  }

  disableFields() {
    this.isDisabled = true;
    this.PurchaseContractForm.get('vchDate').disable();
    this.PurchaseContractForm.get('performaDate').disable();
    this.PurchaseContractForm.get('deliveryDate').disable();
    this.PurchaseContractForm.get('coverDate').disable();
    this.PurchaseContractForm.get('coreNoteDate').disable();
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
    const cleanValue = inputVal.replace(/[^0-9]/g, '');
    // Update the form control value with the cleaned numeric value

    const formControlName =
      event.currentTarget.attributes.formcontrolname.nodeValue;

    const formValue = {};
    formValue[formControlName] = cleanValue;

    this.PurchaseContractForm.patchValue(formValue, { emitEvent: false });
  }

  qtySum: number = 0;
  rateSum: number = 0;
  amountSum: number = 0;
  saleTaxSum: number = 0;
  taxAmountSum: number = 0;
  netAmountSum: number = 0;
  wht: number = 0;
  whtNetAmt: number = 0;

  calculation(data: any) {
    if (!data.uom || data.uom == null || data.uom === null || data.uom === '') {
      this.tostr.warning('Select Item Uom....!');
      return;
    }

    const divUom = this.UomList.find((x) => x.UOM == data.uom).DIVUOM;

    const qtyInput = data.qty ?? 0;
    const rateInput = data.rate ?? 0;
    const sTaxInput = data.sTax ?? 0;

    const qty = parseFloat(qtyInput);
    const rate = parseFloat(rateInput);
    const sTax = parseFloat(sTaxInput);

    const amountN = qty * (rate / divUom);
    data.amount = amountN;

    const sTaxAmountM = (amountN / 100) * sTax;

    data.sTaxAmount = this.com.roundVal(sTaxAmountM);

    data.netAmount = this.com.roundVal(amountN + sTaxAmountM);

    setTimeout(() => {
      this.total(); // Call total() after 20 seconds
    }, 100);

    //this.total();
  }

  total() {
    const rows = this.detailsGrid.nativeElement.querySelectorAll('tr');

    let qtySum = 0;
    let rateSum = 0;
    let amountSum = 0;
    let saleTaxSum = 0;
    let taxAmountSum = 0;
    let netAmountSum = 0;

    rows.forEach((row: HTMLTableRowElement) => {
      const qtyInput = row.querySelector('.qty') as HTMLInputElement;
      const rateInput = row.querySelector('.rate') as HTMLInputElement;
      const amountInput = row.querySelector('.amount') as HTMLInputElement;
      const saleTaxInput = row.querySelector('.sTax') as HTMLInputElement;
      const taxAmountInput = row.querySelector(
        '.sTaxAmount'
      ) as HTMLInputElement;
      const netAmountInput = row.querySelector(
        '.netAmount'
      ) as HTMLInputElement;

      // Add NaN check before parsing and summing
      if (qtyInput && !isNaN(parseFloat(qtyInput.value.replace(/,/g, '')))) {
        qtySum += parseFloat(qtyInput.value.replace(/,/g, ''));
      }
      if (rateInput && !isNaN(parseFloat(rateInput.value.replace(/,/g, '')))) {
        rateSum += parseFloat(rateInput.value.replace(/,/g, ''));
      }
      if (
        amountInput &&
        !isNaN(parseFloat(amountInput.value.replace(/,/g, '')))
      ) {
        amountSum += parseFloat(amountInput.value.replace(/,/g, ''));
      }
      if (
        saleTaxInput &&
        !isNaN(parseFloat(saleTaxInput.value.replace(/,/g, '')))
      ) {
        saleTaxSum += parseFloat(saleTaxInput.value.replace(/,/g, ''));
      }
      if (
        taxAmountInput &&
        !isNaN(parseFloat(taxAmountInput.value.replace(/,/g, '')))
      ) {
        taxAmountSum += parseFloat(taxAmountInput.value.replace(/,/g, ''));
      }
      if (
        netAmountInput &&
        !isNaN(parseFloat(netAmountInput.value.replace(/,/g, '')))
      ) {
        netAmountSum += parseFloat(netAmountInput.value.replace(/,/g, ''));
      }
    });

    this.qtySum = qtySum;
    this.rateSum = rateSum;
    this.amountSum = amountSum;
    this.saleTaxSum = saleTaxSum;
    this.taxAmountSum = parseFloat(taxAmountSum.toFixed(2));
    this.netAmountSum = parseFloat(netAmountSum.toFixed(2));

    let wht = this.PurchaseContractForm.get('incomeTax').value ?? 0;
    let whtN = parseFloat(((netAmountSum / 100) * wht).toFixed(2));

    this.wht = this.com.roundVal(whtN);
    this.whtNetAmt = this.com.roundVal(
      parseFloat((whtN + netAmountSum).toFixed(2))
    );
  }

  onViewReport(event: any) {
    const url = `PrintPurContract?VchNo=${event.VCHNO}&VchType=${
      event.VCHTYPE
    }&CmpId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}&CmpName=${this.auth.cmpName()}&CmpAdr=${this.auth.cmpAdr()}`;
    this.com.viewReport(url);
  }

  onClickPrint() {
    let vchType = this.PurchaseContractForm.get('vchType').value;
    let vchNo = this.PurchaseContractForm.get('vchNo').value;

    const url = `PrintPurContract?VchNo=${vchNo}&VchType=${vchType}&CmpId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}&CmpName=${this.auth.cmpName()}&CmpAdr=${this.auth.cmpAdr()}`;
    this.com.viewReport(url);
  }

  //======================= TERMS =====================//

  getTerms() {
    this.apiService.getData('Purchase/GetTerms').subscribe((data) => {
      this.termsList = data;
    });
  }

  createUpdateTerms() {
    const obj = {
      id: this.termsId,
      name: this.termsDays,
      dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
    };

    if (this.termsDays == '' || this.termsDays == null) {
      this.tostr.warning('Enter Days....!');
      return;
    }

    this.apiService
      .saveObj('Purchase/AddUpdateTerms', obj)
      .subscribe((result) => {
        if (result == true || result == 'true') {
          this.getTerms();
          this.refreshTerms();
          this.tostr.success('Save Successfully');
        } else {
          this.tostr.error('Please Save Again');
        }
      });
  }

  editTerms(id: any, name: any, alias: any): void {
    this.termsDays = name;
    this.termsId = id;
    this.isDisabledTerms = false;
    this.isShowTerms = true;
  }

  deleteTerms(id: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {
      const obj = {
        id: id,
        dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      };

      this.apiService.deleteData('Purchase/DeleteTerms', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.getTerms();
            this.refreshTerms();
            this.tostr.success('Delete Successfully');
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

  refreshTerms() {
    this.termsDays = '';
    this.termsId = 0;
    this.isDisabledTerms = true;
    this.isShowTerms = false;
  }

  newTerms() {
    this.refreshTerms();
    this.isDisabledTerms = false;
    this.isShowTerms = true;
  }

  onViewTestReport(event: any) {
    const url = `ReceivingOfGoods?VchType=RP-RAW&VchNo=4&FinId=1&LocId=U1&FromDate=2024-05-21 11:51:36.000&VehNo=LEL-002&GpNo=4&CmpId=2&Logo=https://localhost:7283/Companies/Sultania%20Poultry%20Farms/CompanyLogo/2.jpeg`;
    this.com.viewReport(url);
  }


}
