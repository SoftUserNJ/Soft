import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtLiveActivityComponent } from './ot-live-activity.component';

describe('OtLiveActivityComponent', () => {
  let component: OtLiveActivityComponent;
  let fixture: ComponentFixture<OtLiveActivityComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OtLiveActivityComponent]
    });
    fixture = TestBed.createComponent(OtLiveActivityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
