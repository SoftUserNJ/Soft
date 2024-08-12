import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InsuranceLoanReportComponent } from './insurance-loan-report.component';

describe('InsuranceLoanReportComponent', () => {
  let component: InsuranceLoanReportComponent;
  let fixture: ComponentFixture<InsuranceLoanReportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [InsuranceLoanReportComponent]
    });
    fixture = TestBed.createComponent(InsuranceLoanReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
