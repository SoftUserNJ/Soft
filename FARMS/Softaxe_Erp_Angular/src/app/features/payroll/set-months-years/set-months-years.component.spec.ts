import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SetMonthsYearsComponent } from './set-months-years.component';

describe('SetMonthsYearsComponent', () => {
  let component: SetMonthsYearsComponent;
  let fixture: ComponentFixture<SetMonthsYearsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SetMonthsYearsComponent]
    });
    fixture = TestBed.createComponent(SetMonthsYearsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
