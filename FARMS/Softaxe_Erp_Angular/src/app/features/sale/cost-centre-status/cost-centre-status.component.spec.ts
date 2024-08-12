import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CostCentreStatusComponent } from './cost-centre-status.component';

describe('CostCentreStatusComponent', () => {
  let component: CostCentreStatusComponent;
  let fixture: ComponentFixture<CostCentreStatusComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CostCentreStatusComponent]
    });
    fixture = TestBed.createComponent(CostCentreStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
