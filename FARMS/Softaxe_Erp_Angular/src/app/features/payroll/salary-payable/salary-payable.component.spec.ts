import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SalaryPayableComponent } from './salary-payable.component';

describe('SalaryPayableComponent', () => {
  let component: SalaryPayableComponent;
  let fixture: ComponentFixture<SalaryPayableComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SalaryPayableComponent]
    });
    fixture = TestBed.createComponent(SalaryPayableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
