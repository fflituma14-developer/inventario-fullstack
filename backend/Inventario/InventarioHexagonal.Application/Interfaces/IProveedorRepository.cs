using InventarioHexagonal.Domain.Entities;

namespace InventarioHexagonal.Application.Interfaces;

public interface IProveedorRepository
{
    Task<List<Proveedor>> GetAllAsync();

    Task<Proveedor?> GetByIdAsync(int id);

    Task<Proveedor> CreateAsync(Proveedor proveedor);

    Task<bool> UpdateAsync(Proveedor proveedor);

    Task<bool> DeleteAsync(int id);
}