import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SaleMenJobsComponent } from './sale-men-jobs.component';

describe('SaleMenJobsComponent', () => {
  let component: SaleMenJobsComponent;
  let fixture: ComponentFixture<SaleMenJobsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SaleMenJobsComponent]
    });
    fixture = TestBed.createComponent(SaleMenJobsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
