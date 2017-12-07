import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AuthorizationsManagerComponent } from './authorizations-manager.component';

describe('AuthorizationsManagerComponent', () => {
  let component: AuthorizationsManagerComponent;
  let fixture: ComponentFixture<AuthorizationsManagerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AuthorizationsManagerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AuthorizationsManagerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
