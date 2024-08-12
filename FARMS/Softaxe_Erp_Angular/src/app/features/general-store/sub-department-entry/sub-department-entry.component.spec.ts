import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubDepartmentEntryComponent } from './sub-department-entry.component';

describe('SubDepartmentEntryComponent', () => {
  let component: SubDepartmentEntryComponent;
  let fixture: ComponentFixture<SubDepartmentEntryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SubDepartmentEntryComponent]
    });
    fixture = TestBed.createComponent(SubDepartmentEntryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
