using InventarioHexagonal.Application.Interfaces;
using InventarioHexagonal.Domain.Entities;

namespace InventarioHexagonal.Application.Services;

public class ProductoService
{
    private readonly IProductoRepository _productoRepository;

    public ProductoService(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    // Listar todos los productos
    public async Task<List<Producto>> GetAllAsync()
    {
        return await _productoRepository.GetAllAsync();
    }

    // Obtener producto por ID
    public async Task<Producto?> GetByIdAsync(int id)
    {
        return await _productoRepository.GetByIdAsync(id);
    }

    // Crear producto
    public async Task<Producto> CreateAsync(Producto producto)
    {
        return await _productoRepository.CreateAsync(producto);
    }

    // Actualizar producto
    public async Task<bool> UpdateAsync(Producto producto)
    {
        return await _productoRepository.UpdateAsync(producto);
    }

    // Eliminar producto lógicamente
    public async Task<bool> DeleteAsync(int id)
    {
        return await _productoRepository.DeleteAsync(id);
    }
}