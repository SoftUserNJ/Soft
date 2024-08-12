import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-party-terms-and-conditions',
  templateUrl: './party-terms-and-conditions.component.html',
  styleUrls: ['./party-terms-and-conditions.component.css']
})
export class PartyTermsAndConditionsComponent {

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

  @ViewChild('voucherLists', { static: false }) voucherLists!: ElementRef;

  fromDate: any;
  toDate: any;
  showLoader:boolean = false;
  TermsConditionForm: FormGroup;
  partySubList: any = [];
  partyMainList: any = [];
  TermsConditionList: any = [];
  voucherList: any = [];
  LabTestNo: number = 0;
  LabTestName: any;
  TestUOM: any;
  isShowPage: boolean = true;
  btnAdd: string = 'Add';
  isShowBtn: boolean = false;
  isSaveDone: boolean = false;
  isShow = false;
  readonly: boolean = false;

  btnSave: string = 'Save';

  editMode: boolean = true;
  editSno: any = '';
  editModeSno: boolean = false;


  ngOnInit() {
    this.formInit();
    this.showLoader = true;
    this.getAllTermsConditions()
    this.getPartyMain();
    this.refreshTermsConditions();
  }

  formInit() {
    this.TermsConditionForm = this.fb.group({
      partyMain: [undefined],
      partySub: [undefined],
      remarks: [''],
      SalesDi1: [''],
      SalesDi2: [''],
      SalesDi3: [''],
      SalesDi4: [''],
      SalesDi5: [''],
      SalesDiDD1: [],
      //SalesDiDD2: [],
      UnId1: [''],
      UnId2: [''],
      UnId3: [''],
      UnId4: [''],
      UnId5: [''],
      UnId1DD: [],
      //UnId2DD: [],
      TvDa1: [''],
      TvDa2: [''],
      TvDa3: [''],
      TvDa4: [''],
      TvDa5: [''],
      TvDa1DD:[],
      //TvDa2DD: [],
      freight: [''],
      freightDD: [undefined],
      condition: [''],
      bonus: [''],
      monthlyDisc: [''],
      pBagAll: [''],
      allowedWanda: [''],
      ID: [''],
      ForPrint: [''],
      RateChoice: [undefined],
      RateDiff: ['']
    });
  }
  refreshTermsConditions() {
    this.resetForm();
    this.readonly = true;
    this.isShowBtn = false;
    this.TermsConditionList = [];
    this.isSaveDone = false;
    this.btnAdd = 'Add';
  }


  resetForm(){
    this.TermsConditionForm.get('partyMain')?.patchValue(undefined);
    this.TermsConditionForm.get('partySub')?.patchValue(undefined);
    this.TermsConditionForm.get('remarks')?.patchValue('');
    this.TermsConditionForm.get('SalesDi1')?.patchValue('');
    this.TermsConditionForm.get('SalesDi2')?.patchValue('');
    this.TermsConditionForm.get('SalesDi3')?.patchValue('');
    this.TermsConditionForm.get('SalesDi4')?.patchValue('');
    this.TermsConditionForm.get('SalesDi5')?.patchValue('');
    this.TermsConditionForm.get('SalesDiDD1')?.patchValue('');
    this.TermsConditionForm.get('RateChoice')?.patchValue(undefined);
    this.TermsConditionForm.get('RateDiff')?.patchValue('');
    this.TermsConditionForm.get('UnId1')?.patchValue('');
    this.TermsConditionForm.get('UnId2')?.patchValue('');
    this.TermsConditionForm.get('UnId3')?.patchValue('');
    this.TermsConditionForm.get('UnId4')?.patchValue('');
    this.TermsConditionForm.get('UnId5')?.patchValue('');
    this.TermsConditionForm.get('UnId1DD')?.patchValue('');
    //this.TermsConditionForm.get('UnId2DD')?.patchValue('');
    this.TermsConditionForm.get('TvDa1')?.patchValue('');
    this.TermsConditionForm.get('TvDa2')?.patchValue('');
    this.TermsConditionForm.get('TvDa3')?.patchValue('');
    this.TermsConditionForm.get('TvDa4')?.patchValue('');
    this.TermsConditionForm.get('TvDa5')?.patchValue('');
    this.TermsConditionForm.get('TvDa1DD')?.patchValue('');
   // this.TermsConditionForm.get('TvDa2DD')?.patchValue('');
    this.TermsConditionForm.get('freight')?.patchValue('');
    this.TermsConditionForm.get('freightDD')?.patchValue(undefined);
    this.TermsConditionForm.get('condition')?.patchValue('');
    this.TermsConditionForm.get('bonus')?.patchValue('');
    this.TermsConditionForm.get('monthlyDisc')?.patchValue('');
    this.TermsConditionForm.get('pBagAll')?.patchValue('');
    this.TermsConditionForm.get('allowedWanda')?.patchValue('');
    this.TermsConditionForm.get('Id')?.patchValue('');
  }

  onClickNew() {
    //this.refreshTermsConditions();
    this.readonly = false;
    this.isShowBtn = true;
    this.btnSave = 'Save';

  }

  togglePages() {
    this.isShowPage = !this.isShowPage;
    if (this.isShowPage) {
      this.refreshTermsConditions();
    }
  }

  onPartyClear(){
    this.partySubList = [];
    this.TermsConditionForm.get('partySub')?.patchValue(undefined);
  }

  getAllTermsConditions() {
    this.apiService.getData('Approval/GetPartyTermsList').subscribe((data) => {
      this.voucherList = data;
    });
  }
  async getPartySub(event:any) {

    const result = await this.apiService
      .getDataById('Common/GetLevel5CodeNameByL4Code', {code: event.CODE})
      .toPromise();
      this.partySubList = result;
  }

  async getPartyMain() {

    const result = await this.apiService
      .getDataById('Common/GetLevel4CodeNameByTag', {tag: 'D'})
      .toPromise();
    this.partyMainList = result;
  }

  
  onAdd() {
    let form = this.TermsConditionForm.value;
    if (form.partySub == undefined) {
      this.tostr.warning('Select Main Party ....!');
      return;
    }

    if (form.partyMain == undefined) {
      this.tostr.warning('Select Sub Party ....!');
      return;
    }
    form.partyMainCode = form.partyMain;
    form.partySubCode = form.partySub;
    let MainPartyName = this.partyMainList.find((i) => i.CODE === form.partyMain);
    let SubPartyName = this.partySubList.find((i) => i.CODE === form.partySub);

    form.partyMain = MainPartyName.NAME;
    form.partySub = SubPartyName.NAME;

    if (this.editModeSno) {
      const index = this.TermsConditionList.findIndex(
        (row) => row.sno === this.editSno
      );
      if (index !== -1) {
        form.sno = this.editSno;
        this.TermsConditionList[index] = form;
        this.editModeSno = false;
        this.editSno = '';
        this.btnAdd = 'Add';
        this.resetForm();
        return;
      }
    }

    form.sno = this.TermsConditionList.length + 1;
    this.TermsConditionList.push(form);
    this.resetForm();
  }

  editTermsConditions(Id: any): void {
    this.refreshTermsConditions();
    this.isShow = true;
    this.editMode = true;
    this.com.showLoader();
    const obj = {
      vchNo: Id,
    };

    this.apiService
      .getDataById('Sale/GetEditTermsConditions', obj)
      .subscribe((data) => {
        this.togglePages();
        this.onClickNew();
        this.TermsConditionForm.get('ID')?.patchValue(data[0].Id);
        data.forEach((item: any) => {
          let form = item;
          form.bonus = item.Bonus;
          form.monthlyDisc=item.MonthDisc;
          form.condition= item.Con;
          form.partyMainCode= item.DmCode;
          form.partyMain= item.MainParty;
          form.partySub= item.SubParty;
          form.partySubCode= item.DmCode+item.Code;
          form.remarks= item.Remarks;
          form.SalesDiDD1= item.Formula1;
          form.UnId1DD= item.Formula2;
          form.TvDa1DD= item.Formula3;
          //form.SalesDiDD2= item.Formula4;
          //form.UnId2DD= item.Formula5;
          //form.TvDa2DD=item.Formula6;
          form.SalesDi1= item.Disc1;
          form.UnId1=item.Disc2;
          form.TvDa1= item.Disc3;
          form.SalesDi3= item.Disc4;
          form.UnId3= item.Disc5;
          form.TvDa3= item.Disc6;
           form.SalesDi2= item.Disc11;
           form.UnId2= item.Disc22;
           form.SalesDi4= item.Disc1111;
           form.UnId4= item.Disc2222;
           form.TvDa4= item.Disc4444;
           form.TvDa2= item.Disc444;
           form.pBagAll= item.PBWandaonly;
           form.allowedWanda= item.AllowWanda;
          // this.getPartySub({CODE: item.DmCode});
          // form.partySub = item.DmCode+item.Code;
          this.TermsConditionList.push(form);
          this.com.hideLoader();
        });
      });
  }


  onClickSave() {
    if (this.TermsConditionList.length == 0) {
      this.tostr.warning('Incomplete Transaction...');
      return;
    }

    try {
      this.com.showLoader();

      const voucher: any[] = this.TermsConditionList.map((data) => ({
        Id: this.TermsConditionForm.get('ID').value || 0,
        DmCode: data.partyMainCode,
        Code: data.partySubCode,
        MonthDisc: data.monthlyDisc || 0,
        Bonus: data.bonus || 0,
        Pbwandaonly: data.pBagAll || false,
        AllowWanda: data.allowedWanda || false,
        Remarks: data.remarks,
        Con: data.condition || 0,
        Disc1: data.SalesDi1 || 0,
        Disc2: data.UnId1 || 0,
        Disc3: data.TvDa1 || 0,
        Disc4: data.SalesDi3 || 0,
        Disc5: data.UnId3 || 0,
        Disc6: data.TvDa3 || 0,
        Disc11: data.SalesDi2 || 0,
        Disc22: data.UnId2 || 0,
        Disc444: data.TvDa2 || 0,
        Disc1111: data.SalesDi4 || 0,
        Disc2222: data.UnId4 || 0,
        Disc4444: data.TvDa4 || 0,
        Formula1: data.SalesDiDD1 || 0,
        Formula2: data.UnId1DD || 0,
        Formula3: data.TvDa1DD|| 0,
        RateChoice: data.RateChoice,
        RateDiff: data.RateDiff
        //Formula4: data.SalesDiDD2,
        //Formula5: data.UnId2DD,
        //Formula6: data.TvDa2DD,
      }));
console.log(voucher);
      this.apiService
        .saveObj('Sale/SaveTermsConditions', voucher)
        .subscribe((result) => {  
          this.com.hideLoader();

          if (result == true || result == 'true') {
            this.tostr.success('Save Successfully');
            //this.refreshTermsConditions();
            this.isSaveDone = true;
            this.getAllTermsConditions();
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

  editItem(row: any) {
    this.btnAdd = 'Update';

    this.editModeSno = true;
    this.editSno = row.sno;
    this.TermsConditionForm.get('partyMain')?.patchValue(row.partyMainCode);
    this.TermsConditionForm.get('partySub')?.patchValue(row.partySubCode);
    this.TermsConditionForm.get('SalesDi1')?.patchValue(row.SalesDi1);
    this.TermsConditionForm.get('SalesDi2')?.patchValue(row.SalesDi2);
    this.TermsConditionForm.get('SalesDi3')?.patchValue(row.SalesDi3);
    this.TermsConditionForm.get('SalesDi4')?.patchValue(row.SalesDi4);
    this.TermsConditionForm.get('SalesDi5')?.patchValue(row.SalesDi5);
    this.TermsConditionForm.get('SalesDiDD1')?.patchValue(row.SalesDiDD1);
   // this.TermsConditionForm.get('SalesDiDD2')?.patchValue(row.SalesDiDD2);
    this.TermsConditionForm.get('UnId1')?.patchValue(row.UnId1);
    this.TermsConditionForm.get('UnId2')?.patchValue(row.UnId2);
    this.TermsConditionForm.get('UnId3')?.patchValue(row.UnId3);
    this.TermsConditionForm.get('UnId4')?.patchValue(row.UnId4);
    this.TermsConditionForm.get('UnId5')?.patchValue(row.UnId5);
    this.TermsConditionForm.get('UnId5')?.patchValue(row.UnId5);
    this.TermsConditionForm.get('UnId1DD')?.patchValue(row.UnId1DD);
    //this.TermsConditionForm.get('UnId2DD')?.patchValue(row.UnId2DD);
    this.TermsConditionForm.get('TvDa1')?.patchValue(row.TvDa1);
    this.TermsConditionForm.get('TvDa2')?.patchValue(row.TvDa2);
    this.TermsConditionForm.get('TvDa3')?.patchValue(row.TvDa3);
    this.TermsConditionForm.get('TvDa4')?.patchValue(row.TvDa4);
    this.TermsConditionForm.get('TvDa5')?.patchValue(row.TvDa5);
    this.TermsConditionForm.get('TvDa1DD')?.patchValue(row.TvDa1DD);
    //this.TermsConditionForm.get('TvDa2DD')?.patchValue(row.TvDa2DD);
    this.TermsConditionForm.get('freight')?.patchValue(row.freight);
    this.TermsConditionForm.get('freightDD')?.patchValue(row.freightDD);
    this.TermsConditionForm.get('condition')?.patchValue(row.condition);
    this.TermsConditionForm.get('bonus')?.patchValue(row.bonus);
    this.TermsConditionForm.get('monthlyDisc')?.patchValue(row.monthlyDisc);
    this.TermsConditionForm.get('pBagAll')?.patchValue(row.pBagAll);
    this.TermsConditionForm.get('allowedWanda')?.patchValue(row.allowedWanda);
    this.TermsConditionForm.get('remarks')?.patchValue(row.remarks);
    this.getPartySub({CODE: row.partyMainCode});
    this.TermsConditionForm.get('partySub')?.patchValue(row.partySubCode);
    this.TermsConditionForm.get('RateDiff')?.patchValue(row.RateDiff);
    this.TermsConditionForm.get('RateChoice')?.patchValue(row.RateChoice);


  }

  deleteItem(row: any) {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (!confirmDelete) {
      return;
    }

    const indexToRemove = this.TermsConditionList.findIndex(
      (item) => item.sno === row.sno
    );
    if (indexToRemove !== -1) {
      this.TermsConditionList.splice(indexToRemove, 1);
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

  deleteTermsConditions(Id: any): void {

    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {
      this.com.showLoader();
      const obj = {
        vchNo: Id,
      };

      this.apiService.deleteData('Sale/DelTermsConditions', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.tostr.success('Delete Successfully');
            this.getAllTermsConditions();
            this.com.hideLoader();
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
    }
  }

  searchGrid(event: any): void {
    const tableElement = this.voucherList.nativeElement;
    const rows = tableElement.querySelectorAll('tr');

    if (typeof event === 'undefined') {
      rows.forEach((row: HTMLTableRowElement) => {
        row.style.display = '';
      });
      return;
    }

    rows.forEach((row: HTMLTableRowElement) => {
      let isShow = true;

      if (
        row.querySelector('.partyCode')?.textContent != event.code &&
        event.code.length > 0
      ) {
        isShow = false;
      }

      if (isShow) {
        row.style.display = '';
      } else {
        row.style.display = 'none';
      }
    });
  }

  PrintTermsConditions(Code?: any) { 
    let codeValue: string;

    if (Code) {
        codeValue = Code;
    } else {
        codeValue = this.TermsConditionList.map((data) => data.partySubCode)[0];
    }

    let url = `RptSaleContract?Code=${codeValue}&Cmp_Id=${this.auth.cmpId()}`;
    this.com.viewReport(url);
}
}
