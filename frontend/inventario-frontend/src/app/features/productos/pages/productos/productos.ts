
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { Producto } from '@features/productos/interfaces/producto.interface';
import { ProductoService } from '@features/productos/services/producto.service';
import { AlertaService } from '@core/services/alerta.service';

@Component({
  selector: 'app-productos',
  imports: [FormsModule],
  templateUrl: './productos.html',
  styleUrl: './productos.scss',
})
export class Productos {

  private productoService = inject(ProductoService);
  private alertaService = inject(AlertaService);

  productos: Producto[] = [];

  mostrarFormulario: boolean = false;
  productoEditandoId: number | null = null;

  guardando: boolean = false;
  cargando: boolean = false;

  eliminandoId: number | null = null;

  nuevoProducto = {
    nombre: '',
    descripcion: '',
    estado: 'A'
  };

  abrirFormulario(producto?: Producto): void {

    this.productoEditandoId = producto?.idProducto ?? null;

    this.nuevoProducto = {
      nombre: producto?.nombre ?? '',
      descripcion: producto?.descripcion ?? '',
      estado: producto?.estado ?? 'A'
    };

    this.mostrarFormulario = true;
  }

  cerrarFormulario(): void {
    this.mostrarFormulario = false;
  }

  cargarProductos(): void {
    this.cargando = true;

    this.productoService.obtenerProductos().subscribe({
      next: (response) => {
        this.productos = response;
        this.cargando = false;
      },
      error: (error: Error) => {
        this.cargando = false;
        this.alertaService.error(error.message);
      }
    });
  }

  guardarProducto(): void {

    if (!this.nuevoProducto.nombre.trim() || this.guardando) {
      return;
    }

    this.guardando = true;

    const producto = {
      nombre: this.nuevoProducto.nombre.trim(),
      descripcion: this.nuevoProducto.descripcion.trim(),
      estado: this.productoEditandoId === null
        ? 'A'
        : this.nuevoProducto.estado
    };

    // CREAR PRODUCTO
    if (this.productoEditandoId === null) {

      this.productoService.crearProducto(producto).subscribe({
        next: (nuevoProducto) => {

          this.productos.push(nuevoProducto);

          this.guardando = false;
          this.cerrarFormulario();

          this.alertaService.exito(
            'Producto registrado correctamente.'
          );
        },
        error: (error: Error) => {
          this.guardando = false;
          this.alertaService.error(error.message);
        }
      });

    } else {

      // ACTUALIZAR PRODUCTO
      const id = this.productoEditandoId;

      this.productoService.actualizarProducto(id, producto).subscribe({
        next: () => {

          const productoActual = this.productos.find(
            p => p.idProducto === id
          );

          if (productoActual) {
            productoActual.nombre = producto.nombre;
            productoActual.descripcion = producto.descripcion;
            productoActual.estado = producto.estado;
          }

          this.guardando = false;
          this.cerrarFormulario();

          this.alertaService.exito(
            'Producto actualizado correctamente.'
          );
        },
        error: (error: Error) => {
          this.guardando = false;
          this.alertaService.error(error.message);
        }
      });
    }
  }

  async eliminarProducto(producto: Producto): Promise<void> {

    if (this.eliminandoId !== null) {
      return;
    }

    const confirmado = await this.alertaService.confirmar(
      `¿Estás seguro de eliminar el producto <b>${producto.nombre}</b>?`
    );

    if (!confirmado) {
      return;
    }

    this.eliminandoId = producto.idProducto;

    this.productoService.eliminarProducto(producto.idProducto).subscribe({
      next: () => {

        this.productos = this.productos.filter(
          p => p.idProducto !== producto.idProducto
        );

        this.eliminandoId = null;

        this.alertaService.exito(
          'Producto eliminado correctamente.'
        );
      },
      error: (error: Error) => {
        this.eliminandoId = null;
        this.alertaService.error(error.message);
      }
    });
  }

  ngOnInit(): void {
    this.cargarProductos();
  }
}
