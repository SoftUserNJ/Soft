import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RequisitionsEntryComponent } from './requisitions-entry.component';

describe('RequisitionsEntryComponent', () => {
  let component: RequisitionsEntryComponent;
  let fixture: ComponentFixture<RequisitionsEntryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [RequisitionsEntryComponent]
    });
    fixture = TestBed.createComponent(RequisitionsEntryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
