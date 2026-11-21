import { Routes } from '@angular/router';
import { authGuard } from '@core/guards/auth.guard';
import { Login } from '@features/auth/pages/login/login';
import { Inventario } from '@features/inventario/pages/inventario/inventario';
import { Productos } from '@features/productos/pages/productos/productos';


export const routes: Routes = [
  {
    path: 'login',
    component: Login
  },
  {
    path: 'productos',
    component: Productos,
    canActivate: [authGuard]
  },
  {
    path: 'inventario',
    component: Inventario,
    canActivate: [authGuard]
  },
  {
    path: '',
    redirectTo: 'productos',
    pathMatch: 'full'
  },
  {
    path: '**',
    redirectTo: 'login'
  }
];
