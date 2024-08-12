import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
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
import { UsersLogStatusComponent } from './users-log-status/users-log-status.component';
import { UsersComponent } from './users/users.component';
import { SecurityComponent } from './security/security.component';
import { MobileAppSliderComponent } from './mobile-app-slider/mobile-app-slider.component';

@NgModule({
  declarations: [
    UsersLogStatusComponent,
    UsersComponent,
    SecurityComponent,
    MobileAppSliderComponent,
  ],

  imports: [
    RouterModule.forChild([
      { path: 'users-log-status', component: UsersLogStatusComponent, canActivate: [authGuard]},
      { path: 'users-list', component: UsersComponent, canActivate: [authGuard]},
      { path: 'security', component: SecurityComponent, canActivate: [authGuard]},
      { path: 'mobile-app-slider', component: MobileAppSliderComponent, canActivate: [authGuard]},
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
    NgSelectModule
  ]
})
export class UtilitiesModule { }
