import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InwardPurchaseStatusComponent } from './inward-purchase-status.component';

describe('InwardPurchaseStatusComponent', () => {
  let component: InwardPurchaseStatusComponent;
  let fixture: ComponentFixture<InwardPurchaseStatusComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [InwardPurchaseStatusComponent]
    });
    fixture = TestBed.createComponent(InwardPurchaseStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
