import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GetChatComponent } from './get-chat.component';

describe('GetChatComponent', () => {
  let component: GetChatComponent;
  let fixture: ComponentFixture<GetChatComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GetChatComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GetChatComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
