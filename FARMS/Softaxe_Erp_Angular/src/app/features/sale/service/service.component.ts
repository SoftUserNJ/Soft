import { Component, ViewChild, ElementRef } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-service',
  templateUrl: './service.component.html',
  styleUrls: ['./service.component.css']
})
export class ServiceComponent {
  @ViewChild('servicesLists', { static: false }) servicesLists!: ElementRef;

  serviceForm!: FormGroup;
  servicesList: any [] = [];
  timePeriodList: any [] = [];

  timePeriodId = 0;
  timePeriodName = '';

  isShowPage: boolean = true;
  isDisabledTP: boolean = true;
  isShowTP: boolean = false;
  timePeriod = '';

  constructor(
    private apiService: ApiService,
    private fb: FormBuilder,
    private tostr: ToastrService,
    private dp: DatePipe
  ) {
    const today = new Date();
    
  }

  ngOnInit() {
    this.getServices();
    this.formInit();
    this.getTimePeriod();
   
  }

  formInit() {
    this.serviceForm = this.fb.group({
      code: [''],
      serviceName: [''],
      timePeriodId: [undefined],
      rate: [''],
      tax: [''],
    });
  }

  togglePages() {
    this.isShowPage = !this.isShowPage;
    if (this.isShowPage) {
      this.onClickRefresh();
    }
  }

  getServices() {
    this.apiService.getData('Sale/GetServices').subscribe((result) => {
      this.servicesList = result;
    });
  }

  getTimePeriod() {
    this.apiService.getData('Sale/GetTimePeriod').subscribe((result) => {
      this.timePeriodList = result;
    });
  }

  createUpdateTimePeriod() {

    const obj = {
      id: this.timePeriodId,
      name: this.timePeriodName,
      dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
    };

    this.apiService
      .saveObj('Sale/SaveUpdateTimePeriod', obj)
      .subscribe((result) => {
        if (result == true || result == 'true') {
          this.tostr.success('Save Successfully');
          this.onClickRefresh();
          this.getTimePeriod();
        } else {
          this.tostr.error('Please Save Again');
        }
        this.refreshTP()
      });
  }

  editTTimePeriod(id: any, name: any): void {
    this.timePeriodName = name;
    this.timePeriodId = id;
    this.isDisabledTP = false;
    this.isShowTP = true;
  }

  deleteTimePeriod(id: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {
      const obj = {
        id: id,
        dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      };

      this.apiService.deleteData('Sale/DeleteTimePeriod', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.getTimePeriod();
            this.tostr.success('Delete Successfully');
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

  onClickSave() {
    ;
    let body = this.serviceForm.value;

    if (body.serviceName == null || body.serviceName == "" ) {
      this.tostr.warning('Enter Service Name....!');
      return;
    }

    if (body.timePeriodId == null) {
      this.tostr.warning('Select Time Period....!');
      return;
    }

    if (body.rate == null || body.rate == 0) {
      this.tostr.warning('Enter Rate....!');
      return;
    }

    if (body.tax == null || body.tax == 0) {
      this.tostr.warning('Enter Tax....!');
      return;
    }

    this.apiService
      .saveObj('Sale/SaveUpdateServices', body)
      .subscribe(
        (result) => {
          if (result == true || result == 'true') {
            this.tostr.success('Save Successfully');
            this.getServices();
            this.onClickRefresh();
          } else {
            this.tostr.error('Please Save Again');
          }
        },
        (error) => {
          this.tostr.error('Error');
        }
        );
       
  }

  
  editServices(item: any) {
    ;

    this.serviceForm.get('code')?.patchValue(item.CODE);
    this.serviceForm.get('serviceName')?.patchValue(item.NAME);
    this.serviceForm.get('timePeriodId')?.patchValue(item.TPID);
    this.serviceForm.get('rate')?.patchValue(item.RATE);
    this.serviceForm.get('tax')?.patchValue(item.TAX);

    this.togglePages();
    this.onClickNew();
  }

  deleteService(CODE: any) {
    ;
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {
      const obj = {
        code: CODE,
        dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      };

      this.apiService.deleteData('Sale/DeleteServices', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.getServices();
            this.tostr.success('Delete Successfully');
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
    const tableElement = this.servicesLists.nativeElement;
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

  resetForm() {
    this.serviceForm.get('serviceName')?.patchValue('');
    this.serviceForm.get('timePeriodId')?.patchValue(undefined);
    this.serviceForm.get('rate')?.patchValue('');
    this.serviceForm.get('tax')?.patchValue('');
  }

  newTimePeriod() {
    this.refreshTP();
    this.isDisabledTP = false;
    this.isShowTP = true;
  }

  refreshTP() {
    this.timePeriodName = '';
    this.isDisabledTP = true;
    this.isShowTP = false;
  }

  onClickRefresh() {
    this.isPageDisabled = true;
    this.isNewClick = false;
    this.resetForm();
  }

  onClickNew() {
    this.isPageDisabled = false;
    this.isNewClick = true;
  }

  isNewClick = false;
  isPageEnabled: boolean = false;
  isPageDisabled: boolean = true;

}