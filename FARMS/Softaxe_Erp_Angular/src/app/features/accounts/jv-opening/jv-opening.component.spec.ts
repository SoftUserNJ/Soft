import { ComponentFixture, TestBed } from '@angular/core/testing';

import { JvOpeningComponent } from './jv-opening.component';

describe('JvOpeningComponent', () => {
  let component: JvOpeningComponent;
  let fixture: ComponentFixture<JvOpeningComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [JvOpeningComponent]
    });
    fixture = TestBed.createComponent(JvOpeningComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
