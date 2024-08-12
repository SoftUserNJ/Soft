import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MaizeRateComponent } from './maize-rate.component';

describe('MaizeRateComponent', () => {
  let component: MaizeRateComponent;
  let fixture: ComponentFixture<MaizeRateComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MaizeRateComponent]
    });
    fixture = TestBed.createComponent(MaizeRateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
