import { Component, ElementRef, ViewChild } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';
import { ToastrService } from 'ngx-toastr';
import { environment } from '../../../../environment/environmemt';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-mobile-app-slider',
  templateUrl: './mobile-app-slider.component.html',
  styleUrls: ['./mobile-app-slider.component.css'],
})
export class MobileAppSliderComponent {
  @ViewChild('sliderLists', { static: false }) sliderLists!: ElementRef;

  basePath = environment.basePath;
  sliderList: any[] = [];

  productImage: File | null = null;
  selectedImage: any = '';
  file: any;

  constructor(
    private apiService: ApiService,
    private tostr: ToastrService,
    private com: CommonService
  ) {}

  ngOnInit() {
    this.GetSliderList();
  }

  GetSliderList() {
    this.apiService.getData('Utilities/GetSlider').subscribe((data) => {
      this.sliderList = data;
    });
  }

  onClickSortUpdate() {
    try {
      this.com.showLoader();

      const slider: any[] = this.sliderList.map((data) => ({
        id: data.Id,
        Sort: data.Sort,
      }));

      this.apiService.saveData('Utilities/UpdateSorting', slider).subscribe(
        (result) => {
          if (result == true || result == 'true') {
            this.com.hideLoader();
            this.tostr.success('Update Successfully');
          } else {
            this.com.hideLoader();
            this.tostr.error('Please Save Again');
          }
        },
        (error) => {
          this.com.hideLoader();
          this.tostr.error('On Err');
        }
      );
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  deleteSliderList(item: any): void {
    const confirmDelete = confirm('Are you sure you want to delete this item?');

    if (!confirmDelete) {
      return;
    }

    try {
      this.com.showLoader();
      const obj = {
        id: item.Id,
        name: item.fileName,
        path: item.path,
      };

      this.apiService.deleteData('Utilities/DeleteSlider', obj).subscribe({
        next: (data) => {
          if (data == 'true' || data == true) {
            this.tostr.success('Delete Successfully');
            this.GetSliderList();
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

  onFileUpload() {
    try {
      this.com.showLoader();
      let formData = new FormData();
      formData.append('image', this.productImage!);

      this.apiService.saveData('Utilities/FileUpload', formData).subscribe(
        (result) => {
          if (result == true || result == 'true') {
            this.tostr.success('Save Successfully');
            this.GetSliderList();
            this.com.hideLoader();
          } else {
            this.com.hideLoader();
            this.tostr.error('Please Save Again');
          }
        },
        (error) => {
          this.com.hideLoader();
          this.tostr.error('On Err');
        }
      );
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  onFileSelected(event: any) {
    this.productImage = event.target.files[0];
    if (this.productImage) {
      this.selectedImage = URL.createObjectURL(event.target.files[0]);
    }
  }

  searchGrid(event: any): void {
    const tableElement = this.sliderLists.nativeElement;
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

  export(fileName: any, formate: any) {
    this.com.ExportFiles(fileName, formate);
  }
}
