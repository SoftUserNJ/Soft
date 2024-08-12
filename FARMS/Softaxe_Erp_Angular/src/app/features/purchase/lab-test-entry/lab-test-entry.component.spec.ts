import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LabTestEntryComponent } from './lab-test-entry.component';

describe('LabTestEntryComponent', () => {
  let component: LabTestEntryComponent;
  let fixture: ComponentFixture<LabTestEntryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LabTestEntryComponent]
    });
    fixture = TestBed.createComponent(LabTestEntryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
