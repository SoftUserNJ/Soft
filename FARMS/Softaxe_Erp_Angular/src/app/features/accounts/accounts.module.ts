import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChartOfAccountsComponent } from './chart-of-accounts/chart-of-accounts.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { RouterModule } from '@angular/router';
import { AccountOpeningsComponent } from './account-openings/account-openings.component';
import { FormsModule } from '@angular/forms';
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
import { TrialBalanceComponent } from './trial-balance/trial-balance.component';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { ReactiveFormsModule } from '@angular/forms';
import { PaymentComponent } from './payment/payment.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CalendarModule } from 'primeng/calendar';
import { JournalVoucherComponent } from './journal-voucher/journal-voucher.component';
import { ReciptsComponent } from './recipts/recipts.component';
import { ReciptsPaymentStatusComponent } from './recipts-payment-status/recipts-payment-status.component';
import { DatePipe } from '@angular/common';
import { AccountLedgerComponent } from './account-ledger/account-ledger.component';
import { authGuard } from 'src/app/guards/auth.guard';
import { VerificationComponent } from './verification/verification.component';
import { ApprovalComponent } from './approval/approval.component';
import { AuditComponent } from './audit/audit.component';
import { VchTypeSettingComponent } from './vch-type-setting/vch-type-setting.component';
import { DayBookComponent } from './day-book/day-book.component';
import { ProfitAndLossComparisonComponent } from './profit-and-loss-comparison/profit-and-loss-comparison.component';
import { PrintVoucherRangeWiseComponent } from './print-voucher-range-wise/print-voucher-range-wise.component';
import { SharedModule } from 'src/app/shared.module';
import { JvOpeningComponent } from './jv-opening/jv-opening.component';
import { BalanceSheetComponent } from './balance-sheet/balance-sheet.component';
import { CashFlowComponent } from './cash-flow/cash-flow.component';
import { FreightPaymentsComponent } from './freight-payments/freight-payments.component';
import { ChartOfAccountsErpComponent } from './chart-of-accounts-erp/chart-of-accounts-erp.component';

@NgModule({
  declarations: [
    ChartOfAccountsComponent,
    AccountOpeningsComponent,
    PaymentComponent,
    ReciptsComponent,
    JournalVoucherComponent,
    ReciptsPaymentStatusComponent,
    AccountLedgerComponent,
    TrialBalanceComponent,
    VerificationComponent,
    ApprovalComponent,
    AuditComponent,
    VchTypeSettingComponent,
    DayBookComponent,
    ProfitAndLossComparisonComponent,
    PrintVoucherRangeWiseComponent,
    JvOpeningComponent,
    BalanceSheetComponent,
    CashFlowComponent,
    FreightPaymentsComponent,
    ChartOfAccountsErpComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild([
      { path: 'chart-of-account', component: ChartOfAccountsComponent, canActivate: [authGuard] },
      { path: 'chart-of-account-erp', component: ChartOfAccountsErpComponent, canActivate: [authGuard] },
      { path: 'accounts-opening', component: AccountOpeningsComponent, canActivate: [authGuard] },
      { path: 'jv-opening', component: JvOpeningComponent, canActivate: [authGuard] },
      { path: 'accounts-payment', component: PaymentComponent, canActivate: [authGuard] },
      { path: 'accounts-receipts', component: ReciptsComponent, canActivate: [authGuard] },
      { path: 'journal-voucher', component: JournalVoucherComponent, canActivate: [authGuard] },
      { path: 'recipts-payment-status', component: ReciptsPaymentStatusComponent, canActivate: [authGuard] },
      { path: 'account-ledger', component: AccountLedgerComponent, canActivate: [authGuard] },
      { path: 'balance-Sheet', component: BalanceSheetComponent, canActivate: [authGuard] },
      { path: 'trial-balance', component: TrialBalanceComponent, canActivate: [authGuard] },
      { path: 'verification', component: VerificationComponent, canActivate: [authGuard] },
      { path: 'approval', component: ApprovalComponent, canActivate: [authGuard] },
      { path: 'audit', component: AuditComponent, canActivate: [authGuard] },
      { path: 'vch-type-setting', component: VchTypeSettingComponent, canActivate: [authGuard] },
      { path: 'profit-and-loss-comparison', component: ProfitAndLossComparisonComponent, canActivate: [authGuard] },
      { path: 'day-book', component: DayBookComponent, canActivate: [authGuard] },
      { path: 'print-voucher-range-wise', component: PrintVoucherRangeWiseComponent, canActivate: [authGuard] },
      { path: 'cash-flow', component: CashFlowComponent, canActivate: [authGuard] },
      { path: 'freight-payments', component: FreightPaymentsComponent, canActivate: [authGuard] },
    ]),
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
    MatIconModule,
    MatCardModule ,
    MatAutocompleteModule,
    BrowserModule,
    MatDatepickerModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    CalendarModule,
    DatePipe,
    SharedModule,
  ],
  exports: [
    ChartOfAccountsComponent,
    AccountOpeningsComponent,
    TrialBalanceComponent,
    PaymentComponent,
    JournalVoucherComponent
  ]
})
export class AccountsModule { }