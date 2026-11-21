using InventarioHexagonal.Domain.Entities;

namespace InventarioHexagonal.Application.Interfaces;

public interface ILoteProductoRepository
{
    Task<List<LoteProducto>> GetAllAsync();
    Task<LoteProducto?> GetByIdAsync(int id);
    Task<LoteProducto> CreateAsync(LoteProducto lote);
    Task<bool> ProductoActivoAsync(int idProducto);
    Task<bool> ProveedorActivoAsync(int idProveedor);
    Task<bool> ExisteLoteAsync(
        int idProducto,
        int idProveedor,
        string numeroLote
    );
}