import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';
declare const $: any;

@Component({
  selector: 'app-purchase-gate-pass-inward-entry',
  templateUrl: './purchase-gate-pass-inward-entry.component.html',
  styleUrls: ['./purchase-gate-pass-inward-entry.component.css'],
})
export class PurchaseGatePassInwardEntryComponent {
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
  @ViewChild('vehicles') vehicles: ElementRef;

  voucherList: any[] = [];
  fromDate: Date;
  toDate: Date;
  vchType: any = 'RP-RAW';
  grnNo: number = 0;
  showLoader: boolean = false;
  editDisabled: boolean = false;

  vchTypes: any = [];
  locationList: any = [];
  freightList: any = [];
  minimumWeightList: any = [];
  godownList: any = [];
  uomList: any = [];
  partyMainList: any = [];
  partySubList: any = [];
  supPartyList: any = [];

  itemMainList: any = [];
  itemSubList: any = [];
  poDetailsList: any = [];
  detailsList: any = [];

  isShow = false;
  isShowPage: boolean = true;
  InwardEntryForm!: FormGroup;
  editModeSno: boolean = false;
  editMode: boolean = true;
  readonly: boolean = true;

  btnAdd: string = 'Add';
  editSno: any = '';

  ngOnInit() {
    this.showLoader = true;
    this.formInit();
    this.getVchNo();
    this.getVchType();
    this.getVouchersList();
    this.getPartyMain();
    this.getItemMain();
    this.getLocation();
    this.getUOM();
    this.getGodowns();
  }

  formInit() {
    this.InwardEntryForm = this.fb.group({
      vchType: ['RP-RAW'],
      vchNo: [''],
      location: [this.auth.locId()],
      vehicleNo: [''],
      biltyNo: [''],
      grnDate: [new Date()],
      freightDD: ['S'],
      freight: [''],
     
      chkMinWeight: [{ value: true}],
      minWeight: ['O'],
      partyMain: [null],
      partySub: [null],
      subParty: [null],
      itemMain: [null],
      itemSub: [null],
      godown: [null],
      uom: [null],
      bagsType: ['W'],
      bags: [''],
      gross: [''],
      tare: [''],
      bagsWt: [''],
      expWt: [''],
      standardWt: [''],
      retStat: ['In-Stock'],
      sTax: [''],
      iTax: [''],
      net: [''],
      remarks: [''],

      manualGpiNo: [''],
      driverName: [''],
      contact: [''],
      tCNIC: [''],
      transitPermit: [''],
      sludge: [''],
      color: [''],
      wbCharge: [''],
      areaMain: [''],
      areaSub: [''],
      rate: [''],

      poNo: ['0'],
      poBal: [''],
    });
  }

  resetForm() {
    this.readonly = true;

    this.InwardEntryForm.get('vchType')?.patchValue('RP-RAW');
    this.InwardEntryForm.get('vchNo')?.patchValue('');
    this.InwardEntryForm.get('location')?.patchValue(this.auth.locId());
    this.InwardEntryForm.get('vehicleNo')?.patchValue('');
    this.InwardEntryForm.get('biltyNo')?.patchValue('');
    this.InwardEntryForm.get('grnDate')?.patchValue(new Date());
    this.InwardEntryForm.get('freightDD')?.patchValue('S');
    this.InwardEntryForm.get('freight')?.patchValue('');
    this.InwardEntryForm.get('chkMinWeight')?.patchValue(false);
    this.InwardEntryForm.get('minWeight')?.patchValue('O');
    this.InwardEntryForm.get('partyMain')?.patchValue(null);
    this.InwardEntryForm.get('partySub')?.patchValue(null);
    this.InwardEntryForm.get('subParty')?.patchValue(null);
    this.InwardEntryForm.get('itemMain')?.patchValue(null);
    this.InwardEntryForm.get('itemSub')?.patchValue(null);
    this.InwardEntryForm.get('godown')?.patchValue(null);
    this.InwardEntryForm.get('uom')?.patchValue(null);
    this.InwardEntryForm.get('bagsType')?.patchValue('W');
    this.InwardEntryForm.get('bags')?.patchValue('');
    this.InwardEntryForm.get('gross')?.patchValue('');
    this.InwardEntryForm.get('tare')?.patchValue('');
    this.InwardEntryForm.get('bagsWt')?.patchValue('');
    this.InwardEntryForm.get('expWt')?.patchValue('');
    this.InwardEntryForm.get('standardWt')?.patchValue('');
    this.InwardEntryForm.get('retStat')?.patchValue('In-Stock');
    this.InwardEntryForm.get('sTax')?.patchValue('');
    this.InwardEntryForm.get('iTax')?.patchValue('');
    this.InwardEntryForm.get('net')?.patchValue('');
    this.InwardEntryForm.get('remarks')?.patchValue('');
    this.InwardEntryForm.get('manualGpiNo')?.patchValue('');
    this.InwardEntryForm.get('driverName')?.patchValue('');
    this.InwardEntryForm.get('contact')?.patchValue('');
    this.InwardEntryForm.get('tCNIC')?.patchValue('');
    this.InwardEntryForm.get('transitPermit')?.patchValue('');
    this.InwardEntryForm.get('sludge')?.patchValue('');
    this.InwardEntryForm.get('color')?.patchValue('');
    this.InwardEntryForm.get('wbCharge')?.patchValue('');
    this.InwardEntryForm.get('areaMain')?.patchValue('');
    this.InwardEntryForm.get('areaSub')?.patchValue('');
    this.InwardEntryForm.get('rate')?.patchValue('');
    this.InwardEntryForm.get('poNo')?.patchValue('0');
    this.InwardEntryForm.get('poBal')?.patchValue('');

    this.itemSubList = [];
    this.partySubList = [];
    this.poDetailsList = [];

    this.InwardEntryForm.get('grnDate').disable();
  }

  async getPartyMain() {
    const result = await this.apiService
      .getDataById('Common/GetLevel4CodeNameByTag', { tag: 'C' })
      .toPromise();
      if(result.length > 0){
        this.partyMainList = result;
      }
  }

  async getPartySub() {
    let code = this.InwardEntryForm.get('partyMain')?.value;
    const result = await this.apiService
      .getDataById('Common/GetLevel5CodeNameByL4Code', { code: code })
      .toPromise();
    this.partySubList = result;
  }

  onPartyClear() {
    this.partySubList = [];
    this.InwardEntryForm.get('partySub')?.patchValue(null);
  }

  async getTblSubParty(event: any) {
    const result = await this.apiService
      .getDataById('Sale/GetSubParty', { code: event.CODE })
      .toPromise();

    this.supPartyList = result;
  }

  async getItemMain() {
    const result = await this.apiService
      .getDataById('Common/GetLevel4CodeNameByTag', { tag: 'S' })
      .toPromise();
      if(result.length > 0){
        this.itemMainList = result;
      }
  }

  async getItemSub() {
    let code =  this.InwardEntryForm.get('itemMain')?.value;
    const result = await this.apiService
      .getDataById('Common/GetLevel5CodeNameByL4Code', { code: code })
      .toPromise();

    this.itemSubList = result;
  }

  onItemClear() {
    this.itemSubList = [];
    this.InwardEntryForm.get('itemSub')?.patchValue(null);
  }

  async getLocation() {
    const result = await this.apiService
      .getData('Common/GetLocation')
      .toPromise();
    this.locationList = result;
  }

  async getUOM() {
    const result = await this.apiService
      .getData('Inventory/GetUom')
      .toPromise();
    this.uomList = result;
  }

  async getGodowns() {
    const result = await this.apiService
      .getData('Common/GetGodowns')
      .toPromise();

      if(result.length > 0){
        this.godownList = result;
      }
  }

  async getPoDetails() {
    const party = this.InwardEntryForm.get('partySub').value;
    const item = this.InwardEntryForm.get('itemSub').value;

    if (party == '' || party == null) {
      this.tostr.warning('Select Party....!');
      return;
    }

    if (item == '' || item == null) {
      this.tostr.warning('Select Items....!');
      return;
    }
    try {
      this.com.showLoader();

      const obj = {
        party: party,
        item: item,
        TransDate : this.dp.transform(
          this.InwardEntryForm.get('grnDate')?.value,
          'yyyy-MM-dd'
        ),
        Vchno :0,
        Pono:0,
      };

      const result = await this.apiService
        .getDataById('Common/GetPoDetailsByPartyAndItems', obj)
        .toPromise();
      this.com.hideLoader();

      $('#poDetailsModal').modal('show');

      const r = result[0];
      this.poDetailsList = result;
      // this.InwardEntryForm.get('freightDD').patchValue(r.FreightType);
      // this.InwardEntryForm.get('poNo').patchValue(r.PoNo);
      // this.InwardEntryForm.get('poBal').patchValue(r.BalQty);
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  selectPo(r: any) {
    this.InwardEntryForm.get('freightDD').patchValue(r.FreightType);
    this.InwardEntryForm.get('poNo').patchValue(r.PoNo);
    this.InwardEntryForm.get('poBal').patchValue(r.BalQty);
    $('#poDetailsModal').modal('hide');
  }

  async getVchNo() {
    const result = await this.apiService
      .getDataById('Purchase/GetGatePassInwardEntryVchNo', {
        vchType: 'RP-RAW',
      })
      .toPromise();
    this.InwardEntryForm.get('vchNo').patchValue(result[0].VCHNO);
  }

  async getVchType() {
    // const result = await this.apiService
    //   .getData('Common/GetTypes')
    //   .toPromise();
    // if(result.length != 0){
    //   this.InwardEntryForm.get('vchType').patchValue(result[0].VCHNO)
    // }
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
        vchType: this.vchType,
        grnNo: this.grnNo,
      };

      const result = await this.apiService
        .getDataById('Purchase/GetGatePassInwardList', obj)
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

  onItemSubChange(event: any) {
    let sdwt = this.itemSubList.find((i) => i.CODE === event.CODE).SdWt;
    let Uomid = this.itemSubList.find((i) => i.CODE === event.CODE).Uomid;
    this.InwardEntryForm.get('standardWt').setValue(sdwt);
    this.InwardEntryForm.get('uom').setValue(Uomid);
    if (sdwt > 0) {
      this.InwardEntryForm.get('gross').disable();
      this.InwardEntryForm.get('tare').disable();
    } else {
      this.InwardEntryForm.get('gross').enable();
      this.InwardEntryForm.get('tare').enable();
    }



  }

  itemRefresh() {
    // this.InwardEntryForm.get('itemMain')?.patchValue(null);
    this.InwardEntryForm.get('itemSub')?.patchValue(null);
    this.InwardEntryForm.get('uom')?.patchValue(null);
    this.InwardEntryForm.get('bagsType')?.patchValue('W');
    this.InwardEntryForm.get('bags')?.patchValue('');
    this.InwardEntryForm.get('gross')?.patchValue('');
    this.InwardEntryForm.get('tare')?.patchValue('');
    this.InwardEntryForm.get('bagsWt')?.patchValue('');
    this.InwardEntryForm.get('expWt')?.patchValue('');
    this.InwardEntryForm.get('standardWt')?.patchValue('');
    // this.InwardEntryForm.get('retStat')?.patchValue('In-Stock');
    // this.InwardEntryForm.get('sTax')?.patchValue('');
    // this.InwardEntryForm.get('iTax')?.patchValue('');
    this.InwardEntryForm.get('net')?.patchValue('');

    // this.itemSubList = [];

    this.InwardEntryForm.get('poBal')?.patchValue('');
    //this.InwardEntryForm.get('poNo').setValue('');
  }

  onAdd() {
    let form;
  
    let EditExist = true;
    const gStatus = this.InwardEntryForm.controls.gross.status;
    if (gStatus === 'DISABLED') {
      this.InwardEntryForm.get('gross').enable();
      this.InwardEntryForm.get('tare').enable();
      form = this.InwardEntryForm.value;
      this.InwardEntryForm.get('gross').disable();
      this.InwardEntryForm.get('tare').disable();
    } else {
      form = this.InwardEntryForm.value;
    }

    if (
      form.bagsType === null ||
      form.bagsType === null ||
      form.bagsType === ''
    ) {
      this.tostr.warning('Select Bags Type....!');
      return;
    }
  
    if (
      form.vehicleNo === null ||
      form.vehicleNo === ''
    ) {
      this.tostr.warning('Select Vehicle No....!');
      return;
    }

    if (
      form.biltyNo === null ||
      form.biltyNo === ''
    ) {
      this.tostr.warning('Select Bilty No....!');
      return;
    }


    //Duplicate Check
    if (this.editModeSno) {
      const ROwData = this.detailsList.find((row) => row.sno === this.editSno);
      if (ROwData) {
        EditExist = false;
        if (ROwData.code + ROwData.sub != form.itemSub) {
          EditExist = true;
        }
      }
    }
    if (EditExist == true) {
      const itemDoubleCheck = this.detailsList.find(
        (row) => row.code + row.sub == form.itemSub
      );
      if (itemDoubleCheck) {
        this.tostr.warning('Item already in table. Select other item....!');
        return;
      }
    }
    if (this.detailsList.length > 0) {
      const sumOfStandardWt = this.detailsList.reduce(
        (sum, row) => (row.standardWt > 0 ? sum + row.standardWt : sum),
        0
      );
      if (sumOfStandardWt > 0 && form.standardWt <= 0) {
        this.tostr.warning('Only Item Added With Standard Weight....!');
        return;
      }
      if (sumOfStandardWt == 0 && this.detailsList.length > 0) {
        if( this.editModeSno ==false)
          {
            this.tostr.warning('Only 1 Item Added Without Standard Weight....!');
            return;
          }
      }
    }


    if ((this.InwardEntryForm.get('bags').value ?? 0) ==0)
    {
      this.tostr.warning('Enter Tanker/Bags/Qty....!');
      return;
    }


    if (this.auth.poMust() && (form.poNo == 0)) {
      this.tostr.warning('Select Po Number....!');
      return;
    }

    if ( form.net > form.poBal && (this.InwardEntryForm.get('poNo').value ?? 0) > 0) {
      this.tostr.warning('Balance is less than Net....!');
      return;
    }


    if (
      form.itemMain === null ||
      form.itemMain === null ||
      form.itemMain === ''
    ) {
      this.tostr.warning('Select Item Main....!');
      return;
    }

    if (form.itemSub === null || form.itemSub === null || form.itemSub === '') {
      this.tostr.warning('Select Item Sub....!');
      return;
    }

    if (form.godown === null || form.godown === null || form.godown === '') {
      this.tostr.warning('Select Godown....!');
      return;
    }

    if (form.uom === null || form.uom === null || form.uom === '') {
      this.tostr.warning('Select UOM....!');
      return;
    }

    if (form.gross === null || form.gross === null || form.gross === '') {
      this.tostr.warning('Enter Gross....!');
      return;
    }

    if (form.tare === null || form.tare === null || form.tare === '') {
      this.tostr.warning('Enter Tare....!');
      return;
    }

    if (form.net === null || form.net === null || form.net === '') {
      this.tostr.warning('Net not be empty....!');
      return;
    }

    let itemName = this.itemSubList.find((i) => i.CODE === form.itemSub);

    form.code = form.itemSub.substring(0, 9);
    form.sub = form.itemSub.substring(9, 14);
    form.itemName = itemName.NAME;

    form.godownid = this.godownList.find(
      (i) => i.GODOWNID === form.godown
    ).GODOWNID;
    form.godown = this.godownList.find(
      (i) => i.GODOWNID === form.godown
    ).GODOWNNAME;
    form.uom = this.uomList.find((i) => i.id === form.uom).name;

    form.rate = form.rate > 1 ? form.rate : 1;

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
        //this.resetForm();
        this.itemRefresh();
        return;
      }
    }

    form.sno = this.detailsList.length + 1;
    this.detailsList.push(form);
    //this.resetForm();
    this.itemRefresh();
  }

  editItem(row: any) {
    this.btnAdd = 'Update';

    this.editModeSno = true;
    this.editSno = row.sno;

    const code = row.code + row.sub;
    this.InwardEntryForm.get('itemMain')?.patchValue(row.code);
    this.getItemSub();
    this.InwardEntryForm.get('itemSub')?.patchValue(code);

    this.InwardEntryForm.get('bags')?.patchValue(row.bags);
    this.InwardEntryForm.get('bagsType')?.patchValue(row.bagsType);

    this.InwardEntryForm.get('gross')?.patchValue(row.gross);
    this.InwardEntryForm.get('tare')?.patchValue(row.tare);

    this.InwardEntryForm.get('net')?.patchValue(row.net);
    this.InwardEntryForm.get('rate')?.patchValue(row.rate);
    this.InwardEntryForm.get('standardWt')?.patchValue(row.standardWt);

    const uom = this.uomList.find((i) => i.name === row.uom).id;
    this.InwardEntryForm.get('uom')?.patchValue(uom);

    this.InwardEntryForm.get('godown')?.setValue(row.godownid);
    this.InwardEntryForm.get('poNo')?.setValue(row.poNo);
    this.InwardEntryForm.get('poBal')?.setValue(row.poBal);
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

  async onClickSave() {
    debugger
    const currentTime = new Date();

    const finalDate = this.dp.transform(currentTime, 'yyyy/MM/dd HH:mm:ss');
    if (this.detailsList.length == 0) {
      this.tostr.warning('Incomplete Transaction...');
      return;
    }

    let body = this.InwardEntryForm.value;

    if (body.vchType === null || body.vchType === null || body.vchType === '') {
      this.tostr.warning('Select Vch Type....!');
      return;
    }

    if (
      body.freightDD === null ||
      body.freightDD === null ||
      body.freightDD === ''
    ) {
      this.tostr.warning('Select Freight Type....!');
      return;
    }

    if (
      body.minWeight === null ||
      body.minWeight === null ||
      body.minWeight === ''
    ) {
      this.tostr.warning('Select Minimun Weight Type....!');
      return;
    }

    if (body.biltyNo === null || body.biltyNo === null || body.biltyNo === '') {
      this.tostr.warning('Enter Bilty No....!');
      return;
    }

    if (
      body.location === null ||
      body.location === null ||
      body.location === ''
    ) {
      this.tostr.warning('Select Location....!');
      return;
    }

    if (
      body.vehicleNo === null ||
      body.vehicleNo === null ||
      body.vehicleNo === ''
    ) {
      this.tostr.warning('Enter Vehicle No....!');
      return;
    }

    // if (body.biltyNo  === null || body.biltyNo  === null || body.biltyNo  === "") {
    //   this.tostr.warning('Enter Bilty No....!');
    //   return;
    // }

    if (
      body.partyMain === null ||
      body.partyMain === null ||
      body.partyMain === ''
    ) {
      this.tostr.warning('Select Party Main....!');
      return;
    }

    if (
      body.partySub === null ||
      body.partySub === null ||
      body.partySub === ''
    ) {
      this.tostr.warning('Select Party Main....!');
      return;
    }

    if (body.retStat === null || body.retStat === null || body.retStat === '') {
      this.tostr.warning('Select Ret-Stat ....!');
      return;
    }

    
    if (body.poNo === null || body.poNo == '0' || body.retStat === '') {
      this.tostr.warning('Enter Pono ....!');
     // return;
    }

    let vchNo = this.editMode ? this.InwardEntryForm.get('vchNo')?.value : 0;

    // let vchNo = 0
    // if(this.editMode == true){vchNo = this.requisitionForm.get('transNo')?.value}
    // else {vchNo = 0}

    const voucher: any[] = this.detailsList.map((data) => ({
      vchType: this.InwardEntryForm.get('vchType')?.value,
      vchNo: vchNo,
      location: this.InwardEntryForm.get('location')?.value,
      vehicleNo: this.InwardEntryForm.get('vehicleNo')?.value,
      biltyNo: this.InwardEntryForm.get('biltyNo')?.value,

      grnDate: this.dp.transform(
        this.InwardEntryForm.get('grnDate')?.value,
        'yyyy-MM-dd'
      ),

      freight:
        this.InwardEntryForm.get('freight')?.value === ''
          ? 0
          : this.InwardEntryForm.get('freight')?.value,
      freightDD: this.InwardEntryForm.get('freightDD')?.value,
      chkMinWeight: this.InwardEntryForm.get('chkMinWeight')?.value,
      minWeight: this.InwardEntryForm.get('minWeight')?.value,
      remarks: this.InwardEntryForm.get('remarks')?.value,
      partyMain: this.InwardEntryForm.get('partyMain')?.value,
      partySub: this.InwardEntryForm.get('partySub')?.value,
      subParty: this.InwardEntryForm.get('subParty')?.value,
      itemMain: data.code,
      itemSub: data.code + data.sub,
      godown: this.godownList.find((i) => i.GODOWNNAME === data.godown)
        .GODOWNID,
      uom: this.uomList.find((i) => i.name === data.uom).id,
      bags: data.bags === '' ? 0 : data.bags,
      bagsType: data.bagsType,
      gross: data.gross,
      tare: data.tare,
      bagsWt: this.InwardEntryForm.get('bagsWt')?.value,
      expWt: (data.standardWt ?? 0) * (data.bags === '' ? 0 : data.bags),
      standardWt: data.standardWt ?? 0,
      retStat: this.InwardEntryForm.get('retStat')?.value,
      sTax: this.InwardEntryForm.get('sTax')?.value,
      iTax: this.InwardEntryForm.get('iTax')?.value,
      net: data.net,
      manualGpiNo: this.InwardEntryForm.get('manualGpiNo')?.value,
      driverName: this.InwardEntryForm.get('driverName')?.value,
      contact: this.InwardEntryForm.get('contact')?.value,
      tCNIC: this.InwardEntryForm.get('tCNIC')?.value,
      transitPermit: this.InwardEntryForm.get('transitPermit')?.value,
      sludge: this.InwardEntryForm.get('sludge')?.value,
      color: this.InwardEntryForm.get('color')?.value,
      wbCharge: this.InwardEntryForm.get('wbCharge')?.value,
      areaMain: this.InwardEntryForm.get('areaMain')?.value,
      areaSub: this.InwardEntryForm.get('areaSub')?.value,
      rate: parseFloat(data.rate),
      pono: this.InwardEntryForm.get('poNo').value ?? 0,

      entrydate: finalDate,


    }));

    try {
      this.com.showLoader();

      var v = await this.checkVechicleGPInward();
      if (v) {
        this.tostr.warning('Vehical Already In....!');
        this.com.hideLoader();
        return;
      }

      this.apiService
        .saveData('Purchase/SaveGatePassInwardEntry', voucher)
        .subscribe((r) => {
          this.com.hideLoader();

          if (r.status == true || r.status == 'true') {
            this.tostr.success('Save Successfully');
            this.InwardEntryForm.get('vchNo').patchValue(r.vchno);
            this.onClickRefresh();
            this.getVouchersList();
            //this.getVchNo();
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

  async checkVechicleGPInward() {
    const d = this.InwardEntryForm.get('grnDate').value;

    const finalDate = this.dp.transform(d, 'yyyy/MM/dd');

    let obj = {
      vehicalNo: this.InwardEntryForm.get('vehicleNo')?.value ?? '',
      vchno: this.InwardEntryForm.get('vchNo')?.value ?? '',
      vchtType: this.InwardEntryForm.get('vchType')?.value ?? '',
      vchDate: finalDate,
    };

    const result = await this.apiService
      .getDataById('Purchase/CheckVechicleGPInward', obj)
      .toPromise();

    if (result.length == 0) {
      return false;
    } else {
      return true;
    }
  }

  editVouchers(VCHNO: any, VCHTYPE: any): void {
    this.onClickRefresh();
    this.isShow = true;
    this.editMode = true;
    this.readonly = false;
    this.InwardEntryForm.get('grnDate').enable();

    try {
      this.com.showLoader();

      const obj = {
        vchNo: VCHNO,
        vchType: VCHTYPE,
      };

      this.apiService
        .getDataById('Purchase/GetEditGatePassInward', obj)
        .subscribe((data) => {
          this.togglePages();

          const d = data[0];
          console.log(data)

          if (d.APROVE == 0 || d.APROVE == false || d.APROVE == 'false') {
            this.editDisabled = false;
          }

          if (d.APROVE == 1 || d.APROVE == true || d.APROVE == 'true') {
            this.editDisabled = true;
          }

          this.InwardEntryForm.get('vchType')?.patchValue(d.VCHTYPE);
          this.InwardEntryForm.get('location')?.patchValue(d.LOCATION);
          this.InwardEntryForm.get('vchNo')?.patchValue(d.VCHNO);
          this.InwardEntryForm.get('vehicleNo').patchValue(d.VEHICLENO);
          this.InwardEntryForm.get('biltyNo').patchValue(d.BILLTYNO);

          this.InwardEntryForm.get('grnDate')?.patchValue(
            new Date(
              d.VCHDATE.split('/')[2],
              d.VCHDATE.split('/')[1] - 1,
              d.VCHDATE.split('/')[0]
            )
          );
          //this.InwardEntryForm.get('grnDate').patchValue(d.VCHDATE);

          this.InwardEntryForm.get('freight').patchValue(d.FREIGHT);
          this.InwardEntryForm.get('freightDD').patchValue(d.FREIGHTTYPE);
          this.InwardEntryForm.get('chkMinWeight').patchValue(d.MINIWT);
          this.InwardEntryForm.get('minWeight').patchValue(d.WTYPE);

          const partyMain = d.PARTYSUB.substring(0, 9);
          
          this.InwardEntryForm.get('partyMain').patchValue(partyMain);
          this.getPartySub();

          this.InwardEntryForm.get('partySub').patchValue(d.PARTYSUB);
          this.InwardEntryForm.get('subParty').patchValue(d.SUBPARTY);
          this.InwardEntryForm.get('remarks').patchValue(d.REMARKS);

          this.InwardEntryForm.get('driverName').patchValue(d.DRIVERNAME);
          this.InwardEntryForm.get('contact').patchValue(d.DRIVERCONTACT);
          this.InwardEntryForm.get('tCNIC').patchValue(d.DRIVERCNIC);

          data.forEach((item: any) => {
            // this.InwardEntryForm
            //   .get('vchDate')
            //   ?.patchValue(
            //     new Date(
            //       item.VCHDATE.split('/')[2],
            //       item.VCHDATE.split('/')[1] - 1,
            //       item.VCHDATE.split('/')[0]
            //     )
            //   );

            let form: any = [];

            form.code = item.ITEMSUBCODE.substring(0, 9);
            form.sub = item.ITEMSUBCODE.substring(9, 14);
            form.sno = this.detailsList.length + 1;
            form.itemName = item.ITEMNAME;
            form.bags = item.SBAGS;
            form.bagsType = item.BAGSTYPE;
            form.net = item.SQTY;
            form.rate = item.RATE;
            form.standardWt = item.SDWT;
            form.uom = item.UOMNAME;
            form.godown = item.GODOWNNAME;
            form.godownid = this.godownList.find(
              (i) => i.GODOWNNAME === item.GODOWNNAME
            ).GODOWNID;
            form.gross = item.GROSS;
            form.tare = item.TARE;
            form.poNo = item.PONO;


            this.detailsList.push(form);
          });


          this.com.hideLoader();
        });
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  deleteVouchers(VCHNO: any, VCHTYPE: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {
      try {
        this.com.showLoader();

        const obj = {
          vchNo: VCHNO,
          vchType: VCHTYPE,
        };

        this.apiService
          .deleteData('Purchase/DelGatePassInward', obj)
          .subscribe({
            next: (data) => {
              this.com.hideLoader();

              if (data == 'true' || data == true) {
                this.tostr.success('Delete Successfully');
                this.getVouchersList();
                //this.getVchNo();
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
 

    this.InwardEntryForm.get('chkMinWeight')?.patchValue(true);
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
    this.getVchNo();

    if(this.partyMainList.length > 0){
      this.InwardEntryForm.get('partyMain')?.patchValue(this.partyMainList[0].CODE);
      this.getPartySub();
    }

    if(this.itemMainList.length > 0){
      this.InwardEntryForm.get('itemMain')?.patchValue(this.itemMainList[0].CODE);
      this.getItemSub();
    }

    if(this.godownList.length > 0){
      this.InwardEntryForm.get('godown')?.patchValue(this.godownList[0].GODOWNID);
    }

    this.vehicles.nativeElement.focus();
    this.InwardEntryForm.get('grnDate').enable();
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

  netCalculation() {
    const standardWt = parseFloat(
      this.InwardEntryForm.get('standardWt')?.value
    );
    const bags = parseFloat(this.InwardEntryForm.get('bags')?.value);
    if (standardWt > 0) {
      const result = standardWt * bags;

      this.InwardEntryForm.get('gross').setValue(result + 500);
      this.InwardEntryForm.get('tare').setValue(500);

      const net = result + 500 - 500;

      this.InwardEntryForm.get('net').patchValue('');
      if (!isNaN(net)) {
        this.InwardEntryForm.get('net').patchValue(net);
      }
    } else {
      const gross = parseFloat(this.InwardEntryForm.get('gross')?.value);
      const tare = parseFloat(this.InwardEntryForm.get('tare')?.value);
      if (isNaN(gross) || isNaN(tare)) {
        this.InwardEntryForm.get('net').patchValue('');
      } else {
        this.InwardEntryForm.get('net').patchValue(gross - tare);
      }
    }
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
}