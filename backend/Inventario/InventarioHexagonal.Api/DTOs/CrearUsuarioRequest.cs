using System.ComponentModel.DataAnnotations;

namespace InventarioHexagonal.Api.DTOs;

public class CrearUsuarioRequest
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string NombreUsuario { get; set; } = string.Empty;

    [Required]
    [StringLength(150)]
    public string NombreCompleto { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(254)]
    public string Correo { get; set; } = string.Empty;

    [Required]
    [MinLength(5)]
    public string Password { get; set; } = string.Empty;
}