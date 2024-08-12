import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
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
import { SharedModule } from 'src/app/shared.module';
import { FlockDetailReportComponent } from './flock-detail-report/flock-detail-report.component';
import { JobUpdateComponent } from './job-update/job-update.component';
import { FinishedGoodsProductionComponent } from './finished-goods-production/finished-goods-production.component';

@NgModule({
  declarations: [
    FlockDetailReportComponent,
    JobUpdateComponent,
    FinishedGoodsProductionComponent
  ],
  imports: [
    RouterModule.forChild([
      { path: 'job-update', component: JobUpdateComponent, canActivate: [authGuard]},
      { path: 'flock-detail-report', component: FlockDetailReportComponent, canActivate: [authGuard]},
      { path: 'finished-goods-production', component: FinishedGoodsProductionComponent, canActivate: [authGuard]},
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
export class ProductionModule { }