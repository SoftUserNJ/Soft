import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IPEDeductionReportComponent } from './ipe-deduction-report.component';

describe('IPEDeductionReportComponent', () => {
  let component: IPEDeductionReportComponent;
  let fixture: ComponentFixture<IPEDeductionReportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [IPEDeductionReportComponent]
    });
    fixture = TestBed.createComponent(IPEDeductionReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
