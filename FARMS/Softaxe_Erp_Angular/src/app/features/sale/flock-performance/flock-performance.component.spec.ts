import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FlockPerformanceComponent } from './flock-performance.component';

describe('FlockPerformanceComponent', () => {
  let component: FlockPerformanceComponent;
  let fixture: ComponentFixture<FlockPerformanceComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [FlockPerformanceComponent]
    });
    fixture = TestBed.createComponent(FlockPerformanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
