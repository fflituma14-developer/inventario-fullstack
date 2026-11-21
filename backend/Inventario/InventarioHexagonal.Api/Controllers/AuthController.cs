using InventarioHexagonal.Api.DTOs;
using InventarioHexagonal.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventarioHexagonal.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        AuthService authService,
        IWebHostEnvironment environment,
        ILogger<AuthController> logger)
    {
        _authService = authService;
        _environment = environment;
        _logger = logger;
    }

    // POST: api/auth/login
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request)
    {
        var token = await _authService.LoginAsync(
            request.NombreUsuario,
            request.Password);

        if (token == null)
        {
            _logger.LogWarning(
                "Intento de autenticación fallido");

            return Unauthorized(new
            {
                mensaje = "Credenciales inválidas."
            });
        }

        _logger.LogInformation(
            "Usuario autenticado correctamente");

        return Ok(new { token });
    }

    // POST: api/auth/registro-inicial
    [AllowAnonymous]
    [HttpPost("registro-inicial")]
    public async Task<IActionResult> RegistroInicial(
        [FromBody] CrearUsuarioRequest request)
    {
        if (!_environment.IsDevelopment())
        {
            return NotFound();
        }

        var registrado = await _authService.RegistrarAsync(
            request.NombreUsuario,
            request.NombreCompleto,
            request.Correo,
            request.Password);

        if (!registrado)
        {
            _logger.LogWarning(
                "No se pudo registrar el usuario. Posible duplicado");

            return Conflict(new
            {
                mensaje = "No se pudo registrar el usuario."
            });
        }

        _logger.LogInformation(
            "Usuario registrado correctamente");

        return StatusCode(201, new
        {
            mensaje = "Usuario creado correctamente."
        });
    }
}