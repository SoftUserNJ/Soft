import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PurchaseContractFormComponent } from './purchase-contract-form.component';

describe('PurchaseContractFormComponent', () => {
  let component: PurchaseContractFormComponent;
  let fixture: ComponentFixture<PurchaseContractFormComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PurchaseContractFormComponent]
    });
    fixture = TestBed.createComponent(PurchaseContractFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
