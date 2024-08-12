import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PurchaseOrderApprovalComponent } from './purchase-order-approval.component';

describe('PurchaseOrderApprovalComponent', () => {
  let component: PurchaseOrderApprovalComponent;
  let fixture: ComponentFixture<PurchaseOrderApprovalComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PurchaseOrderApprovalComponent]
    });
    fixture = TestBed.createComponent(PurchaseOrderApprovalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
