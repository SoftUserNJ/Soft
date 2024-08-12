import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MobileAppSliderComponent } from './mobile-app-slider.component';

describe('MobileAppSliderComponent', () => {
  let component: MobileAppSliderComponent;
  let fixture: ComponentFixture<MobileAppSliderComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MobileAppSliderComponent]
    });
    fixture = TestBed.createComponent(MobileAppSliderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
