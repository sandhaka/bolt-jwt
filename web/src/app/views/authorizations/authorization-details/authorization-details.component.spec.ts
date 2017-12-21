import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AuthorizationDetailsComponent } from './authorization-details.component';

describe('AuthorizationDetailsComponent', () => {
  let component: AuthorizationDetailsComponent;
  let fixture: ComponentFixture<AuthorizationDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AuthorizationDetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AuthorizationDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
