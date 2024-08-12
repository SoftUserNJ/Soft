import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProvidentLoanReportComponent } from './provident-loan-report.component';

describe('ProvidentLoanReportComponent', () => {
  let component: ProvidentLoanReportComponent;
  let fixture: ComponentFixture<ProvidentLoanReportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ProvidentLoanReportComponent]
    });
    fixture = TestBed.createComponent(ProvidentLoanReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
