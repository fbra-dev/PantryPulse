import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductEditDialogComponent } from './product-edit-dialog.component';

describe('ProductEditDialogComponent', () => {
  let component: ProductEditDialogComponent;
  let fixture: ComponentFixture<ProductEditDialogComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ProductEditDialogComponent]
    });
    fixture = TestBed.createComponent(ProductEditDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
