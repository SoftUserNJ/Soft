import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProvLoanBalanceReportComponent } from './prov-loan-balance-report.component';

describe('ProvLoanBalanceReportComponent', () => {
  let component: ProvLoanBalanceReportComponent;
  let fixture: ComponentFixture<ProvLoanBalanceReportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ProvLoanBalanceReportComponent]
    });
    fixture = TestBed.createComponent(ProvLoanBalanceReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
