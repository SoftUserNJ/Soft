import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PurchaseCorrectionFormComponent } from './purchase-correction-form.component';

describe('PurchaseCorrectionFormComponent', () => {
  let component: PurchaseCorrectionFormComponent;
  let fixture: ComponentFixture<PurchaseCorrectionFormComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PurchaseCorrectionFormComponent]
    });
    fixture = TestBed.createComponent(PurchaseCorrectionFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
