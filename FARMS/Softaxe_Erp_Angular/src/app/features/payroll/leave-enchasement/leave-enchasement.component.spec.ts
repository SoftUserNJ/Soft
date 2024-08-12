import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LeaveEnchasementComponent } from './leave-enchasement.component';

describe('LeaveEnchasementComponent', () => {
  let component: LeaveEnchasementComponent;
  let fixture: ComponentFixture<LeaveEnchasementComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LeaveEnchasementComponent]
    });
    fixture = TestBed.createComponent(LeaveEnchasementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
