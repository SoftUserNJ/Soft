import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InwardWeighmentComponent } from './inward-weighment.component';

describe('InwardWeighmentComponent', () => {
  let component: InwardWeighmentComponent;
  let fixture: ComponentFixture<InwardWeighmentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [InwardWeighmentComponent]
    });
    fixture = TestBed.createComponent(InwardWeighmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
