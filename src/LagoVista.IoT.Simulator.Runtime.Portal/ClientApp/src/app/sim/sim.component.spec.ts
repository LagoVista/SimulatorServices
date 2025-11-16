// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: bf094e46e824a891463a08c07144dc9701b06003a1d688da5cc27ca8037c7d29
// IndexVersion: 2
// --- END CODE INDEX META ---
/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { SimComponent } from './sim.component';

describe('SimComponent', () => {
  let component: SimComponent;
  let fixture: ComponentFixture<SimComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SimComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SimComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
