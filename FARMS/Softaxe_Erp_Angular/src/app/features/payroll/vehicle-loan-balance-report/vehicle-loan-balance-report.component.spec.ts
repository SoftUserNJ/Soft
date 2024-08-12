import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VehicleLoanBalanceReportComponent } from './vehicle-loan-balance-report.component';

describe('VehicleLoanBalanceReportComponent', () => {
  let component: VehicleLoanBalanceReportComponent;
  let fixture: ComponentFixture<VehicleLoanBalanceReportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [VehicleLoanBalanceReportComponent]
    });
    fixture = TestBed.createComponent(VehicleLoanBalanceReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
