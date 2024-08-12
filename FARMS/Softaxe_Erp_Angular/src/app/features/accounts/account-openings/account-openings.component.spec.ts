import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountOpeningsComponent } from './account-openings.component';

describe('AccountOpeningsComponent', () => {
  let component: AccountOpeningsComponent;
  let fixture: ComponentFixture<AccountOpeningsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccountOpeningsComponent]
    });
    fixture = TestBed.createComponent(AccountOpeningsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
