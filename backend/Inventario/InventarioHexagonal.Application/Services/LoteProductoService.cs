using InventarioHexagonal.Application.Interfaces;
using InventarioHexagonal.Domain.Entities;

namespace InventarioHexagonal.Application.Services;

public class LoteProductoService
{
    private readonly ILoteProductoRepository _repository;

    public LoteProductoService(ILoteProductoRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<LoteProducto>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<LoteProducto?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<LoteProducto> CreateAsync(LoteProducto lote)
    {
        lote.NumeroLote = lote.NumeroLote.Trim();

        // Validar que el producto esté activo
        if (!await _repository.ProductoActivoAsync(lote.IdProducto))
            throw new ArgumentException(
                "El producto no existe o no está activo.");

        // Validar que el proveedor esté activo
        if (!await _repository.ProveedorActivoAsync(lote.IdProveedor))
            throw new ArgumentException(
                "El proveedor no existe o no está activo.");

        // Validar precio y cantidad
        if (lote.PrecioCompra < 0)
            throw new ArgumentException(
                "El precio no puede ser negativo.");

        if (lote.CantidadStock < 0)
            throw new ArgumentException(
                "La cantidad no puede ser negativa.");

        // Validar lote duplicado
        var existe = await _repository.ExisteLoteAsync(
            lote.IdProducto,
            lote.IdProveedor,
            lote.NumeroLote
        );

        if (existe)
            throw new InvalidOperationException(
                "Ya existe ese lote para el producto y proveedor.");

        return await _repository.CreateAsync(lote);
    }
}