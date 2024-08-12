import { Component, ViewChild, ElementRef, Input, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { environment } from '../../../../environment/environmemt';
import { CommonService } from 'src/app/services/common.service';


@Component({
  selector: 'app-sale-gate-pass-out',
  templateUrl: './sale-gate-pass-out.component.html',
  styleUrls: ['./sale-gate-pass-out.component.css']
})
export class SaleGatePassOutComponent {

  constructor(
    private apiService: ApiService,
    private fb: FormBuilder,
    private tostr: ToastrService,
    private dp: DatePipe,
    private auth: AuthService,
    private com: CommonService,
  ) {
    const today = new Date();
    this.today = today;
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  }

  // Company
  exportDetails:boolean = Boolean(this.auth.exportDetail());
  
  today: Date;
  @ViewChild('InvoiceList', { static: false }) InvoiceList!: ElementRef;
  
  basePath = environment.basePath;
  gatePassOutForm!: FormGroup;
  editMode: boolean = true;

  // INVOICE LIST
  fromDate: Date;
  toDate: Date;
  invoiceList: any[] = [];

  godownsList: any[] = [];
  itemsList: any[] = [];
  partyList: any[] = [];
  transporter: any[] = [];
  
  doListOutPass: any[] = [];
  doListDetailOutPass: any[] = [];
  gpDetailsList: any[] = [];

  // OTHER
  isNewClick: boolean = false;
  isDisabled: boolean = true;
  isShowPage: boolean = true;


   ngOnInit() {
    this.formInit();
    this.getGodowns();
    this.getParty();
    this.getItemsList();
    this.getTransporterList();
    this.resetForm();
  }

  formInit() {
    this.gatePassOutForm = this.fb.group({
      vchNo: [0, Validators.required],
      vchType: ['SP', Validators.required],
      vchDate: [new Date(), Validators.required],
      extraFreight: [''],
      freightSubParty: [''],
      freight: [''],

      vehicleNo: [''],
      driverName: [''],
      driverContact: [''],
      driverCNIC: [''],
      biltyNo: [''],
      cropYear: [''],

      fwdBooking1: [''],
      fwdBooking2: [''],
      country1: [''],
      country2: [''],
      port1: [''],
      port2: [''],
      containerNo1: [''],
      containerNo2: [''],
      sealNo1: [''],
      sealNo2: [''],

      containerSize: [''],
      transporter: [undefined],
    });

  }

  resetForm() {
    this.isNewClick = false;
 
    this.gatePassOutForm.reset();
    this.gatePassOutForm.get('vchNo').setValue(0);
    this.gatePassOutForm.get('vchType').setValue('SP');
    this.gatePassOutForm.get('vchDate').setValue(new Date());
    this.gatePassOutForm.get('extraFreight').setValue('');
    this.gatePassOutForm.get('freightSubParty').setValue('');
    this.gatePassOutForm.get('freight').setValue('');
    this.gatePassOutForm.get('vehicleNo').setValue('');
    this.gatePassOutForm.get('driverName').setValue('');
    this.gatePassOutForm.get('driverContact').setValue('');
    this.gatePassOutForm.get('driverCNIC').setValue('');
    this.gatePassOutForm.get('biltyNo').setValue('');
    this.gatePassOutForm.get('cropYear').setValue('');
    this.gatePassOutForm.get('containerSize').setValue('');
    this.gatePassOutForm.get('transporter').setValue(undefined);
    this.gatePassOutForm.get('fwdBooking1').setValue('');
    this.gatePassOutForm.get('fwdBooking2').setValue('');
    this.gatePassOutForm.get('country1').setValue('');
    this.gatePassOutForm.get('country2').setValue('');
    this.gatePassOutForm.get('port1').setValue('');
    this.gatePassOutForm.get('port2').setValue('');
    this.gatePassOutForm.get('containerNo1').setValue('');
    this.gatePassOutForm.get('containerNo2').setValue('');
    this.gatePassOutForm.get('sealNo1').setValue('');
    this.gatePassOutForm.get('sealNo2').setValue('');
  }

  onClickNew() {
    this.isDisabled = false;
    this.isNewClick = true;
    this.getMaxVchNo();
  }

  onClickRefresh() {
    this.isDisabled = true;
    this.resetForm();
  }

  async getMaxVchNo() {
    const result = await this.apiService
      .getData('Sale/GetMaxVchNoGatePassOut')
      .toPromise();
      this.gatePassOutForm.get('vchNo').patchValue(result[0].VCHNO);
  }

  async getGodowns() {
    const result = await this.apiService
      .getData('Common/GetGodowns')
      .toPromise();
      this.godownsList = result;
  }

  async getParty() {
    this.apiService.getDataById('Common/GetDirectL4L5CodeNamesByL4Tag', { l4Tag: 'C' }).subscribe((data) => {
      this.partyList = data;
    })
  }

  async getItemsList() {
    this.apiService.getDataById('Common/GetDirectL4L5CodeNamesByL4Tag', { l4Tag: 'S' }).subscribe((data) => {
      this.itemsList = data;
    })
  }

  async getTransporterList() {
    this.apiService.getDataById('Common/GetDirectL4L5CodeNamesByL4Tag', { l4Tag: 'F' }).subscribe((data) => {
      this.transporter = data;
    })
  }

  getInvoices() {
    const obj = {
      fromDate: this.dp.transform(this.fromDate, 'yyyy/MM/dd'),
      toDate: this.dp.transform(this.toDate, 'yyyy/MM/dd'),
      vchType: this.gatePassOutForm.get('vchType').value,
    };

    this.apiService
      .getDataById('Sale/GetDoList', obj)
      .subscribe((data) => {
        this.invoiceList = data;
      });
  }

  GetDoListOutPass() {
    const obj = {
      fromDate: this.dp.transform(this.fromDate, 'yyyy/MM/dd'),
      toDate: this.dp.transform(this.toDate, 'yyyy/MM/dd'),
      vchType: 'Do-Sales',
      vchNo: this.gatePassOutForm.get('vchNo').value,
    };

    this.apiService
      .getDataById('Sale/GetDoListOutPass', obj)
      .subscribe((data) => {
        this.doListOutPass = data;
      });
  }

  
  GetDoDetailListOutPass(event: MouseEvent, item:any){

    if (event.detail === 2) {
        return;
    }

    const d = 
      new Date(
        item.DoDate.split('/')[2],
        item.DoDate.split('/')[1] - 1,
        item.DoDate.split('/')[0]
      );

    const obj = {
      doDate: this.dp.transform(d, 'yyyy/MM/dd'),
      vchType: item.VchType,
      vchNo: item.DONO,
    };

    this.apiService
      .getDataById('Sale/GetDoDetailListOutPass', obj)
      .subscribe((data) => {
        this.doListDetailOutPass = data;
      });
  }

  AppendDo(party:any){

    const d = 
      new Date(
        party.DoDate.split('/')[2],
        party.DoDate.split('/')[1] - 1,
        party.DoDate.split('/')[0]
      );

    const obj = {
      doDate: this.dp.transform(d, 'yyyy/MM/dd'),
      vchType: party.VchType,
      vchNo: party.DONO,
    };

       this.apiService
      .getDataById('Sale/GetDoDetailListOutPass', obj)
      .subscribe((data: any[]) => {

        const { DeliverQty, GQTY, ...rest } = party;
        const newParty = rest;

        this.gpDetailsList = data.map((item: any) => ({ ...item, ...newParty }));

        debugger;

        console.log(this.gpDetailsList); // For debugging
      });
  }

  // onAdd(gpDetailsList) {
  //   let form = gpDetailsList;

    // let form = {
    //   DONO: undefined,
    //   category: undefined,
    //   uom: undefined,
    //   cropYear: undefined,
    //   vehicles: '',
    //   qty: '',
    //   rate: '',
    //   amount: '',
    //   sTax: '',
    //   sTaxAmount: '',
    //   netAmount: '',
    // };
  //   this.gpDetailsList.push(form);
  // }

  deleteItem(row: any) {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (!confirmDelete) {
      return;
    }

    const indexToRemove = this.gpDetailsList.findIndex(
      (item) => item.sno === row.sno
    );
    if (indexToRemove !== -1) {
      this.gpDetailsList.splice(indexToRemove, 1);
    }

  }

  onClickSave() {

    const form = this.gatePassOutForm.value;

    if(form.vehicleNo === undefined || form.vehicleNo === null || form.vehicleNo === ""){ 
      this.tostr.warning('Enter Vehical No....!');
      return;
    }


    let vchNo = this.editMode ? this.gatePassOutForm.get('vchNo')?.value : 0;


    const voucher: any[] = this.gpDetailsList.map((data) => ({

      dono: data.DONO ?? "",
      doVchType: data.VchType ?? "",
      doDate: data.DoDate ?? "",
      doParty: data.partyCode ?? "",
      doItem: data.itemCode ?? "",
      doSubParty: data.partyCode ?? "",
      doQty: data.Qty ?? "",
      deliverQty: data.Qty ?? "",
      doFreight: data.doFreight ?? "",
      doGodown: data.godown ?? "",
      
      vchNo: vchNo,
      vchType: form.vchType,
      vchDate: form.vchDate,
      extraFreight: form.extraFreight,
      freightSubParty: form.freightSubParty,
      freight: form.freight,
      vehicleNo: form.vehicleNo,
      driverName: form.driverName,
      driverContact: form.driverContact,
      driverCNIC: form.driverCNIC,
      biltyNo: form.biltyNo,
      cropYear: form.cropYear,
      containerSize: form.containerSize,
      transporter: form.transporter,
      fwdBooking1: form.fwdBooking1,
      fwdBooking2: form.fwdBooking2,
      country1: form.country1,
      country2: form.country2,
      port1: form.port1,
      port2: form.port2,
      containerNo1: form.containerNo1,
      containerNo2: form.containerNo2,
      sealNo1: form.sealNo1,
      sealNo2: form.sealNo2,
      // broker: form.broker  ?? "",
      // incomeTax: isNaN(parseFloat(form.incomeTax)) ? 0 : parseFloat(form.incomeTax),
      // brockerCom: form.brokerCom > 0 ? form.brokerCom : 0,
     
    }));

    // voucher.forEach(item => {
    //     if (!item.item || item.item === null || item.item === "") {
    //       this.tostr.warning('Select Item....!');
    //       return;
    //     }
    //     if (!item.qty || isNaN(item.qty) || item.qty === null || item.qty === "") {
    //       this.tostr.warning('Enter Qty....!');
    //       return;
    //     }
    //     if (!item.rate || isNaN(item.rate) || item.rate === null || item.rate === "") {
    //       this.tostr.warning('Enter Rate....!');
    //       return;
    //     }
    // });


    debugger;


    this.apiService
      .saveData('Sale/SaveSaleGatePassOut', voucher)
      .subscribe((result) => {
        if (result == true || result == 'true') {
          this.tostr.success('Save Successfully');
          this.onClickRefresh();
          //this.getVchNo();
          //this.getVouchersList();
        } else {
          this.tostr.error('Please Save Again');
        }
      });
  }

  deleteInvoice(invNo: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (!confirmDelete) {
      return;
    }

    try {
      this.com.showLoader();
      const obj = {
        vchno: invNo,
        vchType: this.gatePassOutForm.get('vchType').value,
        dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      };

      this.apiService.deleteData('Sale/DeleteDo', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.getInvoices();
            this.onClickRefresh();
            this.com.hideLoader();
            this.tostr.success('Delete Successfully');
          } else if (data == 'false' || data == false) {
            this.com.hideLoader();
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

  async editInvoice(invNo: any) {

  }


 
  // =================== OTHER FUNCTIONS ==================//

  getNum(val) {
    if (val == '') {
      val = 0;
    }
    if (isNaN(val) || val == Infinity) {
      return 0;
    }
    return val;
  }

  onLength(event: any, length: number) {
    if (event.target.value > length) {
      event.target.value = length;
    }
  }

  onZero(e: any) {
    if (e.target.value == '' || e.target.value == null) {
      e.target.value = 0;
    }
  }
  
  isRound(value: any) {
    return this.com.roundVal(value);
  }

  togglePages() {
    this.isShowPage = !this.isShowPage;
    if (this.isShowPage) {
      //this.onClickRefresh();
    }
  }

  onSearchInput(event: any): void {
    const tableElement = this.InvoiceList.nativeElement;
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


}
