import {Component, Inject} from '@angular/core';
import {MAT_DIALOG_DATA} from "@angular/material/dialog";
import {Product} from "@app/features/product-view/product-view.service";

@Component({
  selector: 'app-product-create-dialog',
  templateUrl: './product-create-dialog.component.html',
  styleUrls: ['./product-create-dialog.component.scss']
})
export class ProductCreateDialogComponent {

  pageTitle: string ="Produkt bearbeiten"
  constructor(@Inject (MAT_DIALOG_DATA) public data: Product){}
}
