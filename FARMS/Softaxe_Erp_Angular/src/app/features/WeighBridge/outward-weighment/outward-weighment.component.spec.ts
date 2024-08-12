import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OutwardWeighmentComponent } from './outward-weighment.component';

describe('OutwardWeighmentComponent', () => {
  let component: OutwardWeighmentComponent;
  let fixture: ComponentFixture<OutwardWeighmentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OutwardWeighmentComponent]
    });
    fixture = TestBed.createComponent(OutwardWeighmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
