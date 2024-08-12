import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PurchasePayableAgingComponent } from './purchase-payable-aging.component';

describe('PurchasePayableAgingComponent', () => {
  let component: PurchasePayableAgingComponent;
  let fixture: ComponentFixture<PurchasePayableAgingComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PurchasePayableAgingComponent]
    });
    fixture = TestBed.createComponent(PurchasePayableAgingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
