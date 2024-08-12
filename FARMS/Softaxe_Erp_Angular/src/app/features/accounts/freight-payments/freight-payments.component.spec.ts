import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FreightPaymentsComponent } from './freight-payments.component';

describe('FreightPaymentsComponent', () => {
  let component: FreightPaymentsComponent;
  let fixture: ComponentFixture<FreightPaymentsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [FreightPaymentsComponent]
    });
    fixture = TestBed.createComponent(FreightPaymentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
