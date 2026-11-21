
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import { AuthService } from '@features/auth/services/auth.service';
import { AlertaService } from '@core/services/alerta.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login {

  private authService = inject(AuthService);
  private router = inject(Router);
  private alertaService = inject(AlertaService);

  usuario: string = '';
  password: string = '';
  mostrarPassword: boolean = false;
  cargando: boolean = false;

  onSubmit(): void {

    if (!this.usuario.trim() || !this.password || this.cargando) {
      return;
    }

    this.cargando = true;

    this.authService.login({
      nombreUsuario: this.usuario.trim(),
      password: this.password
    }).subscribe({
      next: (response) => {
        this.cargando = false;

        this.authService.guardarToken(response.token);

        this.router.navigate(['/productos']);
      },
      error: (error: Error) => {
        this.cargando = false;

        this.alertaService.error(error.message);
      }
    });
  }
}
