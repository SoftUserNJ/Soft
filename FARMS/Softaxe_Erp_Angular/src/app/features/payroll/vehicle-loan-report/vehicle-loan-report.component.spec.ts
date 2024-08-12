import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VehicleLoanReportComponent } from './vehicle-loan-report.component';

describe('VehicleLoanReportComponent', () => {
  let component: VehicleLoanReportComponent;
  let fixture: ComponentFixture<VehicleLoanReportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [VehicleLoanReportComponent]
    });
    fixture = TestBed.createComponent(VehicleLoanReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
