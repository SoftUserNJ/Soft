import { Component, ViewChild, ElementRef } from '@angular/core';
import { DatePipe } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { CommonService } from 'src/app/services/common.service';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-users-log-status',
  templateUrl: './users-log-status.component.html',
  styleUrls: ['./users-log-status.component.css'],
})
export class UsersLogStatusComponent {
  @ViewChild('userLogStatusLists', { static: false })
  userLogStatusLists!: ElementRef;

  userLogStatusList: any[] = [];
  logType: any[] = [];

  locationList : any[] = [];
  locId: any = null;
  isDisableLoc: any = false;
  fromDate: Date;
  toDate: Date;

  searchName = '';
  searchType = '';
  searchVchNo = '';

  constructor(
    private apiService: ApiService,
    private toast: ToastrService,
    private dp: DatePipe,
    private com: CommonService,
    private auth: AuthService
  ) {
    this.fromDate = new Date();
    this.toDate = new Date();
  }

  async ngOnInit() {
    this.getLogType();
    
    this.locationList = await this.com.getLocation();
    if (this.auth.locId() == 'HO') {
      this.isDisableLoc = false;
    } else {
      this.isDisableLoc = false;
    }
    this.locId = this.auth.locId();
    await this.getUserLogStatus();
  }

  async getUserLogStatus() {
    try {

      this.com.showLoader();

      const obj = {
        fromDate: this.dp.transform(this.fromDate, 'yyyy/MM/dd'),
        toDate: this.dp.transform(this.toDate, 'yyyy/MM/dd'),
        locId: this.locId ?? '%',
      };

      const data = await this.apiService.getDataById('Utilities/UserLogStatus', obj).toPromise();
      this.userLogStatusList = data;
      this.com.hideLoader();
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  getLogType() {
    this.apiService.getData('Utilities/GetLogType').subscribe((data) => {
      this.logType = data;
    });
  }

  deleteUserLog(id: any, userId: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (!confirmDelete) {
      return;
    }
    try {
      this.com.showLoader();
      const obj = {
        id: id,
        userId: userId
      };

      this.apiService.deleteData('Utilities/DeleteUserLog', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.getUserLogStatus();
            this.com.hideLoader();
            this.toast.success('Delete Successfully');
          } else if (data == 'false' || data == false) {
            this.com.hideLoader();
            this.toast.error('Delete Again');
          }
        },
        error: (error) => {
          this.com.hideLoader();
          this.toast.info(error.error.text);
        },
      });
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  searchInput(event: any) {
    this.searchName = event.target.value;
    this.searchGrid();
  }

  serachInv(event: any) {
    this.searchVchNo = event.target.value;
    this.searchGrid();
  }

  onChangeType(event: any) {
    this.searchType = event.TYPE;
    this.searchGrid();
  }

  onClearType() {
    this.searchType = '';
    this.searchGrid();
  }

  searchGrid(): void {
    const tableElement = this.userLogStatusLists.nativeElement;
    const rows = tableElement.querySelectorAll('tr');

    rows.forEach((row: HTMLTableRowElement) => {
      let isShow = true;

      if (
        row.querySelector('.type')?.textContent != this.searchType &&
        this.searchType.length > 0
      ) {
        isShow = false;
      }

      if (isShow) {
        if (
          row
            .querySelector('.vchno')
            ?.textContent.toLowerCase()
            .indexOf(this.searchVchNo.toLowerCase()) > -1
        ) {
          isShow = true;
        } else {
          isShow = false;
        }
      }

      if (isShow) {
        if (
          row.textContent &&
          row.textContent.toLowerCase().indexOf(this.searchName.toLowerCase()) >
            -1
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
