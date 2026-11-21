
import { Injectable } from '@angular/core';
import Swal from 'sweetalert2';

@Injectable({
  providedIn: 'root'
})
export class AlertaService {

  exito(mensaje: string): void {
    Swal.fire({
      icon: 'success',
      title: 'Operación exitosa',
      text: mensaje,
      confirmButtonText: 'Aceptar',
      confirmButtonColor: '#0d6efd'
    });
  }

  error(mensaje: string): void {
    Swal.fire({
      icon: 'error',
      title: 'Ocurrió un error',
      text: mensaje,
      confirmButtonText: 'Aceptar',
      confirmButtonColor: '#0d6efd'
    });
  }

  async confirmar(mensaje: string): Promise<boolean> {
    const resultado = await Swal.fire({
      icon: 'warning',
      title: 'Confirmar Operación',
      html: mensaje,
      showCancelButton: true,
      confirmButtonText: 'Sí, eliminar',
      cancelButtonText: 'Cancelar',
      confirmButtonColor: '#dc3545'
    });

    return resultado.isConfirmed;
  }
}
