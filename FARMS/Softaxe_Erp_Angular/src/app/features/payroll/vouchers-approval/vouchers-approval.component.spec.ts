import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VouchersApprovalComponent } from './vouchers-approval.component';

describe('VouchersApprovalComponent', () => {
  let component: VouchersApprovalComponent;
  let fixture: ComponentFixture<VouchersApprovalComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [VouchersApprovalComponent]
    });
    fixture = TestBed.createComponent(VouchersApprovalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
