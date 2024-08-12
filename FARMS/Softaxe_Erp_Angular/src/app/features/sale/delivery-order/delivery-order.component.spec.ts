import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeliveryOrderComponent } from './delivery-order.component';

describe('DeliveryOrderComponent', () => {
  let component: DeliveryOrderComponent;
  let fixture: ComponentFixture<DeliveryOrderComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DeliveryOrderComponent]
    });
    fixture = TestBed.createComponent(DeliveryOrderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
