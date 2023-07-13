import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {SensorViewComponent} from "./features/sensor-view/sensor-view.component";
import {AppLayoutComponent} from "./_layouts/app-layout/app-layout.component";
import {AuthGuard} from "@app/authorization/_helpers";
import {ProductViewComponent} from "@app/features/product-view/product-view.component";
const accountModule = () => import('./authorization/account/account.module').then(x => x.AccountModule);
const routes: Routes = [
  {
    path: "",
    component: AppLayoutComponent,
    children: [
      { path: '', component: SensorViewComponent, pathMatch: 'full', canActivate: [AuthGuard] }, // Default route
      { path: 'sensorView', component: SensorViewComponent, canActivate: [AuthGuard] },
      { path: 'account', loadChildren: accountModule },
      { path: 'productView', component: ProductViewComponent, canActivate: [AuthGuard] },
      // otherwise redirect to home
      { path: '**', redirectTo: '' }
    ],
  },
  // Add more routes here
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule { }
