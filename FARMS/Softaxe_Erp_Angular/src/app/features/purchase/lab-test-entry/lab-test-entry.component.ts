import { DatePipe } from '@angular/common';
import { Component, ElementRef, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { CommonService } from 'src/app/services/common.service';
declare const $: any;

@Component({
  selector: 'app-lab-test-entry',
  templateUrl: './lab-test-entry.component.html',
  styleUrls: ['./lab-test-entry.component.css']
})
export class LabTestEntryComponent {

  constructor(
    private fb: FormBuilder,
    private tostr: ToastrService,
    private apiService: ApiService,
    private dp: DatePipe,
    private com: CommonService,
  ) {
    const today = new Date();
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  }

  //List Page
  @ViewChild('voucherLists', { static: false }) voucherLists!: ElementRef;
  @ViewChild('ArrivalList', { static: false }) ArrivalList!: ElementRef;

  voucherList: any[] = [];
  fromDate: Date;
  toDate: Date;
  showLoader: boolean = false;

  //Form Page
  LabEntryForm!: FormGroup;
  LabEntryListTest: any = [];
  isShow = false;
  isReadOnly: boolean = true;
  editMode: boolean = true;
  isShowPage: boolean = false;
  editSno: any = '';
  editModeSno: boolean = false;
  selectedRow: number | null = null;
  btnAdd: string = 'Add';
  isDisabled: boolean;
  UomList: any[] = [];
  TestTypeList: any[] = [];
  ArrvialList: any[] = [];
  LabTestList: any[] = [];
  isFreeze: boolean;


  rejected: boolean = false;
  accepted: boolean = false;

  ngOnInit() {
    this.showLoader = true;
    this.formInit();
    this.getFirstLabList();
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
      sample3: ['']

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

  getFirstLabList() {
    this.apiService.getData('Purchase/GetFirstLabList').subscribe((data) => {

      this.LabTestList = data;
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

  // getLabList() {
  //   try {

  //     if (this.showLoader) {
  //       this.com.showLoader();
  //     }
  //     else {
  //       this.com.hideLoader();
  //     }
  //     const obj = {
  //       fromDate: this.dp.transform(this.fromDate, 'yyyy-MM-dd'),
  //       toDate: this.dp.transform(this.toDate, 'yyyy-MM-dd'),
  //     };

  //     this.apiService
  //       .getDataById('Purchase/GetLabsList', obj)
  //       .subscribe((data) => {

  //         this.com.hideLoader();
  //         this.showLoader = false;
  //         this.voucherList = data;
  //       });

  //   } catch (err) {
  //     this.com.hideLoader();
  //     console.log(err);
  //   } finally {
  //     //this.com.hideLoader();
  //   }
  // }

  onClickSave() {
    const form = this.LabEntryForm.value;

    if (!form.arrvNo) {
      this.tostr.warning('Select Arrvial First....!');
      return;
    }


    


    let LabTestNo = this.editMode ? this.LabEntryForm.get('labNo')?.value : 0;

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
   
    if (!visuallyAccepted && !visuallyRejected && !form.sample1) {
      this.tostr.warning('Accept Or Reject Vehicle or provide a Sample....!');
      return;
    }

    const voucher = {
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
    }

    try {
      this.com.showLoader();

      this.apiService
        .saveData('Purchase/SaveLabFirstSample', voucher)
        .subscribe((result) => {
          this.com.hideLoader();

          if (result == true || result == 'true') {
            this.tostr.success('Save Successfully');
            this.onClickRefresh();
            this.getLabNo();
            this.getFirstLabList()
          } else {
            this.com.hideLoader();

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


  async editLab(LabNo: any) {
    this.onClickRefresh();
    this.isShow = true;
    this.editMode = true;

    try {
      this.com.showLoader();

      const obj = {
        LabTestNo: LabNo,
      };

      await this.apiService.getDataById('Purchase/GetEditLab', obj)
        .subscribe((data) => {
          this.com.hideLoader();
          this.enableFields();
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
            });

          });
        });

    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }

  }


  deleteLab(LabNo: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {

      try {
        this.com.showLoader();
        const obj = {
          LabTestNo: LabNo,
        };

        this.apiService.deleteData('Purchase/DelLab', obj).subscribe({
          next: (data) => {
            this.com.hideLoader();

            if (data == 'true' || data == true) {
              this.tostr.success('Delete Successfully');
              this.getFirstLabList();
              this.getLabNo();
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
    this.disableFields();
    this.getLabNo();
  }

  enableFields() {
    this.isDisabled = false;
  }

  disableFields() {
    this.isDisabled = true;
  }

  togglePages() {
    this.isShowPage = !this.isShowPage;
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

  disableCheckBox(){
    const form = this.LabEntryForm.value;
    if(form.sample1 != "" || form.sample1 != null || form.sample2 != "" || form.sample2 != null || form.sample3 != "" || form.sample3 != null) {
      this.accepted = false;
      this.rejected = false;
    }


    
  }


  AddArrival(row: any) {
    this.LabEntryForm.get('BagsIn').setValue(row.Bags);
    this.LabEntryForm.get('VehNo').setValue(row.Vehicleno);
    this.LabEntryForm.get('arrvNo').setValue(row.VchNo);
    $('.autoClose').click();

  }

}
