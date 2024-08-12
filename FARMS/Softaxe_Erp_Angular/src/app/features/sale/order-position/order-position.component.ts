import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService } from 'src/app/services/common.service';
declare const $: any;

@Component({
  selector: 'app-order-position',
  templateUrl: './order-position.component.html',
  styleUrls: ['./order-position.component.css'],
})
export class OrderPositionComponent {
  map: google.maps.Map;
  marker: google.maps.Marker;

  fromDate: Date;
  toDate: Date;

  status: boolean = false;
  tag: any = '0';
  isBilled: boolean = false;
  orderList: any[] = [];

  constructor(
    private apiService: ApiService,
    private datePipe: DatePipe,
    private tostr: ToastrService,
    private auth: AuthService,
    private com: CommonService
  ) {
    const today = new Date();
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  }

  ngOnInit(): void {
    this.getDOList();
  }

  async onClickStatus(status: any, tag: any) {
    this.tag = tag;
    await this.getDOList();
    this.status = status;
  }

  async getDOList() {
    try {
      this.com.showLoader();
      const obj = {
        fromDate: this.datePipe.transform(this.fromDate, 'yyyy/MM/dd'),
        toDate: this.datePipe.transform(this.toDate, 'yyyy/MM/dd'),
        status: this.tag,
      };

      const data = await this.apiService
        .getDataById('Sale/GetOrderReceivedList', obj)
        .toPromise();
      this.orderList = data;
      this.com.hideLoader();
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  onClickLocation(x: number, y: number) {
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

    $('#LocationModal').modal('show');
  }

  onViewInvoice(i: any, type: any) {
    const dp = i.INVDATE.split('/');
    const invdate = this.datePipe.transform(
      new Date(dp[2], dp[1] - 1, dp[0]),
      'yyyy/MM/dd'
    );

    let url = '';
    if (type === 'invoice') {
      url = `SaleInvoice?VchNoFrom=${i.INVNO}&VchNoTo=${
        i.INVNO
      }&VchType=SP&fromDate=${invdate}&toDate=${invdate}&CmpId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}`;
    } else if (type === 'loading') {
      url = `SaleLoading?VchNo=${
        i.INVNO
      }&VchType=SP&CompId=${this.auth.cmpId()}&FinId=${this.auth.finId()}&LocId=${this.auth.locId()}`;
    }
    this.com.viewReport(url);
  }

  onClickDelivered(i: any) {
    var x = confirm('Are you sure order is delivered?');
    if (x == false) {
      return false;
    }

    try {
      this.com.showLoader();
      var obj = {
        doNo: i.DONO,
        vchNo: i.INVNO,
      };

      this.apiService.saveObj('Sale/OrderDelivered', obj).subscribe((data) => {
        if (data == true || data == 'true') {
          this.tostr.success('Order Delivered...!');
          this.getDOList();
          this.com.hideLoader();
        } else {
        }
      });
    } catch (err) {
      this.com.hideLoader();
      console.log(err);
    } finally {
      //this.com.hideLoader();
    }
  }

  export(fileName: any, formate: any) {
    this.com.ExportFiles(fileName, formate);
  }
}
