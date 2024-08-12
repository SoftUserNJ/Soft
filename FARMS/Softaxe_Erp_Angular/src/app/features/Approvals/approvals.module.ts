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
import { GatePassInwardApprovalComponent } from './gate-pass-inward-approval/gate-pass-inward-approval.component';
import { PurchaseOrderApprovalComponent } from './purchase-order-approval/purchase-order-approval.component';
import { RouterModule } from '@angular/router';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedModule } from 'primeng/api';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { authGuard } from 'src/app/guards/auth.guard';
import { MaizeRateApprovalComponent } from './maize-rate-approval/maize-rate-approval.component';
import { TermAndConditionsApprovalComponent } from './term-and-conditions-approval/term-and-conditions-approval.component';
import { DOApprovalComponent } from './do-approval/do-approval.component';


@NgModule({
  declarations: [
    PurchaseOrderApprovalComponent,
    GatePassInwardApprovalComponent,
    MaizeRateApprovalComponent,
    TermAndConditionsApprovalComponent,
    DOApprovalComponent

  ],
  imports: [  
    RouterModule.forChild([ 
      { path: 'gate-pass-inward-approval', component: GatePassInwardApprovalComponent, canActivate: [authGuard]},
      { path: 'purchase-order-approval', component: PurchaseOrderApprovalComponent, canActivate: [authGuard]},
      { path: 'maize-rate-approval', component: MaizeRateApprovalComponent, canActivate: [authGuard]},
      { path: 'party-terms-condition-approval', component: TermAndConditionsApprovalComponent, canActivate: [authGuard]},
      { path: 'do-approval', component: DOApprovalComponent, canActivate: [authGuard]},
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
export class ApprovalModule {}