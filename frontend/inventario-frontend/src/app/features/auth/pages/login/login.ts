import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '@features/auth/services/auth.service';

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
  
  usuario: string = '';
  password: string = '';
  mostrarPassword: boolean = false;
  cargando: boolean = false;
  mensajeError: string = '';
  
  onSubmit(): void{
    if(!this.usuario.trim() || !this.password){
      return;
    }
  
    this.cargando = true;
    this.mensajeError = '';

    this.authService.login({
      nombreUsuario: this.usuario.trim(),
      password: this.password
    }).subscribe({
      next: (response) => {
        this.cargando = false;
        this.authService.guardarToken(response.token);
        console.log('Autenticación exitosa');
        this.router.navigate(['/productos']);
        // Después gestionaremos el token y la navegación.
      },
      error: (error) => {
        this.cargando = false;

        this.mensajeError = error.status === 401
          ? 'Usuario o contraseña incorrectos.'
          : 'No fue posible iniciar sesión.';
      }
    });
  }

  
}
