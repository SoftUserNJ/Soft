import { ComponentFixture, TestBed } from '@angular/core/testing';

import { JournalVoucherComponent } from './journal-voucher.component';

describe('JournalVoucherComponent', () => {
  let component: JournalVoucherComponent;
  let fixture: ComponentFixture<JournalVoucherComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [JournalVoucherComponent]
    });
    fixture = TestBed.createComponent(JournalVoucherComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
