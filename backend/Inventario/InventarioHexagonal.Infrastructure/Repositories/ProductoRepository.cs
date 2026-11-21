using InventarioHexagonal.Infrastructure.Data;
using InventarioHexagonal.Application.Interfaces;
using InventarioHexagonal.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventarioHexagonal.Infrastructure.Repositories;

public class ProductoRepository : IProductoRepository
{
    private readonly DatabaseContext _dbContext;

    public ProductoRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    // 1. Listar productos
    public async Task<List<Producto>> GetAllAsync()
    {
        var productos = await _dbContext.Productos
            .AsNoTracking()
            .Where(p => p.Estado != "E")
            .OrderBy(p => p.Nombre)
            .ToListAsync();

        return productos;
    }

    // 2. Obtener producto por ID
    public async Task<Producto?> GetByIdAsync(int idProducto)
    {
        return await _dbContext.Productos
            .AsNoTracking()
            .FirstOrDefaultAsync(p =>
                p.IdProducto == idProducto && p.Estado != "E");
    }

    // 3. Crear producto
    public async Task<Producto> CreateAsync(Producto producto)
    {
        producto.Estado = "A";

        _dbContext.Productos.Add(producto);

        await _dbContext.SaveChangesAsync();

        return producto;
    }

    // 4. Actualizar producto
    public async Task<bool> UpdateAsync(Producto producto)
    {
        var productoExistente = await _dbContext.Productos
            .FirstOrDefaultAsync(p =>
                p.IdProducto == producto.IdProducto &&
                p.Estado != "E");

        if (productoExistente == null)
            return false;

        productoExistente.Nombre = producto.Nombre;
        productoExistente.Descripcion = producto.Descripcion;
        productoExistente.Estado = producto.Estado;
        productoExistente.FechaActualizacion = DateTime.Now;

        await _dbContext.SaveChangesAsync();

        return true;
    }

    // 5. Eliminación lógica
    public async Task<bool> DeleteAsync(int id)
    {
        var producto = await _dbContext.Productos
            .FirstOrDefaultAsync(p =>
                p.IdProducto == id && p.Estado != "E");

        if (producto == null)
            return false;

        producto.Estado = "E";
        producto.FechaEliminacion = DateTime.Now;
        producto.FechaActualizacion = DateTime.Now;

        await _dbContext.SaveChangesAsync();

        return true;
    }
}