
# Sistema de Gestión de Inventario

Aplicación web para la gestión de productos e inventario, que incluye
autenticación de usuarios y operaciones de mantenimiento de registros.

## Tecnologías utilizadas

### Backend
- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Arquitectura Hexagonal
- Swagger / OpenAPI

### Frontend
- Angular 20
- TypeScript
- Arquitectura basada en funcionalidades (Feature-Based Architecture)

## Configuración inicial

Antes de iniciar sesión en la aplicación, es necesario registrar un
usuario mediante Swagger.

### 1. Registro de usuario

Una vez ejecutado el backend, acceder a Swagger y localizar el
siguiente endpoint dentro de la sección Auth:

POST /api/auth/registro-inicial

Seleccionar la opción **Try it out** e ingresar los datos del usuario.

Ejemplo:

```json
{
  "nombreUsuario": "oscar",
  "nombreCompleto": "Oscar Prueba",
  "correo": "oscar@example.com",
  "password": "Inventario123*"
}
```

Presionar el botón **Execute** para registrar el usuario.

### 2. Inicio de sesión

Una vez creado el usuario correctamente, acceder a la aplicación
Angular e iniciar sesión con las credenciales registradas.

### 3. Funcionalidades del sistema

- Autenticación de usuarios.
- Gestión de productos.
- Gestión de inventario.
- Registro, consulta y actualización de información.
- Eliminación lógica de productos.
