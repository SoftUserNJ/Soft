import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PartiesTaxDeductionComponent } from './parties-tax-deduction.component';

describe('PartiesTaxDeductionComponent', () => {
  let component: PartiesTaxDeductionComponent;
  let fixture: ComponentFixture<PartiesTaxDeductionComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PartiesTaxDeductionComponent]
    });
    fixture = TestBed.createComponent(PartiesTaxDeductionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
