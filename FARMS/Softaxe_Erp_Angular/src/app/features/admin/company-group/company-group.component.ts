import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-company-group',
  templateUrl: './company-group.component.html',
  styleUrls: ['./company-group.component.css'],
})
export class CompanyGroupComponent {
  isDisabled: boolean = true;
  isShowPage: boolean = true;
  companyForm!: FormGroup;

  companyList: any[] = [];
  isShow = false;

  // SEARCH
  companySearch = '';

  @ViewChild('companiesList', { static: false }) companiesList!: ElementRef;

  constructor(
    private fb: FormBuilder,
    private tostr: ToastrService,
    private apiService: ApiService,
    private com: CommonService
  ) {}

  ngOnInit() {
    this.getCompanies();
    this.formInit();
  }

  formInit() {
    this.companyForm = this.fb.group({
      GrpId: 0,
      GroupName: [''],
      Address: [''],
      City: [''],
      NTN: [''],
      Contact: [''],
      Email: [''],
      IsMulti: false,
    });
  }

  togglePages() {
    this.isShowPage = !this.isShowPage;
    if (!this.isShowPage) {
      this.onClickRefresh();
    }
  }

  getCompanies() {
    this.apiService.getData('Admin/GetCompanyGroup').subscribe((data) => {
      this.companyList = data;
    });
  }

  onClickNew() {
    this.onClickRefresh();
    this.isShow = true;
    this.isDisabled = false;
  }

  onClickRefresh() {
    this.companyForm.reset();
    this.isShow = false;
    this.isDisabled = true;
  }

  onClickSave() {
    let body = this.companyForm.value;

    if (body.GrpId == null) {
      body.GrpId = 0;
    }

    if (body.GroupName == null) {
      this.tostr.warning('Enter Group Name....!');
      return;
    }

    if (body.Address == null) {
      this.tostr.warning('Enter Address....!');
      return;
    }

    if (body.City == null) {
      this.tostr.warning('Enter City....!');
      return;
    }

    if (body.NTN == null) {
      this.tostr.warning('Enter NTN....!');
      return;
    }

    if (body.Contact == null) {
      this.tostr.warning('Enter Contact....!');
      return;
    }

    if (body.Email == null) {
      this.tostr.warning('Enter Email....!');
      return;
    }

    this.apiService.saveObj('Admin/AddCompanyGroup', body).subscribe(
      (result) => {
        if (result == true || result == 'true') {
          this.tostr.success('Save Successfully');
          this.onClickRefresh();
          this.getCompanies();
        } else {
          this.tostr.error('Please Save Again');
        }
      },
      (error) => {
        this.tostr.error('On Err');
      }
    );
  }

  editCompany(data: any) {
    this.togglePages();
    this.onClickNew();

    this.companyForm.get('GrpId')?.setValue(data.GrpId);
    this.companyForm.get('GroupName')?.setValue(data.CompName);
    this.companyForm.get('Address')?.setValue(data.CompAdd);
    this.companyForm.get('City')?.setValue(data.City);
    this.companyForm.get('NTN')?.setValue(data.NTN);
    this.companyForm.get('Contact')?.setValue(data.Contact);
    this.companyForm.get('Email')?.setValue(data.Email);
    this.companyForm.get('IsMulti')?.setValue(data.IsMulti);
  }

  deleteCompany(grpId: any) {
    const confirmDelete = confirm('Are you sure you want to delete this?');
    if (confirmDelete == true) {
      this.apiService.deleteData('Admin/DeleteCompanyGroup', { id: grpId }).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.tostr.success(' Delete Successfully');
              this.getCompanies();
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

  //====================== Search ====================//

  onSearchCompany(event: any) {
    this.companySearch = event.target.value;
    this.searchGrid();
  }

  searchGrid(): void {
    const tableElement = this.companiesList.nativeElement;
    const rows = tableElement.querySelectorAll('tr');

    rows.forEach((row: HTMLTableRowElement) => {
      let isShow = true;

      if (
        row.textContent &&
        row.textContent
          .toLowerCase()
          .indexOf(this.companySearch.toLowerCase()) > -1
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

  export(fileName: any, formate: any) {
    this.com.ExportFiles(fileName, formate);
  }
}
