import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProvidentFundDeductionComponent } from './provident-fund-deduction.component';

describe('ProvidentFundDeductionComponent', () => {
  let component: ProvidentFundDeductionComponent;
  let fixture: ComponentFixture<ProvidentFundDeductionComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ProvidentFundDeductionComponent]
    });
    fixture = TestBed.createComponent(ProvidentFundDeductionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
