import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateAuthModalComponent } from './create-auth-modal.component';

describe('CreateAuthModalComponent', () => {
  let component: CreateAuthModalComponent;
  let fixture: ComponentFixture<CreateAuthModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateAuthModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateAuthModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
