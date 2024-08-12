import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BankEntryComponent } from './bank-entry.component';

describe('BankEntryComponent', () => {
  let component: BankEntryComponent;
  let fixture: ComponentFixture<BankEntryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [BankEntryComponent]
    });
    fixture = TestBed.createComponent(BankEntryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
