import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StockCreditNoteComponent } from './stock-credit-note.component';

describe('StockCreditNoteComponent', () => {
  let component: StockCreditNoteComponent;
  let fixture: ComponentFixture<StockCreditNoteComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [StockCreditNoteComponent]
    });
    fixture = TestBed.createComponent(StockCreditNoteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
