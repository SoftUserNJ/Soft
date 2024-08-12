import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-demand-entry',
  templateUrl: './demand-entry.component.html',
  styleUrls: ['./demand-entry.component.css']
})
export class DemandEntryComponent {
  
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

  demandForm!: FormGroup;
  DemandList: any = [];
  isShow = false;
  isReadOnly: boolean = true;
  editMode: boolean = true;
  isShowPage: boolean = true;
  editSno: any = '';
  editModeSno: boolean = false;
  selectedRow: number | null = null;
  btnAdd:string = 'Add';


  
  ngOnInit() {

    this.formInit();
    this.getItems();
    this.getDemandList();
    this. getDemandNo();
    this.getLastDemand();
    this.disableFields();
  
  }


  itemsList: any = [];


  formInit() {
    this.demandForm = this.fb.group({
      voucherType: ['RQ-DEMAND'],
      demandDate: [new Date()],
      demandNo: [''],
      balance: [''],
      items: [undefined],
      remarks: [''],
      itemCategory: [''],
      itemSubCategory: [''],
      demandVal: [''],
      minLevel: [''],
      maxlevel: [''],
      balanceQty: [''],
      minRate: [''],
      maxRate: [''],
      LastNo: [''],
      lastDate: [new Date()],
      lastRate: [''],
      LastPurRate: [''],
      lastParty: [''],

    });
  }

  togglePages() {
    this.isShowPage = !this.isShowPage;
    if (this.isShowPage) {
     
    }
  }

  onClickNew() {
    this.isShow = true;
    this.editMode = false;
    this.enableFields();
  }

  onClickRefresh() {
    this.isShow = false;
    this.resetForm();
   // this.getTransNo();
    this.DemandList = [];
    this.disableFields();
  }


  disableFields() {
    this.demandForm.get('itemCategory').disable();
    this.demandForm.get('itemSubCategory').disable();
    this.demandForm.get('remarks').disable();
    this.demandForm.get('items').disable();
    this.demandForm.get('demandVal').disable();
  }



  resetForm() {
    this.demandForm.get('items')?.patchValue(undefined);
    this.demandForm.get('remarks')?.patchValue('');
    this.demandForm.get('itemCategory')?.patchValue('');
    this.demandForm.get('itemSubCategory')?.patchValue('');
    this.demandForm.get('demandVal')?.patchValue('');
  }


  enableFields() {
    this.demandForm.get('itemCategory').enable();
    this.demandForm.get('itemSubCategory').enable();
    this.demandForm.get('remarks').enable();
    this.demandForm.get('items').enable();
    this.demandForm.get('demandVal').enable();
  }


  onAdd() { debugger

    let form = this.demandForm.value;

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

    if (form.demandVal === '' || form.demandVal === 0 || form.demandVal === "0") {
      this.tostr.warning('Enter Demand');
      return;
    }

    let itemName = this.itemsList.find((i) => i.code === form.items);
    form.code = form.items.substring(0, 9);
    form.sub = form.items.substring(9, 14);
    form.itemName = itemName.name;
    form.demandVal = form.demandVal;

    // const existingRow = this.DemandList.find((row) => row.code === form.code && row.sub === form.sub);

    // if (existingRow) {
    //   this.tostr.warning('Row already exists in the list!');
    //   return;
    // }
    
    if (this.editModeSno) {
      const index = this.DemandList.findIndex(
        (row) => row.sno === this.editSno
      );
      if (index !== -1) {
        form.sno = this.editSno;
        this.DemandList[index] = form;
        this.editModeSno = false;
        this.editSno = '';
        this.btnAdd = 'Add'
        this.resetForm();
        return;
      }
    }

    form.sno = this.DemandList.length + 1;
    this.DemandList.push(form);
    this.resetForm();
  }


  onlyNumeric(event: any): void {
    const inputVal = event.target.value;
    // Replace non-numeric characters using a regular expression
    const cleanValue = inputVal.replace(/[^0-9]/g, '');
    // Update the form control value with the cleaned numeric value
    this.demandForm.patchValue({ quantity: cleanValue }, { emitEvent: false });
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


  getDemandNo() {
    this.apiService.getData('GeneralStore/GetDemandNo').subscribe((data) => {
      this.demandForm.get('demandNo')?.setValue(data[0].demandNo);
      this.demandForm.get('demandDate')?.patchValue(new Date());
      this.demandForm.get('LastNo')?.patchValue((data[0].demandNo)-1);
    });
  }

  getItems() {
    this.apiService.getData('GeneralStore/GetItems').subscribe((data) => {
      this.itemsList = data;
    });
  }

  getItemsCategory(event: any) { debugger
    this.apiService
      .getDataById('GeneralStore/GetItemsCategoryById', { itemId: event.code })
      .subscribe((data) => {
        console.log(data);
        this.demandForm
          .get('itemCategory')
          ?.setValue(data.category[0].name);
        this.demandForm
          .get('itemSubCategory')
          ?.setValue(data.subCategory[0].name);
          this.demandForm
          .get('minLevel')
          ?.setValue(data.subCategory[0].MinLevel);
          this.demandForm
          .get('balanceQty')
          ?.setValue(data.itemBalance[0].BALQTY);
          this.demandForm
          .get('maxlevel')
          ?.setValue(data.subCategory[0].MaxLevel);
      });
  }

  getLastDemand() {
    this.apiService.getData('GeneralStore/GetLastDemand').subscribe((data)=>{
      this.demandForm.get('maxRate')?.setValue(data[0].maxRate);
      this.demandForm.get('minRate')?.setValue(data[0].minRate);
      this.demandForm.get('lastRate')?.setValue(data[0].lastRate);
      this.demandForm.get('lastParty')?.setValue(data[0].lastParty);
      this.demandForm.get('lastDate')?.setValue(data[0].lastVchDate);
    })

  }

  
  deleteItem(row: any) {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (!confirmDelete) {
      return;
    }

    const indexToRemove = this.DemandList.findIndex(
      (item) => item.sno === row.sno
    );
    if (indexToRemove !== -1) {
      this.DemandList.splice(indexToRemove, 1);
    }
  }


  onClickSave() { debugger
    if (this.DemandList.length == 0) {
      this.tostr.warning('Incomplete Transaction...');
      return;
    }

    let vchNo = this.editMode ? this.demandForm.get('demandNo')?.value : 0;

    // let vchNo = 0
    // if(this.editMode == true){vchNo = this.requisitionForm.get('transNo')?.value}
    // else {vchNo = 0}

    const voucher: any[] = this.DemandList.map((data) => ({
      sno: data.sno,
      vchNo: vchNo,
      date: this.dp.transform(
        this.demandForm.get('demandDate')?.value,
        'yyyy-MM-dd'
      ),
      remarks: data.remarks,
      itemCode: data.code + data.sub,
      demandVal: data.demandVal,
    }));

    this.apiService
      .saveData('GeneralStore/SaveDemand', voucher)
      .subscribe((result) => {
        if (result == true || result == 'true') {
          this.tostr.success('Save Successfully');
          this.onClickRefresh();
          this.getDemandNo();
          this.getDemandList();
        } else {
          this.tostr.error('Please Save Again');
        }
      });
  }


  editItem(row: any) { debugger

    this.btnAdd = 'Update'

    this.editModeSno = true;
    this.editSno = row.sno;
    const code = row.code + row.sub;
    this.demandForm.get('remarks')?.patchValue(row.remarks);
    this.demandForm.get('items')?.patchValue(code);
    this.demandForm.get('demandVal')?.patchValue(row.demandVal);
    this.getItemsCategory({ code: code });
  }

  getDemandList() {
    const obj = {
      fromDate: this.dp.transform(this.fromDate, 'yyyy/MM/dd'),
      toDate: this.dp.transform(this.toDate, 'yyyy/MM/dd'),
    };

    this.apiService
      .getDataById('GeneralStore/GetDemand', obj)
      .subscribe((data) => {
        this.voucherList = data;
      });
  }


  editDM(vchNo: any): void { debugger
    this.onClickRefresh();
    this.isShow = true;
    this.editMode = true;

    const obj = {
      vchNo: vchNo,
    };

    this.apiService
      .getDataById('GeneralStore/GetEditDemand', obj)
      .subscribe((data) => {
        this.togglePages();
        this.enableFields();
        data.forEach((item: any) => {
          this.demandForm
            .get('demandDate')
            ?.patchValue(
              new Date(
                item.DATE.split('/')[2],
                item.DATE.split('/')[1] - 1,
                item.DATE.split('/')[0]
              )
            );
          this.demandForm.get('demandNo')?.patchValue(item.VCHNO);
          this.demandForm.get('LastNo')?.patchValue(item.LastVchNo);
          let form = item;
          form.code = item.CODE;
          form.sub = item.SUB;
          form.itemName = item.ITEMNAME;
          form.demandVal = item.QTY;
          form.sno = item.SNO;
          form.LastNo = item.LastVchNo;
          form.requestFor = item.REQFOR;
          form.remarks = item.REMARKS;
         
          this.DemandList.push(form);
        });
      });
  }

  deleteDM(VCHNO: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {
      const obj = {
        vchNo: VCHNO,
      };

      this.apiService.deleteData('GeneralStore/DelDemand', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.tostr.success('Delete Successfully');
            this.getDemandList();
            this.getDemandNo();
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
 

}
