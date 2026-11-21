import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Producto } from '@features/productos/interfaces/producto.interface';
import { ProductoService } from '@features/productos/services/producto.service';


@Component({
  selector: 'app-productos',
  imports: [FormsModule],
  templateUrl: './productos.html',
  styleUrl: './productos.scss',
})
export class Productos {
  private productoService = inject(ProductoService);
  productos: Producto[] = [];

  mostrarFormulario: boolean = false;
  productoEditandoId: number | null = null;
  guardando: boolean = false;

  cargando: boolean = false;
  mensajeError: string = '';
  
  nuevoProducto = {
    nombre: '',
    descripcion: ''
  };

  abrirFormulario(producto?: Producto): void {
    this.productoEditandoId = producto?.idProducto ?? null;

    this.nuevoProducto = {
      nombre: producto?.nombre ?? '',
      descripcion: producto?.descripcion ?? ''
    };

    this.mensajeError = '';
    this.mostrarFormulario = true;
  }

  cerrarFormulario(): void {
    this.mostrarFormulario = false;
  }
  
  cargarProductos(): void {
    this.cargando = true;
    this.mensajeError = '';

    this.productoService.obtenerProductos().subscribe({
      next: (response) => {
        this.productos = response;
        this.cargando = false;
      },
      error: (error) => {
        console.error('Error al consultar productos:', error.status);
        this.mensajeError = 'No se pudieron cargar los productos.';
        this.cargando = false;
      }
    });
  }
  
  guardarProducto(): void {

    if (!this.nuevoProducto.nombre.trim() || this.guardando) {
      return;
    }

    this.guardando = true;
    this.mensajeError = '';

    const producto = {
      nombre: this.nuevoProducto.nombre.trim(),
      descripcion: this.nuevoProducto.descripcion.trim()
    };

    // CREAR PRODUCTO
    if (this.productoEditandoId === null) {

      this.productoService.crearProducto(producto).subscribe({
        next: (nuevoProducto) => {

          this.productos.push(nuevoProducto);

          this.guardando = false;
          this.cerrarFormulario();
        },
        error: () => {
          this.guardando = false;
          this.mensajeError = 'No se pudo crear el producto.';
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
          }

          this.guardando = false;
          this.cerrarFormulario();
        },
        error: () => {
          this.guardando = false;
          this.mensajeError = 'No se pudo actualizar el producto.';
        }
      });
    }
  }

  eliminandoId: number | null = null;

  eliminarProducto(producto: Producto): void {

    const confirmar = confirm(
      `¿Estás seguro de eliminar el producto "${producto.nombre}"?`
    );

    if (!confirmar) {
      return;
    }

    this.eliminandoId = producto.idProducto;
    this.mensajeError = '';

    this.productoService.eliminarProducto(producto.idProducto).subscribe({
      next: () => {

        this.productos = this.productos.filter(
          p => p.idProducto !== producto.idProducto
        );

        this.eliminandoId = null;
      },
      error: () => {
        this.eliminandoId = null;
        this.mensajeError = 'No se pudo eliminar el producto.';
      }
    });
  }

  ngOnInit(): void {
    this.cargarProductos();
  }


}
