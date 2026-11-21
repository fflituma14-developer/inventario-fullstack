using System.Security.Cryptography;
using System.Text;
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
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public AuthController(
        AuthService authService,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        _authService = authService;
        _configuration = configuration;
        _environment = environment;
    }

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
            return Unauthorized(new
            {
                mensaje = "Credenciales inválidas."
            });
        }

        return Ok(new { token });
    }

    [AllowAnonymous]
    [HttpPost("registro-inicial")]
    public async Task<IActionResult> RegistroInicial(
        [FromBody] CrearUsuarioRequest request)
    {
        if (!_environment.IsDevelopment())
            return NotFound();

        // Aquí verificaremos que todavía no exista
        // ningún usuario registrado.

        var registrado = await _authService.RegistrarAsync(
            request.NombreUsuario,
            request.NombreCompleto,
            request.Correo,
            request.Password);

        if (!registrado)
            return Conflict(new
            {
                mensaje = "No se pudo registrar el usuario."
            });

        return StatusCode(201, new
        {
            mensaje = "Usuario creado correctamente."
        });
    }
}