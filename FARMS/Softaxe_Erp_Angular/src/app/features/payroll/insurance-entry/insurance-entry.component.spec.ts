import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InsuranceEntryComponent } from './insurance-entry.component';

describe('InsuranceEntryComponent', () => {
  let component: InsuranceEntryComponent;
  let fixture: ComponentFixture<InsuranceEntryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [InsuranceEntryComponent]
    });
    fixture = TestBed.createComponent(InsuranceEntryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
