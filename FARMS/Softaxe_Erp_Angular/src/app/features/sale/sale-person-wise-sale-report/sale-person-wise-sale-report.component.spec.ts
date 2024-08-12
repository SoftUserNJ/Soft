import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SalePersonWiseSaleReportComponent } from './sale-person-wise-sale-report.component';

describe('SalePersonWiseSaleReportComponent', () => {
  let component: SalePersonWiseSaleReportComponent;
  let fixture: ComponentFixture<SalePersonWiseSaleReportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SalePersonWiseSaleReportComponent]
    });
    fixture = TestBed.createComponent(SalePersonWiseSaleReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
