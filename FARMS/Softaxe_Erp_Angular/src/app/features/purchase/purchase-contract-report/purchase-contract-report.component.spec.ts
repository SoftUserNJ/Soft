import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PurchaseContractReportComponent } from './purchase-contract-report.component';

describe('PurchaseContractReportComponent', () => {
  let component: PurchaseContractReportComponent;
  let fixture: ComponentFixture<PurchaseContractReportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PurchaseContractReportComponent]
    });
    fixture = TestBed.createComponent(PurchaseContractReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
