import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MaterialReceivedInStoreComponent } from './material-received-in-store.component';

describe('MaterialReceivedInStoreComponent', () => {
  let component: MaterialReceivedInStoreComponent;
  let fixture: ComponentFixture<MaterialReceivedInStoreComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MaterialReceivedInStoreComponent]
    });
    fixture = TestBed.createComponent(MaterialReceivedInStoreComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
