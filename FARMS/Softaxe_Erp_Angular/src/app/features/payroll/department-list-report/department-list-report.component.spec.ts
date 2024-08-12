import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DepartmentListReportComponent } from './department-list-report.component';

describe('DepartmentListReportComponent', () => {
  let component: DepartmentListReportComponent;
  let fixture: ComponentFixture<DepartmentListReportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DepartmentListReportComponent]
    });
    fixture = TestBed.createComponent(DepartmentListReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
