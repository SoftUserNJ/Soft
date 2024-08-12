import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductRateComparisonComponent } from './product-rate-comparison.component';

describe('ProductRateComparisonComponent', () => {
  let component: ProductRateComparisonComponent;
  let fixture: ComponentFixture<ProductRateComparisonComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ProductRateComparisonComponent]
    });
    fixture = TestBed.createComponent(ProductRateComparisonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
