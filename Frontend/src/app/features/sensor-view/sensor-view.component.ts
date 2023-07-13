import {
  AfterContentInit,
  AfterViewInit,
  Component,
  Inject,
  NgModule,
  OnDestroy,
  OnInit,
  ViewChild
} from '@angular/core';
import {Sensor, SensorViewService} from "./sensor-view.service";
import {MAT_DIALOG_DATA, MatDialog, MatDialogModule, MatDialogRef} from "@angular/material/dialog";
import {MatButtonModule} from "@angular/material/button";
import {FormsModule} from "@angular/forms";
import {MatInputModule} from "@angular/material/input";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatSelectModule} from "@angular/material/select";
import {Product, ProductViewService} from "@app/features/product-view/product-view.service";
import {NgForOf} from "@angular/common";

@Component({
  selector: 'app-sensor-view',
  templateUrl: './sensor-view.component.html',
  styleUrls: ['./sensor-view.component.scss'],
})
export class SensorViewComponent implements AfterContentInit, OnInit, OnDestroy {
  value = 0;
  products: Product[] = [];
  constructor(public sensorViewService: SensorViewService,public dialog: MatDialog) {}
  ngAfterContentInit (): void {
    this.sensorViewService.startConnection();
    this.sensorViewService.addTransferChartDataListener();
  }
  ngOnInit(): void {

  }
  onEditSensorButtonClick(sensor : Sensor)
  {
    if(!sensor.product)
    {
      sensor.product = {id: undefined, name: "", regularWeight: 0}
    }
    const dialogRef = this.dialog.open(SensorEditDialog, {
      data: sensor
    });
    dialogRef.afterClosed().subscribe(result => {
      if(result)
      {
        this.sensorViewService.updateSensor(result);
      }

    });
  }
  getDynamicLabel(value: number) : string {
    return Math.floor(value).toString() + " g";
  }

  ngOnDestroy(): void {
    this.sensorViewService.closeConnection();
  }
}
@Component({
  selector: 'sensor-view-renameDialog',
  templateUrl: 'sensor-view.component.editDialog.html',
  standalone: true,
  imports: [MatDialogModule, MatFormFieldModule, MatInputModule, FormsModule, MatButtonModule, MatSelectModule, NgForOf],
})
export class SensorEditDialog implements OnInit {
  constructor(
    public dialogRef: MatDialogRef<SensorEditDialog>,
    @Inject(MAT_DIALOG_DATA) public data: Sensor,
    public productViewService: ProductViewService
  ) {}
  products: Product[] = [];
  selectedProduct: string = "";
  onNoClick(): void {
    this.dialogRef.close();
  }

  onYesClick(): void {
    // if(this.data.product)
    // {
    //   this.data.product.id = this.
    // }
    this.dialogRef.close();
  }

  ngOnInit(): void {
    this.productViewService.loadAllProducts().subscribe(products => this.products = products);
  }
}
