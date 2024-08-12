import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule, Pipe } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatListModule } from '@angular/material/list';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { NgSelectModule } from '@ng-select/ng-select';
import { RouterModule } from '@angular/router';
import { DxReportViewerModule } from 'devexpress-reporting-angular';
import { ReactiveFormsModule, FormsModule, FormGroup } from '@angular/forms';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { CommonModule, DatePipe } from '@angular/common';
import { MatNativeDateModule } from '@angular/material/core';
import {
  NoopAnimationsModule,
  BrowserAnimationsModule,
} from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { MAT_DATE_FORMATS } from '@angular/material/core';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { DateAdapter, MAT_DATE_LOCALE } from '@angular/material/core';
import { LoginComponent } from './features/auth/login/login.component';
import { RegisterComponent } from './features/auth/register/register.component';
import { InventoryModule } from './features/inventory/inventory.module';
import { AccountsModule } from './features/accounts/accounts.module';
import { LayoutModule } from './features/layout/layout.module';
import { ViewReportComponent } from './features/reports/view-report/view-report.component';
import { TokenInterceptor } from './interceptors/token.interceptor';
import { ReportViewerComponent } from './features/report-viewer/report-viewer.component';
import { ReportModalComponent } from './features/report-modal/report-modal.component';
import { ModalModule } from 'ngx-bootstrap/modal';
import { PurchaseModule } from './features/purchase/purchase.module';
import { SaleModule } from './features/sale/sale.module';
import { UtilitiesModule } from './features/utilities/utilities.module';
import { AdminModule } from './features/admin/admin.module';
import { UserProfileComponent } from './features/auth/user-profile/user-profile.component';
import { PayrollModule } from './features/payroll/payroll.module';
import { GeneralStoreModule } from './features/general-store/general-store.module';
import { ProductionModule } from './features/production/production.module';
import { ApprovalModule } from './features/Approvals/approvals.module';
import {WeighBridgeModule} from './features/WeighBridge/weighbridge.module';

export const MY_DATE_FORMATS = {
  parse: {
    dateInput: 'DD/MM/YYYY',
  },
  display: {
    dateInput: 'DD/MM/YYYY',
    monthYearLabel: 'MM YYYY',
    dateA11yLabel: 'DD/MM/YYYY',
    monthYearA11yLabel: 'MM YYYY',
  },
};

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    UserProfileComponent,
    ViewReportComponent,
    ReportViewerComponent,
    ReportModalComponent,
  ],
  
  imports: [
    DxReportViewerModule,
    AdminModule,
    ApprovalModule,
    AccountsModule,
    WeighBridgeModule,
    InventoryModule,
    ProductionModule,
    GeneralStoreModule,
    PurchaseModule,
    PayrollModule,
    SaleModule,
    UtilitiesModule,
    LayoutModule,
    BrowserModule,
    AppRoutingModule,
    CommonModule,
    MatButtonModule,
    FormsModule,
    MatSelectModule,
    MatListModule,
    MatIconModule,
    NgSelectModule,
    RouterModule,
    BsDropdownModule,
    MatToolbarModule,
    MatFormFieldModule,
    MatCardModule,
    MatAutocompleteModule,
    MatDatepickerModule,
    MatInputModule,
    HttpClientModule,
    InventoryModule,
    ToastrModule.forRoot(),
    BrowserAnimationsModule,
    MatNativeDateModule,
    NoopAnimationsModule,
    ReactiveFormsModule,
    MatIconModule,
    ModalModule.forRoot()
  ],
  exports: [],

  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    },
    DatePipe,
    { provide: MAT_DATE_FORMATS, useValue: MY_DATE_FORMATS },
    {
      provide: DateAdapter,
      useClass: MomentDateAdapter,
      deps: [MAT_DATE_LOCALE],
    }
  ],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class AppModule {}
