import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DailySaleActivityReportComponent } from './daily-sale-activity-report.component';

describe('DailySaleActivityReportComponent', () => {
  let component: DailySaleActivityReportComponent;
  let fixture: ComponentFixture<DailySaleActivityReportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DailySaleActivityReportComponent]
    });
    fixture = TestBed.createComponent(DailySaleActivityReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
