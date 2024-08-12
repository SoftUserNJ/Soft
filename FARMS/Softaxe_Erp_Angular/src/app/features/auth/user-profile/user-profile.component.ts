import { Component } from '@angular/core';
import { environment } from '../../../../environment/environmemt';
import { ApiService } from 'src/app/services/api.service';
import { ToastrService } from 'ngx-toastr';
import { CommonService } from 'src/app/services/common.service';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css'],
})
export class UserProfileComponent {
  basePath = environment.basePath;

  userName: any;
  email: any;
  mobile: any;
  oldPassword: any;
  newPassword: any;
  confirmPassword: any;

  isPass1: boolean = true
  isPass2: boolean = true
  passType1: any = 'password';
  passType2: any = 'password';
  eye1: any = 'fa-eye-slash';
  eye2: any = 'fa-eye-slash';

  cmpName: any = localStorage.getItem('CmpName');
  userImage: any = localStorage.getItem('userImage');

  constructor(
    private apiService: ApiService,
    private toast: ToastrService,
    private com: CommonService,
    private dp: DatePipe
  ) {}

  ngOnInit(): void {
    this.userImage = `${this.basePath}/Companies/${this.cmpName}/UserImages/${this.userImage}`;
    this.getCurrentUser();
  }

  getCurrentUser() {
    this.apiService.getData('Auth/GetCurrentUser').subscribe((data) => {
      let r = data[0];
      this.userName = r.UserName;
      this.email = r.Email;
      this.mobile = r.MOBILE;
    });
  }

  onClickSave() {
    if ((this.oldPassword ?? '') != '') {
      if ((this.newPassword ?? '') == '') {
        this.toast.warning('Enter New Password');
        return;
      }

      if ((this.confirmPassword ?? '') == '') {
        this.toast.warning('Enter Confirm Password');
        return;
      }

      if ((this.newPassword ?? '') != (this.confirmPassword ?? '')) {
        this.toast.warning('Your Confirm Password is not Matched');
        return;
      }
    }

    try {
      let obj: any = {};
      obj.userName = this.userName;
      obj.email = this.email ?? '';
      obj.mobile = this.mobile ?? '';
      obj.oldPass = this.oldPassword ?? '';
      obj.newPass = this.newPassword ?? '';
      obj.confirmPass = this.confirmPassword ?? '';
      obj.dtNow = this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss');
      this.com.showLoader();

      let formData = new FormData();
      for (const key of Object.keys(obj)) {
        formData.append(key, obj[key]);
      }
      //formData.append('Picture', this.productImage!);

      this.apiService.saveData('Auth/UpdateProfile', formData).subscribe(
        (result) => {
          if (result == true || result == 'true') {
            this.toast.success('Save Successfully');
            this.oldPassword = '';
            this.newPassword = '';
            this.confirmPassword = '';
          } else if (result == false || result == 'false') {
            this.toast.error('Please Save Again');
          } else {
            this.toast.info(result);
          }
          this.com.hideLoader();
        },
        (error) => {
          //this.toast.error('On Err');
          this.com.hideLoader();
        }
      );
    } catch {
      this.com.hideLoader();
    }
  }

  onClickEye1() {
    this.isPass1 = !this.isPass1;
    if (!this.isPass1) {
      this.passType1 = 'text';
      this.eye1 = 'fa-eye';
    } else {
      this.passType1 = 'password';
      this.eye1 = 'fa-eye-slash';
    }
  }

  onClickEye2() {
    this.isPass2 = !this.isPass2;
    if (!this.isPass2) {
      this.passType2 = 'text';
      this.eye2 = 'fa-eye';
    } else {
      this.passType2 = 'password';
      this.eye2 = 'fa-eye-slash';
    }
  }
}
