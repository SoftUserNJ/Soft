import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FinishGoodsStatusComponent } from './finish-goods-status.component';

describe('FinishGoodsStatusComponent', () => {
  let component: FinishGoodsStatusComponent;
  let fixture: ComponentFixture<FinishGoodsStatusComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [FinishGoodsStatusComponent]
    });
    fixture = TestBed.createComponent(FinishGoodsStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
