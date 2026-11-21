
import { Component, inject, OnInit } from '@angular/core';
import { FormsModule, NgForm, NgModel } from '@angular/forms';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { forkJoin } from 'rxjs';

import { ProductoService } from '@features/productos/services/producto.service';
import { ProveedorService } from '@features/proveedores/services/proveedor.service';
import { InventarioService } from '../../services/inventario.service';

import { Producto } from '@features/productos/interfaces/producto.interface';
import { Proveedor } from '@features/proveedores/interfaces/proveedor.interface';
import { LoteProducto } from '../../interfaces/lote-producto.interface';

import { AlertaService } from '@core/services/alerta.service';
import { validarNumero } from '@shared/helpers/numero.helper';

@Component({
  selector: 'app-inventario',
  standalone: true,
  imports: [FormsModule, CurrencyPipe, DatePipe],
  templateUrl: './inventario.html',
  styleUrl: './inventario.scss'
})
export class Inventario implements OnInit {

  private productoService = inject(ProductoService);
  private proveedorService = inject(ProveedorService);
  private inventarioService = inject(InventarioService);
  private alertaService = inject(AlertaService);

  productos: Producto[] = [];
  proveedores: Proveedor[] = [];
  lotes: LoteProducto[] = [];

  cargando: boolean = false;
  guardando: boolean = false;

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

    forkJoin({
      productos: this.productoService.obtenerProductos(),
      proveedores: this.proveedorService.obtenerProveedores(),
      lotes: this.inventarioService.obtenerLotes()
    }).subscribe({
      next: (response) => {
        this.productos = response.productos.filter(
          p => p.estado === 'A'
        );

        this.proveedores = response.proveedores.filter(
          p => p.estado === 'A'
        );

        this.lotes = response.lotes;
        this.cargando = false;
      },
      error: (error: Error) => {
        this.cargando = false;
        this.alertaService.error(error.message);
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
      error: (error: Error) => {
        this.alertaService.error(error.message);
      }
    });
  }

  guardarLote(formulario: NgForm): void {

    if (
      formulario.invalid ||
      this.cargando ||
      this.guardando ||
      this.nuevoLote.idProducto <= 0 ||
      this.nuevoLote.idProveedor <= 0 ||
      !this.nuevoLote.numeroLote.trim() ||
      !Number.isFinite(this.nuevoLote.precioCompra) ||
      this.nuevoLote.precioCompra <= 0 ||
      !Number.isInteger(this.nuevoLote.cantidadStock) ||
      this.nuevoLote.cantidadStock <= 0 ||
      this.nuevoLote.cantidadStock > 999999
    ) {
      return;
    }

    this.guardando = true;

    const lote = {
      ...this.nuevoLote,
      numeroLote: this.nuevoLote.numeroLote.trim()
    };

    this.inventarioService.crearLote(lote).subscribe({
      next: () => {

        this.nuevoLote = {
          idProducto: 0,
          idProveedor: 0,
          numeroLote: '',
          precioCompra: 0,
          cantidadStock: 0
        };

        formulario.resetForm({
          idProducto: 0,
          idProveedor: 0,
          numeroLote: '',
          precioCompra: '',
          cantidadStock: ''
        });

        this.guardando = false;

        this.obtenerLotes();

        this.alertaService.exito(
          'Ingreso de inventario registrado correctamente.'
        );
      },
      error: (error: Error) => {
        this.guardando = false;
        this.alertaService.error(error.message);
      }
    });
  }

  validarPrecio(event: Event, control: NgModel): void {
    this.nuevoLote.precioCompra =
      validarNumero(event, control, 'precio');
  }

  validarCantidad(event: Event, control: NgModel): void {
    this.nuevoLote.cantidadStock =
      validarNumero(event, control, 'cantidad');
  }
}
