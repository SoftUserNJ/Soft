import { Component } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { SharedService } from 'src/app/services/shared.service';
import { ApiService } from 'src/app/services/api.service';
import { environment } from '../../../../environment/environmemt';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent {
  basePath = environment.basePath;

  notificationList: any[] = [];
  totalNotification: any;

  cmpName: any = localStorage.getItem('CmpName');
  locName: any = localStorage.getItem('locName');
  userName: any = localStorage.getItem('userName');
  designation: any = localStorage.getItem('designation');
  userImage: any = localStorage.getItem('userImage');
  cmpImage: any = localStorage.getItem('Logo');

  cmpLogo: string = '';
  userImg: string = '';

  constructor(
    public sharedService: SharedService,
    private authService: AuthService,
    private apiService: ApiService,
    private router: Router,
    private toast: ToastrService
  ) {}

  ngOnInit() {
    this.getTodayChq();
    this.cmpLogo = `${this.basePath}/Companies/${this.cmpName}/CompanyLogo/${this.cmpImage}`;
    this.userImg = `${this.basePath}/Companies/${this.cmpName}/UserImages/${this.userImage}`;
  }

  onClickLogout() {
    this.authService.logout().subscribe({
      next: (res) => {
        this.toast.success(res.msg);
        localStorage.clear()
        this.router.navigate(['/login'])
      },
      error: (err) => {
        this.toast.error(err?.error.msg);
      },
    });
  }

  getTodayChq() {
    this.apiService.getData('Dashboard/TodayChq').subscribe((data) => {
      this.notificationList = data;
      this.totalNotification = data.length;
    });
  }
}
