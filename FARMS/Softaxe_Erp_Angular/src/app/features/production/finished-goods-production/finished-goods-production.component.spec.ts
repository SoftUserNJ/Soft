import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FinishedGoodsProductionComponent } from './finished-goods-production.component';

describe('FinishedGoodsProductionComponent', () => {
  let component: FinishedGoodsProductionComponent;
  let fixture: ComponentFixture<FinishedGoodsProductionComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [FinishedGoodsProductionComponent]
    });
    fixture = TestBed.createComponent(FinishedGoodsProductionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
