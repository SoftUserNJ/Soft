import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { RouterModule } from '@angular/router';
import { authGuard } from 'src/app/guards/auth.guard';
import { NgSelectModule } from '@ng-select/ng-select';
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
import { CompanyGroupComponent } from './company-group/company-group.component';
import { CompanyComponent } from './company/company.component';
import { LocationComponent } from './location/location.component';

@NgModule({
  declarations: [
    CompanyGroupComponent,
    CompanyComponent,
    LocationComponent,
  ],
  imports: [
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
    MatIconModule,
    MatCardModule ,
    MatAutocompleteModule,
    BrowserModule,
    MatDatepickerModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    CalendarModule,
    DatePipe,
    RouterModule.forChild([
      { path: 'company-group', component: CompanyGroupComponent, canActivate: [authGuard] },
      { path: 'company', component: CompanyComponent, canActivate: [authGuard] },
      { path: 'location', component: LocationComponent, canActivate: [authGuard] },
    ])
  ]
})
export class AdminModule { }
