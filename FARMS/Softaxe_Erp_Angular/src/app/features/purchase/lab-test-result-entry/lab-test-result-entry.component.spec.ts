import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LabTestResultEntryComponent } from './lab-test-result-entry.component';

describe('LabTestResultEntryComponent', () => {
  let component: LabTestResultEntryComponent;
  let fixture: ComponentFixture<LabTestResultEntryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LabTestResultEntryComponent]
    });
    fixture = TestBed.createComponent(LabTestResultEntryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
