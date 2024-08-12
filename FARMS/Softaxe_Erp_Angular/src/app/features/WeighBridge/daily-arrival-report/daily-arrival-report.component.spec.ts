import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DailyArrivalReportComponent } from './daily-arrival-report.component';

describe('DailyArrivalReportComponent', () => {
  let component: DailyArrivalReportComponent;
  let fixture: ComponentFixture<DailyArrivalReportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DailyArrivalReportComponent]
    });
    fixture = TestBed.createComponent(DailyArrivalReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
