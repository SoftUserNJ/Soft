import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SaleAreaUpdationComponent } from './sale-area-updation.component';

describe('SaleAreaUpdationComponent', () => {
  let component: SaleAreaUpdationComponent;
  let fixture: ComponentFixture<SaleAreaUpdationComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SaleAreaUpdationComponent]
    });
    fixture = TestBed.createComponent(SaleAreaUpdationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
