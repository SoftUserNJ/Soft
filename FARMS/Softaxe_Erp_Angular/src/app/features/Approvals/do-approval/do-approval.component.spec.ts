import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DOApprovalComponent } from './do-approval.component';

describe('DOApprovalComponent', () => {
  let component: DOApprovalComponent;
  let fixture: ComponentFixture<DOApprovalComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DOApprovalComponent]
    });
    fixture = TestBed.createComponent(DOApprovalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
