
import {
  HttpErrorResponse,
  HttpInterceptorFn
} from '@angular/common/http';

import { catchError, throwError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {

  return next(req).pipe(

    catchError((error: HttpErrorResponse) => {

      let mensaje = 'Ocurrió un error inesperado.';

      switch (error.status) {

        case 0:
          mensaje = 'No se pudo conectar con el servidor.';
          break;

        case 400:
          mensaje = error.error?.mensaje ||
            'Los datos enviados no son válidos.';
          break;

        case 401:
          mensaje = req.url.includes('/auth/login')
            ? 'Usuario o contraseña incorrectos.'
            : 'Tu sesión no es válida o ha expirado.';
          break;

        case 403:
          mensaje = 'No tienes permisos para esta operación.';
          break;

        case 404:
          mensaje = error.error?.mensaje ||
            'El recurso solicitado no existe.';
          break;

        case 409:
          mensaje = error.error?.mensaje ||
            'Existe un conflicto con los datos enviados.';
          break;

        case 500:
          mensaje = 'Ocurrió un error interno del servidor.';
          break;
      }

      // Registro del error sin exponer tokens ni contraseñas
      console.error('Error HTTP:', {
        estado: error.status,
        mensaje
      });

      return throwError(() => new Error(mensaje));

    })

  );

};
