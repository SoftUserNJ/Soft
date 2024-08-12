import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GPOutWardComponent } from './gp-out-ward.component';

describe('GPOutWardComponent', () => {
  let component: GPOutWardComponent;
  let fixture: ComponentFixture<GPOutWardComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GPOutWardComponent]
    });
    fixture = TestBed.createComponent(GPOutWardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
