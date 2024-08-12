import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VchTypeSettingComponent } from './vch-type-setting.component';

describe('VchTypeSettingComponent', () => {
  let component: VchTypeSettingComponent;
  let fixture: ComponentFixture<VchTypeSettingComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [VchTypeSettingComponent]
    });
    fixture = TestBed.createComponent(VchTypeSettingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
