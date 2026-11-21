
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InventarioHexagonal.Api.DTOs;
using InventarioHexagonal.Application.Services;
using InventarioHexagonal.Domain.Entities;

namespace InventarioHexagonal.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/proveedores")]
public class ProveedorController : ControllerBase
{
    private readonly ProveedorService _proveedorService;
    private readonly ILogger<ProveedorController> _logger;

    public ProveedorController(
        ProveedorService proveedorService,
        ILogger<ProveedorController> logger)
    {
        _proveedorService = proveedorService;
        _logger = logger;
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
        {
            _logger.LogWarning(
                "No se encontró el proveedor {IdProveedor}",
                id);

            return NotFound(new
            {
                mensaje = "Proveedor no encontrado"
            });
        }

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

        _logger.LogInformation(
            "Proveedor {IdProveedor} registrado correctamente",
            nuevo.IdProveedor);

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
        {
            _logger.LogWarning(
                "No se pudo actualizar. Proveedor {IdProveedor} no encontrado",
                id);

            return NotFound(new
            {
                mensaje = "Proveedor no encontrado"
            });
        }

        _logger.LogInformation(
            "Proveedor {IdProveedor} actualizado correctamente",
            id);

        return Ok(new
        {
            mensaje = "Proveedor actualizado correctamente"
        });
    }

    // DELETE: api/proveedores/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var eliminado = await _proveedorService.DeleteAsync(id);

        if (!eliminado)
        {
            _logger.LogWarning(
                "No se pudo eliminar. Proveedor {IdProveedor} no encontrado",
                id);

            return NotFound(new
            {
                mensaje = "Proveedor no encontrado"
            });
        }

        _logger.LogInformation(
            "Proveedor {IdProveedor} eliminado correctamente",
            id);

        return Ok(new
        {
            mensaje = "Proveedor eliminado correctamente"
        });
    }
}
