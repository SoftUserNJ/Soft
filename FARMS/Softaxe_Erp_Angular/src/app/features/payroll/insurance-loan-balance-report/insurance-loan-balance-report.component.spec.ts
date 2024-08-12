import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InsuranceLoanBalanceReportComponent } from './insurance-loan-balance-report.component';

describe('InsuranceLoanBalanceReportComponent', () => {
  let component: InsuranceLoanBalanceReportComponent;
  let fixture: ComponentFixture<InsuranceLoanBalanceReportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [InsuranceLoanBalanceReportComponent]
    });
    fixture = TestBed.createComponent(InsuranceLoanBalanceReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
