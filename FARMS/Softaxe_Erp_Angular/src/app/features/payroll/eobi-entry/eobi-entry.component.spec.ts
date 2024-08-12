import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EobiEntryComponent } from './eobi-entry.component';

describe('EobiEntryComponent', () => {
  let component: EobiEntryComponent;
  let fixture: ComponentFixture<EobiEntryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [EobiEntryComponent]
    });
    fixture = TestBed.createComponent(EobiEntryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
