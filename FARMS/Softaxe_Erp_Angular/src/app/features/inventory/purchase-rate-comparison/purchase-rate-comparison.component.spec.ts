import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PurchaseRateComparisonComponent } from './purchase-rate-comparison.component';

describe('PurchaseRateComparisonComponent', () => {
  let component: PurchaseRateComparisonComponent;
  let fixture: ComponentFixture<PurchaseRateComparisonComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PurchaseRateComparisonComponent]
    });
    fixture = TestBed.createComponent(PurchaseRateComparisonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
