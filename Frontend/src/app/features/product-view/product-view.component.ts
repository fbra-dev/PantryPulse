import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ProductCreateDialogComponent } from './product-create-dialog/product-create-dialog.component';
import { ProductEditDialogComponent } from './product-edit-dialog/product-edit-dialog.component';
import {Product, ProductViewService} from './product-view.service';


@Component({
  selector: 'app-product-view',
  templateUrl: './product-view.component.html',
  styleUrls: ['./product-view.component.scss']
})
export class ProductViewComponent implements AfterViewInit, OnInit {

  addProductBtnText : string ='Produkt anlegen';
  displayedColumns: string[] = ['id', 'name', 'regularWeight'];
  dataSource = new MatTableDataSource<Product>();
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild(MatPaginator) paginator!: MatPaginator;


  constructor(private productViewService: ProductViewService,
              private dialog: MatDialog){}

  ngOnInit(): void
  {
    this.refresh();
  }
  ngAfterViewInit(): void
  {
    this.dataSource.sort      = this.sort;
    this.dataSource.paginator = this.paginator;
  }

  addProduct()
  {
    const dialogConfig = this.setAddDialogConfig();

    const dialogRef = this.dialog.open(ProductCreateDialogComponent, dialogConfig);

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.productViewService.addProduct(result).subscribe(() => this.refresh());
      }
    });
  }
  async editProduct(product: Product)
  {
    this.openEditDialog(product);
  }
  async delete(product: Product)
  {
    let shouldDelete = this.deleteConfirmationDialog();
    if(shouldDelete)
    {
      if(product.id)
      {
        this.productViewService.deleteProduct(product.id).subscribe(() => this.refresh());
      }
    }
  }
  applyFilter(event: Event)
  {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
  refresh()
  {
    this.productViewService.loadAllProducts().subscribe(product =>
      this.dataSource.data = product as Product[]);
  }
  update(product: Product)
  {
    this.productViewService.updateProduct(product).subscribe(() => this.refresh());
  }
  setEditDialogConfig(product: Product) : MatDialogConfig<Product>
  {

    const dialogConfig = new MatDialogConfig<Product>();
    dialogConfig.disableClose = true;
    dialogConfig.width = '500px';
    dialogConfig.height = 'auto';
    dialogConfig.panelClass = 'edit-dialog';

    dialogConfig.data = {
      id: product.id,
      name: product.name,
      regularWeight: product.regularWeight
    }
    return dialogConfig;

  }
  setAddDialogConfig() : MatDialogConfig<Product>
  {
    const dialogConfig = new MatDialogConfig<Product>();
    dialogConfig.width = '500px';
    dialogConfig.height = 'auto';
    dialogConfig.data = {
      id: undefined,
      name: "",
      regularWeight: 0
    }
    return dialogConfig;
  }
  deleteConfirmationDialog()
  {
    return confirm(`Wollen Sie das Produkt wirklich löschen?`);
  }
  openEditDialog(product: Product)
  {
    const dialogConfig    = this.setEditDialogConfig(product);
    const dialogRef       = this.dialog.open(ProductEditDialogComponent, dialogConfig);
    dialogRef.afterClosed().subscribe(async result =>
    {
      if (result)
      {
        this.update(result);
      }
    });
  }
}
