import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { RequisitionsEntryComponent } from './requisitions-entry/requisitions-entry.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
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
import { authGuard } from 'src/app/guards/auth.guard';
import { MaterialReceiveAgainstDemmadComponent } from './material-receive-against-demmad/material-receive-against-demmad.component';
import { DepartmentEntryComponent } from './department-entry/department-entry.component';
import { SubDepartmentEntryComponent } from './sub-department-entry/sub-department-entry.component';
import { DemandEntryComponent } from './demand-entry/demand-entry.component';
import { MaterialReceivedInStoreComponent } from './material-received-in-store/material-received-in-store.component';
import { GatePassInwardReportComponent } from './gate-pass-inward-report/gate-pass-inward-report.component';



@NgModule({
  declarations: [
    RequisitionsEntryComponent,
    MaterialReceiveAgainstDemmadComponent,
    DepartmentEntryComponent,
    SubDepartmentEntryComponent,
    DemandEntryComponent,
    MaterialReceivedInStoreComponent,
    GatePassInwardReportComponent
  ],
  imports: [  
    CommonModule,
    RouterModule.forChild([
      { path: 'requisitions-entry', component: RequisitionsEntryComponent, canActivate: [authGuard] },
      { path: 'material-receive-entry', component: MaterialReceiveAgainstDemmadComponent, canActivate: [authGuard] },
      { path: 'department-entry', component: DepartmentEntryComponent, canActivate: [authGuard] },
      { path: 'sub-department-entry', component: SubDepartmentEntryComponent, canActivate: [authGuard] },
      { path: 'demand-entry', component: DemandEntryComponent, canActivate: [authGuard] },
      { path: 'material-received-in-store', component: MaterialReceivedInStoreComponent, canActivate: [authGuard] },
      { path: 'gate-pass-inward-report', component: GatePassInwardReportComponent, canActivate: [authGuard] },
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
    MatCardModule ,
    MatAutocompleteModule,
    BrowserModule,
    MatDatepickerModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    CalendarModule,
    DatePipe
  ]
})
export class GeneralStoreModule { }
