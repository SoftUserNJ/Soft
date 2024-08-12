import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductRateUpdateComponent } from './product-rate-update.component';

describe('ProductRateUpdateComponent', () => {
  let component: ProductRateUpdateComponent;
  let fixture: ComponentFixture<ProductRateUpdateComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ProductRateUpdateComponent]
    });
    fixture = TestBed.createComponent(ProductRateUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
