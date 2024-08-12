import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PurchasePrintInvRangeWiseComponent } from './purchase-print-inv-range-wise.component';

describe('PurchasePrintInvRangeWiseComponent', () => {
  let component: PurchasePrintInvRangeWiseComponent;
  let fixture: ComponentFixture<PurchasePrintInvRangeWiseComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PurchasePrintInvRangeWiseComponent]
    });
    fixture = TestBed.createComponent(PurchasePrintInvRangeWiseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
