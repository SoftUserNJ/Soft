import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProfitAndLossComparisonComponent } from './profit-and-loss-comparison.component';

describe('ProfitAndLossComparisonComponent', () => {
  let component: ProfitAndLossComparisonComponent;
  let fixture: ComponentFixture<ProfitAndLossComparisonComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ProfitAndLossComparisonComponent]
    });
    fixture = TestBed.createComponent(ProfitAndLossComparisonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
