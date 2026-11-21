import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '@environments/environment';
import { Proveedor } from '../interfaces/proveedor.interface';

@Injectable({
  providedIn: 'root'
})
export class ProveedorService {

  private http = inject(HttpClient);

  private apiUrl = `${environment.apiUrl}/proveedores`;

  obtenerProveedores(): Observable<Proveedor[]> {
    return this.http.get<Proveedor[]>(this.apiUrl);
  }
}