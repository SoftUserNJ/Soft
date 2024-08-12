import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SalesMarginStatusComponent } from './sales-margin-status.component';

describe('SalesMarginStatusComponent', () => {
  let component: SalesMarginStatusComponent;
  let fixture: ComponentFixture<SalesMarginStatusComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SalesMarginStatusComponent]
    });
    fixture = TestBed.createComponent(SalesMarginStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
