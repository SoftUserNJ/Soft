import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PartyBalanceStatusComponent } from './party-balance-status.component';

describe('PartyBalanceStatusComponent', () => {
  let component: PartyBalanceStatusComponent;
  let fixture: ComponentFixture<PartyBalanceStatusComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PartyBalanceStatusComponent]
    });
    fixture = TestBed.createComponent(PartyBalanceStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
