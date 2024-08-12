import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GatePassInwardReportComponent } from './gate-pass-inward-report.component';

describe('GatePassInwardReportComponent', () => {
  let component: GatePassInwardReportComponent;
  let fixture: ComponentFixture<GatePassInwardReportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GatePassInwardReportComponent]
    });
    fixture = TestBed.createComponent(GatePassInwardReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
