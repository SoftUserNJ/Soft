import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StaffLoanBalanceReportComponent } from './staff-loan-balance-report.component';

describe('StaffLoanBalanceReportComponent', () => {
  let component: StaffLoanBalanceReportComponent;
  let fixture: ComponentFixture<StaffLoanBalanceReportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [StaffLoanBalanceReportComponent]
    });
    fixture = TestBed.createComponent(StaffLoanBalanceReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
