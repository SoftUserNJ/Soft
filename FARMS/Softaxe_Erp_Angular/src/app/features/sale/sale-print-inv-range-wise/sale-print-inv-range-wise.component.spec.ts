import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SalePrintInvRangeWiseComponent } from './sale-print-inv-range-wise.component';

describe('SalePrintInvRangeWiseComponent', () => {
  let component: SalePrintInvRangeWiseComponent;
  let fixture: ComponentFixture<SalePrintInvRangeWiseComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SalePrintInvRangeWiseComponent]
    });
    fixture = TestBed.createComponent(SalePrintInvRangeWiseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
