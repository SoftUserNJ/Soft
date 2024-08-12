import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductsErpComponent } from './products-erp.component';

describe('ProductsErpComponent', () => {
  let component: ProductsErpComponent;
  let fixture: ComponentFixture<ProductsErpComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ProductsErpComponent]
    });
    fixture = TestBed.createComponent(ProductsErpComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
