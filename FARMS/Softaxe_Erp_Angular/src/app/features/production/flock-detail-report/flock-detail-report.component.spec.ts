import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FlockDetailReportComponent } from './flock-detail-report.component';

describe('FlockDetailReportComponent', () => {
  let component: FlockDetailReportComponent;
  let fixture: ComponentFixture<FlockDetailReportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [FlockDetailReportComponent]
    });
    fixture = TestBed.createComponent(FlockDetailReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
