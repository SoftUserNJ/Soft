import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DesignationEntryComponent } from './designation-entry.component';

describe('DesignationEntryComponent', () => {
  let component: DesignationEntryComponent;
  let fixture: ComponentFixture<DesignationEntryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DesignationEntryComponent]
    });
    fixture = TestBed.createComponent(DesignationEntryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
