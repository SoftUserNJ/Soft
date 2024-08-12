import { Component } from '@angular/core';


@Component({
  selector: 'app-avail-services',
  templateUrl: './avail-services.component.html',
  styleUrls: ['./avail-services.component.css']
})
export class AvailServicesComponent {
  showPage1: boolean = true;

  
  customAutocompletePanelClass = 'custom-autocomplete-panel';

  groupOptions = [
    { id: 1, name: 'PREMIER FOODS' },
    { id: 2, name: 'TRUE LEAF' },
    { id: 3, name: 'abc' },
    { id: 4, name: 'sys' },
    { id: 5, name: 'softax' },
    { id: 6, name: 'pu' },
    
  ];
  companyOptions = [
    { id: 1, name: 'User' },
    { id: 2, name: 'OT' },
    { id: 3, name: 'Admin' },
    { id: 4, name: 'SM' },
    
  ];
  locationOptions = [
    { id: 1, name: 'Location 1' },
    { id: 2, name: 'Location 2' },
    { id: 3, name: 'Location 4' },
    { id: 4, name: 'Location 5' },
    
  ];

  selectedGroup: any = null;
  selectedCompany: any = null;
  selectedLocation: any = null;
  selectedUser: any = null;

  showUserManagementForm = false;
  showContentAboveForm = true;

  searchTerm: string = '';
  filteredGroups: any[] = this.groupOptions;


  selectedImage: any = ''; // This will hold the base64 image data

  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.convertToBase64(file);
    }
  }

  convertToBase64(file: File) {
    const reader = new FileReader();
    reader.onload = (e: any) => {
      this.selectedImage = e.target.result;
    };
    reader.readAsDataURL(file);
  }

  togglePages() {
    this.showPage1 = !this.showPage1;
  }
}
