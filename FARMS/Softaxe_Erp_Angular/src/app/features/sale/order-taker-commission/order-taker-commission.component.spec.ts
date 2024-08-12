import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderTakerCommissionComponent } from './order-taker-commission.component';

describe('OrderTakerCommissionComponent', () => {
  let component: OrderTakerCommissionComponent;
  let fixture: ComponentFixture<OrderTakerCommissionComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OrderTakerCommissionComponent]
    });
    fixture = TestBed.createComponent(OrderTakerCommissionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
