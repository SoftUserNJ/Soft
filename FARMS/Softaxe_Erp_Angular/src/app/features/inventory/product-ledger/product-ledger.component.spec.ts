import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductLedgerComponent } from './product-ledger.component';

describe('ProductLedgerComponent', () => {
  let component: ProductLedgerComponent;
  let fixture: ComponentFixture<ProductLedgerComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ProductLedgerComponent]
    });
    fixture = TestBed.createComponent(ProductLedgerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
