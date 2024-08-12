import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DemandEntryComponent } from './demand-entry.component';

describe('DemandEntryComponent', () => {
  let component: DemandEntryComponent;
  let fixture: ComponentFixture<DemandEntryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DemandEntryComponent]
    });
    fixture = TestBed.createComponent(DemandEntryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
