import { Component, ViewChild, ElementRef } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { FormBuilder, FormGroup } from '@angular/forms';
import { environment } from 'src/environment/environmemt';
import { DatePipe } from '@angular/common';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css'],
})
export class UsersComponent {
  @ViewChild('usersLists', { static: false }) usersLists!: ElementRef;

  basePath = environment.basePath;
  query = '?v=' + Math.random();
  userForm!: FormGroup;
  locId: any = localStorage.getItem('locId');
  userId: any = localStorage.getItem('userId');

  isShowPage: boolean = true;
  isDisabled: boolean = true;
  isPassword: boolean = true;
  isNewClick = false;

  usersList: any[] = [];
  groupList: any[] = [];
  companyList: any[] = [];
  isMulti: boolean = true;
  locationList: any[] = [];

  productImage: File | null = null;
  selectedImage: any = '';
  file: any;

  passType1: string = 'password';
  passType2: string = 'password';
  eye1: string = 'fa-eye-slash';
  eye2: string = 'fa-eye-slash';

  userType = [
    { name: 'User', value: 'User' },
    { name: 'Admin', value: 'Admin' },
    { name: 'OT', value: 'OT' },
    { name: 'SM', value: 'SM' },
    { name: 'SV', value: 'SV' },
  ];

  permission = [
    { name: 'Self', value: 'Self' },
    { name: 'All', value: 'All' },
  ];

  dashboard = [
    { name: 'Not Allow', value: false },
    { name: 'Allow', value: true },
  ];

  constructor(
    private apiService: ApiService,
    private tostr: ToastrService,
    private fb: FormBuilder,
    private dp: DatePipe,
    private com: CommonService
  ) {}

  ngOnInit() {
    this.getUserList();
    this.getGroupList();
    this.formInit();
  }

  formInit() {
    this.userForm = this.fb.group({
      groupId: [0],
      companyId: [0],
      companyName: [''],
      locationId: [null],
      userType: [null],
      userId: [0],
      userName: [''],
      email: [''],
      password: [''],
      confirmPassword: [''],
      designation: [''],
      cnic: [''],
      mobile: [''],
      permission: ['Self'],
      dashboard: false,
      dtNow: [new Date()],
    });

    this.userForm.get('groupId').disable();
  }

  togglePages() {
    this.isShowPage = !this.isShowPage;
    if (!this.isShowPage) {
      this.onClickRefresh();
    }
  }

  async getGroupList() {
    const data = await this.apiService.getData('Accounts/GetGroup').toPromise();
    this.groupList = data;
    this.userForm.get('groupId')?.patchValue(data[0].id);
    this.isMulti = this.groupList[0].IsMulti;

    await this.getComanyList();
  }

  async getComanyList() {
    const data = await this.apiService
      .getDataById('Accounts/GetCompany', {
        groupId: this.userForm.get('groupId').value,
      })
      .toPromise();

    this.companyList = data;

    if (this.companyList.length > 1 && this.isMulti == true) {
      this.userForm.get('companyId').enable();
    }
    else{
      this.userForm.get('companyId').disable();
    }

    this.userForm.get('companyId')?.patchValue(data[0].id);

    this.getLoc();
  }

  async getLoc() {
    const result = await this.apiService
      .getDataById('Admin/GetLocationById', {
        companyId: this.userForm.get('companyId').value,
      })
      .toPromise();
    this.locationList = result;
    this.userForm.get('locationId').setValue(result[0].ID);
  }

  resetForm() {
    this.userForm.get('userType')?.patchValue(null);
    this.userForm.get('picture')?.patchValue('');
    this.userForm.get('userName')?.patchValue('');
    this.userForm.get('email')?.patchValue('');
    this.userForm.get('password')?.patchValue('');
    this.userForm.get('confirmPassword')?.patchValue('');
    this.userForm.get('designation')?.patchValue('');
    this.userForm.get('cnic')?.patchValue('');
    this.userForm.get('mobile')?.patchValue('');
    this.passType1 = 'password';
    this.passType2 = 'password';
    this.eye1 = 'fa-eye-slash';
    this.eye2 = 'fa-eye-slash';
    this.selectedImage = '';
    this.file = '';
  }

  getUserList() {
    this.apiService
      .getDataById('Auth/GetUsersList', { locId: (this.locId == 'HO') ? '%' : this.locId })
      .subscribe((data) => {
        this.usersList = data;
      });
  }

  onClickRefresh() {
    this.resetForm();
    this.isDisabled = true;
    this.isNewClick = false;
  }

  onClickNew() {
    this.onClickRefresh();
    this.isDisabled = false;
    this.isNewClick = true;
  }

  onClickSave() {
    let body = this.userForm.value;
    body.dtNow = this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss');
    body.groupId = this.userForm.get('groupId').value;
    body.companyId = this.userForm.get('companyId').value;

    const cmp = this.companyList.find((option) => option.id === body.companyId);
    if (cmp) {
      body.companyName = cmp.name;
    }

    if (body.groupId == null) {
      this.tostr.warning('Select Group....!');
      return;
    }

    if (body.companyId == null) {
      this.tostr.warning('Select Company....!');
      return;
    }

    if (body.locationId == null) {
      this.tostr.warning('Select Location....!');
      return;
    }

    if (body.userType == null) {
      this.tostr.warning('Select User Type....!');
      return;
    }

    if (body.userName == null) {
      this.tostr.warning('Enter UserName....!');
      return;
    }

    if (body.email == null) {
      this.tostr.warning('Enter Email....!');
      return;
    }

    if (body.password == null) {
      this.tostr.warning('Enter Password....!');
      return;
    }

    if (body.confirmPassword == null) {
      this.tostr.warning('Enter Confirm Password....!');
      return;
    }

    if (body.password !== body.confirmPassword) {
      this.tostr.warning('Password and confirm password does not match...!');
      return;
    }

    if (body.designation == null) {
      this.tostr.warning('Enter Designation....!');
      return;
    }

    if (body.cnic == null || body.cnic == 0) {
      this.tostr.warning('Enter CNIC....!');
      return;
    }

    if (body.mobile == null) {
      this.tostr.warning('Select Mobile....!');
      return;
    }

    if (body.permission == null) {
      this.tostr.warning('Select Permission....!');
      return;
    }

    if (body.dashboard == null) {
      this.tostr.warning('Select Dashboard....!');
      return;
    }

    try {
      this.com.showLoader();
      let formData = new FormData();
      for (const key of Object.keys(body)) {
        formData.append(key, body[key]);
      }
      formData.append('Picture', this.productImage!);

      this.apiService.saveData('Auth/SaveUpdateUser', formData).subscribe(
        (result) => {
          if (result == true || result == 'true') {
            this.getUserList();
            this.onClickRefresh();
            this.com.hideLoader();
            this.tostr.success('Save Successfully');
          } else {
            this.com.hideLoader();
            this.tostr.error('Please Save Again');
          }
        },
        (error) => {
          this.com.hideLoader();
          this.tostr.error('Error');
        }
      );
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  editUser(item: any) {
    this.togglePages();
    this.onClickNew();

    this.userForm.get('groupId')?.setValue(item.GroupId);
    this.userForm.get('companyId')?.setValue(item.CompanyId);
    this.userForm.get('locationId')?.setValue(item.LocId);
    this.userForm.get('userId')?.setValue(item.UserId);
    this.userForm.get('userType')?.setValue(item.Type);
    this.userForm.get('userName')?.setValue(item.UserName);
    this.userForm.get('email')?.setValue(item.Email);
    this.userForm.get('password')?.setValue(item.Password);
    this.userForm.get('confirmPassword')?.setValue(item.Password);
    this.userForm.get('designation')?.setValue(item.Designation);
    this.userForm.get('cnic')?.setValue(item.Cnic);
    this.userForm.get('mobile')?.setValue(item.Mobile);
    this.userForm.get('permission')?.setValue(item.Permission);
    this.userForm.get('dashboard')?.setValue(item.Dashboard);
    this.selectedImage = this.basePath + item.Image;
  }

  deleteUserList(UserId: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (!confirmDelete) {
      return;
    }

    try {
      this.com.showLoader();
      this.apiService
        .deleteData('Auth/DeleteUser', { userId: UserId })
        .subscribe({
          next: (data) => {
            if (data == 'true' || data == true) {
              this.tostr.success('Delete Successfully');
              this.getUserList();
              this.com.hideLoader();
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

  searchGrid(event: any): void {
    const tableElement = this.usersLists.nativeElement;
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

  onFileSelected(event: any) {
    this.productImage = event.target.files[0];
    if (this.productImage) {
      this.selectedImage = URL.createObjectURL(event.target.files[0]);
    }
  }

  onClickEyes(tag: any) {
    this.isPassword = !this.isPassword;
    if (!this.isPassword) {
      if (tag == 'p1') {
        this.passType1 = 'text';
        this.eye1 = 'fa-eye';
      } else if (tag == 'p2') {
        this.passType2 = 'text';
        this.eye2 = 'fa-eye';
      }
    } else {
      if (tag == 'p1') {
        this.passType1 = 'password';
        this.eye1 = 'fa-eye-slash';
      } else if (tag == 'p2') {
        this.passType2 = 'password';
        this.eye2 = 'fa-eye-slash';
      }
    }
  }

  export(fileName: any, formate: any) {
    this.com.ExportFiles(fileName, formate);
  }
}
