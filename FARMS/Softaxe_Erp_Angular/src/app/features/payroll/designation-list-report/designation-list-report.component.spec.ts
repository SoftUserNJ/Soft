import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DesignationListReportComponent } from './designation-list-report.component';

describe('DesignationListReportComponent', () => {
  let component: DesignationListReportComponent;
  let fixture: ComponentFixture<DesignationListReportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DesignationListReportComponent]
    });
    fixture = TestBed.createComponent(DesignationListReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
