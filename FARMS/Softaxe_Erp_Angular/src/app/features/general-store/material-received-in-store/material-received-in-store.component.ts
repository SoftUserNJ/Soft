import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { isUndefined } from 'ngx-bootstrap/chronos/utils/type-checks';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-material-received-in-store',
  templateUrl: './material-received-in-store.component.html',
  styleUrls: ['./material-received-in-store.component.css']
})
export class MaterialReceivedInStoreComponent {
  
  constructor(
    private tostr: ToastrService,
    private fb: FormBuilder,
    private apiService: ApiService,
    private dp: DatePipe
  ) {
    
    const today = new Date();
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);

  }

  @ViewChild('voucherLists', { static: false }) voucherLists!: ElementRef;
  voucherList: any[] = [];
  fromDate: Date;
  toDate: Date;
 


  ngOnInit() {
    this.formInit();
    this.getItems();
    this.getParty();
    this.getUOM();
    this.getVoucherNo();
    this.getFrtype();
    this.getReceivedList();
    this.disableFields();
  }

  itemsList: any = [];
  UOMList: any = [];
  PartyList: any = [];
  FreightList: any = [];
  btnAdd:string = 'Add';
  editSno: any = '';
  editModeSno: boolean = false;
  editMode: boolean = true;
  ReceivedStoreForm!: FormGroup;
  ReceivedStoreList: any = [];
  isShow = false;
  isShowPage: boolean = true;


  
  formInit() {
    this.ReceivedStoreForm = this.fb.group({
      voucherType: ['RP-STORE'],
      RecievedDate: [new Date()],
      voucherNo: [''],
      lastGpi: [''],
      items: [undefined],
      Party: [undefined],
      freightType: [undefined],
      uom: [undefined],
      remarks: [''],
      itemCategory: [''],
      itemSubCategory: [''],
      billNo: [''],
      Freight: [''],
      VehNo: [''],
      StaxAmnt: [''],
      Bags: [''],
      Quantity: [''],
      rate: [''],
      value: [''],
      FreighType: [undefined]
    

    });
  }


  
  togglePages() {
    this.isShowPage = !this.isShowPage;
    if (this.isShowPage) {
     
    }
  }

  getParty() {
    this.apiService.getData('GeneralStore/GetParty').subscribe((data) => {
      this.PartyList = data;
    });
  }

  getUOM() {
    this.apiService.getData('GeneralStore/GetUOM').subscribe((data) => {
      this.UOMList = data;
    });
  }


  getItems() {
    this.apiService.getData('GeneralStore/GetItems').subscribe((data) => {
      this.itemsList = data;
    });
  }

  getFrtype() {
    this.apiService.getData('GeneralStore/GetFrType').subscribe((data) => {
      this.FreightList = data;
    });
  }



  resetForm() {
    this.ReceivedStoreForm.get('items')?.patchValue(undefined);
    this.ReceivedStoreForm.get('remarks')?.patchValue('');
    this.ReceivedStoreForm.get('itemCategory')?.patchValue('');
    this.ReceivedStoreForm.get('itemSubCategory')?.patchValue('');
    this.ReceivedStoreForm.get('Bags')?.patchValue('');
    this.ReceivedStoreForm.get('Quantity')?.patchValue('');
    this.ReceivedStoreForm.get('uom')?.patchValue(undefined);
    this.ReceivedStoreForm.get('rate')?.patchValue('');
    this.ReceivedStoreForm.get('value')?.patchValue('');
    this.ReceivedStoreForm.get('StaxAmnt')?.patchValue('');
    this.ReceivedStoreForm.get('Party')?.patchValue(undefined);
    this.ReceivedStoreForm.get('netAmnt')?.patchValue('');
    this.ReceivedStoreForm.get('Freight')?.patchValue('');
    this.ReceivedStoreForm.get('VehNo')?.patchValue('');
    this.ReceivedStoreForm.get('FreighType')?.patchValue(undefined);
    this.ReceivedStoreForm.get('billNo')?.patchValue('');
  }


  editItem(row: any) { debugger

    this.btnAdd = 'Update'

    this.editModeSno = true;
    this.editSno = row.sno;
    const code = row.code + row.sub;
    const uomcode = row.uomCode;
    const PartyCode = row.PartyCode;
    const FreighType = row.FreighType;
    this.ReceivedStoreForm.get('remarks')?.patchValue(row.remarks);
    this.ReceivedStoreForm.get('items')?.patchValue(code);
    this.ReceivedStoreForm.get('rate')?.patchValue(row.rate);
    this.ReceivedStoreForm.get('value')?.patchValue(row.value);
    this.ReceivedStoreForm.get('StaxAmnt')?.patchValue(row.StaxAmnt);
    this.ReceivedStoreForm.get('Bags')?.patchValue(row.Bags);
    this.ReceivedStoreForm.get('Quantity')?.patchValue(row.Quantity);
    this.ReceivedStoreForm.get('uom')?.patchValue(uomcode);
    this.ReceivedStoreForm.get('Party')?.patchValue(PartyCode);
    this.ReceivedStoreForm.get('FreighType')?.patchValue(FreighType);
    this.ReceivedStoreForm.get('Freight')?.patchValue(row.Freight);
    this.ReceivedStoreForm.get('VehNo')?.patchValue(row.VehNo);
    this.ReceivedStoreForm.get('billNo')?.patchValue(row.billNo);
  }


  deleteItem(row: any) {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (!confirmDelete) {
      return;
    }

    const indexToRemove = this.ReceivedStoreList.findIndex(
      (item) => item.sno === row.sno
    );
    if (indexToRemove !== -1) {
      this.ReceivedStoreList.splice(indexToRemove, 1);
    }
  }


  onAdd() { debugger

    let form = this.ReceivedStoreForm.value;

    if (form.itemCategory === null || form.itemCategory === undefined) {
      this.tostr.warning('Select Main Category....!');
      return;
    }

    if (form.itemSubCategory === null || form.itemSubCategory === undefined) {
      this.tostr.warning('Select Sub Category....!');
      return;
    }

    if (form.items === null || form.items === undefined) {
      this.tostr.warning('Select Item....!');
      return;
    }

    if (form.Party === null || form.Party === undefined) {
      this.tostr.warning('Select Party....!');
      return;
    }

    
    if (form.FreighType === null || form.FreighType === undefined) {
      this.tostr.warning('Select Freight Type');
      return;
    }

    if (form.uom === null || form.uom === undefined) {
      this.tostr.warning('Select UOM');
      return;
    }

    let itemName = this.itemsList.find((i) => i.code === form.items);
    let PartyCode = this.PartyList.find((i) => i.code === form.Party);
    let uom = this.UOMList.find((i) => i.id === form.uom);
    let FreighType = this.FreightList.find((i) => i.Id === form.FreighType);
    form.code = form.items.substring(0, 9);
    form.sub = form.items.substring(9, 14);
    form.itemName = itemName.name;
    form.PartyCode = PartyCode.code;
    form.FreighType = FreighType.Id;
    form.uom = uom.name;
    form.uomCode = uom.id;
    form.Bags = form.Bags;
    
    let netAmnt = (form.value * form.StaxAmnt)+(form.rate * form.Quantity);
    form.netAmnt = netAmnt;


    // const existingRow = this.ReceivedStoreList.find((row) => row.code === form.code && row.sub === form.sub);

    // if (existingRow) {
    //   this.tostr.warning('Row already exists in the list!');
    //   return;
    // }
    
    if (this.editModeSno) {
      const index = this.ReceivedStoreList.findIndex(
        (row) => row.sno === this.editSno
      );
      if (index !== -1) {
        form.sno = this.editSno;
        this.ReceivedStoreList[index] = form;
        this.editModeSno = false;
        this.editSno = '';
        this.btnAdd = 'Add'
        this.resetForm();
        return;
      }
    }

    form.sno = this.ReceivedStoreList.length + 1;
    this.ReceivedStoreList.push(form);
    this.resetForm();
  }

  enableFields() {
    this.ReceivedStoreForm.get('itemCategory').enable();
    this.ReceivedStoreForm.get('itemSubCategory').enable();
    this.ReceivedStoreForm.get('remarks').enable();
    this.ReceivedStoreForm.get('items').enable();
    this.ReceivedStoreForm.get('Freight').enable();
    this.ReceivedStoreForm.get('billNo').enable();
    this.ReceivedStoreForm.get('VehNo').enable();
    this.ReceivedStoreForm.get('FreighType').enable();
    this.ReceivedStoreForm.get('Party').enable();
    this.ReceivedStoreForm.get('rate').enable();
    this.ReceivedStoreForm.get('value').enable();
    this.ReceivedStoreForm.get('StaxAmnt').enable();
    this.ReceivedStoreForm.get('Bags').enable();
    this.ReceivedStoreForm.get('Quantity').enable();
    this.ReceivedStoreForm.get('uom').enable();
    this.ReceivedStoreForm.get('Party').enable();
    this.ReceivedStoreForm.get('FreighType').enable();
  }

  disableFields() {
    this.ReceivedStoreForm.get('itemCategory').disable();
    this.ReceivedStoreForm.get('itemSubCategory').disable();
    this.ReceivedStoreForm.get('remarks').disable();
    this.ReceivedStoreForm.get('items').disable();
    this.ReceivedStoreForm.get('Freight').disable();
    this.ReceivedStoreForm.get('billNo').disable();
    this.ReceivedStoreForm.get('VehNo').disable();
    this.ReceivedStoreForm.get('FreighType').disable();
    this.ReceivedStoreForm.get('Party').disable();
    this.ReceivedStoreForm.get('rate').disable();
    this.ReceivedStoreForm.get('value').disable();
    this.ReceivedStoreForm.get('StaxAmnt').disable();
    this.ReceivedStoreForm.get('Bags').disable();
    this.ReceivedStoreForm.get('Quantity').disable();
    this.ReceivedStoreForm.get('uom').disable();
    this.ReceivedStoreForm.get('Party').disable();
    this.ReceivedStoreForm.get('FreighType').disable();
    
  }

  getVoucherNo() {
    this.apiService.getData('GeneralStore/GetVoucherNo').subscribe((data) => {
      this.ReceivedStoreForm.get('voucherNo')?.setValue(data[0].VoucherNo);
      this.ReceivedStoreForm.get('lastGpi')?.patchValue((data[0].VoucherNo)-1);
      
    });
  }


  onClickRefresh() {
    this.isShow = false;
    this.resetForm();
   // this.getTransNo();
    this.ReceivedStoreList = [];
    this.disableFields();
  }

  onClickNew() {
    this.isShow = true;
    this.editMode = false;
    this.enableFields();
  }

  getReceivedList() {
    const obj = {
      fromDate: this.dp.transform(this.fromDate, 'yyyy/MM/dd'),
      toDate: this.dp.transform(this.toDate, 'yyyy/MM/dd'),
    };

    this.apiService
      .getDataById('GeneralStore/GetReceivedList', obj)
      .subscribe((data) => {
        this.voucherList = data;
      });
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


  onClickSave() { debugger
    if (this.ReceivedStoreList.length == 0) {
      this.tostr.warning('Incomplete Transaction...');
      return;
    }

    let vchNo = this.editMode ? this.ReceivedStoreForm.get('voucherNo')?.value : 0;

    const voucher: any[] = this.ReceivedStoreList.map((data) => {
      const freightTypeString = data.FreighType.toString();
      const uomCodeString = data.uomCode.toString();
      const billNoString = data.billNo.toString();
  
      return {
        sno: data.sno,
        vchNo: vchNo,
        VchDate: this.dp.transform(this.ReceivedStoreForm.get('RecievedDate')?.value, 'yyyy-MM-dd'),
        remarks: data.remarks,
        itemCode: data.code + data.sub,
        Bags: data.Bags,
        FreightType: freightTypeString,
        Freight: data.Freight,
        PartyCode: data.PartyCode,
        Quantity: data.Quantity,
        StaxAmnt: data.StaxAmnt,
        VehNo: data.VehNo,
        billNo: billNoString,
        rate: data.rate,
        value: data.value,
        uomCode: uomCodeString,
      };
    });

    this.apiService
      .saveData('GeneralStore/SaveStoreReceive', voucher)
      .subscribe((result) => {
        if (result == true || result == 'true') {
          this.tostr.success('Save Successfully');
          this.onClickRefresh();
          this.getReceivedList();
        } else {
          this.tostr.error('Please Save Again');
        }
      });
  }

  editRM(vchNo: any): void { debugger
    this.onClickRefresh();
    this.isShow = true;
    this.editMode = true;

    const obj = {
      vchNo: vchNo,
    };

    this.apiService
      .getDataById('GeneralStore/GetEditReceiveStore', obj)
      .subscribe((data) => {
        this.togglePages();
        this.enableFields();
        data.forEach((item: any) => {
          this.ReceivedStoreForm
            .get('RecievedDate')
            ?.patchValue(
              new Date(
                item.DATE.split('/')[2],
                item.DATE.split('/')[1] - 1,
                item.DATE.split('/')[0]
              )
            );
          this.ReceivedStoreForm.get('voucherNo')?.patchValue(item.VCHNO);
          let form = item;
          form.code = item.CODE;
          form.sub = item.SUB;
          form.itemName = item.ITEMNAME;
          form.Quantity = item.QTY;
          form.sno = item.SNO;
          form.remarks = item.REMARKS;
          form.FreighType = parseInt(item.FreightType, 10);
          form.VehNo = item.VehicleNo;
          form.Freight = item.Freight;
          form.value = item.Debit;
          form.billNo = item.Imported;
          form.rate = item.Rate;
          form.StaxAmnt = item.SalesTax;
          form.Bags = item.Bags;
          form.lastGpi = (item.VCHNO) - 1;
          form.uomCode = parseInt(item.uom, 10);
          form.uom = item.UomName;
          form.PartyCode = item.Mcode+item.SubCode;

          let netAmnt = (item.value * item.StaxAmnt)+(item.rate * item.Quantity);
          form.netAmnt = netAmnt;
         
          this.ReceivedStoreList.push(form);
        });
      });
  }

  deleteRM(VCHNO: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {
      const obj = {
        vchNo: VCHNO,
      };

      this.apiService.deleteData('GeneralStore/DelReceiveStore', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.tostr.success('Delete Successfully');
            this.getReceivedList();
            this.getVoucherNo();
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

}
