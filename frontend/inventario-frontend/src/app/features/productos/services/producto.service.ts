import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '@environments/environment';
import { Producto } from '../interfaces/producto.interface';

@Injectable({
  providedIn: 'root'
})
export class ProductoService {
    private http = inject(HttpClient);
    private apiUrl = `${environment.apiUrl}/productos`;

    obtenerProductos(): Observable<Producto[]> {
        return this.http.get<Producto[]>(this.apiUrl);
    }

    crearProducto(producto: {
        nombre: string;
        descripcion: string;
    }): Observable<Producto> {

        return this.http.post<Producto>(
            this.apiUrl,
            producto
        );
    }

    actualizarProducto(
        id: number,
        producto: { nombre: string; descripcion: string }
    ): Observable<void> {

        return this.http.put<void>(
        `${this.apiUrl}/${id}`,
        producto
        );
    }
    
    eliminarProducto(id: number): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`);
    }
}