import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DailyFreightReportComponent } from './daily-freight-report.component';

describe('DailyFreightReportComponent', () => {
  let component: DailyFreightReportComponent;
  let fixture: ComponentFixture<DailyFreightReportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DailyFreightReportComponent]
    });
    fixture = TestBed.createComponent(DailyFreightReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
