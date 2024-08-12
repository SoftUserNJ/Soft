import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SaleGatePassOutComponent } from './sale-gate-pass-out.component';

describe('SaleGatePassOutComponent', () => {
  let component: SaleGatePassOutComponent;
  let fixture: ComponentFixture<SaleGatePassOutComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SaleGatePassOutComponent]
    });
    fixture = TestBed.createComponent(SaleGatePassOutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
