import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-sale-booking-entry',
  templateUrl: './sale-booking-entry.component.html',
  styleUrls: ['./sale-booking-entry.component.css']
})
export class SaleBookingEntryComponent {

  constructor(
    private fb: FormBuilder,
    private tostr: ToastrService,
    private apiService: ApiService,
    private dp: DatePipe
  ) {
    const today = new Date();
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  }

  //List Page
  @ViewChild('voucherLists', { static: false }) voucherLists!: ElementRef;
  voucherList: any[] = [];
  fromDate: Date;
  toDate: Date;

  //Form Page
  saleBookingForm!: FormGroup;
  saleBookingList: any = [];
  isShow = false;
  isReadOnly: boolean = true;
  editMode: boolean = true;
  isShowPage: boolean = true;
  isDisabled: boolean = true;

  // Drop Down Item
  cropYear: any = [];

  // Drop Down Broker
  brokerList: any = [];

  // Drop Down Party
  partyMainList: any = [];
  partySubList: any = [];

  // Drop Down Item
  itemMainList: any = [];
  itemSubList: any = [];

  // Drop Down UOM
  UOMList: any = [];
  
  // Below Grid
  editModeSno: boolean = false;
  btnAdd: string = 'Add';
  detailGridIndex: any = '';

  ngOnInit() {
    this.getSBList();
    this.formInit();
    this.getVchNo();
    this.getCropYear();
    this.getBroker();
    this.getPartyMain();
    this.getItemMain();
    this.getUOM();
  }

  formInit() {
    this.saleBookingForm = this.fb.group({
      voucherType: ['Booking'],
      vchNo: [''],
      date: [new Date()],
      cropYear: [undefined],
      deliveryTerm: [undefined],
      paymentTerm: [undefined],
      invoiceType: [undefined],
      broker: [undefined],
      partyMain: [undefined],
      partySub: [undefined],
      itemMain: [undefined],
      itemSub: [undefined],
      qty: [''],
      rate: [''],
      brokerComm: [''],
      brokerUOM: [undefined],
      rateUOM: [undefined],
      remarks: [''],
    });
  }

  resetForm() {
    this.saleBookingForm.get('cropYear')?.patchValue(undefined);
    this.saleBookingForm.get('deliveryTerm')?.patchValue(undefined);
    this.saleBookingForm.get('paymentTerm')?.patchValue(undefined);
    this.saleBookingForm.get('invoiceType')?.patchValue(undefined);
    this.saleBookingForm.get('broker')?.patchValue(undefined);
    this.saleBookingForm.get('partyMain')?.patchValue(undefined);
    this.saleBookingForm.get('partySub')?.patchValue(undefined);
    this.saleBookingForm.get('itemMain')?.patchValue(undefined);
    this.saleBookingForm.get('itemSub')?.patchValue(undefined);
    this.saleBookingForm.get('qty')?.patchValue('');
    this.saleBookingForm.get('rate')?.patchValue('');
    this.saleBookingForm.get('brokerComm')?.patchValue('');
    this.saleBookingForm.get('brokerUOM')?.patchValue(undefined);
    this.saleBookingForm.get('rateUOM')?.patchValue(undefined);
    this.saleBookingForm.get('remarks')?.patchValue('');
    this.partySubList = [];
    this.itemSubList = [];
  }

  getSBList() {
    const obj = {
      fromDate: this.dp.transform(this.fromDate, 'yyyy/MM/dd'),
      toDate: this.dp.transform(this.toDate, 'yyyy/MM/dd'),
    };

    this.apiService
      .getDataById('Sale/GetSaleBookingList', obj)
      .subscribe((data) => {
        this.voucherList = data;
      });
  }

  getVchNo() {
    this.apiService
      .getData('Sale/GetSaleBookingVchNo')
      .subscribe((data) => {
        this.saleBookingForm.get('vchNo')?.setValue(data[0].VCHNO);
        this.saleBookingForm.get('date')?.patchValue(new Date());
      });
  }

  getCropYear() {
    this.apiService.getData('Common/GetCropYear').subscribe((data) => {
      this.cropYear = data;
    });
  }

  getBroker() {
    this.apiService.getDataById('Common/GetDirectL4L5CodeNamesByL4Tag', {l4Tag:'D'}).subscribe((data) => {
      this.brokerList = data;
    });
  }

  getPartyMain() {

    this.apiService.getDataById('Common/GetLevel4CodeNameByTag', {l4Tag:'D'}).subscribe((data) => {
      this.partyMainList = data;
    });

  }

  getPartySub(event: any) {
    this.saleBookingForm.get('partySub')?.patchValue(undefined);
    this.partySubList = [];
    this.apiService
      .getDataById('Common/GetLevel5CodeNameByL4Code', { code: event.CODE })
      .subscribe((data) => {
        this.partySubList = data;
      });
  }

  onPartyMainClear() {
    this.saleBookingForm.get('partyMain')?.patchValue(undefined);
    this.saleBookingForm.get('partySub')?.patchValue(undefined);
    this.partySubList = [];
  }
  
  getItemMain() {

    this.apiService.getDataById('Common/GetLevel4CodeNameByTag', {l4Tag:'S'}).subscribe((data) => {
      this.itemMainList = data;
    });

  }

  getItemSub(event: any) {
    this.saleBookingForm.get('itemSub')?.patchValue(undefined);
    this.itemSubList = [];

    this.apiService
      .getDataById('Common/GetLevel5CodeNameByL4Code', { code: event.CODE })
      .subscribe((data) => {
        this.itemSubList = data;
      });
  }

  onItemMainClear() {
    this.saleBookingForm.get('itemMain')?.patchValue(undefined);
    this.saleBookingForm.get('itemSub')?.patchValue(undefined);
    this.itemSubList = [];
  }


  getUOM() {
    this.apiService.getData('Common/GetUom').subscribe((data) => {
      this.UOMList = data;
    });
  }



  onAdd() {
    let form = this.saleBookingForm.value;

    if (form.date === null) {
      this.tostr.warning('Select Vch Date....!');
      return;
    }

    if (form.cropYear === null || form.cropYear === undefined) {
      this.tostr.warning('Select Crop Year....!');
      return;
    }

    if (form.deliveryTerm === null || form.deliveryTerm === undefined) {
      this.tostr.warning('Select Delivery Term....!');
      return;
    }

    if (form.paymentTerm === null || form.paymentTerm === undefined) {
      this.tostr.warning('Select Payment Term....!');
      return;
    }

    if (form.invoiceType === null || form.invoiceType === undefined) {
      this.tostr.warning('Select Invoice Type....!');
      return;
    }

    if (form.broker === null || form.broker === undefined) {
      this.tostr.warning('Select Broker....!');
      return;
    }

    if (form.partyMain === null || form.partyMain === undefined) {
      this.tostr.warning('Select Pary Main....!');
      return;
    }

    if (form.partySub === null || form.partySub === undefined) {
      this.tostr.warning('Select Pary Sub....!');
      return;
    }

    if (form.itemMain === null || form.itemMain === undefined) {
      this.tostr.warning('Select Item Main....!');
      return;
    }

    if (form.itemSub === null || form.itemSub === undefined) {
      this.tostr.warning('Select Item Sub....!');
      return;
    }

    if (form.qty === '' || form.qty === null || form.qty == 0) {
      this.tostr.warning('Enter Quantity....!');
      return;
    }

    if (form.rate === '' || form.rate === null || form.rate == 0) {
      this.tostr.warning('Enter Rate....!');
      return;
    }

    if (form.brokerComm === '' || form.brokerComm === null || form.brokerComm == 0) {
      this.tostr.warning('Enter Broker Commission....!');
      return;
    }

    if (form.brokerUOM === null || form.brokerUOM === undefined) {
      this.tostr.warning('Select Broker UOM....!');
      return;
    }

    if (form.rateUOM === null || form.rateUOM === undefined) {
      this.tostr.warning('Select Rate UOM....!');
      return;
    }

    form.itemTitle = this.itemSubList.find(
      (i) => i.CODE === form.itemSub
    ).NAME;

    //Get Amount
    const qty = form.qty;
    const rate = form.rate;
    form.amount = (parseFloat(qty)*parseFloat(rate));

    if (this.editModeSno) {
      if (this.detailGridIndex !== -1) {
        this.saleBookingList[this.detailGridIndex] = form;
        this.editModeSno = false;
        this.btnAdd = 'Add';
        return;
      }
    }

    this.saleBookingList.push(form);
  }

  editDetails(row: any, i: any) {
    this.btnAdd = 'Update';
    this.detailGridIndex = i;
    this.editModeSno = true;

    this.saleBookingForm.get('itemMain')?.patchValue(row.itemMain);
    this.getItemSub({ CODE: row.itemMain });
    this.saleBookingForm.get('itemSub')?.patchValue(row.itemSub);

    this.saleBookingForm.get('qty')?.patchValue(row.qty);
    this.saleBookingForm.get('rate')?.patchValue(row.rate);
    this.saleBookingForm.get('cropYear')?.patchValue(row.cropYear);
  }

  deleteDetails(index: any) {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (!confirmDelete) {
      return;
    }

    if (index !== -1) {
      this.saleBookingList.splice(index, 1);
    }
  }

  onClickSave() {
    if (this.saleBookingList.length == 0) {
      this.tostr.warning('Incomplete Transaction...');
      return;
    }

    let vchNo = this.editMode
      ? this.saleBookingForm.get('vchNo')?.value
      : 0;

    // let vchNo = 0
    // if(this.editMode == true){vchNo = this.requisitionForm.get('vchNo')?.value}
    // else {vchNo = 0}

    const voucher: any[] = this.saleBookingList.map((data) => ({
      
      vchType: this.saleBookingForm.get('voucherType').value,
      vchNo: vchNo,
      date: this.dp.transform(
        this.saleBookingForm.get('date')?.value,
        'yyyy-MM-dd'
      ),

      cropYear : data.cropYear,
      deliveryTerm : this.saleBookingForm.get('deliveryTerm').value,
      paymentTerm : this.saleBookingForm.get('paymentTerm').value,
      invoiceType : this.saleBookingForm.get('invoiceType').value,
      broker : this.saleBookingForm.get('broker').value,
      party: this.saleBookingForm.get('partySub').value,
      item: data.itemSub,
      qty: parseFloat(data.qty),
      rate: parseFloat(data.rate),
      amount: parseFloat(data.amount),
      brokerComm: this.saleBookingForm.get('brokerComm').value,
      brokerUOM: this.saleBookingForm.get('brokerUOM').value,
      rateUom: this.saleBookingForm.get('rateUOM').value,
      remarks: this.saleBookingForm.get('remarks').value??"",
    }));

    this.apiService
      .saveData('Sale/SaveSaleBookingEntry', voucher)
      .subscribe((result) => {
        if (result == true || result == 'true') {
          this.tostr.success('Save Successfully');
          this.onClickRefresh();
          this.getSBList();
          this.getVchNo();
        } else {
          this.tostr.error('Please Save Again');
        }
      });
  }

  editSB(vchNo: any): void {
    this.onClickRefresh();
    this.isShow = true;
    this.editMode = true;
    this.isDisabled = false;

    const obj = {
      vchNo: vchNo,
    };

    this.apiService
      .getDataById('Sale/GetEditSaleBookingEntry', obj)
      .subscribe((data) => {
        this.togglePages();
        
        let fi = data[0];
        this.saleBookingForm.get('vchNo').patchValue(fi.VCHNO);
        this.saleBookingForm.get('deliveryTerm').patchValue(fi.DELIVERYTERM);
        this.saleBookingForm.get('paymentTerm').patchValue(fi.PAYMENTTERM);
        this.saleBookingForm.get('invoiceType').patchValue(fi.INVOICETYPE);
        this.saleBookingForm.get('broker').patchValue(fi.BROKERCODE);
        this.saleBookingForm.get('brokerComm').patchValue(fi.BROKERCOMM);
        this.saleBookingForm.get('brokerUOM').patchValue(fi.BROKERCOMMUOM);
        this.saleBookingForm.get('rateUOM').patchValue(fi.UOM);
        this.saleBookingForm.get('remarks').patchValue(fi.REMARKS);
        this.saleBookingForm.get('partyMain').patchValue(fi.PARTYCODE.substring(0, 9));
        this.getPartySub({ CODE: fi.PARTYCODE.substring(0, 9) });
        this.saleBookingForm.get('partySub').patchValue(fi.PARTYCODE);
        this.saleBookingForm.get('vchNo')?.patchValue(fi.VCHNO);

        data.forEach((item: any) => {
          this.saleBookingForm
            .get('date')
            ?.patchValue(
              new Date(
                item.VCHDATE.split('/')[2],
                item.VCHDATE.split('/')[1] - 1,
                item.VCHDATE.split('/')[0]
              )
            );
          //this.saleBookingForm.get('vchNo')?.patchValue(vchNo);

          let form = item;
          
          form.itemTitle = item.ITEMNAME
          form.partySub = item.PARYCODE;
          form.itemMain = item.ITEMCODE.substring(0, 9);
          form.itemSub = item.ITEMCODE;
          form.qty = item.QTY;
          form.rate = item.RATE;
          form.amount = item.QTY * item.RATE;
          form.cropYear = item.CROPYEAR;

          this.saleBookingList.push(form);
        });
      });
  }

  deleteSB(VCHNO: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {
      const obj = {
        vchNo: VCHNO,
      };

      this.apiService.deleteData('Sale/DelSaleBookingEntry', obj).subscribe({
        
        next: (data) => {
          if (data == 'true' || data == true) {
            this.tostr.success('Delete Successfully');
            this.getSBList();
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
    this.saleBookingList = [];
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

    this.saleBookingForm.patchValue(formValue, { emitEvent: false });
  }

}
