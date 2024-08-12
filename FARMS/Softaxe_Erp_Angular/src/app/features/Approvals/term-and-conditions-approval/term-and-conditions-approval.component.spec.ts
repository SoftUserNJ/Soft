import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TermAndConditionsApprovalComponent } from './term-and-conditions-approval.component';

describe('TermAndConditionsApprovalComponent', () => {
  let component: TermAndConditionsApprovalComponent;
  let fixture: ComponentFixture<TermAndConditionsApprovalComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TermAndConditionsApprovalComponent]
    });
    fixture = TestBed.createComponent(TermAndConditionsApprovalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
