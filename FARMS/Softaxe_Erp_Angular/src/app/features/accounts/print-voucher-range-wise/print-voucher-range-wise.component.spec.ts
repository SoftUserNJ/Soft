import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PrintVoucherRangeWiseComponent } from './print-voucher-range-wise.component';

describe('PrintVoucherRangeWiseComponent', () => {
  let component: PrintVoucherRangeWiseComponent;
  let fixture: ComponentFixture<PrintVoucherRangeWiseComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PrintVoucherRangeWiseComponent]
    });
    fixture = TestBed.createComponent(PrintVoucherRangeWiseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
