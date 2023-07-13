import {Component, Inject} from '@angular/core';
import {MAT_DIALOG_DATA} from "@angular/material/dialog";
import {Product} from "@app/features/product-view/product-view.service";

@Component({
  selector: 'app-product-edit-dialog',
  templateUrl: './product-edit-dialog.component.html',
  styleUrls: ['./product-edit-dialog.component.scss']
})
export class ProductEditDialogComponent{

  pageTitle: string ="Produkt bearbeiten"
  constructor(@Inject (MAT_DIALOG_DATA) public data: Product){}
}

