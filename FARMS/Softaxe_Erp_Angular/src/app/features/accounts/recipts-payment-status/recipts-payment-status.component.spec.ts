import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReciptsPaymentStatusComponent } from './recipts-payment-status.component';

describe('ReciptsPaymentStatusComponent', () => {
  let component: ReciptsPaymentStatusComponent;
  let fixture: ComponentFixture<ReciptsPaymentStatusComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ReciptsPaymentStatusComponent]
    });
    fixture = TestBed.createComponent(ReciptsPaymentStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
