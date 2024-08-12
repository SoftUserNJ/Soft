import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MaterialReceivingReportComponent } from './material-receiving-report.component';

describe('MaterialReceivingReportComponent', () => {
  let component: MaterialReceivingReportComponent;
  let fixture: ComponentFixture<MaterialReceivingReportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MaterialReceivingReportComponent]
    });
    fixture = TestBed.createComponent(MaterialReceivingReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
