import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild, numberAttribute } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';
declare const $: any;

@Component({
  selector: 'app-purchase-correction-form',
  templateUrl: './purchase-correction-form.component.html',
  styleUrls: ['./purchase-correction-form.component.css']
})
export class PurchaseCorrectionFormComponent {
  
  constructor(
    private tostr: ToastrService,
    private fb: FormBuilder,
    private apiService: ApiService,
    private dp: DatePipe,
    private auth: AuthService,
    private com: CommonService,
  ) {
    const today = new Date();
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  }

  @ViewChild('voucherLists', { static: false }) voucherLists!: ElementRef;
  voucherList: any[] = [];
  fromDate: Date;
  toDate: Date;
  vchType: any = 'RP-Raw';
  grnNo: number = 0;

  locationList: any = [];
  freightList: any = [];
  minimumWeightList: any = [];
  godownList: any = [];
  uomList: any = [];
  bagsTypeList: any = [];
  LabDeductionList: any = [];
  partyMainList: any = [];
  partySubList: any = [];
  supPartyList: any = [];

  itemMainList: any = [];
  itemSubList: any = [];

  detailsList: any = [];
  isMinWtFinal: boolean = false;

  poDetailsList: any = [];

  isShow = false;
  isShowPage: boolean = true;
  PurchaseCorrectionForm!: FormGroup;
  editModeSno: boolean = false;
  editMode: boolean = true;
  readonly: boolean = true;

  btnAdd:string = 'Add';
  editSno: any = '';


  ngOnInit() {
    this.formInit();
    this.getVchNo();
    this.getVchType();
    this.getVouchersList();
    this.getPartyMain();
    this.getItemMain();
    this.getLocation();
    this.getUOM();
    this.getBagsType();
    this.getGodowns();
  }

  formInit() {
    this.PurchaseCorrectionForm = this.fb.group({
      vchType: ['RP-Raw'],
      vchNo: [''],
      gpino: [''],
      location: [this.auth.locId()],
      vehicleNo: [''],
      biltyNo: [''],
      grnDate: [new Date()],
      freightDD: ['S'],
      freight: [''],
      chkMinWeight: [true],
      minWeight: ['O'],
      partyMain: [undefined],
      partySub: [undefined],
      subParty: [undefined],
      itemMain: [undefined],
      itemSub: [undefined],
      godown: [undefined],
      uom: [undefined],
      bagsType: ['W'],
      bags: [''],
      gross: [''],
      tare: [''],
      bagsWt: [''],
      bagswt1:[''],
      expWt: [''],
      standardWt: [''],
      retStat: ['In-Stock'],
      sTax: [''],
      iTax: [''],
      net: [''],
      remarks: [''],
      manualGpiNo: [''],
      Weight: [''],
      // driverName: [''],
      // contact: [''],
      // tCNIC: [''],
      // transitPermit: [''],
      // sludge: [''],
      // color: [''],
      wbCharge: [''],
      // areaMain: [''],
      // areaSub: [''],
      rate: [''],
      calcLab: [''],
      stockWt: [''],
      labStk: [''],
      biltyWt: [''],
      grossWt: [''],
      grossWt2: [''],
      secWt: [''],
      firstWt: [''],
      payableWt: [''],
      labParty: [''],
      netWt: [''],
      tareWt: [''],
      bagsTypeDDS1: [undefined],
      bagWt1: [undefined],
      bag1: [''],
      bagsTypeDDS2: [undefined],
      bagWt2: [undefined],
      bag2: [''],
      bagsTypeDDS3: [undefined],
      bagWt3: [undefined],
      bag3: [''],
      id: [''],

      poNo: [''],
      poBal: [''],
    });
  }

  resetForm() {

    this.readonly = true;

    this.PurchaseCorrectionForm.get('vchType')?.patchValue('RP-RAW');
    this.PurchaseCorrectionForm.get('vchNo')?.patchValue('');
    this.PurchaseCorrectionForm.get('gpino')?.patchValue('');
    this.PurchaseCorrectionForm.get('location')?.patchValue(this.auth.locId());
    this.PurchaseCorrectionForm.get('vehicleNo')?.patchValue('');
    this.PurchaseCorrectionForm.get('biltyNo')?.patchValue('');
    this.PurchaseCorrectionForm.get('grnDate')?.patchValue(new Date());
    this.PurchaseCorrectionForm.get('freightDD')?.patchValue('S');
    this.PurchaseCorrectionForm.get('freight')?.patchValue('');
    this.PurchaseCorrectionForm.get('chkMinWeight')?.patchValue(true);
    this.PurchaseCorrectionForm.get('minWeight')?.patchValue('O');
    this.PurchaseCorrectionForm.get('partyMain')?.patchValue(undefined);
    this.PurchaseCorrectionForm.get('partySub')?.patchValue(undefined);
    this.PurchaseCorrectionForm.get('subParty')?.patchValue(undefined);
    this.PurchaseCorrectionForm.get('itemMain')?.patchValue(undefined);
    this.PurchaseCorrectionForm.get('itemSub')?.patchValue(undefined);
    this.PurchaseCorrectionForm.get('godown')?.patchValue(undefined);
    this.PurchaseCorrectionForm.get('uom')?.patchValue(undefined);
    this.PurchaseCorrectionForm.get('bagsType')?.patchValue('W');
    this.PurchaseCorrectionForm.get('bags')?.patchValue('');
    this.PurchaseCorrectionForm.get('gross')?.patchValue('');
    this.PurchaseCorrectionForm.get('tare')?.patchValue('');
    this.PurchaseCorrectionForm.get('bagsWt')?.patchValue('');
    this.PurchaseCorrectionForm.get('expWt')?.patchValue('');
    this.PurchaseCorrectionForm.get('standardWt')?.patchValue('');
    this.PurchaseCorrectionForm.get('retStat')?.patchValue('In-Stock');
    this.PurchaseCorrectionForm.get('sTax')?.patchValue('');
    this.PurchaseCorrectionForm.get('iTax')?.patchValue('');
    this.PurchaseCorrectionForm.get('net')?.patchValue('');
    this.PurchaseCorrectionForm.get('remarks')?.patchValue('');
    this.PurchaseCorrectionForm.get('manualGpiNo')?.patchValue('');
    // this.PurchaseCorrectionForm.get('driverName')?.patchValue('');
    // this.PurchaseCorrectionForm.get('contact')?.patchValue('');
    // this.PurchaseCorrectionForm.get('tCNIC')?.patchValue('');
    // this.PurchaseCorrectionForm.get('transitPermit')?.patchValue('');
    // this.PurchaseCorrectionForm.get('sludge')?.patchValue('');
    // this.PurchaseCorrectionForm.get('color')?.patchValue('');
    this.PurchaseCorrectionForm.get('wbCharge')?.patchValue('');
    // this.PurchaseCorrectionForm.get('areaMain')?.patchValue('');
    // this.PurchaseCorrectionForm.get('areaSub')?.patchValue('');
    this.PurchaseCorrectionForm.get('rate')?.patchValue('');
    this.PurchaseCorrectionForm.get('calcLab')?.patchValue('');
    this.PurchaseCorrectionForm.get('stockWt')?.patchValue('');
    this.PurchaseCorrectionForm.get('labStk')?.patchValue('');
    this.PurchaseCorrectionForm.get('biltyWt')?.patchValue('');
    this.PurchaseCorrectionForm.get('grossWt')?.patchValue('');
    this.PurchaseCorrectionForm.get('grossWt2')?.patchValue('');
    this.PurchaseCorrectionForm.get('secWt')?.patchValue('');
    this.PurchaseCorrectionForm.get('firstWt')?.patchValue('');
    this.PurchaseCorrectionForm.get('payableWt')?.patchValue('');
    this.PurchaseCorrectionForm.get('labParty')?.patchValue('');
    this.PurchaseCorrectionForm.get('netWt')?.patchValue('');
    this.PurchaseCorrectionForm.get('tareWt')?.patchValue('');
    this.PurchaseCorrectionForm.get('bagsTypeDDS1')?.patchValue(undefined);
    this.PurchaseCorrectionForm.get('bag1')?.patchValue('');
    this.PurchaseCorrectionForm.get('bagWt1')?.patchValue(undefined);
    this.PurchaseCorrectionForm.get('bagsTypeDDS2')?.patchValue(undefined);
    this.PurchaseCorrectionForm.get('bag2')?.patchValue('');
    this.PurchaseCorrectionForm.get('bagWt2')?.patchValue(undefined);
    this.PurchaseCorrectionForm.get('bagsTypeDDS3')?.patchValue(undefined);
    this.PurchaseCorrectionForm.get('bag3')?.patchValue('');
    this.PurchaseCorrectionForm.get('bagWt3')?.patchValue(undefined);

    this.itemSubList = [];
    this.partySubList = [];

    this.PurchaseCorrectionForm.get('grnDate').disable();

    this.PurchaseCorrectionForm.get('poBal')?.patchValue('');
    this.PurchaseCorrectionForm.get('poNo').setValue('');
  }

  async getPartyMain() {

    const result = await this.apiService
      .getDataById('Common/GetLevel4CodeNameByTag', {tag: 'C'})
      .toPromise();
    this.partyMainList = result;
  }

  async getPartySub(event:any) {

    const result = await this.apiService
      .getDataById('Common/GetLevel5CodeNameByL4Code', {code: event.CODE})
      .toPromise();
      this.partySubList = result;
  }

  onPartyClear(){
    this.partySubList = [];
    this.PurchaseCorrectionForm.get('partySub')?.patchValue(undefined);
  }


  async getTblSubParty(event: any) {

    const result = await this.apiService
      .getDataById('Sale/GetSubParty', { code: event.CODE })
      .toPromise();
      this.supPartyList = result;
  }

  async getItemMain() {

    const result = await this.apiService
      .getDataById('Common/GetLevel4CodeNameByTag', {tag: 'S'})
      .toPromise();

    this.itemMainList = result;

  }

  async getItemSub(event:any) {

    const result = await this.apiService
      .getDataById('Common/GetLevel5CodeNameByL4Code', {code: event.CODE})
      .toPromise();

    this.itemSubList = result;
  
  }

  async onItemSubChange(event:any) {

   debugger;
    let sdwt = this.itemSubList.find((i) => i.CODE === event.CODE).SdWt;
    let Uomid = this.itemSubList.find((i) => i.CODE === event.CODE).Uomid;
      this.PurchaseCorrectionForm.get('standardWt').setValue(sdwt);
      this.PurchaseCorrectionForm.get('uom').setValue(Uomid);
      if (sdwt > 0) {
        this.PurchaseCorrectionForm.get('gross').disable();
        this.PurchaseCorrectionForm.get('tare').disable();
      } else {
        this.PurchaseCorrectionForm.get('gross').enable();
        this.PurchaseCorrectionForm.get('tare').enable();
      }
      this.netCalculation();
  }

  onItemClear(){
    this.itemSubList = [];
    this.PurchaseCorrectionForm.get('itemSub')?.patchValue(undefined);
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

  async getBagsType() {
    const result = await this.apiService
    .getData('Common/GetPurchaseBagsType')
    .toPromise();

    this.bagsTypeList = result;

  }

  async getGodowns() {

    const result = await this.apiService
      .getData('Common/GetGodowns')
      .toPromise();
    this.godownList = result;

  }

  async getVchNo() {

    // const result = await this.apiService
    // .getDataById('Purchase/GetPurchaseCorrectionVchNo', {vchType: 'RP-Raw'})
    // .toPromise();
    // this.PurchaseCorrectionForm.get('vchNo').patchValue(result[0].VCHNO)

    const gpino = await this.apiService
    .getData('Purchase/GetMaxGpNo')
    .toPromise();
    this.PurchaseCorrectionForm.get('gpino').patchValue(gpino[0].GPINO);

    const vchno = await this.apiService
    .getDataById('Purchase/GetGatePassInwardEntryVchNo', { vchType: 'RP-Raw' })
    .toPromise();
    this.PurchaseCorrectionForm.get('vchNo').patchValue(vchno[0].VCHNO)

  }

  async getVchType() {

    // const result = await this.apiService
    // .getData('Common/GetTypes')
    // .toPromise();

    // if(result.length != 0){
    //   this.PurchaseCorrectionForm.get('vchType').patchValue(result[0].VCHNO)
    // }

  }

  async getVouchersList() {

    const obj = {
      fromDate: this.dp.transform(this.fromDate, 'yyyy/MM/dd'),
      toDate: this.dp.transform(this.toDate, 'yyyy/MM/dd'),
      vchType: this.vchType,
      grnNo: this.grnNo,
    };

    const result = await this.apiService
    .getDataById('Purchase/GetPurchaseCorrectionList', obj)
    .toPromise();

    this.voucherList = result

  }
  
  async getPoDetails() {
    const party = this.PurchaseCorrectionForm.get('partySub').value;
    const item = this.PurchaseCorrectionForm.get('itemSub').value;

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
          this.PurchaseCorrectionForm.get('grnDate')?.value,
          'yyyy-MM-dd'
        ),
        Vchno : this.PurchaseCorrectionForm.get('id').value,
        Pono:0,
      };

      const result = await this.apiService
        .getDataById('Common/GetPoDetailsByPartyAndItems', obj)
        .toPromise();
      this.com.hideLoader();

      $('#poDetailsModal').modal('show');

      const r = result[0];
      this.poDetailsList = result;
      this.PurchaseCorrectionForm.get('freightDD').patchValue(r.FreightType);
      this.PurchaseCorrectionForm.get('poNo').patchValue(r.PoNo);
      this.PurchaseCorrectionForm.get('poBal').patchValue(r.BalQty);
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  selectPo(r: any) {
    this.PurchaseCorrectionForm.get('freightDD').patchValue(r.FreightType);
    this.PurchaseCorrectionForm.get('poNo').patchValue(r.PoNo);
    this.PurchaseCorrectionForm.get('poBal').patchValue(r.BalQty);
    $('#poDetailsModal').modal('hide');
  }
  
  itemRefresh(){

    this.PurchaseCorrectionForm.get('itemMain')?.patchValue(undefined);
    this.PurchaseCorrectionForm.get('itemSub')?.patchValue(undefined);
    this.PurchaseCorrectionForm.get('uom')?.patchValue(undefined);
    this.PurchaseCorrectionForm.get('bagsType')?.patchValue('W');
    this.PurchaseCorrectionForm.get('bags')?.patchValue('');
    this.PurchaseCorrectionForm.get('gross')?.patchValue('');
    this.PurchaseCorrectionForm.get('tare')?.patchValue('');
    this.PurchaseCorrectionForm.get('bagsWt')?.patchValue('');
    this.PurchaseCorrectionForm.get('expWt')?.patchValue('');
    this.PurchaseCorrectionForm.get('standardWt')?.patchValue('');
    // this.PurchaseCorrectionForm.get('retStat')?.patchValue('In-Stock');
    // this.PurchaseCorrectionForm.get('sTax')?.patchValue('');
    // this.PurchaseCorrectionForm.get('iTax')?.patchValue('');
    this.PurchaseCorrectionForm.get('net')?.patchValue('');

    this.PurchaseCorrectionForm.get('grossWt')?.patchValue('');
    this.PurchaseCorrectionForm.get('biltyWt')?.patchValue('');

    this.PurchaseCorrectionForm.get('poBal')?.patchValue('');
    this.PurchaseCorrectionForm.get('poNo').setValue('');
    
    this.itemSubList = [];
  }

  onAdd() {

    let form;
    let EditExist = true;
    const gStatus = this.PurchaseCorrectionForm.controls.gross.status;
    if (gStatus === 'DISABLED') {
      this.PurchaseCorrectionForm.get('gross').enable();
      this.PurchaseCorrectionForm.get('tare').enable();
      form = this.PurchaseCorrectionForm.value;
      this.PurchaseCorrectionForm.get('gross').disable();
      this.PurchaseCorrectionForm.get('tare').disable();
    }
    else {
      form = this.PurchaseCorrectionForm.value;
    }

    //Duplicate Check
    if (this.editModeSno) {
      const ROwData = this.detailsList.find((row) => row.sno === this.editSno);
      if (ROwData) {
        EditExist = false;
        if (ROwData.code + ROwData.sub != form.itemSub) { EditExist = true; }
      }
    }
    if (EditExist == true) {
      const itemDoubleCheck = this.detailsList.find((row) => row.code + row.sub == form.itemSub);
      if (itemDoubleCheck) {
        this.tostr.warning('Item already in table. Select other item....!');
        return;
      }
    }

    if (this.detailsList.length > 0) {
      const sumOfStandardWt = this.detailsList.reduce((sum, row) => row.standardWt > 0 ? sum + row.standardWt : sum, 0);
      if (sumOfStandardWt > 0 && form.standardWt <= 0) {
        this.tostr.warning('Only Item Added With Standard Weight....!');
        return;
      }
      if (sumOfStandardWt == 0 && form.standardWt > 0) {
        this.tostr.warning('Only 1 Item Added Without Standard Weight....!');
        return;
      }
    }

    if (form.itemMain === null || form.itemMain === undefined || form.itemMain === "") {
      this.tostr.warning('Select Item Main....!');
      return;
    }

    if (form.itemSub === null || form.itemSub === undefined || form.itemSub === "") {
      this.tostr.warning('Select Item Sub....!');
      return;
    }

    if (form.godown === null || form.godown === undefined || form.godown === "") {
      this.tostr.warning('Select Godown....!');
      return;
    }

    if (form.uom === null || form.uom === undefined || form.uom === "") {
      this.tostr.warning('Select UOM....!');
      return;
    }

    if (form.gross === null || form.gross === undefined || form.gross === "") {
      this.tostr.warning('Enter Gross....!');
      return;
    }

    if (form.tare === null || form.tare === undefined || form.tare === "") {
      this.tostr.warning('Enter Tare....!');
      return;
    }

    if (form.net === null || form.net === undefined || form.net === "") {
      this.tostr.warning('Net not be empty....!');
      return;
    }

    if (this.auth.poMust() && (form.poNo == 0)) {
      this.tostr.warning('Select Po Number....!');
      return;
    }

  debugger;
    let itemName = this.itemSubList.find((i) => i.CODE === form.itemSub);

    form.code = form.itemSub.substring(0, 9);
    form.sub = form.itemSub.substring(9, 14);
    form.itemName = itemName.NAME;
    form.godownid = this.godownList.find((i) => i.GODOWNID === form.godown).GODOWNID;
    form.godown = this.godownList.find((i) => i.GODOWNID === form.godown).GODOWNNAME;
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
        this.btnAdd = 'Add'
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

    this.btnAdd = 'Update'

    this.editModeSno = true;
    this.editSno = row.sno;
    debugger;


    const code = row.code + row.sub;
    this.PurchaseCorrectionForm.get('itemMain')?.patchValue(row.code);
    this.getItemSub({ CODE: row.code });
    this.PurchaseCorrectionForm.get('itemSub')?.patchValue(code);

    this.PurchaseCorrectionForm.get('bags')?.patchValue(row.bags);
    this.PurchaseCorrectionForm.get('bagsType')?.patchValue(row.bagsType);

    this.PurchaseCorrectionForm.get('gross')?.patchValue(row.gross);
    this.PurchaseCorrectionForm.get('tare')?.patchValue(row.tare);

    this.PurchaseCorrectionForm.get('net')?.patchValue(row.net);
    this.PurchaseCorrectionForm.get('rate')?.patchValue(row.rate);
    this.PurchaseCorrectionForm.get('standardWt')?.patchValue(row.standardWt);

    const uom = this.uomList.find((i) => i.name === row.uom).id;
    this.PurchaseCorrectionForm.get('uom')?.patchValue(uom);

    this.PurchaseCorrectionForm.get('godown')?.setValue(row.godownid);
    this.PurchaseCorrectionForm.get('id').setValue(row.id);
    this.PurchaseCorrectionForm.get('poNo').setValue(row.poNo);
    this.netCalculation();
    this.CalculateNetWeight();
  }


  deleteItem(row: any) {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (!confirmDelete) {
      return;
    }

    const indexToRemove = this.detailsList.findIndex(
      (item) => item.sno === row.sno
    );
    if (indexToRemove !== -1) {
      this.detailsList.splice(indexToRemove, 1);
    }
  }




  CalculateNetWeight() {
debugger;
    const Weight = Number(this.PurchaseCorrectionForm.get('tareWt').value);
    const GWeight = Number(this.PurchaseCorrectionForm.get('grossWt2').value  )
    const  PartyWeight =Number(this.PurchaseCorrectionForm.get('biltyWt').value)
    let PayableWeight =0;

    const Bags1 = Number(this.PurchaseCorrectionForm.get('bag1').value);
    const Bags2 = Number(this.PurchaseCorrectionForm.get('bag2').value);
    const Bags3 = Number(this.PurchaseCorrectionForm.get('bag3').value);
    
    const Bags1Ded = Number(this.PurchaseCorrectionForm.get('bagWt1').value);
    const Bags2Ded = Number(this.PurchaseCorrectionForm.get('bagWt2').value);
    const Bags3Ded = Number(this.PurchaseCorrectionForm.get('bagWt3').value);

    const TBags =Bags1+Bags2+Bags3;
    const TBagsDed =Math.round((Bags1*Bags1Ded)+(Bags2*Bags2Ded)+(Bags3*Bags3Ded));


   

    // this.PurchaseCorrectionForm.get('bags1').setValue(Bags1); 
    // this.PurchaseCorrectionForm.get('bags2').setValue(Bags2); 
    // this.PurchaseCorrectionForm.get('bags3').setValue(Bags3); 

    // this.PurchaseCorrectionForm.get('deduction1').setValue(  Math.round((Bags1*Bags1Ded)) ); 
    // this.PurchaseCorrectionForm.get('deduction2').setValue(Math.round((Bags2*Bags2Ded))); 
    // this.PurchaseCorrectionForm.get('deduction3').setValue(Math.round((Bags3*Bags3Ded))); 


    //this.PurchaseCorrectionForm.get('NoOfBags').setValue(TBags); 
    this.PurchaseCorrectionForm.get('bagswt1').setValue(TBagsDed); 






    


    this.PurchaseCorrectionForm.get('netWt').setValue((GWeight- Weight)); 
    
    this.PurchaseCorrectionForm.get('payableWt').setValue((GWeight - Weight)); 

    this.PurchaseCorrectionForm.get('labParty').setValue(0);
    this.PurchaseCorrectionForm.get('labStk').setValue(0);


    //Minimum Weight Checking
    if(this.PurchaseCorrectionForm.get('chkMinWeight').value==true  && PartyWeight< Number(this.PurchaseCorrectionForm.get('netWt').value)) 
    {
      this.PurchaseCorrectionForm.get('payableWt').setValue(PartyWeight); 
    }
    //Assign PayableWeight
    PayableWeight= Number(this.PurchaseCorrectionForm.get('payableWt').value);

    
    //Lab Calculation


      this.LabDeductionList.forEach((item) => {

        let labwt=0;
        if (TBags>0)
          {
            if (item.Bags< Number(TBags))
              {
                labwt=  (PayableWeight/TBags)*  Number(item.Bags);
              } 
              else
              {
                labwt=  PayableWeight;
              }
          }
         

        const  LabDedParty=(labwt*item.PartyDed)/100;
        const  LabDedStock=(labwt*item.StockDed)/100;
        this.PurchaseCorrectionForm.get('labParty').setValue( Math.round( Number( this.PurchaseCorrectionForm.get('labParty').value)+LabDedParty ) ); 
        this.PurchaseCorrectionForm.get('labStk').setValue( Math.round(Number( this.PurchaseCorrectionForm.get('labStk').value)+ LabDedStock ));
        item.PartyDedKg=LabDedParty;
        item.StockDedKg=LabDedStock;
      });
    
    // if(this.PurchaseCorrectionForm.get('firstWeight').value=="true")
    // {
    // this.PurchaseCorrectionForm.get('gDiff').setValue(Weight-Gross);
    // }
    // else
    // {
    //   this.PurchaseCorrectionForm.get('gDiff').setValue(FristWeight-Gross);
    //   this.PurchaseCorrectionForm.get('tDiff').setValue(Weight-Tare);
    //   this.PurchaseCorrectionForm.get('nDiff').setValue((this.PurchaseCorrectionForm.get('nWeight').value - PartyWeight)); 
    // }
    
       this.PurchaseCorrectionForm.get('stockWt').setValue( this.PurchaseCorrectionForm.get('netWt').value  -TBagsDed);
    

      if ( this.PurchaseCorrectionForm.get('bagsType').value =="W")
      {
        this.PurchaseCorrectionForm.get('payableWt').setValue(Math.round(  this.PurchaseCorrectionForm.get('netWt').value - Number(this.PurchaseCorrectionForm.get('labParty').value))); 
      }
      else
      {
        this.PurchaseCorrectionForm.get('payableWt').setValue(Math.round(  this.PurchaseCorrectionForm.get('netWt').value  - (Number(this.PurchaseCorrectionForm.get('labParty').value)+TBagsDed))); 
      }




       if ( PartyWeight!=0 )
        {





          
          if ( this.PurchaseCorrectionForm.get('minWeight').value =="S")
            {
              if ( this.PurchaseCorrectionForm.get('bagsType').value =="W")
              {
                this.PurchaseCorrectionForm.get('payableWt').setValue(Math.round(  PartyWeight - Number(this.PurchaseCorrectionForm.get('labParty').value))); 
              }
              else
              {
                this.PurchaseCorrectionForm.get('payableWt').setValue(Math.round(  PartyWeight - (Number(this.PurchaseCorrectionForm.get('labParty').value)+TBagsDed))); 
              }

            }

            if(this.PurchaseCorrectionForm.get('chkMinWeight').value==true ) 
              {
                if (PartyWeight < Number(this.PurchaseCorrectionForm.get('netWt').value))
                  {
                   

                    if ( this.PurchaseCorrectionForm.get('bagsType').value =="W")
                      {
                        this.PurchaseCorrectionForm.get('payableWt').setValue(Math.round(  PartyWeight - Number(this.PurchaseCorrectionForm.get('labParty').value))); 
                      }
                      else
                      {
                        this.PurchaseCorrectionForm.get('payableWt').setValue(Math.round(  PartyWeight - (Number(this.PurchaseCorrectionForm.get('labParty').value)+TBagsDed))); 
                      }


                  }

                  else 
                    {
                     
  
                      if ( this.PurchaseCorrectionForm.get('bagsType').value =="W")
                        {
                          this.PurchaseCorrectionForm.get('payableWt').setValue(Math.round(  this.PurchaseCorrectionForm.get('netWt').value - Number(this.PurchaseCorrectionForm.get('labParty').value))); 
                        }
                        else
                        {
                          this.PurchaseCorrectionForm.get('payableWt').setValue(Math.round(  this.PurchaseCorrectionForm.get('netWt').value - (Number(this.PurchaseCorrectionForm.get('labParty').value)+TBagsDed))); 
                        }
  
  
                    }



                
              }


          
        }
     




        // this.PurchaseCorrectionForm.get('PartyOurDiff2').setValue( this.PurchaseCorrectionForm.get('payableWeight').value  -    PartyWeight);

    
  }





  async onClickSave() {
    if (this.detailsList.length == 0) {
      this.tostr.warning('Incomplete Transaction...');
      return;
    }
   debugger;
    let form = this.PurchaseCorrectionForm.value;
    let vchNo = this.editMode ? this.PurchaseCorrectionForm.get('vchNo')?.value : 0;
    let  stockWt= form.stockWt;
    let labStk= form.labStk;
    const voucher: any[] = this.detailsList.map((data) => ({
    vchType: form.vchType,
    vchNo: vchNo,
    gpino: form.gpino??0,
    location: form.location,
    vehicleNo: form.vehicleNo,
    biltyNo: form.biltyNo,
    grnDate: this.dp.transform(form.grnDate, 'yyyy-MM-dd'),
    freightDD: form.freightDD ?? 0,
    freight: form.freight ?? 0,
    chkMinWeight: form.chkMinWeight,
    weighttype: form.minWeight,
    partyMain: form.partyMain,
    partySub: form.partySub,
    subParty: form.subParty,
    itemSub: data.code+data.sub,
    godown: this.godownList.find((i) => i.GODOWNNAME === data.godown).GODOWNID,
    uom: this.uomList.find((i) => i.name === data.uom).id,
    bagsType: data.bagsType,
    sbags: data.bags ?? 0,
    gross: data.gross ?? 0,
    tare: data.tare ?? 0,
    bagsWt:Number( form.bagswt1),
    expWt: (data.standardWt ?? 0) * (data.bags === '' ? 0 : data.bags),
    standardWt: data.standardWt ?? 0,
    retStat: form.retStat,
    sTax: Number(form.sTax),
    iTax: Number(form.iTax),
    net: data.net ?? 0,
    remarks: form.remarks,
    manualGpiNo: Number(form.manualGpiNo),
    weight:Number( form.Weight),
    wbCharge: Number(form.wbCharge),
    rate: data.rate?? 0,
    calcLab: Number(form.calcLab) ,
    stockWt: Number(form.stockWt),
    labStk:  Number(form.labStk),
    biltyWt:  Number(form.biltyWt),
    grossWt: isNaN(parseInt(form.grossWt)) ? 0 : parseInt(form.grossWt),
    grossWt2: isNaN(parseInt(form.grossWt2)) ? 0 : parseInt(form.grossWt2),
    secWt: Number(form.secWt),
    firstWt:  isNaN(parseInt(form.firstW)) ? 0 : parseInt(form.firstW) ,
    payableWt: Number(form.payableWt),
    labParty: isNaN(parseInt(form.labParty)) ? 0 : parseInt(form.labParty) ,
    netWt: isNaN(parseInt(form.labPnetWtarty)) ? 0 : parseInt(form.netWt),
    tareWt: isNaN(parseInt(form.tareWt)) ? 0 : parseInt(form.tareWt),
    bagsTypeDDS1: form.bagsTypeDDS1,
    bagWt1: isNaN(parseFloat(form.bagWt1)) ? 0 : parseFloat(form.bagWt1),
    bag1: isNaN(parseInt(form.bag1)) ? 0 : parseInt(form.bag1),
    bagsTypeDDS2: form.bagsTypeDDS2,
    bagWt2: isNaN(parseFloat(form.bagWt2)) ? 0 : parseFloat(form.bagWt2),
    bag2: isNaN(parseInt(form.bag2)) ? 0 : parseInt(form.bag2),
    bagsTypeDDS3: form.bagsTypeDDS3,
    bagWt3: isNaN(parseFloat(form.bagWt3)) ? 0 : parseFloat(form.bagWt3),
    bag3: isNaN(parseInt(form.bag3)) ? 0 : parseInt(form.bag3),
    pono:data.poNo ?? 0,
    id:data.id,

    }));

    debugger;
   // return;
    try
    {
      this.com.showLoader();
        this.apiService
          .saveData('Purchase/SavePurchaseCorrection', voucher)
          .subscribe((result) => {
            this.com.hideLoader();
            if (result == true || result == 'true') {
              this.tostr.success('Save Successfully');
              this.onClickRefresh();
              this.getVouchersList();
              //this.getVchNo();
            } else {
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

  
     
    PrintReport(reportname:any) {
      debugger;
      let form = this.PurchaseCorrectionForm.value;
      let VchNo = this.PurchaseCorrectionForm.get('vchNo').value;
      let GpNo = this.PurchaseCorrectionForm.get('gpino').value;
      let Vehicle = this.PurchaseCorrectionForm.get('vehicleNo').value;
      let FromDate=this.dp.transform(form.grnDate, 'yyyy-MM-dd');
      let url = `${reportname}?VchType=RP-RAW&VchNo=${VchNo}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}&FromDate=${FromDate}&VehNo=${Vehicle}&GpNo=${GpNo}&CmpId=${this.auth.cmpId()}`;
      this.com.viewReport(url);
      }
      printGrn(reportname:any , VchNo :any , GpNo:any , Vehicle:any, FromDate:any  ) {
        debugger;
        let url = `${reportname}?VchType=RP-RAW&VchNo=${VchNo}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}&FromDate=${FromDate}&VehNo=${Vehicle}&GpNo=${GpNo}&CmpId=${this.auth.cmpId()}`;
        this.com.viewReport(url);
        }
      
    




  editVouchers(VCHNO: any, VCHTYPE: any): void {
    this.onClickRefresh();
    this.isShow = true;
    this.editMode = true;
    this.readonly = false;
    this.PurchaseCorrectionForm.get('grnDate').enable();

    const obj = {
      vchNo: VCHNO,
    };

    this.apiService
      .getDataById('Purchase/GetEditPurchaseCorrection', obj)
      .subscribe((data) => {
        this.togglePages();
 debugger;
        const d =  data.GI[0];

        this.PurchaseCorrectionForm.get('vchType')?.patchValue(d.VCHTYPE);
        this.PurchaseCorrectionForm.get('location')?.patchValue(d.LOCATION);
        this.PurchaseCorrectionForm.get('vchNo')?.patchValue(d.VCHNO);

        this.PurchaseCorrectionForm.get('gpino')?.patchValue(d.GPNO);
        this.PurchaseCorrectionForm.get('vehicleNo').patchValue(d.VEHICLENO);
        this.PurchaseCorrectionForm.get('biltyNo').patchValue(d.BILLTYNO);
        this.PurchaseCorrectionForm.get('minWeight').patchValue(d.WTYPE);
        this.PurchaseCorrectionForm.get('chkMinWeight').patchValue(d.MINIWT);
        this.isMinWtFinal=d.MINIWT;;

          this.PurchaseCorrectionForm
            .get('grnDate')
            ?.patchValue(
              new Date(
                d.VCHDATE.split('/')[2],
                d.VCHDATE.split('/')[1] - 1,
                d.VCHDATE.split('/')[0]
              )
            );

        this.PurchaseCorrectionForm.get('freight').patchValue(d.FREIGHT);
        this.PurchaseCorrectionForm.get('freightDD').patchValue(d.FREIGHTTYPE);

        const partyMain = d.PARTYSUB.substring(0, 9);
        this.getPartySub({CODE: partyMain});
        this.getTblSubParty({CODE: d.PARTYSUB});

        this.PurchaseCorrectionForm.get('partyMain').patchValue(partyMain);
        this.PurchaseCorrectionForm.get('partySub').patchValue(d.PARTYSUB);
        this.PurchaseCorrectionForm.get('subParty').patchValue(d.SUBPARTY);
        this.PurchaseCorrectionForm.get('remarks').patchValue(d.REMARKS);
        this.PurchaseCorrectionForm.get('godown').patchValue(d.GODOWNID);

        this.PurchaseCorrectionForm.get('retStat').patchValue(d.RETSTAT);
        this.PurchaseCorrectionForm.get('bagsTypeDDS1').patchValue(d.CMB1);
        this.PurchaseCorrectionForm.get('bagsTypeDDS2').patchValue(d.CMB2);
        this.PurchaseCorrectionForm.get('bagsTypeDDS3').patchValue(d.CMB3);
        this.PurchaseCorrectionForm.get('bag1').patchValue(d.BAGS1);
        this.PurchaseCorrectionForm.get('bag2').patchValue(d.BAGS2);
        this.PurchaseCorrectionForm.get('bag3').patchValue(d.BAGS3);
        this.PurchaseCorrectionForm.get('bagWt1').patchValue(d.DED1);
        this.PurchaseCorrectionForm.get('bagWt2').patchValue(d.DED2);
        this.PurchaseCorrectionForm.get('bagWt3').patchValue(d.DED3);
        this.PurchaseCorrectionForm.get('grossWt2').patchValue(d.FIRSTWEIGHT);
        this.PurchaseCorrectionForm.get('tareWt').patchValue(d.SECWEIGHT);
        this.PurchaseCorrectionForm.get('labParty').patchValue(d.LABPARTY);
        this.PurchaseCorrectionForm.get('labStk').patchValue(d.LABSTOCK);

        this.PurchaseCorrectionForm.get('payableWt').patchValue(d.PAYABLEWT1);
        this.PurchaseCorrectionForm.get('stockWt').patchValue(d.PAYABLEWT);

        


        this.PurchaseCorrectionForm.get('firstWt').patchValue(d.FIRSTWEIGHT);
        //this.PurchaseCorrectionForm.get('secWt').patchValue(d.SECWEIGHT);

        const netWt = d.FIRSTWEIGHT - d.SECWEIGHT
        this.PurchaseCorrectionForm.get('netWt').patchValue(netWt);

        data.GI.forEach((item: any) => {

          let form: any = [];
          form.code = item.ITEMSUBCODE.substring(0, 9);
          form.sub = item.ITEMSUBCODE.substring(9, 14);
          form.sno = this.detailsList.length + 1;
          form.itemName = item.ITEMNAME;
          form.bags = item.SBAGS;
          form.bagsType = item.BAGSTYPE;
          form.net = item.SQTY;
          form.rate = item.RATE;
          form.standardWt = item.standardWt;
          form.uom = item.UOMNAME;
          form.godown = item.GODOWNNAME;
          form.godownid = this.godownList.find((i) => i.GODOWNNAME === item.GODOWNNAME).GODOWNID;
          form.gross = item.GROSS;
          form.tare = item.TARE;
          form.id = item.ID;
          form.poNo = item.PONO;

          this.detailsList.push(form);
        });

        this.LabDeductionList = data.LabDed;

      });
  }

  deleteVouchers(VCHNO: any, VCHTYPE: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {
      const obj = {
        vchNo: VCHNO,
        vchType: VCHTYPE
      };

      this.apiService.deleteData('Purchase/DelPurchaseCorrection', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.tostr.success('Delete Successfully');
            this.getVouchersList();
            //this.getVchNo();
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


  onClickRefresh() {
    this.isShow = false;
    this.resetForm();
    this.detailsList = [];
    this.btnAdd = 'Add'
    this.LabDeductionList = [];
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

    this.PurchaseCorrectionForm.get('grnDate').enable();
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

    const standardWt = parseFloat(this.PurchaseCorrectionForm.get('standardWt')?.value);
    const bags = parseFloat(this.PurchaseCorrectionForm.get('bags')?.value);
    if (standardWt > 0) {

      const result = standardWt * bags;

      this.PurchaseCorrectionForm.get('gross').setValue(result + 500);
      this.PurchaseCorrectionForm.get('tare').setValue(500);

      const net = (result + 500) - 500

      this.PurchaseCorrectionForm.get('net').patchValue('');
      this.PurchaseCorrectionForm.get('biltyWt').patchValue('');
      if (!isNaN(net)) {
        this.PurchaseCorrectionForm.get('net').patchValue(net);
        this.PurchaseCorrectionForm.get('biltyWt').patchValue(net);
      }
    }
    else {
      const gross = parseFloat(this.PurchaseCorrectionForm.get('gross')?.value);
      const tare = parseFloat(this.PurchaseCorrectionForm.get('tare')?.value);
      if (isNaN(gross) || isNaN(tare)) {
        this.PurchaseCorrectionForm.get('net').patchValue('');
        this.PurchaseCorrectionForm.get('biltyWt').patchValue('');
      } else {
        this.PurchaseCorrectionForm.get('net').patchValue(gross - tare);
        this.PurchaseCorrectionForm.get('biltyWt').patchValue(gross - tare);
      }
    }


    const gross = parseFloat(this.PurchaseCorrectionForm.get('gross')?.value);
    if(!isNaN(gross)){
      this.PurchaseCorrectionForm.get('grossWt').setValue(gross);
    }
    else{
      this.PurchaseCorrectionForm.get('grossWt').setValue('');
    }

  }


  rowHighLight(event: any) {
    // Get the clicked row
    const clickedRow = event.target.closest('tr');
  
    // Add 'HighLightRow' class to all td elements in the clicked row
    const tds = clickedRow.querySelectorAll('td');
    tds.forEach(td => {
      td.classList.add('HighLightRow');
    });
  
    // Remove 'HighLightRow' class from other rows
    const allRows = document.querySelectorAll('tr');
    allRows.forEach(row => {
      if (row !== clickedRow) {
        const otherRowTds = row.querySelectorAll('td');
        otherRowTds.forEach(td => {
          td.classList.remove('HighLightRow');
        });
      }
    });
  }

}
