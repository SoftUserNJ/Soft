import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DispatchOrderSalComponent } from './dispatch-order-sal.component';

describe('DispatchOrderSalComponent', () => {
  let component: DispatchOrderSalComponent;
  let fixture: ComponentFixture<DispatchOrderSalComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DispatchOrderSalComponent]
    });
    fixture = TestBed.createComponent(DispatchOrderSalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
