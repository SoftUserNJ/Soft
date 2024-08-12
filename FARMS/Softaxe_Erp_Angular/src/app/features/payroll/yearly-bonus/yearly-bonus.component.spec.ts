import { ComponentFixture, TestBed } from '@angular/core/testing';

import { YearlyBonusComponent } from './yearly-bonus.component';

describe('YearlyBonusComponent', () => {
  let component: YearlyBonusComponent;
  let fixture: ComponentFixture<YearlyBonusComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [YearlyBonusComponent]
    });
    fixture = TestBed.createComponent(YearlyBonusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
