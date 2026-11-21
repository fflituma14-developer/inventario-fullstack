using InventarioHexagonal.Domain.Entities;

namespace InventarioHexagonal.Application.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> GetByNombreUsuarioAsync(string nombreUsuario);
    Task<bool> ExisteUsuarioAsync(string nombreUsuario, string correo);

    Task UpdateAsync(Usuario usuario);

    Task<Usuario> CreateAsync(Usuario usuario);
}