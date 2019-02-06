/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { ValueModelComponent } from './valueModel.component';

describe('ValueModelComponent', () => {
  let component: ValueModelComponent;
  let fixture: ComponentFixture<ValueModelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ValueModelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ValueModelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
