import { DatePipe } from '@angular/common';
import { Component, ElementRef, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-lab-test-result-entry',
  templateUrl: './lab-test-result-entry.component.html',
  styleUrls: ['./lab-test-result-entry.component.css']
})
export class LabTestResultEntryComponent {

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
  @ViewChild('ArrivalList', { static: false }) ArrivalList!: ElementRef;


  @ViewChildren('sno') snoInputs: QueryList<ElementRef>;
  @ViewChildren('testType') testTypeInputs: QueryList<ElementRef>;
  @ViewChildren('result') resultInputs: QueryList<ElementRef>;
  @ViewChildren('uom') uomInputs: QueryList<ElementRef>;
  @ViewChildren('tanker') tankerInputs: QueryList<ElementRef>;
  @ViewChildren('PartyDed') PartyDedInputs: QueryList<ElementRef>;
 @ViewChildren('PDedKgs') PDedKgsInputs: QueryList<ElementRef>;
 @ViewChildren('StkDed') StkDedInputs: QueryList<ElementRef>;
 @ViewChildren('SDedKg') SDedKgInputs: QueryList<ElementRef>;

  voucherList: any[] = [];
  fromDate: Date;
  toDate: Date;
  showLoader: boolean = false;
  TotalBags: number;
  //Form Page
  LabEntryForm!: FormGroup;
  LabEntryList: any = [];
  LabEntryListTest: any = [];
  isShow = false;
  isReadOnly: boolean = true;
  editMode: boolean = true;
  isShowPage: boolean = true;
  editSno: any = '';
  editModeSno: boolean = false;
  selectedRow: number | null = null;
  btnAdd: string = 'Add';
  isDisabled: boolean;
  UomList: any[] = [];
  TestTypeList: any[] = [];
  ArrvialList: any[] = [];
  isFreeze: boolean;

  rejected: boolean = false;
  accepted: boolean = false;

  ngOnInit() {
    this.showLoader = true;
    this.getLabList();
    this.formInit();
    this.disableFields();
    this.getUomList();
    this.getLabNo();
    this.getTestTypeList();
  }

  formInit() {
    this.LabEntryForm = this.fb.group({
      vchType: ['LB'],
      Vchdate: [new Date()],
      labNo: [''],
      BagsIn: [''],
      arrvNo: [''],
      VehNo: [''],
      remarks: [''],
      sample1: [''],
      sample2: [''],
      sample3: [''],
      UserId: ['']
    });
  }

  resetForm() {
    this.LabEntryForm.get('sample1').patchValue('');
    this.LabEntryForm.get('sample2').patchValue('');
    this.LabEntryForm.get('sample3').patchValue('');
    this.LabEntryForm.get('VehNo').patchValue('');
    this.LabEntryForm.get('arrvNo').patchValue('');
    this.LabEntryForm.get('BagsIn').patchValue('');
    this.LabEntryForm.get('remarks').patchValue('');
    this.accepted = false;
    this.rejected = false;
  }

  getUomList() {
    this.apiService.getData('Inventory/GetUOM').subscribe((data) => {
      this.UomList = data;
    })
  }

  getArrivalList() {
    this.apiService.getData('Purchase/GetArrivalList').subscribe((data) => {
      this.ArrvialList = data;
    })
  }

  getLabNo() {
    this.apiService.getData('Purchase/GetLabNo').subscribe((data) => {
      this.LabEntryForm.get('labNo').setValue(data[0].LabNo);
    })
  }

  getTestTypeList() {
    this.apiService.getData('Purchase/GetTestTypes').subscribe((data) => {
      this.TestTypeList = data;
    })
  }

  getLabList() {

    try {

      if (this.showLoader) {
        this.com.showLoader();
      }
      else {
        this.com.hideLoader();
      }
      const obj = {
        fromDate: this.dp.transform(this.fromDate, 'yyyy-MM-dd'),
        toDate: this.dp.transform(this.toDate, 'yyyy-MM-dd'),
      };

      this.apiService
        .getDataById('Purchase/GetLabsResultList', obj)
        .subscribe((data) => {
          this.com.hideLoader();
          this.showLoader = false;
          console.log(data);
          this.voucherList = data;
        });

    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  onAdd() {
    let form = this.LabEntryForm.value;

    if (form.vchType === null || form.vchType === undefined) {
      this.tostr.warning('Select Voucher Type....!');
      return;
    }

    if (this.editModeSno) {
      const index = this.LabEntryList.findIndex(
        (row) => row.sno === this.editSno
      );
      if (index !== -1) {
        form.sno = this.editSno;
        this.LabEntryList[index] = form;
        this.editModeSno = false;
        this.editSno = '';
        this.btnAdd = 'Add';
        // this.resetForm();
        return;
      }
    }

    form.sno = this.LabEntryList.length + 1;
    this.LabEntryList.push(form);
    // this.resetForm();
  }

  deleteItem(row: any) {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (!confirmDelete) {
      return;
    }

    const indexToRemove = this.LabEntryList.findIndex(
      (item) => item.sno === row.sno
    );
    if (indexToRemove !== -1) {
      this.LabEntryList.splice(indexToRemove, 1);
    }
    setTimeout(() => {
      this.CalculateBags();
    }, 1000);
    
  }

  onClickSave() {
    const form = this.LabEntryForm.value;

    if (!form.arrvNo) {
      this.tostr.warning('Select Arrvial First....!');
      return;
    }

    // if(this.TotalBags != 0) {
    //   this.tostr.warning('Total Bags should be  0....!');
    //   return;
    // }

    const emptyFields = this.LabEntryList.some((data, index) => {
      const testType = this.testTypeInputs.toArray()[index]?.nativeElement.value;
      const result = this.resultInputs.toArray()[index]?.nativeElement.value;
      const uom = this.uomInputs.toArray()[index]?.nativeElement.value;
      const bags = this.tankerInputs.toArray()[index]?.nativeElement.value;
      const partyDed = this.PartyDedInputs.toArray()[index]?.nativeElement.value;
      //const stkDed = this.StkDedInputs.toArray()[index]?.nativeElement.value;
      //const sDedKg = this.SDedKgInputs.toArray()[index]?.nativeElement.value;
  
      return !testType || !result || !uom || !bags || !partyDed;
    });
  
    if (emptyFields) {
      this.tostr.warning('Please fill in all fields before saving.');
      return;
    }
    

    let LabTestNo = this.LabEntryForm.get('labNo')?.value;

    let visuallyAccepted: boolean = false;
    let visuallyRejected: boolean = false;
    if (this.accepted) {
      visuallyAccepted = true;

    } else if (this.rejected) {
      visuallyRejected = true;

    } else {
      visuallyAccepted = false;
      visuallyRejected = false;

    }
   
    const voucher: any[] = this.LabEntryList.map((data, index) => ({
   
      Sno: index + 1,
      LabTestName: this.testTypeInputs.toArray()[index]?.nativeElement.value,
      Percentage: this.resultInputs.toArray()[index]?.nativeElement.value,
      Uom: this.uomInputs.toArray()[index]?.nativeElement.value,
      Bags: this.tankerInputs.toArray()[index]?.nativeElement.value,
      PartyDed: this.PartyDedInputs.toArray()[index]?.nativeElement.value,
      PartyDedKg: 0,
      StockDed: this.StkDedInputs.toArray()[index]?.nativeElement.value > 0 ? this.StkDedInputs.toArray()[index]?.nativeElement.value : 0,
      StockDedKg: this.SDedKgInputs.toArray()[index]?.nativeElement.value > 0 ? this.StkDedInputs.toArray()[index]?.nativeElement.value : 0,
      LabTestNo: LabTestNo,
      ResDate: form.Vchdate,
      BagsIn: form.BagsIn,
      VisAcc: visuallyAccepted,
      VisRej: visuallyRejected,
      ArrivalNo: form.arrvNo,
      VehicleNo: form.VehNo,
      Remarks: form.remarks,
      Test1: form.sample1,
      Test2: form.sample2,
      Test3: form.sample3,
      UID: form.UserId
    }));
    try {
      this.com.showLoader();

      this.apiService
        .saveData('Purchase/SaveLabResult', voucher)
        .subscribe((result) => {
          this.com.hideLoader();

          if (result == true || result == 'true') {
            this.tostr.success('Save Successfully');
            this.togglePages();
            this.onClickRefresh();
            this.getLabNo();
            this.onAdd();
            this.getLabList();
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


  async editLab(LabTestNo: any) {
    this.onClickRefresh();
    this.isShow = true;
    this.editMode = true;

    try {
      this.com.showLoader();

      const obj = {
        LabTestNo: LabTestNo,
      };

      await this.apiService.getDataById('Purchase/GetEditLab', obj)
        .subscribe((data) => {
          this.com.hideLoader();
          this.togglePages();
          this.enableFields();
          this.LabEntryList = [];
          if (data[0].VisAcc == true) {
            this.accepted = true;

          } else if (data[0].VisRej) {
            this.rejected = true;

          } else {
            this.accepted = false;
            this.rejected = false;

          }

          data.forEach((item: any) => {
            this.LabEntryForm.patchValue({
              Vchdate: item.ResDate,
              labNo: item.LabTestNo,
              arrvNo: item.ArrivalNo,
              vchType: item.VchType,
              BagsIn: item.BagsIn,
              VehNo: item.VehicleNo,
              sample1: item.Test1,
              sample2: item.Test2,
              sample3: item.Test3,
              remarks: item.Remarks,
              UserId: item.UID
            });


            this.LabEntryList.push({
              sno: item.Sno,
              testType: item.LabTestName,
              result: item.Percentage,
              uom: item.UOM,
              tanker: item.Bags,
              PartyDed: item.PartyDed,
              PDedKgs: item.PartyDedKg,
              StkDed: item.StockDed,
              SDedKg: item.StockDedKg,
            });


          });
          setTimeout(() => {
            this.setLabValues();
          }, 1000);
        });

    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }


  }


  setLabValues() {

    this.LabEntryList.forEach((item, index) => {
      //this.snoInputs.toArray()[index].nativeElement.value = item.Sno;
      this.testTypeInputs.toArray()[index].nativeElement.value = item.testType;
      this.resultInputs.toArray()[index].nativeElement.value = item.result;
      this.uomInputs.toArray()[index].nativeElement.value = item.uom;
      this.tankerInputs.toArray()[index].nativeElement.value = item.tanker;
      this.PartyDedInputs.toArray()[index].nativeElement.value = item.PartyDed;
      this.PDedKgsInputs.toArray()[index].nativeElement.value = item.PDedKgs;
      this.StkDedInputs.toArray()[index].nativeElement.value = item.StkDed;
      this.SDedKgInputs.toArray()[index].nativeElement.value = item.SDedKg;
    });

    this.bindBags();
  }



  deleteLab(LabTestNo: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {

      try {

        this.com.showLoader();

        const obj = {
          LabTestNo: LabTestNo,
        };

        this.apiService.deleteData('Purchase/DelLab', obj).subscribe({
          next: (data) => {
            this.com.hideLoader();

            if (data == 'true' || data == true) {
              this.tostr.success('Delete Successfully');
              this.getLabList();
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

  onClickNew() {
    this.isShow = true;
    this.editMode = false;
    this.getLabNo();
    this.enableFields();
  }

  onClickRefresh() {
    this.isShow = false;
    this.resetForm();
    this.LabEntryList = [];
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
    this.onAdd();
    // this.isFreeze = true;
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

  searchArrivals(event: any): void {
    const tableElement = this.ArrivalList.nativeElement;
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

    this.LabEntryForm.patchValue(formValue, { emitEvent: false });
  }

  AddArrival(row: any) {
    this.LabEntryForm.get('BagsIn').setValue(row.Bags);
    this.LabEntryForm.get('VehNo').setValue(row.Vehicleno);
    this.LabEntryForm.get('arrvNo').setValue(row.VchNo);
    $('.autoClose').click();

  }

  // Lab Test Slip

  PrintSlip(ResultDate, ArrivalNo) {
    const parts = ResultDate.split('/');
    const date = new Date(`${parts[2]}-${parts[1]}-${parts[0]}`);
    const VchDate = date.toISOString().split('T')[0];
    let form = this.LabEntryForm.value;
  
    let url = `LabTestSlip?CmpId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}&VchDate=${VchDate}&VchNo=${ArrivalNo}&VchType=RP-RAW`;
    this.com.viewReport(url);
}

// bindBags() {
//   const bags = this.LabEntryForm.get('BagsIn').value;

//   this.tankerInputs.toArray().forEach((input, index) => {
//     if (index === 0) {
//       input.nativeElement.value = bags;
//     }
//   });
//   this.CalculateBags();
// }

bindBags() {
  const bags = this.LabEntryForm.get('BagsIn').value;
  
  this.tankerInputs.toArray().forEach((input, index) => {
  
    if (index === 0 && input.nativeElement.value <= 0) {
      input.nativeElement.value = bags;
    }
  });
  this.CalculateBags();
}


CalculateBags() {
  const bagsIn = this.LabEntryForm.get('BagsIn').value;
  let bags = 0;
  this.tankerInputs.toArray().forEach((input, index) => {
      bags += parseFloat(input.nativeElement.value);
  });
  this.TotalBags = bagsIn - bags;
}


}
