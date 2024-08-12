import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BillDueStatusComponent } from './bill-due-status.component';

describe('BillDueStatusComponent', () => {
  let component: BillDueStatusComponent;
  let fixture: ComponentFixture<BillDueStatusComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [BillDueStatusComponent]
    });
    fixture = TestBed.createComponent(BillDueStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
