import { DatePipe } from '@angular/common';
import { Component, ElementRef, HostListener, ViewChild, numberAttribute } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';
import { SerialPortService } from 'src/app/services/services/serial-port.service';
import { CostCentreComponent } from '../../sale/cost-centre/cost-centre.component';
import { BehaviorSubject } from 'rxjs';


@Component({
  selector: 'app-inward-weighment',
  templateUrl: './inward-weighment.component.html',
  styleUrls: ['./inward-weighment.component.css']
})
export class InwardWeighmentComponent {
  private dataSubject: BehaviorSubject<string> = new BehaviorSubject<string>('');
  public data: string = '';
  MAX_DATA_SIZE: number = 1024;
  private port: any;
  constructor(
    private fb: FormBuilder,
    private tostr: ToastrService,
    private apiService: ApiService,
    private dp: DatePipe,
    private com: CommonService,
    private auth: AuthService,
    private WB: SerialPortService
  )
   {
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
  isShow = false;
  isManualAllow = true;
  isReadOnly: boolean = true;
  editMode: boolean = true;
  isShowPage: boolean = true;
  editSno: any = '';
  editModeSno: boolean = false;
  selectedRow: number | null = null;
  btnAdd: string = 'Add';
  isDisabled: boolean;
  isSecondWeight = true;
  bagsTypeList: any = [];
  isMinWtFinal: boolean = false;

  //Items
  itemsList: any = [];
  itemsCategory: any;
  itemsSubCategory: any;


baudRate: any;
dataBits: any;
parity: any;
stopBits: any;
wbWeight: any;
secLabCheck: boolean;
DayCloseTime:any;

inlimit:any ;


  ngOnInit() {
    debugger;
    this.formInit();
    this.disableFields();
    this.DayCloseTime=this.auth.shiftime();
    this.updateTime(this.DayCloseTime);
    this.getGodowns();
    this.getFirstWeight();
    this.getSecondWeight();
    if(this.FirstWeightList.length > 0 || this.SecondWeightList.length > 0){
      this.isShow = true;
    }
    setInterval(() => {
      this.updateTime(this.DayCloseTime);
    }, 1000);
    this.getBagsType();

    this.getWBSettings();





    // setInterval(() => {
    //   this.WB.dataSubject.subscribe(data => {
    //     this.wbWeight += data;
    //     console.log(this.wbWeight);
    //   });
    // }, 2000);




    
  }




  // connectToPort(){
  //   this.WB.connectToPort();
  // }
  // async connectToPort() {
  //   debugger;
  //   const nav: any = navigator;
  //   if (!this.port) {
  //     this.port = await nav.serial.requestPort();
  //   }

  //   // Check if the port is already open
  //   if (!this.port.readable || !this.port.writable) {
  //     await this.port.open({
  //       baudRate: 2400,
  //       dataBits: 8,
  //       parity: 'none',
  //       stopBits: 1,
  //     });
  //   }

  //   const reader = this.port.readable.getReader();
  //   try {
  //     while (true) {
  //       const { value, done } = await reader.read();
  //       if (done) {
  //         break;
  //       }
  //       if (value) {
  //         const text = new TextDecoder().decode(value);
          
  //         this.data += text;  
  //         console.log(text);  
  //       }
  //     }
  //   } catch (error) {
  //     console.error('Error reading from serial port:', error);
  //   } finally {
  //     reader.releaseLock();
  //     await this.port.close();  
  //   }
  // }

  

  // async connectToPort() {
  //   debugger;
  //   const nav: any = navigator;
  
  //   // Request a port if not already available
  //   if (!this.port) {
  //     this.port = await nav.serial.requestPort();
  //   }
  
  //   // Check if the port is already open
  //   if (!this.port.readable || !this.port.writable) {
  //     await this.port.open({
  //       baudRate: 2400,
  //       dataBits: 8,
  //       parity: 'none',
  //       stopBits: 1,
  //     });
  //   }
  
  //   const reader = this.port.readable.getReader();
  //   const decoder = new TextDecoder();
  //   this.data = '';  // Ensure data is initialized
  
  //   try {
  //     while (true) {
  //       const { value, done } = await reader.read();
  
  //       if (done) {
  //         // Reader is done, break out of the loop
  //         break;
  //       }
  
  //       if (value) {

  //         //console.log('Raw data received:', value);
  
      
  //         const text = decoder.decode(value, { stream: true });

  //         this.data += text;
  //         const cleanedText = this.data.replace(/[^\x20-\x7E]/g, ''); 
  //         console.log('Cleaned text:', cleanedText);
  //         this.processMessage(cleanedText);
  //         this.checkAndClearData();
  //       }
  //     }
  //   } catch (error) {
  //     console.error('Error reading from serial port:', error);
  //   } finally {
  //     reader.releaseLock();
  

  //     if (this.port) {
  //       await this.port.close();
  //     }
  //   }
  // }
  


  async connectToPort() {
    debugger;
    const nav: any = navigator;
  
    
    if (!this.port) {
      this.port = await nav.serial.requestPort();
    }
  
    // Check if the port is already open
    if (!this.port.readable || !this.port.writable) {
      await this.port.open({
        baudRate: 2400,
        dataBits: 8,
        parity: 'none',
        stopBits: 1,
      });
    }
  
    this.readFromPort();
  }
  
  async readFromPort() {
    const reader = this.port.readable.getReader();
    const decoder = new TextDecoder();
    this.data = '';  
  
    try {
      while (true) {
        const { value, done } = await reader.read();
  
        if (done) {
      
          console.log('Reader is done, stopping reading loop');
          break;
        }
  
        if (value) {
          const text = decoder.decode(value, { stream: true });
  
          this.data += text;
          const cleanedText = this.data.replace(/[^\x20-\x7E]/g, ''); 
          console.log('Cleaned text:', cleanedText);
          this.processMessage(cleanedText);
          this.checkAndClearData();
        }
      }
    } catch (error) {
      console.error('Error reading from serial port:', error);
    } finally {
      reader.releaseLock();
  
   
      if (this.port) {
        await this.port.close();
        this.port = null;
        console.log('Port closed, attempting to reconnect...');
        setTimeout(() => this.connectToPort(), 1000); 
      }
    }
  }


  


  processMessage(message: string) {
    const lastIndex = message.lastIndexOf('+');
    if (lastIndex !== -1 && lastIndex + 6 <= message.length) {
        const weightString = message.substring(lastIndex + 1, lastIndex + 7);
        if (weightString.length === 6) {
            const weight = parseFloat(weightString);
            if (!isNaN(weight)) {
                console.log('Weight:', weight);
                this.WeighBridgeForm.get('weight').patchValue(weight);
                this.CalculateNetWeight();
            }
        }
    }
}

  checkAndClearData() {
    if (this.data.length > this.MAX_DATA_SIZE) {
      this.data = '';
    }
  }

  


  formInit() {
    this.WeighBridgeForm = this.fb.group({
      vchNo: [''],
      firstWeight: [''],
      Bag1: [undefined],
      UnBag1: [''],
      WBag1: [''],
      Bag2: [undefined],
      UnBag2: [''],
      WBag2: [''],
      Bag3: [undefined],
      UnBag3: [''],
      WBag3: [''],
      freight: [''],
      godowns: [undefined],
      ArrvNo: [''],
      Vehicleno:[''],
      currentdate: [''],
      TodayRate: [''],
      time: [''],
      partyName: [''],
      DrpBag: [undefined],
      NoOfBags: [''],
      WType: [undefined],
      DedKgBag: [''],
      ItemName: [''],
      PartyBags: [''],
      PartyWeight: [''],
      gDiff: [''],
      tDiff: [''],
      nDiff: [''],
      AvgBagWt: [''],
      gWeight: [''],
      nWeight: [''],
      bagsWtDed: [''],
      LabDedStock: [''],
      LabDedParty: [''],
      StkWeight: [''],
      payableWeight: [''],
      // PartyOurDiff1: [''],
      PartyOurDiff2: [''],
      PONO: [''],
      miniWtasFinal: [''],
      bags1: [''],
      bags2: [''],
      bags3: [''],
      deduction1: [''],
      deduction2: [''],
      deduction3: [''],
      WtDiff: [''],
      ExpWt: [''],
      AllowedWtDiff: [this.auth.inlimit()],
      FrgDedKg: [''],
      FrgDedAmnt: [''],
      NWeightBilty: [''],
      TWeightBilty: [''],
      GWeightBilty: [''],
      manualWeight: [false],
      weight: ['0'],
      freightDD: ['']
    });
  }

  async getBagsType() {
    const result = await this.apiService
    .getData('Common/GetPurchaseBagsType')
    .toPromise();

    this.bagsTypeList = result;

  }

  resetForm() {
    this.WeighBridgeForm.get('Bag1')?.patchValue(undefined);
    this.WeighBridgeForm.get('Bag2')?.patchValue(undefined);
    this.WeighBridgeForm.get('Bag3')?.patchValue(undefined);
    this.WeighBridgeForm.get('godowns')?.patchValue(undefined);
    this.WeighBridgeForm.get('DrpBag')?.patchValue(undefined);
    this.WeighBridgeForm.get('NoOfBags')?.patchValue('');
    this.WeighBridgeForm.get('WType')?.patchValue(undefined);
    this.WeighBridgeForm.get('UnBag1')?.patchValue('');
    this.WeighBridgeForm.get('WBag1')?.patchValue('');
    this.WeighBridgeForm.get('UnBag2')?.patchValue('');
    this.WeighBridgeForm.get('WBag2')?.patchValue('');
    this.WeighBridgeForm.get('UnBag3')?.patchValue('');
    this.WeighBridgeForm.get('WBag3')?.patchValue('');
    this.WeighBridgeForm.get('freight')?.patchValue('');
    this.WeighBridgeForm.get('vchNo')?.patchValue('');
    this.WeighBridgeForm.get('ArrvNo')?.patchValue('');
    this.WeighBridgeForm.get('date')?.patchValue('');
    this.WeighBridgeForm.get('TodayRate')?.patchValue('');
    this.WeighBridgeForm.get('time')?.patchValue('');
    this.WeighBridgeForm.get('partyName')?.patchValue('');
    this.WeighBridgeForm.get('DedKgBag')?.patchValue('');
    this.WeighBridgeForm.get('ItemName')?.patchValue('');
    this.WeighBridgeForm.get('PartyBags')?.patchValue('');
    this.WeighBridgeForm.get('PartyWeight')?.patchValue('');
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
    this.WeighBridgeForm.get('Vehicleno')?.patchValue('');
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
    //this.WeighBridgeForm.get('AllowedWtDiff')?.patchValue(this.auth.inlimit());
    this.WeighBridgeForm.get('FrgDedKg')?.patchValue('');
    this.WeighBridgeForm.get('FrgDedAmnt')?.patchValue('');
    this.WeighBridgeForm.get('NWeightBilty')?.patchValue('');
    this.WeighBridgeForm.get('TWeightBilty')?.patchValue('');
    this.WeighBridgeForm.get('GWeightBilty')?.patchValue('');
    
  }

  toggleManualAllow(event: Event) {
    this.isManualAllow = !(event.target as HTMLInputElement).checked;
  }


  getFirstWeight() {
    const obj = {
      VchType: "RP-RAW"
    }
    this.apiService.getDataById('WeighBridge/GetFirstWeight',obj).subscribe((data) => {
      this.FirstWeightList = data;
    });
  }

  getSecondWeight() {
    const obj = {
      VchType: "RP-RAW"
    }
    this.apiService.getDataById('WeighBridge/GetSecondWeight', obj).subscribe((data) => {
      this.SecondWeightList = data;
    });
  }


  getWeightDetail(VchNo:any, ArrivalNo:any, VchType:any , Status:string) {
    this.onClickRefresh();
    const obj ={
      VchNo: VchNo,
      ArrivalNo: ArrivalNo,
      VchType: VchType ,
      Status: Status ,
    }

    this.apiService.getDataById('WeighBridge/GetFirstWeightDetail', obj).subscribe((data) => {
       

      let Gross=0;
      let Tare=0;
      let ExpWt=0;
      let PartyBags=0;
      let PartyWeight=0;
      let FirstWeight=0;
      FirstWeight=data.FirstWeight[0].FirstWeight;
      data.FirstWeight.forEach((item: any) => {
        Gross+=item.Gross;
        Tare+=item.Tare;
        ExpWt+=item.ExpWt;
        PartyBags+=item.SBAGS;
        PartyWeight+=item.SQTY;
      });
      this.WeighBridgeForm.get('vchNo').setValue(data.FirstWeight[0].VchNo);
      this.WeighBridgeForm.get('firstWeight').setValue(Status);
      if (Status=="true") {this.isSecondWeight=true; }
      else{ 
        

        this.WeighBridgeForm.get('gWeight').setValue(FirstWeight);
        this.isSecondWeight=false;
      }
      
      this.WeighBridgeForm.get('miniWtasFinal').setValue(data.FirstWeight[0].MiniWt);
      this.WeighBridgeForm.get('ItemName').setValue(data.FirstWeight[0].ItemName);
      this.WeighBridgeForm.get('partyName').setValue(data.FirstWeight[0].PartyName);
      this.WeighBridgeForm.get('freight').setValue(data.FirstWeight[0].Freight);
      this.WeighBridgeForm.get('godowns').setValue(data.FirstWeight[0].GodownId);
      this.WeighBridgeForm.get('ArrvNo').setValue(data.FirstWeight[0].VchNo);
      this.WeighBridgeForm.get('PONO').setValue(data.FirstWeight[0].PONO);
      this.WeighBridgeForm.get('ExpWt').setValue(ExpWt);
      this.WeighBridgeForm.get('Vehicleno').setValue(data.FirstWeight[0].VehicleNo);

      //Bilty Information
      this.WeighBridgeForm.get('GWeightBilty').setValue(Gross);
      this.WeighBridgeForm.get('TWeightBilty').setValue(Tare);
      this.WeighBridgeForm.get('NWeightBilty').setValue(Gross-Tare);

      this.WeighBridgeForm.get('PartyBags').setValue(PartyBags);
      this.WeighBridgeForm.get('PartyWeight').setValue(PartyWeight);
   
      this.WeighBridgeForm.get('freightDD').setValue(data.FirstWeight[0].FreightType);
      this.WeighBridgeForm.get('DrpBag').setValue(data.FirstWeight[0].BagsType);
      this.WeighBridgeForm.get('WType').setValue(data.FirstWeight[0].WType);
      this.isMinWtFinal = data.FirstWeight[0].MiniWt

      this.LabDeductionList = data.LabDed;

      this.CalculateNetWeight();

 
    });
  }


  onItemSubChange(event: any, cmbno: any) {
    let bgwt = this.bagsTypeList.find((i) => i.Code === event.Code).Rate;
     
    if(cmbno==1){this.WeighBridgeForm.get('WBag1').setValue(bgwt);}
    if(cmbno==2){this.WeighBridgeForm.get('WBag2').setValue(bgwt);}
    if(cmbno==3){this.WeighBridgeForm.get('WBag3').setValue(bgwt);}

    const Bags1 = Number(this.WeighBridgeForm.get('UnBag1').value);
    const Bags2 = Number(this.WeighBridgeForm.get('UnBag2').value);
    const Bags3 = Number(this.WeighBridgeForm.get('UnBag3').value);
    const PartyBags = Number(this.WeighBridgeForm.get('PartyBags').value);
  
    
    const TBags =Bags1+Bags2+Bags3;

      if(TBags==0)
      {
        this.WeighBridgeForm.get('UnBag1').setValue( PartyBags );
      }

   this.CalculateNetWeight();
  }
  
  async getGodowns() {

    const result = await this.apiService
      .getData('Common/GetGodowns')
      .toPromise();
    this.godownList = result;

  }


 
   CalculateNetWeight() {
    console.log(this.isSecondWeight);
    const Gross = Number(this.WeighBridgeForm.get('GWeightBilty').value);
    const Tare = Number(this.WeighBridgeForm.get('TWeightBilty').value);
    const Weight = Number(this.WeighBridgeForm.get('weight').value);
    const FristWeight = Number(this.WeighBridgeForm.get('gWeight').value);
    const  PartyWeight =Number(this.WeighBridgeForm.get('PartyWeight').value)
    let PayableWeight =0;

    const Bags1 = Number(this.WeighBridgeForm.get('UnBag1').value);
    const Bags2 = Number(this.WeighBridgeForm.get('UnBag2').value);
    const Bags3 = Number(this.WeighBridgeForm.get('UnBag3').value);
    
    const Bags1Ded = Number(this.WeighBridgeForm.get('WBag1').value);
    const Bags2Ded = Number(this.WeighBridgeForm.get('WBag2').value);
    const Bags3Ded = Number(this.WeighBridgeForm.get('WBag3').value);

    const TBags =Bags1+Bags2+Bags3;

    const TBagsDed =Math.round((Bags1*Bags1Ded)+(Bags2*Bags2Ded)+(Bags3*Bags3Ded));
   

    this.WeighBridgeForm.get('bags1').setValue(Bags1); 
    this.WeighBridgeForm.get('bags2').setValue(Bags2); 
    this.WeighBridgeForm.get('bags3').setValue(Bags3); 

    this.WeighBridgeForm.get('deduction1').setValue(  Math.round((Bags1*Bags1Ded)) ); 
    this.WeighBridgeForm.get('deduction2').setValue(Math.round((Bags2*Bags2Ded))); 
    this.WeighBridgeForm.get('deduction3').setValue(Math.round((Bags3*Bags3Ded))); 


    this.WeighBridgeForm.get('NoOfBags').setValue(TBags); 
    this.WeighBridgeForm.get('bagsWtDed').setValue(TBagsDed); 






    

    // if(this.isSecondWeight == false){
      this.WeighBridgeForm.get('nWeight').setValue((this.WeighBridgeForm.get('gWeight').value - Weight)); 
      this.WeighBridgeForm.get('payableWeight').setValue((this.WeighBridgeForm.get('gWeight').value - Weight)); 

    // }
   
    

    this.WeighBridgeForm.get('LabDedParty').setValue(0);
    this.WeighBridgeForm.get('LabDedStock').setValue(0);


    //Minimum Weight Checking
    if(this.isMinWtFinal==true  && PartyWeight< Number(this.WeighBridgeForm.get('nWeight').value)) 
    {
      this.WeighBridgeForm.get('payableWeight').setValue(PartyWeight); 
    }
    //Assign PayableWeight
    PayableWeight= Number(this.WeighBridgeForm.get('payableWeight').value);

    
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
        this.WeighBridgeForm.get('LabDedParty').setValue( Math.round( Number( this.WeighBridgeForm.get('LabDedParty').value)+LabDedParty ) ); 
        this.WeighBridgeForm.get('LabDedStock').setValue( Math.round(Number( this.WeighBridgeForm.get('LabDedStock').value)+ LabDedStock ));
        item.PartyDedKg= Math.round(LabDedParty);
        item.StockDedKg=Math.round(LabDedStock);
      });
    
    if(this.WeighBridgeForm.get('firstWeight').value=="true")
    {
    this.WeighBridgeForm.get('gDiff').setValue(Weight-Gross);
    }
    else
    {
      this.WeighBridgeForm.get('gDiff').setValue(FristWeight-Gross);
      this.WeighBridgeForm.get('tDiff').setValue(Weight-Tare);
      this.WeighBridgeForm.get('nDiff').setValue((this.WeighBridgeForm.get('nWeight').value - PartyWeight)); 
    }
    // if(this.isSecondWeight == false){
      this.WeighBridgeForm.get('StkWeight').setValue( this.WeighBridgeForm.get('nWeight').value  -TBagsDed);

    //  }
    

      if ( this.WeighBridgeForm.get('DrpBag').value =="W")
      {
        this.WeighBridgeForm.get('payableWeight').setValue(Math.round(  this.WeighBridgeForm.get('nWeight').value - Number(this.WeighBridgeForm.get('LabDedParty').value))); 
      }
      else
      {
        this.WeighBridgeForm.get('payableWeight').setValue(Math.round(  this.WeighBridgeForm.get('nWeight').value  - (Number(this.WeighBridgeForm.get('LabDedParty').value)+TBagsDed))); 
      }




       if ( PartyWeight!=0 )
        {





          
          if ( this.WeighBridgeForm.get('WType').value =="S")
            {
              if ( this.WeighBridgeForm.get('DrpBag').value =="W")
              {
                this.WeighBridgeForm.get('payableWeight').setValue(Math.round(  PartyWeight - Number(this.WeighBridgeForm.get('LabDedParty').value))); 
              }
              else
              {
                this.WeighBridgeForm.get('payableWeight').setValue(Math.round(  PartyWeight - (Number(this.WeighBridgeForm.get('LabDedParty').value)+TBagsDed))); 
              }

            }

            if(this.isMinWtFinal==true ) 
              {
                if (PartyWeight < Number(this.WeighBridgeForm.get('nWeight').value))
                  {
                   

                    if ( this.WeighBridgeForm.get('DrpBag').value =="W")
                      {
                        this.WeighBridgeForm.get('payableWeight').setValue(Math.round(  PartyWeight - Number(this.WeighBridgeForm.get('LabDedParty').value))); 
                      }
                      else
                      {
                        this.WeighBridgeForm.get('payableWeight').setValue(Math.round(  PartyWeight - (Number(this.WeighBridgeForm.get('LabDedParty').value)+TBagsDed))); 
                      }


                  }

                  else 
                    {
                     
  
                      if ( this.WeighBridgeForm.get('DrpBag').value =="W")
                        {
                          this.WeighBridgeForm.get('payableWeight').setValue(Math.round(  this.WeighBridgeForm.get('nWeight').value - Number(this.WeighBridgeForm.get('LabDedParty').value))); 
                        }
                        else
                        {
                          this.WeighBridgeForm.get('payableWeight').setValue(Math.round(  this.WeighBridgeForm.get('nWeight').value - (Number(this.WeighBridgeForm.get('LabDedParty').value)+TBagsDed))); 
                        }
  
  
                    }



                
              }


          
        }
     
        this.WeighBridgeForm.get('PartyOurDiff2').setValue( this.WeighBridgeForm.get('payableWeight').value  -    PartyWeight);
       if( Number(this.WeighBridgeForm.get('ExpWt').value)>0  )
        {
          this.WeighBridgeForm.get('WtDiff').setValue( Number(this.WeighBridgeForm.get('ExpWt').value)  -    Number( this.WeighBridgeForm.get('payableWeight').value));
         
        }


      




         

        // If Val(TxtNetWeight.Text) > 0 And Val(TxtBags.Text) > 0 Then
        // TxtNetWeight.Text = Val(TxtGrossWeight.Text) - Val(TxtWeight.Text)
        // TxtAvgBagWt.Text = Math.Round((Val(TxtNetWeight.Text) - Val(TxtBagsDed.Text)) / Val(TxtBags.Text), 0)
        // End If





  
    // if(this.WeighBridgeForm.get('ExpWt').value > 0) {
    //   this.WeighBridgeForm.get('AllowedWtDiff').setValue((this.WeighBridgeForm.get('ExpWt').value - this.WeighBridgeForm.get('nWeight').value));
    //  }
  }



  updateTime(shiftTime) {
    const currentTime = new Date();
    const hours = this.padZero(currentTime.getHours());
    const minutes = this.padZero(currentTime.getMinutes());
    const seconds = this.padZero(currentTime.getSeconds());
    const formattedTime = `${hours}:${minutes}:${seconds}`;
    this.WeighBridgeForm.get('time').setValue(formattedTime);

    // Parse the shift time
    const [shiftHour, shiftMinute] = shiftTime.split(':').slice(0, 2).map(Number);

    // Get the current date
    let year = currentTime.getFullYear();
    let month = this.padZero(currentTime.getMonth() + 1); // Months are zero-based
    let day = this.padZero(currentTime.getDate());

    // Check if the current time is before the shift start time (only considering HH:mm)
    if (currentTime.getHours() < shiftHour || (currentTime.getHours() === shiftHour && currentTime.getMinutes() < shiftMinute)) {
        // If it's before the shift start time, set the date to the previous day
        const previousDay = new Date(currentTime);
        previousDay.setDate(currentTime.getDate() - 1);
        year = previousDay.getFullYear();
        month = this.padZero(previousDay.getMonth() + 1);
        day = this.padZero(previousDay.getDate());
    }

    const formattedDate = `${year}-${month}-${day}`;
    this.WeighBridgeForm.get('currentdate').setValue(formattedDate);
}

  padZero(num: number): string {
    return num < 10 ? '0' + num : num.toString();
  }

 
  checkIfFirstLabTestNameIsEmpty(): boolean {
    return this.LabDeductionList.length > 0 && this.LabDeductionList[0].LabTestName === '';
  }
    onClickSave() {
      debugger;

    //  var manualWeight=  this.WeighBridgeForm.get('manualWeight')?.value;
     



      if(this.WeighBridgeForm.get('weight').value == 0){
        this.tostr.info("Weight 0 is not Allowed");
        return;
      }

      debugger;
      if(this.WeighBridgeForm.get('firstWeight').value != "true"  && (this.WeighBridgeForm.get('ExpWt').value  || 0 ==0 ) ){
      if (this.LabDeductionList.length > 0) {
        const firstRow = this.LabDeductionList[0];
        if (!firstRow.LabTestName) {
          const userConfirmed = confirm('Second lab is not done. Are you sure you want to save without Second lab?');
        
          if (userConfirmed) {
            
          } else {
            return;
          }


        }
      }
     }


      

      debugger;

      if(this.WeighBridgeForm.get('ExpWt').value  || 0 > 0   ){
        const AllowedWtDiff= this.WeighBridgeForm.get('AllowedWtDiff').value  || 0;
        const WeightDiff= this.WeighBridgeForm.get('WtDiff').value  || 0;


      
    



        if(   (Number( AllowedWtDiff) + Number(WeightDiff)) < 0  )
          {

            if (  (Number( AllowedWtDiff) + Number(WeightDiff)) < Number(AllowedWtDiff))
              {

                this.tostr.info("Weight difference is greater than allowed difference");
                return;

              }
        
          }

        
      
      }
      else
      {

        debugger;
        if(this.WeighBridgeForm.get('firstWeight').value != "true"){


      
        const NoOfBags= this.WeighBridgeForm.get('NoOfBags').value  || 0;
        if (Number(NoOfBags)==0)
          {
            this.tostr.info("Select Unloaded Bags");
            return;
          }
        }
      }

      const currentTime = new Date();

      const year = currentTime.getFullYear();
      const month = ('0' + (currentTime.getMonth() + 1)).slice(-2);
      const day = ('0' + currentTime.getDate()).slice(-2);
      const hours = ('0' + currentTime.getHours()).slice(-2);
      const minutes = ('0' + currentTime.getMinutes()).slice(-2);
      const seconds = ('0' + currentTime.getSeconds()).slice(-2);

      //const VchDate = new Date(`${year}-${month}-${day} ${hours}:${minutes}:${seconds}`);

      const VchDate =  this.WeighBridgeForm.get('currentdate').value;
    
       
      if(this.WeighBridgeForm.get('vchNo').value == null || this.WeighBridgeForm.get('vchNo').value == undefined || this.WeighBridgeForm.get('vchNo').value == ""){
        this.tostr.warning('Please Select Voucher');
        return;
      }
      
      let voucher = {};
      let lab = {   };

      if(this.WeighBridgeForm.get('firstWeight').value == "true"){
        voucher = {
          VchNo: this.WeighBridgeForm.get('vchNo').value,
          FirstWeight: this.WeighBridgeForm.get('weight').value,
          TimeIn: this    .WeighBridgeForm.get('time').value,
          vchDate: VchDate ,
          Type: "FirstWeight",
          Vchtype:"RP-RAW" ,
          Bags:  0, 
          Qty: 0, 
          StockWeight: 0, 
          PayableWeight: 0, 
          Cmb1: "", 
          Cmb2: "",
          Cmb3: "",
          Bags1: 0, 
          Bags2: 0, 
          Bags3: 0, 
          Ded1: 0, 
          Ded2 : 0, 
          Ded3 : 0, 
          Godownid: 0,  
          LabDedStock: 0,  
          LabDedParty: 0, 
          PartyName: "",
          ItemName:"",
          BagsDed:0,
          manualWeight:  this.WeighBridgeForm.get('manualWeight')?.value,
          lab: this.LabDeductionList,
        }
       

      }
      else{
       debugger;
        // if (this.checkIfFirstLabTestNameIsEmpty() && this.WeighBridgeForm.get('ExpWt').value ==0) {
        //   const userConfirmed = window.confirm("Second Lab is not done want to save ?");
        //   if (userConfirmed) {    
        //   } else {
        //     return;
        //   }
        // } 

        voucher = {
          VchNo: this.WeighBridgeForm.get('vchNo').value,
          SecondWeight: this.WeighBridgeForm.get('weight').value,
          TimeOut: this.WeighBridgeForm.get('time').value,
          Freight: this.WeighBridgeForm.get('freight').value,
          FreightType: this.WeighBridgeForm.get('freightDD').value,
          Type: "SecondWeight",
          Vchtype:"RP-RAW",
          vchDate: VchDate ,
          Bags:   this.WeighBridgeForm.get('NoOfBags').value|| 0, 
          Qty: this.WeighBridgeForm.get('StkWeight').value || 0, 
          StockWeight: this.WeighBridgeForm.get('StkWeight').value || 0, 
          PayableWeight: this.WeighBridgeForm.get('payableWeight').value || 0, 
          Cmb1: this.WeighBridgeForm.get('Bag1').value|| "", 
          Cmb2: this.WeighBridgeForm.get('Bag2').value || "", 
          Cmb3: this.WeighBridgeForm.get('Bag3').value || "", 
          Bags1: this.WeighBridgeForm.get('UnBag1').value || 0, 
          Bags2: this.WeighBridgeForm.get('UnBag2').value || 0, 
          Bags3: this.WeighBridgeForm.get('UnBag3').value || 0, 
          Ded1: this.WeighBridgeForm.get('WBag1').value || 0, 
          Ded2 : this.WeighBridgeForm.get('WBag2').value || 0, 
          Ded3 : this.WeighBridgeForm.get('WBag3').value || 0, 
          Godownid: this.WeighBridgeForm.get('godowns').value || 0, 
          LabDedStock:   this.WeighBridgeForm.get('LabDedStock').value || 0,  
          LabDedParty:   this.WeighBridgeForm.get('LabDedParty').value || 0,  
          PartyName: this.WeighBridgeForm.get('partyName').value || "", 
          ItemName: this.WeighBridgeForm.get('ItemName').value || "", 
          BagsDed: this.WeighBridgeForm.get('bagsWtDed').value || 0, 
          manualWeight:  this.WeighBridgeForm.get('manualWeight')?.value,
          
          lab: this.LabDeductionList,
        }
      }

      //let vchNo = this.editMode ? this.WeighBridgeForm.get('transNo')?.value : 0;
      const requestBody = {
        weigh: voucher,
        lab: this.LabDeductionList
      };
      this.com.showLoader();
      this.apiService
        .saveData('WeighBridge/SaveWeighment', voucher)
        .subscribe((result) => {
          this.com.hideLoader();
          if (result.report == "FirstWeight") {
          this.tostr.success('Save Successfully');
          this.onClickRefresh();
          this.getFirstWeight();
          this.getSecondWeight();
          this.FirstWeightSlip(result.VchNo, result.FromDate);
          } else if (result.report == "SecondWeight"){
            this.com.hideLoader();
          this.tostr.success('Save Successfully');
          this.onClickRefresh();
          this.getFirstWeight();
          this.getSecondWeight();
          this.SecondWeightSlip(result.VchNo, result.FromDate, result.Vehicle, result.GpNo);
          this.ReceiveingReportSlip(result.VchNo, result.FromDate, result.Vehicle, result.GpNo);
          } else {
            this.com.hideLoader();
            this.tostr.error('Please Save Again');
          }
        });
    }


    FirstWeightSlip(VchNo:any, FromDate:any) {
      let form = this.WeighBridgeForm.value;
    
    let url = `FirstWeightSlip?VchType=RP-RAW&VchNo=${VchNo}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}&FromDate=${FromDate}&CmpId=${this.auth.cmpId()}`;
    this.com.viewReport(url);
  }

ReceiveingReportSlip(VchNo:any, FromDate:any, Vehicle:any, GpNo:any) {
    let form = this.WeighBridgeForm.value;
  
  let url = `ReceivingOfGoods?VchType=RP-RAW&VchNo=${VchNo}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}&FromDate=${FromDate}&VehNo=${Vehicle}&GpNo=${GpNo}&CmpId=${this.auth.cmpId()}`;
  this.com.viewReport(url);
}

SecondWeightSlip(VchNo:any, FromDate:any, Vehicle:any, GpNo:any) {
  let form = this.WeighBridgeForm.value;

let url = `SecondWeightSlip?VchType=RP-RAW&VchNo=${VchNo}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}&FromDate=${FromDate}&VehNo=${Vehicle}&GpNo=${GpNo}&CmpId=${this.auth.cmpId()}`;
this.com.viewReport(url);
}

  // editRQ(vchNo: any): void {
  //   this.onClickRefresh();
  //   this.isShow = true;
  //   this.editMode = true;

  //   const obj = {
  //     vchNo: vchNo,
  //   };

  //   this.apiService
  //     .getDataById('GeneralStore/GetEditRequisition', obj)
  //     .subscribe((data) => {
  //       this.togglePages();
  //       this.enableFields();
  //       data.forEach((item: any) => {
  //         this.WeighBridgeForm
  //           .get('date')
  //           ?.patchValue(
  //             new Date(
  //               item.DATE.split('/')[2],
  //               item.DATE.split('/')[1] - 1,
  //               item.DATE.split('/')[0]
  //             )
  //           );
  //         this.WeighBridgeForm.get('transNo')?.patchValue(item.VCHNO);
  //         let form = item;
  //         form.code = item.CODE;
  //         form.sub = item.SUB;
  //         form.itemName = item.ITEMNAME;
  //         form.qty = item.QTY;
  //         form.did = item.SUBDEPT;
  //         form.didMain = item.MAINDEPT;
  //         form.sno = item.SNO;
  //         form.requestFor = item.REQFOR;
  //         form.remarks = item.REMARKS;
  //         form.subDidName = item.SUBDIDNAME;
  //         form.mainDidName = item.MAINDIDNAME;
  //         this.requisitionList.push(form);
  //       });
  //     });
  // }

  // deleteRQ(VCHNO: any): void {
  //   const confirmDelete = confirm('Are you sure you want to delete this item?');

  //   if (confirmDelete == true) {
  //     const obj = {
  //       vchNo: VCHNO,
  //     };

  //     this.apiService.deleteData('GeneralStore/DelRequisition', obj).subscribe({
  //       next: (data) => {
  //         if (data == 'true' || data == true) {
  //           this.tostr.success('Delete Successfully');
  //           this.getRQList();
  //           this.getTransNo();
  //         } else if (data == 'false' || data == false) {
  //           this.tostr.error('Delete Again');
  //         }
  //       },
  //       error: (error) => {
  //         this.tostr.info(error.error.text);
  //       },
  //     });
  //   }
  // }

  // onClickNew() {
  //   this.isShow = true;
  //   this.editMode = false;
  //   this.enableFields();
  // }

  onClickRefresh() {
    this.isShow = false;

    this.getFirstWeight();
    this.getSecondWeight();
    // if(this.FirstWeightList.length > 0 || this.SecondWeightList.length > 0){
    //   this.isShow = true;
    // }

    
    this.resetForm();
    //this.requisitionList = [];
    this.LabDeductionList = [];
    this.disableFields();
    this.isSecondWeight = true;
    
  }

  enableFields() {
   this.isDisabled = false;
  }

  disableFields() {
    this.isDisabled = true;
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


  // WB Settings


  getWBSettings(){

    this.apiService.getData('WeighBridge/GetWBSettings').subscribe((data) => {
      
      this.baudRate = data.Baudrate;
      this.dataBits = data.DataBits;
      this.parity = data.Parity;
      this.stopBits = data.StopBits
      if ( this.WeighBridgeForm.get("ExpWt").value==0)
          {
            //this.WeighBridgeForm.get("AllowedWtDiff").setValue(data.InLimit);

          }
    });
  }
 // On Ctrl+S Save this Weighment
  @HostListener('document:keydown', ['$event'])
  handleKeyboardEvent(event: KeyboardEvent) {
    if (event.ctrlKey && event.key === 's') {
      this.onClickSave();
      event.preventDefault();
    }
  }

}