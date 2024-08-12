import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChartOfAccountsErpComponent } from './chart-of-accounts-erp.component';

describe('ChartOfAccountsErpComponent', () => {
  let component: ChartOfAccountsErpComponent;
  let fixture: ComponentFixture<ChartOfAccountsErpComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ChartOfAccountsErpComponent]
    });
    fixture = TestBed.createComponent(ChartOfAccountsErpComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
