using Microsoft.EntityFrameworkCore;
using InventarioHexagonal.Application.Interfaces;
using InventarioHexagonal.Domain.Entities;
using InventarioHexagonal.Infrastructure.Data;

namespace InventarioHexagonal.Infrastructure.Repositories;

public class LoteProductoRepository : ILoteProductoRepository
{
    private readonly DatabaseContext _dbContext;

    public LoteProductoRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Listar lotes
    public async Task<List<LoteProducto>> GetAllAsync()
    {
        return await _dbContext.LotesProductos
            .Where(l => l.Estado != "E")
            .OrderByDescending(l => l.IdLote)
            .ToListAsync();
    }

    // Consultar lote por ID
    public async Task<LoteProducto?> GetByIdAsync(int id)
    {
        return await _dbContext.LotesProductos
            .FirstOrDefaultAsync(l =>
                l.IdLote == id && l.Estado == "A");
    }

    // Registrar lote
    public async Task<LoteProducto> CreateAsync(LoteProducto lote)
    {
        lote.Estado = "A";

        _dbContext.LotesProductos.Add(lote);

        await _dbContext.SaveChangesAsync();

        return lote;
    }

    // Validar producto activo
    public async Task<bool> ProductoActivoAsync(int idProducto)
    {
        return await _dbContext.Productos
            .AnyAsync(p =>
                p.IdProducto == idProducto &&
                p.Estado == "A");
    }

    // Validar proveedor activo
    public async Task<bool> ProveedorActivoAsync(int idProveedor)
    {
        return await _dbContext.Proveedores
            .AnyAsync(p =>
                p.IdProveedor == idProveedor &&
                p.Estado == "A");
    }

    // Validar lote duplicado
    public async Task<bool> ExisteLoteAsync(
        int idProducto,
        int idProveedor,
        string numeroLote)
    {
        return await _dbContext.LotesProductos
            .AnyAsync(l =>
                l.IdProducto == idProducto &&
                l.IdProveedor == idProveedor &&
                l.NumeroLote == numeroLote);
    }
}