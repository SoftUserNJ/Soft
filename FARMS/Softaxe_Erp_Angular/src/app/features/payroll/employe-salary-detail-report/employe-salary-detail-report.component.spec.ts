import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeSalaryDetailReportComponent } from './employe-salary-detail-report.component';

describe('EmployeSalaryDetailReportComponent', () => {
  let component: EmployeSalaryDetailReportComponent;
  let fixture: ComponentFixture<EmployeSalaryDetailReportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [EmployeSalaryDetailReportComponent]
    });
    fixture = TestBed.createComponent(EmployeSalaryDetailReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
