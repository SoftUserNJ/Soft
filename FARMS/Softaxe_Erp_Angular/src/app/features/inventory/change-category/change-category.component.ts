import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-change-category',
  templateUrl: './change-category.component.html',
  styleUrls: ['./change-category.component.css'],
})
export class ChangeCategoryComponent {
  fromCategory: any[] = [];
  toCategory: any[] = [];
  brand: any[] = [];

  fCategoryId: any;
  fBrandId: any;
  tCategoryId: any;

  constructor(
    private apiService: ApiService,
    private tostr: ToastrService,
    private dp: DatePipe,
    private com: CommonService
  ) {}

  ngOnInit(): void {
    this.getCategory();
  }

  getCategory() {
    this.apiService.getData('Inventory/GetCategory').subscribe((data) => {
      this.fromCategory = data;
      this.toCategory = data;
    });
  }

  onChangeCategory(event: any) {
    this.fBrandId = undefined;

    let obj = {
      categoryId: event.id,
    };
    this.apiService.getDataById('Inventory/GetBrand', obj).subscribe((data) => {
      this.brand = data;
    });
  }

  onClearCategory() {
    this.brand = [];
    this.fBrandId = undefined;
  }

  onClickSave() {
    if (this.fCategoryId == undefined) {
      this.tostr.warning('Select From Category');
      return;
    }

    if (this.fBrandId == undefined) {
      this.tostr.warning('Select Brand');
      return;
    }

    if (this.tCategoryId == undefined) {
      this.tostr.warning('Select To Category');
      return;
    }

    if (this.fCategoryId == this.tCategoryId) {
      this.tostr.warning('To Category Selection Wrong');
      return;
    }

    try {
      this.com.showLoader();
      let obj = {
        fCategory: this.fCategoryId,
        fBrand: this.fBrandId,
        tCategory: this.tCategoryId,
        dtNow: this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss'),
      };

      this.apiService
        .saveObj('Inventory/UpdateCategory', obj)
        .subscribe((result) => {
          if (result == true || result == 'true') {
            this.tostr.success('Save Successfully');
            this.onChangeCategory({ id: obj.fCategory });
            this.fBrandId = undefined;
            this.com.hideLoader();
          } else {
            this.com.hideLoader();
            this.tostr.error('Please Save Again');
          }
        });
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }
}
