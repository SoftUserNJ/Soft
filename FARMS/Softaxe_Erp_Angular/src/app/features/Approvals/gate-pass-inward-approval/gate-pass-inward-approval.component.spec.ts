import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GatePassInwardApprovalComponent } from './gate-pass-inward-approval.component';

describe('GatePassInwardApprovalComponent', () => {
  let component: GatePassInwardApprovalComponent;
  let fixture: ComponentFixture<GatePassInwardApprovalComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GatePassInwardApprovalComponent]
    });
    fixture = TestBed.createComponent(GatePassInwardApprovalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
