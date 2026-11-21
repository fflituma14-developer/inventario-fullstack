using InventarioHexagonal.Application.Interfaces;
using InventarioHexagonal.Domain.Entities;

namespace InventarioHexagonal.Application.Services;

public class AuthService
{
    private readonly IUsuarioRepository _repository;
    private readonly IPasswordService _passwordService;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthService(
        IUsuarioRepository repository,
        IPasswordService passwordService,
        IJwtTokenService jwtTokenService)
    {
        _repository = repository;
        _passwordService = passwordService;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<string?> LoginAsync(
        string nombreUsuario,
        string password)
    {
        // Buscar el usuario
        var usuario = await _repository
            .GetByNombreUsuarioAsync(nombreUsuario);

        // Comprobar que exista y esté activo
        if (usuario == null || usuario.Estado != "A")
            return null;

        var ahora = DateTime.UtcNow;

        // Verificar bloqueo temporal
        if (usuario.BloqueadoHasta.HasValue &&
            usuario.BloqueadoHasta.Value > ahora)
        {
            return null;
        }

        // Verificar contraseña
        if (!_passwordService.VerifyPassword(
            usuario.PasswordHash, password))
        {
            usuario.IntentosFallidos++;

            if (usuario.IntentosFallidos >= 5)
            {
                usuario.BloqueadoHasta = ahora.AddMinutes(15);
                usuario.IntentosFallidos = 0;
            }

            await _repository.UpdateAsync(usuario);

            return null;
        }

        // Restablecer intentos fallidos
        usuario.IntentosFallidos = 0;
        usuario.BloqueadoHasta = null;

        await _repository.UpdateAsync(usuario);

        // Generar token JWT
        return _jwtTokenService.GenerateToken(usuario);
    }

    public async Task<bool> RegistrarAsync(
        string nombreUsuario,
        string nombreCompleto,
        string correo,
        string password)
    {
        nombreUsuario = nombreUsuario.Trim();
        nombreCompleto = nombreCompleto.Trim();
        correo = correo.Trim();

        // Verificar si ya existe el usuario o correo
        var existe = await _repository.ExisteUsuarioAsync(
            nombreUsuario, correo);

        if (existe)
            return false;

        // Generar el hash de la contraseña
        var passwordHash = _passwordService.HashPassword(password);

        var usuario = new Usuario
        {
            NombreUsuario = nombreUsuario,
            NombreCompleto = nombreCompleto,
            Correo = correo,
            PasswordHash = passwordHash,
            Estado = "A",
            IntentosFallidos = 0,
            FechaCreacion = DateTime.UtcNow
        };

        await _repository.CreateAsync(usuario);

        return true;
    }
}