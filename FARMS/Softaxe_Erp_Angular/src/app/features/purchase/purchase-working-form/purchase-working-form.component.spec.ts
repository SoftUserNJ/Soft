import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PurchaseWorkingFormComponent } from './purchase-working-form.component';

describe('PurchaseWorkingFormComponent', () => {
  let component: PurchaseWorkingFormComponent;
  let fixture: ComponentFixture<PurchaseWorkingFormComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PurchaseWorkingFormComponent]
    });
    fixture = TestBed.createComponent(PurchaseWorkingFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
