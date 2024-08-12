import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SupplierLeadgerComponent } from './supplier-leadger.component';

describe('SupplierLeadgerComponent', () => {
  let component: SupplierLeadgerComponent;
  let fixture: ComponentFixture<SupplierLeadgerComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SupplierLeadgerComponent]
    });
    fixture = TestBed.createComponent(SupplierLeadgerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
