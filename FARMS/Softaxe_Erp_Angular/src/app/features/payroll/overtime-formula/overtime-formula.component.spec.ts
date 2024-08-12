import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OvertimeFormulaComponent } from './overtime-formula.component';

describe('OvertimeFormulaComponent', () => {
  let component: OvertimeFormulaComponent;
  let fixture: ComponentFixture<OvertimeFormulaComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OvertimeFormulaComponent]
    });
    fixture = TestBed.createComponent(OvertimeFormulaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
