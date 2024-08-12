import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdvancedSalaryReportComponent } from './advanced-salary-report.component';

describe('AdvancedSalaryReportComponent', () => {
  let component: AdvancedSalaryReportComponent;
  let fixture: ComponentFixture<AdvancedSalaryReportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AdvancedSalaryReportComponent]
    });
    fixture = TestBed.createComponent(AdvancedSalaryReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
