import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-sale-men-jobs',
  templateUrl: './sale-men-jobs.component.html',
  styleUrls: ['./sale-men-jobs.component.css'],
})
export class SaleMenJobsComponent {
  isDisabled: boolean = true;
  isShowPage: boolean = true;
  saleMenJobForm!: FormGroup;

  // Jobs
  jobList: any[] = [];
  isShow = false;

  // SEARCH
  jobSearch = '';

  // Shift
  shiftList: any[] = [];

  // Till
  tillList: any[] = [];

  // SaleMan
  saleManList: any[] = [];

  // Cash Received From
  cashRecList: any[] = [];

  @ViewChild('jobsList', { static: false }) jobsList!: ElementRef;

  constructor(
    private fb: FormBuilder,
    private tostr: ToastrService,
    private apiService: ApiService,
    private dp: DatePipe
  ) {}

  ngOnInit() {
    this.getSalesMenJobs();
    this.getFieldsData();
    this.getCashRecList();
    this.formInit();
  }

  formInit() {
    this.saleMenJobForm = this.fb.group({
      id: 0,
      date: [new Date()],
      shiftId: [''],
      tillId: [''],
      salesManId: [''],
      cashReceivedFrom: [''],
      cash: [''],
      dayWise: false,
    });
  }

  togglePages() {
    this.isShowPage = !this.isShowPage;
    if (!this.isShowPage) {
      this.onClickRefresh();
    }
  }

  getSalesMenJobs() {
    this.apiService.getData('Sale/GetSalesMenJobs').subscribe((data) => {
      this.jobList = data;
    });
  }

  onClickNew() {
    this.onClickRefresh();
    this.isShow = true;
    this.isDisabled = false;
    this.saleMenJobForm.get('date')?.patchValue(new Date());
  }

  onClickRefresh() {
    this.saleMenJobForm.reset();
    this.isShow = false;
    this.isDisabled = true;
  }

  onClickSave() {
    let body = this.saleMenJobForm.value;

    if (body.id == null) {body.id = 0;}

    if (body.date == null) {
      this.tostr.warning('Select Date....!');
      return;
    }

    if (body.shiftId == null) {
      this.tostr.warning('Select Shift....!');
      return;
    }

    if (body.tillId == null) {
      this.tostr.warning('Select Till....!');
      return;
    }

    if (body.salesManId == null) {
      this.tostr.warning('Select Sale Man....!');
      return;
    }

    if (body.cashReceivedFrom == null) {
      this.tostr.warning('Select Cash Received From....!');
      return;
    }

    if (body.cash == null) {
      this.tostr.warning('Enter Cash....!');
      return;
    }

    body.date = this.dp.transform(body.date, 'yyyy/MM/dd')

    this.apiService.saveObj('Sale/SaveSalesMenJobs', body).subscribe(
      (result) => {
        if (result == true || result == 'true') {
          this.tostr.success('Save Successfully');
          this.onClickRefresh();
          this.getSalesMenJobs();
        } else {
          this.tostr.error('Please Save Again');
        }
      },
      (error) => {
        this.tostr.error('On Err');
      }
    );
  }

  editSalesMenJobs(data: any) {
    this.togglePages();
    this.onClickNew();

    this.saleMenJobForm.get('id')?.setValue(data.JOBNO);
    this.saleMenJobForm.get('date')?.setValue(data.DATETIME);
    this.saleMenJobForm.get('shiftId')?.setValue(data.SHIFTID);
    this.saleMenJobForm.get('tillId')?.setValue(data.TILLID);
    this.saleMenJobForm.get('salesManId')?.setValue(data.SALEMANID);
    this.saleMenJobForm.get('cashReceivedFrom')?.setValue(data.L5CODE);
    this.saleMenJobForm.get('cash')?.setValue(data.CASH);
    this.saleMenJobForm.get('dayWise')?.setValue(data.DAYWISE);
  }

  deleteSalesMenJobs(code: any) {
    const confirmDelete = confirm('Are you sure you want to delete this item?');
    if (confirmDelete == true) {
      // const obj = {
      //   id: code,
      //   // dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      // };

      this.apiService.deleteData('Sale/DeleteSalesMenJobs', {id: code}).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.getSalesMenJobs();
            this.tostr.success(' Delete Successfully');
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

   //======================= Fields Data =======================//

   getFieldsData() {
    this.apiService.getData('Sale/GetSalesMenJobsFields').subscribe((data) => {
      this.shiftList = data.shift;
      this.tillList = data.till;
      this.saleManList = data.sm;
    });
  }

  //======================= Cash Rec =======================//

  getCashRecList() {
    this.apiService.getData('Accounts/GetAccountsList').subscribe((data) => {
      this.cashRecList = data;
    });
  }

  //====================== Search ====================//

  onSearchJobs(event: any) {
    this.jobSearch = event.target.value;
    this.searchGrid();
  }

  searchGrid(): void {
    const tableElement = this.jobsList.nativeElement;
    const rows = tableElement.querySelectorAll('tr');

    rows.forEach((row: HTMLTableRowElement) => {
      let isShow = true;

      if (
        row.textContent &&
        row.textContent.toLowerCase().indexOf(this.jobSearch.toLowerCase()) > -1
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
