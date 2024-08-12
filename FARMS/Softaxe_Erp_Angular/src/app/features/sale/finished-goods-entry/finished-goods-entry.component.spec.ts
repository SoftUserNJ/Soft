import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FinishedGoodsEntryComponent } from './finished-goods-entry.component';

describe('FinishedGoodsEntryComponent', () => {
  let component: FinishedGoodsEntryComponent;
  let fixture: ComponentFixture<FinishedGoodsEntryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [FinishedGoodsEntryComponent]
    });
    fixture = TestBed.createComponent(FinishedGoodsEntryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
