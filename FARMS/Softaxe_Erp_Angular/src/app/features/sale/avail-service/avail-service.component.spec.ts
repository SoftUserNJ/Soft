import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AvailServiceComponent } from './avail-service.component';

describe('AvailServiceComponent', () => {
  let component: AvailServiceComponent;
  let fixture: ComponentFixture<AvailServiceComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AvailServiceComponent]
    });
    fixture = TestBed.createComponent(AvailServiceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
