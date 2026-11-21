using System.ComponentModel.DataAnnotations;

namespace InventarioHexagonal.Api.DTOs;

public class LoginRequest
{
    [Required]
    public string NombreUsuario { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}