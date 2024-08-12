import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StockOpeningEntryComponent } from './stock-opening-entry.component';

describe('StockOpeningEntryComponent', () => {
  let component: StockOpeningEntryComponent;
  let fixture: ComponentFixture<StockOpeningEntryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [StockOpeningEntryComponent]
    });
    fixture = TestBed.createComponent(StockOpeningEntryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
