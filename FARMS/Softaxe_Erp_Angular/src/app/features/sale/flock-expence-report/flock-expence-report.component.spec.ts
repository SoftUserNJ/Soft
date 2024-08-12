import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FlockExpenceReportComponent } from './flock-expence-report.component';

describe('FlockExpenceReportComponent', () => {
  let component: FlockExpenceReportComponent;
  let fixture: ComponentFixture<FlockExpenceReportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [FlockExpenceReportComponent]
    });
    fixture = TestBed.createComponent(FlockExpenceReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
