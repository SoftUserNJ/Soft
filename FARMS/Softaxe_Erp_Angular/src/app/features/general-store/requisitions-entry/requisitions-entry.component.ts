import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-requisitions-entry',
  templateUrl: './requisitions-entry.component.html',
  styleUrls: ['./requisitions-entry.component.css']
})
export class RequisitionsEntryComponent {

  
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
  requisitionForm!: FormGroup;
  requisitionList: any = [];
  isShow = false;
  isReadOnly: boolean = true;
  editMode: boolean = true;
  isShowPage: boolean = true;
  editSno: any = '';
  editModeSno: boolean = false;
  selectedRow: number | null = null;
  btnAdd: string = 'Add';

  //Department
  mainDepartmentList: any = [];
  subDepartmentList: any = [];

  //Items
  itemsList: any = [];
  itemsCategory: any;
  itemsSubCategory: any;

  ngOnInit() {
    this.getRQList();
    this.formInit();
    this.getTransNo();
    this.getMainDepartment();
    this.getItems();
  }

  formInit() {
    this.requisitionForm = this.fb.group({
      voucherType: ['RQ-Store'],
      date: [new Date()],
      transNo: [''],
      balance: [''],
      requestFor: [undefined],
      mainDepartment: [undefined],
      subDepartment: [undefined],
      items: [undefined],
      remarks: [''],
      itemCategory: [''],
      itemSubCategory: [''],
      quantity: [''],
    });
  }

  resetForm() {
    this.requisitionForm.get('requestFor')?.patchValue(undefined);
    this.requisitionForm.get('mainDepartment')?.patchValue(undefined);
    this.requisitionForm.get('subDepartment')?.patchValue(undefined);
    this.requisitionForm.get('items')?.patchValue(undefined);
    this.requisitionForm.get('remarks')?.patchValue('');
    this.requisitionForm.get('itemCategory')?.patchValue('');
    this.requisitionForm.get('itemSubCategory')?.patchValue('');
    this.requisitionForm.get('quantity')?.patchValue('');
    this.onMainDepartmentClear();
  }

  getRQList() {
    const obj = {
      fromDate: this.dp.transform(this.fromDate, 'yyyy/MM/dd'),
      toDate: this.dp.transform(this.toDate, 'yyyy/MM/dd'),
    };

    this.apiService
      .getDataById('GeneralStore/GetRequisition', obj)
      .subscribe((data) => {
        this.voucherList = data;
      });
  }

  getTransNo() {
    this.apiService.getData('GeneralStore/GetTransNo').subscribe((data) => {
      this.requisitionForm.get('transNo')?.setValue(data[0].transNo);
      this.requisitionForm.get('date')?.patchValue(new Date());
    });
  }
  getMainDepartment() {
    this.apiService.getData('GeneralStore/GetMainDepartment').subscribe((data) => {
      this.mainDepartmentList = data;
    });
  }

  onMainDepartmentClear() {
    this.requisitionForm.get('subDepartment')?.patchValue(undefined);
    this.subDepartmentList = [];
  }

  getSubDepartment(event: any) {
    this.onMainDepartmentClear();
    this.mainDepartMentId = event.id;
    this.apiService
      .getDataById('GeneralStore/GetSubDepartmentById', { mainId: event.id })
      .subscribe((data) => {
        this.subDepartmentList = data;
      });
  }

  getItems() {
    this.apiService.getData('GeneralStore/GetItems').subscribe((data) => {
      this.itemsList = data;
    });
  }

  onItemClear() {
    this.requisitionForm.get('itemCategory')?.patchValue('');
    this.requisitionForm.get('itemSubCategory')?.patchValue('');
  }

  getItemsCategory(event: any) {
    this.apiService
      .getDataById('GeneralStore/GetItemsCategoryById', { itemId: event.code })
      .subscribe((data) => {
        this.requisitionForm
          .get('itemCategory')
          ?.setValue(data.category[0].name);
        this.requisitionForm
          .get('itemSubCategory')
          ?.setValue(data.subCategory[0].name);
        this.requisitionForm
          .get('balance')
          ?.setValue(data.itemBalance[0].BALQTY);
      });
  }

  onAdd() {
    let form = this.requisitionForm.value;

    if (form.requestFor === null || form.requestFor === undefined) {
      this.tostr.warning('Select Request For....!');
      return;
    }

    if (form.mainDepartment === null || form.mainDepartment === undefined) {
      this.tostr.warning('Select Main Dept....!');
      return;
    }

    if (form.subDepartment === null || form.subDepartment === undefined) {
      this.tostr.warning('Select Sub Dept....!');
      return;
    }

    if (form.items === null || form.items === undefined) {
      this.tostr.warning('Select Item....!');
      return;
    }

    if (form.quantity === '' || form.quantity === 0 || form.quantity === '0') {
      this.tostr.warning('Enter Qty');
      return;
    }

    let itemName = this.itemsList.find((i) => i.code === form.items);
    let mainDid = this.mainDepartmentList.find(
      (i) => i.id === form.mainDepartment
    );
    let subDid = this.subDepartmentList.find(
      (i) => i.id === form.subDepartment
    );
    form.code = form.items.substring(0, 9);
    form.sub = form.items.substring(9, 14);
    form.itemName = itemName.name;
    form.qty = form.quantity;

    form.mainDidName = mainDid.name;
    form.subDidName = subDid.name;
    form.did = form.subDepartment;
    form.didMain = form.mainDepartment;

    if (this.editModeSno) {
      const index = this.requisitionList.findIndex(
        (row) => row.sno === this.editSno
      );
      if (index !== -1) {
        form.sno = this.editSno;
        this.requisitionList[index] = form;
        this.editModeSno = false;
        this.editSno = '';
        this.btnAdd = 'Add';
        // this.resetForm();
        return;
      }
    }

    form.sno = this.requisitionList.length + 1;
    this.requisitionList.push(form);
    // this.resetForm();
  }

  editItem(row: any) {
    this.btnAdd = 'Update';

    this.editModeSno = true;
    this.editSno = row.sno;
    this.requisitionForm.get('requestFor')?.patchValue(row.requestFor);
    const code = row.code + row.sub;
    this.requisitionForm.get('mainDepartment')?.patchValue(row.didMain);
    this.getSubDepartment({ id: row.didMain });
    this.requisitionForm.get('subDepartment')?.patchValue(row.did);
    this.requisitionForm.get('remarks')?.patchValue(row.remarks);
    this.requisitionForm.get('items')?.patchValue(code);
    this.requisitionForm.get('quantity')?.patchValue(row.qty);
    this.getItemsCategory({ code: code });
  }

  deleteItem(row: any) {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (!confirmDelete) {
      return;
    }

    const indexToRemove = this.requisitionList.findIndex(
      (item) => item.sno === row.sno
    );
    if (indexToRemove !== -1) {
      this.requisitionList.splice(indexToRemove, 1);
    }
  }

  onClickSave() {
    if (this.requisitionList.length == 0) {
      this.tostr.warning('Incomplete Transaction...');
      return;
    }

    let vchNo = this.editMode ? this.requisitionForm.get('transNo')?.value : 0;

    // let vchNo = 0
    // if(this.editMode == true){vchNo = this.requisitionForm.get('transNo')?.value}
    // else {vchNo = 0}

    const voucher: any[] = this.requisitionList.map((data) => ({
      sno: data.sno,
      vchNo: vchNo,
      date: this.dp.transform(
        this.requisitionForm.get('date')?.value,
        'yyyy-MM-dd'
      ),
      reqFor: data.requestFor,
      mainDept: data.didMain,
      subDept: data.did,
      remarks: data.remarks,
      itemCode: data.code + data.sub,
      qty: data.qty,
    }));

    this.apiService
      .saveData('GeneralStore/SaveRequisition', voucher)
      .subscribe((result) => {
        if (result == true || result == 'true') {
          this.tostr.success('Save Successfully');
          this.onClickRefresh();
          this.getRQList();
          this.getTransNo();
        } else {
          this.tostr.error('Please Save Again');
        }
      });
  }

  editRQ(vchNo: any): void {
    this.onClickRefresh();
    this.isShow = true;
    this.editMode = true;

    const obj = {
      vchNo: vchNo,
    };

    this.apiService
      .getDataById('GeneralStore/GetEditRequisition', obj)
      .subscribe((data) => {
        this.togglePages();
        this.enableFields();
        data.forEach((item: any) => {
          this.requisitionForm
            .get('date')
            ?.patchValue(
              new Date(
                item.DATE.split('/')[2],
                item.DATE.split('/')[1] - 1,
                item.DATE.split('/')[0]
              )
            );
          this.requisitionForm.get('transNo')?.patchValue(item.VCHNO);
          let form = item;
          form.code = item.CODE;
          form.sub = item.SUB;
          form.itemName = item.ITEMNAME;
          form.qty = item.QTY;
          form.did = item.SUBDEPT;
          form.didMain = item.MAINDEPT;
          form.sno = item.SNO;
          form.requestFor = item.REQFOR;
          form.remarks = item.REMARKS;
          form.subDidName = item.SUBDIDNAME;
          form.mainDidName = item.MAINDIDNAME;
          this.requisitionList.push(form);
        });
      });
  }

  deleteRQ(VCHNO: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {
      const obj = {
        vchNo: VCHNO,
      };

      this.apiService.deleteData('GeneralStore/DelRequisition', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.tostr.success('Delete Successfully');
            this.getRQList();
            this.getTransNo();
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
    this.enableFields();
  }

  onClickRefresh() {
    this.isShow = false;
    this.resetForm();
    this.getTransNo();
    this.requisitionList = [];
    this.disableFields();
  }

  enableFields() {
    this.requisitionForm.get('requestFor').enable();
    this.requisitionForm.get('mainDepartment').enable();
    this.requisitionForm.get('subDepartment').enable();
    this.requisitionForm.get('remarks').enable();
    this.requisitionForm.get('items').enable();
    this.requisitionForm.get('quantity').enable();
  }

  disableFields() {
    this.requisitionForm.get('requestFor').disable();
    this.requisitionForm.get('mainDepartment').disable();
    this.requisitionForm.get('subDepartment').disable();
    this.requisitionForm.get('remarks').disable();
    this.requisitionForm.get('items').disable();
    this.requisitionForm.get('quantity').disable();
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

    this.requisitionForm.patchValue(formValue, { emitEvent: false });
  }
  //=========================== Main Department=========================//

  isDisabledMainDepartment: boolean = true;
  isShowMainDepartment: boolean = false;

  mainDepartMentId: number = 0;
  mainDepartMentName: any;

  refreshMainDepartment() {
    this.mainDepartMentId = 0;
    this.mainDepartMentName = '';
    this.isDisabledMainDepartment = true;
    this.isShowMainDepartment = false;
  }

  newMainDepartment() {
    this.refreshMainDepartment();
    this.isDisabledMainDepartment = false;
    this.isShowMainDepartment = true;
  }

  createUpdateMainDepartment() {
    if (this.mainDepartMentName == '') {
      this.tostr.warning('Enter Main Department Name ....!');
      return;
    }

    const obj = {
      id: this.mainDepartMentId,
      name: this.mainDepartMentName,
    };

    this.apiService
      .saveObj('GeneralStore/AddUpdateMainDepartment', obj)
      .subscribe((result) => {
        if (result == true || result == 'true') {
          this.tostr.success('Save Successfully');
          this.refreshMainDepartment();
          this.getMainDepartment();
        } else {
          this.tostr.error('Please Save Again');
        }
      });
  }

  editMainDepartment(id: any, name: any) {
    this.mainDepartMentId = id;
    this.mainDepartMentName = name;
    this.isDisabledMainDepartment = false;
    this.isShowMainDepartment = true;
  }

  deleteMainDepartment(id: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {
      const obj = {
        id: id,
      };
      this.apiService.deleteData('GeneralStore/DeleteMainDepartment', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.getMainDepartment();
            this.tostr.success('Delete Successfully');
          } else if (data == 'false' || data == false) {
            this.tostr.error('Delete Again');
          } else {
            this.tostr.warning('In Used');
          }
        },
        error: (error) => {
          this.tostr.info(error.error.text);
        },
      });
    }
  }

  //=========================== Sub Department==========================//

  isDisabledSubDepartment: boolean = true;
  isShowSubDepartment: boolean = false;

  subDepartMentId: number = 0;
  subDepartMentName: any;

  refreshSubDepartment() {
    this.subDepartMentId = 0;
    this.subDepartMentName = '';
    this.isDisabledSubDepartment = true;
    this.isShowSubDepartment = false;
  }

  newSubDepartment() {
    this.refreshSubDepartment();
    this.isDisabledSubDepartment = false;
    this.isShowSubDepartment = true;
  }

  createUpdateSubDepartment() {
    if (this.mainDepartMentId == 0) {
      this.tostr.warning('Select Main Department ....!');
      return;
    }

    if (this.subDepartMentName == '') {
      this.tostr.warning('Enter Sub Department Name ....!');
      return;
    }

    const obj = {
      id: this.subDepartMentId,
      name: this.subDepartMentName,
      mainDeptId: this.mainDepartMentId,
    };

    this.apiService
      .saveObj('GeneralStore/AddUpdateSubDepartment', obj)
      .subscribe((result) => {
        if (result == true || result == 'true') {
          this.tostr.success('Save Successfully');
          this.refreshSubDepartment();
          this.getSubDepartment({ id: this.mainDepartMentId });
        } else {
          this.tostr.error('Please Save Again');
        }
      });
  }

  editSubDepartment(id: any, name: any) {
    this.subDepartMentId = id;
    this.subDepartMentName = name;
    this.isDisabledSubDepartment = false;
    this.isShowSubDepartment = true;
  }

  deleteSubDepartment(id: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {
      const obj = {
        id: id,
        mainDeptId: this.mainDepartMentId,
      };
      this.apiService.deleteData('GeneralStore/DeleteSubDepartment', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.tostr.success('Delete Successfully');
            this.getSubDepartment({ id: this.mainDepartMentId });
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
