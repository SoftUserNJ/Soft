import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SalaryDaysComponent } from './salary-days.component';

describe('SalaryDaysComponent', () => {
  let component: SalaryDaysComponent;
  let fixture: ComponentFixture<SalaryDaysComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SalaryDaysComponent]
    });
    fixture = TestBed.createComponent(SalaryDaysComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
