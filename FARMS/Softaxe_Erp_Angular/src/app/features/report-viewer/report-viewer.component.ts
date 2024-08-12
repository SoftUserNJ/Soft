import { Component, ViewEncapsulation, Input } from '@angular/core';
import { environment } from '../../../environment/environmemt';

@Component({
  selector: 'app-report-viewer',
  encapsulation: ViewEncapsulation.None,
  templateUrl: './report-viewer.component.html',
  styleUrls: ['./report-viewer.component.css']
})
export class ReportViewerComponent {
  @Input() reportUrl: string; // Add this @Input() property to receive reportUrl from the parent component

  title = 'DXReportViewerSample';
  hostUrl: string =  environment.basePath + "/";
  invokeAction: string = 'DXXRDV';
  showReport: boolean = false;
  useSameTab: boolean = true;
  showPrintNotificationDialog: boolean = false;

  ngOnInit() {
    this.reportUrl = this.reportUrl;
    this.showReport = true;
    
  }
}



// export class ReportViewerComponent {
  
//   title = 'DXReportViewerSample';
//   reportUrl: string = 'Products';
//   hostUrl: string = 'https://localhost:7151/';
//   invokeAction: string = "DXXRDV";
//   showReport: boolean = false;
//   ngOnInit()
//  {

//   var parameterValue = "Hello World";
//   this.reportUrl = "Products?parameter1=" + parameterValue;
//   this.showReport = true;
//  }


// }





