import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-outward-weighment',
  templateUrl: './outward-weighment.component.html',
  styleUrls: ['./outward-weighment.component.css']
})
export class OutwardWeighmentComponent {

  constructor(
    private fb: FormBuilder,
    private tostr: ToastrService,
    private apiService: ApiService,
    private dp: DatePipe,
    private com: CommonService,
    private auth: AuthService
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
  WeighBridgeForm!: FormGroup;
  FirstWeightList: any = [];
  SecondWeightList: any = [];
  LabDeductionList: any = [];
  godownList: any = [];
  bagsTypeList: any = [];
  isShow = false;
  isManualAllow = true;
  isReadOnly: boolean = true;
  editMode: boolean = true;
  isShowPage: boolean = true;
  editSno: any = '';
  editModeSno: boolean = false;
  selectedRow: any = null;
  btnAdd: string = 'Add';
  isDisabled: boolean;
  isBig: boolean;
  isSecondWeight = false;
  isFirstWeight = false;

  //Items
  itemsList: any = [];
  itemsCategory: any;
  itemsSubCategory: any;

  ngOnInit() {
    this.formInit();
    this.disableFields();
    this.updateTime();  
    this.getGodowns();
    this.getFirstWeight();
    this.getSecondWeight();
    this.getAllowedWtDiff();
    setInterval(() => {
      this.updateTime();
    }, 1000);
  }

  formInit() {
    this.WeighBridgeForm = this.fb.group({
      vchNo: [''],
      firstWeight: [''],
      Bag1: [undefined],
      Vehicleno: [''],
      UnBag1: [''],
      WBag1: [''],
      Bag2: [undefined],
      Date: [new Date()],
      vchDate: [''],
      manualWt: [''],
      //minWtFinal: [''],
      UnBag2: [''],
      WBag2: [''],
      Bag3: [undefined],
      UnBag3: [''],
      WBag3: [''],
      expWt: [''],
      godowns: [undefined],
      gpNo: [''],
      time: [''],
      cashRcvd: [''],
      partyName: [''],
      partyMain: [''],
      MainItemName: [''],
      ItemName: [''],
      PartyWeight: [''],
      partyBags: [''],
      stockWeight: [''],
      netWeight: [''],
      StkWeight: [''],
      freight: [''],
      tareWeight: [''],
      CurrWtDiff: [''],
      allWtDiff: [''],
      weight: ['0'],
      CurrentDate: [{ value: new Date().toISOString().slice(0, 10), disabled: false }]
    });
  }

  resetForm() {
    this.WeighBridgeForm.get('Bag1')?.patchValue(undefined);
    this.WeighBridgeForm.get('Bag2')?.patchValue(undefined);
    this.WeighBridgeForm.get('Bag3')?.patchValue(undefined);
    this.WeighBridgeForm.get('godowns')?.patchValue(undefined);
    this.WeighBridgeForm.get('WType')?.patchValue(undefined);
    this.WeighBridgeForm.get('UnBag1')?.patchValue('');
    this.WeighBridgeForm.get('WBag1')?.patchValue('');
    this.WeighBridgeForm.get('UnBag2')?.patchValue('');
    this.WeighBridgeForm.get('WBag2')?.patchValue('');
    this.WeighBridgeForm.get('UnBag3')?.patchValue('');
    this.WeighBridgeForm.get('WBag3')?.patchValue('');
    this.WeighBridgeForm.get('freight')?.patchValue('');
    this.WeighBridgeForm.get('ArrvNo')?.patchValue('');
    this.WeighBridgeForm.get('finalFreight')?.patchValue('');
    this.WeighBridgeForm.get('TodayRate')?.patchValue('');
    this.WeighBridgeForm.get('time')?.patchValue('');
    this.WeighBridgeForm.get('partyName')?.patchValue('');
    this.WeighBridgeForm.get('DedKgBag')?.patchValue('');
    this.WeighBridgeForm.get('ItemName')?.patchValue('');
    this.WeighBridgeForm.get('PartyBags')?.patchValue('');
    this.WeighBridgeForm.get('PartyWeight')?.patchValue('');
    this.WeighBridgeForm.get('Vehicleno')?.patchValue('');
    this.WeighBridgeForm.get('gDiff')?.patchValue('');
    this.WeighBridgeForm.get('tDiff')?.patchValue('');
    this.WeighBridgeForm.get('nDiff')?.patchValue('');
    this.WeighBridgeForm.get('AvgBagWt')?.patchValue('');
    this.WeighBridgeForm.get('gWeight')?.patchValue('');
    this.WeighBridgeForm.get('nWeight')?.patchValue('');
    this.WeighBridgeForm.get('bagsWtDed')?.patchValue('');
    this.WeighBridgeForm.get('LabDedStock')?.patchValue('');
    this.WeighBridgeForm.get('LabDedParty')?.patchValue('');
    this.WeighBridgeForm.get('StkWeight')?.patchValue('');
    this.WeighBridgeForm.get('payableWeight')?.patchValue('');
    this.WeighBridgeForm.get('PartyOurDiff1')?.patchValue('');
    this.WeighBridgeForm.get('PartyOurDiff2')?.patchValue('');
    this.WeighBridgeForm.get('PONO')?.patchValue('');
    this.WeighBridgeForm.get('miniWtasFinal')?.patchValue('');
    this.WeighBridgeForm.get('bags1')?.patchValue('');
    this.WeighBridgeForm.get('bags2')?.patchValue('');
    this.WeighBridgeForm.get('bags3')?.patchValue('');
    this.WeighBridgeForm.get('deduction1')?.patchValue('');
    this.WeighBridgeForm.get('deduction2')?.patchValue('');
    this.WeighBridgeForm.get('deduction3')?.patchValue('');
    this.WeighBridgeForm.get('WtDiff')?.patchValue('');
    this.WeighBridgeForm.get('ExpWt')?.patchValue('');
    this.WeighBridgeForm.get('AllowedWtDiff')?.patchValue('');
    this.WeighBridgeForm.get('FrgDedKg')?.patchValue('');
    this.WeighBridgeForm.get('FrgDedAmnt')?.patchValue('');
    this.WeighBridgeForm.get('NWeightBilty')?.patchValue('');
    this.WeighBridgeForm.get('TWeightBilty')?.patchValue('');
    this.WeighBridgeForm.get('GWeightBilty')?.patchValue('');
   
  }

  getFirstWeight() {
    this.apiService.getData('WeighBridge/GetFirstWeightOutward').subscribe((data) => {
      this.FirstWeightList = data;
    });
  }

  getAllowedWtDiff() {
    this.apiService.getData('WeighBridge/GetAllowedWtDiff').subscribe((data) => {
      this.WeighBridgeForm.get('allWtDiff').setValue(data[0].OutLimitOn);
    });
  }


  getSecondWeight() {
    this.apiService.getData('WeighBridge/GetSecondWeightOutward').subscribe((data) => {
      this.SecondWeightList = data;
    });
  }
  

  toggleManualAllow(event: Event) {
    this.isManualAllow = !(event.target as HTMLInputElement).checked;
  }

  getFirstWeightDetail(VchNo:any) {
    const obj ={
      VchNo: VchNo
    }
    let Qty = 0;
    let Freight = 0;
    let ExpWeight = 0;

    this.apiService.getDataById('WeighBridge/GetFirstWeightOutwardDetail', obj).subscribe((data) => {
      this.WeighBridgeForm.get('vchNo').setValue(data[0].VchNo);
      this.WeighBridgeForm.get('firstWeight').setValue("true");
      this.WeighBridgeForm.get('gpNo').setValue(data[0].GPNO);
      this.WeighBridgeForm.get('ItemName').setValue(data[0].ItemName);
      this.WeighBridgeForm.get('partyName').setValue(data[0].PartyName);
      this.WeighBridgeForm.get('Vehicleno').setValue(data[0].VehicleNo);
      this.WeighBridgeForm.get('tareWeight').setValue(data[0].FirstWeight);
      this.WeighBridgeForm.get('MainItemName').setValue(data[0].MainItem);
      this.WeighBridgeForm.get('partyMain').setValue(data[0].PartyMain);
      this.WeighBridgeForm.get('vchDate').setValue(data[0].VchDate);

      data.forEach((item: any) => {
        Qty += item.Qty;
        Freight += item.Freight;
        ExpWeight += item.ExpWt;
       });

       this.WeighBridgeForm.get('partyBags').setValue(Qty);
       this.WeighBridgeForm.get('freight').setValue(Freight);
       this.WeighBridgeForm.get('expWt').setValue(ExpWeight);
    });
  }


  getSecondWeightDetail(VchNo:any) {
    const obj ={
      VchNo: VchNo
    }
    let Qty = 0;
    let Freight = 0;
    let ExpWeight = 0;

    this.apiService.getDataById('WeighBridge/GetSecondWeightOutwardDetail', obj).subscribe((data) => {
      this.WeighBridgeForm.get('vchNo').setValue(data[0].VchNo);
      this.WeighBridgeForm.get('firstWeight').setValue("false");
      this.WeighBridgeForm.get('gpNo').setValue(data[0].GPNO);
      this.WeighBridgeForm.get('ItemName').setValue(data[0].ItemName);
      this.WeighBridgeForm.get('partyName').setValue(data[0].PartyName);
      this.WeighBridgeForm.get('Vehicleno').setValue(data[0].VehicleNo);
      this.WeighBridgeForm.get('tareWeight').setValue(data[0].FirstWeight);
      this.WeighBridgeForm.get('MainItemName').setValue(data[0].MainItem);
      this.WeighBridgeForm.get('partyMain').setValue(data[0].PartyMain);
      this.WeighBridgeForm.get('vchDate').setValue(data[0].VchDate);
      

      data.forEach((item: any) => {
        Qty += item.Qty;
        Freight += item.Freight;
        ExpWeight += item.ExpWt;
       });

       this.WeighBridgeForm.get('partyBags').setValue(Qty);
       this.WeighBridgeForm.get('freight').setValue(Freight);
       this.WeighBridgeForm.get('expWt').setValue(ExpWeight);
    });
  }
  
  async getGodowns() {

    const result = await this.apiService
      .getData('Common/GetGodowns')
      .toPromise();
    this.godownList = result;

  }



  updateTime() {
    const currentTime = new Date();
    const hours = this.padZero(currentTime.getHours());
    const minutes = this.padZero(currentTime.getMinutes());
    const seconds = this.padZero(currentTime.getSeconds());
    const formattedTime = `${hours}:${minutes}:${seconds}`;
    this.WeighBridgeForm.get('time').setValue(formattedTime);
  }

  padZero(num: number): string {
    return num < 10 ? '0' + num : num.toString();
  }
  
  

  onClickSave() {
    if(this.WeighBridgeForm.get('vchNo').value == null || this.WeighBridgeForm.get('vchNo').value == undefined){
      this.tostr.warning('Please Select Voucher');
      return;
    }

    let CurrWtDiff = this.WeighBridgeForm.get('CurrWtDiff').value;
    let Allowed = this.WeighBridgeForm.get('allWtDiff').value;
    CurrWtDiff = Math.abs(CurrWtDiff);

    if(CurrWtDiff>Allowed){
      this.tostr.warning('Current Weight Difference cannot be greater than Allowed Weight Difference');
      return;
    }

    if(this.WeighBridgeForm.get('weight').value == 0){
      this.tostr.info("Weight 0 is not Allowed");
      return;
    }

    let voucher = {};

    if(this.WeighBridgeForm.get('firstWeight').value == "true"){
       voucher = {
        VchNo: this.WeighBridgeForm.get('vchNo').value,
        FirstWeight: this.WeighBridgeForm.get('weight').value,
        TimeIn: this.WeighBridgeForm.get('time').value,
        Type: "FirstWeight"
      }
      this.isFirstWeight = true;
      this.isSecondWeight = false;
    }
    else{
       voucher = {
        VchNo: this.WeighBridgeForm.get('vchNo').value,
        SecondWeight: this.WeighBridgeForm.get('weight').value,
        TimeOut: this.WeighBridgeForm.get('time').value,
        Type: "SecondWeight"
      }

      this.isSecondWeight = true;
      this.isFirstWeight = false;
      
    }


    this.apiService
      .saveData('WeighBridge/SaveOutWardWeighment', voucher)
      .subscribe((result) => {
        if (result == true || result == 'true') {
          this.tostr.success('Save Successfully');
          this.onClickRefresh();
         this.getFirstWeight();
         this.getSecondWeight();
        } else {
          this.tostr.error('Please Save Again');
        }
      });
  }

  onClickRefresh() {
    this.isShow = false;
    this.resetForm();
    //this.requisitionList = [];
    this.disableFields();
  }

  enableFields() {
   this.isDisabled = false;
  }

  disableFields() {
    this.isDisabled = true;
  }

  togglePages() {
    this.isShowPage = !this.isShowPage;
    if (this.isShowPage) {
      this.onClickRefresh();
    }
  }

  searchSecondWtGrid(event: Event): void {
    const searchTerm = (event.target as HTMLInputElement).value.toLowerCase();
    const rows = document.querySelectorAll('.second-weight tbody tr');
  
    rows.forEach((row: HTMLTableRowElement) => {
      const rowData = row.textContent?.toLowerCase() || '';
  
      if (rowData.includes(searchTerm)) {
        row.style.display = '';
      } else {
        row.style.display = 'none';
      }
    });
  }

  searchFirstWtGrid(event: Event): void {
    const searchTerm = (event.target as HTMLInputElement).value.toLowerCase();
    const rows = document.querySelectorAll('.first-weight tbody tr');
  
    rows.forEach((row: HTMLTableRowElement) => {
      const rowData = row.textContent?.toLowerCase() || '';
  
      if (rowData.includes(searchTerm)) {
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

    this.WeighBridgeForm.patchValue(formValue, { emitEvent: false });
  }

  CalculateWeighment(){
    if(this.WeighBridgeForm.get('firstWeight').value == "false"){
      let gross = this.WeighBridgeForm.get('weight').value;
      let Tare = this.WeighBridgeForm.get('tareWeight').value;
      let weight = (gross-Tare);
      this.WeighBridgeForm.get('netWeight').setValue(weight);
      this.WeighBridgeForm.get('stockWeight').setValue(weight);
      let expWt = this.WeighBridgeForm.get('expWt').value;
      let netWeight = this.WeighBridgeForm.get('netWeight').value;
      this.WeighBridgeForm.get('CurrWtDiff').setValue(expWt-netWeight);
      if( this.WeighBridgeForm.get('CurrWtDiff').value > this.WeighBridgeForm.get('allWtDiff').value){
       this.isBig = true;
      }
      else {
       this.isBig = false;
      }
    }
   

  }
  LoadingSlip(){
    let form = this.WeighBridgeForm.value;
    
    let url = `SaleLoadingSlip?VchType=SP&VchNo=${form.vchNo}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}&FromDate=${form.vchDate}&CmpId=${this.auth.cmpId()}`;
    this.com.viewReport(url);
  }

  DeliveryChallan(){
    let form = this.WeighBridgeForm.value;
    
    let url = `DeliveryChallan?VchType=SP&VchNo=${form.vchNo}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}&FromDate=${form.vchDate}&CmpId=${this.auth.cmpId()}`;
    this.com.viewReport(url);
  }

  GatePassOutward() {
    let form = this.WeighBridgeForm.value;
    
    let url = `GatePassOutwardSale?VchType=SP&VchNo=${form.vchNo}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}&FromDate=${form.vchDate}&CmpId=${this.auth.cmpId()}`;
    this.com.viewReport(url);
  }

  SecondWeight() {
    let form = this.WeighBridgeForm.value;
    
    let url = `SecWeightSlipSale?VchType=SP&VchNo=${form.vchNo}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}&FromDate=${form.vchDate}&CmpId=${this.auth.cmpId()}`;
    this.com.viewReport(url);
  }
}
