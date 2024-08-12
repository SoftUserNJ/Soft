import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PostDatedChequeComponent } from './post-dated-cheque.component';

describe('PostDatedChequeComponent', () => {
  let component: PostDatedChequeComponent;
  let fixture: ComponentFixture<PostDatedChequeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PostDatedChequeComponent]
    });
    fixture = TestBed.createComponent(PostDatedChequeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
