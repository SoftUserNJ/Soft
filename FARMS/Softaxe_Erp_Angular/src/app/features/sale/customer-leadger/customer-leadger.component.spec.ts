import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerLeadgerComponent } from './customer-leadger.component';

describe('CustomerLeadgerComponent', () => {
  let component: CustomerLeadgerComponent;
  let fixture: ComponentFixture<CustomerLeadgerComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CustomerLeadgerComponent]
    });
    fixture = TestBed.createComponent(CustomerLeadgerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
