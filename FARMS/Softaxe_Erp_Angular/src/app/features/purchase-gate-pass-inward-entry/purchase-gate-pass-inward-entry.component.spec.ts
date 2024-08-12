import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PurchaseGatePassInwardEntryComponent } from './purchase-gate-pass-inward-entry.component';

describe('PurchaseGatePassInwardEntryComponent', () => {
  let component: PurchaseGatePassInwardEntryComponent;
  let fixture: ComponentFixture<PurchaseGatePassInwardEntryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PurchaseGatePassInwardEntryComponent]
    });
    fixture = TestBed.createComponent(PurchaseGatePassInwardEntryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
