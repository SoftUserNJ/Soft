import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewInvoiceOnAppComponent } from './view-invoice-on-app.component';

describe('ViewInvoiceOnAppComponent', () => {
  let component: ViewInvoiceOnAppComponent;
  let fixture: ComponentFixture<ViewInvoiceOnAppComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ViewInvoiceOnAppComponent]
    });
    fixture = TestBed.createComponent(ViewInvoiceOnAppComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
