import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExpenseOrderComponent } from './expense-order.component';

describe('ExpenseOrderComponent', () => {
  let component: ExpenseOrderComponent;
  let fixture: ComponentFixture<ExpenseOrderComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ExpenseOrderComponent]
    });
    fixture = TestBed.createComponent(ExpenseOrderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
