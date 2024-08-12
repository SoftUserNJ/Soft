import { ComponentFixture, TestBed } from '@angular/core/testing';

import { JobWiseDueStatusComponent } from './job-wise-due-status.component';

describe('JobWiseDueStatusComponent', () => {
  let component: JobWiseDueStatusComponent;
  let fixture: ComponentFixture<JobWiseDueStatusComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [JobWiseDueStatusComponent]
    });
    fixture = TestBed.createComponent(JobWiseDueStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
