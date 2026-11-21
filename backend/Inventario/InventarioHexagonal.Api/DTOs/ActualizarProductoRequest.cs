using System.ComponentModel.DataAnnotations;

namespace InventarioHexagonal.Api.DTOs;

public class ActualizarProductoRequest
{
    [Required]
    [MaxLength(150)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Descripcion { get; set; }
}