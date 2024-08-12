import { Component, OnInit } from '@angular/core';
import { DatePipe } from '@angular/common';
import { ApiService } from 'src/app/services/api.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-ot-live-activity',
  templateUrl: './ot-live-activity.component.html',
  styleUrls: ['./ot-live-activity.component.css'],
})
export class OtLiveActivityComponent implements OnInit {
  constructor(
    private apiService: ApiService,
    private datePipe: DatePipe,
    private com: CommonService
  ) {}

  orderTakerList: any[] = [];
  orderTakerId: any;
  fromDate: Date;
  toDate: Date;
  tableData: any[] = [];
  status = null;

  statusOptions = [
    { id: 1, status: 'Login' },
    { id: 2, status: 'Logout' },
    { id: 3, status: 'New' },
    { id: 4, status: 'Edit' },
    { id: 5, status: 'Delete' },
    { id: 5, status: 'OnField' },
  ];

  map: google.maps.Map;
  marker: google.maps.Marker;

  ngOnInit(): void {
    this.getOrderTakerList();
    this.fromDate = new Date();
    this.toDate = new Date();
  }

  getOrderTakerList() {
    this.apiService.getData('Sale/GetOrderTakerList').subscribe((data) => {
      this.orderTakerList = data;
    });
  }

  fetchData() {
    if (this.orderTakerId == null) {
      this.tableData = [];
      return;
    }

    try {
      this.com.showLoader();
      const obj = {
        userId: this.orderTakerId,
        status: this.status == null ? '' : this.status,
        fromDate: this.datePipe.transform(this.fromDate, 'yyyy/MM/dd'),
        toDate: this.datePipe.transform(this.toDate, 'yyyy/MM/dd'),
      };

      this.apiService
        .getDataById('Sale/GetActivityList', obj)
        .subscribe((data) => {
          this.tableData = data;
          this.com.hideLoader();
        });
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  getLocation(x: number, y: number) {
    const latLng = new google.maps.LatLng(x, y);
    const mapOptions: google.maps.MapOptions = {
      zoom: 18,
      center: latLng,
      mapTypeId: google.maps.MapTypeId.ROADMAP,
    };

    this.map = new google.maps.Map(
      document.getElementById('map') as HTMLElement,
      mapOptions
    );

    this.marker = new google.maps.Marker({
      position: latLng,
      map: this.map,
      title: 'Your current location!',
    });
  }
  export(fileName: any, formate: any) {
    this.com.ExportFiles(fileName, formate);
  }
}
