import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewAuthUsageModalComponent } from './view-auth-usage-modal.component';

describe('ViewAuthUsageModalComponent', () => {
  let component: ViewAuthUsageModalComponent;
  let fixture: ComponentFixture<ViewAuthUsageModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewAuthUsageModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewAuthUsageModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
