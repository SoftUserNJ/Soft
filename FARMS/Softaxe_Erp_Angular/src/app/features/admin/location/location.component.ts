import { Component, ElementRef, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { CommonService } from 'src/app/services/common.service';
declare const $: any;
@Component({
  selector: 'app-location',
  templateUrl: './location.component.html',
  styleUrls: ['./location.component.css'],
})
export class LocationComponent {

  locationList: any[] = [];
  companyGroup: any[] = [];
  company: any[] = [];
  location: any[] = [];
  
  groupId: any;
  cmpId: any;
  locId: any;

  groupName = '';
  cmpName = '';
  locName = '';

  selectedCompanyGroup: any;
  selectedCompany: any;
  selectedLocation: any;

  // Location
  isDisabledLocation: boolean = true;
  isShowLocation: boolean = false;
  isLocId: boolean = false;
  locationId: any = '';
  locationName: any = '';
  city: any = '';
  address: any = '';
  contact: any = '';
  email: any = '';
  cmpNameLoc: any = '';

  @ViewChild('locationLists') locationLists!: ElementRef<HTMLInputElement>;

  constructor(
    private apiService: ApiService,
    private tostr: ToastrService,
    private com: CommonService
  ) {}

  ngOnInit(): void {
    this.getLocationList();
    this.getCompanyGroup();
  }

  async getLocationList() {
    const data  = await this.apiService.getData('Admin/GetLocation').toPromise();
      this.locationList = data;
      setTimeout(() => {
        this.searchGrid();
      }, 10);
  }

  getCompanyGroup() {
    this.apiService.getData('Admin/GetCompanyGroup').subscribe((data) => {
      this.companyGroup = data;
    });
  }

  onGroupChange(event: any) {
    if(event == undefined){
      this.company = [];
      this.location = [];
      this.selectedCompany = null;
      this.selectedLocation = null;
      this.groupName = '';
      this.cmpName = '';
      this.locName = '';
      this.searchGrid();
    }

    var obj = {
      groupId: event.GrpId,
    };

    this.groupId = event.GrpId;
    this.groupName = event.CompName;
    this.searchGrid();

    this.apiService
      .getDataById('Admin/GetCompanyById', obj)
      .subscribe((data) => {
        this.company = data;
      });
  }

  onCompanyChange(event: any) {
    if(event == undefined){
      this.location = [];
      this.selectedLocation = null;
      this.cmpName = '';
      this.locName = '';
      this.searchGrid();
    }

    var obj = {
      companyId: event.id,
    };

    this.cmpId = event.id;
    this.cmpName = event.name;
    this.searchGrid();
    
    this.apiService
      .getDataById('Admin/GetLocationById', obj)
      .subscribe((data) => {
        this.location = data;
      });
  }

  onChangeLocation(event: any) {
    if(event == undefined){
      this.locName = '';
      this.searchGrid();
    }

    this.locId = event.ID
    this.locName = event.NAME
    this.searchGrid();
  }

  newLocation() {
    this.refreshLocation();
    this.isDisabledLocation = false;
    this.isLocId = false;
    this.isShowLocation = true;
  }

  refreshLocation() {
    this.locationId = '';
    this.locationName = '';
    this.city = '';
    this.address = '';
    this.contact = '';
    this.email = '';
    this.cmpNameLoc = '';
    this.isDisabledLocation = true;
    this.isLocId = true;
    this.isShowLocation = false;
  }

  createUpdateLocation() {
    const obj = {
      companyId: this.cmpId,
      locId: this.locationId,
      name: this.locationName,
      city: this.city,
      address: this.address,
      contact: this.contact,
      email: this.email,
      cmpName: this.cmpNameLoc,
    };

    if (this.cmpId == 0) {
      this.tostr.warning('Select Company ....!');
      return;
    }

    if (this.locationId == '' || this.locationId == null) {
      this.tostr.warning('Enter Location Id ....!');
      return;
    }

    if (this.locationName == '' || this.locationName == null) {
      this.tostr.warning('Enter Location Name....!');
      return;
    }

    if (this.city == '' || this.city == null) {
      this.tostr.warning('Enter City....!');
      return;
    }

    if (this.address == '' || this.address == null) {
      this.tostr.warning('Enter Address....!');
      return;
    }
    
    if (this.contact == '' || this.contact == null) {
      this.tostr.warning('Enter Contact....!');
      return;
    }
    
    if (this.email == '' || this.email == null) {
      this.tostr.warning('Enter Email....!');
      return;
    }
    
    if (this.cmpNameLoc == '' || this.cmpNameLoc == null) {
      this.tostr.warning('Enter Company Name....!');
      return;
    }

    this.apiService
      .saveObj('Admin/AddUpdateLocation', obj)
      .subscribe(async (result) => {
        if (result == true || result == 'true') {
          await this.getLocationList();
          this.refreshLocation();
          this.onCompanyChange({ id: this.cmpId, name: this.cmpName });
          this.tostr.success('Save Successfully');
        } else {
          this.tostr.error('Please Save Again');
        }
      });
  }

  editLocation(item: any, tag: any): void {
    if(tag){
      this.selectedCompanyGroup = item.GRPID,
      this.onGroupChange({GrpId: item.GRPID, CompName: item.GROUPNAME});
      this.selectedCompany = item.CMPID,
      this.onCompanyChange({ id: item.CMPID, name: item.COMPANYNAME });
      $("#LocationsModal").modal('show');
      this.locationId = item.LOCID;
      this.locationName = item.LOCATIONNAME;
    }
    else{
      this.locationId = item.ID;
      this.locationName = item.NAME;
    }
    this.city = item.CITY;
    this.address = item.ADDRESS;
    this.contact = item.CONTACT;
    this.email = item.EMAIL;
    this.cmpNameLoc = item.CMPNAME;
    this.isLocId = true;
    this.isDisabledLocation = false;
    this.isShowLocation = true;
  }

  deleteLocation(id: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (confirmDelete == true) {
      const obj = {
        locId: id,
        companyId: this.cmpId,
      };

      this.apiService.deleteData('Admin/DeleteLocation', obj).subscribe({
        next: async (data) => {
          if (data == 'true' || data == true) {
            await this.getLocationList();
            this.refreshLocation();
            this.onCompanyChange({ id: this.cmpId, name: this.cmpName });
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

  searchGrid(): void {
    const tableElement = this.locationLists.nativeElement;
    const rows = tableElement.querySelectorAll('tr');

    rows.forEach((row: HTMLTableRowElement) => {
      let isShow = true;

      if (
        row.querySelector('.groupName')?.textContent != this.groupName &&
        this.groupName.length > 0
      ) {
        isShow = false;
      }

      if (
        row.querySelector('.cmpName')?.textContent != this.cmpName &&
        this.cmpName.length > 0
      ) {
        isShow = false;
      }

      if (
        row.querySelector('.locName')?.textContent != this.locName &&
        this.locName.length > 0
      ) {
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
