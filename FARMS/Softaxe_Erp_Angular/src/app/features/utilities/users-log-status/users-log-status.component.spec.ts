import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UsersLogStatusComponent } from './users-log-status.component';

describe('UsersLogStatusComponent', () => {
  let component: UsersLogStatusComponent;
  let fixture: ComponentFixture<UsersLogStatusComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [UsersLogStatusComponent]
    });
    fixture = TestBed.createComponent(UsersLogStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
