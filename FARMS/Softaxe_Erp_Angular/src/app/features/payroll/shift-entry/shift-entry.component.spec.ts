import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShiftEntryComponent } from './shift-entry.component';

describe('ShiftEntryComponent', () => {
  let component: ShiftEntryComponent;
  let fixture: ComponentFixture<ShiftEntryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ShiftEntryComponent]
    });
    fixture = TestBed.createComponent(ShiftEntryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
