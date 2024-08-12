import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PurchaseReciptsComponent } from './purchase-recipts.component';

describe('PurchaseReciptsComponent', () => {
  let component: PurchaseReciptsComponent;
  let fixture: ComponentFixture<PurchaseReciptsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PurchaseReciptsComponent]
    });
    fixture = TestBed.createComponent(PurchaseReciptsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
