import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SalarySettlementLabelsComponent } from './salary-settlement-labels.component';

describe('SalarySettlementLabelsComponent', () => {
  let component: SalarySettlementLabelsComponent;
  let fixture: ComponentFixture<SalarySettlementLabelsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SalarySettlementLabelsComponent]
    });
    fixture = TestBed.createComponent(SalarySettlementLabelsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
