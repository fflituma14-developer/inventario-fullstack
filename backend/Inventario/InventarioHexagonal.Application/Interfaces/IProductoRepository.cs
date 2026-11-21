using InventarioHexagonal.Domain.Entities;

namespace InventarioHexagonal.Application.Interfaces;

public interface IProductoRepository
{
    // Listar productos
    Task<List<Producto>> GetAllAsync();

    // Obtener producto por ID
    Task<Producto?> GetByIdAsync(int id);

    // Crear producto
    Task<Producto> CreateAsync(Producto producto);

    // Actualizar producto
    Task<bool> UpdateAsync(Producto producto);

    // Eliminar producto lógicamente
    Task<bool> DeleteAsync(int id);
}