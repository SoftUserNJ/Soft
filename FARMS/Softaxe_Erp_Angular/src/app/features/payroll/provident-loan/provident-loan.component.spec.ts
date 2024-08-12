import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProvidentLoanComponent } from './provident-loan.component';

describe('ProvidentLoanComponent', () => {
  let component: ProvidentLoanComponent;
  let fixture: ComponentFixture<ProvidentLoanComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ProvidentLoanComponent]
    });
    fixture = TestBed.createComponent(ProvidentLoanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
