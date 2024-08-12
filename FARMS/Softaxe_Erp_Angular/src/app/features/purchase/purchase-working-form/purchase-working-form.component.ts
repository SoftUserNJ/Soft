import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild, numberAttribute } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { DebugMode } from '@devexpress/analytics-core/analytics-internal';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { environment } from '../../../../environment/environmemt';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';


declare const $: any;




@Component({
  selector: 'app-purchase-working-form',
  templateUrl: './purchase-working-form.component.html',
  styleUrls: ['./purchase-working-form.component.css']
})
export class PurchaseWorkingFormComponent {
  
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
  vchType: any = 'RP-Raw';
  grnNo: number = 0;
  vchNo: number = 0;
  billed: boolean = false;
  poDetailFieldsDisabled: boolean = false;

  uiLocId:any;

  vchTypes: any = [];
  locationList: any = [];
  
  partyMainList: any = [];
  partySubList: any = [];
  
  itemMainList: any = [];
  itemSubList: any = [];
  
  detailsList: any = [];
  pendingWorkList: any = [];
  bagsTypeList: any = [];

  UomList: any[] = [];

  brokerList: any[] = [];
  isShow = false;
  isShowPage: boolean = true;
  PurchaseWorkingForm!: FormGroup;
  editModeSno: boolean = false;
  editMode: boolean = true;
  readonly: boolean = true;

  btnAdd:string = 'Add';
  editSno: any = '';


  ngOnInit() {
    this.formInit();
    this.getVchNo();
    this.getVchType();
    this.getPartyMain();
    this.getBagsType();
    this.getLocation();
    this.getItemMain();
    this.getUomList();
    this.getBroker();
    this.poDetailFieldsDisabled=true;
  }

  formInit() {
    this.PurchaseWorkingForm = this.fb.group({
      vchType: ['Rp-Raw'],
      vchNo: [''],
      gpNO: [''],
      location: [undefined],
      grnDate: [new Date()],
      vehicleNo: [''],
      biltyNo: [''],
      biltyWt: [''],
      transporter: [undefined],
      partyMain: [undefined],
      partySub: [undefined],
      itemMain: [undefined],
      itemSub: [undefined],
      broker: [undefined],
      pono: [''],
      poNo2: [''],
      rate: [''],
      poRate2: [''],
      poBalQty1: [''],
      poBalQty2: [''],
      poShortQty1: [''],
      poShortQty2: [''],
      partyBillNo: [''],
      partyQty: [''],
      ourQty: [''],
      qtyDiff: [''],
      commission: [''],
      comuom: [''],
      comamount: [''],
      uom: [''],
      amount1: [''],
      amount2: [''],
      amount3: [''],

      ded1: [''],
      ded2: [''],
      ded3: [''],


      bags: [''],
      bagsType: ['S'],
      etype: ['S'],
      whtax: [''],
      whtaxamount:[''],
      freight: [''],
      freightNo: [''],
      freightType: ['S'],
      frgRate: [''],
      salesTaxRate: [''],
      salesTax: [''],

      kantaExps: [''],
      furtherTax: [''],
      stockQty: [''],
      otherExps: [''],
      proteine: [''],
      payableQty: [''],
      payableAmt: [''],
      netPayableAmt: [''],
      remarks: [''],

      gross: [''],
      tare: [''],
      bagsWt: [''],
      net: [''],

      bagsType1: [undefined],
      bagsType2: [undefined],
      bagsType3: [undefined],
      bags1: [''],
      bags2: [''],
      bags3: [''],
      rate1: [''],
      rate2: [''],
      rate3: [''],
      id: [''],
    });
  }

  resetForm() {

    this.readonly = true;

    this.PurchaseWorkingForm.get('vchType')?.patchValue('RP-Raw');
    this.PurchaseWorkingForm.get('vchNo')?.patchValue('');
    this.PurchaseWorkingForm.get('location')?.patchValue(this.uiLocId);
    this.PurchaseWorkingForm.get('grnDate')?.patchValue(new Date());
    this.PurchaseWorkingForm.get('vehicleNo')?.patchValue('');
    this.PurchaseWorkingForm.get('biltyNo')?.patchValue('');
    this.PurchaseWorkingForm.get('biltyWt')?.patchValue('');
    this.PurchaseWorkingForm.get('transporter')?.patchValue(undefined);
    this.PurchaseWorkingForm.get('partyMain')?.patchValue(undefined);
    this.PurchaseWorkingForm.get('partySub')?.patchValue(undefined);
    this.PurchaseWorkingForm.get('itemMain')?.patchValue(undefined);
    this.PurchaseWorkingForm.get('itemSub')?.patchValue(undefined);
    this.PurchaseWorkingForm.get('broker').patchValue(undefined);
    this.PurchaseWorkingForm.get('pono')?.patchValue('');
    this.PurchaseWorkingForm.get('poNo2')?.patchValue('');
    this.PurchaseWorkingForm.get('rate')?.patchValue('');
    this.PurchaseWorkingForm.get('poRate2')?.patchValue('');
    this.PurchaseWorkingForm.get('poBalQty1')?.patchValue('');
    this.PurchaseWorkingForm.get('poBalQty2')?.patchValue('');
    this.PurchaseWorkingForm.get('poShortQty1')?.patchValue('');
    this.PurchaseWorkingForm.get('poShortQty2')?.patchValue('');
    this.PurchaseWorkingForm.get('partyBillNo')?.patchValue('');
    this.PurchaseWorkingForm.get('partyQty')?.patchValue('');
    this.PurchaseWorkingForm.get('ourQty')?.patchValue('');
    this.PurchaseWorkingForm.get('qtyDiff')?.patchValue('');
    this.PurchaseWorkingForm.get('commission')?.patchValue('');
    this.PurchaseWorkingForm.get('comUom')?.patchValue('');
    this.PurchaseWorkingForm.get('bags')?.patchValue('');
    this.PurchaseWorkingForm.get('bagsType')?.patchValue('S');
    this.PurchaseWorkingForm.get('etype')?.patchValue('S');
    this.PurchaseWorkingForm.get('whTax')?.patchValue('');
    this.PurchaseWorkingForm.get('whtaxamount')?.patchValue('');
    
    this.PurchaseWorkingForm.get('freight')?.patchValue('');
    this.PurchaseWorkingForm.get('freightNo')?.patchValue('');
    this.PurchaseWorkingForm.get('freightType')?.patchValue('S');
    this.PurchaseWorkingForm.get('frgRate')?.patchValue('');
    this.PurchaseWorkingForm.get('salesTax')?.patchValue('');
    this.PurchaseWorkingForm.get('kantaExps')?.patchValue('');
    this.PurchaseWorkingForm.get('furtherTax')?.patchValue('');
    this.PurchaseWorkingForm.get('stockQty')?.patchValue('');
    this.PurchaseWorkingForm.get('otherExps')?.patchValue('');
    this.PurchaseWorkingForm.get('proteine')?.patchValue('');
    this.PurchaseWorkingForm.get('payableQty')?.patchValue('');
    this.PurchaseWorkingForm.get('payableAmt')?.patchValue('');
    this.PurchaseWorkingForm.get('netPayableAmt')?.patchValue('');
    this.PurchaseWorkingForm.get('remarks')?.patchValue('');
    this.PurchaseWorkingForm.get('gross')?.patchValue('');
    this.PurchaseWorkingForm.get('tare')?.patchValue('');
    this.PurchaseWorkingForm.get('bagsWt')?.patchValue('');
    this.PurchaseWorkingForm.get('net')?.patchValue('');
    this.PurchaseWorkingForm.get('bagsType1')?.patchValue(undefined);
    this.PurchaseWorkingForm.get('bagsType2')?.patchValue(undefined);
    this.PurchaseWorkingForm.get('bagsType3')?.patchValue(undefined);
    this.PurchaseWorkingForm.get('bags1')?.patchValue('');
    this.PurchaseWorkingForm.get('bags2')?.patchValue('');
    this.PurchaseWorkingForm.get('bags3')?.patchValue('');
    this.PurchaseWorkingForm.get('rate1')?.patchValue('');
    this.PurchaseWorkingForm.get('rate2')?.patchValue('');
    this.PurchaseWorkingForm.get('rate3')?.patchValue('');

    this.itemSubList = [];
    this.partySubList = [];
    this.PurchaseWorkingForm.get('grnDate').disable();
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
    this.PurchaseWorkingForm.get('partySub')?.patchValue(undefined);
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

  async getBagsType() {
    const result = await this.apiService
    .getData('Common/GetPurchaseBagsType')
    .toPromise();

    this.bagsTypeList = result;
  }

  onItemClear(){
    this.itemSubList = [];
    this.PurchaseWorkingForm.get('itemSub')?.patchValue(undefined);
  }

  async getLocation() {

    const result = await this.apiService
      .getData('Common/LocationWithLoc')
      .toPromise();

    this.locationList = result

    if (this.locationList.length > 0) {
      this.uiLocId = this.locationList[0].LocID
      this.PurchaseWorkingForm.get('location')?.patchValue(this.locationList[0].LocID);
    } else {
      this.uiLocId = this.locationList.LocID
      this.PurchaseWorkingForm.get('location')?.patchValue(this.locationList.LocID);
    }

    this.getPendingWorkList();
    this.getVouchersList();

  }

  getUomList() {
    this.apiService.getData('Common/GetUom').subscribe((data) => {
      this.UomList = data;
    });
  }

  getBroker() {
    // this.apiService.getDataById('Common/GetDirectL4L5CodeNamesByL4Tag', {l4Tag: 'BR'}).subscribe((data)=>{
    this.apiService
      .getDataById('Common/GetDirectL4L5CodeNamesByL4Tag', { l4Tag: 'D' })
      .subscribe((data) => {
        this.brokerList = data;
      });
  }
  async getPendingWorkList() {

    const result = await this.apiService
    .getDataById('Purchase/GetPendingWorkList', { locIdUnit: this.uiLocId })
    .toPromise();
    this.pendingWorkList = result;
  }

  async getPendingWorkDetail(event:any , Status:any) {
 //Shahzad
 debugger;
    var obj = {
      vchNo: event.VCHNO, 
      vchType: 'RP-RAW', 
      workDone: Status,
      locIdUnit: this.uiLocId,
    }

    const result = await this.apiService
    .getDataById('Purchase/GetPendingWorkDetail', obj)
    .toPromise();
    if (Status==true)
      {
        this.togglePages();
      }
      let Gross=0;
      let Tare=0;
      let ExpWt=0;
      let PartyBags=0;
      let PartyWeight=0;
      let FirstWeight=0;
      let SecWeight=0;
      result.forEach((item: any) => {
        Gross+=item.Gross;
        Tare+=item.Tare;
        ExpWt+=item.ExpWt;
        PartyBags+=item.SBAGS;
        PartyWeight+=item.SQTY;
        FirstWeight+=item.FirstWeight;
        SecWeight+=item.SecWeight;
      });

      
    
   
    const d = result[0];
    this.vchNo = d.VchNo;
    debugger;
    this.PurchaseWorkingForm.get('vchType')?.patchValue('RP-Raw');
   
    //this.PurchaseWorkingForm.get('location')?.patchValue(this.auth.locId());
    const grnDate = 
    new Date(
      d.VCHDATE.split('/')[2],
      d.VCHDATE.split('/')[1] - 1,
      d.VCHDATE.split('/')[0]
    );
    debugger;
    
   


    this.PurchaseWorkingForm.get('rate1').disable();
    this.PurchaseWorkingForm.get('rate2').disable();
    this.PurchaseWorkingForm.get('rate3').disable();
    
    this.PurchaseWorkingForm.get('vchNo').patchValue(d.VCHNO);
    this.PurchaseWorkingForm.get('gpNO')?.patchValue(d.GPNO);
    this.PurchaseWorkingForm.get('partyQty').patchValue(Gross-Tare);
    this.PurchaseWorkingForm.get('ourQty').patchValue(FirstWeight-SecWeight);
    this.PurchaseWorkingForm.get('qtyDiff').patchValue( (Gross-Tare)   - (FirstWeight-SecWeight) );
    this.PurchaseWorkingForm.get('partyBillNo').patchValue(d.SubName);
    
    
    



    this.PurchaseWorkingForm.get('grnDate')?.patchValue(grnDate);
    this.PurchaseWorkingForm.get('vehicleNo')?.patchValue(d.VehicleNo);
    this.PurchaseWorkingForm.get('biltyNo').patchValue(d.BilltyNo);
    this.PurchaseWorkingForm.get('biltyWt')?.patchValue('N/A');
    this.PurchaseWorkingForm.get('transporter')?.patchValue(d.DriverName);
    this.PurchaseWorkingForm.get('partyMain')?.patchValue(d.Mcode.substring(0, 9));

    this.getPartySub({CODE: d.Mcode.substring(0, 9)});
    this.PurchaseWorkingForm.get('partySub')?.patchValue(d.Mcode);

    this.PurchaseWorkingForm.get('broker').patchValue(d.BrokeCode);

    this.PurchaseWorkingForm.get('bags')?.patchValue(d.Bags);
    this.PurchaseWorkingForm.get('bagsType')?.patchValue(d.BagsType);

    this.PurchaseWorkingForm.get('freight').patchValue(d.Freight);
    this.PurchaseWorkingForm.get('freightType')?.patchValue(d.FreightType);
    this.PurchaseWorkingForm.get('frgRate')?.patchValue('0');
 
    this.PurchaseWorkingForm.get('kantaExps')?.patchValue(d.ExpWt);
    this.PurchaseWorkingForm.get('furtherTax')?.patchValue(d.FurtherTax);
    this.PurchaseWorkingForm.get('otherExps')?.patchValue(d.OtherExP);

    this.PurchaseWorkingForm.get('remarks')?.patchValue(d.Remarks);
    this.PurchaseWorkingForm.get('gross')?.patchValue(d.Gross);
    this.PurchaseWorkingForm.get('tare')?.patchValue(d.Tare);
    this.PurchaseWorkingForm.get('bagsWt')?.patchValue(d.BAGSDED);
    this.PurchaseWorkingForm.get('net')?.patchValue(d.NetAmount);

    this.PurchaseWorkingForm.get('bagsType1').patchValue(d.Cmb1);
    this.PurchaseWorkingForm.get('bagsType2').patchValue(d.Cmb2);
    this.PurchaseWorkingForm.get('bagsType3').patchValue(d.Cmb3);





    this.PurchaseWorkingForm.get('bags1')?.patchValue(d.Bags1);
    this.PurchaseWorkingForm.get('bags2')?.patchValue(d.Bags2);
    this.PurchaseWorkingForm.get('bags3')?.patchValue(d.Bags3);
   debugger;
    this.PurchaseWorkingForm.get('ded1').patchValue(d.Ded1);
    this.PurchaseWorkingForm.get('ded2').patchValue(d.Ded2);
    this.PurchaseWorkingForm.get('ded3').patchValue(d.Ded3);


    this.PurchaseWorkingForm.get('rate1').patchValue(d.BG1);
    this.PurchaseWorkingForm.get('rate2').patchValue(d.BG2);
    this.PurchaseWorkingForm.get('rate3').patchValue(d.BG3);

    this.PurchaseWorkingForm.get('amount1').patchValue( Math.round(d.Bags1*d.BG1));
    this.PurchaseWorkingForm.get('amount2').patchValue(Math.round(d.Bags2*d.BG2));
    this.PurchaseWorkingForm.get('amount3').patchValue(Math.round(d.Bags3*d.BG3));
   
  
    this.PurchaseWorkingForm.get('proteine').patchValue(d.Proteine);
    
   


    const updatedResult = result.map((item, index) => {
      debugger;
      if ( item.BagsType !="P")
        {
          item.Debit = item.Debit + Math.round(d.Bags1 * d.BG1) + Math.round(d.Bags2 * d.BG2) + Math.round(d.Bags3 * d.BG3);
        }

        item.NetAmount  =   (item.Debit+ item.SalesTax)-item.Fed;
      
      item.sno = index + 1;
      return item;
     });
     


    this.detailsList = result;



    this.onClickNew();
  }

  async getVchNo() {
    // const result = await this.apiService
    // .getDataById('Purchase/GetGatePassInwardEntryVchNo', {vchType: 'RP-Raw'})
    // .toPromise();
    // this.PurchaseWorkingForm.get('vchNo').patchValue(result[0].VCHNO)
  }

  async getVchType() {
    // const result = await this.apiService
    // .getData('Common/GetTypes')
    // .toPromise();

  }

  async getVouchersList() {

    const obj = {
      fromDate: this.dp.transform(this.fromDate, 'yyyy/MM/dd'),
      toDate: this.dp.transform(this.toDate, 'yyyy/MM/dd'),
      vchType: this.vchType,
      grnNo: this.grnNo,
      locIdUnit: this.uiLocId,
    };

    const result = await this.apiService
    .getDataById('Purchase/GetPurchaseWorking', obj)
    .toPromise();

    this.voucherList = result
  }

  onAdd() {

    let form = this.PurchaseWorkingForm.value;

    const itemDoubleCheck = this.detailsList.find((row) => row.itemSub == form.itemSub);
    if(itemDoubleCheck){
      this.tostr.warning('Item already in table. Select other item....!');
          return;
    }

    if (form.itemMain === null || form.itemMain === undefined || form.itemMain === "") {
      this.tostr.warning('Select Item Main....!');
      return;
    }

    if (form.itemSub === null || form.itemSub === undefined || form.itemSub === "") {
      this.tostr.warning('Select Item Sub....!');
      return;
    }

    if (form.uom === null || form.uom === undefined || form.uom === "") {
      this.tostr.warning('Select UOM....!');
      return;
    }



    debugger;
    // let itemName = this.itemSubList.find((i) => i.CODE === form.itemSub);
   
    // form.code = form.itemSub.substring(0, 9);
    // form.sub = form.itemSub.substring(9, 14);
    // form.itemName = itemName.NAME;

    if (this.editModeSno) {
      const index = this.detailsList.findIndex(
        (row) => row.sno === this.editSno
      );
      if (index !== -1) {

        debugger;
        this.detailsList[index].BagsType = this.PurchaseWorkingForm.get('bagsType').value;
        this.detailsList[index].PONO = this.PurchaseWorkingForm.get('pono').value ?? 0;
        this.detailsList[index].Rate = this.PurchaseWorkingForm.get('rate').value ?? 0;
        this.detailsList[index].Debit =  this.PurchaseWorkingForm.get('payableAmt').value ?? 0;
        this.detailsList[index].UOM =  String(this.PurchaseWorkingForm.get('uom').value) ;

       
        this.detailsList[index].Brokercom =  this.PurchaseWorkingForm.get('commission').value ?? 0;
        this.detailsList[index].Brokercomtype = this.PurchaseWorkingForm.get('comuom').value;
        this.detailsList[index].Commission =  this.PurchaseWorkingForm.get('comamount').value ?? 0;
        this.detailsList[index].SalesTaxrate =  this.PurchaseWorkingForm.get('salesTaxRate').value ?? 0;


        this.detailsList[index].Fedrate =  this.PurchaseWorkingForm.get('whtax').value || 0;
        this.detailsList[index].Fed = this.PurchaseWorkingForm.get('whtaxamount').value || 0;



        this.detailsList[index].SalesTax =  this.PurchaseWorkingForm.get('salesTax').value ?? 0;
        this.detailsList[index].NetAmount =  this.PurchaseWorkingForm.get('netPayableAmt').value ?? 0;
      
        this.editModeSno = false;
        this.editSno = '';
        this.btnAdd = 'Add'
        //this.resetForm();
        return;
      }
    }

    // form.sno = this.detailsList.length + 1;
    // this.detailsList.push(form);
    //this.resetForm();
  }

  editItem(row: any) {


    debugger;
    this.btnAdd = 'Update'

    this.editModeSno = true;
    this.editSno = row.sno;

    const code = row.DMCode + row.Code;
    this.PurchaseWorkingForm.get('itemMain').patchValue(row.DMCode);
    this.getItemSub({CODE: row.DMCode});
    this.PurchaseWorkingForm.get('itemSub').patchValue(code);

    this.PurchaseWorkingForm.get('bags').patchValue(row.Bags);
    this.PurchaseWorkingForm.get('bagsType').patchValue(row.BagsType);


    this.PurchaseWorkingForm.get('gross').patchValue(row.gross);
    this.PurchaseWorkingForm.get('tare').patchValue(row.tare);

    this.PurchaseWorkingForm.get('net')?.patchValue(row.net);
    
    this.PurchaseWorkingForm.get('pono').patchValue(row.PONO);
    this.PurchaseWorkingForm.get('rate').patchValue(row.Rate);
    this.PurchaseWorkingForm.get('commission').patchValue(row.Brokercom);
    this.PurchaseWorkingForm.get('comuom').patchValue(row.Brokercomtype);
    this.PurchaseWorkingForm.get('comamount').patchValue(row.Commission);

  
    this.PurchaseWorkingForm.get('whtax').patchValue(row.Fedrate);
    this.PurchaseWorkingForm.get('whtaxamount').patchValue(row.Fed);
    this.PurchaseWorkingForm.get('salesTaxRate').patchValue(row.SalesTaxrate);
    this.PurchaseWorkingForm.get('salesTax').patchValue(row.SalesTax);



    this.PurchaseWorkingForm.get('uom').patchValue( Number( row.UOM));

    
    this.PurchaseWorkingForm.get('standardWt')?.patchValue(row.standardWt);
    this.PurchaseWorkingForm.get('stockQty').patchValue(row.Qty);
    this.PurchaseWorkingForm.get('payableQty').patchValue(row.PayableWT1);
 
  
    this.PurchaseWorkingForm.get('payableAmt').patchValue(row.Debit);

    this.PurchaseWorkingForm.get('netPayableAmt').patchValue(row.NetAmount);
    this.PurchaseWorkingForm.get('id').patchValue(row.Id)
    
      this.getPoDetails();
      this.Calrate();
    // this.PurchaseWorkingForm.get('payableQty')?.patchValue(row.PayableWT1);




    //const uom = this.uomList.find((i) => i.name === row.uom).id;
    //this.PurchaseWorkingForm.get('uom')?.patchValue(uom);
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


  bagsrate()
  {
    debugger;
    let bagstype=  this.PurchaseWorkingForm.get('bagsType').value;
    if (bagstype=="P")
      {
    let bags1 =this.PurchaseWorkingForm.get('bags1').value ?? 0;
    let bags2 =this.PurchaseWorkingForm.get('bags2').value ?? 0;
    let bags3 =this.PurchaseWorkingForm.get('bags3').value ?? 0;

    let rate1 =this.PurchaseWorkingForm.get('rate1').value ?? 0;
    let rate2 =this.PurchaseWorkingForm.get('rate2').value ?? 0;
    let rate3 =this.PurchaseWorkingForm.get('rate3').value ?? 0;


  
        this.PurchaseWorkingForm.get('amount1').patchValue(Math.round( bags1*rate1) );
        this.PurchaseWorkingForm.get('amount2').patchValue(Math.round( bags2*rate2));
        this.PurchaseWorkingForm.get('amount3').patchValue(Math.round( bags3*rate3));

      }

  }


  Calrate()
  {

    debugger;
    let Gamount = 0;
    let Amount = 100;
    let AmountWithComm = 0;
    let SalesTaxAmt = 0;
    let fedamt = 0;
    let Rate = 0;
    let MPayablewt1 = 0;
    let Payablewt1 = 0;
    let Commission = 0;
    let BrokerComm = 0;
    let BrokerCommUom = "";
    let bags1 =this.PurchaseWorkingForm.get('bags1').value || 0;
    let bags2 =this.PurchaseWorkingForm.get('bags2').value || 0;
    let bags3 =this.PurchaseWorkingForm.get('bags3').value || 0;
    let proteine =this.PurchaseWorkingForm.get('proteine').value || 0;

    
    let bagstype=  this.PurchaseWorkingForm.get('bagsType').value;
    this.PurchaseWorkingForm.get('rate1').disable();
    this.PurchaseWorkingForm.get('rate2').disable();
    this.PurchaseWorkingForm.get('rate3').disable();
     if (bagstype=="P")
      {   
       this.PurchaseWorkingForm.get('rate1').enable();
       this.PurchaseWorkingForm.get('rate2').enable();
       this.PurchaseWorkingForm.get('rate3').enable();
      }


       let DivUOM = this.UomList.find((i) => i.ID === this.PurchaseWorkingForm.get('uom').value).DIVUOM;
       let UOM1 = this.UomList.find((i) => i.ID === this.PurchaseWorkingForm.get('uom').value).UOM;
        MPayablewt1 =   this.PurchaseWorkingForm.get('payableQty').value || 0;
  
        Rate = this.PurchaseWorkingForm.get('rate').value || 0;
        BrokerComm = this.PurchaseWorkingForm.get('commission').value || 0;
        BrokerCommUom = this.PurchaseWorkingForm.get('comuom').value;


        debugger;
        if(UOM1=="Protien" && proteine >0 )
          {  
           Gamount =  Math.round(  ( MPayablewt1 * Rate * proteine) / DivUOM)  ;

          }
          else
          {
            Gamount =  Math.round(  ( MPayablewt1 * Rate) / DivUOM)  ;
          }
        Commission = Math.round(this.getCommAmount(BrokerComm, BrokerCommUom, this.PurchaseWorkingForm.get('bags').value ?? 0, Gamount, MPayablewt1));
        this.PurchaseWorkingForm.get('comamount').patchValue(Commission);

        
         SalesTaxAmt = Math.round(this.getTaxAmount(Gamount, this.PurchaseWorkingForm.get('salesTaxRate').value ?? 0));
         this.PurchaseWorkingForm.get('salesTax').patchValue(SalesTaxAmt);

         fedamt = Math.round(this.getTaxAmount(Commission, this.PurchaseWorkingForm.get('whtax').value ?? 0));
         this.PurchaseWorkingForm.get('whtaxamount').patchValue(fedamt);
         
         AmountWithComm = Gamount + Commission;


         const Bagsrate1 = this.getBagsRate(AmountWithComm, MPayablewt1, bags1, this.PurchaseWorkingForm.get('ded1').value ?? 0,bagstype);
         const Bagsrate2 = this.getBagsRate(AmountWithComm, MPayablewt1,bags2, this.PurchaseWorkingForm.get('ded2').value ?? 0,  bagstype);
         const Bagsrate3 = this.getBagsRate(AmountWithComm, MPayablewt1,bags3, this.PurchaseWorkingForm.get('ded3').value ?? 0,  bagstype);

        const Bagsamt1 = Math.round(bags1 * Bagsrate1);
        const Bagsamt2 = Math.round(bags2 * Bagsrate2);
        const Bagsamt3 = Math.round(bags3 * Bagsrate3);

        this.PurchaseWorkingForm.get('rate1').patchValue(Bagsrate1);
        this.PurchaseWorkingForm.get('rate2').patchValue(Bagsrate2);
        this.PurchaseWorkingForm.get('rate3').patchValue(Bagsrate3);


        this.PurchaseWorkingForm.get('amount1').patchValue(Bagsamt1);
        this.PurchaseWorkingForm.get('amount2').patchValue(Bagsamt2);
        this.PurchaseWorkingForm.get('amount3').patchValue(Bagsamt3);

      // Amount = Gamount - (Bagsamt1 + Bagsamt2 + Bagsamt3);
      Amount = Gamount ;
       this.PurchaseWorkingForm.get('payableAmt').patchValue(Amount);

       this.PurchaseWorkingForm.get('netPayableAmt').patchValue((Amount+SalesTaxAmt)-fedamt);
       

  }

  getCommAmount(mComm: number, mCommType: string, mBags: number, PayableValue: number, mPayableQty: number): number {
    let mCommAmount = 0;

    if (mComm > 0) {
        switch (mCommType.trim()) {
            case 'Amount %':
                mCommAmount = Math.round((PayableValue / 100) * mComm);
                break;
            case 'Bags':
                mCommAmount = Math.round(mBags * mComm);
                break;
            case '40Kgs':
                mCommAmount = Math.round((mPayableQty / 40) * mComm);
                break;
            case 'Kgs':
                mCommAmount = Math.round(mPayableQty * mComm);
                break;
        }
    }
    return mCommAmount;
  }
  getTaxAmount(amount: number, taxRate: number): number {
    let taxAmount = 0;

    if (amount > 0 && taxRate > 0) {
        taxAmount = Math.round((amount / 100) * taxRate);
    }

    return taxAmount;
  }
   getBagsRate(payableAmount: number, payableQty: number, selectedBags: number, bagsDed: number, bagsType: string): number {
    if (payableAmount !== 0 && payableQty !== 0 && selectedBags !== 0) {
        let mBagswt: number;
        let mBagrate: number;

        mBagswt = selectedBags * bagsDed;
        mBagrate = Math.round((payableAmount / payableQty) * mBagswt);
        mBagrate = Math.round((mBagrate / selectedBags) * 100) / 100; // Round to 2 decimal places

        if (bagsType !== "P") {
            return mBagrate;
        } else {
            return 0;
        }
    }
    return 0;
  }


  onClickPrint(tag: any ,reportname:any) {
    debugger
    const vchNo = this.PurchaseWorkingForm.get('vchNo').value;
    const GpNo = this.PurchaseWorkingForm.get('gpNO').value;
    const vehicleNo = this.PurchaseWorkingForm.get('vehicleNo').value;
    
    const vchType = "RP-RAW";
    const Locid = this.PurchaseWorkingForm.get('location').value;
    const vchDate = this.dp.transform(
      this.PurchaseWorkingForm.get('grnDate').value,
      'yyyy/MM/dd'
    );
    let url = '';

    if (tag == 'Voucher') {
   
      url = `${reportname}?DateFrom=${vchDate}&DateTo=${vchDate}&VchType=${vchType}&VchNoFrom=${vchNo}&VchNoTo=${vchNo}&CompId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${Locid}`;
    }
     else
     {
      url = `${reportname}?VchType=${vchType}&VchNo=${vchNo}&FinId=${this.auth.finId()}&LocId=${Locid}&FromDate=${vchDate}&VehNo=${vehicleNo}&GpNo=${GpNo}&CmpId=${this.auth.cmpId()}`;
     }
    this.com.viewReport(url);
  }



  printvch(reportname:any , VchNo :any , FromDate:any , Locid:any  ) {
    
    const vchType = "RP-RAW";
    debugger;
    let url = `${reportname}?DateFrom=${FromDate}&DateTo=${FromDate}&VchType=${vchType}&VchNoFrom=${VchNo}&VchNoTo=${VchNo}&CompId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${Locid}`;

    this.com.viewReport(url);
    }
  


    async getPoDetails() {
     debugger;
      const party = this.PurchaseWorkingForm.get('partySub').value;
      const item = this.PurchaseWorkingForm.get('itemSub').value;
      const pono = this.PurchaseWorkingForm.get('pono').value || 0;
      
      if (party == '' || party == null) {
       
        return;
      }
  
      if (item == '' || item == null) {
    
        return;
      }
      try {
       // this.com.showLoader();

       
  




        const obj = {
          party: party,
          item: item,
          TransDate : this.dp.transform(
            this.PurchaseWorkingForm.get('grnDate')?.value,
            'yyyy-MM-dd'
          ),
          Vchno :this.PurchaseWorkingForm.get('id').value || 0 ,
          Pono:pono
        };
  
        const result = await this.apiService
          .getDataById('Common/GetPoDetailsByPartyAndItems', obj).toPromise();
     if (result && result.length > 0) {
        const r = result[0];
        this.PurchaseWorkingForm.get('pono').patchValue('');
        this.PurchaseWorkingForm.get('rate').patchValue('');
        this.PurchaseWorkingForm.get('poBalQty1').patchValue('');
        
        debugger;
       // this.poDetailsList = result;
       // this.PurchaseWorkingForm.get('freightDD').patchValue(r.FreightType);
        this.PurchaseWorkingForm.get('pono').patchValue(r.PoNo);
        this.PurchaseWorkingForm.get('rate').patchValue(r.Rate);
        this.PurchaseWorkingForm.get('poBalQty1').patchValue(r.BalQty);
        this.Calrate();

        }
     
      } catch (err) {
       // this.com.hideLoader();
        console.log(err);
      } finally {
    
      }
    }
  



  async onClickSave() {

    if (this.detailsList.length == 0) {
      this.tostr.warning('Incomplete Transaction...');
      return;
    }
    debugger;
    let body = this.PurchaseWorkingForm.value;



    // if (body.vchType === null || body.vchType === undefined || body.vchType === "") {
    //   this.tostr.warning('Select Vch Type....!');
    //   return;
    // }

    // if (body.location  === null || body.location  === undefined || body.location  === "") {
    //   this.tostr.warning('Select Location....!');
    //   return;
    // }
    
    // if (body.vehicleNo  === null || body.vehicleNo  === undefined || body.vehicleNo  === "") {
    //   this.tostr.warning('Enter Vehicle No....!');
    //   return;
    // }

    // if (body.partyMain  === null || body.partyMain  === undefined || body.partyMain  === "") {
    //   this.tostr.warning('Select Party Main....!');
    //   return;
    // }

    // if (body.partySub  === null || body.partySub  === undefined || body.partySub  === "") {
    //   this.tostr.warning('Select Party Main....!');
    //   return;
    // }

    //let vchNo = this.editMode ? this.PurchaseWorkingForm.get('vchNo')?.value : 0;

    // let vchNo = 0
    // if(this.editMode == true){vchNo = this.requisitionForm.get('transNo')?.value}
    // else {vchNo = 0}


    const voucher: any[] = this.detailsList.map((data) => ({
      
      VchType: data.VCHTYPE,
      VchNo: data.VCHNO,
      Location:  this.PurchaseWorkingForm.get('location').value,
      Id:data.Id,
      BagsType:data.BagsType ,
      Pono:data.PONO ,
      Rate:data.Rate,
      Amount:data.Debit,
      Uom:data.UOM,
      BrokerComm:data.Brokercom,
      Brokercomuom:data.Brokercomtype,
      Commission:data.Commission,
      Fed:data.Fed,
      FedRate:data.Fedrate,
      SalesTaxRate:data.SalesTaxrate,
      SalesTax:data.SalesTax,
      BagsType1: this.PurchaseWorkingForm.get('bagsType1').value ?? "",
      BagsType2: this.PurchaseWorkingForm.get('bagsType2').value ?? "",
      BagsType3: this.PurchaseWorkingForm.get('bagsType3').value ?? "",
      Bags1: this.PurchaseWorkingForm.get('bags1').value || 0,
      Bags2: this.PurchaseWorkingForm.get('bags2').value || 0,
      Bags3: this.PurchaseWorkingForm.get('bags3').value || 0,
      Rate1: this.PurchaseWorkingForm.get('rate1').value || 0,
      Rate2: this.PurchaseWorkingForm.get('rate2').value || 0,
      Rate3: this.PurchaseWorkingForm.get('rate3').value || 0,
      BrokerCode: this.PurchaseWorkingForm.get('broker').value ?? "",
      Protein :  body.proteine || 0,

      



      // grnNo: body.vchNo??0,
      // location: body.location??"",
      // grnDate: body.grnDate??"",
      // vehicleNo: body.vehicleNo??"",
      // biltyNo: body.biltyNo??"",
      // biltyWt: body.biltyWt??"",
      // transporter: body.transporter??"",
      // partyMain: body.partyMain??"",
      // partySub: body.partySub??"",
      // itemMain: data.DMCode??"",
      // itemSub: data.DMCode+data.Code??"",
      // pono: body.pono??"",
      // poNo2: body.poNo2??"",
      // rate: body.rate??"",
      // poRate2: body.poRate2??"",
      // poBalQty1: body.poBalQty1??"",
      // poBalQty2: body.poBalQty2??"",
      // poShortQty1: body.poShortQty1??"",
      // poShortQty2: body.poShortQty2??"",
      // partyBillNo: body.partyBillNo??"",
      // partyQty: body.partyQty??"",
      // ourQty: body.ourQty??"",
      // qtyDiff: body.qtyDiff??"",
      // commission: body.commission??"",
      // comUom: body.comUom??"",
      // bags: body.bags??"",
      // : body.bagsType??"",
      // etype: body.etype??"",
      // whTax: body.whTax??"",
      // freight: body.freight??"",
      // freightNo: body.freightNo??"",
      // freightType: body.freightType??"",
      // frgRate: body.frgRate??"",
      // salesTax: body.salesTax??"",
      // kantaExps: body.kantaExps??"",
      // furtherTax: body.furtherTax??"",
      // stockQty: body.stockQty??"",
      // otherExps: body.otherExps??"",
      // proteine: body.proteine??"",
      // payableQty: body.payableQty??"",
      // payableAmt: body.payableAmt??"",
      // netPayableAmt: body.netPayableAmt??"",
      // remarks: body.remarks??"",
      // gross: body.gross??"",
      // tare: body.tare??"",
      // bagsWt: body.bagsWt??"",
      // net: body.net??"",
 
      

    }));

debugger;

  


      try
      {
        this.com.showLoader();
          this.apiService
          .saveData('Purchase/SavePurchaseWorking', voucher)
            .subscribe((result) => {
              this.com.hideLoader();
              if (result == true || result == 'true') {
                this.tostr.success('Save Successfully');
                // this.onClickRefresh();
                // this.getVouchersList();
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

  async checkVechicleGPInward() {

    const d = this.PurchaseWorkingForm.get('grnDate').value;

    const finalDate = this.dp.transform(d, 'yyyy/MM/dd')

    let obj = {
      vehicalNo: this.PurchaseWorkingForm.get('vehicleNo')?.value ?? '',
      vchno: this.PurchaseWorkingForm.get('vchNo')?.value ?? '',
      vchtType: this.PurchaseWorkingForm.get('vchType')?.value ?? '',
      vchDate: finalDate
    }
    
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
    this.PurchaseWorkingForm.get('grnDate').enable();

    const obj = {
      vchNo: VCHNO,
      vchType: VCHTYPE,
      workDone: false,
      locIdUnit: this.uiLocId,
    };

    this.apiService
      .getDataById('Purchase/GetPendingWorkDetail', obj)
      .subscribe((result) => {
        this.togglePages();

    this.detailsList = result;
   
    const d = result[0];
    this.vchNo = d.VchNo;

    this.PurchaseWorkingForm.get('vchType')?.patchValue('RP-Raw');
    this.PurchaseWorkingForm.get('vchNo')?.patchValue(d.GPNO);
    
    //this.PurchaseWorkingForm.get('location')?.patchValue(this.auth.locId());

    const grnDate = 
    new Date(
      d.VCHDATE.split('/')[2],
      d.VCHDATE.split('/')[1] - 1,
      d.VCHDATE.split('/')[0]
    );
    debugger;
  //   this.PurchaseWorkingForm.get('grnDate')?.patchValue(grnDate);
  //   this.PurchaseWorkingForm.get('vehicleNo')?.patchValue(d.VehicleNo);
  //   this.PurchaseWorkingForm.get('biltyNo')?.patchValue(d.BilltyNo);
  //   this.PurchaseWorkingForm.get('biltyWt')?.patchValue(d.Gross-d.Tare);

   

  //   this.PurchaseWorkingForm.get('transporter')?.patchValue(d.DriverName);
  //   this.PurchaseWorkingForm.get('partyMain')?.patchValue(d.Mcode.substring(0, 9));

  //   this.getPartySub({CODE: d.Mcode.substring(0, 9)});
  //   this.PurchaseWorkingForm.get('partySub')?.patchValue(d.Mcode);
    
   
  //   this.PurchaseWorkingForm.get('poNo1')?.patchValue('N/A');
  //   this.PurchaseWorkingForm.get('poNo2')?.patchValue('N/A');
  //   this.PurchaseWorkingForm.get('poRate1')?.patchValue('N/A');
  //   this.PurchaseWorkingForm.get('poRate2')?.patchValue('N/A');
  //   this.PurchaseWorkingForm.get('poBalQty1')?.patchValue('N/A');
  //   this.PurchaseWorkingForm.get('poBalQty2')?.patchValue('N/A');
  //   this.PurchaseWorkingForm.get('poShortQty1')?.patchValue('N/A');
  //   this.PurchaseWorkingForm.get('poShortQty2')?.patchValue('N/A');
  //   this.PurchaseWorkingForm.get('partyBillNo')?.patchValue('N/A');
  //   this.PurchaseWorkingForm.get('partyQty')?.patchValue('N/A');
  //   this.PurchaseWorkingForm.get('ourQty')?.patchValue('N/A');
  //   this.PurchaseWorkingForm.get('qtyDiff')?.patchValue('N/A');
  //   this.PurchaseWorkingForm.get('commission')?.patchValue(d.Commission);
  //   this.PurchaseWorkingForm.get('comUom')?.patchValue('N/A');
  //   this.PurchaseWorkingForm.get('etype')?.patchValue('N/A');
  //   this.PurchaseWorkingForm.get('whTax')?.patchValue(d.Whtax);
  //   this.PurchaseWorkingForm.get('freight')?.patchValue(d.Freight);
  //  this.PurchaseWorkingForm.get('freightNo')?.patchValue('N/A');
  //   this.PurchaseWorkingForm.get('freightType')?.patchValue(d.FreightType);
  //  this.PurchaseWorkingForm.get('frgRate')?.patchValue('N/A');
  //   this.PurchaseWorkingForm.get('salesTax')?.patchValue(d.SalesTax);
  //   this.PurchaseWorkingForm.get('kantaExps')?.patchValue(d.ExpWt);
  //   this.PurchaseWorkingForm.get('furtherTax')?.patchValue(d.FurtherTax);
  
  //   this.PurchaseWorkingForm.get('otherExps')?.patchValue(d.OtherExP);
  //  this.PurchaseWorkingForm.get('proteine')?.patchValue('N/A');
  //   this.PurchaseWorkingForm.get('payableQty')?.patchValue('N/A');
  //  this.PurchaseWorkingForm.get('payableAmt')?.patchValue('N/A');
  //  this.PurchaseWorkingForm.get('netPayableAmt')?.patchValue('N/A');
  //   this.PurchaseWorkingForm.get('remarks')?.patchValue(d.Remarks);
  //   this.PurchaseWorkingForm.get('bagsWt').patchValue(d.BagsDED);
  //   this.PurchaseWorkingForm.get('bagsType1').patchValue(d.Cmb1);
  //   this.PurchaseWorkingForm.get('bagsType2').patchValue(d.Cmb2);
  //   this.PurchaseWorkingForm.get('bagsType3').patchValue(d.Cmb3);
  //   this.PurchaseWorkingForm.get('bags1').patchValue(d.Bags1);
  //   this.PurchaseWorkingForm.get('bags2').patchValue(d.Bags2);
  //   this.PurchaseWorkingForm.get('bags3').patchValue(d.Bags3);
  //   this.PurchaseWorkingForm.get('rate1').patchValue(d.bg1);
  //   this.PurchaseWorkingForm.get('rate2').patchValue(d.BG2);
  //   this.PurchaseWorkingForm.get('rate3').patchValue(d.BG3);

  //   this.PurchaseWorkingForm.get('amount1').patchValue( Math.round(d.Bags1*d.bg1));
  //   this.PurchaseWorkingForm.get('amount2').patchValue(Math.round(d.Bags2*d.BG2));
  //   this.PurchaseWorkingForm.get('amount3').patchValue(Math.round(d.Bags3*d.BG3));





    this.onClickNew();


      
      });
  }

  deleteVouchers(VCHNO: any, VCHTYPE: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {
      const obj = {
        vchNo: VCHNO,
        vchType: VCHTYPE
      };

      this.apiService.deleteData('Purchase/DelPurchaseWorking', obj).subscribe({
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

    this.PurchaseWorkingForm.get('grnDate').enable();
    this.PurchaseWorkingForm.get('location')?.patchValue(this.uiLocId);
  }

  poDetailEnable(event:any){
    event.target.checked == true ? this.poDetailFieldsDisabled = false : this.poDetailFieldsDisabled = true;
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

    const standardWt = parseFloat(this.PurchaseWorkingForm.get('standardWt')?.value);
    const bags = parseFloat(this.PurchaseWorkingForm.get('bags')?.value);
    if (standardWt > 0) {

      const result = standardWt * bags;

      this.PurchaseWorkingForm.get('gross').setValue(result+500);
      this.PurchaseWorkingForm.get('tare').setValue(500);

      const net = (result+500) - 500

      this.PurchaseWorkingForm.get('net').patchValue('');
      if (!isNaN(net)) {
        this.PurchaseWorkingForm.get('net').patchValue(net);
      }
    }
    else {
      const gross = parseFloat(this.PurchaseWorkingForm.get('gross')?.value);
      const tare = parseFloat(this.PurchaseWorkingForm.get('tare')?.value);
      if (isNaN(gross) || isNaN(tare)) {
        this.PurchaseWorkingForm.get('net').patchValue('');
      } else {
        this.PurchaseWorkingForm.get('net').patchValue(gross - tare);
      }
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