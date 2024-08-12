import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SupplierBioDataComponent } from './supplier-bio-data.component';

describe('SupplierBioDataComponent', () => {
  let component: SupplierBioDataComponent;
  let fixture: ComponentFixture<SupplierBioDataComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SupplierBioDataComponent]
    });
    fixture = TestBed.createComponent(SupplierBioDataComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
