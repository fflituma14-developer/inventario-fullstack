using InventarioHexagonal.Application.Interfaces;
using InventarioHexagonal.Domain.Entities;
using InventarioHexagonal.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InventarioHexagonal.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly DatabaseContext _context;

    public UsuarioRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> GetByNombreUsuarioAsync(
        string nombreUsuario)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(
                u => u.NombreUsuario == nombreUsuario
            );
    }

    public async Task<bool> ExisteUsuarioAsync(
        string nombreUsuario,
        string correo)
    {
        return await _context.Usuarios
            .AnyAsync(u =>
                u.NombreUsuario == nombreUsuario ||
                u.Correo == correo
            );
    }

    public async Task<Usuario> CreateAsync(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);

        await _context.SaveChangesAsync();

        return usuario;
    }

    public async Task UpdateAsync(Usuario usuario)
    {
        usuario.FechaActualizacion = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }
}