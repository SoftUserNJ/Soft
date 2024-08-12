import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DayClosingComponent } from './day-closing.component';

describe('DayClosingComponent', () => {
  let component: DayClosingComponent;
  let fixture: ComponentFixture<DayClosingComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DayClosingComponent]
    });
    fixture = TestBed.createComponent(DayClosingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
