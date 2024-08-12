import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductSaleIssueComponent } from './product-sale-issue.component';

describe('ProductSaleIssueComponent', () => {
  let component: ProductSaleIssueComponent;
  let fixture: ComponentFixture<ProductSaleIssueComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ProductSaleIssueComponent]
    });
    fixture = TestBed.createComponent(ProductSaleIssueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
