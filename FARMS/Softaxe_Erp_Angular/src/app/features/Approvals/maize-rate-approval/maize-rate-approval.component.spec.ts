import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MaizeRateApprovalComponent } from './maize-rate-approval.component';

describe('MaizeRateApprovalComponent', () => {
  let component: MaizeRateApprovalComponent;
  let fixture: ComponentFixture<MaizeRateApprovalComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MaizeRateApprovalComponent]
    });
    fixture = TestBed.createComponent(MaizeRateApprovalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
