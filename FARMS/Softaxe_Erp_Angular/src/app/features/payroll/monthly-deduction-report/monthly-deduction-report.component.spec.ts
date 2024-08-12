import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MonthlyDeductionReportComponent } from './monthly-deduction-report.component';

describe('MonthlyDeductionReportComponent', () => {
  let component: MonthlyDeductionReportComponent;
  let fixture: ComponentFixture<MonthlyDeductionReportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MonthlyDeductionReportComponent]
    });
    fixture = TestBed.createComponent(MonthlyDeductionReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
