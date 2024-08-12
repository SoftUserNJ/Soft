import { Component } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';
import { DatePipe } from '@angular/common';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-flock-detail-report',
  templateUrl: './flock-detail-report.component.html',
  styleUrls: ['./flock-detail-report.component.css']
})
export class FlockDetailReportComponent {
  
  fromDate: Date;
  toDate: Date;
  userId = localStorage.getItem('userId');

  constructor(
    private tostr: ToastrService,
    private com: CommonService,
    private dp: DatePipe,
    private auth: AuthService,
    private apiService: ApiService
  ) {
    const today = new Date();
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  }

  JobList = [];
  jobNo: any = null;

  async ngOnInit(){
    this.JobList = await this.getJobList();
  }

  async getJobList() {
    return await this.apiService.getDataById('Sale/GetJobNumberByUser', {userId: this.userId}).toPromise();
  }

  onViewReport() {

    if(this.jobNo == null || this.jobNo == undefined) {
      this.tostr.warning('Select Flock....!');
      return;
    }
    const fromDate = this.dp.transform(this.fromDate, 'yyyy/MM/dd');
    const toDate = this.dp.transform(this.toDate, 'yyyy/MM/dd');

   let jobName = this.JobList.find((x) => x.ID == this.jobNo).NAME

    let url = `FlockDetail1?jobNo=${this.jobNo}&fromDate=2000/01/01&toDate=3000/01/01&locId=%&cmpId=${this.auth.cmpId()}&finId=${this.auth.finId()}&jobName=${jobName}`;
    this.com.viewReport(url);
  }

} 
