import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StockDebitNoteComponent } from './stock-debit-note.component';

describe('StockDebitNoteComponent', () => {
  let component: StockDebitNoteComponent;
  let fixture: ComponentFixture<StockDebitNoteComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [StockDebitNoteComponent]
    });
    fixture = TestBed.createComponent(StockDebitNoteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
