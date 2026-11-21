import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '@environments/environment';
import { LoteProducto } from '../interfaces/lote-producto.interface';

@Injectable({
  providedIn: 'root'
})
export class InventarioService {

  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/lotes`;

  obtenerLotes(): Observable<LoteProducto[]> {
    return this.http.get<LoteProducto[]>(this.apiUrl);
  }

  crearLote(lote: {
    idProducto: number;
    idProveedor: number;
    numeroLote: string;
    precioCompra: number;
    cantidadStock: number;
  }): Observable<LoteProducto> {
    return this.http.post<LoteProducto>(this.apiUrl, lote);
  }
}