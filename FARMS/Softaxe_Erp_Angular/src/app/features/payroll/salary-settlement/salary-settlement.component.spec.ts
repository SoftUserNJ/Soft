import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SalarySettlementComponent } from './salary-settlement.component';

describe('SalarySettlementComponent', () => {
  let component: SalarySettlementComponent;
  let fixture: ComponentFixture<SalarySettlementComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SalarySettlementComponent]
    });
    fixture = TestBed.createComponent(SalarySettlementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
