import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LoanLedgerComponent } from './loan-ledger.component';

describe('LoanLedgerComponent', () => {
  let component: LoanLedgerComponent;
  let fixture: ComponentFixture<LoanLedgerComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LoanLedgerComponent]
    });
    fixture = TestBed.createComponent(LoanLedgerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
