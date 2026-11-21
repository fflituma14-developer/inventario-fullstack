using Microsoft.EntityFrameworkCore;
using InventarioHexagonal.Application.Interfaces;
using InventarioHexagonal.Domain.Entities;
using InventarioHexagonal.Infrastructure.Data;

namespace InventarioHexagonal.Infrastructure.Repositories;

public class ProveedorRepository : IProveedorRepository
{
    private readonly DatabaseContext _dbContext;

    public ProveedorRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Listar proveedores
    public async Task<List<Proveedor>> GetAllAsync()
    {
        return await _dbContext.Proveedores
            .Where(p => p.Estado != "E")
            .OrderBy(p => p.RazonSocial)
            .ToListAsync();
    }

    // Obtener por ID
    public async Task<Proveedor?> GetByIdAsync(int id)
    {
        return await _dbContext.Proveedores
            .FirstOrDefaultAsync(p =>
                p.IdProveedor == id && p.Estado != "E");
    }

    // Crear proveedor
    public async Task<Proveedor> CreateAsync(Proveedor proveedor)
    {
        proveedor.Estado = "A";

        _dbContext.Proveedores.Add(proveedor);

        await _dbContext.SaveChangesAsync();

        return proveedor;
    }

    // Actualizar proveedor
    public async Task<bool> UpdateAsync(Proveedor proveedor)
    {
        var existente = await _dbContext.Proveedores
            .FirstOrDefaultAsync(p =>
                p.IdProveedor == proveedor.IdProveedor &&
                p.Estado != "E");

        if (existente == null)
            return false;

        existente.RazonSocial = proveedor.RazonSocial;
        existente.NombreComercial = proveedor.NombreComercial;
        existente.RUC = proveedor.RUC;
        existente.Correo = proveedor.Correo;
        existente.Telefono = proveedor.Telefono;
        existente.Direccion = proveedor.Direccion;
        existente.FechaActualizacion = DateTime.Now;

        await _dbContext.SaveChangesAsync();

        return true;
    }

    // Eliminación lógica
    public async Task<bool> DeleteAsync(int id)
    {
        var proveedor = await _dbContext.Proveedores
            .FirstOrDefaultAsync(p =>
                p.IdProveedor == id && p.Estado != "E");

        if (proveedor == null)
            return false;

        proveedor.Estado = "E";
        proveedor.FechaEliminacion = DateTime.Now;
        proveedor.FechaActualizacion = DateTime.Now;

        await _dbContext.SaveChangesAsync();

        return true;
    }
}