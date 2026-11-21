namespace InventarioHexagonal.Domain.Entities;

public class Usuario
{
    public int IdUsuario { get; set; }

    public string NombreUsuario { get; set; } = string.Empty;

    public string NombreCompleto { get; set; } = string.Empty;

    public string Correo { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string Estado { get; set; } = "A";

    public int IntentosFallidos { get; set; }

    public DateTime? BloqueadoHasta { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }
}