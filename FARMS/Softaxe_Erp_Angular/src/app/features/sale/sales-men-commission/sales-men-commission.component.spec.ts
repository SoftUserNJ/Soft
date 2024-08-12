import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SalesMenCommissionComponent } from './sales-men-commission.component';

describe('SalesMenCommissionComponent', () => {
  let component: SalesMenCommissionComponent;
  let fixture: ComponentFixture<SalesMenCommissionComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SalesMenCommissionComponent]
    });
    fixture = TestBed.createComponent(SalesMenCommissionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
