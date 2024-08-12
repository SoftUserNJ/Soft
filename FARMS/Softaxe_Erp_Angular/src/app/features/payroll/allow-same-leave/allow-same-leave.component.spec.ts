import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllowSameLeaveComponent } from './allow-same-leave.component';

describe('AllowSameLeaveComponent', () => {
  let component: AllowSameLeaveComponent;
  let fixture: ComponentFixture<AllowSameLeaveComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AllowSameLeaveComponent]
    });
    fixture = TestBed.createComponent(AllowSameLeaveComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
