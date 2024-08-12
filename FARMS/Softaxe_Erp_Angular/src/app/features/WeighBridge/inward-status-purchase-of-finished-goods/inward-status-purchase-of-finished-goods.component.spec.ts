import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InwardStatusPurchaseOfFinishedGoodsComponent } from './inward-status-purchase-of-finished-goods.component';

describe('InwardStatusPurchaseOfFinishedGoodsComponent', () => {
  let component: InwardStatusPurchaseOfFinishedGoodsComponent;
  let fixture: ComponentFixture<InwardStatusPurchaseOfFinishedGoodsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [InwardStatusPurchaseOfFinishedGoodsComponent]
    });
    fixture = TestBed.createComponent(InwardStatusPurchaseOfFinishedGoodsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
