namespace InventarioHexagonal.Domain.Entities;

public class Proveedor
{
    public int IdProveedor { get; set; }

    public string RazonSocial { get; set; } = string.Empty;
    public string? NombreComercial { get; set; }
    public string RUC { get; set; } = string.Empty;

    public string? Correo { get; set; }
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }

    public string Estado { get; set; } = "A";

    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaActualizacion { get; set; }
    public DateTime? FechaEliminacion { get; set; }

    public int? UsuarioCreacion { get; set; }
    public int? UsuarioActualizacion { get; set; }
    public int? UsuarioEliminacion { get; set; }
}