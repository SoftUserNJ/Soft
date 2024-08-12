import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MaterialReceiveAgainstDemmadComponent } from './material-receive-against-demmad.component';

describe('MaterialReceiveAgainstDemmadComponent', () => {
  let component: MaterialReceiveAgainstDemmadComponent;
  let fixture: ComponentFixture<MaterialReceiveAgainstDemmadComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MaterialReceiveAgainstDemmadComponent]
    });
    fixture = TestBed.createComponent(MaterialReceiveAgainstDemmadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
