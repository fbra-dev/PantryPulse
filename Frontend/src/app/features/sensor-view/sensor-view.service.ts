import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {Observable, Subscriber} from "rxjs";

import * as signalR from "@microsoft/signalr"
import {environment} from "@environments/environment";
import {AccountService} from "@app/authorization/_services";
import {IHttpConnectionOptions} from "@microsoft/signalr";
import {Product} from "@app/features/product-view/product-view.service";

@Injectable({
  providedIn: 'root'
})
export class SensorViewService {
  // @ts-ignore
  public data: Sensor[] = [];
  constructor(private accountService: AccountService) {
  }
   // constructor() { this.data = null}
  // loadSensors()
  // {
  //   return this.httpClient.get<Sensor[]>('http://172.22.128.75:5142/api/scalesensor');
  // }
  // public fetchModel(): Observable<Sensor[]> {
  //   return new Observable<Sensor[]>((subscriber: Subscriber<Sensor[]>) => subscriber.next(EXAMPLE_DATA));
  // }
  // @ts-ignore
  private hubConnection: signalR.HubConnection
  public closeConnection()
  {
    this.hubConnection.stop();
  }
  public startConnection = () => {
    const options: IHttpConnectionOptions = {
      accessTokenFactory: () => {
        const user = this.accountService.userValue;
        const isLoggedIn = user && user.token;
        if (isLoggedIn && user.token) {
          return user.token;
        }
        return "";
      }
    };
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.apiUrl}scalesensorhub`,options)
      .withAutomaticReconnect([0, 2000, 10000, 30000,60000,120000])
      .build();
    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch(err => console.log('Error while starting connection: ' + err))
  }
  public updateSensor(sensor: Sensor)
  {
    this.hubConnection.invoke('UpdateSensor',sensor)
      .catch(err => console.error(err));
  }

  public addTransferChartDataListener = () => {
    this.hubConnection.on('ReceiveSensorInfo', (input) => {
      let sensor = this.data.find(sensor => sensor.id == input.id);
      if(sensor)
      {
        if(sensor.Info?.id == input.id)
        {
          sensor.Info.weight = input.weight;
        }
        else
        {
          sensor.Info = {id: input.id, weight: input.weight};
        }

      }
      else {
        this.data.push(input);
      }
      console.log(input);
    });
    this.hubConnection.on('Initialize', (sensorArray) => {
      if(sensorArray)
      {
        this.data = sensorArray;
      }
      else
      {
        this.data = [];
      }
      console.log(sensorArray);
    });
  }
}
export interface Sensor {
  id: string;
  name: string;
  Info: SensorInfo;
  product: Product;
}
export interface SensorInfo {
  id: string;
  weight: number;
}
