import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { SafeUrlPipe } from 'src/app/features/safe-url.pipe';
import { FileAttachmentComponent } from './common/file-attachment/file-attachment.component';
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


@NgModule({
  declarations: [
    SafeUrlPipe,
    FileAttachmentComponent,
    
    
  ],
  exports:[
    SafeUrlPipe, 
    FileAttachmentComponent
  ],
  imports: [
    CommonModule,
    NgSelectModule,
    DatePipe,
    RouterModule,
    FormsModule,
    BrowserModule,
    MatDatepickerModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    CalendarModule,
  ]
})
export class SharedModule { }
