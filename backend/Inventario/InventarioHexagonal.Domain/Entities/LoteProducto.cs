namespace InventarioHexagonal.Domain.Entities;

public class LoteProducto
{
    public int IdLote { get; set; }

    public int IdProducto { get; set; }
    public int IdProveedor { get; set; }

    public string NumeroLote { get; set; } = string.Empty;
    public decimal PrecioCompra { get; set; }
    public int CantidadStock { get; set; }

    public DateTime FechaIngreso { get; set; }
    public string Estado { get; set; } = "A";

    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaActualizacion { get; set; }
    public DateTime? FechaEliminacion { get; set; }

    public int? UsuarioCreacion { get; set; }
    public int? UsuarioActualizacion { get; set; }
    public int? UsuarioEliminacion { get; set; }
}