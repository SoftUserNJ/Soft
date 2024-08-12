import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { CalendarModule } from 'primeng/calendar';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserModule } from '@angular/platform-browser';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatListModule } from '@angular/material/list';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedModule } from 'primeng/api';
// import { SharedModule } from 'src/app/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { authGuard } from 'src/app/guards/auth.guard';
import { FinishGoodsStatusComponent } from './finish-goods-status/finish-goods-status.component';
import { InwardStatusPurchaseOfFinishedGoodsComponent } from './inward-status-purchase-of-finished-goods/inward-status-purchase-of-finished-goods.component';
import { InwardWeighmentComponent } from './inward-weighment/inward-weighment.component';
import { OutwardWeighmentComponent } from './outward-weighment/outward-weighment.component';
import { InwardPurchaseStatusComponent } from './inward-purchase-status/inward-purchase-status.component';
import { DailyArrivalReportComponent } from './daily-arrival-report/daily-arrival-report.component';








@NgModule({
  declarations: [
    FinishGoodsStatusComponent,
    InwardStatusPurchaseOfFinishedGoodsComponent,
    OutwardWeighmentComponent,
    InwardWeighmentComponent,
    InwardPurchaseStatusComponent,
    DailyArrivalReportComponent
  ],
  imports: [  
    RouterModule.forChild([ 
      { path: 'sale-status', component: FinishGoodsStatusComponent, canActivate: [authGuard]},
      { path: 'outward-weighment', component: OutwardWeighmentComponent, canActivate: [authGuard]},
      { path: 'inward-weighment', component: InwardWeighmentComponent, canActivate: [authGuard]},
      { path: 'inward-status-finised-goods', component: InwardStatusPurchaseOfFinishedGoodsComponent, canActivate: [authGuard]},
      { path: 'inward-purchase-status', component: InwardPurchaseStatusComponent, canActivate: [authGuard]},
      { path: 'daily-arrival-report', component: DailyArrivalReportComponent, canActivate: [authGuard]},
    ]),
    CommonModule,
    NgSelectModule,
    RouterModule,
    FormsModule,
    MatButtonModule,
    MatInputModule,
    MatSelectModule,
    MatToolbarModule,
    MatListModule,
    MatFormFieldModule,
    MatIconModule,
    MatCardModule ,
    MatAutocompleteModule,
    BrowserModule,
    MatDatepickerModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    CalendarModule,
    DatePipe,
    SharedModule
  ],
  exports: [
 
  ],
})
export class WeighBridgeModule {}