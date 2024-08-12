import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductWiseSaleReportComponent } from './product-wise-sale-report.component';

describe('ProductWiseSaleReportComponent', () => {
  let component: ProductWiseSaleReportComponent;
  let fixture: ComponentFixture<ProductWiseSaleReportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ProductWiseSaleReportComponent]
    });
    fixture = TestBed.createComponent(ProductWiseSaleReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
