namespace InventarioHexagonal.Domain.Entities;


public class Producto
{
    public int IdProducto { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string Estado { get; set; } = "A";
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaActualizacion { get; set; }
    public DateTime? FechaEliminacion { get; set; }
    public int? UsuarioCreacion { get; set; }
    public int? UsuarioActualizacion { get; set; }
    public int? UsuarioEliminacion { get; set; }
}