import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LeavesEntryComponent } from './leaves-entry.component';

describe('LeavesEntryComponent', () => {
  let component: LeavesEntryComponent;
  let fixture: ComponentFixture<LeavesEntryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LeavesEntryComponent]
    });
    fixture = TestBed.createComponent(LeavesEntryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
