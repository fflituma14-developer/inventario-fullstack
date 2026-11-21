using System.ComponentModel.DataAnnotations;

namespace InventarioHexagonal.Api.DTOs;

public class CrearProveedorRequest
{
    [Required]
    [MaxLength(200)]
    public string RazonSocial { get; set; } = string.Empty;

    [MaxLength(150)]
    public string? NombreComercial { get; set; }

    [Required]
    [RegularExpression(@"^[0-9]{13}$")]
    public string RUC { get; set; } = string.Empty;

    [EmailAddress]
    [MaxLength(150)]
    public string? Correo { get; set; }

    [MaxLength(20)]
    public string? Telefono { get; set; }

    [MaxLength(250)]
    public string? Direccion { get; set; }
}