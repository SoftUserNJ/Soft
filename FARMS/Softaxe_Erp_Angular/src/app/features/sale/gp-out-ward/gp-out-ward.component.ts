import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { DatePipe } from '@angular/common';
import { CommonService } from 'src/app/services/common.service';
import { CloseScrollStrategy } from '@angular/cdk/overlay';
import { CdkVirtualScrollableWindow } from '@angular/cdk/scrolling';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-gp-out-ward',
  templateUrl: './gp-out-ward.component.html',
  styleUrls: ['./gp-out-ward.component.css']
})

export class GPOutWardComponent {
  isDisabled: boolean = true;
  isShowPage: boolean = true;
  GatePassOutwardForm!: FormGroup;

  // GPInward
  gpList: any[] = [];
  level4: any[] = [];
  L4Code: any = null;
  isShow = false;

  Freight: number;
  editMode: boolean = false;

  // SEARCH
  customerSearch = '';
  all: boolean = true;
  active: boolean = false;
  inActive: boolean = false;


  @ViewChild('customerLists', { static: false }) customerLists!: ElementRef;

  constructor(
    private fb: FormBuilder,
    private tostr: ToastrService,
    private apiService: ApiService,
    private dp: DatePipe,
    private com: CommonService
  ) {

  }

  PendingDOList: any[];
  GpDetailList: any [];
  OrderDetailList: any [];

  ngOnInit() {
    this.getGpList();
    this.formInit();
     
  }

  formInit() {
    this.GatePassOutwardForm = this.fb.group({
      GPNO: [''],
      gpDate: [new Date()],
      vehicleNo: [''],
      DriverName: [''],
      Phone: [''],
      BiltyNo: [''],
    });

    //this.GatePassOutwardForm.get('GPNO')?.disable();
  }

  togglePages() {
    this.isShowPage = !this.isShowPage;
    if (!this.isShowPage) {
      this.onClickRefresh();
    }
  }

  async getGpList() {
    const data = await this.apiService.getData('Sale/GetGatePassList').toPromise();
    this.gpList = data;
  }

  async onClickNew() {
    this.onClickRefresh();
    this.isShow = true;
    this.isDisabled = false;
    this.getPendingOrders();
    if(this.editMode == false){
      this.getMaxGpNo();
    }
    
  }

  async getMaxGpNo() { debugger
    const data = await this.apiService.getData('Sale/GetMaxGpNo').toPromise();
    this.GatePassOutwardForm.get('GPNO')?.patchValue(data[0].VCHNO);
  }

  getPendingOrders(){
    this.apiService.getData('Sale/GetPendingOrders').subscribe((result) => {
      this.PendingDOList = result;
    });
  }

  onClickPendingDo(i: any, index: any){
    if (index !== -1) {
      this.PendingDOList.splice(index, 1);
    }
    this.GpDetailList.push(i)
  }

  onClickDoDetail(i){ debugger
    this.apiService.getDataById('Sale/GetIOrderDetail', {doNo: i.DONO}).subscribe((result) => {
      this.OrderDetailList  = result;
    });
  }

  onInputDelQty(index: number) {
    const data = this.OrderDetailList[index];
    if (data.DELQTY <= data.QTY) {
      data.QTYBALANCE = data.QTY - data.DELQTY;
    } else {
      data.QTYBALANCE =  data.QTY;
    }
  }
  

  onClickRefresh() {
    this.GatePassOutwardForm.reset();
    this.GatePassOutwardForm.get('gpDate')?.setValue(new Date());
    this.PendingDOList = [];
    this.OrderDetailList = [];
    this.GpDetailList = [];
    this.isShow = false;
    this.isDisabled = true;
    this.Freight = 0;
  }

  onClickSave() {
    if (this.OrderDetailList.length === 0) {
      this.tostr.info("Select GP Detail First");
      return; 
    }
    let body = this.GatePassOutwardForm.value;
    


    if (body.vehicleNo == null) {
      this.tostr.warning('Enter Vehicle No..!');
      return;
    }

    if (body.DriverName == null) {
      this.tostr.warning('Enter Driver Name....!');
      return;
    }

    if (body.Phone == null) {
      this.tostr.warning('Enter Phone Number...!');
      return;
    }

    if (body.BiltyNo == null) {
      this.tostr.warning('Enter Bilty Number....!');
      return;
    }
    const sumDelQty = this.OrderDetailList.reduce((total, data) => total + data.DELQTY, 0);


    const voucher: any[] = this.OrderDetailList.map((data) => ({
      GpNo: data.DELQTY === 0 ? 0 : body.GPNO,
      GpDate: body.gpDate,
      VehicleNo: body.vehicleNo,
      DriverName: body.DriverName,
      Phone: body.Phone,
      BiltyNo: body.BiltyNo,
      DONO: data.DONO,
      DELQTY: data.DELQTY,
      Freight: this.Freight,
      ProductCode: data.ProductCode,
      sumDelQty: sumDelQty
    }));

    try {
      this.com.showLoader();

      this.apiService.saveData('Sale/UpdateDO', voucher).subscribe(
        (result) => {
          if (result == true || result == 'true') {
            this.tostr.success('Save Successfully');
            this.com.hideLoader();
            this.OrderDetailList = [];
            this.getGpList();
          } else {
            this.com.hideLoader();
            this.tostr.error('Please Save Again');
          }
        },
        (error) => {
          this.com.hideLoader();
          this.tostr.error('On Err');
        }
      );
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }


  deleteCustomer(code: any) {
    const confirmDelete = confirm('Are you sure you want to delete this item?');
    if (!confirmDelete) {
      return;
    }
    try {
      this.com.showLoader();
      const obj = {
        code: code,
        dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      };

      this.apiService.deleteData('Sale/DeleteCustomer', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.com.hideLoader();
            this.tostr.success(' Delete Successfully');
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


  editGatePassOutward(GPNO:any){
    this.onClickRefresh();
    this.isShow = true;
    this.editMode = true;
    this.com.showLoader();
    const obj = {
      GPNO: GPNO,
    };

    this.apiService
      .getDataById('Sale/GetEditGatePassOutward', obj)
      .subscribe((data) => {
        this.togglePages();
        this.onClickNew();
        this.GatePassOutwardForm.get('GPNO').setValue(data[0].GPNO);
        this.GatePassOutwardForm.get('gpDate').setValue(data[0].GPDate);
        this.GatePassOutwardForm.get('vehicleNo').setValue(data[0].VehicleNo);
        this.GatePassOutwardForm.get('DriverName').setValue(data[0].DriverName);
        this.GatePassOutwardForm.get('Phone').setValue(data[0].DriverContact);
        this.GatePassOutwardForm.get('BiltyNo').setValue(data[0].BilltyNo);

        data.forEach((item: any) => {
          let form = item;
          form.DONO = item.DONO;
          form.DODATE=item.DODATE;
          form.PARTY= item.PARTY;
          form.QTY= item.QTY;
          form.DELQTY= item.DELQTY;
          this.GpDetailList.push(form);
          this.com.hideLoader();
        });

        this.Freight = data[0].Freight
      });
  }



  ClearGpNo(DONO:any) {
    const obj = {
      DONO: DONO,
    };
    try {
      this.com.showLoader();

    this.apiService.saveData('Sale/ClearGPNO', obj).subscribe(
      (result) => {
        if (result == true || result == 'true') {
          this.tostr.success('Save Successfully');
          this.com.hideLoader();
          this.GpDetailList = this.GpDetailList.filter(item => item.DONO !== DONO);
          this.OrderDetailList = [];
          this.getPendingOrders();
          this.getGpList(); 
        } else {
          this.com.hideLoader();
          this.tostr.error('Please Save Again');
        }
      },
      (error) => {
        this.com.hideLoader();
        this.tostr.error('On Err');
      }
    );
  } catch (err) {
    this.com.hideLoader();
    console.log(err);
  } finally {
    //this.com.hideLoader();
  }


  }


  DeleteGpNo(GPNO:any) {

    const confirmDelete = confirm('Are you sure you want to delete this item?');
    if (!confirmDelete) {
      return;
    }


    const obj = {
      GPNO: GPNO,
    };
    try {
      this.com.showLoader();

    this.apiService.saveData('Sale/DeleteGPNO', obj).subscribe(
      (result) => {
        if (result == true || result == 'true') {
          this.tostr.success('Save Successfully');
          this.com.hideLoader();
          this.getGpList();
        } else {
          this.com.hideLoader();
          this.tostr.error('Please Save Again');
        }
      },
      (error) => {
        this.com.hideLoader();
        this.tostr.error('On Err');
      }
    );
  } catch (err) {
    this.com.hideLoader();
    console.log(err);
  } finally {
    //this.com.hideLoader();
  }


  }

  //====================== FILTER ====================//


  searchGrid() {
    const tableElement = this.customerLists.nativeElement;
    const rows = tableElement.querySelectorAll('tr');

    rows.forEach((row: HTMLTableRowElement) => {
      let isShow = true;

      if(this.L4Code != null){
        if (
          row.querySelector('.code')?.textContent.substring(0, 9) != this.L4Code &&
          this.L4Code.length > 0
          ) {isShow = false;}
      }


      if (this.active) {
        if (row.querySelector('.active')?.textContent == 'true') {
          isShow = true;
        } else {
          isShow = false;
        }
      }

      if (this.inActive) {
        if (row.querySelector('.active')?.textContent == 'false') {
          isShow = true;
        } else {
          isShow = false;
        }
      }

      if (isShow) {
        if (
          row.textContent &&
          row.textContent
            .toLowerCase()
            .indexOf(this.customerSearch.toLowerCase()) > -1
        ) {
          isShow = true;
        } else {
          isShow = false;
        }
      }

      if (isShow) {
        row.style.display = '';
      } else {
        row.style.display = 'none';
      }
    });
  }

  
  export(fileName: any, formate: any) {
    this.com.ExportFiles(fileName, formate);
  }
}