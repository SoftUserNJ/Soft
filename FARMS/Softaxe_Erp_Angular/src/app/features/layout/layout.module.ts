import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SidebarComponent } from './sidebar/sidebar.component';
import { HeaderComponent } from './header/header.component';
import { FooterComponent } from './footer/footer.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { RouterModule } from '@angular/router';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatSelectModule } from '@angular/material/select';
import { NgSelectModule } from '@ng-select/ng-select';
import { FormsModule } from '@angular/forms';
import { SharedModule } from 'src/app/shared.module';

@NgModule({
  declarations: [
    SidebarComponent,
    HeaderComponent,
    FooterComponent,
    DashboardComponent,
  ],

  imports: [
    CommonModule,
    RouterModule,
    MatDatepickerModule,
    FormsModule,
    MatSelectModule,
    NgSelectModule,
    SharedModule
  ],
  exports: [
    SidebarComponent,
    HeaderComponent,
    FooterComponent,
    DashboardComponent
  ],
})
export class LayoutModule { }
