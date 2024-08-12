import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PartyTermsAndConditionsComponent } from './party-terms-and-conditions.component';

describe('PartyTermsAndConditionsComponent', () => {
  let component: PartyTermsAndConditionsComponent;
  let fixture: ComponentFixture<PartyTermsAndConditionsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PartyTermsAndConditionsComponent]
    });
    fixture = TestBed.createComponent(PartyTermsAndConditionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
