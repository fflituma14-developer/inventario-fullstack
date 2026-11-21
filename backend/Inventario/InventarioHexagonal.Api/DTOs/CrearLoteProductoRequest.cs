using System.ComponentModel.DataAnnotations;

namespace InventarioHexagonal.Api.DTOs;

public class CrearLoteProductoRequest
{
    [Range(1, int.MaxValue)]
    public int IdProducto { get; set; }

    [Range(1, int.MaxValue)]
    public int IdProveedor { get; set; }

    [Required]
    [MaxLength(50)]
    public string NumeroLote { get; set; } = string.Empty;

    [Range(typeof(decimal), "0", "9999999999999999")]
    public decimal PrecioCompra { get; set; }

    [Range(0, int.MaxValue)]
    public int CantidadStock { get; set; }
}