import {Injectable, inject} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import { Observable} from 'rxjs';
import { environment } from '@environments/environment';

interface LoginRequest {
  nombreUsuario: string;
  password: string;
}

interface LoginResponse {
  token: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
    private http = inject(HttpClient);
    private apiUrl = `${environment.apiUrl}/auth`;

    login(credentials: LoginRequest): Observable<LoginResponse> {
        return this.http.post<LoginResponse>(
            `${this.apiUrl}/login`,
            credentials
        );
    }
    guardarToken(token: string): void {
        sessionStorage.setItem('token', token);
    }

    obtenerToken(): string | null {
        return sessionStorage.getItem('token');
    }

    cerrarSesion(): void {
        sessionStorage.removeItem('token');
    }

    estaAutenticado(): boolean {
        return this.obtenerToken() !== null;
    }
}
