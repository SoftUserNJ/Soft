import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ArrearsComponent } from './arrears.component';

describe('ArrearsComponent', () => {
  let component: ArrearsComponent;
  let fixture: ComponentFixture<ArrearsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ArrearsComponent]
    });
    fixture = TestBed.createComponent(ArrearsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
