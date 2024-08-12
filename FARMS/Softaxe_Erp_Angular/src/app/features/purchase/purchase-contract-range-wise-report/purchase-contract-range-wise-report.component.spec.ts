import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PurchaseContractRangeWiseReportComponent } from './purchase-contract-range-wise-report.component';

describe('PurchaseContractRangeWiseReportComponent', () => {
  let component: PurchaseContractRangeWiseReportComponent;
  let fixture: ComponentFixture<PurchaseContractRangeWiseReportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PurchaseContractRangeWiseReportComponent]
    });
    fixture = TestBed.createComponent(PurchaseContractRangeWiseReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
