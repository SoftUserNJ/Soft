import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AvailServicesComponent } from './avail-services.component';

describe('AvailServicesComponent', () => {
  let component: AvailServicesComponent;
  let fixture: ComponentFixture<AvailServicesComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AvailServicesComponent]
    });
    fixture = TestBed.createComponent(AvailServicesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
