import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { forkJoin } from 'rxjs';

import { ProductoService } from '@features/productos/services/producto.service';
import { ProveedorService } from '@features/proveedores/services/proveedor.service';
import { InventarioService } from '../../services/inventario.service';

import { Producto } from '@features/productos/interfaces/producto.interface';
import { Proveedor } from '@features/proveedores/interfaces/proveedor.interface';
import { LoteProducto } from '../../interfaces/lote-producto.interface';

@Component({
  selector: 'app-inventario',
  standalone: true,
  imports: [FormsModule,CurrencyPipe, DatePipe],
  templateUrl: './inventario.html',
  styleUrl: './inventario.scss'
})
export class Inventario implements OnInit {

  private productoService = inject(ProductoService);
  private proveedorService = inject(ProveedorService);
  private inventarioService = inject(InventarioService);

  productos: Producto[] = [];
  proveedores: Proveedor[] = [];
  lotes: LoteProducto[] = [];

  cargando: boolean = false;
  guardando: boolean = false;
  mensajeError: string = '';
  mensajeExito: string = '';

  nuevoLote = {
    idProducto: 0,
    idProveedor: 0,
    numeroLote: '',
    precioCompra: 0,
    cantidadStock: 0
  };

  ngOnInit(): void {
    this.cargarDatos();
  }

  cargarDatos(): void {
    this.cargando = true;
    this.mensajeError = '';

    forkJoin({
      productos: this.productoService.obtenerProductos(),
      proveedores: this.proveedorService.obtenerProveedores(),
      lotes: this.inventarioService.obtenerLotes()
    }).subscribe({
      next: (response) => {
        this.productos = response.productos.filter(p => p.estado === 'A');
        this.proveedores = response.proveedores.filter(p => p.estado === 'A');
        this.lotes = response.lotes;

        this.cargando = false;
      },
      error: () => {
        this.mensajeError = 'No se pudieron cargar los datos del inventario.';
        this.cargando = false;
      }
    });
  }

  obtenerNombreProducto(idProducto: number): string {
    const producto = this.productos.find(
      p => p.idProducto === idProducto
    );

    return producto?.nombre ?? 'No disponible';
  }

  obtenerNombreProveedor(idProveedor: number): string {
    const proveedor = this.proveedores.find(
      p => p.idProveedor === idProveedor
    );

    return proveedor?.nombreComercial ?? 'No disponible';
  }

  obtenerLotes(): void {
    this.inventarioService.obtenerLotes().subscribe({
      next: (response) => {
        this.lotes = response;
      },
      error: () => {
        this.mensajeError = 'No se pudo cargar el listado de lotes.';
      }
    });
  }
  guardarLote(): void {
    if (
      this.guardando ||
      this.nuevoLote.idProducto <= 0 ||
      this.nuevoLote.idProveedor <= 0 ||
      !this.nuevoLote.numeroLote.trim() ||
      this.nuevoLote.precioCompra < 0 ||
      this.nuevoLote.cantidadStock < 0
    ) {
      return;
    }

    this.guardando = true;
    this.mensajeError = '';
    this.mensajeExito = '';

    const lote = {
      ...this.nuevoLote,
      numeroLote: this.nuevoLote.numeroLote.trim()
    };

    this.inventarioService.crearLote(lote).subscribe({
      next: (nuevoLote) => {
        this.nuevoLote = {
          idProducto: 0,
          idProveedor: 0,
          numeroLote: '',
          precioCompra: 0,
          cantidadStock: 0
        };

        this.guardando = false;

        this.obtenerLotes();

        this.mensajeExito = 'Ingreso de inventario registrado correctamente.';
      },
      error: (error) => {
        this.guardando = false;
        this.mensajeError = error.status === 409
          ? 'Ya existe un lote con esos datos.'
          : 'No se pudo registrar el ingreso de inventario.';
      }
    });
  }
}