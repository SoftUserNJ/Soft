import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtherDeductionComponent } from './other-deduction.component';

describe('OtherDeductionComponent', () => {
  let component: OtherDeductionComponent;
  let fixture: ComponentFixture<OtherDeductionComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OtherDeductionComponent]
    });
    fixture = TestBed.createComponent(OtherDeductionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
