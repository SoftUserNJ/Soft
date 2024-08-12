import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SaleBookingEntryComponent } from './sale-booking-entry.component';

describe('SaleBookingEntryComponent', () => {
  let component: SaleBookingEntryComponent;
  let fixture: ComponentFixture<SaleBookingEntryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SaleBookingEntryComponent]
    });
    fixture = TestBed.createComponent(SaleBookingEntryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
