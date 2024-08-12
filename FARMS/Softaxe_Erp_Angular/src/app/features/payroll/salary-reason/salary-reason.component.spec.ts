import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SalaryReasonComponent } from './salary-reason.component';

describe('SalaryReasonComponent', () => {
  let component: SalaryReasonComponent;
  let fixture: ComponentFixture<SalaryReasonComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SalaryReasonComponent]
    });
    fixture = TestBed.createComponent(SalaryReasonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
