import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { SupplierBioDataComponent } from './supplier-bio-data/supplier-bio-data.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NgSelectModule } from '@ng-select/ng-select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { authGuard } from 'src/app/guards/auth.guard';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { BrowserModule } from '@angular/platform-browser';
import { MatListModule } from '@angular/material/list';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CalendarModule } from 'primeng/calendar';
import { PostDatedChequeComponent } from './post-dated-cheque/post-dated-cheque.component';
import { BillDueStatusComponent } from './bill-due-status/bill-due-status.component';
import { SupplierLeadgerComponent } from './supplier-leadger/supplier-leadger.component';
import { PartyBalanceStatusComponent } from './party-balance-status/party-balance-status.component';
import { PurchaseInvoiceComponent } from './purchase-invoice/purchase-invoice.component';
import { PurchasePaymentComponent } from './purchase-payment/purchase-payment.component';
import { PurchaseReciptsComponent } from './purchase-recipts/purchase-recipts.component';
import { PurchasePayableAgingComponent } from './purchase-payable-aging/purchase-payable-aging.component';
import { SharedModule } from 'src/app/shared.module';
import { PurchasePrintInvRangeWiseComponent } from './purchase-print-inv-range-wise/purchase-print-inv-range-wise.component';
import { PurchaseContractFormComponent } from './purchase-contract-form/purchase-contract-form.component';
import { PurchaseContractReportComponent } from './purchase-contract-report/purchase-contract-report.component';
import { PurchaseContractRangeWiseReportComponent } from './purchase-contract-range-wise-report/purchase-contract-range-wise-report.component';
import { PurchaseGatePassInwardEntryComponent } from '../purchase-gate-pass-inward-entry/purchase-gate-pass-inward-entry.component';
import { PurchaseCorrectionFormComponent } from './purchase-correction-form/purchase-correction-form.component';
import { LabTestEntryComponent } from './lab-test-entry/lab-test-entry.component';
import { LabTestResultEntryComponent } from './lab-test-result-entry/lab-test-result-entry.component';
import { LabTestTypeComponent } from './lab-test-type/lab-test-type.component';
import { PurchaseWorkingFormComponent } from './purchase-working-form/purchase-working-form.component';
import { MaizeRateComponent } from './maize-rate/maize-rate.component';


@NgModule({
  declarations: [
    SupplierBioDataComponent,
    BillDueStatusComponent,
    PostDatedChequeComponent,
    SupplierLeadgerComponent,
    PartyBalanceStatusComponent,
    PurchaseInvoiceComponent,
    PurchasePaymentComponent,
    PurchaseReciptsComponent,
    PurchasePayableAgingComponent,
    PurchasePrintInvRangeWiseComponent,
    PurchaseContractFormComponent,
    PurchaseContractRangeWiseReportComponent,
    PurchaseContractReportComponent,
    PurchaseGatePassInwardEntryComponent,
    PurchaseCorrectionFormComponent,
    LabTestTypeComponent,
    LabTestResultEntryComponent,
    LabTestEntryComponent,
    PurchaseWorkingFormComponent,
    MaizeRateComponent
  ],
  imports: [
    RouterModule.forChild([
      { path: 'supplier-bio-data', component: SupplierBioDataComponent, canActivate: [authGuard]},
      { path: 'purchase-bill-due-status', component: BillDueStatusComponent, canActivate: [authGuard]},
      { path: 'purchase-post-dated-cheque', component: PostDatedChequeComponent, canActivate: [authGuard]},
      { path: 'supplier-ledger', component: SupplierLeadgerComponent, canActivate: [authGuard]},
      { path: 'purchase-party-balance-status', component: PartyBalanceStatusComponent, canActivate: [authGuard]},
      { path: 'purchase-invoice',  component: PurchaseInvoiceComponent, canActivate: [authGuard]},
      { path: 'purchase-payable-aging',  component: PurchasePayableAgingComponent, canActivate: [authGuard]},
      { path: 'purchase-payment',  component: PurchasePaymentComponent, canActivate: [authGuard]},
      { path: 'purchase-receipts',  component: PurchaseReciptsComponent, canActivate: [authGuard]},
      { path: 'purchase-print-invoice-range-wise',  component: PurchasePrintInvRangeWiseComponent, canActivate: [authGuard]},
      { path: 'purchase-contract-form',  component: PurchaseContractFormComponent, canActivate: [authGuard]},
      { path: 'purchase-contract-range-wise-report',  component: PurchaseContractRangeWiseReportComponent, canActivate: [authGuard]},
      { path: 'purchase-contract-report',  component: PurchaseContractReportComponent, canActivate: [authGuard]},
      { path: 'purchase-correction-form',  component: PurchaseCorrectionFormComponent, canActivate: [authGuard]},
      { path: 'purchase-gate-pass-inward-entry',  component: PurchaseGatePassInwardEntryComponent, canActivate: [authGuard]},
      { path: 'lab-test-type-entry',  component: LabTestTypeComponent, canActivate: [authGuard]},
      { path: 'lab-test-result-entry',  component: LabTestResultEntryComponent, canActivate: [authGuard]},
      { path: 'lab-test-entry',  component: LabTestEntryComponent, canActivate: [authGuard]},
      { path: 'purchase-working-form',  component: PurchaseWorkingFormComponent, canActivate: [authGuard]},
      { path: 'maize-rate',  component: MaizeRateComponent, canActivate: [authGuard]},
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
})
export class PurchaseModule { }