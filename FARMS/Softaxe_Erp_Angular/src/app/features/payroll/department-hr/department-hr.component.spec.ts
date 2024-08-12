import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DepartmentHRComponent } from './department-hr.component';

describe('DepartmentHRComponent', () => {
  let component: DepartmentHRComponent;
  let fixture: ComponentFixture<DepartmentHRComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DepartmentHRComponent]
    });
    fixture = TestBed.createComponent(DepartmentHRComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
