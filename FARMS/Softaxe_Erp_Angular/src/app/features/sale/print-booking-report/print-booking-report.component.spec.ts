import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PrintBookingReportComponent } from './print-booking-report.component';

describe('PrintBookingReportComponent', () => {
  let component: PrintBookingReportComponent;
  let fixture: ComponentFixture<PrintBookingReportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PrintBookingReportComponent]
    });
    fixture = TestBed.createComponent(PrintBookingReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
