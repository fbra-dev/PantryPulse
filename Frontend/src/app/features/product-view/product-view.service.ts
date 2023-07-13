import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "@environments/environment";

@Injectable({
  providedIn: 'root'
})
export class ProductViewService {


  constructor(private httpClient: HttpClient) { }

  loadAllProducts() {
    return this.httpClient.get<Product[]>(`${environment.apiUrl}api/product`)
  }
  loadProduct(id: string) {
    return this.httpClient.get<Product>(`${environment.apiUrl}api/product/${id}`)
  }
  updateProduct(product: Product) {
    return this.httpClient.patch<Product>(`${environment.apiUrl}api/product`, product)
  }
  addProduct(product: Product) {
    return this.httpClient.post<Product>(`${environment.apiUrl}api/product`, product)
  }
  deleteProduct(id: string) {
    return this.httpClient.delete<Product>(`${environment.apiUrl}api/product/${id}`)
  }

}
export interface Product {
  id: string | undefined;
  name: string;
  regularWeight: number;
}

