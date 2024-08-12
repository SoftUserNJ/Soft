import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { CustomerBioDataComponent } from './customer-bio-data/customer-bio-data.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { authGuard } from 'src/app/guards/auth.guard';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatListModule } from '@angular/material/list';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { BrowserModule } from '@angular/platform-browser';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CalendarModule } from 'primeng/calendar';
import { BillDueStatusComponent } from './bill-due-status/bill-due-status.component';
import { CustomerLeadgerComponent } from './customer-leadger/customer-leadger.component';
import { OtLiveActivityComponent } from './ot-live-activity/ot-live-activity.component';
import { SaleAreaUpdationComponent } from './sale-area-updation/sale-area-updation.component';
import { SalesComponent } from './sales/sales.component';
import { SalesPaymentComponent } from './sales-payment/sales-payment.component';
import { PostDatedChequeComponent } from './post-dated-cheque/post-dated-cheque.component';
import { PartyBalanceStatusComponent } from './party-balance-status/party-balance-status.component';
import { ServiceComponent } from './service/service.component';
import { AvailServiceComponent } from './avail-service/avail-service.component';
import { SalesMarginStatusComponent } from './sales-margin-status/sales-margin-status.component';
import { SalesByOrderComponent } from './sales-by-order/sales-by-order.component';
import { SalesMenCommissionComponent } from './sales-men-commission/sales-men-commission.component';
import { CustomerReceivableAgingComponent } from './customer-receivable-aging/customer-receivable-aging.component';
import { OrderTakerCommissionComponent } from './order-taker-commission/order-taker-commission.component';
import { SalesReceiptsComponent } from './sales-receipts/sales-receipts.component';
import { ProductWiseSaleComponent } from './product-wise-sale/product-wise-sale.component';
import { CustomerUpdationComponent } from './customer-updation/customer-updation.component';
import { SharedModule } from 'src/app/shared.module';
import { PartiesTaxDeductionComponent } from './parties-tax-deduction/parties-tax-deduction.component';
import { OrderPositionComponent } from './order-position/order-position.component';
import { SalePrintInvRangeWiseComponent } from './sale-print-inv-range-wise/sale-print-inv-range-wise.component';
import { CostCentreComponent } from './cost-centre/cost-centre.component';
import { CostCentreStatusComponent } from './cost-centre-status/cost-centre-status.component';
import { JobWiseDueStatusComponent } from './job-wise-due-status/job-wise-due-status.component';
import { ProductSaleIssueComponent } from './product-sale-issue/product-sale-issue.component';
import { FlockExpenceReportComponent } from './flock-expence-report/flock-expence-report.component';
import { FlockPerformanceComponent } from './flock-performance/flock-performance.component';
import { SaleGatePassOutComponent } from './sale-gate-pass-out/sale-gate-pass-out.component';
import { FinishedGoodsEntryComponent } from './finished-goods-entry/finished-goods-entry.component';
import { SaleBookingEntryComponent } from './sale-booking-entry/sale-booking-entry.component';
import { ProductWiseSaleReportComponent } from './product-wise-sale-report/product-wise-sale-report.component';
import { DailyFreightReportComponent } from './daily-freight-report/daily-freight-report.component';
import { DailySaleActivityReportComponent } from './daily-sale-activity-report/daily-sale-activity-report.component';
import { PrintBookingReportComponent } from './print-booking-report/print-booking-report.component';
import { SalePersonWiseSaleReportComponent } from './sale-person-wise-sale-report/sale-person-wise-sale-report.component';
import { PartyTermsAndConditionsComponent } from './party-terms-and-conditions/party-terms-and-conditions.component';
import { DeliveryOrderComponent } from './delivery-order/delivery-order.component';
import { MaterialReceivingReportComponent } from './material-receiving-report/material-receiving-report.component';
import { GPOutWardComponent } from './gp-out-ward/gp-out-ward.component';

@NgModule({
  declarations: [
    CustomerBioDataComponent,
    BillDueStatusComponent,
    CustomerLeadgerComponent ,
    OtLiveActivityComponent ,
    SaleAreaUpdationComponent ,
    SalesComponent ,
    SalesPaymentComponent,
    PostDatedChequeComponent,
    PartyBalanceStatusComponent,
    SalesMenCommissionComponent,
    SalesByOrderComponent,
    SalesMarginStatusComponent,
    AvailServiceComponent,
    ServiceComponent,
    CustomerReceivableAgingComponent,
    OrderTakerCommissionComponent,
    SalesReceiptsComponent,
    ProductWiseSaleComponent,
    CustomerUpdationComponent,
    PartiesTaxDeductionComponent,
    OrderPositionComponent,
    SalePrintInvRangeWiseComponent,
    CostCentreComponent,
    CostCentreStatusComponent,
    JobWiseDueStatusComponent,
    ProductSaleIssueComponent,
    FlockExpenceReportComponent,
    FlockPerformanceComponent,
    SaleGatePassOutComponent,
    FinishedGoodsEntryComponent,
    SaleBookingEntryComponent,
    ProductWiseSaleReportComponent,
    DailyFreightReportComponent,
    DailySaleActivityReportComponent,
    PrintBookingReportComponent,
    SalePersonWiseSaleReportComponent,
    PartyTermsAndConditionsComponent,
    DeliveryOrderComponent,
    MaterialReceivingReportComponent,
    GPOutWardComponent,
  ],
  imports: [
    RouterModule.forChild([
      { path: 'customer-bio-data', component: CustomerBioDataComponent, canActivate: [authGuard]},
      { path: 'cost-centre', component: CostCentreComponent, canActivate: [authGuard]},
      { path: 'cost-centre-status', component: CostCentreStatusComponent, canActivate: [authGuard]},
      { path: 'customer-updation', component: CustomerUpdationComponent, canActivate: [authGuard]},
      { path: 'ot-live-activity', component: OtLiveActivityComponent, canActivate: [authGuard]},
      { path: 'sale-invoice', component: SalesComponent, canActivate: [authGuard]},
      { path: 'sale-bill-due-status', component: BillDueStatusComponent, canActivate: [authGuard]},
      { path: 'job-wise-due-status', component: JobWiseDueStatusComponent, canActivate: [authGuard]},
      { path: 'sale-payment', component: SalesPaymentComponent, canActivate: [authGuard]},
      { path: 'sale-receipts', component: SalesReceiptsComponent, canActivate: [authGuard]},
      { path: 'sale-post-dated-cheque', component: PostDatedChequeComponent, canActivate: [authGuard]},
      { path: 'customer-ledger', component: CustomerLeadgerComponent, canActivate: [authGuard]},
      { path: 'sale-party-balance-status', component: PartyBalanceStatusComponent, canActivate: [authGuard]},
      { path: 'service', component: ServiceComponent, canActivate: [authGuard]},
      { path: 'avail-service', component: AvailServiceComponent, canActivate: [authGuard]},
      { path: 'sale-area-updation', component: SaleAreaUpdationComponent, canActivate: [authGuard]},
      { path: 'sale-margin-status', component: SalesMarginStatusComponent, canActivate: [authGuard]},
      { path: 'sale-by-order', component: SalesByOrderComponent, canActivate: [authGuard]},
      { path: 'customer-receivable-aging', component: CustomerReceivableAgingComponent, canActivate: [authGuard]},
      { path: 'salesman-comission', component:SalesMenCommissionComponent, canActivate: [authGuard]},
      { path: 'order-taker-comission', component:OrderTakerCommissionComponent, canActivate: [authGuard]},
      { path: 'product-wise-sale', component:ProductWiseSaleComponent, canActivate: [authGuard]},
      { path: 'parties-tax-deduction', component:PartiesTaxDeductionComponent, canActivate: [authGuard]},
      { path: 'order-position', component:OrderPositionComponent, canActivate: [authGuard]},
      { path: 'sale-print-invoice-range-wise', component:SalePrintInvRangeWiseComponent, canActivate: [authGuard]},
      { path: 'product-sale-issue', component:ProductSaleIssueComponent, canActivate: [authGuard]},
      { path: 'flock-exp-report', component:FlockExpenceReportComponent, canActivate: [authGuard]},
      { path: 'flock-performance', component:FlockPerformanceComponent, canActivate: [authGuard]},
      { path: 'gate-pass-out-ward', component:GPOutWardComponent, canActivate: [authGuard]},
      
      { path: 'sale-gate-pass-out', component:SaleGatePassOutComponent, canActivate: [authGuard]},
      { path: 'finished-goods-entry', component:FinishedGoodsEntryComponent, canActivate: [authGuard]},
      { path: 'sale-booking-entry', component:SaleBookingEntryComponent, canActivate: [authGuard]},
      { path: 'products-wise-sale-report', component:ProductWiseSaleReportComponent, canActivate: [authGuard]},
      { path: 'daily-freight-report', component:DailyFreightReportComponent, canActivate: [authGuard]},
      { path: 'daily-sale-activity-report', component:DailySaleActivityReportComponent, canActivate: [authGuard]},
      { path: 'print-booking-report', component:PrintBookingReportComponent, canActivate: [authGuard]},
      { path: 'sale-person-wise-sale-report', component:SalePersonWiseSaleReportComponent, canActivate: [authGuard]},
      { path: 'party-terms-and-conditions', component:PartyTermsAndConditionsComponent, canActivate: [authGuard]},
      { path: 'delivery-order', component:DeliveryOrderComponent, canActivate: [authGuard]},
      { path: 'material-receiving-report', component:MaterialReceivingReportComponent, canActivate: [authGuard]},
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
  ]
})
export class SaleModule { 

}
