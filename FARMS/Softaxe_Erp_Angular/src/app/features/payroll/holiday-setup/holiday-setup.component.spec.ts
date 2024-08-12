import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HolidaySetupComponent } from './holiday-setup.component';

describe('HolidaySetupComponent', () => {
  let component: HolidaySetupComponent;
  let fixture: ComponentFixture<HolidaySetupComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HolidaySetupComponent]
    });
    fixture = TestBed.createComponent(HolidaySetupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
