import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerBioDataComponent } from './customer-bio-data.component';

describe('CustomerBioDataComponent', () => {
  let component: CustomerBioDataComponent;
  let fixture: ComponentFixture<CustomerBioDataComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CustomerBioDataComponent]
    });
    fixture = TestBed.createComponent(CustomerBioDataComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
