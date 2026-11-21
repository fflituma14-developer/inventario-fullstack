using Microsoft.AspNetCore.Mvc;
using InventarioHexagonal.Api.DTOs;
using InventarioHexagonal.Application.Services;
using InventarioHexagonal.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace InventarioHexagonal.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/proveedores")]
public class ProveedorController : ControllerBase
{
    private readonly ProveedorService _proveedorService;

    public ProveedorController(ProveedorService proveedorService)
    {
        _proveedorService = proveedorService;
    }

    // GET: api/proveedores
    [HttpGet]
    public async Task<ActionResult<List<Proveedor>>> GetAllAsync()
    {
        var proveedores = await _proveedorService.GetAllAsync();

        return Ok(proveedores);
    }

    // GET: api/proveedores/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Proveedor>> GetByIdAsync(int id)
    {
        var proveedor = await _proveedorService.GetByIdAsync(id);

        if (proveedor == null)
            return NotFound(new { mensaje = "Proveedor no encontrado" });

        return Ok(proveedor);
    }

    // POST: api/proveedores
    [HttpPost]
    public async Task<ActionResult<Proveedor>> CreateAsync(
        [FromBody] CrearProveedorRequest request)
    {
        var proveedor = new Proveedor
        {
            RazonSocial = request.RazonSocial,
            NombreComercial = request.NombreComercial,
            RUC = request.RUC,
            Correo = request.Correo,
            Telefono = request.Telefono,
            Direccion = request.Direccion
        };

        var nuevo = await _proveedorService.CreateAsync(proveedor);

        return CreatedAtAction(
            nameof(GetByIdAsync),
            new { id = nuevo.IdProveedor },
            nuevo
        );
    }

    // PUT: api/proveedores/1
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(
        int id,
        [FromBody] ActualizarProveedorRequest request)
    {
        var proveedor = new Proveedor
        {
            IdProveedor = id,
            RazonSocial = request.RazonSocial,
            NombreComercial = request.NombreComercial,
            RUC = request.RUC,
            Correo = request.Correo,
            Telefono = request.Telefono,
            Direccion = request.Direccion
        };

        var actualizado = await _proveedorService.UpdateAsync(proveedor);

        if (!actualizado)
            return NotFound(new { mensaje = "Proveedor no encontrado" });

        return Ok(new { mensaje = "Proveedor actualizado correctamente" });
    }

    // DELETE: api/proveedores/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var eliminado = await _proveedorService.DeleteAsync(id);

        if (!eliminado)
            return NotFound(new { mensaje = "Proveedor no encontrado" });

        return Ok(new { mensaje = "Proveedor eliminado correctamente" });
    }
}