import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-job-update',
  templateUrl: './job-update.component.html',
  styleUrls: ['./job-update.component.css']
})
export class JobUpdateComponent {
  constructor(
    private tostr: ToastrService,
    private apiService: ApiService,
    private dp: DatePipe,
    private com: CommonService
  ) {
    const today = new Date();
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  }

  //List Page
  @ViewChild('voucherLists', { static: false }) voucherLists!: ElementRef;
  @ViewChild('detailsGrid', { static: false }) detailsGrid!: ElementRef;

  voucherList: any[] = [];
  fromDate: Date;
  toDate: Date;

  // Job List
  userId = localStorage.getItem('userId');
  userType = localStorage.getItem('userType');

  JobList: any[] = [];
  vchDate: Date = new Date();
  jobNo: any = null;
  week: any = null;
  detailsList: any = [];

  //
  avgWeightSum: number = 0;
  feedConsSum: number = 0;
  motalitySum: number = 0;
  dieselSum: number = 0;

  async ngOnInit() {
    this.JobList = await this.getJobList();
    this.onAdd();
  }

  async getJobList() {
    return await this.apiService.getDataById('Sale/GetJobNumberByUser', {userId: this.userId}).toPromise();
  }

  getVouchersList() {
    const obj = {
      fromDate: this.dp.transform(this.fromDate, 'yyyy-MM-dd'),
      toDate: this.dp.transform(this.toDate, 'yyyy-MM-dd'),
    };

    this.apiService
      .getDataById('Purchase/GetDailyConsList', obj)
      .subscribe((data) => {
        this.voucherList = data;
      });
  }

  onAdd() {
    let form = {
      transDate: new Date(),
      week: null,
      avgWeight: '',
      feedCons: '',
      motality: '',
      diesel: '',
      remarks: '',
      filter: true,
    };
    this.detailsList.push(form);
  }

  onClickRefresh() {
    this.jobNo = null;
    this.vchDate = new Date();
    this.detailsList = [];
    this.onAdd();
  }

  deleteItem(index: any) {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (!confirmDelete) {
      return;
    }

    if (index !== -1) {
      this.detailsList.splice(index, 1);
    }

    this.calculation();
  }

  onClickSave() {

    if (this.jobNo == undefined) {
      this.tostr.warning('Select Job No....!');
      return;
    }

    const voucher: any[] = this.detailsList.map((data) => ({
      transDate: this.dp.transform(data.transDate, 'yyyy-MM-dd'),
      week: data.week ?? 0,
      avgWeight: parseFloat(data.avgWeight),
      feedConsumed: parseFloat(data.feedCons),
      motality: isNaN(parseFloat(data.motality))? 0 : parseFloat(data.motality),
      diesel: isNaN(parseFloat(data.diesel))? 0 : parseFloat(data.diesel),
      remarks: data.remarks,
      jobNo: this.jobNo,
      date: this.dp.transform(this.vchDate, 'yyyy-MM-dd'),
    }));
    
    voucher.forEach((item) => {
      if (item.week == 0) {
        this.tostr.warning('Select Week....!');
        return;
      }

      if (
        isNaN(item.avgWeight) ||
        isNaN(item.feedConsumed)
      ) {
        this.tostr.warning('Invalid Transaction....!');
        return;
      }
    });

    this.apiService
      .saveData('Purchase/SaveDailyCons', voucher)
      .subscribe((result) => {
        if (result == true || result == 'true') {
          this.tostr.success('Save Successfully');
          this.detailsList.sort((a, b) => new Date(a.transDate).getTime() - new Date(b.transDate).getTime());
        } else {
          this.tostr.error('Please Save Again');
        }
      });
  }

  onChangeJobNo(): void {

    if(this.jobNo == null){
      this.vchDate = new Date();
      this.detailsList = [];
      this.onAdd();
      this.calculation();
      return;
    }

    this.detailsList = [];
    this.apiService
      .getDataById('Purchase/GetEditDailyCons', {jobNo: this.jobNo})
      .subscribe((data) => {

        if(data.length == 0){
          this.vchDate = new Date();
          this.detailsList = [];
          this.onAdd();
          return;
        }

        let vd = data[0].VchDate;
        let dp = vd.split("/");

        this.vchDate = new Date(dp[2], dp[1] - 1, dp[0]);

        data.forEach((item: any) => {
          let form: any = [];

          const transDate = new Date(
            item.TransDate.split('/')[2],
            item.TransDate.split('/')[1] - 1,
            item.TransDate.split('/')[0]
          );

          form.transDate = transDate;
          form.week = item.WeekNo;
          form.avgWeight = item.AvgWeight;
          form.feedCons = item.FeedConsumed;
          form.motality = item.Motality;
          form.diesel = item.DieselConsumed;
          form.remarks = item.Remarks;
          form.filter = true;

          this.calculation();
          this.detailsList.push(form);
          this.detailsList.sort(
            (a, b) =>
              new Date(a.transDate).getTime() -
              new Date(b.transDate).getTime()
          );

        });
      });
  }


  delete(vchNo: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {
      const obj = {
        vchNo: vchNo,
      };

      this.apiService.deleteData('Purchase/DelDailyCons', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.tostr.success('Delete Successfully');
            this.getVouchersList();
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

  weekFilter(event: any) {
    this.avgWeightSum = 0;
    this.feedConsSum = 0;
    this.motalitySum = 0;
    this.dieselSum = 0;

    if (event != 0 && event != undefined) {
      this.detailsList.forEach((item) => {
        item.filter = false;
      });

      this.detailsList.forEach((item, i) => {
        if (item.week == event) {
          item.filter = true;
          this.avgWeightSum += item.avgWeight ?? 0;
          this.feedConsSum += item.feedCons ?? 0;
          this.motalitySum += item.motality ?? 0;
          this.dieselSum += item.diesel ?? 0;
        }
      });
    } else {
      this.detailsList.forEach((item) => {
        item.filter = true;
        this.avgWeightSum += item.avgWeight ?? 0;
        this.feedConsSum += item.feedCons ?? 0;
        this.motalitySum += item.motality ?? 0;
        this.dieselSum += item.diesel ?? 0;
      });
    }
  }

  calculation() {
    setTimeout(() => {
      this.total();
    }, 50);
  }

  total() {
    const rows = this.detailsGrid.nativeElement.querySelectorAll('tr');

    this.avgWeightSum = 0;
    this.feedConsSum = 0;
    this.motalitySum = 0;
    this.dieselSum = 0;

    rows.forEach((row: HTMLTableRowElement) => {
      const avgWeightN = row.querySelector('.avgWeight') as HTMLInputElement;
      const feedConsN = row.querySelector('.feedCons') as HTMLInputElement;
      const motalityN = row.querySelector('.motality') as HTMLInputElement;
      const dieselN = row.querySelector('.diesel') as HTMLInputElement;

      // Add NaN check before parsing and summing
      if (
        avgWeightN &&
        !isNaN(parseFloat(avgWeightN.value.replace(/,/g, '')))
      ) {
        this.avgWeightSum += parseFloat(avgWeightN.value.replace(/,/g, ''));
      }
      if (feedConsN && !isNaN(parseFloat(feedConsN.value.replace(/,/g, '')))) {
        this.feedConsSum += parseFloat(feedConsN.value.replace(/,/g, ''));
      }
      if (motalityN && !isNaN(parseFloat(motalityN.value.replace(/,/g, '')))) {
        this.motalitySum += parseFloat(motalityN.value.replace(/,/g, ''));
      }
      if (dieselN && !isNaN(parseFloat(dieselN.value.replace(/,/g, '')))) {
        this.dieselSum += parseFloat(dieselN.value.replace(/,/g, ''));
      }
    });

  }
}
