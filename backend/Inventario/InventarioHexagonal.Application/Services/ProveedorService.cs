using InventarioHexagonal.Application.Interfaces;
using InventarioHexagonal.Domain.Entities;

namespace InventarioHexagonal.Application.Services;

public class ProveedorService
{
    private readonly IProveedorRepository _proveedorRepository;

    public ProveedorService(IProveedorRepository proveedorRepository)
    {
        _proveedorRepository = proveedorRepository;
    }

    public async Task<List<Proveedor>> GetAllAsync()
    {
        return await _proveedorRepository.GetAllAsync();
    }

    public async Task<Proveedor?> GetByIdAsync(int id)
    {
        return await _proveedorRepository.GetByIdAsync(id);
    }

    public async Task<Proveedor> CreateAsync(Proveedor proveedor)
    {
        return await _proveedorRepository.CreateAsync(proveedor);
    }

    public async Task<bool> UpdateAsync(Proveedor proveedor)
    {
        return await _proveedorRepository.UpdateAsync(proveedor);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _proveedorRepository.DeleteAsync(id);
    }
}