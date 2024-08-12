import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerReceivableAgingComponent } from './customer-receivable-aging.component';

describe('CustomerReceivableAgingComponent', () => {
  let component: CustomerReceivableAgingComponent;
  let fixture: ComponentFixture<CustomerReceivableAgingComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CustomerReceivableAgingComponent]
    });
    fixture = TestBed.createComponent(CustomerReceivableAgingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
